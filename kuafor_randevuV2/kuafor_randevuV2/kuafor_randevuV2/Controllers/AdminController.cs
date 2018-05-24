using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kuafor_randevuV2.Models;
using System.Web.Security;

namespace kuafor_randevuV2.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (username != "" && password != "")
            {
                var isValid = "false";

                using (kuafor_randevuEntities1 entity = new kuafor_randevuEntities1())
                {
                    if (entity.admin.Any(x => x.kullanici_adi == username && x.sifre == password))
                    {
                        Session["user"] = username;
                        return RedirectToAction("Appoinments");
                    }
                    else
                    {
                        isValid = "true";
                        ViewBag.IsValid = "Kullanıcı adı veya şifre hatalı!";
                        return View();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Appoinments()
        {
            if (Session["user"] != null)
            {
                return View();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Prices()
        {
            if (Session["user"] != null )
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Personnel()
        {
            if (Session["user"] != null)
            {
                return View();

            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); 
            return RedirectToAction("Index");
        }

        public ActionResult GetAppoinmentData()
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                List<randevu> randevuList = db.randevu.ToList<randevu>();
                return Json(new { data = randevuList }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPriceData()
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                List<fiyat_listesi> fiyatList = db.fiyat_listesi.ToList<fiyat_listesi>();
                return Json(new { data = fiyatList }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPersonnelData()
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                List<calisanlar> calisanlarList = db.calisanlar.ToList<calisanlar>();
                return Json(new { data = calisanlarList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEditAppoinment(int id = 0)
        {
            if (id == 0)
            {
                return View(new randevu());
            }
            else
            {
                using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
                {
                    return View(db.randevu.Where(x => x.id == id).FirstOrDefault<randevu>());
                }
            }

        }

        [HttpGet]
        public ActionResult AddOrEditPrice(int id = 0)
        {
            if (id == 0)
            {
                return View(new fiyat_listesi());
            }
            else
            {
                using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
                {
                    return View(db.fiyat_listesi.Where(x => x.id == id).FirstOrDefault<fiyat_listesi>());
                }
            }

        }

        [HttpGet]
        public ActionResult AddOrEditPersonnel(int id = 0)
        {
            if (id == 0)
            {
                return View(new calisanlar());
            }
            else
            {
                using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
                {
                    return View(db.calisanlar.Where(x => x.id == id).FirstOrDefault<calisanlar>());
                }
            }

        }

        [HttpPost]
        public ActionResult AddOrEditAppoinment(randevu randevu)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                if (randevu.id == 0)
                {
                    db.randevu.Add(randevu);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(randevu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult AddOrEditPrice(fiyat_listesi fiyatlar)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                if (fiyatlar.id == 0)
                {
                    db.fiyat_listesi.Add(fiyatlar);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(fiyatlar).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult AddOrEditPersonnel(calisanlar calisanlar)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                if (calisanlar.id == 0)
                {
                    db.calisanlar.Add(calisanlar);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Kayıt Başarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(calisanlar).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Güncelleme Başarılı" }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public ActionResult DeleteAppoinment(int id)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                randevu icerik = db.randevu.Where(x => x.id == id).FirstOrDefault<randevu>();
                db.randevu.Remove(icerik);
                db.SaveChanges();
                return Json(new { success = true, message = "Randevu Kaydı Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeletePrice(int id)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                fiyat_listesi icerik = db.fiyat_listesi.Where(x => x.id == id).FirstOrDefault<fiyat_listesi>();
                db.fiyat_listesi.Remove(icerik);
                db.SaveChanges();
                return Json(new { success = true, message = "Fiyat Kaydı Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeletePersonnel(int id)
        {
            using (kuafor_randevuEntities1 db = new kuafor_randevuEntities1())
            {
                calisanlar icerik = db.calisanlar.Where(x => x.id == id).FirstOrDefault<calisanlar>();
                db.calisanlar.Remove(icerik);
                db.SaveChanges();
                return Json(new { success = true, message = "Personel Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}