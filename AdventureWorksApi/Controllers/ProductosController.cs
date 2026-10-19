using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using AdventureWorksApi.Models;

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

    }
}
