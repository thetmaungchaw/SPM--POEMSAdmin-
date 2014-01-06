﻿/* 
 * Purpose:         Leads Contact Management Webservices data access layer
 * Created By:      Yin Mon Win
 * Date:            14/09/2011
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class LeadsContactDA
    {
        private string sql;
        private GenericDA genericDA;

        public LeadsContactDA(GenericDA da)
        {
            genericDA = da;
        }


        public int UpdateLeadsContactFollowUp(string dealerCode, string userId, string leadId, string sex, string MobileNo, string HomeNo, string content,
             string needFollowup,  string preferMode, string projId, string recId)
        {
            //DateTime? followupDt = null; 
            int result = -1, updatedId = 0;
            OleDbParameter[] oledbParams = null;

            //ContactDate, LeadId, ContactPhone, Content, DealerCode,  ModifiedUser, ModifiedDate, NeedFollowUp,FollowupDate,PreferMode
            //sql = " UPDATE SPM.dbo.LeadContact  SET MobileNo=?,HomeNo=?, Content=?, DealerCode=?, ModifiedUser=?, " +
            //            " ModifiedDate=GETDATE(), NeedFollowUp= ?, PreferMode=? WHERE RecId=? ";


            sql = " UPDATE SPM.dbo.LeadContact  SET NeedFollowUp= ? WHERE RecId=? ";
            oledbParams = new OleDbParameter[] 
                                        {                                              
                                            //new OleDbParameter("@MobileNo", OleDbType.VarChar),
                                            //new OleDbParameter("@HomeNo", OleDbType.VarChar),
                                            //new OleDbParameter("@content", OleDbType.VarChar),
                                            //new OleDbParameter("@levent", OleDbType.VarChar),
                                            //new OleDbParameter("@dealerCode", OleDbType.VarChar),
                                            //new OleDbParameter("@modifiedUser", OleDbType.VarChar), 
                                            new OleDbParameter("@needFollowup", OleDbType.VarChar),
                                            //new OleDbParameter("@followupDate", OleDbType.Date),
                                            //new OleDbParameter("@preferMode", OleDbType.VarChar),
                                            new OleDbParameter("@recId", OleDbType.VarChar)
                                        };
            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;

            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                //cmd.Parameters["@MobileNo"].Value = MobileNo;
                //cmd.Parameters["@HomeNo"].Value = HomeNo;
                //cmd.Parameters["@content"].Value = content;
                // cmd.Parameters["@levent"].Value = Levent;
                //cmd.Parameters["@dealerCode"].Value = dealerCode;
                //cmd.Parameters["@modifiedUser"].Value = dealerCode;
                cmd.Parameters["@needFollowup"].Value = needFollowup;
                //cmd.Parameters["@followupDate"].Value = followupDt;
                //cmd.Parameters["@preferMode"].Value = preferMode;
                cmd.Parameters["@recId"].Value = recId;

                updatedId = cmd.ExecuteNonQuery();
                if (updatedId > 0)             //if (result > 0) for Get InsertedId with Separate Command
                {
                    sqlTrans.Commit();

                    result = updatedId;
                }
            }
            catch (Exception e)
            {
                result = -1;
            }
            finally
            {
                if (result < 1)
                {
                    try
                    {
                        if (sqlTrans != null)
                            sqlTrans.Rollback();
                    }
                    catch (Exception ex)
                    { }
                }
            }

            return result;
        }

        //ContactDate, LeadId, ContactPhone, Content, DealerCode,  ModifiedUser, ModifiedDate, NeedFollowUp,FollowupDate,PreferMode
        public int InsertLeadsContact(string dealerCode, string userId, string leadId,string sex, string MobileNo,string HomeNo, string content,
              string needFollowup, string followupDate,string preferMode, string projId,string recId, string status)
        {
            //string dateFormat = "dd/MM/yyyy";
            DateTime? followupDt = null;

           //followupDt = DateTime.ParseExact(followupDate, dateFormat, null);//Convert.ToDateTime(followupDate);
            if (!string.IsNullOrEmpty(followupDate))
            {
                IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                String datetime = followupDate;
                followupDt = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            }
            
            int result = -1, insertedId = 0;
            OleDbParameter[] oledbParams = null;

            //sql = " INSERT INTO SPM.dbo.ClientContact(ContactDate, AcctNo, ContactPhone, Content, Remarks, Rank, ModifiedUser, ModifiedDate, Keep) " +
            //        " VALUES(GETDATE(), UPPER('" + acctNo + "'), '" + contactPhone + "', N'" + content + "', N'" + remark + "', '" +
            //        rank + "', '" + dealerCode + "', GETDATE(), '" + keep + "') ";

            if (status == "new")
            {
                //sql = " INSERT INTO SPM.dbo.ClientContact(ContactDate, AcctNo, ContactPhone, Content, Remarks, Rank, ModifiedUser, ModifiedDate, Keep, AdminId) " +
                //    " VALUES(GETDATE(), UPPER('" + acctNo + "'), '" + contactPhone + "', '" + content + "', '" + remark + "', '" +
                //    rank + "', '" + dealerCode + "', GETDATE(), '" + keep + "', '" + adminId + "'); " +
                //    "SELECT SCOPE_IDENTITY();";

                sql = " INSERT INTO SPM.dbo.LeadContact(ContactDate, LeadId, MobileNo, HomeNo, Content,  AECode, ModifiedUser, ModifiedDate, NeedFollowUp, FollowUpDate,PreferMode,ProjectID) " +
                    " VALUES(GETDATE(), ?, ?, ?, ?, ?, ?,GETDATE(), ?, ?,?,?); " +
                    "SELECT SCOPE_IDENTITY();";

                oledbParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@leadId", OleDbType.VarChar), 
                                            new OleDbParameter("@MobileNo", OleDbType.VarChar),
                                            new OleDbParameter("@HomeNo", OleDbType.VarChar),
                                            new OleDbParameter("@content", OleDbType.VarChar),
                                            //new OleDbParameter("@levent", OleDbType.VarChar),
                                            new OleDbParameter("@dealerCode", OleDbType.VarChar),
                                            new OleDbParameter("@modifiedUser", OleDbType.VarChar), 
                                            new OleDbParameter("@needFollowup", OleDbType.VarChar),
                                            new OleDbParameter("@followupDate", OleDbType.VarChar),
                                            new OleDbParameter("@preferMode", OleDbType.VarChar),
                                            new OleDbParameter("@projectID", OleDbType.VarChar)
                                        };
            }
            else
            {
                //sql = " UPDATE SPM.dbo.ClientContact ";
                //sql = sql + " SET ContactPhone='" + contactPhone + "', Keep='" + keep + "', Content='" + content +
                //    "', Remarks='" + remark + "', Rank=" + rank + ", ModifiedUser='" + dealerCode + "', ModifiedDate=GETDATE() " +
                //    ", AdminId='" + adminId + "' ";
                //sql = sql + " WHERE RecId='" + recId + "' ";

                //ContactDate, LeadId, ContactPhone, Content, DealerCode,  ModifiedUser, ModifiedDate, NeedFollowUp,FollowupDate,PreferMode
                sql = " UPDATE SPM.dbo.LeadContact  SET MobileNo=?,HomeNo=?, Content=?, AECode=?, ModifiedUser=?, " +
                            " ModifiedDate=GETDATE(), NeedFollowUp= ?, FollowupDate=?, PreferMode=? WHERE RecId=? ";

                oledbParams = new OleDbParameter[] 
                                        {                                              
                                            new OleDbParameter("@MobileNo", OleDbType.VarChar),
                                            new OleDbParameter("@HomeNo", OleDbType.VarChar),
                                            new OleDbParameter("@content", OleDbType.VarChar),
                                            //new OleDbParameter("@levent", OleDbType.VarChar),
                                            new OleDbParameter("@dealerCode", OleDbType.VarChar),
                                            new OleDbParameter("@modifiedUser", OleDbType.VarChar), 
                                            new OleDbParameter("@needFollowup", OleDbType.VarChar),
                                            new OleDbParameter("@followupDate", OleDbType.VarChar),
                                            new OleDbParameter("@preferMode", OleDbType.VarChar),
                                            new OleDbParameter("@recId", OleDbType.VarChar)
                                        };
            }                                        

            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            //StringBuilder sb = new StringBuilder("");
            string insertIdStr = "";

            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                cmd.Parameters["@MobileNo"].Value = MobileNo ;
                cmd.Parameters["@HomeNo"].Value = HomeNo;
                cmd.Parameters["@content"].Value = content;
               // cmd.Parameters["@levent"].Value = Levent;
                cmd.Parameters["@dealerCode"].Value = dealerCode;
                cmd.Parameters["@modifiedUser"].Value = dealerCode;
                cmd.Parameters["@needFollowup"].Value = needFollowup;

                if (followupDt == null)
                    cmd.Parameters["@followupDate"].Value = "";
                else
                    cmd.Parameters["@followupDate"].Value = followupDt.Value.ToString("yyyy-MM-dd");
                
                cmd.Parameters["@preferMode"].Value = preferMode ;

                if (status == "new")
                {
                    cmd.Parameters["@leadId"].Value = leadId;
                    cmd.Parameters["@projectID"].Value = projId;
                }
                else
                    cmd.Parameters["@recId"].Value = recId;

                //result = cmd.ExecuteNonQuery();

                if (status == "new")
                {
                    insertIdStr = cmd.ExecuteScalar().ToString();
                    if (!String.IsNullOrEmpty(insertIdStr))
                    {
                        insertedId = int.Parse(insertIdStr);
                    }
                }
                else
                {
                    insertedId = cmd.ExecuteNonQuery();
                }


                if (insertedId > 0)             //if (result > 0) for Get InsertedId with Separate Command
                {
                   
                    sqlTrans.Commit();

                    //if (status == "new")
                    //{
                        //result = insertedId;
                    //}

                    result = insertedId;
                }
            }
            catch (Exception e)
            {
                result = -1;                
            }
            finally
            {
                if (result < 1)
                {
                    try
                    {
                        if (sqlTrans != null)
                            sqlTrans.Rollback();
                    }
                    catch (Exception ex)
                    { }
                }
            }
            
            return result;
        }

        public int DeleteLeadsContact(string recId)
        {
            sql = "DELETE FROM LeadContact WHERE RecId = '" + recId + "'";

            return genericDA.ExecuteNonQuery(sql);
        }

        public DataTable RetrieveUnContactedAssignment(string dealerCode, String UserID)
        {
            
            //Changes for Sorting
            //CONVERT(VARCHAR(10), CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate
            sql = "SELECT CLA.LeadId, CLA.AECode, CLA.AssignDate, CLA.CutOffDate, CCT.ModifiedUser, " +
                    "CONVERT(VARCHAR, CCT.ContactDate, 120) AS ContactDate, CCT.ModifiedUser AS DealerCode, LeadGender as Sex, "+
                    "CLM.LeadMobile, CLM.LeadHomeNo,  CLM.LeadEMAIL ,CLM.LeadNAME AS LeadName, CLM.LeadNRIC,CLM.Event,PreferMode, " +
                    "DD.AEGroup AS DealerTeam,CLA.ProjectID, ProjectName " +
                    "FROM LeadAssign CLA " +
                    "INNER JOIN " +             // JOIN for retrieve lastest Client Assignment record
                    "( " +
                    "   SELECT LeadId, MAX(AssignDate) AS MADate  " +
                    "   FROM LeadAssign WITH (NOLOCK) " +
                    "   WHERE AECode IN ('" + dealerCode + "')" +
                    "   AND CutOffDate >= GETDATE() " +             //New added at 27 April 2010 for checking with CutOff Date, to retrieve latest assignment only.
                    "   GROUP BY LeadId " +
                    ") MCLA ON CLA.AssignDate = MCLA.MADate AND CLA.LeadId = MCLA.LeadId AND CLA.AECode IN ('" + dealerCode + "') " +
                    "LEFT JOIN " +		// JOIN for retrieve lastest Client Contact record
                    "( " +
                    "    SELECT CCT1.LeadId, CCT1.PreferMode,CCT1.Content,CCT1.ModifiedUser, CCT1.ContactDate " +
                    "    FROM LeadContact CCT1 " +
                    "    INNER JOIN " +
                    "    ( " +
                    "        SELECT LeadId, MAX(ContactDate) AS MADate  " +
                    "        FROM LeadContact WITH (NOLOCK)  " +
                    "        GROUP BY LeadId " +
                    "    ) MCCT ON CCT1.LeadId = MCCT.LeadId AND CCT1.ContactDate = MCCT.MADate " +
                    ") CCT ON CLA.LeadId = CCT.LeadId " +
                    //"LEFT JOIN ClientSex CS WITH (NOLOCK)  ON CLA.AcctNo = CS.AcctNo " +
                    "LEFT JOIN ProjectDetail PD WITH(NOLOCK) ON CLA.ProjectID = PD.ProjectID " +
                    "LEFT JOIN LeadDetail CLM WITH (NOLOCK) ON CLA.LeadId = CLM.LeadId " +
                    "LEFT JOIN DealerDetail DD WITH(NOLOCK) ON CLA.AECode = DD.AECode " +
                    "WHERE (CCT.ContactDate < CLA.AssignDate) OR (CCT.ContactDate IS NULL) AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ORDER BY CLA.LeadId";      //WHERE CCT.ContactDate IS NULL

            return genericDA.ExecuteQueryForDataTable(sql, "dtUnContactedAssignment");
        }

        public DataTable RetrieveClientInfoByShortKey(string shortKey)
        {
            sql = "SELECT SK.AcctNo, SK.ShortKey, CLM.LNAME, ISNULL(CP.Phone, '') AS Phone, ISNULL(CS.Sex, '') AS Sex, " +
                    "ISNULL(CPR.PreferA, '') AS PreferA , ISNULL(CPR.PreferB, '') AS PreferB " +
                    "FROM ShortKey SK WITH (NOLOCK) " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON SK.AcctNo = CLM.LACCT " +
                    "LEFT JOIN ClientPhone CP WITH (NOLOCK) ON SK.AcctNo = CP.AcctNo " +
                    "LEFT JOIN ClientSex CS WITH (NOLOCK) ON SK.AcctNo = CS.AcctNo " +
                    "LEFT JOIN ClientPrefer CPR WITH (NOLOCK) ON SK.AcctNo = CPR.AcctNo " +
                    "WHERE SK.ShortKey = '" + shortKey + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable getContactHistoryByLeadId(string leadId)
        {
            //sql = GetContactHistorySQL();
            //sql = sql + " WHERE CCT.AcctNo = '" + accountNo + "' ORDER BY ContactDate DESC";

            StringBuilder sb = new StringBuilder(GetContactHistorySQL("History"));

            sql = "INNER JOIN " +     //For Extra Call
                    "( " +
                    "    SELECT CLA.AssignDate, CCT1.LeadId, CCT1.ModifiedUser, CCT1.ContactDate " +
                    "    FROM LeadContact CCT1 " +
                    "    LEFT JOIN LeadAssign CLA WITH (NOLOCK) ON CLA.LeadId = CCT1.LeadId " +
                    "    AND CLA.AECode = CCT1.ModifiedUser	" +
                    "    AND CCT1.ContactDate BETWEEN CLA.AssignDate AND CLA.CutOffDate " +
                    "    WHERE CCT1.LeadId = '" + leadId + "' " +
                    ") CLA2 ON CLA2.ContactDate = CCT.ContactDate ";
            
            sb.Append(sql);
            sb.Append(" WHERE CCT.LeadId = '").Append(leadId).Append("' ORDER BY CCT.ContactDate DESC");
            
            return genericDA.ExecuteQueryForDataTable(sb.ToString(), "dtContactHistory");
        }

        public DataTable RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                            string preference, string content, string teamCode)
        {
            bool whereFlag = false;

            //Changes for Sorting
            //CONVERT(VARCHAR(20), CCT.ContactDate, 120) AS ContactDate
            sql = "SELECT CCT.ContactDate, DD.AEGroup AS Team, CCT.AcctNo, ISNULL(LName, '') AS Name, " +
                    "ISNULL(Sex, '') AS Sex, ISNULL(CCT.ContactPhone, '') AS ContactNo, ISNULL(Content, '') AS Content, " +
                    "ISNULL(Remarks, '') AS Remarks, Rank, ISNULL(CP.PreferA, '00') AS PreferA, " +
                    "ISNULL(CP.PreferB, '00') AS PreferB, CCT.ModifiedUser AS Dealer, CLA2.AssignDate, CCT.AdminId " +
                    ", RankText = CASE Rank WHEN 0 THEN 'No Rank' WHEN 1 THEN 'Excellent Relationship' WHEN 2 THEN 'Good Relationship' " +
                    "WHEN 3 THEN 'Average Relationship' WHEN 4 THEN 'Fair Relationship' WHEN 5 THEN 'Poor Relationship' END " +
                    "FROM ClientContact CCT WITH (NOLOCK) " +
                    "LEFT JOIN CLMAST CM WITH (NOLOCK) ON CCT.AcctNo=CM.LACCT " +
                    "LEFT JOIN ClientSex CS WITH (NOLOCK) ON CCT.AcctNo=CS.AcctNo " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON CCT.ModifiedUser=DD.AECode " +
                    "LEFT JOIN ClientPrefer CP WITH (NOLOCK) ON CCT.AcctNo=CP.AcctNo " +
                    "INNER JOIN " +     //For Extra Call
                    "( " +
                    "    SELECT CLA.AssignDate, CCT1.AcctNo, CCT1.ModifiedUser, CCT1.ContactDate " +
                    "    FROM ClientContact CCT1 " +
                    "    LEFT JOIN ClientAssign CLA WITH (NOLOCK) ON CLA.AcctNo = CCT1.AcctNo " +
                    "    AND CLA.AECode = CCT1.ModifiedUser	" +
                    "    AND CCT1.ContactDate BETWEEN CLA.AssignDate AND CLA.CutOffDate ";

            StringBuilder sb = new StringBuilder(sql);

            if(!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE CCT1.AcctNo = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            //Checking for Contact Date
            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), CCT1.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), CCT1.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }                
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), CCT1.ContactDate, 120) >= '").Append(dateFrom).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), CCT1.ContactDate, 120) >= '").Append(dateFrom).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), CCT1.ContactDate, 120) <= '").Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), CCT1.ContactDate, 120) <= '").Append(dateTo).Append("' ");
                }                
            }

            sb.Append(") CLA2 ON CLA2.ContactDate = CCT.ContactDate ");                        
            whereFlag = false;


            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE CCT.ModifiedUser = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(teamCode))
            {
                if (whereFlag)
                    sb.Append(" AND DD.AEGroup = '").Append(teamCode).Append("' ");
                else
                {
                    sb.Append(" WHERE DD.AEGroup = '").Append(teamCode).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND CCT.AcctNo = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE CCT.AcctNo = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND CONVERT(VARCHAR(10), CCT.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                    whereFlag = true;
                }
            }
            else if(!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) >= '").Append(dateFrom).Append("' ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) <= '").Append(dateTo).Append("' ");
            }


            if (!String.IsNullOrEmpty(rank))
            {
                if (whereFlag)
                    sb.Append(" AND CCT.Rank = '").Append(rank).Append("' ");
                else
                {
                    sb.Append(" WHERE CCT.Rank = '").Append(rank).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(preference))
            {
                if (whereFlag)
                    sb.Append(" AND CP.PreferA = '").Append(preference).Append("' OR CP.PreferB = '").Append(preference).Append("' ");
                else
                {
                    sb.Append(" WHERE CP.PreferA = '").Append(preference).Append("' OR CP.PreferB = '").Append(preference).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(content))
            {
                if (whereFlag)
                    sb.Append(" AND CCT.Content = '").Append(content).Append("' ");
                else
                {
                    sb.Append(" WHERE CCT.Content = '").Append(content).Append("' ");
                    whereFlag = true;
                }
            }
            
            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo)
        {
            bool whereFlag = false;

            sql = "SELECT CC.ContactDate, CC.ModifiedUser AS DealerCode, TCA.AECode AS TeamCode, CLTD.AcctNo, TCA.ClientName, " +
                    "CLTD.LTradeDate, TCA.LastTradeDate, TCA.TotalComm  " +
                    "FROM  " +
                    "( " +
                    "    SELECT DISTINCT AcctNo, MIN(CreateDate) AS CreateDate, LTradeDate  " +
                    "    FROM ClientLTradeDate WITH (NOLOCK) " +
                    "    GROUP BY AcctNo, LTradeDate  " +
                    ") CLTD  " +
                    "INNER JOIN ClientContact CC WITH (NOLOCK) ON CLTD.AcctNo=CC.AcctNo AND  " +
                    "    CONVERT(VARCHAR(10), CLTD.CreateDate, 120)=CONVERT(VARCHAR(10), CC.ContactDate, 120)  " +
                    "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLTD.AcctNo=TCA.AcctNo " +
                    " AND TCA.LastTradeDate > CLTD.LTradeDate ";

                    //" AND DATEDIFF(dd, CLTD.LTradeDate , TCA.LastTradeDate) >= 0 ";                    
                    //" AND TCA.LastTradeDate > CLTD.LTradeDate ";

            StringBuilder sb = new StringBuilder(sql);

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE CC.ModifiedUser = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND CC.AcctNo = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE CC.AcctNo = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND CC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                else
                {
                    sb.Append(" WHERE CC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE CC.ContactDate >= '").Append(dateFrom).Append("' ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE CC.ContactDate <= '").Append(dateTo).Append("' ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable RetrieveCallReport(string assignDate)
        {
            //Old way of calcuation of Total Assignment
            //ISNULL(MI.Miss, 0) + ISNULL(EX.Extra, 0) + ISNULL(ReTradeA, 0) + ISNULL(ReTradeE, 0) AS TotalAssign

            sql = "SELECT DD.AEGroup AS Team, DD.AECode AS Dealer, ISNULL(DD.AEName, '-') AS AEName " +
                  ", TCA.TotalAssign " +      //Total Assignments for each Dealer
                  ", ISNULL(MI.Miss, 0) AS Miss," +
                  "ISNULL(EX.Extra, 0) AS Extra, ISNULL(ReTradeA, 0) AS ReTradeA,  " +
                  "ISNULL(ReTradeE, 0) AS ReTradeE,  " +
                  "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 + ISNULL(ReTradeA, 0)*2.6 + ISNULL(ReTradeE, 0)*1.6 ) AS Score  " +                  
                  "FROM DealerDetail DD WITH (NOLOCK)  " +
                  "INNER JOIN  " +				// -- JOIN to retrieve Assignments and Dealer info to match User entered Assignment Date 
                  "( " +
                  "     SELECT AECode, COUNT(AcctNo) AS TotalAssign " +
                  "     FROM ClientAssign WITH (NOLOCK)  " +
                  "     WHERE AssignDate = '" + assignDate + "' " +
                  "     GROUP BY AECode  " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT CA.AECode, COUNT(CA.AcctNo) AS Miss  " +
                  "     FROM ClientAssign CA WITH (NOLOCK) LEFT JOIN ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser " +
                  "     AND CA.AcctNo=CC.AcctNo AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "     WHERE CA.AssignDate = '" + assignDate + "' " +
                  "     AND CC.ContactDate IS NULL " +
                  "     AND GETDATE() > CA.CutOffDate " +
                  "     GROUP BY CA.AECode " +
                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT CC.ModifiedUser, COUNT(CC.AcctNo) -  COUNT(CA.AcctNo) AS Extra " +
	              "      FROM ClientContact CC WITH (NOLOCK) " +
	              "      LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
	              "      AND CC.AcctNo = CA.AcctNo " +
	              "      AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
	              "      WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
	              "      AND CC.ContactDate >= '" + assignDate + "' " +
	              "      GROUP BY CC.ModifiedUser " +
                  ") EX ON DD.AECode=EX.ModifiedUser  " +
                 " LEFT JOIN " +				            // -- JOIN to retrieve ReTrade Client Extra Call
                 " ( " +
                 "       SELECT COUNT(CLD1.AcctNo) AS ReTradeE, ExtraCall.ModifiedUser " +
                 "       FROM ClientLTradeDate CLD1 WITH (NOLOCK) " +
                 "       INNER JOIN " +
                 "       ( " +
                 "              SELECT ModifiedUser, AcctNo, ContactDate " +        // Join to retrieve Extra Call
                 "              FROM " +
                 "              ( " +
	             "                 SELECT CC.ModifiedUser, CC.AcctNo, CC.ContactDate, CA.AssignDate " +
	             "                 FROM ClientContact CC WITH (NOLOCK) " +     
	             "                 LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
	             "                 AND CC.AcctNo = CA.AcctNo " +
	             "                 AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
	             "                 WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
	             "                 AND CC.ContactDate >= '" + assignDate + "' " +
                 "              ) TExtra " +
                 "              WHERE AssignDate IS NULL " +
                 "      ) ExtraCall ON CLD1.AcctNo = ExtraCall.AcctNo " +
                 "   AND CONVERT(VARCHAR(10), CLD1.CreateDate, 120) = CONVERT(VARCHAR(10), ExtraCall.ContactDate, 120) " +
                 "   INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLD1.AcctNo=TCA.AcctNo " +
                 "   WHERE TCA.LastTradeDate > CLD1.LTradeDate " +
                 "   GROUP BY ExtraCall.ModifiedUser " +        //GROUP BY ExtraCall.ModifiedUser, CLD1.LTradeDate, TCA.LastTradeDate
                 ") TRE ON DD.AECode = TRE.ModifiedUser " +
                 "LEFT JOIN	" +									//-- For Client ReTrader Count after Assignment
                 "( " +
                 "       SELECT COUNT(TCLD.AcctNo) AS ReTradeA, CC.ModifiedUser " +
                 "       FROM " +
                 "       ( " +
	             "           SELECT CLD1.AcctNo, CLD1.CreateDate, LTradeDate " +
	             "           FROM ClientLTradeDate CLD1 WITH (NOLOCK) " +
	             "           LEFT JOIN ClientAssign CA1 WITH (NOLOCK) ON CLD1.AcctNo = CA1.AcctNo " +
	             "           AND CONVERT(VARCHAR(10), CreateDate, 120) BETWEEN CA1.AssignDate AND CA1.CutOffDate " +
	             "           WHERE CA1.AssignDate = '" + assignDate + "' " +
                 "       ) TCLD	" +
                 "       INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCLD.AcctNo=TCA.AcctNo " +
                 "       INNER JOIN ClientContact CC WITH (NOLOCK) ON TCLD.AcctNo=CC.AcctNo " +
                 "       AND CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120) " +
                 "       WHERE TCA.LastTradeDate > TCLD.LTradeDate " +
                 "       GROUP BY CC.ModifiedUser " +   // GROUP BY CC.ModifiedUser, TCLD.LTradeDate, TCA.LastTradeDate
                 ") TRA ON DD.AECode = TRA.ModifiedUser " +
                 "WHERE ISNULL(TCA.AECode, '') <> '' ";


            /*
             // Based on ASP and HK IT version
             "LEFT JOIN " +				//-- JOIN to retrieve ReTrade Client after Assign and ReTrade Client after Extra Contact
                  "( " +
                  "      SELECT CC.ModifiedUser, COUNT(CA.AssignDate) AS ReTradeA, COUNT(CC.ModifiedUser)-COUNT(CA.AssignDate) AS ReTradeE  " +
                  "      FROM  " +
                  "      ( " +
                  "         SELECT CLD1.AcctNo, MIN(CreateDate) AS CreateDate, LTradeDate  " +
                  "         FROM ClientLTradeDate CLD1 WITH (NOLOCK) " +
                  "         LEFT JOIN ClientAssign CA1 WITH (NOLOCK) ON CLD1.AcctNo = CA1.AcctNo AND CA1.AssignDate = '" + assignDate + "' " +
                  "         WHERE CONVERT(VARCHAR(10), CreateDate, 120) BETWEEN CA1.AssignDate AND CA1.CutOffDate " +
                  "         GROUP BY CLD1.AcctNo, CLD1.LTradeDate  " +
                  "      ) CLTD INNER JOIN ClientContact CC WITH (NOLOCK) ON CLTD.AcctNo=CC.AcctNo AND CONVERT(VARCHAR(10), CLTD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120)  " +
                  "      INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLTD.AcctNo=TCA.AcctNo  " +
                  "      LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CLTD.AcctNo=CA.AcctNo  " +
                  "      AND CONVERT(VARCHAR(10), CA.AssignDate, 120) = '" + assignDate + "' " +
                  "      WHERE TCA.LastTradeDate > CLTD.LTradeDate AND CC.ContactDate > '" + assignDate + "' " +
                  "      GROUP BY CC.ModifiedUser , TCA.LastTradeDate, CLTD.LTradeDate " +
                  ") TRE ON DD.AECode=TRE.ModifiedUser " +
             */


            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveCallReportDetail(string assignDate, string dealerCode)
        {

            sql = "SELECT CA.AECode, CA.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +		//-- SQL to retrieve Miss Call Detail
                        "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
                        "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser AND CA.AcctNo=CC.AcctNo " +
                        "AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                        "WHERE CA.AssignDate = '" + assignDate + "' " +
                        //Condition for Miss Call, according to feedbacks from user, all the Assignments need to be displayed.
                        //"AND CC.ContactDate IS NULL AND GETDATE() > CA.CutOffDate " +     
                        "AND AECode = '" + dealerCode + "' " +
                        "UNION " +
                        "SELECT ModifiedUser AS AECode, AcctNo, ContactDate, AssignDate, CutOffDate " +	        // -- SQL to retrieve Extra Call Details
                        "FROM " +
                        "( " +
                        "    SELECT CC.ModifiedUser, CC.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +
                        "    FROM ClientContact CC WITH (NOLOCK) " +
                        "    LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
                        "    AND CC.AcctNo = CA.AcctNo " +
                        "    AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                        "    WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
                        "    AND CC.ContactDate >= '" + assignDate + "' " +
                        ") TExtra " +
                        "WHERE AssignDate IS NULL " +
                        "AND ModifiedUser = '" + dealerCode + "' " +
                        "ORDER BY ContactDate DESC ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveClientAnalysis(string teamCode, string dealerCode, string accountCreateDate, string lastTradeDate)
        {
            sql = "SELECT TALL.AECode, UPPER(ISNULL(DIT.GroupName, '')) AS GroupName, ISNULL(TotAct, 0) AS TotAct, " +
                    "ISNULL(Tot3None, 0) AS Tot3None, ISNULL(TotInBeforeAOBefore, 0) AS TotInBeforeAOBefore, ISNULL(TotInBeforeAOAfter, 0) AS TotInBeforeAOAfter, " +
                    "ISNULL(TotInAfterAOBefore, 0) AS TotInAfterAOBefore, ISNULL(TotInAfterAOAfter, 0) AS TotInAfterAOAfter " +
                    "FROM ( SELECT DISTINCT AECode FROM SPM.dbo.TmpClientAssign WITH (NOLOCK) ) TALL " +
                    "LEFT JOIN " +  			//-- Active Client
                    "(  " +
                    "   SELECT AECode, COUNT(AECode) AS TotAct FROM TmpClientAssign WITH (NOLOCK)  " +
                    "   WHERE Status='A' GROUP BY AECode " +
                    ") TAA ON TALL.AECode=TAA.AECode " +
                    "LEFT JOIN " +  			//-- 2N Client
                    "( " +
                        "SELECT AECode, COUNT(AECode) AS Tot3None from SPM.dbo.TmpClientAssign TCA WITH (NOLOCK) " +
                        "LEFT JOIN SPM.dbo.CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo=CLM.LACCT " +
                        "WHERE ( Status='I' AND TotalComm=0 AND out_Bal<0 AND DateDiff(MM, ISNULL(LastTradeDate, '1900-01-01'), GETDATE())>=3 " +
                        "AND DateDiff(MM, ISNULL(CLM.LCRDATE, '1900-01-01'), GETDATE())>=3 ) " +
                        "GROUP BY AECode " +
                    ") T3N ON TALL.AECode=T3N.AECode " +
                    "LEFT JOIN " +  				//-- Inactive Client with Last Tading Date < SetDate & Open Date < SetDate
                    "( " +
                    "   SELECT AECode, COUNT(AECode) AS TotInBeforeAOBefore FROM SPM.dbo.TmpClientAssign TCA WITH (NOLOCK) " +
                    "   LEFT JOIN CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo=CLM.LACCT  " +
                    "   LEFT JOIN ClientLTD LTD WITH (NOLOCK) ON TCA.AcctNo=LTD.AcctNo " +
                    "   WHERE Status='I' AND NOT (TotalComm=0 AND out_Bal<0 AND DateDiff(MM, ISNULL(LastTradeDate, '1900-01-01'), GETDATE())>=3 " +
                    "   AND DateDiff(MM, ISNULL(CLM.LCRDATE, '1900-01-01'), GETDATE())>=3) " +
                    "   AND ISNULL(LTD.OverallLTD, LTD.AcctCreate) < '" + lastTradeDate + "' " +
                    "   AND ISNULL(LTD.AcctCreate, '1900-01-01') < '" + accountCreateDate + "' " +
                    "   GROUP BY AECode " +
                    ") TIBOB ON TALL.AECode=TIBOB.AECode " +
                    "LEFT JOIN " + 				    //-- Inactive Client with Last Tading Date < SetDate & Open Date < SetDate
                    "( " +
                    "   SELECT AECode, COUNT(AECode) AS TotInBeforeAOAfter FROM SPM.dbo.TmpClientAssign TCA WITH (NOLOCK) " +
                    "   LEFT JOIN SPM.dbo.CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo=CLM.LACCT " +
                    "   LEFT JOIN SPM.dbo.ClientLTD LTD WITH (NOLOCK) ON TCA.AcctNo=LTD.AcctNo " +
                    "   WHERE Status='I' AND NOT ( TotalComm=0 AND out_Bal<0 AND DateDiff(MM, ISNULL(LastTradeDate, '1900-01-01'), GETDATE())>=3 " +
                    "   AND DateDiff(MM, ISNULL(CLM.LCRDATE, '1900-01-01'), GETDATE())>=3 ) " +
                    "   AND ISNULL(LTD.OverallLTD, LTD.AcctCreate)< '" + lastTradeDate + "' " +
                    "AND ISNULL(LTD.AcctCreate, '1900-01-01') >= '" + accountCreateDate + "' " +
                    "   GROUP BY AECode " +
                    ") TIBOA ON TALL.AECode=TIBOA.AECode " +
                    "LEFT JOIN " + 				//-- Inactive Client with Last Tading Date < SetDate & Open Date < SetDate
                    "( " +
                    "   SELECT AECode, COUNT(AECode) AS TotInAfterAOBefore FROM SPM.dbo.TmpClientAssign TCA WITH (NOLOCK) " +
                    "   LEFT JOIN SPM.dbo.CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo=CLM.LACCT " +
                    "   LEFT JOIN SPM.dbo.ClientLTD LTD WITH (NOLOCK) ON TCA.AcctNo=LTD.AcctNo " +
                    "   WHERE Status='I' AND NOT ( TotalComm=0 AND out_Bal<0 AND DateDiff(MM, ISNULL(LastTradeDate, '1900-01-01'), GETDATE())>=3 " +
                    "   AND DateDiff(MM, ISNULL(CLM.LCRDATE, '1900-01-01'), GETDATE())>=3 ) " +
                    "   AND ISNULL(LTD.OverallLTD, LTD.AcctCreate) >= '" + lastTradeDate + "' " +
                    "AND ISNULL(LTD.AcctCreate, '1900-01-01') < '" + accountCreateDate + "' " +
                    "   GROUP BY AECode " +
                    ") TIAOB ON TALL.AECode=TIAOB.AECode " +
                    "LEFT JOIN " +  				//-- Inactive Client with Last Tading Date < SetDate & Open Date < SetDate
                    "( " +
                    "    SELECT AECode, COUNT(AECode) AS TotInAfterAOAfter FROM SPM.dbo.TmpClientAssign TCA WITH (NOLOCK) " +
                    "    LEFT JOIN CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo=CLM.LACCT " +
                    "    LEFT JOIN ClientLTD LTD WITH (NOLOCK) ON TCA.AcctNo=LTD.AcctNo " +
                    "    WHERE Status='I' AND NOT ( TotalComm=0 AND out_Bal<0 AND DateDiff(MM, ISNULL(LastTradeDate, '1900-01-01'), GETDATE())>=3 " +
                    "    AND DateDiff(MM, ISNULL(CLM.LCRDATE, '1900-01-01'), GETDATE())>=3 ) " +
                    "    AND ISNULL(LTD.OverallLTD, LTD.AcctCreate) >= '" + lastTradeDate + "' " +
                    "   AND ISNULL(LTD.AcctCreate, '1900-01-01') >= '" + accountCreateDate + "' " +
                    "    GROUP BY AECode " +
                    ") TIAOA ON TALL.AECode=TIAOA.AECode " +
                    "LEFT JOIN DealerIDTable DIT WITH (NOLOCK) ON TALL.AECode=DIT.AECodeGp AND GroupType='Dealer' AND " +
                    "RefMonthYr = '" + DateTime.Now.ToString("YYYYMM") + "'  ";

            StringBuilder sb = new StringBuilder(sql);
            bool whereFlag = false;

            if (!String.IsNullOrEmpty(teamCode))
            {
                sb.Append(" WHERE UPPER(ISNULL(DIT.GroupName, ''))=UPPER('").Append(teamCode).Append("') ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerCode))
            {
                if (whereFlag)
                {
                    sb.Append(" AND TALL.AECode = '").Append(dealerCode).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE TALL.AECode = '").Append(dealerCode).Append("' ");
                }
                whereFlag = true;
            }

            // order by team code
            sb.Append(" ORDER BY TALL.AECode");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable GetLeadsByLeadId(string leadId)
        {
            sql = "SELECT LeadId, LeadName, LeadNRIC,LeadMobile,LeadHomeNo,LeadGender,LeadEmail,Event,AEGroup,AECode,CreateDate FROM LeadDetail WHERE LeadId = '" + leadId + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataSet RetrieveContactEntryForToday(DataSet ds, string dealerCode, String UserID)
        {
            StringBuilder sb = new StringBuilder(GetContactHistorySQL("Entry"));

            //sb.Append(" AND CONVERT(VARCHAR(10), CCT.ModifiedDate, 120) = CONVERT(VARCHAR(10), GETDATE(), 120) ");

            #region Update by Thet Maung Chaw

            //sb.Append(" WHERE CCT.ModifiedUser IN ('").Append(dealerCode).Append("') ");
            sb.Append(" WHERE CCT.ModifiedUser IN (SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "')) ");

            #endregion
            
            sb.Append(" AND ContactDate >= ");
            sb.Append(" (SELECT ISNULL(DATEADD(HOUR, 0, MAX(AssignDate)), DATEADD(DAY, -7, GETDATE())) FROM LeadAssign WHERE AECode IN (");

            #region Updated by Thet Maung Chaw

            //sb.Append(dealerCode).Append("'))");
            sb.Append(" SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "'").Append(")))");

            #endregion
            
            sb.Append(" ORDER BY CCT.ContactDate DESC ");

            return genericDA.FillDataSet(ds, "dtEntryHistory", sb.ToString());
        }


        //Add for SPM III
        public DataTable RetrieveFollowUpLeads(string dealerCode)
        {

            sql = "SELECT LC.RecId ,LC.LeadID,LC.AECode,FollowUpDate,NeedFollowUp,LeadName,LeadNRIC,LeadEmail,LC.AECode,AEName,PreferMode,LeadGender As sex, " +
                  "MobileNo AS LeadMobile,HomeNo AS LeadHome,Event, ProjectID " +
                 " FROM LeadContact LC  INNER JOIN LeadDetail  LD on LC.LeadID=LD.LeadID  INNER JOIN DealerDetail DD on DD.AECode=LC.AECode " +
                 " WHERE NeedFollowUp='N' and FollowUpDate<>'' AND LC.AECode IN ('"+ dealerCode +"') ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        private string GetContactHistorySQL(string contactType)
        {
            //Changes for Sorting
            //CONVERT(VARCHAR(20), CCT.ContactDate, 120) AS ContactDate
            sql = "SELECT CCT.RecId, CCT.ContactDate, CCT.LeadId, ISNULL(LeadName, '') AS LeadName, ISNULL(LeadNRIC, '') AS LeadNRIC," +
                    "CCT.AECode AS AECode,CCT.AECode AS DealerCode, ISNULL(LeadGender, '') AS Sex, ISNULL(CCT.MobileNo, '') AS LeadMobile,ISNULL(CCT.HomeNo, '') AS LeadHome, " +
                    "ISNULL(LeadEmail, '') AS LeadEmail,ISNULL(CCT.PreferMode, '') AS PreferMode,ISNULL(Content, '') AS Content, " +
                    " ISNULL(Event, '') AS Event, --ISNULL(SeminarName, '') AS SeminarName,\r\nISNULL(NeedFollowUp, 'N') AS NeedFollowUp, " +
                    "CCT.FollowUpDate, CCT.ModifiedUser, DD.AEGroup,CCT.ProjectID, " +
                    "CONVERT(VARCHAR(20), CCT.ModifiedDate, 120) AS ModifiedDate ";
                    

            if (contactType == "History")
            {
                sql += " , CLA2.AssignDate ";
            }
            else
            {
                sql += " , '' AS AssignDate ";
            }                
                    
            sql +=  " FROM SPM.dbo.LeadContact CCT WITH (NOLOCK) " +
                    " LEFT JOIN SPM.dbo.LeadDetail CM WITH (NOLOCK) ON CCT.LeadId=CM.LeadId " +                   
                    " LEFT JOIN SPM.dbo.DealerDetail DD WITH (NOLOCK) ON CCT.ModifiedUser=DD.AECode \r\n" +
                    " --LEFT JOIN SPM.dbo.SeminarRegistration CP WITH (NOLOCK) ON CCT.LeadId=CP.LeadId \r\n ";

            if (contactType == "Entry")
            {
                //sql += " LEFT JOIN SPM.dbo.ShortKey SK WITH (NOLOCK) ON CCT.AcctNo=SK.AcctNo ";
            }

            return sql;
        }

        /// <summary>
        /// Added by Thet Maung Chaw
        /// To retrieve follow up records for Lead Contact Page
        /// </summary>
        /// <param name="AECode"></param>
        /// <returns></returns>
        public DataTable RetrieveFollowUpLead(String AECode)
        {
            String SQL = String.Empty;

            SQL = "SELECT" + "\r\n";
            SQL += "    LC.RecId, LC.LeadID, LC.AECode, FollowUpDate, NeedFollowUp, LeadName, LeadNRIC, LeadEmail, LC.AECode, AEName, PreferMode, LeadGender As sex," + "\r\n";
            SQL += "    MobileNo AS LeadMobile, HomeNo AS LeadHome, [Event], ProjectID" + "\r\n";
            SQL += "FROM" + "\r\n";
            SQL += "    LeadContact LC" + "\r\n";
            SQL += "    INNER JOIN LeadDetail  LD on LC.LeadID = LD.LeadID" + "\r\n";
            SQL += "    INNER JOIN DealerDetail DD on DD.AECode = LC.AECode" + "\r\n";
            SQL += "WHERE" + "\r\n";
            SQL += "    NeedFollowUp = 'N' and FollowUpDate <> '' AND LC.AECode = '" + AECode + "'";

            return genericDA.ExecuteQueryForDataTable(SQL);
        }
    }
}
