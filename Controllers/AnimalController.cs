using dogsitting_backend.ApplicationServices;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;
using dogsitting_backend.Domain;
using dogsitting_backend.Domain.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.IO;
using System.Security.Claims;

namespace dogsitting_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private AnimalService AnimalService;
        private AuthUser _authUser;

        public AnimalController(AnimalService animalService, IHttpContextAccessor httpContextAccessor, UserManager<AuthUser> userManager)
        {
            this.AnimalService = animalService;
            ClaimsPrincipal claimsPrincipal = httpContextAccessor.HttpContext.User;
            this._authUser = userManager.GetUserAsync(claimsPrincipal).Result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details([FromRoute] Guid id)
        {
            AnimalResponse animal = await this.AnimalService.GetAnimalById(id);
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
        public async Task<ActionResult> Create([FromForm] CreateAnimalDto animal)
        {
            IFormFile? file = Request.Form.Files.FirstOrDefault();
            await this.AnimalService.CreateUserAnimal(animal, this._authUser.ApplicationUser.Id, file);
            return Ok();
        }

        [HttpPut("edit/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromForm] CreateAnimalDto animal)
        {
            IFormFile? file = Request.Form.Files.FirstOrDefault();
            await this.AnimalService.UpdateAnimal(id, animal, this._authUser.ApplicationUser.Id);
            if (file != null)
            {
                await this.AnimalService.UpdateAnimalMedia(id, file);
            }

            return Ok();
        }


        [HttpPost("{Id}/media", Name = "UpdateMedia")]
        public async Task<ActionResult> UpdateMedia([FromRoute] Guid Id)
        {
            IFormFile? file = Request.Form.Files.FirstOrDefault();
            if (file != null)
            {
                await this.AnimalService.UpdateAnimalMedia(Id, file);
            }
            else
            {
                await this.AnimalService.DeleteAnimalMedia(Id);
            }
            return Ok();
        }

    }
}
