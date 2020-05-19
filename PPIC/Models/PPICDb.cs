using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BCrypt;

namespace PPIC.Models
{
    public class PPICDb
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["PPIC"].ConnectionString;
        string hrdcs = ConfigurationManager.ConnectionStrings["DBHRD"].ConnectionString;

        /*---Login  ----*/
        public List<LoginModels> ListMenu(string uname, string pname)
        {
            List<LoginModels> lst = new List<LoginModels>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Username", uname);
                com.Parameters.AddWithValue("@Password", pname);
                com.Parameters.AddWithValue("@Action", "Login");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new LoginModels
                    {
                        UserId = Convert.ToInt32(rdr["id"]),
                        CustCode = rdr["eCustCode"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        Password = rdr["Password"].ToString(),
                        UserRoleId = Convert.ToInt32(rdr["RoleId"])
                    });
                }
                con.Close();
                return lst;
            }
        }

        /*-----Menu model ------*/
        public List<MenuModels> ListMenu(int ID)
        {
            List<MenuModels> lst = new List<MenuModels>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Roleid", ID);
                com.Parameters.AddWithValue("@Action", "Menu");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new MenuModels
                    {
                        MainMenuName = rdr["MainMenu"].ToString(),
                        MainMenuId = Convert.ToInt32(rdr["MainMenuId"]),
                        SubMenuName = rdr["SubMenu"].ToString(),
                        SubMenuId = Convert.ToInt32(rdr["id"]),
                        ControllerName = rdr["Controller"].ToString(),
                        ActionName = rdr["Action"].ToString(),
                        RoleId = Convert.ToInt32(rdr["RoleId"]),
                        RoleName = rdr["Roles"].ToString()
                    });
                }
                con.Close();
                return lst;
            }

        }

        public List<LoginModels> ListManageUser()
        {
            List<LoginModels> lst = new List<LoginModels>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "LogAll");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new LoginModels
                    {
                        UserId = Convert.ToInt32(rdr["id"]),
                        UserName = rdr["UserName"].ToString(),
                        Password = rdr["Password"].ToString(),
                        CustCode = rdr["eCustCode"].ToString(),
                        UserRoleId = Convert.ToInt32(rdr["RoleId"]),
                        RoleName = rdr["Roles"].ToString()
                    });
                }
                con.Close();
                return lst;
            }

        }

        public int AddUser(LoginModels emp)
        {
            int i;
            string encPass = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                encPass = CryptoEngine.Encrypt(emp.Password);
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Username", emp.UserName);
                com.Parameters.AddWithValue("@Password", encPass);
                com.Parameters.AddWithValue("@Roleid", emp.UserRoleId);
                com.Parameters.AddWithValue("@CustCode", emp.CustCode);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int UpdateUser(LoginModels emp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserID", emp.UserId);
                com.Parameters.AddWithValue("@Password", emp.Password);
                com.Parameters.AddWithValue("@Roleid", emp.UserRoleId);
                com.Parameters.AddWithValue("@CustCode", emp.CustCode);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public int UpdateUserExt(LoginModels emp)
        {
            int i;
            string encPass = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                encPass = CryptoEngine.Encrypt(emp.Password);
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserID", emp.UserId);
                com.Parameters.AddWithValue("@Password", encPass);
                com.Parameters.AddWithValue("@Roleid", emp.UserRoleId);
                com.Parameters.AddWithValue("@CustCode", emp.CustCode);
                com.Parameters.AddWithValue("@Action", "UpdateExternal");
                i = com.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }
        public int DeleteUser(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserID", ID);
                com.Parameters.AddWithValue("@Action", "Delete");
                i = com.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public List<LoginModels> ListAllUser()
        {
            int nRoleid;
            List<LoginModels> lst = new List<LoginModels>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "LogAll");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["RoleId"] == DBNull.Value)
                        nRoleid = 0;
                    else
                        nRoleid = Convert.ToInt32(rdr["RoleId"]);
                    lst.Add(new LoginModels
                    {
                        UserId = Convert.ToInt32(rdr["id"]),
                        UserName = rdr["UserName"].ToString(),
                        Password = rdr["Password"].ToString(),
                        UserRoleId = nRoleid,
                        CustCode = rdr["eCustCode"].ToString(),
                        CustName = rdr["cCustName"].ToString(),
                        RoleName = rdr["Roles"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Customers> ListUserExternal(string NIK)
        {
            string name;
            List<Customers> lst = new List<Customers>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("[Extranet_sp_mLogin]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", NIK);
                com.Parameters.AddWithValue("@Action", "User");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["cCustName"].ToString().Length > 20)
                        name = rdr["cCustName"].ToString().Substring(0, 19);
                    else
                        name = rdr["cCustName"].ToString();

                    lst.Add(new Customers
                    {
                        CustCode = rdr["cCustcode"].ToString(),
                        CustName = name
                    });
                }
                con.Close();
                return lst;
            }
        }

        /*------MANAGE USER OPERATOR ------------------*/
        public List<Employee> ListEmployeMachining()
        {
            List<Employee> lst = new List<Employee>();
            using (SqlConnection conhrd = new SqlConnection(hrdcs))
            {
                conhrd.Open();
                SqlCommand com = new SqlCommand("sp_mEmployeeZAP", conhrd);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "Machine");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Employee
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        nik = rdr["nik"].ToString(),
                        nama = rdr["name"].ToString(),
                        department = Convert.ToInt32(rdr["department"]),
                        section = Convert.ToInt32(rdr["section"]),
                        position = Convert.ToInt32(rdr["position"]),
                        namedep = rdr["subD"].ToString(),
                        namesection = rdr["subS"].ToString(),
                        nameposition = rdr["subP"].ToString()
                    });
                }
                conhrd.Close();
                return lst;
            }
        }

        public int CrudOperator(OPERATOR oprt, string type)
        {
            int i;
            string encPass = "";
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mOperator", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@NIK", oprt.NIK);
                com.Parameters.AddWithValue("@Name", oprt.Name);
                com.Parameters.AddWithValue("@PosID", oprt.PosID);
                com.Parameters.AddWithValue("@Position", oprt.PosName);
                com.Parameters.AddWithValue("@USER", oprt.UserName);
                if (type == "ADD")
                {
                    encPass = CryptoEngine.Encrypt(oprt.Password);
                    com.Parameters.AddWithValue("@PASS", encPass);
                    com.Parameters.AddWithValue("@Action", "AddOprt");
                }
                else if (type == "EDIT")
                {
                    encPass = CryptoEngine.Encrypt(oprt.Password);
                    com.Parameters.AddWithValue("@PASS", encPass);
                    com.Parameters.AddWithValue("@Action", "EditOprt");
                }
                else if (type == "DELETE")
                    com.Parameters.AddWithValue("@Action", "DelOprt");
                else com.Parameters.AddWithValue("@Action", "Nothing");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<OPERATOR> ShowOperator()
        {
            List<OPERATOR> lst = new List<OPERATOR>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mOperator", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowOprt");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new OPERATOR
                    {
                        NIK = rdr["NIK"].ToString(),
                        Name = rdr["Name"].ToString(),
                        PosID = Convert.ToInt32(rdr["positionID"]),
                        PosName = rdr["position"].ToString(),
                        UserName = rdr["username"].ToString(),
                        Password = rdr["password"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }
        public List<OPERATOR> ShowOperatorName(string name)
        {
            List<OPERATOR> lst = new List<OPERATOR>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mOperator", con);
                com.CommandType = CommandType.StoredProcedure;                
                com.Parameters.AddWithValue("@Name", name);
                com.Parameters.AddWithValue("@Action", "ShowName");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new OPERATOR
                    {
                        NIK = rdr["NIK"].ToString(),
                        Name = rdr["Name"].ToString(),
                        PosID = Convert.ToInt32(rdr["positionID"]),
                        PosName = rdr["position"].ToString(),
                        UserName = rdr["username"].ToString(),
                        Password = rdr["password"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        /*------------BATAS-----------------*/

        public List<ViewPOH> ViewPOH(string ID)
        {
            List<ViewPOH> lst = new List<ViewPOH>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string name;
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_OutstandingSalesOrder", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", ID);
                com.Parameters.AddWithValue("@Action", "HEADERALL");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["NAMACUST"].ToString().Length > 48)
                        name = rdr["NAMACUST"].ToString().Substring(0, 49);
                    else
                        name = rdr["NAMACUST"].ToString();

                    lst.Add(new ViewPOH
                    {
                        SO = rdr["KODESO"].ToString(),
                        PO = rdr["KODEPO"].ToString(),
                        Date = string.Format("{0:dd/MM/yyyy }", rdr["TGLORDER"]),
                        KodeCustomer = rdr["KODECUST"].ToString(),
                        Customer = name,
                        EDDate = string.Format("{0:dd/MM/yyyy }", rdr["TGLESTIMASI"])
                    });
                }
                con.Close();
                return lst;
            }
        }
        public List<ViewPOD> ViewPOD(string ID)
        {
            int qtyorder, qtydelivery, qtysisa;
            List<ViewPOD> lst = new List<ViewPOD>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_OutstandingSalesOrder", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", ID);
                com.Parameters.AddWithValue("@Action", "DETAIL");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["QTYORDER"] == DBNull.Value)
                        qtyorder = 0;
                    else
                        qtyorder = Convert.ToInt32(rdr["QTYORDER"]);

                    if (rdr["QTYDELIVERY"] == DBNull.Value)
                        qtydelivery = 0;
                    else
                        qtydelivery = Convert.ToInt32(rdr["QTYDELIVERY"]);

                    if (rdr["QTYSISA"] == DBNull.Value)
                        qtysisa = 0;
                    else
                        qtysisa = Convert.ToInt32(rdr["QTYSISA"]);

                    lst.Add(new ViewPOD
                    {
                        KodeProduct = rdr["KODEPROD"].ToString(),
                        NameProduct = rdr["NAMAPROD"].ToString(),
                        QtyOrder = qtyorder,
                        QtyDelivery = qtydelivery,
                        QtySisa = qtysisa
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int AddPlan(POPlan plan)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_PlanOutStandingPO", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", plan.CustCode);
                com.Parameters.AddWithValue("@POCode", plan.POCode);
                com.Parameters.AddWithValue("@SOCode", plan.SOCode);
                com.Parameters.AddWithValue("@INVCode", plan.INVCode);
                com.Parameters.AddWithValue("@DATEDelivery", plan.DATEDelivery);
                com.Parameters.AddWithValue("@QTYDelivery", plan.QTYDelivery);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int UpdatePlan(POPlan plan)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_PlanOutStandingPO", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", plan.CustCode);
                com.Parameters.AddWithValue("@POCode", plan.POCode);
                com.Parameters.AddWithValue("@SOCode", plan.SOCode);
                com.Parameters.AddWithValue("@INVCode", plan.INVCode);
                com.Parameters.AddWithValue("@DATEDelivery", plan.DATEDelivery);
                com.Parameters.AddWithValue("@QTYDelivery", plan.QTYDelivery);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int DeletePlan(POPlan plan)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_PlanOutStandingPO", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", plan.CustCode);
                com.Parameters.AddWithValue("@POCode", plan.POCode);
                com.Parameters.AddWithValue("@SOCode", plan.SOCode);
                com.Parameters.AddWithValue("@INVCode", plan.INVCode);
                com.Parameters.AddWithValue("@DATEDelivery", plan.DATEDelivery);
                com.Parameters.AddWithValue("@Action", "Delete");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<Customers> ListMemberActive()
        {
            List<Customers> lst = new List<Customers>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "Active");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Customers
                    {
                        CustCode = rdr["cCustCode"].ToString(),
                        CustName = rdr["cCustName"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Certificated> ListCert(string id)
        {
            List<Certificated> lst = new List<Certificated>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_OutstandingSalesOrder", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CUSTCODE", id);
                com.Parameters.AddWithValue("@Action", "CERT");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Certificated
                    {
                        CustCode = rdr["cPoCodeReff"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<POPlan> ViewPlan(string Cust, string PO, string SO, string Inv)
        {
            int qtydelivery;
            string dateInv;
            List<POPlan> lst = new List<POPlan>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_PlanOutStandingPO", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", Cust);
                com.Parameters.AddWithValue("@POCode", PO);
                com.Parameters.AddWithValue("@SOCode", SO);
                com.Parameters.AddWithValue("@INVCode", Inv);
                com.Parameters.AddWithValue("@Action", "Show");
                SqlDataReader rdr = com.ExecuteReader();
                //SalesOrderCode, PoCodeReff, ProductCode, cCustCode, dDeliveryDate, iQtydelivery
                //POCode,SOCode,INVCode,CustCode,DATEDelivery,QTYDelivery
                while (rdr.Read())
                {
                    if (rdr["dDeliveryDate"] == DBNull.Value)
                        dateInv = "";
                    else
                        dateInv = string.Format("{0:dd/MM/yyyy }", rdr["dDeliveryDate"]);

                    if (rdr["iQtydelivery"] == DBNull.Value)
                        qtydelivery = 0;
                    else
                        qtydelivery = Convert.ToInt32(rdr["iQtydelivery"]);


                    lst.Add(new POPlan
                    {
                        POCode = rdr["PoCodeReff"].ToString(),
                        SOCode = rdr["SalesOrderCode"].ToString(),
                        INVCode = rdr["ProductCode"].ToString(),
                        CustCode = rdr["cCustCode"].ToString(),
                        DATEDelivery = dateInv,
                        QTYDelivery= qtydelivery
                    });
                }
                return lst;
            }
        }


        public List<Certificated> ListAllCertificated()
        {
            List<Certificated> lst = new List<Certificated>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_Support", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowAll");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Certificated
                    {                       
                        CustCode = rdr["cCustCode"].ToString(),
                        CustName = rdr["cCustName"].ToString(),
                        MatCertNo = rdr["MatCertNo"].ToString(),
                        HeatNo = rdr["HeatNo"].ToString(),
                        INVCode = rdr["InvNo"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        DateUpdate = string.Format("{0:dd/MM/yyyy }", rdr["dateUpd"]),
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int AddCert(Certificated cert)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_Support", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", cert.CustCode);
                com.Parameters.AddWithValue("@MatCertNo", cert.MatCertNo);
                com.Parameters.AddWithValue("@HeatNo", cert.HeatNo);
                com.Parameters.AddWithValue("@INVCode", cert.INVCode);
                com.Parameters.AddWithValue("@FilePath", cert.FilePath);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int DeleteCert(Certificated cert)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_Support", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CustCode", cert.CustCode);
                com.Parameters.AddWithValue("@MatCertNo", cert.MatCertNo);
                com.Parameters.AddWithValue("@HeatNo", cert.HeatNo);
                com.Parameters.AddWithValue("@INVCode", cert.INVCode);
                com.Parameters.AddWithValue("@FilePath", cert.FilePath);
                com.Parameters.AddWithValue("@Action", "Delete");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<Certificated> Thrash()
        {
            List<Certificated> lst = new List<Certificated>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_Support", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "Thrash");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new Certificated
                    {
                        CustCode = rdr["cCustCode"].ToString(),
                        MatCertNo = rdr["MatCertNo"].ToString(),
                        HeatNo = rdr["HeatNo"].ToString(),
                        INVCode = rdr["InvNo"].ToString(),
                        FilePath = rdr["FilePath"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }


        public List<General> ViewToKBM(string begda)
        {
            string tglA = begda.Trim().Substring(0, 2);
            string blnA = begda.Trim().Substring(3, 2);
            string thnA = begda.Trim().Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@Action", "TOKBM");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        FinishingCode = rdr["cDelProFinishingCode"].ToString(),
                        ProductName = rdr["cProductName"].ToString(),
                        HeatNo = rdr["heatid"].ToString(),
                        Qty = Convert.ToDouble(rdr["iqty"]),
                        GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                        Total = Convert.ToDouble(rdr["total"])

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewToKBMPeriod(string begda, string endda)
        {
            string tglA = begda.Trim().Substring(0, 2);
            string blnA = begda.Trim().Substring(3, 2);
            string thnA = begda.Trim().Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;
            string tglB = endda.Trim().Substring(0, 2);
            string blnB = endda.Trim().Substring(3, 2);
            string thnB = endda.Trim().Substring(6, 4);
            string spEnnda = thnB + "/" + blnB + "/" + tglB;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@DateParam2", spEnnda);
                com.Parameters.AddWithValue("@Action", "TOKBMPERIOD");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        FinishingCode = rdr["cDelProFinishingCode"].ToString(),
                        ProductName = rdr["cProductName"].ToString(),
                        HeatNo = rdr["heatid"].ToString(),
                        Qty = Convert.ToDouble(rdr["iqty"]),
                        GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                        Total = Convert.ToDouble(rdr["total"])

                    });
                }
                con.Close();
                return lst;
            }
        }


        public List<General> ViewFromKBM(string begda)
        {
            string tglA = begda.Trim().Substring(0, 2);
            string blnA = begda.Trim().Substring(3, 2);
            string thnA = begda.Trim().Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@Action", "FROMKBM");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        FinishingCode = rdr["cDelProFinishingCode"].ToString(),
                        ProductName = rdr["cProductName"].ToString(),
                        HeatNo = rdr["heatid"].ToString(),
                        Qty = Convert.ToDouble(rdr["iqty"]),
                        GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                        Total = Convert.ToDouble(rdr["total"])

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewFromKBMPeriod(string begda, string endda)
        {
            string tglA = begda.Trim().Substring(0, 2);
            string blnA = begda.Trim().Substring(3, 2);
            string thnA = begda.Trim().Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;
            string tglB = endda.Trim().Substring(0, 2);
            string blnB = endda.Trim().Substring(3, 2);
            string thnB = endda.Trim().Substring(6, 4);
            string spEndda = thnB + "/" + blnB + "/" + tglB;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@DateParam2", spEndda);
                com.Parameters.AddWithValue("@Action", "FROMKBMPERIOD");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        FinishingCode = rdr["cDelProFinishingCode"].ToString(),
                        ProductName = rdr["cProductName"].ToString(),
                        HeatNo = rdr["heatid"].ToString(),
                        Qty = Convert.ToDouble(rdr["iqty"]),
                        GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                        Total = Convert.ToDouble(rdr["total"])

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewComsumables(string begda)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);
            int qty;
            double  hpp;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "CONSUMABLES");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["iqty"] == DBNull.Value)
                        qty = 0;
                    else
                        qty = Convert.ToInt32(rdr["iqty"]);


                    if (rdr["iHPP"] == DBNull.Value)
                        hpp = 0;
                    else
                        hpp = Convert.ToDouble(rdr["iHPP"]);

                    lst.Add(new General
                    {
                        dMaterialRequestDate = string.Format("{0:dd/MM/yyyy }", rdr["dMaterialRequestDate"]),
                        FinishingCode = rdr["cMaterialRequestCode"].ToString(),
                        cInvCode = rdr["cInvCode"].ToString(),
                        ProductName = rdr["cInvName"].ToString(),
                        Qty = qty,
                        GrossWeight = hpp,
                        Total = Math.Round(qty * hpp, 2),
                        cRequestByUser = rdr["cRequestByUser"].ToString(),
                        CDeptID = rdr["CDeptID"].ToString(),
                        CDimintaOleh = rdr["CDimintaOleh"].ToString(),
                        type = rdr["type"].ToString()

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewPaymentTerm(string begda)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);
            int diff;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "PAYMENT");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["selisih"] == DBNull.Value)
                        diff = 0;
                    else
                        diff = Convert.ToInt32(rdr["selisih"]);


                    lst.Add(new General
                    {
                        dMaterialRequestDate = string.Format("{0:dd/MM/yyyy }", rdr["Tanggal"]),
                        FinishingCode = rdr["cVoucherCode"].ToString(),
                        cInvCode = rdr["cMMRCode"].ToString(),
                        dMMRDate = string.Format("{0:dd/MM/yyyy }", rdr["dMMRDate"]),
                        Qty = diff

                    });
                }
                con.Close();
                return lst;
            }
        }


        public List<General> ViewMatReceive(string begda)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);

            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "MATRECEIVE");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {


                    lst.Add(new General
                    {
                        cInvCode = rdr["cMMRCode"].ToString(),
                        dMMRDate = string.Format("{0:dd/MM/yyyy }", rdr["dMMRDate"])

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> vTotDelivery(string begda)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);

            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "vTotDelivery");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new General
                    {
                        Id = Convert.ToInt32(rdr["prodID"]),
                        ProductName = rdr["cDescription"].ToString(),
                        Total = Convert.ToDouble(rdr["totdelivery"])

                    });
                }
                con.Close();
                return lst;
            }
        }


        public List<General> viewDeliveryToFinish(string begda)
        {
            string tgl;
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "PRINTTOKBM");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["dDelProFinishingDate"] == DBNull.Value)
                        tgl = "";
                    else
                        tgl = string.Format("{0:dd/MM/yyyy }", rdr["dDelProFinishingDate"]);


                    lst.Add(new General
                    {
                        FinishingCode = rdr["cDelProFinishingCode"].ToString(),
                        dMMRDate = tgl,

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewNCRtoNMC(string begda)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                com.Parameters.AddWithValue("@Action", "NCRNMC");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new General
                    {
                        FinishingCode = rdr["cNCRCode"].ToString(),
                        ProductName = rdr["cProductName"].ToString(),
                        GrossWeight = Convert.ToDouble(rdr["weight"]),
                        Qty = Convert.ToInt32(rdr["iQTY"]),
                        cInvCode = rdr["cTipeReject"].ToString(),
                        HeatNo = rdr["cHeatNo"].ToString(),
                        cRequestByUser = rdr["cStatusProses2"].ToString(),
                        CDimintaOleh = rdr["cStatusProses3"].ToString(),
                        type = rdr["cgradeMaterial"].ToString(),
                        Total = Convert.ToDouble(rdr["totalWeight"]),


                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<General> viewProduct(char code)
        {
            string nFile="";
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                if (code=='C')
                    com.Parameters.AddWithValue("@Action", "CASTING");
                else
                    com.Parameters.AddWithValue("@Action", "MACHINING");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (code == 'C')
                    {
                        lst.Add(new General
                        {
                            Id = Convert.ToInt32(rdr["id"]),
                            FinishingCode = rdr["cGradeMaterial"].ToString(),
                            ProductName = rdr["cProductName"].ToString(),
                            GrossWeight = Convert.ToDouble(rdr["igrossweight"]),
                        });
                    }
                    else
                    {
                        if (rdr["nameFile"] == DBNull.Value)
                            nFile = "";
                        else
                            nFile = rdr["nameFile"].ToString();

                        lst.Add(new General
                        {
                            Id  = Convert.ToInt32(rdr["id"]),
                            FinishingCode = rdr["cGradeMaterial"].ToString(),
                            ProductName = rdr["cProductName"].ToString(),
                            GrossWeight = Convert.ToDouble(rdr["igrossweight"]),
                            measureName = nFile
                        });

                    }
                }
                con.Close();
                return lst;
            }
        }

        public List<General> ViewKBMMonthly(string begda, string inp)
        {
            string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA);


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
               
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", iBlnA);
                com.Parameters.AddWithValue("@YearParam", ithnA);
                if (inp=="TO")
                 com.Parameters.AddWithValue("@Action", "TOKBMMONTHLY");
                else if (inp == "FR")
                 com.Parameters.AddWithValue("@Action", "FROMKBMMONTHLY");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new General
                        {
                            ProductName = rdr["cProductName"].ToString(),
                            Qty = Convert.ToDouble(rdr["sqty"]),
                            GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                            Total = Convert.ToDouble(rdr["total"])

                        });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return lst;
            }
        }

        public List<General> ViewKBMPeriod(string begda, string endda, string inp)
        {
            string tglA = begda.Trim().Substring(0, 2);
            string blnA = begda.Trim().Substring(3, 2);
            string thnA = begda.Trim().Substring(6, 4);
            string tglB = endda.Trim().Substring(0, 2);
            string blnB = endda.Trim().Substring(3, 2);
            string thnB = endda.Trim().Substring(6, 4);

            string TGLa = thnA + '/' + blnA + '/' + tglA;
            string TGLb = thnB + '/' + blnB + '/' + tglB;


            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {

                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", TGLa);
                com.Parameters.AddWithValue("@DateParam2", TGLb);
                if (inp == "TO")
                    com.Parameters.AddWithValue("@Action", "TOKBMPERIOD");
                else if (inp == "FR")
                    com.Parameters.AddWithValue("@Action", "FROMKBMPERIOD");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new General
                        {
                            ProductName = rdr["cProductName"].ToString(),
                            Qty = Convert.ToDouble(rdr["iqty"]),
                            GrossWeight = Convert.ToDouble(rdr["iGrossWeight"]),
                            Total = Convert.ToDouble(rdr["total"])

                        });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                return lst;
            }
        }

        /*----------batas awal ---------------*/
        public List<Pajak> ViewNomerSeriPajak(int begda)
        {
            /*--string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA); --*/

            List<Pajak> lst = new List<Pajak>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@YearParam", begda);
                com.Parameters.AddWithValue("@Action", "PVMRPAJAK");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    //Tanggal,MRcode,PVcode,nominal
                    lst.Add(new Pajak
                    {
                        dPayment = string.Format("{0:dd/MM/yyyy }", rdr["Tanggal"]),
                        MRcode = rdr["MRcode"].ToString(),
                        PVcode = rdr["PVcode"].ToString(),
                        nominal = Convert.ToDouble(rdr["nominal"])
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int deleteSeriPajak(Pajak pjk)
        {
            int i;
            string tglA = pjk.dPayment.Substring(0, 2);
            string blnA = pjk.dPayment.Substring(3, 2);
            string thnA = pjk.dPayment.Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@MR", pjk.MRcode);
                com.Parameters.AddWithValue("@PV", pjk.PVcode);
                com.Parameters.AddWithValue("@SERI", pjk.seripajak);
                com.Parameters.AddWithValue("@Action", "DelPVMRPAJAK");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int insertSeriPajak(Pajak pjk)
        {
            int i;
            string tglA = pjk.dPayment.Substring(0, 2);
            string blnA = pjk.dPayment.Substring(3, 2);
            string thnA = pjk.dPayment.Substring(6, 4);
            string spBegda = thnA + "/" + blnA + "/" + tglA;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DateParam", spBegda);
                com.Parameters.AddWithValue("@MR", pjk.MRcode);
                com.Parameters.AddWithValue("@PV", pjk.PVcode);
                com.Parameters.AddWithValue("@NOMINAL", pjk.nominal);
                com.Parameters.AddWithValue("@SERI", pjk.seripajak);
                com.Parameters.AddWithValue("@Action", "InsPVMRPAJAK");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<Pajak> ViewPVMRPAJAK(int begda)
        {
            /*--string blnA = begda.Trim().Substring(0, 2);
            string thnA = begda.Trim().Substring(3, 4);
            int iBlnA = Convert.ToInt32(blnA);
            int ithnA = Convert.ToInt32(thnA); --*/

            List<Pajak> lst = new List<Pajak>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@YearParam", begda);
                com.Parameters.AddWithValue("@Action", "ViewPVMRPAJAK");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Pajak
                    {
                        dPayment = string.Format("{0:dd/MM/yyyy }", rdr["dTax"]),
                        MRcode = rdr["MRCode"].ToString(),
                        PVcode = rdr["PVCode"].ToString(),
                        nominal = Convert.ToDouble(rdr["Nominal"]),
                        seripajak = rdr["Noseri"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        /*--------------batas awal proses Ctq--------------------*/
        public List<General> ViewMachining()
        {
            int cncTime, manualTime;
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "VIEWMACHINING");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["CNCTime"] == DBNull.Value)
                        cncTime = 0;
                    else
                        cncTime = Convert.ToInt32(rdr["CNCTime"]);

                    if (rdr["ManualTime"] == DBNull.Value)
                        manualTime = 0;
                    else
                        manualTime = Convert.ToInt32(rdr["ManualTime"]);

                    lst.Add(new General
                    {
                        Id = Convert.ToInt32(rdr["id"]),
                        ProductName = rdr["Product"].ToString(),
                        cncTime = cncTime,
                        manualTime = manualTime
                    });
                }
                con.Close();
                return lst;
            }
        }


        public List<General> ViewHeadSeq(int idprod)
        {
            int tackTime, handTime;
            double chuck, rpm, rout;
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDProd", idprod);
                com.Parameters.AddWithValue("@Action", "seqHDR");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["tacktime"] == DBNull.Value)
                        tackTime = 0;
                    else
                        tackTime = Convert.ToInt32(rdr["tacktime"]);

                    if (rdr["handlingtime"] == DBNull.Value)
                        handTime = 0;
                    else
                        handTime = Convert.ToInt32(rdr["handlingtime"]);

                    if (rdr["chuckSize"] == DBNull.Value)
                        chuck = 0;
                    else
                        chuck = Convert.ToDouble(rdr["chuckSize"]);

                    if (rdr["rpm"] == DBNull.Value)
                        rpm = 0;
                    else
                        rpm = Convert.ToDouble(rdr["rpm"]);

                    if (rdr["runOut"] == DBNull.Value)
                        rout = 0;
                    else
                        rout = Convert.ToDouble(rdr["runOut"]);


                    lst.Add(new General
                    {
                        idCtq1 = Convert.ToInt32(rdr["idCtq1"]),
                        Id = Convert.ToInt32(rdr["idproduct"]),
                        IdSeqH = Convert.ToInt32(rdr["sequence_#"]),
                        tacktime = tackTime,
                        handlingtime = handTime,
                        remarkH = rdr["remark"].ToString(),
                        machineType = rdr["machineType"].ToString(),
                        dimabs  = chuck,
                        upperdim = rpm,
                        lowerdim = rout
                    });
                }
                con.Close();
                return lst;
            }
        }
        

        public int UpdateseqHDR(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;                
                com.Parameters.AddWithValue("@idCtq1", obj.idCtq1);
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Seq", obj.IdSeqH);
                com.Parameters.AddWithValue("@Info", obj.remarkH);
                com.Parameters.AddWithValue("@MachineType", obj.machineType);
                com.Parameters.AddWithValue("@Tack", obj.tacktime);
                com.Parameters.AddWithValue("@Hand", obj.handlingtime);
                com.Parameters.AddWithValue("@DimAbs", obj.dimabs);
                com.Parameters.AddWithValue("@UppDim", obj.upperdim);
                com.Parameters.AddWithValue("@LowDim", obj.lowerdim);
                com.Parameters.AddWithValue("@Action", "UpdateseqHDR");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int DelseqHDR(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idCtq1", obj.idCtq1);
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Seq", obj.IdSeqH);
                com.Parameters.AddWithValue("@Action", "DelseqHDR");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }
        public int InsertSeqHeader(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Info", obj.remarkH);
                com.Parameters.AddWithValue("@MachineType", obj.machineType);
                com.Parameters.AddWithValue("@Tack", obj.tacktime);
                com.Parameters.AddWithValue("@Hand", obj.handlingtime);
                com.Parameters.AddWithValue("@DimAbs", obj.dimabs);
                com.Parameters.AddWithValue("@UppDim", obj.upperdim);
                com.Parameters.AddWithValue("@LowDim", obj.lowerdim);
                com.Parameters.AddWithValue("@Action", "InsseqHDR");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int InsertSeqDetail(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idCtq1", obj.idCtq1);
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Seq", obj.IdSeqH);
                com.Parameters.AddWithValue("@Info", obj.remarkD);
                com.Parameters.AddWithValue("@DimAbs", obj.dimabs);
                com.Parameters.AddWithValue("@UppDim", obj.upperdim);
                com.Parameters.AddWithValue("@LowDim", obj.lowerdim);
                com.Parameters.AddWithValue("@Tools", obj.measureID);
                com.Parameters.AddWithValue("@Hand", obj.manualTime);
                com.Parameters.AddWithValue("@Action", "InsseqDTL");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int UpdateseqDTL(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idCtq1", obj.idCtq1);
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Seq", obj.IdSeqH);
                com.Parameters.AddWithValue("@SeqDtl", obj.IdSeqD);
                com.Parameters.AddWithValue("@Info", obj.remarkD);                
                com.Parameters.AddWithValue("@DimAbs", obj.dimabs);
                com.Parameters.AddWithValue("@UppDim", obj.upperdim);
                com.Parameters.AddWithValue("@LowDim", obj.lowerdim);
                com.Parameters.AddWithValue("@Tools", obj.measureID);
                com.Parameters.AddWithValue("@Hand", obj.manualTime);
                com.Parameters.AddWithValue("@Action", "UpdateseqDTL");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int DelseqDTL(General obj)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idCtq1", obj.idCtq1);
                com.Parameters.AddWithValue("@IDProd", obj.Id);
                com.Parameters.AddWithValue("@Seq", obj.IdSeqH);
                com.Parameters.AddWithValue("@SeqDtl", obj.IdSeqD);
                com.Parameters.AddWithValue("@Action", "DelseqDTL");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }


        public List<General> ViewDetailSeq(int idctq, int idprod, int seq)
        {
            double uppdim, lowdim, dimABS;
            string info;
            int check;
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@idCtq1", idctq);
                com.Parameters.AddWithValue("@IDProd", idprod);
                com.Parameters.AddWithValue("@Seq", seq);
                com.Parameters.AddWithValue("@Action", "seqDTL");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    if (rdr["dimabs"] == DBNull.Value)
                        dimABS = 0;
                    else
                        dimABS = Convert.ToDouble(rdr["dimabs"]);

                    if (rdr["upperdim"] == DBNull.Value)
                        uppdim = 0;
                    else
                        uppdim = Convert.ToDouble(rdr["upperdim"]);

                    if (rdr["lowerdim"] == DBNull.Value)
                        lowdim = 0;
                    else
                        lowdim = Convert.ToDouble(rdr["lowerdim"]);

                    if (rdr["remark"] == DBNull.Value)
                        info = "";
                    else
                        info = rdr["remark"].ToString();

                    if (rdr["intervalCheck"] == DBNull.Value)
                        check = 0;
                    else
                        check = Convert.ToInt32(rdr["intervalCheck"]);


                    lst.Add(new General
                    {
                        idCtq1 = Convert.ToInt32(rdr["idcTQ1"]),
                        Id = Convert.ToInt32(rdr["idproduct"]),
                        IdSeqH = Convert.ToInt32(rdr["sequence_#"]),
                        IdSeqD = Convert.ToInt32(rdr["ctq_#"]),
                        measureID = Convert.ToInt32(rdr["toolsID"]),
                        measureName = rdr["Name"].ToString(),
                        remarkD = info,
                        upperdim = uppdim,
                        lowerdim = lowdim,
                        dimabs = dimABS,
                        manualTime = check
                    });
                }
                con.Close();
                return lst;
            }
        }
        /*----------------batas trans CTQ--------------------*/
     

        //ViewMachine
        public List<Machine> ViewMachine()
        {
            string Code, Model, Control, Brand;
            decimal Rpm, RunOut, Size;
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "Show");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    if (rdr["MachineCode"] == DBNull.Value)
                        Code = "";
                    else
                        Code = rdr["MachineCode"].ToString();

                    if (rdr["rpm"] == DBNull.Value)
                        Rpm = 0;
                    else
                        Rpm = Convert.ToDecimal(rdr["rpm"]);

                    if (rdr["runOut"] == DBNull.Value)
                        RunOut = 0;
                    else
                        RunOut = Convert.ToDecimal(rdr["runOut"]);

                    if (rdr["chuckSize"] == DBNull.Value)
                        Size = 0;
                    else
                        Size = Convert.ToDecimal(rdr["chuckSize"]);

                    if (rdr["model"] == DBNull.Value)
                        Model = "";
                    else
                        Model = rdr["model"].ToString();

                    if (rdr["control"] == DBNull.Value)
                        Control = "";
                    else
                        Control = rdr["control"].ToString();

                    if (rdr["brand"] == DBNull.Value)
                        Brand = "";
                    else
                        Brand = rdr["brand"].ToString();

                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["machineID"]),
                        Name = rdr["machineName"].ToString(),
                        Type = rdr["machineType"].ToString(),
                        Axis = Convert.ToInt32(rdr["axis"]),
                        Revisi = rdr["revisiChuck"].ToString(),
                        codeMachine = Code,
                        rpm = Rpm,
                        runout = RunOut,
                        chucksize = Size,
                        model = Model,
                        control = Control,
                        brand = Brand

                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<MachineFilter> ViewMachineFilter(int idctq1)
        {
            string dday1, dday2, dday3, dday4, dday5, dday6, dday7;
            int sts1,sts2, sts3, sts4, sts5, sts6, sts7;
            List<MachineFilter> lst = new List<MachineFilter>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", idctq1);
                com.Parameters.AddWithValue("@Action", "ShowType");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    if (rdr["DAY1"] == DBNull.Value)
                    {
                        dday1 = ""; sts1 = 0;
                    }
                    else {
                        dday1 =  string.Format("{0:dd/MM/yyyy }", rdr["DAY1"]);
                        sts1 = 1;
                    }


                    if (rdr["DAY2"] == DBNull.Value)
                    {
                        dday2 = ""; sts2 = 0;
                    }
                    else {
                        dday2 = string.Format("{0:dd/MM/yyyy }", rdr["DAY2"]);
                        sts2 = 1;
                    }


                    if (rdr["DAY3"] == DBNull.Value)
                    {
                        dday3 = ""; sts3 = 0;
                    }
                    else
                    {
                        dday3 = string.Format("{0:dd/MM/yyyy }", rdr["DAY3"]);
                        sts3 = 1;
                    }

                    if (rdr["DAY4"] == DBNull.Value)
                    {
                        dday4 = ""; sts4 = 0;
                    }
                    else
                    {
                        dday4 = string.Format("{0:dd/MM/yyyy }", rdr["DAY4"]);
                        sts4 = 1;
                    }

                    if (rdr["DAY5"] == DBNull.Value)
                    {
                        dday5 = ""; sts5 = 0;
                    }
                    else
                    {
                        dday5 = string.Format("{0:dd/MM/yyyy }", rdr["DAY5"]);
                        sts5 = 1;
                    }

                    if (rdr["DAY6"] == DBNull.Value)
                    {
                        dday6 = ""; sts6 = 0;
                    }
                    else
                    {
                        dday6 = string.Format("{0:dd/MM/yyyy }", rdr["DAY6"]);
                        sts6 = 1;
                    }

                    if (rdr["DAY7"] == DBNull.Value)
                    {
                        dday7 = ""; sts7 = 0;
                    }
                    else
                    {
                        dday7 = string.Format("{0:dd/MM/yyyy }", rdr["DAY7"]);
                        sts7 = 1;
                    }


                    lst.Add(new MachineFilter
                    {
                        idmesin = Convert.ToInt32(rdr["idmachine"]),
                        mesin = rdr["namemachine"].ToString(),
                        day1 = dday1,
                        day2 = dday2,
                        day3 = dday3,
                        day4 = dday4,
                        day5 = dday5,
                        day6 = dday6,
                        day7 = dday7,
                        status1 = sts1,
                        status2 = sts2,
                        status3 = sts3,
                        status4 = sts4,
                        status5 = sts5,
                        status6 = sts6,
                        status7 = sts7,

                    });
                }
                con.Close();
                return lst;
            }
        }
        public List<Machine> ViewMachineID(int id)
        {
            string Code, Model, Control, Brand;
            decimal Rpm, RunOut, Size;

            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Action", "Showid");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["MachineCode"] == DBNull.Value)
                        Code = "";
                    else
                        Code = rdr["MachineCode"].ToString();

                    if (rdr["rpm"] == DBNull.Value)
                        Rpm = 0;
                    else
                        Rpm = Convert.ToDecimal(rdr["rpm"]);

                    if (rdr["runOut"] == DBNull.Value)
                        RunOut = 0;
                    else
                        RunOut = Convert.ToDecimal(rdr["runOut"]);

                    if (rdr["chuckSize"] == DBNull.Value)
                        Size = 0;
                    else
                        Size = Convert.ToDecimal(rdr["chuckSize"]);

                    if (rdr["model"] == DBNull.Value)
                        Model = "";
                    else
                        Model = rdr["model"].ToString();

                    if (rdr["control"] == DBNull.Value)
                        Control = "";
                    else
                        Control = rdr["control"].ToString();

                    if (rdr["brand"] == DBNull.Value)
                        Brand = "";
                    else
                        Brand = rdr["brand"].ToString();

                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["machineID"]),
                        Name = rdr["machineName"].ToString(),
                        Type = rdr["machineType"].ToString(),
                        Axis = Convert.ToInt32(rdr["axis"]),
                        Revisi = rdr["revisiChuck"].ToString(),
                        codeMachine = Code,
                        rpm = Rpm,
                        runout = RunOut,
                        chucksize = Size,
                        model = Model,
                        control = Control,
                        brand = Brand

                    });
                }
                con.Close();
                return lst;
            }
        }
        public int addNewMachine(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Name", mec.Name);
                    com.Parameters.AddWithValue("@Type", mec.Type);
                    com.Parameters.AddWithValue("@axis", mec.Axis);
                    com.Parameters.AddWithValue("@chuck", mec.Revisi);
                    com.Parameters.AddWithValue("@machineCode", mec.codeMachine);
                    com.Parameters.AddWithValue("@Rpm", mec.rpm);
                    com.Parameters.AddWithValue("@runOut", mec.runout);
                    com.Parameters.AddWithValue("@chuckSize", mec.chucksize);
                    com.Parameters.AddWithValue("@model", mec.model);
                    com.Parameters.AddWithValue("@control", mec.control);
                    com.Parameters.AddWithValue("@brand", mec.brand);                    
                    com.Parameters.AddWithValue("@Action", "Insert");
                    i = com.ExecuteNonQuery();                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
                return i;
            }
        }

        public int deleteMachineID(int id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID", id);
                    com.Parameters.AddWithValue("@Action", "DeleteID");
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
                return i;
            }
        }

        public int updateMachineID(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("Extranet_sp_mMachine", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ID", mec.ID);
                    com.Parameters.AddWithValue("@Name", mec.Name);
                    com.Parameters.AddWithValue("@Type", mec.Type);
                    com.Parameters.AddWithValue("@axis", mec.Axis);
                    com.Parameters.AddWithValue("@chuck", mec.Revisi);
                    com.Parameters.AddWithValue("@machineCode", mec.codeMachine);
                    com.Parameters.AddWithValue("@Rpm", mec.rpm);
                    com.Parameters.AddWithValue("@runOut", mec.runout);
                    com.Parameters.AddWithValue("@chuckSize", mec.chucksize);
                    com.Parameters.AddWithValue("@model", mec.model);
                    com.Parameters.AddWithValue("@control", mec.control);
                    com.Parameters.AddWithValue("@brand", mec.brand);
                    com.Parameters.AddWithValue("@Action", "Update");
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
                return i;
            }
        }

        //Holiday
        public List<Holiday> ViewHolidayID(int id)
        {
            List<Holiday> lst = new List<Holiday>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Action", "ShowDayid");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Holiday
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        ddate = string.Format("{0:dd/MM/yyyy }", rdr["ddate"]),
                        desc = rdr["desc"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Holiday> ViewHoliday()
        {
            List<Holiday> lst = new List<Holiday>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowDay");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Holiday
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        ddate = string.Format("{0:dd/MM/yyyy }", rdr["ddate"]),
                        desc = rdr["desc"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int addNewHoliday(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Ddate", lib.ddate);
                com.Parameters.AddWithValue("@Desc", lib.desc);
                com.Parameters.AddWithValue("@Action", "InsHoliday");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int deleteHolidayID(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", lib.id);
                com.Parameters.AddWithValue("@Action", "DelDayid");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int updateHoliday(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", lib.id);
                com.Parameters.AddWithValue("@Ddate", lib.ddate);
                com.Parameters.AddWithValue("@Desc", lib.desc);
                com.Parameters.AddWithValue("@Action", "UpdHoliday");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }
        //BreakHour
        public List<Holiday> ViewBreakHour()
        {
            List<Holiday> lst = new List<Holiday>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowBreak");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Holiday
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        start = string.Format("{0:HH:mm}", rdr["start"]),
                        stop = string.Format("{0:HH:mm}", rdr["stop"]),
                        type = rdr["type"].ToString(),
                        desc = rdr["desc"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Holiday> ViewBreakHourID(int id)
        {
            List<Holiday> lst = new List<Holiday>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                com.Parameters.AddWithValue("@Action", "ShowBreakid");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Holiday
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        start = string.Format("{0:HH:mm}", rdr["start"]),
                        stop = string.Format("{0:HH:mm}", rdr["stop"]),
                        type = rdr["type"].ToString(),
                        desc = rdr["desc"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int addNewBreakHour(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Start", lib.start);
                com.Parameters.AddWithValue("@Stop", lib.stop);
                com.Parameters.AddWithValue("@Type", lib.type);
                com.Parameters.AddWithValue("@Desc", lib.desc);
                com.Parameters.AddWithValue("@Action", "InsBreak");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int deleteBreakHourID(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", lib.id);
                com.Parameters.AddWithValue("@Action", "DelBreakid");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int updateBreakHour(Holiday lib)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mDay", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", lib.id);
                com.Parameters.AddWithValue("@Start", lib.start);
                com.Parameters.AddWithValue("@Stop", lib.stop);
                com.Parameters.AddWithValue("@Type", lib.type);
                com.Parameters.AddWithValue("@Desc", lib.desc);
                com.Parameters.AddWithValue("@Action", "UpdBreak");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        //CUTT TOOLS
        public List<General> viewCtqProd()
        {
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ViewCTQProd");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        idCtq1 = Convert.ToInt32(rdr["idCtq1"]),
                        remarkH = rdr["remark"].ToString(),
                        ProductName = rdr["cProductName"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        //---general - tools

        public List<tools> CuttTools()
        {
            List<tools> lst = new List<tools>();
            //List<string> mySKU = new List<string>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "CuttTools");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new tools
                    {
                        cInvCode = rdr["cInvCode"].ToString(),
                        ProductName = rdr["cInvName"].ToString()
                    });
                }
                con.Close();
                //mySKU = lst.Select(l => l.cInvCode).ToList();
                return lst;
            }
        }

        public List<General> showCuttTools()
        {
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mCutTools", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowTool");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {

                    lst.Add(new General
                    {
                        Id = Convert.ToInt32(rdr["id"]),
                        remarkH = rdr["remark"].ToString(),
                        cInvCode = rdr["cInvName"].ToString(),
                        ProductName = rdr["cProductName"].ToString()

                    });
                }
                con.Close();
                return lst;
            }
        }

        public int addNewCuttTools(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mCutTools", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IdCtq1", mec.IDTools);
                com.Parameters.AddWithValue("@CinvCode", mec.NameTools);
                com.Parameters.AddWithValue("@Action", "AddTool");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int deleteCuttTools(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mCutTools", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", ID);
                com.Parameters.AddWithValue("@Action", "DelTool");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }
        //Tools
        public List<Machine> ViewAllTools()
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowTool");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["toolsID"]),
                        Name = rdr["Name"].ToString(),
                        Type = rdr["Type"].ToString(),
                    });
                }
                con.Close();
                return lst;
            }
        }
        public List<Machine> ViewTools()            
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowToolDim");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["toolsID"]),
                        Name = rdr["Name"].ToString(),
                        Type = rdr["Type"].ToString(),
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Machine> ViewToolsID(int id)
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ToolsID", id);
                com.Parameters.AddWithValue("@Action", "ShowToolid");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["toolsID"]),
                        Name = rdr["Name"].ToString(),
                        Type = rdr["Type"].ToString()
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int addNewTools(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Name", mec.Name);
                com.Parameters.AddWithValue("@Type", mec.Type);
                com.Parameters.AddWithValue("@Action", "AddTool");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int deleteToolsID(int id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ToolsID", id);
                com.Parameters.AddWithValue("@Action", "DeleteTool");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int updateToolsID(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ToolsID", mec.IDTools);
                com.Parameters.AddWithValue("@Name", mec.Name);
                com.Parameters.AddWithValue("@Type", mec.Type);
                com.Parameters.AddWithValue("@Action", "EditTool");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        //Measure
        public List<Machine> ViewMeasure()
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowMeasure");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        IDTools = Convert.ToInt32(rdr["toolsID"]),
                        NameTools = rdr["NameTools"].ToString(),
                        ID = Convert.ToInt32(rdr["measureID"]),
                        Name = rdr["Name"].ToString(),
                        dValidate = string.Format("{0:dd/MM/yyyy }", rdr["dValidate"]),
                        calibrate = Convert.ToDouble(rdr["calibration"]),
                        status = rdr["status"].ToString(),
                    });
                }
                con.Close();
                return lst;
            }
        }

        public List<Machine> ViewMeasureID(int id)
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MeasureID", id);
                com.Parameters.AddWithValue("@Action", "ShowMeasureid");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        IDTools = Convert.ToInt32(rdr["toolsID"]),
                        NameTools = rdr["NameTools"].ToString(),
                        ID = Convert.ToInt32(rdr["measureID"]),
                        Name = rdr["Name"].ToString(),
                        dValidate = string.Format("{0:dd/MM/yyyy }", rdr["dValidate"]),
                        calibrate = Convert.ToDouble(rdr["calibration"]),
                        status = rdr["status"].ToString(),
                    });
                }
                con.Close();
                return lst;
            }
        }

        public int addNewMeasure(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ToolsID", mec.IDTools);
                com.Parameters.AddWithValue("@Name", mec.Name);
                com.Parameters.AddWithValue("@dValidate", mec.dValidate);
                com.Parameters.AddWithValue("@Calibration", mec.calibrate);
                com.Parameters.AddWithValue("@Status", mec.status);
                com.Parameters.AddWithValue("@Action", "AddMeasure");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int deleteMeasureID(int id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MeasureID", id);
                com.Parameters.AddWithValue("@Action", "DeleteMeasure");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public int updateMeasureID(Machine mec)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MeasureID", mec.ID);
                com.Parameters.AddWithValue("@ToolsID", mec.IDTools);
                com.Parameters.AddWithValue("@Name", mec.Name);
                com.Parameters.AddWithValue("@dValidate", mec.dValidate);
                com.Parameters.AddWithValue("@Calibration", mec.calibrate);
                com.Parameters.AddWithValue("@Status", mec.status);
                com.Parameters.AddWithValue("@Action", "EditMeasure");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        //capability
        public int CrudCapablity(Machine mec, string type)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {               
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ToolsID", mec.IDTools);
                com.Parameters.AddWithValue("@NIK", mec.Name);
                com.Parameters.AddWithValue("@Calibration", mec.calibrate);
                if (type == "ADD")
                    com.Parameters.AddWithValue("@Action", "AddCapabilty");
                else if (type == "EDIT")
                    com.Parameters.AddWithValue("@Action", "EditCapabilty");
                else if (type == "DELETE")
                    com.Parameters.AddWithValue("@Action", "DeleteCapabilty");
                else com.Parameters.AddWithValue("@Action", "Nothing");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<Machine> ShowCapability(string nik)
        {
            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mMeasure", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@NIK", nik);
                com.Parameters.AddWithValue("@Action", "ShowCapabiltyV2");

                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Machine
                    {
                        NIK = rdr["NIK"].ToString(),
                        Name = rdr["NameEmployee"].ToString(),
                        IDTools = Convert.ToInt32(rdr["toolsID"]),
                        NameTools = rdr["NameTools"].ToString(),
                        calibrate = Convert.ToDouble(rdr["capability"])
                    });
                }
                con.Close();
                return lst;
            }
        }

        //getStockOnHand
        public List<Machine> getStockOnHand(string tgl)
        {
            double awal, IN, OUT, stock;
            int BUL, THN;
            string blnA = tgl.Substring(3, 2);
            string thnA = tgl.Substring(6, 4);
            BUL = Convert.ToInt32(blnA);
            THN = Convert.ToInt32(thnA);

            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@MonthParam", BUL);
                com.Parameters.AddWithValue("@YearParam", THN);
                com.Parameters.AddWithValue("@Action", "ViewStock");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["awal"] == DBNull.Value)
                        awal = 0;
                    else
                        awal = Convert.ToDouble(rdr["awal"]);

                    if (rdr["qtyin"] == DBNull.Value)
                        IN = 0;
                    else
                        IN = Convert.ToDouble(rdr["qtyin"]);

                    if (rdr["qtyout"] == DBNull.Value)
                        OUT = 0;
                    else
                        OUT = Convert.ToDouble(rdr["qtyout"]);

                    stock = awal + IN - OUT;


                    lst.Add(new Machine
                    {
                        ID = Convert.ToInt32(rdr["id"]),
                        Name = rdr["cProductName"].ToString(),
                        calibrate = stock
                    });
                }
                con.Close();
                return lst;
            }
        }

        //---POR
        public List<Machine> getViewPOR()
        {
            double IN;
            //doule OUT, stock;

            List<Machine> lst = new List<Machine>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mPOR", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "POR");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["iQtyPOR"] == DBNull.Value)
                        IN = 0;
                    else
                        IN = Convert.ToDouble(rdr["iQtyPOR"]);

                    //if (rdr["qtyprod"] == DBNull.Value)
                    //    OUT = 0;
                    //else
                    //    OUT = Convert.ToDouble(rdr["qtyprod"]);

                    //stock = IN - OUT;

                    //if (stock > 0) {
                        lst.Add(new Machine
                        {
                            ID = Convert.ToInt32(rdr["id"]),
                            Name = rdr["cProductionOrderRequestCode"].ToString(),
                            NameTools = rdr["cProductName"].ToString(),
                            calibrate = IN
                        });
                    //}
                    
                }
                con.Close();
                return lst;
            }
        }

        public int CrudPOR(POR mec, string type)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mPOR", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", mec.ID);
                com.Parameters.AddWithValue("@IDPROD", mec.IDProd);
                com.Parameters.AddWithValue("@IDCTQ1", mec.IDCTQ);
                com.Parameters.AddWithValue("@IDMACHINE", mec.IDMachine);
                com.Parameters.AddWithValue("@SEQUENCE", mec.sequence);
                com.Parameters.AddWithValue("@POR", mec.por);
                com.Parameters.AddWithValue("@REMARK", mec.remark);
                com.Parameters.AddWithValue("@QTY", mec.qty);
                com.Parameters.AddWithValue("@dPOR", mec.dPOR);
                if (type == "ADD")
                    com.Parameters.AddWithValue("@Action", "INSERT");
                else if (type == "EDIT")
                    com.Parameters.AddWithValue("@Action", "UPDATE");
                else if (type == "DELETE")
                    com.Parameters.AddWithValue("@Action", "DELETE");
                else if (type == "LINK")
                    com.Parameters.AddWithValue("@Action", "LinktCtq");
                else if (type == "LINKP2")
                    com.Parameters.AddWithValue("@Action", "LinktCtqP2");
                else
                    com.Parameters.AddWithValue("@Action", "Nothing");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        public List<POR> LinktCtqP1(int prod)
        {
            string remark ;
            List<POR> lst = new List<POR>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mPOR", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDPROD", prod);
                com.Parameters.AddWithValue("@Action", "LinktCtqP1");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["remark"] == DBNull.Value)
                        remark = "";
                    else
                        remark = rdr["remark"].ToString();

                    lst.Add(new POR
                    {
                        IDCTQ = Convert.ToInt32(rdr["idCtq1"]),
                        IDProd = prod,
                        remark = rdr["remark"].ToString(),
                        sequence = Convert.ToInt32(rdr["sequence_#"])
                    });

                }
                con.Close();
                return lst;
            }
        }

        public List<POR> showPOR(int prod, string por)
        {
            string remark, nameMachine, tglPOR;
            int idMachine, Qty;
            List<POR> lst = new List<POR>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mPOR", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDPROD", prod);
                com.Parameters.AddWithValue("@POR", por);
                com.Parameters.AddWithValue("@Action", "ShowPOR");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["remark"] == DBNull.Value)
                        remark = "";
                    else
                        remark = rdr["remark"].ToString();

                    if (rdr["machineName"] == DBNull.Value)
                        nameMachine = "";
                    else
                        nameMachine = rdr["machineName"].ToString();

                    if (rdr["idmachine"] == DBNull.Value)
                        idMachine = 0;
                    else
                        idMachine = Convert.ToInt32(rdr["idmachine"]);

                    if (rdr["dPOR"] == DBNull.Value)
                        tglPOR = "";
                    else
                        tglPOR = string.Format("{0:dd/MM/yyyy }", rdr["dPOR"]);

                    if (rdr["qty"] == DBNull.Value)
                        Qty = 0;
                    else
                        Qty = Convert.ToInt32(rdr["qty"]);


                    lst.Add(new POR
                    {
                        ID = Convert.ToInt32(rdr["id"]),
                        IDCTQ = Convert.ToInt32(rdr["idCtq1"]),
                        IDProd = Convert.ToInt32(rdr["idprod"]),
                        NameProd = rdr["cProductName"].ToString(),
                        sequence = Convert.ToInt32(rdr["sequence_#"]),
                        por = rdr["por_#"].ToString(),
                        IDMachine = idMachine,
                        NameMachine = nameMachine,
                        remark = remark,
                        dPOR = tglPOR,
                        qty = Qty
                    });

                }
                con.Close();
                return lst;
            }
        }

        public List<survey> ViewSurvey(int id, char ch)
        {
            List<survey> lst = new List<survey>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mSurvey", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDsurvey", id);
                
                if (ch == 'A')
                    com.Parameters.AddWithValue("@Action", "ShowSurvey");
                else com.Parameters.AddWithValue("@Action", "ShowSurveyID");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new survey
                        {
                            id = Convert.ToInt32(rdr["idsurvey"]),
                            dateStart = string.Format("{0:dd/MM/yyyy }", rdr["dateStart"]),
                            dateStop = string.Format("{0:dd/MM/yyyy }", rdr["dateStop"]),
                            status = rdr["status"].ToString()
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public List<survey> ViewSurveyDetail(int id)
        {
            decimal Rates;
            List<survey> lst = new List<survey>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mSurvey", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDsurvey", id);
                com.Parameters.AddWithValue("@Action", "showDetail");

                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        Rates = Math.Round(Convert.ToDecimal(rdr["val"]) / 21,2);
                        lst.Add(new survey
                        {
                            id = Convert.ToInt32(rdr["idsurvey"]),
                            cusid = rdr["cusid"].ToString(),
                            cusname  = rdr["cCustName"].ToString(),
                            dateSurvey = string.Format("{0:dd/MM/yyyy }", rdr["dateSurvey"]),
                            rate = Rates
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public int CRUDsurvey(survey svy, char ch)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mSurvey", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDsurvey", svy.id);
                com.Parameters.AddWithValue("@Cusid", svy.cusid);
                com.Parameters.AddWithValue("@dStart", svy.dateStart);
                com.Parameters.AddWithValue("@dStop", svy.dateStop);
                com.Parameters.AddWithValue("@Status", svy.status);
                if (ch == 'I')
                    com.Parameters.AddWithValue("@Action", "addSurvey");
                else if (ch == 'U')
                    com.Parameters.AddWithValue("@Action", "editSurvey");
                else if (ch == 'D')
                    com.Parameters.AddWithValue("@Action", "deleteSurvey");
                try
                {
                    con.Open();
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return i;

            }
        }

        public int CRUDRejectCustomer(survey svy, char ch)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", svy.id);
                com.Parameters.AddWithValue("@IDDESC", svy.productID);
                com.Parameters.AddWithValue("@DATE1", svy.dateSurvey);
                com.Parameters.AddWithValue("@DATE2", svy.cusid);
                com.Parameters.AddWithValue("@DESC", svy.description);
                com.Parameters.AddWithValue("@KPI1", svy.rate);
                if (ch == 'I')
                    com.Parameters.AddWithValue("@Action", "InsReject");
                else if (ch == 'U')
                    com.Parameters.AddWithValue("@Action", "UpdReject");
                else if (ch == 'D')
                    com.Parameters.AddWithValue("@Action", "DelReject");
                else if (ch == 'i')
                    com.Parameters.AddWithValue("@Action", "InsCredit");
                else if (ch == 'u')
                    com.Parameters.AddWithValue("@Action", "UpdCredit");
                else if (ch == 'd')
                    com.Parameters.AddWithValue("@Action", "DelCredit");
                try
                {
                    con.Open();
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return i;

            }
        }

        public List<survey> ViewReject(int id, char ch)
        {
            string desciptionName="";
            List<survey> lst = new List<survey>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);

                if (ch == 'A')
                    com.Parameters.AddWithValue("@Action", "ShowReject");
                else if (ch == 'I') com.Parameters.AddWithValue("@Action", "ShowRejectID");
                else if (ch == 'C') com.Parameters.AddWithValue("@Action", "ShowCredit");
                else if (ch == 'c') com.Parameters.AddWithValue("@Action", "ShowCreditID");
                else if (ch == 'V') com.Parameters.AddWithValue("@Action", "ShowAchieve");
                else if (ch == 'v') com.Parameters.AddWithValue("@Action", "ShowAchieveID");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (ch == 'A' || ch == 'I') {
                            lst.Add(new survey
                            {
                                id = Convert.ToInt32(rdr["id"]),
                                dateSurvey = string.Format("{0:dd/MM/yyyy }", rdr["dTgl"]),
                                productID = Convert.ToInt32(rdr["iCustomerProductID"]),
                                cusname = rdr["Name"].ToString(),
                                rate = Convert.ToDecimal(rdr["Qty"]),
                                description = rdr["desc"].ToString()
                            });
                        }
                        else if (ch == 'C' || ch == 'c')
                        {
                            lst.Add(new survey
                            {
                                id = Convert.ToInt32(rdr["id"]),
                                dateSurvey = string.Format("{0:dd/MM/yyyy }", rdr["dTgl"]),
                                cusid = rdr["cCustCode"].ToString(),
                                cusname = rdr["Name"].ToString(),
                                rate = Convert.ToDecimal(rdr["amount"]),
                                description = rdr["desc"].ToString()
                            });

                        }
                        else
                        {
                            switch (rdr["desc"].ToString())
                            {
                                case "NP1":
                                    desciptionName = "ZENGATE+ 100mm, 150mm, 200mm (Progress)";
                                    break;
                                case "NP2":
                                    desciptionName = "ZENPUMP+ Centrifugal 1 - 1.5inch, 2inch & 2.5inch (Progress)";
                                    break;
                                case "NP3":
                                    desciptionName = "ZENPUMP+ Loop 1 - 1.5inch, 2inch & 2.5inch (Progress)";
                                    break;
                                case "NPK":
                                    desciptionName = "ZENGATE+ c/w Actuator 1 - 1.5inch, 2inch & 2.5inch (Progress)";
                                    break;
                                case "PED":
                                    desciptionName = "PED - Cert";
                                    break;
                                case "ISOMD":
                                    desciptionName = "ISO - 13485 & MDD";
                                    break;
                                case "PM":
                                    desciptionName = "Preventive Maintenance";
                                    break;
                                case "FNC":
                                    desciptionName = "Finishing Cost";
                                    break;
                                case "NPI":
                                    desciptionName = "New Product Innovation";
                                    break;
                                default:
                                    desciptionName = "";
                                    break;
                            }

                            lst.Add(new survey
                            {
                                id = Convert.ToInt32(rdr["id"]),
                                dateSurvey = string.Format("{0:dd/MM/yyyy }", rdr["dDate"]),
                                achieve = Convert.ToInt32(rdr["Achievement"]),
                                description = rdr["desc"].ToString(),
                                descName = desciptionName
                            });

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public List<survey> VmCustomerProduct()
        {
            List<survey> lst = new List<survey>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowProd");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new survey
                        {
                            id = Convert.ToInt32(rdr["id"]),
                            cusname = rdr["Name"].ToString()
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public List<survey> VmCustomer()
        {
            List<survey> lst = new List<survey>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowCust");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new survey
                        {
                            cusid = rdr["id"].ToString(),
                            cusname = rdr["Name"].ToString()
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public int CRUDAchievement(survey svy, char ch)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", svy.id);
                com.Parameters.AddWithValue("@DATE1", svy.dateSurvey);
                com.Parameters.AddWithValue("@DESC", svy.description);
                com.Parameters.AddWithValue("@GRADE", svy.achieve);
                if (ch == 'I')
                    com.Parameters.AddWithValue("@Action", "InsAchieve");
                else if (ch == 'U')
                    com.Parameters.AddWithValue("@Action", "UpdAchieve");
                else if (ch == 'D')
                    com.Parameters.AddWithValue("@Action", "DelAchieve");
                try
                {
                    con.Open();
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return i;

            }
        }

        public int CRUDPurchaseReturn(TempObjectArray TPO, char ch)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", TPO.id);
                com.Parameters.AddWithValue("@DDATE1", TPO.date1);
                com.Parameters.AddWithValue("@CODE1", TPO.code1);
                com.Parameters.AddWithValue("@CODE2", TPO.code2);
                com.Parameters.AddWithValue("@DESC", TPO.desc);
                com.Parameters.AddWithValue("@QTY1", TPO.qty1);
                if (ch == 'I')
                    com.Parameters.AddWithValue("@Action", "InsPurcRet");
                else if (ch == 'U')
                    com.Parameters.AddWithValue("@Action", "UpdPurcRet");
                else if (ch == 'D')
                    com.Parameters.AddWithValue("@Action", "DelPurcRet");
                try
                {
                    con.Open();
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return i;

            }
        }

        public List<TempObjectArray> vPurchaseReturn(int id, char ch)
        {
            List<TempObjectArray> lst = new List<TempObjectArray>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                if (ch == 'A')
                    com.Parameters.AddWithValue("@Action", "ShowPurcRet");
                else if (ch == 'B')
                    com.Parameters.AddWithValue("@Action", "ShowByIDPurcRet");

                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new TempObjectArray
                        {
                            id = Convert.ToInt32(rdr["id"]),
                            date1 = string.Format("{0:dd/MM/yyyy }", rdr["dDate"]),
                            code1 = rdr["cInvCode"].ToString(),
                            name1 = rdr["cInvName"].ToString(),
                            code2 = rdr["cPurchaseOrderCode"].ToString(),
                            qty1 = Convert.ToDecimal(rdr["qty"]),
                            desc = rdr["desc"].ToString()
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public List<TempObjectArray> VmPurchRet(string code)
        {
            List<TempObjectArray> lst = new List<TempObjectArray>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@CODE1", code);
                com.Parameters.AddWithValue("@Action", "ShowInv");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new TempObjectArray
                        {
                            code1 = rdr["Code"].ToString(),
                            name1 = rdr["Name"].ToString(),
                            qty1 =  Convert.ToDecimal(rdr["Qty"])
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public List<TempObjectArray> VmPurchPO()
        {
            List<TempObjectArray> lst = new List<TempObjectArray>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Action", "ShowPurcOrd");
                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {

                        lst.Add(new TempObjectArray
                        {
                            code1 = rdr["Code"].ToString(),
                            name1 = rdr["Name"].ToString()
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        public int CRUDMachineDown(TempObjectArray TPO, char ch)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", TPO.id);
                com.Parameters.AddWithValue("@DDOWN", TPO.date1);
                com.Parameters.AddWithValue("@MACHINE", TPO.name1);
                com.Parameters.AddWithValue("@CODE1", TPO.name2);
                com.Parameters.AddWithValue("@ISSUE", TPO.code1);
                com.Parameters.AddWithValue("@ACT", TPO.code2);
                com.Parameters.AddWithValue("@DCLOSED", TPO.date2);                
                com.Parameters.AddWithValue("@STATE", TPO.state);
                com.Parameters.AddWithValue("@DESC", TPO.desc);
                if (ch == 'I')
                    com.Parameters.AddWithValue("@Action", "InsMachine");
                else if (ch == 'U')
                    com.Parameters.AddWithValue("@Action", "UpdMachine");
                else if (ch == 'D')
                    com.Parameters.AddWithValue("@Action", "DelMachine");
                try
                {
                    con.Open();
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return i;

            }
        }

        public List<TempObjectArray> vMachineDown(int id, char ch)
        {
            List<TempObjectArray> lst = new List<TempObjectArray>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string aksi, oleh, tglttp, catatan, Time2, kode;
                SqlCommand com = new SqlCommand("Extranet_sp_mKPI_Dept_V3", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", id);
                if (ch == 'A')
                    com.Parameters.AddWithValue("@Action", "ShowMachine");
                else if (ch == 'B')
                    com.Parameters.AddWithValue("@Action", "ShowByIDMachine");

                try
                {
                    con.Open();
                    SqlDataReader rdr = com.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr["Action"] == DBNull.Value)
                            aksi = "";
                        else
                            aksi = rdr["Action"].ToString();

                        if (rdr["ByWho"] == DBNull.Value)
                            oleh = "";
                        else
                            oleh = rdr["ByWho"].ToString();

                        if (rdr["ClosedDate"] == DBNull.Value)
                            tglttp = "";
                        else
                            tglttp = string.Format("{0:dd/MM/yyyy }", rdr["ClosedDate"]);

                        if (rdr["ClosedDate"] == DBNull.Value)
                            Time2 = "";
                        else
                            Time2 = string.Format("{0:hh:mm:ss tt }", rdr["ClosedDate"]);


                        if (rdr["remark"] == DBNull.Value)
                            catatan = "";
                        else
                            catatan = rdr["remark"].ToString();

                        if (rdr["code"] == DBNull.Value)
                            kode = "";
                        else
                            kode = rdr["code"].ToString();


                        lst.Add(new TempObjectArray
                        {
                            id = Convert.ToInt32(rdr["id"]),
                            date1 = string.Format("{0:dd/MM/yyyy }", rdr["dIssued"]),
                            time1 = string.Format("{0:hh:mm:ss tt }", rdr["dIssued"]),
                            name1 = rdr["MachineName"].ToString(),
                            code1 = rdr["Issue"].ToString(),
                            code2 = aksi,
                            name2 = oleh,
                            date2 = tglttp,
                            time2 = Time2,
                            state = Convert.ToInt32(rdr["State"]),
                            desc = catatan,
                            codeForm = kode
                        });

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
                return lst;

            }
        }

        /*---------------UPLOAD DRAW MACHINE --------------*/
        public int CrudUploadDrawMachine( string nmfile, int id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NameFile", nmfile);
                    com.Parameters.AddWithValue("@IDProd", id);
                    com.Parameters.AddWithValue("@Action", "stsFILE");
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                   
                }
                return i;
            }
        }
        /*--------------END UPLOAD DRAW MACHINE --------------*/
       
        /*---------------DELETE DRAW MACHINE --------------*/
        public int deleteData(int idData)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDProd", idData);
                com.Parameters.AddWithValue("@Action", "delFileDraw");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }
        /*--------------END UPLOAD GENERATE --------------*/
        public int vGenerate(int ID1, int ID2)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_mPOR", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ID", ID1);
                com.Parameters.AddWithValue("@IDCTQ1", ID2);
                com.Parameters.AddWithValue("@Action", "GENERATE");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }

        /*----------------Draw Casting---------------------*/
        public List<General> vCastingImage(char code)
        {
            string nFile = "";
            List<General> lst = new List<General>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                if (code == 'C')
                    com.Parameters.AddWithValue("@Action", "CASTIMG");
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (code == 'C')
                    {
                        if (rdr["nameFile"] == DBNull.Value)
                            nFile = "";
                        else
                            nFile = rdr["nameFile"].ToString();

                        lst.Add(new General
                        {
                            Id = Convert.ToInt32(rdr["id"]),
                            FinishingCode = rdr["cGradeMaterial"].ToString(),
                            ProductName = rdr["cProductName"].ToString(),
                            GrossWeight = Convert.ToDouble(rdr["igrossweight"]),
                            measureName = nFile

                        });
                    }
                }
                con.Close();
                return lst;
            }
        }

        public int CrudUploadFileCasting(string nmfile, int id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NameFile", nmfile);
                    com.Parameters.AddWithValue("@IDProd", id);
                    com.Parameters.AddWithValue("@Action", "CASTUPLOADIMG");
                    i = com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
                return i;
            }
        }


        public int deleteDataCasting(int idData)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("Extranet_sp_General", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDProd", idData);
                com.Parameters.AddWithValue("@Action", "CASTDELIMG");
                i = com.ExecuteNonQuery();
                con.Close();
                return i;
            }
        }
    }

}