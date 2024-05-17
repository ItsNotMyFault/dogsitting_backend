using Microsoft.AspNetCore.Mvc;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using dogsitting_backend.ApplicationServices;
using Newtonsoft.Json;
using dogsitting_backend.ApplicationServices.dto;

namespace dogsitting_backend.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("users")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationUserService _applicationUserService;

        public ApplicationUserController(ApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        [HttpGet("{id}")]
        public ActionResult Details([FromRoute] Guid id)
        {
            ApplicationUser user = this._applicationUserService.GetUserById(id).Result;
            //todo transform application user object to a response/dto to avoid showing implicit data
            string json = JsonConvert.SerializeObject(user);

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
