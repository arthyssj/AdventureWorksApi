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

    }
}
