/* 
 * Purpose:         Leads Assignment Management Webservices data access layer
 * Created By:      Yin Mon Win
 * Date:            12/09/2011
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 *
 * 
 */


using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SPMWebServiceApp.DataAccess
{
    public class LeadsAssignmentDA
    {
        private string sql;
        private GenericDA genericDA;

        public LeadsAssignmentDA(GenericDA da)
        {
            genericDA = da;
        }

        
        public DataSet RetrieveLeadsForAssignment(string teamCode)
        {
            DataSet ds = null;
            string andClause = " ", orderClause = " ORDER BY LeadName ";

            //Changes for Sorting
            //CONVERT(VARCHAR, CLM.LCRDATE, 103) AS ACOpen
            //CONVERT(VARCHAR, TCA.LastTradeDate, 103) AS LastTradeDate
            //CONVERT(VARCHAR, CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate, CONVERT(VARCHAR, CLA.CutOffDate, 120) AS LastCutOffDate
            //CONVERT(VARCHAR, CCT.LastCallDate, 120) AS LastCallDate

          sql = "SELECT  TCA.LeadId,TCA.LeadName, TCA.AEGroup AS TeamCode, TCA.LeadNRIC, TCA.LeadMobile, TCA.LeadHomeNo, " +
                " CASE WHEN LeadGender  = 'F' THEN 'Female' ELSE 'Male' END AS LeadGender, Event, PreferMode, LeadEmail, " +
                " CLA.AECode AS LastAssignDealer, " +
                " CONVERT(VARCHAR, CLA.AssignDate, 103) AS LastAssignDate, CLA.CutOffDate, CCT.LastCallDate, " +
                " CCT.ModifiedUser AS LastCallDealer, " +
                //" ISNULL(CT.TotalCall, 0) as TotalCall,CC.AECode AS CoreAECode, 
                " 't' AS AssignmentType, 's' AS AssignmentStatus, CLA.AECode AS AssignDealer, CLA.AssignDate, " +
                " CLA.CutOffDate AS LastCutOffDate , " +
                " CLA.ModifiedUser, CLA.ModifiedDate " +       //New added for AuditLog
                "FROM LeadDetail TCA " +
           
                "LEFT JOIN	" +         // JOIN for retrieving MAX Assingment Record
                "(" +
                "    SELECT DISTINCT CA.AECode, CA.LeadId, MDT.MDate AS AssignDate, MDT.CutOffDate AS CutOffDate " +
                "    , CA.ModifiedUser, CA.ModifiedDate " +                 //New added for AuditLog
                "    FROM LeadAssign CA WITH (NOLOCK) " +
                "    INNER JOIN " +
                "    ( " +
                "        SELECT LeadId, MAX(AssignDate) AS MDate, MAX(CutOffDate) AS CutOffDate " +
                "        FROM LeadAssign WITH (NOLOCK) GROUP BY LeadId " +
                "    ) MDT ON CA.LeadId = MDT.LeadId AND CA.AssignDate = MDT.MDate " +
                ") CLA ON TCA.LeadId = CLA.LeadId " +  /// <Updated by OC> AND TCA.AECode = CLA.AECode

                "LEFT JOIN " +	        // JOIN for retrieving MAX Contact Record
                "(" +
                "    SELECT CCT.LeadID, PreferMode, ModifiedUser, Max(ContactDate) AS LastCallDate " +
                "    FROM LeadContact CCT WITH (NOLOCK) " +
                "    LEFT JOIN " +
                "    ( " +
                "        SELECT LeadID, Max(ContactDate) AS MDate " +
                "        FROM LeadContact WITH (NOLOCK) GROUP BY LeadId " +
                "    ) MDT ON CCT.LeadId=MDT.LeadId " +
                "    WHERE CCT.ContactDate=MDT.MDate " +
                "    GROUP BY CCT.LeadId, PreferMode, ModifiedUser " +
                ") " +
                "CCT ON TCA.LeadId = CCT.LeadId " +
               // "LEFT JOIN ClientTotal CT ON TCA.AcctNo = CT.AcctNo " +
               // "LEFT JOIN CoreClient CC ON TCA.AcctNo = CC.AcctNo " +
               // "WHERE CLM.LCRDATE BETWEEN '" + accountFromDate + "' AND '" + accountToDate + "' " +
                " WHERE TCA.AEGroup IN (" + teamCode + ")";
  
              sql = sql + andClause + orderClause;

            //Retrive without setting Row No from Program Code
            ds = genericDA.ExecuteQuery(sql);


            //To set Row No from Program Code
            //SqlCommand cmd = genericDA.GetSqlCommand();
            //SqlDataReader dr = cmd.EndExecuteReader();

            return ds;
        }


        protected string getNewID(string prevID)
        {
            int lenNumbers = 6;
            string prefix = "P";
            string num = "";

            //prevID = cmc.GetMaxID(0);
            if (prevID == "0")
            {
                num = "0";
            }
            else
            {
                num = prevID.Substring(prevID.Length - lenNumbers);
            }

            num = padZeros(Convert.ToString(Convert.ToInt32(num) + 1), lenNumbers);
            string newID = prefix + num;

            return newID;
        }

        //filling with zero
        public static string padZeros(string stringToPad, Int32 desiredLength)
        {
            string pZ = stringToPad;
            if (stringToPad.Length < desiredLength)
            {
                for (int i = stringToPad.Length + 1; i <= desiredLength; i++)
                    pZ = "0" + pZ;
            }
            return pZ;
        }

        public string SaveProjectInfo(string ProjectName, DateTime  CutOffDate)
        {
            string insertedIdStr = "";
            string generateID ="" ;
            string result = "";
            OleDbCommand cmd = null;
            

            try
            {
                sql = " SELECT MAX(ProjectID) from ProjectDetail";
                cmd = genericDA.GetSqlCommand();
                cmd.CommandText = sql;
                insertedIdStr = cmd.ExecuteScalar().ToString();
                if (!String.IsNullOrEmpty(insertedIdStr))
                {
                    generateID = getNewID(insertedIdStr);
                }
                else
                {
                    generateID = getNewID("0");
                }

                sql = " INSERT INTO ProjectDetail(ProjectID,ProjectName, ProjectObjective, ProjectType,AssignDate,CutoffDate,FilePath) "
                        + " VALUES('" + generateID + "', '" + ProjectName + "', '','L',GETDATE(),'" + CutOffDate.ToString("yyyy-MM-dd hh:mm:ss.000") + "','')";
                        //+ ");SELECT SCOPE_IDENTITY();";

                cmd = genericDA.GetSqlCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                result = generateID;
            }
            catch
            {
                return result;
            }
            return result;
        }

        public DataTable RetrieveAllProjectInfo()
        {
            sql = "  SELECT ProjectID,ProjectName,ProjectObjective,ProjectType,FilePath FROM [SPM].[dbo].[ProjectDetail]";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric, 
                            string fromDate, string toDate, bool retradeFlag)
        {
            //Testing GridView DateFormatString
            //CONVERT(VARCHAR, CLM.LCRDATE, 103) AS AcctCreateDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate
            //CONVERT(VARCHAR, CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate
            //CONVERT(VARCHAR, TAssignLTD.LTradeDate, 103) AS AssignLTD
            //CONVERT(VARCHAR, LTD.OverallLTD, 103) AS CurrentLTD

            //Changes 27 April 2010 => change retrieval of client team from TmpClientAssign to CLMAST
            //TCA.AECode as TeamCode

            bool whereFlag = false;

            sql = "SELECT CLA.AcctNo, CLA.AECode AS Dealer, CLA.AssignDate AS AssignDate, " +
                    "CLM.LCRDATE AS AcctCreateDate, CLM.NRIC, CLM.LNAME AS ClientName, " +
                    "CLM.LRNER as TeamCode, TAssignLTD.CTradeDate AS AssignLTD, " +
                    "LTD.OverallLTD AS CurrentLTD, MCC.LastCallDate, " +
                    "CLA.CutOffDate AS CutOffDate, " +
                    "DATEDIFF(mm, TAssignLTD.CTradeDate, LTD.OverallLTD) AS MonthDiff, " +
                    "ISNULL(TCA.TotalComm, 0) AS TotalComm " +
                    "FROM ClientAssign CLA " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON CLA.AcctNo = CLM.LACCT " +
                    "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCA.AcctNo = CLA.AcctNO " +
                    "LEFT JOIN " +
                    "(	" +
                    "    SELECT DISTINCT CLTD.AcctNo, MinCDate, LTradeDate " +              //LTradeDate is to use in WHERE clause to get Retrade Client
                    "       , CTradeDate = CASE CONVERT(VARCHAR(10), LTradeDate, 103) " +   //CTradeDate is to calculate NULL value for MonthDiff
				    "       WHEN '01/01/1900' THEN NULL ELSE LTradeDate END " +
                    "    FROM SPM..ClientLTradeDate CLTD WITH (NOLOCK) " +
                    "    LEFT JOIN " +
                    "    (	" +
                    "        SELECT DISTINCT AcctNo, MIN(CreateDate) AS MinCDate " +
                    "        FROM SPM.dbo.ClientLTradeDate " +
                    // --WHERE CreateDate BETWEEN '" & pDate & "' AND '" & pDate2 & "' "
                    "        GROUP BY AcctNo " +
                    "    ) TMDate ON CLTD.AcctNo=TMDate.AcctNo " +
                    "    WHERE CreateDate=MinCDate " +
                    ") TAssignLTD ON CLA.AcctNo=TAssignLTD.AcctNo " +
                    "LEFT JOIN ClientLTD LTD WITH (NOLOCK) ON CLA.AcctNo=LTD.AcctNo " + //-- AND CA.AECode='" & pAECode AND CA.AcctNo='" & pAcctNo & "' "
                    "INNER JOIN " +
                    "( " +
	                "    SELECT cla.AECode, cla.AcctNo, cla.AssignDate, cla.CutOffDate, MAX(cc.ContactDate) as LastCallDate " +
	                "    FROM ClientAssign cla " +
	                "    LEFT JOIN " +
	                "    ( " +
		            "        SELECT AcctNo, ContactDate " +
		            "        FROM ClientContact   ";

                    if (!String.IsNullOrEmpty(accountNo))
                    {
                         sql = sql + " WHERE AcctNo = '" + accountNo + "' ";        //--WHERE AcctNo = '0520259'
                    }
		                    
	                 sql = sql + " ) cc ON cc.AcctNo = cla.AcctNo AND cc.ContactDate BETWEEN cla.AssignDate AND cla.CutOffDate ";
	                    
                            //--WHERE cla.AcctNo = '0520259' AND AECode = 'TA_CQ'
                    if (!String.IsNullOrEmpty(accountNo))
                    {
                        sql = sql + " WHERE cla.AcctNo = '" + accountNo + "' ";
                        whereFlag = true;
                    }        

                    if (!String.IsNullOrEmpty(dealerCode))
                    {
                        if(whereFlag)
                        {
                            sql = sql + " AND AECode = '" + dealerCode + "' ";
                        }
                        else
                        {
                            sql = sql + " WHERE AECode = '" + dealerCode + "' ";
                        }
                    }

                    sql = sql + " GROUP BY cla.AECode, cla.AcctNo, cla.AssignDate, cla.CutOffDate " +
                                    " ) MCC ON CLA.AcctNo = MCC.AcctNo AND CLA.AssignDate = MCC.AssignDate ";


            StringBuilder sb = new StringBuilder(sql);
            whereFlag = false;

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE CLA.AECode = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }
            
            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.AcctNo = '").Append(accountNo).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.AcctNo = '").Append(accountNo).Append("' ");
                }                                
            }

            if (!String.IsNullOrEmpty(nric))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLM.NRIC = '").Append(nric).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLM.NRIC = '").Append(nric).Append("' ");
                }
            }

            if ((!String.IsNullOrEmpty(fromDate)) && (!String.IsNullOrEmpty(toDate)))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.AssignDate BETWEEN '").Append(fromDate).Append("' AND '").Append(toDate).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CLA.AssignDate BETWEEN '").Append(fromDate).Append("' AND '").Append(toDate).Append("' ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(fromDate))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.AssignDate >= '").Append(fromDate).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.AssignDate >= '").Append(fromDate).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(toDate))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.AssignDate <= '").Append(toDate).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.AssignDate <= '").Append(toDate).Append("' ");
                }
            }

            //New added for retrieving LastTradeDate only
            if (retradeFlag)
            {
                //sb.Append(" AND TCA.LastTradeDate > TAssignLTD.LTradeDate ");
                sb.Append(" AND DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) >= 0 ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable RetrieveAssignedLeadsInfo(string teamCode, string dealerCode, string accountNo, string assignDate)
        {
            //Removed Column for Excel Report
            //GroupName, AssignDate

            sql = "SELECT DISTINCT CA.AECode AS Dealer, LRNER AS Team, CA.LeadId, ISNULL(LeadName, '') AS LeadName, CLTD.OverallLTD AS LTD, " +
                    "LMOBILE AS Mobile, ISNULL(LEMAIL, '') AS Email, ISNULL((LADDR1+' ' +LADDR2+' '+LADDR3), '') AS Address " +
                    //", ISNULL(LADDR1, '') AS LADDR1, ISNULL(LADDR2, '') AS LADDR2, ISNULL(LADDR3, '') AS LADDR3 " +
                    "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
                    "LEFT JOIN SPM.dbo.ClientLTD CLTD WITH (NOLOCK) ON CA.AcctNo=CLTD.AcctNo " +
                    "LEFT JOIN  " +
                    "( " +
                    "    SELECT GroupName, AECodeGp FROM SPM..DealerIDTable WITH (NOLOCK) WHERE GroupType='Dealer'  " +
                    "    AND RefMonthYr = (SELECT LEFT(Max(LastUpdate), 6) AS MaxDay FROM SPM.dbo.LastUpdateTable WITH (NOLOCK)) " +
                    ") " +
                    "DID ON CA.AECode=DID.AECodeGp  " +
                    "LEFT JOIN SPM.dbo.CLMAST CL WITH (NOLOCK) ON CA.AcctNo=CL.LACCT  " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = CA.AECode " +      //To serach with Team
                    "WHERE CA.AssignDate = '" + assignDate + "' ";

            StringBuilder sb = new StringBuilder(sql);

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" AND CA.AECode = '").Append(dealerCode).Append("' ");
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                sb.Append(" AND CA.AcctNo = '").Append(accountNo).Append("' ");
            }

            if (!String.IsNullOrEmpty(teamCode))
            {
                //sb.Append(" AND GroupName = '").Append(teamCode).Append("' ");        //Use in ASP Program
                sb.Append(" AND DD.AEGroup = '").Append(teamCode).Append("' ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public int InsertLeadsAssignment(DataSet ds)
        {
            int result = 1;


            // Keep in mind the sequence in which you are adding the parameters should be same as the sequence they appear in the query
            OleDbParameter[] sqlParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@AECode", OleDbType.VarChar), 
                                            new OleDbParameter("@LeadId", OleDbType.VarChar),
                                            new OleDbParameter("@AssignDate", OleDbType.Date),
                                            new OleDbParameter("@CutOffDate", OleDbType.Date),
                                            new OleDbParameter("@ModifiedUser", OleDbType.VarChar),
                                            new OleDbParameter("@ProjectID",OleDbType.VarChar)
                                        };

            // Keep in mind the sequence in which you are adding the parameters should be same as the sequence they appear in the query
            OleDbParameter[] sqlUpdateParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@AECode", OleDbType.VarChar),                                             
                                            new OleDbParameter("@CutOffDate", OleDbType.Date),
                                            new OleDbParameter("@ModifiedUser", OleDbType.VarChar),                                            
                                            new OleDbParameter("@LastAssignDealer", OleDbType.VarChar),
                                            new OleDbParameter("@LeadId", OleDbType.VarChar),
                                            new OleDbParameter("@AssignDate", OleDbType.Date) 
                                           // new OleDbParameter("@ProjectID",OleDbType.VarChar)                                     
                                        };

            //Parameters for AuditLog
            OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                                            new OleDbParameter("@istrLeadId", OleDbType.VarChar),
                                            new OleDbParameter("@idtAssignDate", OleDbType.Date),
                                            new OleDbParameter("@idtCutOffDate", OleDbType.Date),
                                            new OleDbParameter("@istrModifiedUser", OleDbType.VarChar),                                            
                                            new OleDbParameter("@idtModifiedDate", OleDbType.Date),                                           
                                            new OleDbParameter("@istrActionCode", OleDbType.Char),
                                            new OleDbParameter("@istrActionBy", OleDbType.VarChar)
                                        };


            OleDbCommand cmd = null, cmdUpdate = null, cmdAuditLog = null;
            OleDbTransaction sqlTransaction = null;

            //string auditLogSql = "INSERT INTO ClientAssignAudit(OldAECode, OldAcctNo, OldAssignDate, OldCutOffDate, OldModifiedUser, OldModifiedDate," +
            //             "NewAECode, NewCutOffDate, Action, ActionBy, ActionDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, CONVERT(VARCHAR, GetDate(), 120))";

            string auditLogSql = "SPM_spLeadAssignmentAuditIns";

            //Parameterized Query for OLEDB Parameters
            string updateSql = "UPDATE LeadAssign SET AECode = ?, " +
                " CutOffDate = ?, ModifiedUser = ?, ModifiedDate = CONVERT(VARCHAR, GetDate(), 120) " +
                " WHERE AECode = ? AND LeadId = ? AND AssignDate = ? ";


            sql = "INSERT INTO LeadAssign(AECode, LeadId, AssignDate, CutOffDate, ModifiedUser, ModifiedDate,ProjectID) " +
                        "VALUES (?, ?, ?, ?, ?, CONVERT(VARCHAR, GetDate(), 120),?)";
            
            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

            
            //Create Insert command
            cmd = genericDA.GetSqlCommand();
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(sqlParams);            

            //Create Update command
            cmdUpdate = genericDA.GetNewSqlCommand();
            cmdUpdate.Transaction = sqlTransaction;
            cmdUpdate.CommandText = updateSql;
            cmdUpdate.Parameters.AddRange(sqlUpdateParams);

            //Create AuditLog command
            cmdAuditLog = genericDA.GetNewSqlCommand();            
            cmdAuditLog.Transaction = sqlTransaction;
            cmdAuditLog.CommandType = CommandType.StoredProcedure;
            cmdAuditLog.CommandText = auditLogSql;
            cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);

            DataTable dtClientAssign = ds.Tables[0];

            try
            {
                for (int i = 0; i < dtClientAssign.Rows.Count; i++)
                {
                    if (dtClientAssign.Rows[i]["AssignmentStatus"].ToString().Equals("NEW"))
                    {
                        cmd.Parameters["@AECode"].Value = dtClientAssign.Rows[i]["AssignDealer"].ToString();
                        cmd.Parameters["@LeadId"].Value = dtClientAssign.Rows[i]["LeadId"].ToString();

                        cmd.Parameters["@AssignDate"].Value = dtClientAssign.Rows[i]["AssignDate"].ToString();
                        //Correct Date Format
                        //cmd.Parameters["@AssignDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");                        

                        //cmd.Parameters["@CutOffDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["CutOffDate"].ToString(), "dd/MM/yyyy", null);
                        cmd.Parameters["@CutOffDate"].Value = dtClientAssign.Rows[i]["CutOffDate"].ToString();
                        cmd.Parameters["@ModifiedUser"].Value = dtClientAssign.Rows[i]["ModifiedUser"].ToString();
                        cmd.Parameters["@ProjectID"].Value = dtClientAssign.Rows[i]["ProjectID"].ToString();

                        result = cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmdUpdate.Parameters["@AECode"].Value = dtClientAssign.Rows[i]["AssignDealer"].ToString();
                        cmdUpdate.Parameters["@LastAssignDealer"].Value = dtClientAssign.Rows[i]["LastAssignDealer"].ToString();
                        cmdUpdate.Parameters["@LeadId"].Value = dtClientAssign.Rows[i]["LeadId"].ToString();

                        cmdUpdate.Parameters["@AssignDate"].Value = dtClientAssign.Rows[i]["AssignDate"].ToString();
                        //Correct Date Format
                        //cmdUpdate.Parameters["@AssignDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");


                        //cmd.Parameters["@CutOffDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["CutOffDate"].ToString(), "dd/MM/yyyy", null);
                        cmdUpdate.Parameters["@CutOffDate"].Value = dtClientAssign.Rows[i]["CutOffDate"].ToString();
                        cmdUpdate.Parameters["@ModifiedUser"].Value = dtClientAssign.Rows[i]["ModifiedUser"].ToString();
                        //cmdUpdate.Parameters["@ProjectID"].Value = dtClientAssign.Rows[i]["ProjectID"].ToString();

                        result = cmdUpdate.ExecuteNonQuery();

                        if (result > 0)
                        {
                            cmdAuditLog.Parameters["@istrAECode"].Value = dtClientAssign.Rows[i]["LastAssignDealer"].ToString();
                            cmdAuditLog.Parameters["@istrLeadId"].Value = dtClientAssign.Rows[i]["LeadId"].ToString();
                            cmdAuditLog.Parameters["@idtAssignDate"].Value = dtClientAssign.Rows[i]["AssignDate"].ToString();
                            cmdAuditLog.Parameters["@idtCutOffDate"].Value = dtClientAssign.Rows[i]["OldCutOffDate"].ToString();
                            cmdAuditLog.Parameters["@istrModifiedUser"].Value = dtClientAssign.Rows[i]["OldModifiedUser"].ToString();
                            cmdAuditLog.Parameters["@idtModifiedDate"].Value = dtClientAssign.Rows[i]["OldModifiedDate"].ToString();

                            //cmdAuditLog.Parameters["@NewAECode"].Value = dtClientAssign.Rows[i]["AssignDealer"].ToString();
                            //cmdAuditLog.Parameters["@NewCutOffDate"].Value = dtClientAssign.Rows[i]["CutOffDate"].ToString();

                            cmdAuditLog.Parameters["@istrActionCode"].Value = "U";
                            cmdAuditLog.Parameters["@istrActionBy"].Value = dtClientAssign.Rows[i]["ModifiedUser"].ToString();

                            cmdAuditLog.ExecuteNonQuery();
                        }
                        //else 
                        //{
                        //    sql = "DELETE FROM ProjectDetail WHERE ProjectID = '" +  + "' ";

                        //    result = genericDA.ExecuteNonQuery(sql);
                        //}
                    }
                }

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                //sql = "DELETE FROM ProjectDetail WHERE ProjectID = '" + +"' ";
                //result = genericDA.ExecuteNonQuery(sql);
                sqlTransaction.Rollback();
                result = -1;

                
            }
            finally
            {
                try
                {
                    cmdUpdate.Dispose();
                    cmdAuditLog.Dispose();
                }
                catch (Exception ex) { }
            }
            
            return result;
        }

        public DataTable BatchDeleteLeadsAssignment(DataTable dtAssignDelete)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("DataRowIndex", String.Empty.GetType());
            dtResult.Columns.Add("dealerCode", String.Empty.GetType());
            dtResult.Columns.Add("leadId", String.Empty.GetType());
            dtResult.Columns.Add("assignDate", String.Empty.GetType());
            dtResult.Columns.Add("result", String.Empty.GetType());

            int result = -1;

            for (int i = 0; i < dtAssignDelete.Rows.Count; i++)
            {
                string dataRowIndex = "";
                string dealerCode = "";
                string leadId = "";
                string assignDate = "";
                string cutOffDate = "";
                string modifiedUser = "";
                string modifiedDate = "";
                string newModifiedUser = "";
                DataRow drResult = null;

                dataRowIndex = dtAssignDelete.Rows[i]["DataRowIndex"].ToString();
                dealerCode = dtAssignDelete.Rows[i]["dealerCode"].ToString();
                leadId = dtAssignDelete.Rows[i]["leadId"].ToString();
                assignDate = dtAssignDelete.Rows[i]["assignDate"].ToString();
                cutOffDate = dtAssignDelete.Rows[i]["cutOffDate"].ToString();
                modifiedUser = dtAssignDelete.Rows[i]["modifiedUser"].ToString();
                modifiedDate = dtAssignDelete.Rows[i]["modifiedDate"].ToString();
                newModifiedUser = dtAssignDelete.Rows[i]["newModifiedUser"].ToString();

                sql = "DELETE FROM LeadAssign WHERE AECode = '" + dealerCode + "' AND LeadId = '" + leadId
                        + "' AND AssignDate = '" + assignDate + "'";

                result = genericDA.ExecuteNonQuery(sql);
                if (result > 0)
                {

                    string auditLogSql = "SPM_spLeadAssignmentAuditIns";
                    OleDbCommand cmdAuditLog = genericDA.GetSqlCommand();

                    //Parameters for AuditLog
                    OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                                            new OleDbParameter("@istrLeadId", OleDbType.VarChar),
                                            new OleDbParameter("@idtAssignDate", OleDbType.Date),
                                            new OleDbParameter("@idtCutOffDate", OleDbType.Date),
                                            new OleDbParameter("@istrModifiedUser", OleDbType.VarChar),                                            
                                            new OleDbParameter("@idtModifiedDate", OleDbType.Date),                                           
                                            new OleDbParameter("@istrActionCode", OleDbType.Char),
                                            new OleDbParameter("@istrActionBy", OleDbType.VarChar)
                                        };

                    //Create AuditLog command
                    cmdAuditLog = genericDA.GetNewSqlCommand();
                    cmdAuditLog.CommandType = CommandType.StoredProcedure;
                    cmdAuditLog.CommandText = auditLogSql;
                    cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);


                    cmdAuditLog.Parameters["@istrAECode"].Value = dealerCode;
                    cmdAuditLog.Parameters["@istrLeadId"].Value = leadId;
                    cmdAuditLog.Parameters["@idtAssignDate"].Value = assignDate;
                    cmdAuditLog.Parameters["@idtCutOffDate"].Value = cutOffDate;
                    cmdAuditLog.Parameters["@istrModifiedUser"].Value = modifiedUser;
                    cmdAuditLog.Parameters["@idtModifiedDate"].Value = modifiedDate;

                    cmdAuditLog.Parameters["@istrActionCode"].Value = "D";
                    cmdAuditLog.Parameters["@istrActionBy"].Value = newModifiedUser;

                    cmdAuditLog.ExecuteNonQuery();

                    cmdAuditLog.Dispose();
                }

                drResult = dtResult.NewRow();
                drResult["DataRowIndex"] = dataRowIndex;
                drResult["dealerCode"] = dealerCode;
                drResult["leadId"] = leadId;
                drResult["assignDate"] = assignDate;
                drResult["result"] = result;
                dtResult.Rows.Add(drResult);
            }
            return dtResult;
        }

        public int DeleteLeadsAssignment(string dealerCode, string LeadId, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser)
        {
            int result = -1;

            //WHERE AECode = 'T11_PIC2' AND AcctNo = '0336021' AND AssignDate = '2010-03-08'
            sql = "DELETE FROM LeadAssign WHERE AECode = '" + dealerCode + "' AND LeadId = '" + LeadId
                    + "' AND AssignDate = '" + assignDate + "'";

            result = genericDA.ExecuteNonQuery(sql);
            if (result > 0)
            {

                ////string auditLogSql = "INSERT INTO ClientAssignAudit(OldAECode, OldAcctNo, OldAssignDate, OldCutOffDate, OldModifiedUser, OldModifiedDate," +
                ////        "Action, ActionBy, ActionDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, CONVERT(VARCHAR, GetDate(), 120))";


                //string auditLogSql = "SPM_spLeadAssignmentAuditIns";


                string auditLogSql = "INSERT INTO LeadAssignAudit " +
                                    "(AECode, LeadId, AssignDate, CutOffDate, ModifiedUser, ModifiedDate,ActionCode, ActionBy, ActionDate)" +
                                    "VALUES ('" + dealerCode + "', '" + LeadId + "', '" + assignDate + "', '" + cutOffDate + "', " +
                                    "'" + modifiedUser + "', '" + modifiedDate + "', 'D', '" + newModifiedUser + "',CONVERT(VARCHAR, GetDate(), 120))";
                OleDbCommand cmdAuditLog = genericDA.GetSqlCommand();
                //Parameters for AuditLog
                //OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                //                        { 
                //                            new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                //                            new OleDbParameter("@istrLeadId", OleDbType.VarChar),
                //                            new OleDbParameter("@idtAssignDate", OleDbType.Date),
                //                            new OleDbParameter("@idtCutOffDate", OleDbType.Date),
                //                            new OleDbParameter("@istrModifiedUser", OleDbType.VarChar),                                            
                //                            new OleDbParameter("@idtModifiedDate", OleDbType.Date),                                           
                //                            new OleDbParameter("@istrActionCode", OleDbType.Char),
                //                            new OleDbParameter("@istrActionBy", OleDbType.VarChar)
                //                        };

                //////Create AuditLog command
                cmdAuditLog = genericDA.GetNewSqlCommand();
                cmdAuditLog.CommandType = CommandType.Text;
                cmdAuditLog.CommandText = auditLogSql;
                //cmdAuditLog.Parameters.AddRange(sqlAuditLogParams);


                //cmdAuditLog.Parameters["@istrAECode"].Value = dealerCode;
                //cmdAuditLog.Parameters["@istrLeadId"].Value = LeadId;
                //cmdAuditLog.Parameters["@idtAssignDate"].Value = assignDate;
                //cmdAuditLog.Parameters["@idtCutOffDate"].Value = cutOffDate;
                //cmdAuditLog.Parameters["@istrModifiedUser"].Value = modifiedUser;
                //cmdAuditLog.Parameters["@idtModifiedDate"].Value = modifiedDate;

                //cmdAuditLog.Parameters["@istrActionCode"].Value = "D";
                //cmdAuditLog.Parameters["@istrActionBy"].Value = newModifiedUser;

                cmdAuditLog.ExecuteNonQuery();

                cmdAuditLog.Dispose();
            }

            return result;
        }


        
        public int DeleteLeadsProjects(string ProjectID)
        {
            int result = -1;

            sql = "DELETE FROM ProjectDetail WHERE ProjectID = '" + ProjectID  + "' ";

            result = genericDA.ExecuteNonQuery(sql);
          
            return result;
        }
        
        public DataTable RetrieveCrossEnabledTeam()
        {
            sql = "SELECT CRT.TeamCode, CRT.TeamCode + ' - ' + AEL.AE_Name AS TeamName " +
                  "FROM AEList AEL " +
                  "INNER JOIN " +
                  "( " +
                  "      SELECT RTRIM(LTRIM(CA.AEGroup)) AS TeamCode " +
                  "      FROM CommAECode CA WITH (NOLOCK) " +
                  "      LEFT JOIN DealerDetail DD WITH (NOLOCK) ON CA.AECode = DD.AECode " +
                  "      WHERE DD.Enable='1' AND DD.CrossGroup = 'Y' AND ISNULL(CA.AEGroup, '') <> '' " +
                  "      GROUP BY CA.AEGroup " +
                  ") CRT ON AEL.GroupName = CRT.TeamCode " +
                  "GROUP BY CRT.TeamCode, AEL.AE_Name";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveCrossEnabledDealer(string crossTeamCode)
        {
            sql = "SELECT DISTINCT CAC.AECode, ISNULL(DD.AEName, '-') AS AEName, ISNULL(TotAss, 0) AS CurrentAssign " +
                        "FROM CommAECode CAC WITH (NOLOCK) " +
                        "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON CAC.AECode=DD.AECode " +
                        "LEFT JOIN " +
                        "( " +
                        "    SELECT AECode, COUNT(AECode) AS TotAss FROM LeadAssign WITH (NOLOCK) " +
                        "    WHERE CutOffDate > GETDATE() " +
                        "    GROUP BY AECode " +
                        ") " +
                        "CAT ON DD.AECode=CAT.AECode " +
                        "WHERE DD.AECode<>'CB910' AND Enable='1' AND CrossGroup = 'Y' " +
                        "AND CAC.AEGroup = '" + crossTeamCode + "'" +
                        "ORDER BY CAC.AECode ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsAssignmentForExtraCall(string leadId, string dealerCode, string contactDate)
        {
            sql = "SELECT CLA.AssignDate, CLA.AcctNo, CLA.AECode " +
                        "FROM LeadAssign CLA " +
                        "WHERE CLA.LeadId = '" + leadId + "' "  +
                        "AND CLA.AECode = '" + dealerCode + "' " +
                        "AND '" + contactDate + "' BETWEEN CLA.AssignDate AND CLA.CutOffDate";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsAssignmentDateByDateRange(string fromDate, string toDate)
        {
            sql = "SELECT CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDateStr " +
                    "FROM LeadAssign " +
                    "WHERE AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + "' " +
                    "GROUP BY AssignDate " +
                    "ORDER BY AssignDate DESC ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataSet RetrieveLeadsAssignedProjectByUserId(String userId)
        {
            sql = "SELECT totalassigned.projectid AS projectid, " +
                       "totalassigned.projectname AS projectname, " +
                       "totalassigned.assigndate AS assigneddate, " +
                       "totalassigned.cutoffdate AS cutoffdate, " +
                       "( totalassigned.assignedclient - Isnull(totalcontact.totalcontacted, 0) ) AS callsleft, " +
                       "Isnull(totalcallstofollowup.nooffollowup, 0) AS callstofollowup, " +
                       "totalcallstofollowup.followupdate AS followupdate " +
                "FROM   (SELECT p.projectid, " +
                               "p.projectname, " +
                               "p.assigndate, " +
                               "p.cutoffdate, " +
                               "d.aecode, " +
                               "COUNT(p.projectid) AS assignedclient " +
                        "FROM   LeadAssign la " +
                               "INNER JOIN projectdetail p " +
                                 "ON la.projectid = p.projectid " +
                               "INNER JOIN dealerdetail d " +
                                 "ON la.aecode = d.aecode " +
                        "WHERE  d.userid = '" + userId + "' " +
                               "AND p.projecttype = 'L' " +
                               "AND la.CutoffDate >= GETDATE() " +
                        "GROUP  BY p.projectid, " +
                                  "p.projectname, " +
                                  "p.assigndate, " +
                                  "p.cutoffdate, " +
                                  "d.aecode) AS totalassigned " +
                       "LEFT JOIN (SELECT lc.projectid, " +
                                          "COUNT(lc.projectid) AS totalcontacted " +
                                   "FROM   LeadContact lc " +
                                          "INNER JOIN LeadAssign la " +
                                            "ON lc.projectid = la.projectid " +
                                               "AND lc.LeadID = la.LeadID " +
                                               "AND la.CutoffDate >= GETDATE() " +
                                          "INNER JOIN projectdetail p " +
                                            "ON p.projectid = lc.projectid " +
                                          "INNER JOIN dealerdetail d " +
                                            "ON d.aecode = la.aecode " +
                                   "WHERE  d.userid = '" + userId + "' " +
                                          "AND p.projecttype = 'L' " +
                                          "AND lc.NeedFollowUp NOT IN ('F') " +
                                   "GROUP  BY lc.projectid) AS totalcontact " +
                         "ON totalassigned.projectid = totalcontact.projectid " +
                       "LEFT JOIN (SELECT lc.projectid    AS projectid, " +
                                          "lc.AECode AS followupby, " +
                                          "followupdatetbl.followupdate, " +
                                          "COUNT(lc.recid) AS nooffollowup " +
                                   "FROM   dbo.LeadContact lc " +
                                          "INNER JOIN dealerdetail d " +
                                            "ON lc.AECode = d.aecode " +
                                               "AND d.userid = '" + userId + "' " +
                                          "INNER JOIN LeadAssign la " +
                                            "ON lc.projectid = la.projectid " +
                                               "AND la.aecode = d.aecode " +
                                               "AND lc.LeadID = la.LeadID " +
                                          "INNER JOIN (SELECT cc.projectid         AS projectid, " +
                                                             "MAX(cc.followupdate) AS " +
                                                             "followupdate " +
                                                      "FROM   LeadContact cc " +
                                                             "INNER JOIN dealerdetail d " +
                                                               "ON cc.AECode = d.aecode " +
                                                                  "AND d.userid = '" + userId + "' " +
                                                             "INNER JOIN LeadAssign ca " +
                                                               "ON cc.projectid = ca.projectid " +
                                                                  "AND ca.aecode = d.aecode " +
                                                                  "AND ca.LeadID = cc.LeadID " +
                                                      "WHERE  cc.NeedFollowUp = 'N' " +
                                                             "AND cc.AECode IS NOT NULL " +
                                                             "AND cc.followupdate IS NOT NULL " +
                                                      "GROUP  BY cc.projectid) AS followupdatetbl " +
                                            "ON followupdatetbl.projectid = lc.projectid " +
                                   "WHERE  lc.NeedFollowUp = 'N' " +
                                          "AND lc.AECode IS NOT NULL " +
                                          "AND lc.followupdate IS NOT NULL " +
                                   "GROUP  BY lc.projectid, " +
                                             "lc.AECode, " +
                                             "followupdatetbl.followupdate) AS " +
                                  "totalcallstofollowup " +
                         "ON totalcallstofollowup.projectid = totalassigned.projectid   ";
            return genericDA.ExecuteQuery(sql);

            //DataSet ds = new DataSet();
            //ds.Tables.Add(GetTable());
            //return ds;
        }

        public int InsertEmailLog(string fromEmail, string toEmail, string subject, string emailContent)
        {
            int result = 1;

            OleDbParameter[] sqlParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@FromEmail", OleDbType.VarChar), 
                                            new OleDbParameter("@ToEmail", OleDbType.VarChar),
                                            new OleDbParameter("@LogDate", OleDbType.Date),
                                            new OleDbParameter("@Subject", OleDbType.VarChar),
                                            new OleDbParameter("@EmailContent", OleDbType.VarChar)
                                        };

            OleDbCommand cmd = null;
            OleDbTransaction sqlTransaction = null;

            sql = "INSERT INTO EmailLog(FromEmail, ToEmail, LogDate, Subject, EmailContent) " +
                        "VALUES (?, ?, ?, ?, ?)";
            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

            //Create Insert command
            cmd = genericDA.GetSqlCommand();
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(sqlParams);

            try
            {
                cmd.Parameters["@FromEmail"].Value = fromEmail;
                cmd.Parameters["@ToEmail"].Value = toEmail;
                cmd.Parameters["@LogDate"].Value = System.DateTime.Now;
                cmd.Parameters["@Subject"].Value = subject;
                cmd.Parameters["@EmailContent"].Value = emailContent;
                result = cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                result = -1;
            }
            finally
            {

            }
            return result;
        }
    }
}