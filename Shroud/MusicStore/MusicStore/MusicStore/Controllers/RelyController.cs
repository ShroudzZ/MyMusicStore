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
            var list = _context.Replys.Where(x => x.Album.ID == ID&&x.ParentReply==null).OrderByDescending(x => x.CreateDateTime).ToList();
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
                htmlString += " <span style=\"color:#aaa\">" + item.CreateDateTime.ToString("yyyy-MM-dd hh:mm:ss") + "</span><i class='glyphicon glyphicon-thumbs-down' style='float:right;margin-top:0px;margin-left:20px' onclick=\"Like('"+item.ID+"'))\">(" + item.Hate + ")</i> <i class='glyphicon glyphicon-thumbs-up' style='float:right;margin-top:0px;margin-left:20px' onclick=\"Like('" + item.ID + "'))\">(" + item.Like + ")</i> <a href=\"#\" style='float:right' onclick=\"javascript:ShowCmt('"+item.ID+"')\" >查看所有回复</a> <a href='#' id='parentReply' style='float:right; margin-right:10px' onclick=\"parentReply('"+ item.ID+"')\">回复</a>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
                htmlString += "</div>";
            }
            return Json(htmlString);
        }


        public ActionResult ShowCmts(string pid)
        {
            var htmlString = "";
            //子回复
            Guid id = Guid.Parse(pid);
            var cmts = _context.Replys.Where(x => x.ParentReply.ID == id).OrderByDescending(x => x.CreateDateTime).ToList();
            //原回复
            var pcmt = _context.Replys.Find(id);
            htmlString += "<div class=\"modal-header\">";
            htmlString += "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button>";
            htmlString += "<h4 class=\"modal-title\" id=\"myModalLabel\">";
            htmlString += "<em>楼主&nbsp;&nbsp;</em>" + pcmt.Person.Name + "&nbsp;&nbsp;发表于" + pcmt.CreateDateTime.ToString("yyyy年MM月dd日 hh点mm分ss秒") + ":<br/>" + pcmt.Content;
            htmlString += " </h4> </div>";

            htmlString += "<div class=\"modal-body\">";
            //子回复
            htmlString += "<ul class='media-list' style='margin-left:20px;'>";
            foreach (var item in cmts)
            {
                htmlString += "<li class='media'>";
                htmlString += "<div class='media-left'>";
                htmlString += "<img class='media-object' src='" + item.Person.Avarda +
                              "' alt='头像' style='width:40px;border-radius:50%;'>";
                htmlString += "</div>";
                htmlString += "<div class='media-body' id='Content-" + item.ID + "'>";
                htmlString += "<h5 class='media-heading'><em>" + item.Person.Name + "</em>&nbsp;&nbsp;发表于" +
                              item.CreateDateTime.ToString("yyyy年MM月dd日") + "</h5>";
                htmlString += item.Content;
                htmlString += "</div>";
                htmlString += "<h6><a href='#div-editor' class='reply' onclick=\"javascript:GetQuote('" + item.ParentReply.ID + "','" + item.ID + "');\">回复</a>" +
                              "<a href='#' class='reply' style='margin:0 20px 0 40px'   onclick=\"javascript:Like('" + item.ID + "');\"><i class='glyphicon glyphicon-thumbs-up'></i>(" + item.Like + ")</a>" +
                              "<a href='#' class='reply' style='margin:0 20px'   onclick=\"javascript:Hate('" + item.ID + "');\"><i class='glyphicon glyphicon-thumbs-down'></i>(" + item.Hate + ")</a></h6>";
                htmlString += "</li>";
            }
            htmlString += "</ul>";
            htmlString += "</div><div class=\"modal-footer\"></div>";
            return Json(htmlString);
        }

        [HttpPost]
        public ActionResult Like(Guid id)
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var person = _context.Persons.Find(((LoginUserSessionModel)(Session["LoginUserSessionModel"])).Person.ID);
            var sure = _context.LikeReplys.SingleOrDefault(x => x.Reply.ID == id && x.Person.ID == person.ID);
            if (sure ==null)
            {
                var like = new LikeReply()
                {
                    IsNotLike = true,
                    Person = person,
                    Reply = _context.Replys.Find(id),
                };
                _context.LikeReplys.Add(like);
                var reply = _context.Replys.Find(id);
                reply.Like++;
                _context.SaveChanges();
                return Json("");
            }
            else
            {
                return Content("<script>alert('已经踩过或者赞过该评论了')</script>");
            }
            
           
        }

        [HttpPost]
        public ActionResult Hate(Guid id)
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account");
            }
            var person = _context.Persons.Find(((LoginUserSessionModel)(Session["LoginUserSessionModel"])).Person.ID);
            var sure = _context.LikeReplys.SingleOrDefault(x => x.Reply.ID == id && x.Person.ID == person.ID);
            if (sure == null)
            {
                var hate = new LikeReply()
                {
                    IsNotLike = false,
                    Person = person, 
                    Reply = _context.Replys.Find(id),
                };
                _context.LikeReplys.Add(hate);
                var reply = _context.Replys.Find(id);
                reply.Hate++;
                _context.SaveChanges();
                return Json("");
            }
            else
            {
                return Content("<script>alert('已经踩过或者赞过该评论了')</script>");
            }
        }
    }
}