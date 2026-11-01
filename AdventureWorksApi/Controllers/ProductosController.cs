using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using AdventureWorksApi.Models;
using System.Data;

namespace AdventureWorksApi.Controllers
{

    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProductosController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return
                Ok(
                    new
                    {
                        mensaje = "Ping " + DateTime.Now.ToString()
                    });

        }
        [HttpGet("eco/{id}")]
        public IActionResult Eco(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { mensaje = "El id no puede ser menor o igual a cero" });
            }
            return Ok(new { id });

        }

        [HttpGet("top")]
        public async Task<IActionResult> Top()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("AdventureWorks")))
            {
                var productos = await conn.QueryAsync<ProductoDto>("SELECT Top 10 ProductID, Name, ListPrice, Color FROM Production.Product ORDER BY ListPrice DESC");

                return Ok(productos);
            }

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> PorId(int id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("AdventureWorks")))
            {
                var producto = await conn.QueryFirstOrDefaultAsync <ProductoDto>("SELECT Name FROM Production.Product WHERE ProductID = @id", new { id });

                if (producto is null)
                {
                    return NotFound(new { mensaje = "Producto no encontrado" });
                }

                return Ok(producto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PorRango([FromQuery] decimal precioMin = 0, [FromQuery] decimal? precioMax = 0)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("AdventureWorks"))){

                var productos = await conn.QueryAsync<ProductoDto>("usp_ProductosPorRango", 
                    new { @PrecioMin = @precioMin, @PrecioMax = @precioMax}, 
                    commandType: CommandType.StoredProcedure);
            

                return Ok(productos);

            }
        }


        [HttpPut("{id:int}/precio")]
        public async Task<IActionResult> ActualizarPrecio(int id, [FromBody] ActualizarPrecioDto dto)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("AdventureWorks")))
            {
                var p = new DynamicParameters();
                p.Add("ProductID", id);
                p.Add("NuevoPrecio", dto.NuevoPrecio);
                p.Add("Mensaje", direction: ParameterDirection.Output, dbType: DbType.String, size: 100);

                try
                {

                await conn.ExecuteAsync("usp_ActualizarPrecioProducto", p, commandType: CommandType.StoredProcedure);
                    var mensaje = p.Get<string>("Mensaje");
                    return Ok(new { mensaje });

                }
                catch (SqlException ex) when (ex.Number == 50001)
                {
                    return NotFound(new { mensaje = ex.Message });
                }
                catch (SqlException ex) when (ex.Number == 50002)
                {
                    return BadRequest(new { mensaje = ex.Message });
                }

                

            }
        }
    }
}
