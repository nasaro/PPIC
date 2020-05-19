using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PPIC.Models;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace PPIC.Repository
{
    public class OrbRepository
    {
        public SqlConnection con, conPPIC;

        //To Handle connection related activities      
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["PPIC"].ToString();
            con = new SqlConnection(constr);

        }

        private void connectionPPIC()
        {
            string constrPPIC = ConfigurationManager.ConnectionStrings["DBPPIC"].ToString();
            conPPIC = new SqlConnection(constrPPIC);

        }
        //To view employee details with generic list       
        public List<ORB> GetAllOrb(int yyear)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Par1", yyear);
                param.Add("@Action", "ORB");
                connection();
                con.Open();
                IList<ORB> OrbList = SqlMapper.Query<ORB>(con, "Extranet_sp_ORB", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return OrbList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProgressOrb> GetAllDetailOrb(int custProd, string orbCode)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Par2", custProd);
                param.Add("@Par3", orbCode);
                param.Add("@Action", "ORBDetail");
                connection();
                con.Open();
                IList<ProgressOrb> OrbDetailList = SqlMapper.Query<ProgressOrb>(con, "Extranet_sp_ORB", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return OrbDetailList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProgressOrb> GetByIdProgress(int IdCode)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Par1", IdCode);
                param.Add("@Action", "ProgessID");
                connection();
                con.Open();
                IList<ProgressOrb> OrbDetailList = SqlMapper.Query<ProgressOrb>(con, "Extranet_sp_ORB", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return OrbDetailList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //GIFA
        public List<Gifa> GetAllGifa(int id, char pil)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", id);
                if (pil=='A')
                param.Add("@Action", "GifaAll");
                else
                param.Add("@Action", "GifaById");
                connectionPPIC();
                conPPIC.Open();
                IList<Gifa> gifaList = SqlMapper.Query<Gifa>(conPPIC, "sp_Tools", param, commandType: CommandType.StoredProcedure).ToList();
                conPPIC.Close();
                return gifaList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //FOH
        public List<FOH> GetAllFOH(int id, int periode, char pil)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", id);
                if (pil == 'A')
                {
                    param.Add("@Period", periode);
                    param.Add("@Action", "FOHAll");
                }
                else
                    param.Add("@Action", "FOHById");
                connectionPPIC();
                conPPIC.Open();
                IList<FOH> FOHList = SqlMapper.Query<FOH>(conPPIC, "sp_Tools", param, commandType: CommandType.StoredProcedure).ToList();
                conPPIC.Close();
                return FOHList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //PO Delivery Monitoring
        public List<POMonitor> GetAllPODelivery(int id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Par1", id);
                param.Add("@Action", "POMonitor");
                connection();
                con.Open();
                IList<POMonitor> monitorList = SqlMapper.Query<POMonitor>(con, "Extranet_sp_ORB", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return monitorList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SCHEDULE> GetAllSchedule(int id)
        {
            try
            {
                //DynamicParameters param = new DynamicParameters();
                //param.Add("@ID", id);
               //param.Add("@Action", "SCHEDULE");

                string strComSql = "select DATEADD(DAY, 1, t.dPOR) as dPOR,  DATEADD(DAY, 1+ROUND(isnull((CONVERT(DECIMAL, t.qty) * CONVERT(DECIMAL, c.tacktime) * CONVERT(DECIMAL, c.handlingtime) / CONVERT(DECIMAL, 1170)), 0), 0), t.dPOR) as dEND,  (convert(varchar, t.qty) + ',' + b.cname + ',' + t.remark + ',' + m.machineName) as gab from tPOR t inner "
                    + "join mMachine m on t.idmachine = m.machineID "
                    + "inner join (select idCtq1, tacktime, handlingtime from tCtq1 ) c on c.idCtq1 = t.idCtq1 "
                    + "inner join (SELECT cProductionOrderRequestCode as code, cProductName as cname FROM ProductionOrderRequest, mcustomerProduct WHERE ProductionOrderRequest.cFinishCondition = 'MACHINING' "
                    + "AND ProductionOrderRequest.icustomerProductID = mcustomerProduct.id ) b on b.code = t.por_#  where year(t.dPOR) = " + id;
                connection();
                con.Open();
                IList<SCHEDULE> scheduleList = SqlMapper.Query<SCHEDULE>(con, strComSql, commandType: CommandType.Text).ToList();
                con.Close();
                return scheduleList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Barcode> GetAllBarcode(int id)
        {

            try
            {
                string strComSql = "select a.cProductionOrderRequestCode+'_'+sectionid as barcode, Stuff(a.cProductionOrderRequestCode, Len(a.cProductionOrderRequestCode)-7, 0, '>5')+'>6_'+sectionid as printcode, "
                + "cproductname,iqtyPOR as qty, "
                + "case sectionid when 'C01' then 'WAX' when 'C02' then 'ASSEMBLY' when 'C03' then 'LOSTWAX' else 'POURING' end as dept from "
                + "(select 'link' as con,cProductionOrderRequestCode, dPORDate,icustomerproductID,iQtyPOR from productionorderrequest) a "
                + "left join (select distinct sectionid,'link' as con from tshopfloor) b on a.con=b.con inner join "
                + "(select id,cproductname from mcustomerProduct)c on a.icustomerproductid  = c.id "
                + "left join ( select a.cproductionorderrequestcode,a.iqtyPOR - isnull(b.qtyPROD,0) + isnull(c.qtyNC,0) as qtySisa from "
                + "( select  cproductionorderrequestcode,iqtyPOR from productionorderrequest ) a left join "
                + "( select cPORCODE,SectionID,sum(iqty) as qtyPROD from tshopfloor where sectionID = 'C01' group by cPORCODE,SectionID ) b on a.cproductionorderrequestcode = b.cPORCODE "
                + "left join ( select  cPORCODE,sum(iqty) as qtyNC from tNCR2_PRODUCTION where  left(cNCRCOde,3) = 'NCA' group by  cPORCODE ) c on a.cproductionorderrequestcode = c.cPORCODE "
                + ") d on a.cproductionorderrequestcode = d.cproductionorderrequestcode "
                + " where d.qtysisa <>0 and c.id = " + id;
                connection();
                con.Open();
                IList<Barcode> BarcodeList = SqlMapper.Query<Barcode>(con, strComSql, commandType: CommandType.Text).ToList();
                con.Close();
                return BarcodeList.ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ppicsubcon> GetAllPpicSubCon(int thn, int bln)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Par1", bln);
                param.Add("@Par2", thn);
                param.Add("@Action", "ppicsubcon");
                connection();
                con.Open();
                IList<ppicsubcon> ppicsubconList = SqlMapper.Query<ppicsubcon>(con, "Extranet_sp_ORB", param, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
                return ppicsubconList.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}