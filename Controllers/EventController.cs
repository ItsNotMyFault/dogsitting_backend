using dogsitting_backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace dogsitting_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        public EventController() {
        }


        [HttpGet(Name = "GetTest")]
        public string test()
        {
            return "test";
        }
    }
}
