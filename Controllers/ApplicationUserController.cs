using Microsoft.AspNetCore.Mvc;
using dogsitting_backend.Domain;
using Microsoft.AspNetCore.Authorization;
using dogsitting_backend.ApplicationServices;
using Newtonsoft.Json;

namespace dogsitting_backend.Controllers
{
    [Authorize]
    [Route("Users")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationUserService _applicationUserService;

        public ApplicationUserController(ApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        // GET: ApplicationUser
        public ActionResult Index()
        {
            List<ApplicationUser> users = this._applicationUserService.Index().Result.ToList();
            string json = JsonConvert.SerializeObject(users);

            return Ok(users);
        }

        // GET: ApplicationUser/Details/5
        [HttpGet("{id}")]
        public ActionResult Details([FromRoute] Guid id)
        {
            ApplicationUser user = this._applicationUserService.Details(id).Result;
            string json = JsonConvert.SerializeObject(user);

            return Ok(json);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task Edit([FromRoute] Guid id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                throw new ArgumentException("Wrong id");
            }

            await this._applicationUserService.Edit(id, applicationUser);
        }

        [HttpDelete("delete")]
        [ValidateAntiForgeryToken]
        public async Task Delete(Guid id)
        {
            await this._applicationUserService.Delete(id);
        }

    }
}
