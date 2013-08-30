using System.Web.Mvc;

namespace Nustache.Mvc3.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["DoesViewDataWork"] = "ViewData works!";
            ViewBag.DoesViewBagWork = "ViewBag works!";

            var model = new
                        {
                            DoModelPropertiesWork = "Model properties work!",
                            DoesHtmlEncodingWork = "<em>Should this be encoded?</em>",
                            DoesInternationalCharacterEncodingWork = "Iñtërnâtiônàlizætiøn",
                            DoesRussianCharacterEncodingWork = "Привет, как дела"
                        };

            // TODO: Find a better way to specify the default master.
            return View("Index", "_Layout", model);
        }

        public ActionResult Handlebars()
        {
            ViewData["DoesViewDataWork"] = "ViewData works!";
            ViewBag.DoesViewBagWork = "ViewBag works!";

            var model = new
            {
                Title = "General Title",
                Article = new
                {
                    Title = "This is the article title.", 
                    Intro = "This is the article intro.",
                    Tags = new[] {"Tag1", "Tag2", "Tag3"}
                },
                Articles = new[]
                {
                    new {Subject = "Article 1 Subject", Body = "Article 1 Body"},
                    new {Subject = "Article 2 Subject", Body = "Article 2 Body"},
                    new {Subject = "Article 3 Subject", Body = "Article 3 Body"}
                },
                Text = "This is a link",
                Url = "http://www.google.com",
                Thing = true,
                OtherThing = false
            };

            // TODO: Find a better way to specify the default master.
            return View("Handlebars", "_Layout", model);
        }

        public ActionResult RazorWithPartialNustache()
        {
            ViewBag.InternationalCharacters = "Iñtërnâtiônàlizætiøn";

            var model = new
                        {
                            DoModelPropertiesWork = "Model properties work!"
                        };

            return View(model);
        }

        public ActionResult MissingView()
        {
            return View();
        }

        public ActionResult MissingPartial()
        {
            return View();
        }
    }
}