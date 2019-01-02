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
        public ActionResult AddRely(string id, string[] relydata, string replyid)
        {

            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var ID = Guid.Parse(id);

            var p = _context.Persons.Find((Session["LoginUserSessionModel"] as LoginUserSessionModel).Person.ID);
            var a = _context.Albums.Find(ID);
            var imgPath = p.Avarda;
            var htmlString = "";
            var rely = new Reply()
            {

                Content = relydata[0],
                Person = p,
                Album = a,
            };
            if (replyid.Equals(""))
            {
                rely.Title = p.Name + ":评论了专辑:" + a.Title;
                rely.ParentReply = null;
            }
            else
            {
                rely.Title = p.Name + ":回复了:";
                var PID = Guid.Parse(replyid);
                rely.ParentReply = _context.Replys.Find(PID);
            }
            _context.Replys.Add(rely);
            _context.SaveChanges();
            var replylist = new List<Reply>();
            var list = _context.Replys.Where(x => x.Album.ID == ID).OrderByDescending(x => x.CreateDateTime).ToList();
            foreach (var r in list)
            {
                replylist.Add(r);
            }
            foreach (var item in replylist)
            {
                var sonCmt = _context.Replys.Where(x => x.ParentReply.ID == item.ID).ToList();
                htmlString += "<div class='row'>";
                htmlString += "<div style=\"box-sizing:border-box;padding:50px\">";
                htmlString += "<div class=\"col-xs-2 col-sm-1 col-md-1 bg\">";
                htmlString += "<img src='" + item.Person.Avarda + "' alt='图片' class=\"img-thumbnail\" style = \"max-height: 45px;max-width: 45px\" >";
                htmlString += "</div>";
                htmlString += "<div class=\"col-xs-10 col-sm-11 col-md-11\">";
                htmlString += "<div style=\"min-width:400px; \">";
                htmlString += item.Person.Name +"："+ item.Content;
                htmlString += "</div>";
                htmlString += "<div style=\"min-width:400px; \">";
                htmlString += " <span style=\"color:#aaa\">" + item.CreateDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "</span><i class='glyphicon glyphicon-thumbs-down' style='float:right;margin-top:0px;margin-left:20px'>(" + item.Hate + ")</i> <i class='glyphicon glyphicon-thumbs-up' style='float:right;margin-top:0px;margin-left:20px'>(" + item.Like + ")</i><a href='#' id='parentReply' style='float:right' onclick=\"parentReply('@i.ID')\">回复(" + sonCmt.Count + ")</a>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
            }
            return Json(htmlString);
        }




        [HttpPost]
        public ActionResult Like(Guid id)
        {
            return Json("");
        }

        [HttpPost]
        public ActionResult Hate(Guid id)
        {
            return Json("");
        }
    }
}