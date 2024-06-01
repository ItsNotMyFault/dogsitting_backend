using Microsoft.AspNetCore.Mvc;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using dogsitting_backend.ApplicationServices;
using Newtonsoft.Json;
using dogsitting_backend.ApplicationServices.dto;
using dogsitting_backend.ApplicationServices.response;

namespace dogsitting_backend.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("users")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationUserService _applicationUserService;
        private readonly AnimalService _animalService;

        public ApplicationUserController(ApplicationUserService applicationUserService, AnimalService animalService)
        {
            this._animalService = animalService;
            this._applicationUserService = applicationUserService;
        }

        [HttpGet("{id}")]
        public ActionResult Details([FromRoute] Guid id)
        {
            ApplicationUser user = this._applicationUserService.GetUserById(id).Result;
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(user, settings);
            return Ok(json);
        }

        [HttpGet("{id}/animals")]
        public ActionResult Animals([FromRoute] Guid id)
        {
            List<AnimalResponse> animals = this._animalService.GetAnimalsByUserId(id).Result;
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            string json = JsonConvert.SerializeObject(animals, settings);
            return Ok(json);
        }


        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] UpdateUserDto applicationUser)
        {
            await this._applicationUserService.Edit(id, applicationUser);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task Delete(Guid id)
        {
            await this._applicationUserService.Delete(id);
        }

    }
}
