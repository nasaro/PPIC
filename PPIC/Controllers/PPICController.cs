using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPIC.Models;
using System.Web.Security;
using System.Web.Helpers;
using BCrypt;
using System.IO;
using System.Threading;

namespace PPIC.Controllers
{
    public class PPICController : Controller
    {
        PPICDb ppicdb = new PPICDb();
        // GET: PPIC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImgCasting()
        {
            return View();
        }
        

        /**-----------  Manage Operator           ------------*/
        public ActionResult ManageOperator()
        {
            return View();
        }

        public JsonResult ListEmployeMachining()
        {
            return Json(ppicdb.ListEmployeMachining(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CrudOperator(OPERATOR opr, string type)
        {
            return Json(ppicdb.CrudOperator(opr, type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowOperator()
        {
            return Json(ppicdb.ShowOperator(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowOperatorName(string name)
        {
            return Json(ppicdb.ShowOperatorName(name), JsonRequestBehavior.AllowGet);
        }
        /**-----------  batas           ------------*/
        public JsonResult viewProduct(char code)
        {
            return Json(ppicdb.viewProduct(code), JsonRequestBehavior.AllowGet);
        }

        public ActionResult OutStandingPO()
        {
            return View();
        }

        public ActionResult MatCert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("/CertFrm/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }

        }

        public ActionResult deleteFileServer(string namaFile)
        {
            string fullPath;
            //var fullPath = Request.MapPath(path);
            fullPath = Path.Combine(Server.MapPath("/CertFrm/"), namaFile);
            if (!System.IO.File.Exists(fullPath)) return Json("No files selected.");

            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(fullPath);
                return Json("File Deleted Successfully!");
            }
            catch (Exception ex)                      
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }          
        }


        public JsonResult AddCert(Certificated cert)
        {
            return Json(ppicdb.AddCert(cert), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCert(Certificated cert)
        {
            return Json(ppicdb.DeleteCert(cert), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ListMemberActive()
        {
            return Json(ppicdb.ListMemberActive(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListCert(string id)
        {
            return Json(ppicdb.ListCert(id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login(LoginModels _login)
        {
            int userroleid = 0;
            int useridlogin = 0;
            string niklogin = "";
            if (ModelState.IsValid) //validating the user inputs
            {
                //bool isExist = false;
                string username = _login.UserName.Trim();
                string password = CryptoEngine.Encrypt(_login.Password.Trim());
                //string password = _login.Password.Trim();

                List<LoginModels> LogList = ppicdb.ListMenu(username, password);
                if (LogList.Count == 0)
                {
                    ViewBag.ErrorMsg = "Please enter the valid credentials!...";
                    return View();
                }
                userroleid = Convert.ToInt32(LogList[0].UserRoleId);
                if (userroleid > 0)
                {
                    List<MenuModels> LogListMenu = ppicdb.ListMenu(userroleid);

                    FormsAuthentication.SetAuthCookie(LogList[0].UserName, false);  // set the formauthentication cookie
                    Session["LoginCredentials"] = LogList[0];   // Bind the _logincredentials details to "LoginCredentials" session
                    Session["MenuMaster"] = LogListMenu;        //Bind the _menus list to MenuMaster session
                    Session["UserName"] = LogList[0].UserName;
                    Session["codeMMR"] = "";
                    Session["TypePrint"] = "";

                    useridlogin = Convert.ToInt32(LogList[0].UserId);
                    if (useridlogin > 0)
                    {
                        Session["UserIDLogin"] = useridlogin;
                    }
                    niklogin = LogList[0].CustCode;
                    if (niklogin != "")
                    {
                        Session["UserNIKLogin"] = niklogin;

                        List<Customers> LogNIkEmp = ppicdb.ListUserExternal(niklogin);
                        Session["UserIDEmployee"] = LogNIkEmp[0].CustCode;
                        Session["UserNameEmployee"] = LogNIkEmp[0].CustName;


                    }

                    if (userroleid == 5)
                        return RedirectToAction("ImgCasting", "PPIC");
                    else
                        return RedirectToAction("Index", "PPIC");
                }
                else
                {
                    ViewBag.ErrorMsg = "Please enter the valid credentials!...";
                    return View();
                }
            }
            return View();
        }

        public JsonResult ViewPOH(string ID)
        {
            return Json(ppicdb.ViewPOH(ID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListAllCertificated()
        {
            return Json(ppicdb.ListAllCertificated(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPdf(string fileName)
        {
            string filePath = "/CertFrm/" + fileName;
            Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);

            return File(filePath, "application/pdf");
        }

        public JsonResult ViewPOD(string ID)
        {
            return Json(ppicdb.ViewPOD(ID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddPlan(POPlan plan)
        {
            return Json(ppicdb.AddPlan(plan), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePlan(POPlan plan)
        {
            return Json(ppicdb.UpdatePlan(plan), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeletePlan(POPlan plan)
        {
            return Json(ppicdb.DeletePlan(plan), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewPlan(string Cust, string PO, string SO, string Inv)
        {
            return Json(ppicdb.ViewPlan(Cust, PO, SO, Inv), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOff()
        {

            Session["User"] = null;
            Session["UserIDLogin"] = null;
            Session["UserNIKLogin"] = null;
            Session["UserIDEmployee"] = null;
            Session["UserNameEmployee"] = null;
            Session["MenuMaster"] = null;
            Session["codeMMR"] = null;
            Session["TypePrint"] = null;

            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "PPIC");
        }

        [HttpPost]
        public ActionResult SendEmail(SendEmail obj)
        {

            try
            {
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "mail.allmart-precision.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 26;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = false;
                //EmailId used to send emails from application  
                WebMail.UserName = "ict02@allmart-precision.com";
                WebMail.Password = "Nn9@#23";

                //Sender email address.  
                WebMail.From = "ict02@allmart-precision.com";

                //Send email  
                WebMail.Send(to: obj.ToEmail, subject: obj.EmailSubject, body: obj.EMailBody, cc: obj.EmailCC, bcc: obj.EmailBCC, isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }
            return View();
        }

        public JsonResult Thrash()
        {
            return Json(ppicdb.Thrash(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult deliveryToKBM()
        {
            return View();
        }
        public JsonResult ViewToKBM(string tgl)
        {
            return Json(ppicdb.ViewToKBM(tgl), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewToKBMPeriod(string tgl, string tglB)
        {
            return Json(ppicdb.ViewToKBMPeriod(tgl, tglB), JsonRequestBehavior.AllowGet);
        }

        public ActionResult deliveryFromKBM()
        {
            return View();
        }
        public JsonResult ViewFromKBM(string tgl)
        {
            return Json(ppicdb.ViewFromKBM(tgl), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewFromKBMPeriod(string tgl, string tglB)
        {
            return Json(ppicdb.ViewFromKBMPeriod(tgl, tglB), JsonRequestBehavior.AllowGet);
        }

        public ActionResult consumablesRequest()
        {
            return View();
        }
        //ViewComsumables
        public JsonResult ViewComsumables(string tgl)
        {
            return Json(ppicdb.ViewComsumables(tgl), JsonRequestBehavior.AllowGet);
        }
        public ActionResult paymentTerm()
        {
            return View();
        }
        public JsonResult ViewPaymentTerm(string tgl)
        {
            return Json(ppicdb.ViewPaymentTerm(tgl), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewMatReceive(string tgl)
        {
            return Json(ppicdb.ViewMatReceive(tgl), JsonRequestBehavior.AllowGet);
        }

        public JsonResult passMatReceive(string id, string type)
        {
            int hasil;
            Session["codeMMR"] = id;
            Session["TypePrint"] = type;
            hasil = 1;
            return Json(hasil, JsonRequestBehavior.AllowGet);
        }
        //total delivery
        public ActionResult printTotDelivery()
        {
            return View();
        }

        public JsonResult vTotDelivery(string tgl)
        {
            return Json(ppicdb.vTotDelivery(tgl), JsonRequestBehavior.AllowGet);
        }

        public JsonResult viewDeliveryToFinish(string tgl)
        {
            return Json(ppicdb.viewDeliveryToFinish(tgl), JsonRequestBehavior.AllowGet);
        }


        public ActionResult NCRtoNMC()
        {
            return View();
        }
        public JsonResult ViewNCRtoNMC(string tgl)
        {
            return Json(ppicdb.ViewNCRtoNMC(tgl), JsonRequestBehavior.AllowGet);
        }

        public ActionResult deliveryToKBMMonthly()
        {
            return View();
        }

        public ActionResult deliveryFromKBMMonthly()
        {
            return View();
        }

        public JsonResult ViewKBMMonthly(string tgl,string masuk)
        {
            return Json(ppicdb.ViewKBMMonthly(tgl, masuk), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewKBMPeriod(string tglA, string tglB, string masuk)
        {
            return Json(ppicdb.ViewKBMPeriod(tglA, tglB, masuk), JsonRequestBehavior.AllowGet);
        }

        public ActionResult masterSequenceCtq()
        {
            return View();
        }
        public JsonResult ViewMachining()
        {
            return Json(ppicdb.ViewMachining(), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult ViewHeadSeq(int ID)
        {
            return Json(ppicdb.ViewHeadSeq(ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertSeqHeader(General obj)
        {
            return Json(ppicdb.InsertSeqHeader(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateseqHDR(General obj)
        {
            return Json(ppicdb.UpdateseqHDR(obj), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelseqHDR(General obj)
        {
            return Json(ppicdb.DelseqHDR(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertSeqDetail(General obj)
        {
            return Json(ppicdb.InsertSeqDetail(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateseqDTL(General obj)
        {
            return Json(ppicdb.UpdateseqDTL(obj), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelseqDTL(General obj)
        {
            return Json(ppicdb.DelseqDTL(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewDetailSeq(int idctq, int ID, int seq)
        {
            return Json(ppicdb.ViewDetailSeq(idctq, ID, seq), JsonRequestBehavior.AllowGet);
        }

        //ViewPVMRPAJAK
        public ActionResult ViewMRSeriPajak()
        {
            return View();
        }

        public JsonResult ViewNomerSeriPajak(int tgl)
        {
            return Json(ppicdb.ViewNomerSeriPajak(tgl), JsonRequestBehavior.AllowGet);
        }
        public JsonResult insertSeriPajak(Pajak Tax)
        {
            return Json(ppicdb.insertSeriPajak(Tax), JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteSeriPajak(Pajak Tax)
        {
            return Json(ppicdb.deleteSeriPajak(Tax), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewPVMRPAJAK(int tgl)
        {
            return Json(ppicdb.ViewPVMRPAJAK(tgl), JsonRequestBehavior.AllowGet);
        }

        //dayoff
        public ActionResult Holiday()
        {
            return View();
        }

        public JsonResult ViewHoliday()
        {
            return Json(ppicdb.ViewHoliday(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult addNewHoliday(Holiday lib)
        {
            return Json(ppicdb.addNewHoliday(lib), JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateHoliday(Holiday lib)
        {
            return Json(ppicdb.updateHoliday(lib), JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteHolidayID(Holiday lib)
        {
            return Json(ppicdb.deleteHolidayID(lib), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewHolidayID(int id)
        {
            return Json(ppicdb.ViewHolidayID(id), JsonRequestBehavior.AllowGet);
        }
        //breakhour
        public ActionResult BreakHour()
        {
            return View();
        }
        public JsonResult ViewBreakHour()
        {
            return Json(ppicdb.ViewBreakHour(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewBreakHourID(int id)
        {
            return Json(ppicdb.ViewBreakHourID(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult addNewBreakHour(Holiday lib)
        {
            return Json(ppicdb.addNewBreakHour(lib), JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteBreakHourID(Holiday lib)
        {
            return Json(ppicdb.deleteBreakHourID(lib), JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateBreakHour(Holiday lib)
        {
            return Json(ppicdb.updateBreakHour(lib), JsonRequestBehavior.AllowGet);
        }
        //Master Cutting Tools
        public ActionResult CuttTools()
        {
            return View();
        }
        public JsonResult viewCtqProd()
        {
            return Json(ppicdb.viewCtqProd(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult viewCuttTools()
        {
            return Json(ppicdb.CuttTools(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult showCuttTools()
        {
            return Json(ppicdb.showCuttTools(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult addNewCuttTools(Machine mec)
        {
            return Json(ppicdb.addNewCuttTools(mec), JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteCuttTools(int id)
        {
            return Json(ppicdb.deleteCuttTools(id), JsonRequestBehavior.AllowGet);
        }
        //Machine
        public ActionResult Machine()
        {
            return View();
        }

        public JsonResult ViewMachine()
        {
            return Json(ppicdb.ViewMachine(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewMachineFilter(int id)
        {
            return Json(ppicdb.ViewMachineFilter(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewMachineID(int id)
        {
            return Json(ppicdb.ViewMachineID(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteMachineID(int id)
        {
            return Json(ppicdb.deleteMachineID(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult addNewMachine(Machine mec)
        {
            return Json(ppicdb.addNewMachine(mec), JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateMachineID(Machine mec)
        {
            return Json(ppicdb.updateMachineID(mec), JsonRequestBehavior.AllowGet);
        }
        //tools
        public ActionResult Tools()
        {
            return View();
        }
        public JsonResult ViewMToolsAll()
        {
            return Json(ppicdb.ViewAllTools(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewMTools()
        {
            return Json(ppicdb.ViewTools(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewToolsID(int id)
        {
            return Json(ppicdb.ViewToolsID(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteToolsID(int id)
        {
            return Json(ppicdb.deleteToolsID(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult addNewTools(Machine mec)
        {
            return Json(ppicdb.addNewTools(mec), JsonRequestBehavior.AllowGet);

        }
        public JsonResult updateToolsID(Machine mec)
        {
            return Json(ppicdb.updateToolsID(mec), JsonRequestBehavior.AllowGet);
        }
        //measure
        public ActionResult Measure()
        {
            return View();
        }
        public JsonResult ViewMeasure()
        {
            return Json(ppicdb.ViewMeasure(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ViewMeasureID(int id)
        {
            return Json(ppicdb.ViewMeasureID(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteMeasureID(int id)
        {
            return Json(ppicdb.deleteMeasureID(id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult addNewMeasure(Machine mec)
        {
            return Json(ppicdb.addNewMeasure(mec), JsonRequestBehavior.AllowGet);

        }
        public JsonResult updateMeasureID(Machine mec)
        {
            return Json(ppicdb.updateMeasureID(mec), JsonRequestBehavior.AllowGet);
        }

        //-- capability
        public ActionResult Capability()
        {
            return View();
        }

        public JsonResult CrudCapablity(Machine mec, string type)
        {
            return Json(ppicdb.CrudCapablity(mec, type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowCapability(string nik)
        {
            return Json(ppicdb.ShowCapability(nik), JsonRequestBehavior.AllowGet);
        }
        //-- stock on hand product
        public ActionResult StockOnHand()
        {
            return View();
        }
        public JsonResult getStockOnHand(string tgl)
        {
            return Json(ppicdb.getStockOnHand(tgl), JsonRequestBehavior.AllowGet);
        }

        //-- POR
        public ActionResult vPOR()
        {
            return View();
        }
        public JsonResult getViewPOR()
        {
            return Json(ppicdb.getViewPOR(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LinktCtqP1(int idprod)
        {
            return Json(ppicdb.LinktCtqP1(idprod), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CrudPOR(POR mec, string type)
        {
            return Json(ppicdb.CrudPOR(mec, type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult showPOR(int prodcode, string porcode)
        {
            return Json(ppicdb.showPOR(prodcode, porcode), JsonRequestBehavior.AllowGet);
        }

        public ActionResult printMatReceive()
        {
            return View();
        }
        public ActionResult printDeliveryToKBM()
        {
            return View();
        }

        public ActionResult Survey()
        {
            return View();
        }

        public JsonResult ViewSurvey(int id, char ch)
        {
            return Json(ppicdb.ViewSurvey(id, ch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CRUDsurvey(survey svy, char ch)
        {
            return Json(ppicdb.CRUDsurvey(svy,ch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewSurveyDetail(int id)
        {
            return Json(ppicdb.ViewSurveyDetail(id), JsonRequestBehavior.AllowGet);
        }
        //----------reject customer
        public ActionResult RejectCustomer()
        {
            return View();
        }

        public JsonResult CRUDRejectCustomer(survey svy, char ch)
        {
            return Json(ppicdb.CRUDRejectCustomer(svy, ch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewReject(int id, char ch)
        {
            return Json(ppicdb.ViewReject(id, ch), JsonRequestBehavior.AllowGet);
        }
        public JsonResult VmCustomerProduct()
        {
            return Json(ppicdb.VmCustomerProduct(), JsonRequestBehavior.AllowGet);
        }

        //------Credit Nota
        public ActionResult CreditNote()
        {
            return View();
        }

        public JsonResult VmCustomer()
        {
            return Json(ppicdb.VmCustomer(), JsonRequestBehavior.AllowGet);
        }
        //------Achievement  
        public ActionResult Achievement()
        {
            return View();
        }

        public JsonResult CRUDAchievement(survey svy, char ch)
        {
            return Json(ppicdb.CRUDAchievement(svy, ch), JsonRequestBehavior.AllowGet);
        }

        //--------return purchase
        public ActionResult PurchaseReturn()
        {
            return View();
        }

        public JsonResult CRUDPurchaseReturn(TempObjectArray TPO, char ch)
        {
            return Json(ppicdb.CRUDPurchaseReturn(TPO, ch), JsonRequestBehavior.AllowGet);
        }
        public JsonResult vPurchaseReturn(int id, char ch)
        {
            return Json(ppicdb.vPurchaseReturn(id, ch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult VmPurchRet(string ID)
        {
            return Json(ppicdb.VmPurchRet(ID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult VmPurchPO()
        {
            //return new JsonResult { ppicdb.VmPurchPO(), MaxJsonLength = Int32.MaxValue };
            return Json(ppicdb.VmPurchPO(), JsonRequestBehavior.AllowGet);
        }

        //------Machine Down  
        public ActionResult MachineDown()
        {
            return View();
        }

        public JsonResult CRUDMachineDown(TempObjectArray TPO, char ch)
        {
            return Json(ppicdb.CRUDMachineDown(TPO, ch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult vMachineDown(int id, char ch)
        {
            return Json(ppicdb.vMachineDown(id, ch), JsonRequestBehavior.AllowGet);
        }
       
        /*---- view load draw machine --------*/
        public ActionResult vDrawMachine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadDrawMachine()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadFile/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }

        }

        public ActionResult GetPdfDrawmachine(string fileName)
        {
            string filePath = "/UploadFile/" + fileName;
            Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);

            return File(filePath, "application/pdf");

            /*------
            string filePath;
            string drek = @"E:\WebServer\PPIC\UploadFile\";
            //E:\WebServer\PPIC\UploadFile
            //local "D:\ASP MVC\PPIC\PPIC\UploadFile\
            filePath = Path.Combine(drek, fileName);
            Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);

            return File(filePath, "application/pdf");
            * ----*/
        }

        public ActionResult deleteFileDrawMachine(string namaFile)
        {
            string fullPath;
            //--var fullPath = Request.MapPath(path);
            string drek = @"E:\WebServer\PPIC\UploadFile\";
            //---kalo local---fullPath = Path.Combine(Server.MapPath("/UploadFile/"), namaFile);
            fullPath = Path.Combine(drek, namaFile);
            if (!System.IO.File.Exists(fullPath)) return Json("No files selected.");

            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(fullPath);
                return Json("File Deleted Successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        public JsonResult deleteData(int id)
        {
            return Json(ppicdb.deleteData(id), JsonRequestBehavior.AllowGet);
        }
        /*---- End view load draw machine --------*/
        public JsonResult CrudUploadDrawMachine(string nmfile, int id)
        {
            return Json(ppicdb.CrudUploadDrawMachine(nmfile, id), JsonRequestBehavior.AllowGet);
        }
        /*---- End view load draw machine --------*/

        /*----------------Draw Casting---------------------*/
        public ActionResult vDrawCasting()
        {
            return View();
        }

        public JsonResult vCastingImage(char code)
        {
            return Json(ppicdb.vCastingImage(code), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFileCasting()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/UploadFile/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }

        }

        public JsonResult CrudUploadFileCasting(string nmfile, int id)
        {
            return Json(ppicdb.CrudUploadFileCasting(nmfile, id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult deleteFileCasting(string namaFile)
        {
            string fullPath;
            //--var fullPath = Request.MapPath(path);
            string drek = @"E:\WebServer\PPIC\UploadFile\";
            //--fullPath = Path.Combine(Server.MapPath("/UploadFile/"), namaFile);
            fullPath = Path.Combine(drek, namaFile);
            if (!System.IO.File.Exists(fullPath)) return Json("No files selected.");

            try //Maybe error could happen like Access denied or Presses Already User used
            {
                System.IO.File.Delete(fullPath);
                return Json("File Deleted Successfully!");
            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }
        }

        public JsonResult deleteDataCasting(int id)
        {
            return Json(ppicdb.deleteDataCasting(id), JsonRequestBehavior.AllowGet);
        }


        public JsonResult vGenerate(int id1, int id2)
        {
            return Json(ppicdb.vGenerate(id1, id2), JsonRequestBehavior.AllowGet);
        }

        public ActionResult vBarcodePOR()
        {
            return View();
        }
    }
}