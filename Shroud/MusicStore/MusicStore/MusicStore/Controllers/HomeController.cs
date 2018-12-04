using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using System.Threading.Tasks;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        
        public ActionResult Index(int pageIndex=1,int pageSize=20)
        {
            var list = _context.Albums;
            var pagelist = list.OrderByDescending(a => a.PublisherDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagelistmodel = new PageViewModel()
            {
                Items = pagelist,
                PageInfo = new ViewModels.PageInfo
                {
                    Itemall = list.Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                }
            };
            Session["PageModel"] = pagelistmodel;
            return View(pagelist);
        }

        public ActionResult Genre(Guid id)
        {
            Session["Genreid"] = id;
            var list = _context.Albums.Where(g => g.Genre.ID == id).OrderByDescending(x => x.PublisherDate).ToList();
            var pagelist = list.OrderByDescending(a => a.PublisherDate).Skip((1 - 1) * 20).Take(20).ToList();
            var pagelistmodel = new PageViewModel()
            {
                Items = pagelist,
                PageInfo = new ViewModels.PageInfo
                {
                    Itemall = list.Count(),
                    PageIndex = 1,
                    PageSize = 20
                }
            };
            Session["PageModel"] = pagelistmodel;
            return View(list);
        }
        public ActionResult GenrePage(Guid id, int pageIndex = 1, int pageSize = 20)
        {
            var list = _context.Albums.Where(g => g.Genre.ID == id);
            Session["Genreid"] = id;
            var pagelist = list.OrderByDescending(a => a.PublisherDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var pagelistmodel = new PageViewModel()
            {
                Items = pagelist,
                PageInfo = new ViewModels.PageInfo
                {
                    Itemall = list.Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                }
            };
            Session["PageModel"] = pagelistmodel;
            return View("Genre",pagelist);
        }
        /// <summary>
        /// 伪造攻击
        /// </summary>
        /// <returns></returns>
        public ActionResult TestHack()
        {
            return View();
        }

        public async Task<ActionResult> TesthackC()
        {
            var chent = new HttpClient();

            var values=new List<KeyValuePair<string,string>>();
            values.Add(new KeyValuePair<string, string>("txt_卡号", "20170310089"));
            values.Add(new KeyValuePair<string, string>("txt_密码", "abcwkt45"));
            var content=new FormUrlEncodedContent(values);
            var respnse = await chent.PostAsync("http://jw.lzzy.net/st/login.aspx", content);
            var html = await respnse.Content.ReadAsStreamAsync();
            return Redirect("http://jw.lzzy.net/st/student/index.aspx");
        }
    }
}