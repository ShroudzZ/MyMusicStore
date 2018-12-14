using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStoreEntity;
using MusicStore.ViewModels;

namespace MusicStore.Controllers
{
    public class AddressController : Controller
    {
        private static MusicContext _context = new MusicContext();

        // GET: Address
        public ActionResult Index()
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account", new {returnUrl = Url.Action("index", "home")});
            }

            var p = Session["LoginUserSessionModel"] as LoginUserSessionModel;
            var list = _context.PersonAddresses.Where(x => x.Person.ID == p.Person.ID).ToList();
            var addresslist = new List<PersonAddress>();
            foreach (var item in list)
            {
                addresslist.Add(item);
            }

            var addressview = new AddressViewModel()
            {
                Addresslist = addresslist
            };
            return View(addressview);
        }


        [HttpPost]
        public ActionResult CreateAddress(AddressViewModel model)
        {
            if ((Session["LoginUserSessionModel"] as LoginUserSessionModel) == null)
            {
                return RedirectToAction("login", "Account", new {returnUrl = Url.Action("index", "home")});
            }

            var p = (Session["LoginUserSessionModel"] as LoginUserSessionModel).Person;
            var paddress = new PersonAddress()
            {
                Person = _context.Persons.Find(p.ID),
                Phone = model.Phone,
                Address = model.Address,
                AddressPerson = model.AddressPerson
            };
            p.PersonAddresses.Add(paddress);
            _context.PersonAddresses.Add(paddress);
            _context.SaveChanges();
            return RedirectToAction("Index", "Address");
        }

        public ActionResult EditAddress(AddressViewModel model)
        {

            return Json("");
        }
    }
}