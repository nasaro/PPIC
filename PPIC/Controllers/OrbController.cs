using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPIC.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using PPIC.Repository;

namespace PPIC.Controllers
{
    public class OrbController : Controller
    {
        
        // GET: Orb/GetAllOrbDetails      
        public JsonResult GetAllOrbDetails(int yyear)
        {
            OrbRepository OrbRepo = new OrbRepository();
            //return View(OrbRepo.GetAllOrb(yyear));
            return Json(OrbRepo.GetAllOrb(yyear), JsonRequestBehavior.AllowGet);
        }

        // GET: Orb
        public ActionResult Index()
        {
            return View();
        }

        // GET: Orb/GetAllProgress      
        public JsonResult GetAllProgress(int custProd, string orbCode)
        {
            OrbRepository progressRepo = new OrbRepository();
            return Json(progressRepo.GetAllDetailOrb(custProd, orbCode), JsonRequestBehavior.AllowGet);
        }

        // GET: Orb/GetAllProgress      
        public JsonResult GetDetailProgressID(int IdCode)
        {
            OrbRepository progressDetail = new OrbRepository();
            return Json(progressDetail.GetByIdProgress(IdCode), JsonRequestBehavior.AllowGet);
        }


        // GET: Orb/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Orb/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Orb/Create
        [HttpPost]
        public ActionResult Create(ProgressOrb _friend)
        {
            string endda = _friend.tgl;
            string tglB = endda.Substring(0, 2);
            string blnB = endda.Substring(3, 2);
            string thnB = endda.Substring(6, 4);
            string NewEndda = thnB + "/" + blnB + "/" + tglB;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PPIC"].ConnectionString))

            {

                string sqlQuery = "Insert Into tProgressOrb (iCustomerProductID, cInquiryCode, progress, tgl) Values(" + _friend.iCustomerProductID + ",\'" + _friend.cInquiryCode + "\'," + _friend.progress + ",\'" + NewEndda + "\')";

                int rowsAffected = db.Execute(sqlQuery);
            }

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        // GET: Orb/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult Edit(ProgressOrb _friend)
        {
            string endda = _friend.tgl;
            string tglB = endda.Substring(0, 2);
            string blnB = endda.Substring(3, 2);
            string thnB = endda.Substring(6, 4);
            string NewEndda = thnB + "/" + blnB + "/" + tglB;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PPIC"].ConnectionString))

            {

                string sqlQuery = "Update tProgressOrb Set progress = " + _friend.progress + ", tgl = \'" + NewEndda + "\'  Where id = " + _friend.id;

                int rowsAffected = db.Execute(sqlQuery);
            }

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        // GET: Orb/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Orb/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["PPIC"].ConnectionString))

            {

                string sqlQuery = "Delete tProgressOrb   Where id = " + id;

                int rowsAffected = db.Execute(sqlQuery);
            }

            //return RedirectToAction("Index");
            return new EmptyResult();
        }

        /*-------gifa exhibition form----------*/

        public ActionResult Gifa()
        {
            return View();
        }

        // GET: Orb/GetAllOrbDetails      
        public JsonResult GetAllGifa(int id, char pil)
        {
            OrbRepository gifaRepo = new OrbRepository();
            //return View(OrbRepo.GetAllOrb(yyear));
            return Json(gifaRepo.GetAllGifa(id, pil), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditGifa(Gifa _gifa)
        {
            int id = _gifa.id;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DBPPIC"].ConnectionString))

            {

                string sqlQuery = "Update mVisitor Set email = \'" + _gifa.email + "\',  position =\'" + _gifa.position + "\' , company = \'" + _gifa.company + "\', industry = \'" + _gifa.industry + "\' , department = \'" + _gifa.department + "\' , country = \'" + _gifa.country + "\' , contactStatus = \'" + _gifa.contactStatus + "\' ";
                sqlQuery += ", businessType = \'" + _gifa.businessType + "\' , productInterested = \'" + _gifa.productInterested + "\', visitorClassification =\'" + _gifa.visitorClassification + "\', followUp =\'" + _gifa.followUp + "\', about =\'" + _gifa.about + "\'  Where id = " + _gifa.id;

                int rowsAffected = db.Execute(sqlQuery);
            }

            return new EmptyResult();
        }
        /*-------end gifa exhibition form----------*/
       

        /*-------FOH form----------*/
        public ActionResult FOH()
        {
            return View();
        }

        // GET: Orb/GetAllFOHDetails      
        public JsonResult GetAllFOH(int id, int periode, char pil)
        {
            OrbRepository FOHRepo = new OrbRepository();
            return Json(FOHRepo.GetAllFOH(id, periode, pil), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditFOH(FOH _foh)
        {
            int id = _foh.id;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DBPPIC"].ConnectionString))

            {

                string sqlQuery = "Update tfoh Set remark = \'" + _foh.remark + "\'  Where id = " + _foh.id;
                int rowsAffected = db.Execute(sqlQuery);
            }

            return new EmptyResult();
        }

        /*-------PO Delivery Monitoring----------*/

        public ActionResult vPOMonitor()
        {
            return View();
        }

        // GET: Orb/GetAllOrbDetails      
        public JsonResult GetAllPODeliveryMonitor(int id)
        {
            OrbRepository gifaRepo = new OrbRepository();
            return Json(gifaRepo.GetAllPODelivery(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult vSchedulePOR()
        {
            return View();
        }

        public JsonResult GetAllScheduleMachine(int id)
        {
            OrbRepository scheduleRepo = new OrbRepository();
            return Json(scheduleRepo.GetAllSchedule(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBarcode(int id)
        {
            OrbRepository barcodeRepo = new OrbRepository();
            return Json(barcodeRepo.GetAllBarcode(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult vPpicSubCon()
        {
            return View();
        }

        public JsonResult GetAllPpicSubCon(int thn, int bln)
        {
            OrbRepository ppicsubcon = new OrbRepository();
            return Json(ppicsubcon.GetAllPpicSubCon(thn, bln), JsonRequestBehavior.AllowGet);
        }


    }
}
