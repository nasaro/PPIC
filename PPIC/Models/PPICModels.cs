using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace PPIC.Models
{
    public class MenuModels
    {
        public string MainMenuName { get; set; }
        public int MainMenuId { get; set; }
        public string SubMenuName { get; set; }
        public int SubMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class LoginModels
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter the User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? UserRoleId { get; set; }
        public string RoleName { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
    }

    public class Customers
    {
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public int? active { get; set; }


    }

    public class Employee
    {
        public int id { get; set; }
        public string nik { get; set; }
        public string nama { get; set; }
        public int department { get; set; }
        public int section { get; set; }
        public int subsection { get; set; }
        public int position { get; set; }
        public string namedep { get; set; }
        public string namesection { get; set; }
        public string namesubsection { get; set; }
        public string nameposition { get; set; }

    }


    public class ViewPOH
    {
        public string SO { get; set; }
        public string PO { get; set; }
        public string Date { get; set; }
        public string Customer { get; set; }
        public string KodeCustomer { get; set; }
        public string EDDate { get; set; }

    }
    public class ViewPOD
    {
        public string KodeProduct { get; set; }
        public string NameProduct { get; set; }
        public int? QtyOrder { get; set; }
        public int? QtyDelivery { get; set; }
        public int? QtySisa { get; set; }

    }

    public class POPlan
    {
        
        public string POCode { get; set; }
        public string SOCode { get; set; }
        public string INVCode { get; set; }
        public string CustCode { get; set; }
        public string DATEDelivery { get; set; }
        public int? QTYDelivery { get; set; }
    }

    public class Certificated
    {

        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string MatCertNo { get; set; }
        public string HeatNo { get; set; }
        public string INVCode { get; set; }
        public string FilePath { get; set; }
        public string DateUpdate { get; set; }
    }
    public class SendEmail
    {
        [DataType(DataType.EmailAddress), Display(Name = "To")]
        [Required]
        public string ToEmail { get; set; }
        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string EMailBody { get; set; }
        [Display(Name = "Subject")]
        public string EmailSubject { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "CC")]
        public string EmailCC { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "BCC")]
        public string EmailBCC { get; set; }
    }

    public class General
    {

        public int Id { get; set; }
        public int idCtq1 { get; set; }
        public int IdSeqH { get; set; }
        public int IdSeqD { get; set; }
        public int tacktime { get; set; }
        public int handlingtime { get; set; }
        public string remarkH { get; set; }
        public string remarkD { get; set; }
        public string dMaterialRequestDate { get; set; }
        public string dMMRDate { get; set; }
        public string FinishingCode { get; set; }
        public string cInvCode { get; set; }
        public string ProductName { get; set; }
        public string HeatNo { get; set; }
        public double Qty { get; set; }
        public double GrossWeight { get; set; }
        public double Total { get; set; }
        public string cRequestByUser { get; set; }
        public string CDeptID { get; set; }
        public string CDimintaOleh { get; set; }
        public string type { get; set; }
        public int cncTime { get; set; }
        public int manualTime { get; set; }
        public double upperdim { get; set; }
        public double lowerdim { get; set; }
        public double dimabs { get; set; }
        public string machineType { get; set; }
        public int measureID { get; set; }
        public string measureName { get; set; }

    }

    public class tools
    {
        public string cInvCode { get; set; }
        public string ProductName { get; set; }
       
    }

  
    public class Pajak
    {
        public string dPayment { get; set; }
        public string MRcode { get; set; }
        public string PVcode { get; set; }
        public double nominal { get; set; }
        public string seripajak { get; set; }

    }

    public class Machine
    {
        public int ID { get; set; }
        public int IDTools { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string NameTools { get; set; }
        public string Type { get; set; }
        public int Axis { get; set; }
        public string Revisi { get; set; }
        public string dValidate { get; set; }
        public double calibrate { get; set; }
        public string status { get; set; }
        public string codeMachine { get; set; }
        public decimal rpm { get; set; }
        public decimal runout { get; set; }
        public decimal chucksize { get; set; }
        public string model { get; set; }
        public string control { get; set; }
        public string brand { get; set; } 

    }

    public class OPERATOR
    {
        public string NIK { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PosID { get; set; }
        public string PosName { get; set; }

    }
    

    public class POR
    {
        public int ID { get; set; }
        public int IDProd { get; set; }
        public string NameProd { get; set; }
        public int IDCTQ { get; set; }
        public int IDMachine { get; set; }
        public string NameMachine { get; set; }
        public int sequence { get; set; }
        public int qty { get; set; }
        public string por { get; set; }
        public string remark { get; set; }
        public string dPOR { get; set; }

    }

    public class MachineFilter
    {
        public int idmesin { get; set; }
        public string mesin { get; set; }
        public string day1 { get; set; }
        public string day2 { get; set; }
        public string day3 { get; set; }
        public string day4 { get; set; }
        public string day5 { get; set; }
        public string day6 { get; set; }
        public string day7 { get; set; }
        public int status1 { get; set; }
        public int status2 { get; set; }
        public int status3 { get; set; }
        public int status4 { get; set; }
        public int status5 { get; set; }
        public int status6 { get; set; }
        public int status7 { get; set; }

    }

    public class Holiday
    {
        public int id { get; set; }
        public string ddate { get; set; }
        public string desc { get; set; }
        public string start { get; set; }
        public string stop { get; set; }
        public string type { get; set; }
    }

    public class survey
    {
        public int id { get; set; }
        public string dateStart { get; set; }
        public string dateStop { get; set; }
        public string status { get; set; }
        public string cusid { get; set; }
        public int productID { get; set; }
        public string cusname { get; set; }
        public decimal rate { get; set; }
        public string dateSurvey { get; set; }
        public string description { get; set; }
        public string descName { get; set; }
        public int achieve { get; set; }
    }

    public class TempObjectArray
    {
        public int id { get; set; }
        public string date1 { get; set; }
        public string date2 { get; set; }
        public string time1 { get; set; }
        public string time2 { get; set; }
        public string code1 { get; set; }
        public string code2 { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public int state { get; set; }
        public string desc { get; set; }
        public string codeForm { get; set; }
        public DateTime DDown { get; set; }
        public DateTime DClosed { get; set; }

        public decimal qty1 { get; set; }
    }

    public class ORB
    {
        public string dinquirydate { get; set; }
        public string cinquirycode { get; set; }
        public int icustomerproductid { get; set; }
        public string cproductname { get; set; }
        public int iinquiryqty { get; set; }
        public int totkirim { get; set; }
        public int totdelivery { get; set; }
    }
    public class ProgressOrb
    {
        public int id { get; set; }
        public string cInquiryCode { get; set; }
        public int iCustomerProductID { get; set; }
        [Required(ErrorMessage = "Please enter progress")]
        public int progress { get; set; }
        [Required(ErrorMessage = "Please enter Tgl")]
        public string tgl { get; set; }
    }

    public class Gifa
    {
        public string nmfile { get; set; }
        public int id { get; set; }
        public string visitor { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string industry { get; set; }
        public string department { get; set; }
        public string country { get; set; }
        public string contactStatus { get; set; }
        public string businessType { get; set; }
        public string productInterested { get; set; }
        public string visitorClassification { get; set; }
        public string followUp { get; set; }
        public string about { get; set; }
        public string location { get; set; }
        public string position { get; set; }
        public string ddate { get; set; }

    }

    public class FOH
    {
        public int id { get; set; }
        public decimal kwh { get; set; }
        public string ddate { get; set; }
        public string ttime { get; set; }
        public string dupdate { get; set; }
        public string remark { get; set; }

    }

    public class POMonitor
    {
        public string CPO { get; set; }
        public string invname { get; set; }
        public decimal qtyPO { get; set; }
        public string ddate { get; set; }

    }

    public class SCHEDULE
    {
        public DateTime dPOR { get; set; }
        public DateTime dEND { get; set; }
        public string gab { get; set; }
    }

    public class Barcode
    {
        public string barcode { get; set; }
        public string printcode { get; set; }
        public string cproductname { get; set; }
        public double qty { get; set; }
        public string dept { get; set; }
    }

    public class ppicsubcon
    {
        public string cpurchaseordercode { get; set; }
        public string cdate { get; set; }
        public int iqtyPO { get; set; }
        public string cproductname { get; set; }
        public int iqtydel { get; set; }
        public int iqtyrecieve { get; set; }

    }
}