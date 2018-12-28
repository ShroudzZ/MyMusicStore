using MusicStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using System.Data.Entity.Validation;
using System.Threading;

namespace MusicStore.Controllers
{
    public class RelyController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: Rely
        public ActionResult Index()
        {
            return View();
        }
        //Album id;
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddRely(Guid id, string[] relydata)
        {
           
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var p = _context.Persons.Find((Session["LoginUserSessionModel"] as LoginUserSessionModel).Person.ID);
            var a = _context.Albums.Find(id);
            var imgPath = p.Avarda;
            var htmlString = "";
            var rely = new Reply()
            {

                Content = relydata[0],
                Person = p,
                Album = a,
            };
            rely.Title = p.Name + ":评论了专辑:" + a.Title;
            _context.Replys.Add(rely);
            _context.SaveChanges();
            var replylist = new List<Reply>();
            var list = _context.Replys.Where(x => x.Album.ID == id).OrderByDescending(x => x.CreateDateTime).ToList();
            foreach (var r in list)
            {
                replylist.Add(r);
            }
            foreach (var item in replylist)
            {
                htmlString += "<div class='row'>";
                htmlString += "<div style=\"box-sizing:border-box;padding:50px\">";
                htmlString += "<div class=\"col-xs-2 col-sm-1 col-md-1 bg\">";
                htmlString += "<img src='" + item.Person.Avarda + "' alt='图片' class=\"img-thumbnail\" style = \"max-height: 45px;max-width: 45px\" >";
                htmlString += "</div>";
                htmlString += "<div class=\"col-xs-10 col-sm-11 col-md-11\">";
                htmlString += "<div style=\"min-width:400px; \">";
                htmlString += " <a href=\"#\">" + item.Person.Name + "</a>:" + item.Content + "";
                htmlString += "</div>";
                htmlString += "<div style=\"min-width:400px; \">";
                htmlString += " <span style=\"color:#aaa\">" + item.CreateDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "</span> <i class='glyphicon glyphicon-thumbs-up' style='float:right;margin-top:0px;margin-left:20px'></i><a href='#' id='parentReply' style='float:right'>回复</a>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
            }
            return Json(htmlString);
        }
        [HttpPost]
        public ActionResult AddParentReply()
        {

            return Json("");
        }



    }
}