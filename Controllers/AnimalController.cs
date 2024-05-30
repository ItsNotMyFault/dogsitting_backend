using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;

namespace dogsitting_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private AnimalService AnimalService;
        public AnimalController(AnimalService animalService)
        {
            this.AnimalService = animalService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details([FromRoute] Guid id)
        {
            Animal animal = await this.AnimalService.GetAnimal(id);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(animal, settings);

            return Ok(json);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAnimals()
        {
            List<Animal> animals = this.AnimalService.GetAnimals().Result.ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(animals, settings);
            return Ok(json);
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] CreateAnimalDto animal)
        {
            var test = HttpContext.User.Claims;
            await this.AnimalService.CreateAnimal(animal);
            return Ok();
        }

        [HttpPut("edit/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] Animal animal)
        {
            await this.AnimalService.UpdateAnimal(id, animal);
            return Ok();
        }

    }
}
