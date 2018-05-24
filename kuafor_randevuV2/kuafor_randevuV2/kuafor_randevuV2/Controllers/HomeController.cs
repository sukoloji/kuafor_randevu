using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using kuafor_randevuV2.Models;
using System.Data.SqlClient;

namespace kuafor_randevuV2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAppoinment()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string firstName, string lastName, string number, string email)
        {
            Session["firstName"] = firstName;
            Session["lastName"] = lastName;
            Session["number"] = number;
            Session["email"] = email;

            return RedirectToAction("Appoinment");
        }

        public ActionResult Appoinment()
        {
            if (Session["firstName"] != "" && Session["lastName"] != "" && Session["number"] != "")
            {
                kuafor_randevuEntities1 db = new kuafor_randevuEntities1();
                List<SelectListItem> services = new List<SelectListItem>();
                List<SelectListItem> personel = new List<SelectListItem>();

                foreach (var item in db.hizmet_turleri.ToList())
                {
                    services.Add(
                        new SelectListItem
                        {
                            Text = item.hizmet_tur,
                            Value = item.hizmet_tur
                        });
                }

                foreach (var item in db.calisanlar.ToList())
                {
                    personel.Add(
                        new SelectListItem
                        {
                            Text = item.calisan_adi,
                            Value = item.calisan_adi
                        });
                }

                ViewBag.services = services;
                ViewBag.personel = personel;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Appoinment(string TransDate, string hour, string services, string personel)
        {
            var isValid = "false";
            using(kuafor_randevuEntities1 entity = new kuafor_randevuEntities1())
            {
                if (entity.randevu.Any(x => x.randevu_tarih == TransDate.Replace("/",".") + " " + hour))
                {
                     isValid = "true";
                    kuafor_randevuEntities1 db = new kuafor_randevuEntities1();
                    List<SelectListItem> _services = new List<SelectListItem>();
                    List<SelectListItem> _personel = new List<SelectListItem>();

                    foreach (var item in db.hizmet_turleri.ToList())
                    {
                        _services.Add(
                            new SelectListItem
                            {
                                Text = item.hizmet_tur,
                                Value = item.hizmet_tur
                            });
                    }

                    foreach (var item in db.calisanlar.ToList())
                    {
                        _personel.Add(
                            new SelectListItem
                            {
                                Text = item.calisan_adi,
                                Value = item.calisan_adi
                            });
                    }

                    ViewBag.services = _services;
                    ViewBag.personel = _personel;
                    ViewBag.IsValid = "Bu tarih ve saat uygun değil, lütfen tekrar deneyiniz..";
                    return View();
                }
            }
            randevu _randevu = new randevu();
            _randevu.ad_soyad = Session["firstName"].ToString() + " " + Session["lastName"].ToString();
            _randevu.email = Session["email"].ToString();
            _randevu.tel = Session["number"].ToString();
            _randevu.randevu_tarih = TransDate + " " + hour;
            _randevu.hizmet_turu = services;
            _randevu.personel = personel;

            using (kuafor_randevuEntities1 entities = new kuafor_randevuEntities1())
            {
                entities.randevu.Add(_randevu);
                entities.SaveChanges();
            }

             return RedirectToAction("AppoinmentSuccess");
        }

        public ActionResult AppoinmentSuccess()
        {
            return View();
        }

        public ActionResult PriceList() {

            kuafor_randevuEntities1 db = new kuafor_randevuEntities1();
            var model = db.fiyat_listesi;
            return View(model);
        }
        public ActionResult ProcessTimes()
        {
            kuafor_randevuEntities1 db = new kuafor_randevuEntities1();
            var model = db.fiyat_listesi;
            return View(model);
        }

        public ActionResult List()
        {
            kuafor_randevuEntities1 db = new kuafor_randevuEntities1();
            var model = db.randevu;
            return View(model);
        }
    }
}