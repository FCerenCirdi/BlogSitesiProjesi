using BlogSitesiProjesi.Filters;
using BlogSitesiProjesi.Manager;
using BlogSitesiProjesi.Models.Data;
using BlogSitesiProjesi.Models.Entity;
using BlogSitesiProjesi.ViewModels.Article.Create;
using BlogSitesiProjesi.ViewModels.Article.Edit;
using BlogSitesiProjesi.ViewModels.Article.Topic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BlogSitesiProjesi.Controllers
{
    [LoggedUser]
    public class ArticleController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(DatabaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Create(string yonlen)
        {
            ViewBag.yonlen = yonlen;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel model, string yonlen)
        {
            if (ModelState.IsValid)
            {
                Article article = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = int.Parse(HttpContext.Session.GetString("userId")),
                    ArticlePicture = model.ArticlePicture.GetUniqueNameAndSavePhotoToDisk(_webHostEnvironment)
                };
                _context.Articles.Add(article);
                _context.SaveChanges();
                TempData["message"] = "Article Created..!";
                if (yonlen == null) return RedirectToAction("Index", "Home");
                return Redirect(yonlen);
            }
            else return View(model);
        }

        public IActionResult Edit(int id)
        {
            Article article = _context.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));

            if (article is not null) return View(new EditViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                ArticlePictureName = article.ArticlePicture
            });
            else
            {
                TempData["error"] = "Data could'n find";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Article article = _context.Articles.FirstOrDefault(x => x.Id.Equals(model.Id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
                if (article is null)
                {
                    ViewData["error"] = "Edit failed..!";
                    return View(model);
                }
                article.Title = model.Title;
                article.Content = model.Content;
                if (model.ArticlePicture != null)
                {
                    article.ArticlePicture = model.ArticlePicture.GetUniqueNameAndSavePhotoToDisk(_webHostEnvironment);
                    FileManager.RemoveImageFromDisk(model.ArticlePictureName, _webHostEnvironment);
                }

                _context.SaveChanges();

                TempData["message"] = "Article Editing Completed..!";
                return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Article article = _context.Articles.FirstOrDefault(x => x.Id.Equals(id) && x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")));
            if (article is not null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
                FileManager.RemoveImageFromDisk(article.ArticlePicture, _webHostEnvironment);
                TempData["message"] = "Delete Completed..!";
            }
            else TempData["error"] = "Data couldn't find";

            return RedirectToAction("Profile", "Home", new { username = HttpContext.Session.GetString("username") });
        }

        public IActionResult Topic() //Konu listeleme
        {
            ViewBag.Topic = _context.Topics.Select(w =>
                new SelectListItem
                {
                    Text = w.Name,
                    Value = w.Id.ToString()
                }
            ).ToList();
            return View();

        }
        [HttpPost]
        public IActionResult CreateTopic(TopicViewModel model, string yonlen)
        {
            if (ModelState.IsValid)
            {
                Topic topic = new Topic
                {
                    Name = model.Name,
                    
                };
                _context.Topics.Add(topic);
                _context.SaveChanges();
                TempData["message"] = "Topic Created..!";
                if (yonlen == null) return RedirectToAction("Index", "Home");
                return Redirect(yonlen);
            }
            else return View(model);
        }
    }
}
