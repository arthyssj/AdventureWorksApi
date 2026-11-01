using AdventureWorksApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorksApi.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClientesController(IConfiguration config)
        {
            _config = config;

        }

        [HttpGet("{id:int}/resumen")]
        public async Task<IActionResult> Resumen(int id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("AdventureWorks")))
            {
                var resumen = await conn.QuerySingleAsync<ResumenClienteDto>("usp_ResumenVentasPorCliente", new { ClienteID = id}, commandType: CommandType.StoredProcedure);

                return Ok(resumen);
            }
        }

    }
}
