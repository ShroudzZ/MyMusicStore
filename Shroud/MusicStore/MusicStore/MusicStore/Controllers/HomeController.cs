using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using System.Threading.Tasks;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly MusicContext _context = new MusicContext();
        // GET: test
        public ActionResult Index()
        {
            var list = _context.Albums.OrderByDescending(x=>x.PublisherDate).Take(20).ToList();
            return View(list);
        }

        public ActionResult Genre(Guid id)
        {
            var list = _context.Albums.Where(g=>g.Genre.ID==id).OrderByDescending(x => x.PublisherDate).ToList();
            return View(list);
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