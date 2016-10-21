using Microsoft.AspNetCore.Mvc;

namespace DryRun
{
    public class HomeController : Controller
    {
        private readonly IUnknownService service;
        public HomeController(IUnknownService service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            service.AddRef();
            var model = new Results();
            model.Title = "Welcome";
            model.Value = $"Reference counts so far {service.QueryInterface("IID_IDispatch")}";
            return Json(model);
        }
    }
}