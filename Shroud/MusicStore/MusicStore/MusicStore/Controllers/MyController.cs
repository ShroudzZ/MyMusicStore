using MusicStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using MusicStoreEntity.UserAndRole;
using System.IO;

namespace MusicStore.Controllers
{
    public class MyController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: My
        public ActionResult Index()
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
            var person = _context.Persons.Find(p.ID);
            var mymodel = new MyViewModel()
            {
                Name = person.Name,
                Email = person.Email,
                Description = person.Description,
                MobileNumber = person.MobileNumber,
                TelephoneNumber = person.TelephoneNumber,
            };
            ViewBag.AvardaUrl = "/Upload/Normal/logo.png";
            return View(mymodel);
        }
        [HttpPost]
        public ActionResult Index(MyViewModel model)
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
            var person = _context.Persons.Find(p.ID);

            var oldAvarda = person.Avarda;

            if (ModelState.IsValid)
            {
                if (model.Photo != null)
                {
                    var uploadDir = "~/Upload/Photo/";
                    var fileLastName = model.Photo.FileName.Substring(model.Photo.FileName.LastIndexOf(".") + 1,
                        (model.Photo.FileName.Length - model.Photo.FileName.LastIndexOf(".") - 1));
                    var imgPath = Path.Combine(Server.MapPath(uploadDir), person.ID + "." + fileLastName);
                    model.Photo.SaveAs(imgPath);
                    oldAvarda = "/Upload/Photo/" + person.ID + "." + fileLastName;
                }

                person.Email = model.Email;
                person.Description = model.Description;
                person.MobileNumber = model.MobileNumber;
                person.TelephoneNumber = model.MobileNumber;
                person.UpdateTime = DateTime.Now;
                person.FirstName = model.Name.Substring(0, 1);
                person.LastName = model.Name.Substring(1, model.Name.Length - 1);
                person.Sex = model.Sex;
                person.Avarda = oldAvarda;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}