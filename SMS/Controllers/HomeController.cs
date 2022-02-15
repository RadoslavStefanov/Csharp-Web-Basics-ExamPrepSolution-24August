using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(Request request) 
            : base(request)
        {
        }

        public Response Index()
        {
            var model = new
            {
                IsAuthenticated = User.IsAuthenticated
            };
            return this.View(model);
        }
    }
}