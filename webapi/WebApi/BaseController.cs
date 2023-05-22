using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        public ObjectResult ServerError(string message)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}
