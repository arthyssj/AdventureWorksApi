using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksApi.Controllers
{

    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return 
                Ok(
                    new {
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

    }
}
