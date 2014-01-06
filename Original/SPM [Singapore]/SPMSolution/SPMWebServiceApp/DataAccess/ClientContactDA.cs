﻿/* 
 * Purpose:         Contact Management Webservices data access layer
 * Created By:      Than Htike Tun
 * Date:            10/3/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 * Than Htike Tun   16/03/2010  Add in data access method for Contact History
 * Than Htike Tun   17/03/2010  Add in data access method for Call Report
 * Than Htike Tun   18/03/2010  Add in data access method for Contact Analysis
 * Than Htike Tun   19/03/2010  Add in data access method for Client Analysis
 * Than Htike Tun   01/04/2010  Add in data access method for Contact Entrhy Admin
 * Than Htike Tun   05/04/2010  Add in audit trail
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
    public class ClientContactDA
    {
        private string sql;       
        private GenericDA genericDA;

        public ClientContactDA(GenericDA da)
        {
            genericDA = da;
        }

        public int InsertClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId, int followupStatus, string followUpDealer, string followUpDate, string projectID)
        {
            int result = -1, insertedId = 0;
            OleDbParameter[] oledbParams = null;
            string followUpStatus = "Y";
           

            sql = " INSERT INTO SPM.dbo.ClientContact(ContactDate, AcctNo, ContactPhone, Content, Remarks, Rank, ModifiedUser, ModifiedDate, Keep, AdminId,FollowUpStatus, FollowUpBy , FollowUpDate,projectID) " +
                    " VALUES(GETDATE(), ?, ?, ?, ?, ?, ?, GETDATE(), ?, ? , ?, ?, ?, ?); " +
                    "SELECT SCOPE_IDENTITY();";

            oledbParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@acctNo", OleDbType.VarChar), 
                                            new OleDbParameter("@contactPhone", OleDbType.VarChar),
                                            new OleDbParameter("@content", OleDbType.VarChar),
                                            new OleDbParameter("@remark", OleDbType.VarChar),
                                            new OleDbParameter("@rank", OleDbType.VarChar), 
                                            new OleDbParameter("@dealerCode", OleDbType.VarChar),
                                            new OleDbParameter("@keep", OleDbType.VarChar),
                                            new OleDbParameter("@adminId", OleDbType.VarChar),
                                            new OleDbParameter("@FollowUpStatus", OleDbType.Char),                                            
                                            new OleDbParameter("@FollowUpBy", OleDbType.VarChar),
                                            new OleDbParameter("@FollowUpDate",OleDbType.Date),
                                            new OleDbParameter("@ProjectID",OleDbType.VarChar)
                                        };
            
            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            StringBuilder sb = new StringBuilder("");
            string insertIdStr = "";
            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                if (followupStatus == 1)
                {
                    followUpStatus = "N";
                    IFormatProvider provider = new System.Globalization.CultureInfo("en-CA", true);
                    String datetime = followUpDate;
                    DateTime dtFormat = DateTime.Parse(datetime, provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
                    cmd.Parameters["@FollowUpDate"].Value = dtFormat;
                    cmd.Parameters["@followUpBy"].Value = followUpDealer;
                }
                else if (followupStatus == 0)
                {
                    cmd.Parameters["@FollowUpDate"].Value = null;
                    cmd.Parameters["@followUpBy"].Value = null;
                    //cmd.Parameters["@FollowUpDate2"].Value = DateTime.ParseExact(followUpDate, "dd/MM/yyyy", null);
                }
                else if(followupStatus == 2)
                {
                    followUpStatus = "F";

                    /// <Updated by Thet Maung Chaw>
                    //cmd.Parameters["@FollowUpDate"].Value = null;
                    //cmd.Parameters["@followUpBy"].Value = null;
                    cmd.Parameters["@FollowUpDate"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                    cmd.Parameters["@followUpBy"].Value = followUpDealer;
                }
                cmd.Parameters["@contactPhone"].Value = contactPhone;
                cmd.Parameters["@content"].Value = content;
                cmd.Parameters["@remark"].Value = remark;
                cmd.Parameters["@rank"].Value = rank;
                cmd.Parameters["@dealerCode"].Value = dealerCode;
                cmd.Parameters["@keep"].Value = keep;
                cmd.Parameters["@adminId"].Value = adminId;
                cmd.Parameters["@FollowUpStatus"].Value = followUpStatus;
                cmd.Parameters["@acctNo"].Value = acctNo;
                cmd.Parameters["@ProjectID"].Value = projectID;
                
                insertIdStr = cmd.ExecuteScalar().ToString();
                if (!String.IsNullOrEmpty(insertIdStr))
                {
                    insertedId = int.Parse(insertIdStr);
                }

                if (insertedId > 0)             //if (result > 0) for Get InsertedId with Separate Command
                {

                    //'************** Delete Previous Client Sex and Insert New one *********
                    sb.Append("DELETE ClientSex WHERE AcctNo='" + acctNo + "';");
                    if (!String.IsNullOrEmpty(sex))
                        sb.Append("INSERT INTO ClientSex VALUES(UPPER('" + acctNo + "'), '" + sex + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Contact and Insert New one *********
                    sb.Append("DELETE ClientPhone WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPhone VALUES(UPPER('" + acctNo + "'), '" + contactPhone + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Preference *********
                    sb.Append("DELETE ClientPrefer WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPrefer(AcctNo, PreferA, PreferB, ModifiedUser, ModifiedDate) " +
                            " VALUES(UPPER('" + acctNo + "'), '" + preferA + "', '" + preferB + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous ShortKey and Insert New one *********
                    /*
                    if (!String.IsNullOrEmpty(shortKey))
                    {
                        sb.Append("DELETE ShortKey WHERE UserID='" + userId + "' AND (AcctNo='" + acctNo + "' OR ShortKey='" + shortKey + "');");
                        sb.Append("INSERT INTO ShortKey VALUES('" + userId + "', '" + shortKey + "', '" + acctNo + "');");
                    }
                    */

                    cmd.CommandText = sb.ToString();
                    result = cmd.ExecuteNonQuery();

                    sqlTrans.Commit();
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

        //ContactDate, AcctNo, ContactPhone, Content, Remarks, Rank, ModifiedUser, ModifiedDate, Keep
        public int UpdateClientContact(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId,  int followup, string followUpDealer, string followUpDate,string projectID)
        {
            int result = -1, insertedId = 0;
            OleDbParameter[] oledbParams = null;
            string followUpStatus = "N";

            sql = " UPDATE SPM.dbo.ClientContact  SET ContactPhone=?, Rank=?, " +
                        " ModifiedUser=?, Keep=?, ModifiedDate=GETDATE(), AdminId=?,FollowUpStatus=?,FollowUpBy=?,FollowUpDate=? WHERE RecId=? ";

            oledbParams = new OleDbParameter[] 
                                    {                                              
                                        new OleDbParameter("@contactPhone", OleDbType.VarChar),
                                        //new OleDbParameter("@content", OleDbType.VarChar),
                                        //new OleDbParameter("@remark", OleDbType.VarChar),
                                        new OleDbParameter("@rank", OleDbType.VarChar), 
                                        new OleDbParameter("@dealerCode", OleDbType.VarChar),
                                        new OleDbParameter("@keep", OleDbType.VarChar),
                                        new OleDbParameter("@adminId", OleDbType.VarChar),
                                        new OleDbParameter("@FollowUpStatus", OleDbType.VarChar),
                                        new OleDbParameter("@FollowUpBy", OleDbType.VarChar),
                                        new OleDbParameter("@FollowUpDate", OleDbType.Date),
                                        new OleDbParameter("@recId", OleDbType.VarChar),
                                      
                                    };
            
            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            StringBuilder sb = new StringBuilder("");            

            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;
                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);

                

                cmd.Parameters["@contactPhone"].Value = contactPhone;
                //cmd.Parameters["@content"].Value = content;
                //cmd.Parameters["@remark"].Value = remark;
                cmd.Parameters["@rank"].Value = rank;
                cmd.Parameters["@dealerCode"].Value = dealerCode;
                cmd.Parameters["@keep"].Value = keep;
                cmd.Parameters["@adminId"].Value = adminId;
                cmd.Parameters["@FollowUpStatus"].Value = followUpStatus;
                if (followup == 2)
                {
                    followUpStatus = "Y";

                    /// <Added by Thet Maung Chaw>
                    //cmd.Parameters["@FollowUpDate"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");

                    cmd.Parameters["@FollowUpBy"].Value = followUpDealer;
                }
                else if (followup == 1)
                {
                    followUpStatus = "N";
                    cmd.Parameters["@FollowUpBy"].Value = followUpDealer;
                    cmd.Parameters["@FollowUpDate"].Value = DateTime.ParseExact(followUpDate, "dd/MM/yyyy", null);
                }
                else
                {
                    /// <Updated by Thet Maung Chaw>
                    //cmd.Parameters["@FollowUpBy"].Value = null;
                    //cmd.Parameters["@FollowUpDate"].Value = null;
                    cmd.Parameters["@FollowUpBy"].Value = followUpDealer;
                    //cmd.Parameters["@FollowUpDate"].Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
                }                
                cmd.Parameters["@recId"].Value = recId;

                insertedId = cmd.ExecuteNonQuery();              


                if (insertedId > 0)             //if (result > 0) for Get InsertedId with Separate Command
                {
                    
                    //'************** Delete Previous Client Sex and Insert New one *********
                    sb.Append("DELETE ClientSex WHERE AcctNo='" + acctNo + "';");
                    if (!String.IsNullOrEmpty(sex))
                        sb.Append("INSERT INTO ClientSex VALUES(UPPER('" + acctNo + "'), '" + sex + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Contact and Insert New one *********
                    sb.Append("DELETE ClientPhone WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPhone VALUES(UPPER('" + acctNo + "'), '" + contactPhone + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Preference *********
                    sb.Append("DELETE ClientPrefer WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPrefer(AcctNo, PreferA, PreferB, ModifiedUser, ModifiedDate) " +
                            " VALUES(UPPER('" + acctNo + "'), '" + preferA + "', '" + preferB + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous ShortKey and Insert New one *********
                    /*
                    if (!String.IsNullOrEmpty(shortKey))
                    {
                        sb.Append("DELETE ShortKey WHERE UserID='" + userId + "' AND (AcctNo='" + acctNo + "' OR ShortKey='" + shortKey + "');");
                        sb.Append("INSERT INTO ShortKey VALUES('" + userId + "', '" + shortKey + "', '" + acctNo + "');");
                    }
                    */

                    cmd.CommandText = sb.ToString();
                    result = cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
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

        /// <Added by Thet Maung Chaw>
        public int UpdateClientContact_OnlyTheFollowUpStatus(string dealerCode, string userId, string acctNo, string shortKey, string sex, string contactPhone, string content,
                string preferA, string preferB, string remark, string rank, string keep, string adminId, string recId, int followup, string followUpDealer, string followUpDate, string projectID)
        {
            int result = -1, insertedId = 0;
            OleDbParameter[] oledbParams = null;
            string followUpStatus = "N";

            sql = " UPDATE SPM.dbo.ClientContact SET FollowUpStatus=? WHERE RecId=? ";

            oledbParams = new OleDbParameter[] 
                                    {
                                        new OleDbParameter("@FollowUpStatus", OleDbType.VarChar),
                                        new OleDbParameter("@recId", OleDbType.VarChar)
                                    };

            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;
            StringBuilder sb = new StringBuilder("");

            try
            {
                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;

                /// <Use OleDbParameter class to insert upper comma '>
                cmd.Parameters.AddRange(oledbParams);

                if (followup == 2)
                {
                    followUpStatus = "Y";
                }
                else if (followup == 1)
                {
                    followUpStatus = "N";
                }

                cmd.Parameters["@FollowUpStatus"].Value = followUpStatus;
                cmd.Parameters["@recId"].Value = recId;

                insertedId = cmd.ExecuteNonQuery();

                if (insertedId > 0)             //if (result > 0) for Get InsertedId with Separate Command
                {

                    //'************** Delete Previous Client Sex and Insert New one *********
                    sb.Append("DELETE ClientSex WHERE AcctNo='" + acctNo + "';");
                    if (!String.IsNullOrEmpty(sex))
                        sb.Append("INSERT INTO ClientSex VALUES(UPPER('" + acctNo + "'), '" + sex + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Contact and Insert New one *********
                    sb.Append("DELETE ClientPhone WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPhone VALUES(UPPER('" + acctNo + "'), '" + contactPhone + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous Client Preference *********
                    sb.Append("DELETE ClientPrefer WHERE AcctNo='" + acctNo + "';");
                    sb.Append("INSERT INTO ClientPrefer(AcctNo, PreferA, PreferB, ModifiedUser, ModifiedDate) " +
                            " VALUES(UPPER('" + acctNo + "'), '" + preferA + "', '" + preferB + "', '" + userId + "', GETDATE());");

                    //'************** Delete Previous ShortKey and Insert New one *********
                    /*
                    if (!String.IsNullOrEmpty(shortKey))
                    {
                        sb.Append("DELETE ShortKey WHERE UserID='" + userId + "' AND (AcctNo='" + acctNo + "' OR ShortKey='" + shortKey + "');");
                        sb.Append("INSERT INTO ShortKey VALUES('" + userId + "', '" + shortKey + "', '" + acctNo + "');");
                    }
                    */

                    cmd.CommandText = sb.ToString();
                    result = cmd.ExecuteNonQuery();
                    sqlTrans.Commit();
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

        internal int UpdateClientContactFollowUpStatus(String recId, String followUpStatus)
        {
            int result = -1;
            OleDbParameter[] oledbParams = null;
            OleDbTransaction sqlTrans = null;
            OleDbCommand cmd = null;

            try
            {
                sql = " UPDATE SPM.dbo.ClientContact  SET FollowUpStatus=? " +
                       " WHERE RecId=? ";
                oledbParams = new OleDbParameter[] 
                                    {
                                        new OleDbParameter("@FollowUpStatus", OleDbType.VarChar),
                                        new OleDbParameter("@RecId", OleDbType.VarChar)
                                    };

                sqlTrans = genericDA.GetSqlConnection().BeginTransaction();
                cmd = genericDA.GetSqlCommand();
                cmd.Transaction = sqlTrans;
                cmd.CommandText = sql;

                //Use OleDbParameter class to insert upper comma '
                cmd.Parameters.AddRange(oledbParams);
                cmd.Parameters["@FollowUpStatus"].Value = followUpStatus;
                cmd.Parameters["@RecId"].Value = recId;
                result = cmd.ExecuteNonQuery();
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

        public int DeleteClientContact(string recId)
        {
            sql = "DELETE FROM ClientContact WHERE RecId = '" + recId + "'";

            return genericDA.ExecuteNonQuery(sql);
        }

        public DataTable RetrieveUnContactedAssignment(string dealerCode, String UserID)
        {
            //Changes 27 April 2010 => change retrieval of client name from TmpClientAssign to CLMAST
            //TCA.ClientName

            //Changes for Sorting
            //CONVERT(VARCHAR(10), CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate
            sql = "SELECT DISTINCT CLA.AcctNo,CLM.NRIC, CLA.AECode, CLA.AssignDate, CLA.CutOffDate, " +
                    "CCT.ModifiedUser, CONVERT(VARCHAR, CCT.ContactDate, 120) AS ContactDate, CCT.Rank, CCT.ModifiedUser AS DealerCode, CS.Sex, " +
                    "CLM.LTEL, CLM.LMOBILE, CLM.LOFFTEL, CLM.LFAX, --CLM.LEMAIL,\r\n" +
                    "CONVERT(VARCHAR(10), CLTD.OverallLTD, 103) AS OverallLTD, CT.TotalCall, " +
                    "CLM.LNAME AS ClientName, (-TCA.out_Bal) AS out_Bal, TCA.Market_vl, TCA.Status AS ClientStatus, CP.Phone, CC.AECode AS CoreDealer, " +
                    "CPR.PreferA, CPR.PreferB, SK.ShortKey, DD.AEGroup AS DealerTeam ,'' AS AccServiceType,--ISNULL(CLM.AccServiceType,'') As AccServiceType,\r\n ISNULL(CLA.ProjectID,'') As ProjectID " +
                    "FROM ClientAssign CLA " +
                    "INNER JOIN " +             // JOIN for retrieve lastest Client Assignment record
                    "( " +
                    "   SELECT AcctNo, MAX(AssignDate) AS MADate  " +
                    "   FROM ClientAssign WITH (NOLOCK) " +
                    "   WHERE AECode IN ('" + dealerCode + "')" + "\r\n" +
                    "--WHERE  AECode IN (SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "'))" + "\r\n" +
                    "   AND CutOffDate >= GETDATE() " +             //New added at 27 April 2010 for checking with CutOff Date, to retrieve latest assignment only.
                    "   GROUP BY AcctNo " +
                    ") MCLA ON CLA.AssignDate = MCLA.MADate AND CLA.AcctNo = MCLA.AcctNo AND CLA.AECode IN ('" + dealerCode + "') " + "\r\n" +
                    "--CLA.AECode IN (SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "'))" + "\r\n" +
                    "LEFT JOIN " +		// JOIN for retrieve lastest Client Contact record
                    "( " +
                    "    SELECT CCT1.AcctNo, CCT1.ModifiedUser, CCT1.ContactDate, CCT1.Rank " +
                    "    FROM ClientContact CCT1 " +
                    "    INNER JOIN " +
                    "    ( " +
                    "        SELECT AcctNo, MAX(ContactDate) AS MADate  " +
                    "        FROM ClientContact WITH (NOLOCK)  " +
                    "        GROUP BY AcctNo " +
                    "    ) MCCT ON CCT1.AcctNo = MCCT.AcctNo AND CCT1.ContactDate = MCCT.MADate " +
                    ") CCT ON CLA.AcctNo = CCT.AcctNo " +
                    "LEFT JOIN ClientSex CS WITH (NOLOCK)  ON CLA.AcctNo = CS.AcctNo " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON CLA.AcctNo = CLM.LACCT " +
                    "LEFT JOIN ClientLTD CLTD WITH (NOLOCK) ON CLA.AcctNo=CLTD.AcctNo " +
                    "LEFT JOIN ClientTotal CT WITH (NOLOCK) ON CLA.AcctNo=CT.AcctNo " +
                    "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLA.AcctNo = TCA.AcctNo AND cla.ModifiedUser = tca.AECode " +
                    "LEFT JOIN CoreClient CC WITH (NOLOCK) ON CLA.AcctNo = CC.AcctNo " +
                //"LEFT JOIN DealerDetail DD WITH(NOLOCK) ON CC.AECode = DD.AECode " +        //For Core Dealer Team (currently no need)
                    "LEFT JOIN DealerDetail DD WITH(NOLOCK) ON CLA.AECode = DD.AECode " +
                    "LEFT JOIN ClientPrefer CPR WITH (NOLOCK) ON CLA.AcctNo = CPR.AcctNo " +
                    "LEFT JOIN ShortKey SK WITH (NOLOCK) ON CLA.AcctNo = SK.AcctNo " +
                    "LEFT JOIN ClientPhone CP WITH (NOLOCK) ON CLA.AcctNo = CP.AcctNo " +
                    "WHERE CCT.ContactDate < CLA.AssignDate OR CCT.ContactDate IS NULL ORDER BY CLA.AcctNo";      //WHERE CCT.ContactDate IS NULL

            return genericDA.ExecuteQueryForDataTable(sql, "dtUnContactedAssignment");
        }

        /// <Add new by OC>
        public DataTable RetrieveByAccountNo(String AccountNo, String Param)
        {
            String SQL = String.Empty;

            SQL = "SELECT" + "\r\n";
            SQL += "    ClientAssign.AcctNo, ClientContact.ContactPhone, ClientSex.Sex, CLMAST.LNAME, CLMAST.AccServiceType, ClientContact.[Rank]," + "\r\n";
            SQL += "    ShortKey.ShortKey, ClientPrefer.PreferA, ClientPrefer.PreferB" + "\r\n";
            SQL += "FROM" + "\r\n";
            SQL += "    ClientAssign" + "\r\n";
            SQL += "    LEFT JOIN CLMAST ON ClientAssign.AcctNo = CLMAST.LACCT" + "\r\n";
            SQL += "    LEFT JOIN ClientPrefer ON ClientAssign.AcctNo = ClientPrefer.AcctNo" + "\r\n";
            SQL += "    LEFT JOIN ClientSex ON ClientAssign.AcctNo = ClientSex.AcctNo" + "\r\n";
            SQL += "    LEFT JOIN ClientContact ON ClientAssign.AcctNo = ClientContact.AcctNo" + "\r\n";
            SQL += "    LEFT JOIN ShortKey ON ClientAssign.AcctNo = ShortKey.AcctNo" + "\r\n";
            SQL += "    INNER JOIN DealerDetail ON ClientAssign.AECode = DealerDetail.AECode" + "\r\n";
            SQL += "WHERE" + "\r\n";
            SQL += "    " + Param + "\r\n";
            SQL += "    DealerDetail.UserType NOT LIKE 'FAR%'" + "\r\n";
            SQL += "    AND ClientAssign.AcctNo = '" + AccountNo + "'";

            return genericDA.ExecuteQueryForDataTable(SQL, "dtIndividual");
        }

        public DataTable RetrieveUnContactedAssignmentByProjectID(string dealerCode)
        {
            sql = "SELECT DISTINCT CLA.AcctNo, CLA.AECode, CLA.AssignDate, CLA.CutOffDate, " +
                    "CCT.ModifiedUser, CONVERT(VARCHAR, CCT.ContactDate, 120) AS ContactDate, CCT.Rank, CCT.ModifiedUser AS DealerCode, CS.Sex, " +
                    "CLM.LTEL, CLM.LMOBILE, CLM.LOFFTEL, CLM.LFAX, '' AS LEMAIL,--CLM.LEMAIL,\r\n " +
                    "CONVERT(VARCHAR(10), CLTD.OverallLTD, 103) AS OverallLTD, CT.TotalCall, " +
                    "CLM.LNAME AS ClientName, (-TCA.out_Bal) AS out_Bal, TCA.Market_vl, TCA.Status AS ClientStatus, CP.Phone, CC.AECode AS CoreDealer, " +
                    "CPR.PreferA, CPR.PreferB, SK.ShortKey, DD.AEGroup AS DealerTeam ,'' AS AccServiceType,--CLM.AccServiceType,\r\nCLA.ProjectID " +
                    "FROM ClientAssign CLA " +
                    "INNER JOIN " +             // JOIN for retrieve lastest Client Assignment record
                    "( " +
                    "   SELECT AcctNo, MAX(AssignDate) AS MADate  " +
                    "   FROM ClientAssign WITH (NOLOCK) " +
                    "   WHERE ProjectID = '" + dealerCode + "'" +
                    "   AND CutOffDate >= GETDATE() " +             //New added at 27 April 2010 for checking with CutOff Date, to retrieve latest assignment only.
                    "   GROUP BY AcctNo " +
                    ") MCLA ON CLA.AssignDate = MCLA.MADate AND CLA.AcctNo = MCLA.AcctNo AND CLA.ProjectID = '" + dealerCode + "' " +
                    "LEFT JOIN " +		// JOIN for retrieve lastest Client Contact record
                    "( " +
                    "    SELECT CCT1.AcctNo, CCT1.ModifiedUser, CCT1.ContactDate, CCT1.Rank " +
                    "    FROM ClientContact CCT1 " +
                    "    INNER JOIN " +
                    "    ( " +
                    "        SELECT AcctNo, MAX(ContactDate) AS MADate  " +
                    "        FROM ClientContact WITH (NOLOCK)  " +
                    "        GROUP BY AcctNo " +
                    "    ) MCCT ON CCT1.AcctNo = MCCT.AcctNo AND CCT1.ContactDate = MCCT.MADate " +
                    ") CCT ON CLA.AcctNo = CCT.AcctNo " +
                    "LEFT JOIN ClientSex CS WITH (NOLOCK)  ON CLA.AcctNo = CS.AcctNo " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON CLA.AcctNo = CLM.LACCT " +
                    "LEFT JOIN ClientLTD CLTD WITH (NOLOCK) ON CLA.AcctNo=CLTD.AcctNo " +
                    "LEFT JOIN ClientTotal CT WITH (NOLOCK) ON CLA.AcctNo=CT.AcctNo " +
                    "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLA.AcctNo = TCA.AcctNo AND cla.ModifiedUser = tca.AECode " +
                    "LEFT JOIN CoreClient CC WITH (NOLOCK) ON CLA.AcctNo = CC.AcctNo " +              
                    "LEFT JOIN DealerDetail DD WITH(NOLOCK) ON CLA.AECode = DD.AECode " +
                    "LEFT JOIN ClientPrefer CPR WITH (NOLOCK) ON CLA.AcctNo = CPR.AcctNo " +
                    "LEFT JOIN ShortKey SK WITH (NOLOCK) ON CLA.AcctNo = SK.AcctNo " +
                    "LEFT JOIN ClientPhone CP WITH (NOLOCK) ON CLA.AcctNo = CP.AcctNo " +
                    "WHERE CCT.ContactDate < CLA.AssignDate OR CCT.ContactDate IS NULL ORDER BY CLA.AcctNo";      //WHERE CCT.ContactDate IS NULL

            return genericDA.ExecuteQueryForDataTable(sql, "dtUnContactedAssignment");
        }

        public DataTable RetrieveClientInfoByShortKey(string shortKey)
        {
            sql = "SELECT SK.AcctNo, SK.ShortKey, CLM.LNAME, ISNULL(CP.Phone, '') AS Phone, ISNULL(CS.Sex, '') AS Sex, " +
                    "ISNULL(CPR.PreferA, '') AS PreferA , ISNULL(CPR.PreferB, '') AS PreferB,CLM.AccServiceType  " +
                    "FROM ShortKey SK WITH (NOLOCK) " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON SK.AcctNo = CLM.LACCT " +
                    "LEFT JOIN ClientPhone CP WITH (NOLOCK) ON SK.AcctNo = CP.AcctNo " +
                    "LEFT JOIN ClientSex CS WITH (NOLOCK) ON SK.AcctNo = CS.AcctNo " +
                    "LEFT JOIN ClientPrefer CPR WITH (NOLOCK) ON SK.AcctNo = CPR.AcctNo " +
                    "WHERE SK.ShortKey = '" + shortKey + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable getContactHistoryByAccountNo(string accountNo)
        {
            //sql = GetContactHistorySQL();
            //sql = sql + " WHERE CCT.AcctNo = '" + accountNo + "' ORDER BY ContactDate DESC";

            StringBuilder sb = new StringBuilder(GetContactHistorySQL("History"));

            sql = "INNER JOIN " +     //For Extra Call
                    "( " +
                    "    SELECT CLA.AssignDate, CCT1.AcctNo, CCT1.ModifiedUser, CCT1.ContactDate " +
                    "    FROM ClientContact CCT1 " +
                    "    LEFT JOIN ClientAssign CLA WITH (NOLOCK) ON CLA.AcctNo = CCT1.AcctNo " +
                    "    AND CLA.AECode = CCT1.ModifiedUser	" +
                    "    AND CCT1.ContactDate BETWEEN CLA.AssignDate AND CLA.CutOffDate " +
                    "    WHERE CCT1.AcctNo = '" + accountNo + "' " +
                    ") CLA2 ON CLA2.ContactDate = CCT.ContactDate ";

            sb.Append(sql);
            sb.Append(" WHERE CCT.AcctNo = '").Append(accountNo).Append("' ORDER BY CCT.ContactDate DESC");

            return genericDA.ExecuteQueryForDataTable(sb.ToString(), "dtContactHistory");
        }
        //old sql for RetrieveContactHistoryByCriteria
       /* public DataTable RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
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

            if (!String.IsNullOrEmpty(accountNo))
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
            else if (!String.IsNullOrEmpty(dateFrom))
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
        }*/

      

        //old RetrieveCOntactAnalysis
        /*
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
        }*/

       
        
      /*   public DataTable RetrieveCallReport(string assignDate)
        {
            //Old way of calcuation of Total Assignment
            //ISNULL(MI.Miss, 0) + ISNULL(EX.Extra, 0) + ISNULL(ReTradeA, 0) + ISNULL(ReTradeE, 0) AS TotalAssign


            //Old SQL
            /*
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
                  "     SELECT CA.AECode, COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss  " +
                  "     FROM ClientAssign CA WITH (NOLOCK) LEFT JOIN ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser " +
                  "     AND CA.AcctNo=CC.AcctNo AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "     WHERE CA.AssignDate = '" + assignDate + "' " +
                  "     AND CC.ContactDate IS NULL " +
                 // "     AND GETDATE() > CA.CutOffDate " +
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
                
            //End Old SQL.

            /*
             //Based on ASP and HK IT version
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
           }  */

       
       

        public DataTable RetrieveCallReportDetail(string assignDate, string dealerCode)
        {

            //sql = "SELECT CA.AECode, CA.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +		//-- SQL to retrieve Miss Call Detail
            //            "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
            //            "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser AND CA.AcctNo=CC.AcctNo " +
            //            "AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "WHERE CA.AssignDate = '" + assignDate + "' " +
            //    //Condition for Miss Call, according to feedbacks from user, all the Assignments need to be displayed.
            //    //"AND CC.ContactDate IS NULL AND GETDATE() > CA.CutOffDate " +     
            //            "AND AECode = '" + dealerCode + "' " +
            //            "UNION " +
            //            "SELECT ModifiedUser AS AECode, AcctNo, ContactDate, AssignDate, CutOffDate " +	        // -- SQL to retrieve Extra Call Details
            //            "FROM " +
            //            "( " +
            //            "    SELECT CC.ModifiedUser, CC.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +
            //            "    FROM ClientContact CC WITH (NOLOCK) " +
            //            "    LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
            //            "    AND CC.AcctNo = CA.AcctNo " +
            //            "    AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "    WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
            //            "    AND CC.ContactDate >= '" + assignDate + "' " +
            //            ") TExtra " +
            //            "WHERE AssignDate IS NULL " +
            //            "AND ModifiedUser = '" + dealerCode + "' " +
            //            "ORDER BY ContactDate DESC ";

            sql = "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, ProjectType FROM  " +
                        "(SELECT  " +
                            "CA.AECode,  " +
                            "CA.AcctNo,  " +
                            "CL.LNAME AS Name, " +
                            "CC.ContactDate,  " +
                            "CA.AssignDate,  " +
                            "CA.CutOffDate,  " +
                            "pd.ProjectType " +
                        "FROM  " +
                            "SPM.dbo.ClientAssign CA WITH (NOLOCK)  " +
                                "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON 			 " +
                                    // commented by winms "CA.AECode=CC.ModifiedUser AND  " + 
                                    "CA.AcctNo=CC.AcctNo AND  " +
                                    "CC.FollowUpStatus != 'F' AND " +
                                    "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate  " +
                                "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
                                    "CA.AcctNo = CL.LACCT " +
                                "INNER JOIN spm..projectdetail pd WITH (nolock) " +
                                    "ON pd.ProjectID = ca.ProjectID " +
                        "WHERE CA.AssignDate = '" + assignDate + "' AND AECode = '" + dealerCode + "'  " +
                        "UNION  " +
                        "SELECT  " +
                            "ModifiedUser AS AECode,  " +
                            "AcctNo,  " +
                            "name, " +
                            "ContactDate,  " +
                            "AssignDate,  " +
                            "CutOffDate,  " +
                            "ProjectType " +
                        "FROM (      " +
                                "SELECT  " +
                                    "CC.ModifiedUser,  " +
                                    "CC.AcctNo,  " +
                                    "CL.LNAME AS name, " +
                                    "CC.ContactDate,  " +
                                    "CA.AssignDate,  " +
                                    "CA.CutOffDate,  " +
                                    "pd.ProjectType " +
                                "FROM  " +
                                    "ClientContact CC WITH (NOLOCK)      " +
                                        "LEFT JOIN ClientAssign CA WITH (NOLOCK) ON  " +
                                            // commented by winms  "CC.ModifiedUser = CA.AECode AND  " +
                                            "CC.AcctNo = CA.AcctNo AND  " +
                                            "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
                                        "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
                                            "CA.AcctNo = CL.LACCT  " +
                                        "Inner JOIN spm..ProjectDetail pd WITH (nolock) " +
                                            "ON pd.ProjectID = ca.projectid " +
                                "WHERE  " +
                                    "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                    "CC.ContactDate >= '" + assignDate + "'  " +
                            ") TExtra  " +
                        "WHERE  " +
                            "AssignDate IS NULL AND ModifiedUser = '" + dealerCode + "'  " +
                        ") ClientDetail " +
                        "UNION " +
                        "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, projectType FROM  " +
                        "(SELECT  " +
                            "LA.AECode, LA.LeadID AS AcctNo, LD.LeadName AS Name, LC.ContactDate,LA.AssignDate, LA.CutoffDate, pd.projectType " +
                        "FROM  " +
                            "SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                                "LEFT JOIN SPM.dbo.LeadContact LC WITH (NOLOCK) ON " +
                                    "LA.AECode = LC.AECode AND " +
                                    "LA.LeadID = LC.LeadID AND " +
                                     "LC.NeedFollowUp != 'F' AND " +
                                    "LC.ContactDate BETWEEN LA.AssignDate AND LA.CutoffDate " +
                                "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                                    "LA.LeadID = LD.LeadID " +
                                "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                                    "ON pd.ProjectID = la.ProjectID " +
                        "WHERE LA.AssignDate = '" + assignDate + "' AND LA.AECode = '" + dealerCode + "' " +
                        "UNION " +
                        "SELECT  " +
                            "AECode AS AECode,  " +
                            "LeadID AS AcctNo,  " +
                            "LeadName AS Name, " +
                            "ContactDate,  " +
                            "AssignDate,  " +
                            "CutOffDate,  " +
                            "projectType " +
                        "FROM (      " +
                                "SELECT  " +
                                    "CC.AECode,  " +
                                    "CC.LeadID,  " +
                                    "LD.LeadName, " +
                                    "CC.ContactDate,  " +
                                    "CA.AssignDate,  " +
                                    "CA.CutOffDate,      " +
                                    "pd.projecttype " +
                                "FROM  " +
                                    "LeadContact CC WITH (NOLOCK)      " +
                                        "LEFT JOIN LeadAssign CA WITH (NOLOCK) ON  " +
                                            "CC.AECode = CA.AECode AND  " +
                                            "CC.LeadID = CA.LeadID AND  " +
                                            "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
                                        "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                                            "LD.LeadID = CA.LeadID " +
                                        "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                                            "ON pd.ProjectID = ca.projectid " +
                                "WHERE  " +
                                    "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                    "CC.ContactDate >= '" + assignDate + "'  " +
                            ") TExtra  " +
                        "WHERE  " +
                            "AssignDate IS NULL AND AECode = '" + dealerCode + "'  " +
                        ") LeadDetail "+

                        /// <Commented by Thet Maung Chaw for Lead Sync and added in the below again.>
                        //"ORDER BY ContactDate DESC ";

            #region This block is added by Thet Maung Chaw for Lead Sync

              " WHERE LeadDetail.AcctNo " +
                        "NOT IN (SELECT LD.LeadID " +
                             "FROM   LeadContact LC " +
                      "INNER JOIN LeadDetail LD " +
                      "        ON LC.LeadId = Ld.LeadID " +
                      "INNER JOIN CLMAST CM " +
                      "        ON ( LD.LeadNRIC = CM.NRIC " +
                      "             AND CM.NRIC <> '' ) " +
                      "            OR ( LD.LeadName = CM.LNAME ) " +
              " WHERE  EXISTS (SELECT 1 " +
                      "        FROM   ClientContact " +
                       "       WHERE  CM.LACCT = ClientContact.AcctNo " +
                        "             AND LC.ContactDate = ClientContact.ContactDate)) " +
            "UNION " +
            "SELECT CC.ModifiedUser AS AECode, " +
            "       CC.AcctNo, " +
            "       CM.LNAME, " +
            "       CC.ContactDate, " +
            "       PD.AssignDate, " +
            "       PD.CutOffDate, " +
            "       'C' " +
            "       FROM   LeadContact LC " +
            "              INNER JOIN LeadDetail LD " +
            "                      ON LC.LeadId = Ld.LeadID " +
            "              INNER JOIN CLMAST CM " +
            "                      ON ( LD.LeadNRIC = CM.NRIC " +
            "                           AND CM.NRIC <> '' ) " +
            "                          OR ( LD.LeadName = CM.LNAME ) " +
            "              INNER JOIN ClientContact CC ON CM.LACCT=CC.AcctNo AND LC.ContactDate=CC.ContactDate " +
            "              INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID " +
            "WHERE CC.ContactDate >= '" + assignDate + "' " +
            "ORDER BY ContactDate DESC ";

            #endregion

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveCallReportProjectDetail(string ProjectName, string dealerCode)
        {

            //sql = "SELECT CA.AECode, CA.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +		//-- SQL to retrieve Miss Call Detail
            //            "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
            //            "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser AND CA.AcctNo=CC.AcctNo " +
            //            "AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "WHERE CA.AssignDate = '" + assignDate + "' " +
            //    //Condition for Miss Call, according to feedbacks from user, all the Assignments need to be displayed.
            //    //"AND CC.ContactDate IS NULL AND GETDATE() > CA.CutOffDate " +     
            //            "AND AECode = '" + dealerCode + "' " +
            //            "UNION " +
            //            "SELECT ModifiedUser AS AECode, AcctNo, ContactDate, AssignDate, CutOffDate " +	        // -- SQL to retrieve Extra Call Details
            //            "FROM " +
            //            "( " +
            //            "    SELECT CC.ModifiedUser, CC.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +
            //            "    FROM ClientContact CC WITH (NOLOCK) " +
            //            "    LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
            //            "    AND CC.AcctNo = CA.AcctNo " +
            //            "    AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "    WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
            //            "    AND CC.ContactDate >= '" + assignDate + "' " +
            //            ") TExtra " +
            //            "WHERE AssignDate IS NULL " +
            //            "AND ModifiedUser = '" + dealerCode + "' " +
            //            "ORDER BY ContactDate DESC ";

            sql = "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, ProjectType FROM  " +
                        "(SELECT  " +
                            "CA.AECode,  " +
                            "CA.AcctNo,  " +
                            "CL.LNAME AS Name, " +
                            "CC.ContactDate,  " +
                            "CA.AssignDate,  " +
                            "CA.CutOffDate,  " +
                            "pd.ProjectType " +
                        "FROM  " +
                            "SPM.dbo.ClientAssign CA WITH (NOLOCK)  " +
                                "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON 			 " +
                                    "CA.AECode=CC.ModifiedUser AND \r\n " + /// <Commented by Thet Maung Chaw.  To be able to handle one user has many AECodes case.>
                                    "CA.AcctNo=CC.AcctNo AND  " +
                                    "CC.FollowUpStatus != 'F' AND " +
                                    "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate  " +
                                "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
                                    "CA.AcctNo = CL.LACCT " +
                                "INNER JOIN spm..projectdetail pd WITH (nolock) " +
                                    "ON pd.ProjectID = ca.ProjectID " +
                        "WHERE CA.ProjectID = '" + ProjectName + "' AND AECode = '" + dealerCode + "'  " +
                       "UNION  " +
                        "SELECT  " +
                            "ModifiedUser AS AECode,  " +
                            "AcctNo,  " +
                            "name, " +
                            "ContactDate,  " +
                            "AssignDate,  " +
                            "CutOffDate,  " +
                            "ProjectType " +
                        "FROM (      " +
                                "SELECT  " +
                                    "CC.ModifiedUser,  " +
                                    "CC.AcctNo,  " +
                                    "CL.LNAME AS name, " +
                                    "CC.ContactDate,  " +
                                    "CA.AssignDate,  " +
                                    "CA.CutOffDate,  " +
                                    "pd.ProjectType " +
                                "FROM  " +
                                    "ClientContact CC WITH (NOLOCK)      " +
                                        "LEFT JOIN ClientAssign CA WITH (NOLOCK) ON  " +
                                            "CC.ModifiedUser = CA.AECode AND \r\n " + /// <Commented by Thet Maung Chaw.  To be able to handle one user has many AECodes case.>
                                            "CC.AcctNo = CA.AcctNo AND  " +
                                           
                                            "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
                                        "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
                                            "CA.AcctNo = CL.LACCT  " +
                                        "Inner JOIN spm..ProjectDetail pd WITH (nolock) " +
                                            "ON pd.ProjectID = ca.projectid " +
                                "WHERE  " +
                                    "(CA.ProjectID = '" + ProjectName + "' OR CA.AssignDate IS NULL) AND  " +
                                    "CC.ContactDate >= CA.AssignDate  " +
                            ") TExtra  " +
                        "WHERE  " +
                            "AssignDate IS NULL AND ModifiedUser = '" + dealerCode + "'  " +
                       ") ClientDetail ";                   
                      //"UNION " +
                      //  "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, projectType FROM  " +
                      //  "(SELECT  " +
                      //      "LA.AECode, LA.LeadID AS AcctNo, LD.LeadName AS Name, LC.ContactDate,LA.AssignDate, LA.CutoffDate, pd.projectType " +
                      //  "FROM  " +
                      //      "SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                      //          "LEFT JOIN SPM.dbo.LeadContact LC WITH (NOLOCK) ON " +
                      //              "LA.AECode = LC.DealerCode AND " +
                      //              "LA.LeadID = LC.LeadID AND " +
                      //              "LC.ContactDate BETWEEN LA.AssignDate AND LA.CutoffDate " +
                      //          "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                      //              "LA.LeadID = LD.LeadID " +
                      //          "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                      //              "ON pd.ProjectID = la.ProjectID " +
                      //              "WHERE LA.ProjectID = '" + ProjectName + "' AND LA.AECode = '" + dealerCode + "' "+
                      // "UNION " +
                      //  "SELECT  " +
                      //      "ModifiedUser AS AECode,  " +
                      //      "LeadID AS AcctNo,  " +
                      //      "LeadName AS Name, " +
                      //      "ContactDate,  " +
                      //      "AssignDate,  " +
                      //      "CutOffDate,  " +
                      //      "projectType " +
                      //  "FROM (      " +
                      //          "SELECT  " +
                      //              "CC.ModifiedUser,  " +
                      //              "CC.LeadID,  " +
                      //              "LD.LeadName, " +
                      //              "CC.ContactDate,  " +
                      //              "CA.AssignDate,  " +
                      //              "CA.CutOffDate,      " +
                      //              "pd.projecttype " +
                      //          "FROM  " +
                      //              "LeadContact CC WITH (NOLOCK)      " +
                      //                  "LEFT JOIN LeadAssign CA WITH (NOLOCK) ON  " +
                      //                      "CC.ModifiedUser = CA.AECode AND  " +
                      //                      "CC.LeadID = CA.LeadID AND  " +
                      //                      "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
                      //                  "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                      //                      "LD.LeadID = CA.LeadID " +
                      //                  "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                      //                      "ON pd.ProjectID = ca.projectid " +
                      //          "WHERE  " +
                      //              "(CA.ProjectID = '" + ProjectName + "' OR CA.AssignDate IS NULL) AND  " +
                      //              "CC.ContactDate >= CA.AssignDate  " +
                      //      ") TExtra  " +
                      //  "WHERE  " +
                      //      "AssignDate IS NULL AND ModifiedUser = '" + dealerCode + "'  " +
                      //  ") LeadDetail " +
                      //  "ORDER BY ContactDate DESC ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveCallReportLeadProjectDetail(string ProjectName, string dealerCode)
        {

            //sql = "SELECT CA.AECode, CA.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +		//-- SQL to retrieve Miss Call Detail
            //            "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
            //            "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser AND CA.AcctNo=CC.AcctNo " +
            //            "AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "WHERE CA.AssignDate = '" + assignDate + "' " +
            //    //Condition for Miss Call, according to feedbacks from user, all the Assignments need to be displayed.
            //    //"AND CC.ContactDate IS NULL AND GETDATE() > CA.CutOffDate " +     
            //            "AND AECode = '" + dealerCode + "' " +
            //            "UNION " +
            //            "SELECT ModifiedUser AS AECode, AcctNo, ContactDate, AssignDate, CutOffDate " +	        // -- SQL to retrieve Extra Call Details
            //            "FROM " +
            //            "( " +
            //            "    SELECT CC.ModifiedUser, CC.AcctNo, CC.ContactDate, CA.AssignDate, CA.CutOffDate " +
            //            "    FROM ClientContact CC WITH (NOLOCK) " +
            //            "    LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
            //            "    AND CC.AcctNo = CA.AcctNo " +
            //            "    AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
            //            "    WHERE (CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) " +
            //            "    AND CC.ContactDate >= '" + assignDate + "' " +
            //            ") TExtra " +
            //            "WHERE AssignDate IS NULL " +
            //            "AND ModifiedUser = '" + dealerCode + "' " +
            //            "ORDER BY ContactDate DESC ";

            //sql = "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, ProjectType FROM  " +
            //            "(SELECT  " +
            //                "CA.AECode,  " +
            //                "CA.AcctNo,  " +
            //                "CL.LNAME AS Name, " +
            //                "CC.ContactDate,  " +
            //                "CA.AssignDate,  " +
            //                "CA.CutOffDate,  " +
            //                "pd.ProjectType " +
            //            "FROM  " +
            //                "SPM.dbo.ClientAssign CA WITH (NOLOCK)  " +
            //                    "LEFT JOIN SPM..ClientContact CC WITH (NOLOCK) ON 			 " +
            //                        "CA.AECode=CC.ModifiedUser AND  " +
            //                        "CA.AcctNo=CC.AcctNo AND  " +
            //                        "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate  " +
            //                    "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
            //                        "CA.AcctNo = CL.LACCT " +
            //                    "INNER JOIN spm..projectdetail pd WITH (nolock) " +
            //                        "ON pd.ProjectID = ca.ProjectID " +
            //            "WHERE CA.ProjectID = '" + ProjectName + "' AND AECode = '" + dealerCode + "'  " +
            //            "UNION  " +
            //            "SELECT  " +
            //                "ModifiedUser AS AECode,  " +
            //                "AcctNo,  " +
            //                "name, " +
            //                "ContactDate,  " +
            //                "AssignDate,  " +
            //                "CutOffDate,  " +
            //                "ProjectType " +
            //            "FROM (      " +
            //                    "SELECT  " +
            //                        "CC.ModifiedUser,  " +
            //                        "CC.AcctNo,  " +
            //                        "CL.LNAME AS name, " +
            //                        "CC.ContactDate,  " +
            //                        "CA.AssignDate,  " +
            //                        "CA.CutOffDate,  " +
            //                        "pd.ProjectType " +
            //                    "FROM  " +
            //                        "ClientContact CC WITH (NOLOCK)      " +
            //                            "LEFT JOIN ClientAssign CA WITH (NOLOCK) ON  " +
            //                                "CC.ModifiedUser = CA.AECode AND  " +
            //                                "CC.AcctNo = CA.AcctNo AND  " +
            //                                "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
            //                            "INNER JOIN SPM..CLMAST CL WITH (NOLOCK) ON " +
            //                                "CA.AcctNo = CL.LACCT  " +
            //                            "Inner JOIN spm..ProjectDetail pd WITH (nolock) " +
            //                                "ON pd.ProjectID = ca.projectid " +
            //                    "WHERE  " +
            //                        "(CA.ProjectID = '" + ProjectName + "' OR CA.AssignDate IS NULL) AND  " +
            //                        "CC.ContactDate >= CA.AssignDate  " +
            //                ") TExtra  " +
            //            "WHERE  " +
            //                "AssignDate IS NULL AND ModifiedUser = '" + dealerCode + "'  " +
            //            ") ClientDetail " +
            //            "UNION " +
            sql = "SELECT AECode, AcctNo, Name, ContactDate, AssignDate, CutOffDate, projectType FROM  " +
                        "(SELECT  " +
                            "LA.AECode, LA.LeadID AS AcctNo, LD.LeadName AS Name, LC.ContactDate,LA.AssignDate, LA.CutoffDate, pd.projectType  " +
                        "FROM  " +
                            "SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                                "LEFT JOIN SPM.dbo.LeadContact LC WITH (NOLOCK) ON " +
                                    "LA.AECode = LC.AECode AND " +
                                    "LA.LeadID = LC.LeadID AND " +
                                    "LC.NeedFollowUp != 'F' AND " +
                                    "LC.ContactDate BETWEEN LA.AssignDate AND LA.CutoffDate " +
                                    "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                                    "LA.LeadID = LD.LeadID " +
                                "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                                    "ON pd.ProjectID = la.ProjectID " +
                        "WHERE LA.ProjectID = '" + ProjectName + "' AND LA.AECode = '" + dealerCode + "'  " +
                        "UNION " +
                        "SELECT  " +
                            "ModifiedUser AS AECode,  " +
                            "LeadID AS AcctNo,  " +
                            "LeadName AS Name, " +
                            "ContactDate,  " +
                            "AssignDate,  " +
                            "CutOffDate,  " +
                            "projectType " +                           
                        "FROM (      " +
                                "SELECT  " +
                                    "CC.ModifiedUser,  " +
                                    "CC.LeadID,  " +
                                    "LD.LeadName, " +
                                    "CC.ContactDate,  " +
                                    "CA.AssignDate,  " +
                                    "CA.CutOffDate,      " +
                                    "pd.projecttype " +                                    
                                "FROM  " +
                                    "LeadContact CC WITH (NOLOCK)      " +
                                        "LEFT JOIN LeadAssign CA WITH (NOLOCK) ON  " +
                                            "CC.ModifiedUser = CA.AECode AND  " +
                                            "CC.LeadID = CA.LeadID AND  " +
                                            "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate      " +
                                        "INNER JOIN SPM..LeadDetail LD WITH (NOLOCK) ON " +
                                            "LD.LeadID = CA.LeadID " +
                                        "INNER JOIN spm..ProjectDetail pd WITH (nolock) " +
                                            "ON pd.ProjectID = ca.projectid " +
                                "WHERE  " +
                                    "(CA.ProjectID = '" + ProjectName + "' OR CA.AssignDate IS NULL) AND  " +
                                    "CC.ContactDate >= CA.AssignDate  " +                                   
                            ") TExtra  " +
                        "WHERE  " +
                            "AssignDate IS NULL AND ModifiedUser = '" + dealerCode + "'  " +
                        ") LeadDetail " +

            #region Added by Thet Maung Chaw for Lead Sync

                //"ORDER BY ContactDate DESC ";
                         "WHERE  LeadDetail.AcctNo NOT IN (SELECT LD.LeadID " +
                        "                                 FROM   LeadContact LC " +
                        "                                        INNER JOIN LeadDetail LD " +
                        "                                                ON LC.LeadId = Ld.LeadID " +
                        "                                        INNER JOIN CLMAST CM " +
                        "                                                ON ( LD.LeadNRIC = CM.NRIC " +
                        "                                                     AND CM.NRIC <> '' ) " +
                        "                                                    OR ( LD.LeadName = CM.LNAME ) " +
                         "                                WHERE  EXISTS (SELECT 1 " +
                         "                                               FROM   ClientContact " +
                         "                                               WHERE  CM.LACCT = ClientContact.AcctNo " +
                         "                                                      AND LC.ContactDate = ClientContact.ContactDate)) " +
                        "UNION " +
                        "SELECT CC.ModifiedUser AS AECode, " +
                        "       CC.AcctNo, " +
                        "       CM.LNAME, " +
                        "       CC.ContactDate, " +
                        "       PD.AssignDate, " +
                        "       PD.CutOffDate, " +
                        "       'C' " +
                        "FROM   LeadContact LC " +
                        "       INNER JOIN LeadDetail LD " +
                        "               ON LC.LeadId = Ld.LeadID " +
                        "       INNER JOIN CLMAST CM " +
                        "               ON ( LD.LeadNRIC = CM.NRIC " +
                        "                    AND CM.NRIC <> '' ) " +
                        "                   OR ( LD.LeadName = CM.LNAME ) " +
                        "       INNER JOIN ClientContact CC " +
                        "               ON CM.LACCT = CC.AcctNo " +
                        "                  AND LC.ContactDate = CC.ContactDate " +
                        "       INNER JOIN ProjectDetail PD " +
                        "               ON CC.ProjectID = PD.ProjectID " +
                        "WHERE CC.ProjectID='" + ProjectName + "' " +
                        "ORDER  BY ContactDate DESC";

            #endregion

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

        public DataTable GetClientByAcctNo(string acctNo)
        {
            sql = "SELECT LACCT, LNAME, NRIC,LEMAIL FROM CLMAST WHERE LACCT = '" + acctNo + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable GetDetailUserInformation(string acctNo)
        {
             sql = "SELECT LNAME As Name,(LADDR1) + '-' + (LADDR2) + '-' + (LADDR3) As Address, " +
             "LEMAIL AS EMAIL,LOFFTEL As OFFICENO,LTEL AS HOMENO,LMOBILE As MOBILENO,CPFInvAC,(GSA1 + GSA2) AS GSA, " +
             "Occupation,SRSAC ,Employer ,BirthDate ,EPSBankAC,NRIC ,PRStatus ,GIROBankAC ,Nationality ,MsiaAC,RefBankAC , " +
             "POEMSAC ,USOnline ,RefBank ,W8Form ,OTC ,CPFBank ,GSALink,FuturesCFD,SRSBank,EPSLink,PPC_Start, " +
             "StartDate ,GIROLink,PPC_End ,LastTransaction ,Kinetics ,RDS,AuthParty ,EConsent ,MMF, " +
             "CASE " +
             "WHEN MMF='Y' THEN (SELECT CONVERT(DECIMAL(24,2),marketvalue) AS marketvalue FROM   spm.dbo.ClientMMF WHERE  AccNo = CLMAST.LACCT AND [ClientMMF].[Currency]='SGD' AND ReportDate = (SELECT MAX(ReportDate) FROM ClientMMF)) " +
             "WHEN MMF='N' THEN CONVERT(DECIMAL(24,2),0) " +
             "END AS MMFValue,AuthPartyIC ,IncomeLevel ,networth " +
             "FROM CLMAST  " +
             "WHERE LACCT= '" + acctNo + "'";           
             return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable GetSeminorInformationByAccNo(string acctNo)
        {
            sql = "SELECT Top 5 RecId,AcctNo,LeadID,SeminarName,SeminarDate FROM SeminarRegistration WHERE AcctNo='" + acctNo + "'"; 
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataSet RetrieveContactEntryForToday(DataSet ds, string dealerCode, String UserID)
        {
            StringBuilder sb = new StringBuilder(GetContactHistorySQL("Entry"));

            //sb.Append(" AND CONVERT(VARCHAR(10), CCT.ModifiedDate, 120) = CONVERT(VARCHAR(10), GETDATE(), 120) ");

            #region Updated by Thet Maung Chaw

            //sb.Append(" WHERE CCT.ModifiedUser IN ('").Append(dealerCode).Append("') ");
            sb.Append(" WHERE CCT.ModifiedUser IN (SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "')) ");

            #endregion

            //sb.Append(" AND CCT.FollowUpStatus NOT IN ('F','Y') "); 
            sb.Append(" AND ContactDate >= ");
            sb.Append(" (SELECT ISNULL(DATEADD(HOUR, 0, MAX(AssignDate)), DATEADD(DAY, -7, GETDATE())) FROM ClientAssign WHERE AECode IN (");

            #region Updated by Thet Maung Chaw

            //sb.Append(dealerCode).Append("'))");
            sb.Append(" SELECT AECode FROM DealerDetail WHERE UserID IN (SELECT UserID FROM SuperAdmin WHERE UserID='" + UserID + "'").Append(")))");

            #endregion
            
            sb.Append(" ORDER BY CCT.ContactDate DESC ");

            return genericDA.FillDataSet(ds, "dtEntryHistory", sb.ToString());
        }

        private string GetContactHistorySQL(string contactType)
        {
            //Changes for Sorting
            //CONVERT(VARCHAR(20), CCT.ContactDate, 120) AS ContactDate
            //sql = "SELECT CCT.RecId, CCT.ContactDate, CCT.AcctNo, ISNULL(LName, '') AS CName, " +
            //        "LRNER AS AECode, ISNULL(Sex, '') AS Sex, ISNULL(CCT.ContactPhone, '') AS Phone, ISNULL(Content, '') AS Content, " +
            //        "ISNULL(Remarks, '') AS Remarks, Rank, ISNULL(Keep, 'N') AS Keep, ISNULL(CP.PreferA, '00') AS PreferA, " +
            //        "ISNULL(CP.PreferB, '00') AS PreferB, CCT.ModifiedUser, DD.AEGroup, " +
            //        "CONVERT(VARCHAR(20), CCT.ModifiedDate, 120) AS ModifiedDate " +
            //        ", RankText = CASE Rank WHEN 0 THEN 'No Rank' WHEN 1 THEN 'Excellent Relationship' WHEN 2 THEN 'Good Relationship' " +
            //        " WHEN 3 THEN 'Average Relationship' WHEN 4 THEN 'Fair Relationship' WHEN 5 THEN 'Poor Relationship' END ";
            sql = "SELECT DISTINCT CCT.RecId, CCT.ContactDate, CCT.AcctNo, ISNULL(LName, '') AS CName, " +
                    "ISNULL(Sex, '') AS Sex, ISNULL(CCT.ContactPhone, '') AS Phone, ISNULL(Content, '') AS Content, --ISNULL(SR.SeminarName,'') As SeminarName, \r\n" +
                    "--ISNULL(CCT.FollowUpStatus,'') As FollowUpStatus,\r\n" + /// <Updated by Thet Maung Chaw.  To be meaningful for the follow up status>
                    "CASE " +
                    //"WHEN CCT.FollowUpStatus='Y' THEN 'No Need' " +
                    //"WHEN CCT.FollowUpStatus='F' THEN 'Done' " +
                    //"WHEN CCT.FollowUpStatus='N' THEN 'Not Yet' " +
                    "WHEN CCT.FollowUpStatus = 'Y' AND FollowUpDate IS NULL THEN 'No Need' " +
                    //"WHEN CCT.FollowUpStatus = 'Y' AND FollowUpDate IS NOT NULL THEN 'Not Yet' " +
                    "WHEN CCT.FollowUpStatus = 'Y' AND FollowUpDate IS NOT NULL THEN 'Done' " +
                    "WHEN CCT.FollowUpStatus = 'F' AND FollowUpDate IS NOT NULL THEN 'Done' " +
                    "WHEN CCT.FollowUpStatus = 'N' AND FollowUpDate IS NOT NULL THEN 'Not Yet' " +
                    "END AS FollowUpStatus, " +
                    "ISNULL(CONVERT(VARCHAR(20), CCT.FollowUpDate, 120),'') As FollowUpDate, CCT.FollowUpBy, " +
                    "ISNULL(Remarks, '') AS Remarks, Rank, ISNULL(Keep, 'N') AS Keep, ISNULL(CP.PreferA, '00') AS PreferA, " +
                    "ISNULL(CP.PreferB, '00') AS PreferB, CCT.ModifiedUser, DD.AEGroup, " +
                    "CONVERT(VARCHAR(20), CCT.ModifiedDate, 120) AS ModifiedDate " +
                    ", RankText = CASE Rank WHEN 0 THEN 'No Rank' WHEN 1 THEN 'Excellent Relationship' WHEN 2 THEN 'Good Relationship' " +
                    " WHEN 3 THEN 'Average Relationship' WHEN 4 THEN 'Fair Relationship' WHEN 5 THEN 'Poor Relationship' END ";


            if (contactType == "History")
            {
                sql += " , CLA2.AssignDate ";
            }
            else
            {
                sql += " , '' AS AssignDate, SK.ShortKey ";
            }

            sql += " , '' AS AccServiceType,--CM.AccServiceType,\r\n DD.AECode As AECode,CM.AltAECode As AltAECode,CCT.ProjectID ";

            sql += " FROM SPM.dbo.ClientContact CCT WITH (NOLOCK) " +
                    " LEFT JOIN SPM.dbo.CLMAST CM WITH (NOLOCK) ON CCT.AcctNo=CM.LACCT " +
                    " LEFT JOIN SPM.dbo.ClientSex CS WITH (NOLOCK) ON CCT.AcctNo=CS.AcctNo " +
                    " LEFT JOIN SPM.dbo.DealerDetail DD WITH (NOLOCK) ON CCT.ModifiedUser=DD.AECode " +
                    " LEFT JOIN SPM.dbo.ClientPrefer CP WITH (NOLOCK) ON CCT.AcctNo=CP.AcctNo \r\n" +
                    " --LEFT JOIN  (select AcctNo,SeminarName from SeminarRegistration where SeminarDate in ( \r\n" +
                    " --Select Max(SeminarDate) As SeminarDate from SeminarRegistration group by AcctNo)) SR ON CCT.AcctNo=SR.AcctNo \r\n";
                    //" LEFT JOIN SPM.dbo.SeminarRegistration SR WITH (NOLOCK) ON CCT.AcctNo=SR.AcctNo ";
            if (contactType == "Entry")
            {
                sql += " LEFT JOIN SPM.dbo.ShortKey SK WITH (NOLOCK) ON CCT.AcctNo=SK.AcctNo ";
            }

            return sql;
        }


        /// <summary>
        /// Created by:		Thet Su Mon 
        /// </summary>
        /// 

        //Retrieve all callreport by searching client and lead assignment
       public DataTable RetrieveCallReport(string assignDate, String UserID)
        {
            //Old way of calcuation of Total Assignment
            //ISNULL(MI.Miss, 0) + ISNULL(EX.Extra, 0) + ISNULL(ReTradeA, 0) + ISNULL(ReTradeE, 0) AS TotalAssign
            sql = "SELECT " +
                        "Team, " +
                        "Dealer, " +
                        "AEName, " +
                        "SUM(TotalAssign) AS TotalAssign, " +
                        "SUM(Miss) AS Miss, " +
                        "SUM(Extra) AS Extra, " +
                        "SUM(ReTradeA) AS ReTradeA, " +
                        "SUM(RetradeE) AS ReTradeE, " +
                        "SUM(Score) AS Score " +
                    "FROM " +
                    "( " +
                    "SELECT " +
                        "DD.AEGroup AS Team,  " +
                        "DD.AECode AS Dealer,  " +
                        "ISNULL(DD.AEName, '-') AS AEName ,  " +
                        "TCA.TotalAssign ,  " +
                        "ISNULL(MI.Miss, 0) AS Miss, " +
                        "ISNULL(EX.Extra, 0) AS Extra,  " +
                        "ISNULL(ReTradeA, 0) AS ReTradeA,   " +
                        "ISNULL(ReTradeE, 0) AS ReTradeE,   " +
                        "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 + ISNULL(ReTradeA, 0)*2.6 + ISNULL(ReTradeE, 0)*1.6 ) AS Score   " +
                    "FROM  " +
                        "DealerDetail DD WITH (NOLOCK)   " +
                            "INNER JOIN ( " +
                             "SELECT AECode,COUNT(AcctNo) AS TotalAssign  " +
                             "FROM ClientAssign WITH (NOLOCK)  " +
                             "WHERE AssignDate = '" + assignDate + "'" +
                             "GROUP BY AECode" +
                              ") TCA ON  DD.AECode=TCA.AECode " +
                             "LEFT JOIN ( " +
                                            "SELECT  " +
                                                "CA.AECode,  " +
                                                "COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss        " +
                                            "FROM  " +
                                                "ClientAssign CA WITH (NOLOCK)  " +
                                                    "LEFT JOIN ClientContact CC WITH (NOLOCK) ON  " +
                                                        "CA.AcctNo=CC.AcctNo AND  " +
                                                        /* Commented by winms "CA.AECode=CC.ModifiedUser AND  " +*/                                                       
                                                        "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate       " +
                                            "WHERE  " +
                                                "CA.AssignDate = '" + assignDate + "' AND  " +
                                                "CC.ContactDate IS NULL       " +
                                            "GROUP BY CA.AECode  " +
                                        ") MI ON  " +
                                    "DD.AECode=MI.AECode  " +
                            "LEFT JOIN (        " +
                                            "SELECT  " +
                                                "CC.ModifiedUser,  " +
                                                "COUNT(CC.AcctNo) - COUNT(CA.AcctNo) AS Extra        " +
                                            "FROM  " +
                                                "ClientContact CC WITH (NOLOCK)        " +
                                                    "LEFT JOIN  " +
                                                        "ClientAssign CA WITH (NOLOCK) ON  " +                                                            
                                                            "CC.AcctNo = CA.AcctNo AND  " +
                                                            /* Commented by winms  "CC.ModifiedUser = CA.AECode AND  " +*/
                                                            "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate        " +
                                            "WHERE  " +
                                                "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                                "CC.ContactDate >= '" + assignDate + "'" +
                                                "AND CC.FollowUpStatus!= 'F' " +
                                            "GROUP BY CC.ModifiedUser  " +
                                        ") EX ON  " +
                                    "DD.AECode=EX.ModifiedUser    " +
                            "LEFT JOIN (         " +
                                            "SELECT  " +
                                                "COUNT(CLD1.AcctNo) AS ReTradeE,  " +
                                                "ExtraCall.ModifiedUser         " +
                                            "FROM  " +
                                                "ClientLTradeDate CLD1 WITH (NOLOCK)         " +
                                                    "INNER JOIN (                " +
                                                                    "SELECT  " +
                                                                        "ModifiedUser,  " +
                                                                        "AcctNo,  " +
                                                                        "ContactDate                " +
                                                                    "FROM                " +
                                                                        "( " +
                                                                            "SELECT  " +
                                                                                "CC.ModifiedUser,  " +
                                                                                "CC.AcctNo,  " +
                                                                                "CC.ContactDate,  " +
                                                                                "CA.AssignDate           " +
                                                                            "FROM  " +
                                                                                "ClientContact CC WITH (NOLOCK)            " +
                                                                                    "LEFT JOIN ClientAssign CA WITH (NOLOCK) ON  " +                                                                                        
                                                                                        "CC.AcctNo = CA.AcctNo AND  " +
                                                                                        /* Commented by winms "CC.ModifiedUser = CA.AECode AND  " + */
                                                                                        "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate   " +
                                                                            "WHERE  " +
                                                                                "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                                                                "CC.ContactDate >= '" + assignDate + "' " +
                                                                        ") TExtra          " +
                                                                    "WHERE  " +
                                                                        "AssignDate IS NULL        " +
                                                                    ") ExtraCall ON  " +
                                                                        "CLD1.AcctNo = ExtraCall.AcctNo  " +
                                                                        "AND CONVERT(VARCHAR(10), CLD1.CreateDate, 120) = CONVERT(VARCHAR(10), ExtraCall.ContactDate, 120)     " +
                                                        "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON  " +
                                                            "CLD1.AcctNo=TCA.AcctNo     " +
                                            "WHERE  " +
                                                "TCA.LastTradeDate > CLD1.LTradeDate     " +
                                            "GROUP BY  " +
                                                "ExtraCall.ModifiedUser  " +
                                        ") TRE ON  " +
                                            "DD.AECode = TRE.ModifiedUser  " +
                            "LEFT JOIN ( " +
                                            "SELECT  " +
                                                "COUNT(TCLD.AcctNo) AS ReTradeA,  " +
                                                "CC.ModifiedUser         " +
                                            "FROM         " +
                                                "(             " +
                                                    "SELECT  " +
                                                        "CLD1.AcctNo,  " +
                                                        "CLD1.CreateDate,  " +
                                                        "LTradeDate             " +
                                                    "FROM  " +
                                                        "ClientLTradeDate CLD1 WITH (NOLOCK)          " +
                                                            "LEFT JOIN ClientAssign CA1 WITH (NOLOCK) ON  " +
                                                                "CLD1.AcctNo = CA1.AcctNo AND  " +
                                                                "CONVERT(VARCHAR(10), CreateDate, 120) BETWEEN CA1.AssignDate AND CA1.CutOffDate     " +
                                                    "WHERE  " +
                                                        "CA1.AssignDate = '" + assignDate + "'  " +
                                                ") TCLD	        " +
                                                    "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON  " +
                                                        "TCLD.AcctNo=TCA.AcctNo         " +
                                                    "INNER JOIN ClientContact CC WITH (NOLOCK) ON  " +
                                                        "TCLD.AcctNo=CC.AcctNo AND  " +
                                                        "CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120)      " +
                                            "WHERE  " +
                                                "TCA.LastTradeDate > TCLD.LTradeDate   " +
                                            "GROUP BY  " +
                                                "CC.ModifiedUser  " +
                                    ") TRA ON  " +
                                        "DD.AECode = TRA.ModifiedUser  " +
                            "WHERE  " +
                                "ISNULL(TCA.AECode, '') <> ''  " +
                    "UNION " +
                    "SELECT  " +
                        "DD.AEGroup AS Team,  " +
                        "DD.AECode AS Dealer,  " +
                        "ISNULL(DD.AEName, '-') AS AEName, " +
                        "TCA.TotalAssign,  " +
                        "ISNULL(MI.Miss, 0) AS Miss,  " +
                        "ISNULL(EX.Extra, 0) AS Extra,  " +
                        "'' AS ReTradeA,    " +
                        "'' AS ReTradeE,    " +
                        "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 ) AS Score    " +
                    "FROM  " +
                        "DealerDetail DD WITH (NOLOCK) " +
                          "INNER JOIN    " +
                          "(   " +
                               "SELECT AECode, COUNT(LeadID) AS TotalAssign   " +
                               "FROM LeadAssign WITH (NOLOCK)    " +
                               "WHERE AssignDate = '" + assignDate + "'   " +
                               "GROUP BY AECode    " +
                          ") TCA ON DD.AECode=TCA.AECode  " +
                          "LEFT JOIN   " +
                          "(   " +
                               "SELECT LA.AECode, COUNT(LA.LeadID)-COUNT(LC.LeadID) AS Miss    " +
                               "FROM LeadAssign LA WITH (NOLOCK) LEFT JOIN LeadContact LC WITH (NOLOCK) ON LA.AECode=LC.AECode   " +
                               "AND LA.LeadID=LC.LeadID AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate   " +
                               "WHERE LA.AssignDate = '" + assignDate + "' " +
                               "AND LC.ContactDate IS NULL       " +
                               "GROUP BY LA.AECode   " +
                          ") MI ON DD.AECode=MI.AECode   " +
                          "LEFT JOIN   " +
                          "(   " +
                                "SELECT LC.AECode, COUNT(LC.LeadID) -  COUNT(LA.LeadID) AS Extra   " +
                                "FROM LeadContact LC WITH (NOLOCK)   " +
                                "LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LC.AECode = LA.AECode   " +
                                "AND LC.LeadID = LA.LeadID   " +
                                "AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate   " +
                                "WHERE (LA.AssignDate IS NOT NULL AND LA.AssignDate = '" + assignDate + "') " +
                                "AND LC.ContactDate >= LA.AssignDate   " +
                // "AND CC.FollowUpStatus!= 'F'" +
                                "GROUP BY LC.AECode   " +
                          ") EX ON DD.AECode=EX.AECode " +
                    ") CallReport  " +
                    "WHERE CallReport.Team IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') " +
                    "GROUP BY Team, Dealer, AEName ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
       public DataTable RetrieveCallReportByUserOrSupervisor(String assignDate, String Param, String UserID)
       {
           //Old way of calcuation of Total Assignment
           //ISNULL(MI.Miss, 0) + ISNULL(EX.Extra, 0) + ISNULL(ReTradeA, 0) + ISNULL(ReTradeE, 0) AS TotalAssign
           sql = "SELECT " +
                       "Team, " +
                       "Dealer, " +
                       "AEName, " +
                       "SUM(TotalAssign) AS TotalAssign, " +
                       "SUM(Miss) AS Miss, " +
                       "SUM(Extra) AS Extra, " +
                       "SUM(ReTradeA) AS ReTradeA, " +
                       "SUM(RetradeE) AS ReTradeE, " +
                       "SUM(Score) AS Score " +
                   "FROM " +
                   "( " +
                   "SELECT " +
                       "DD.AEGroup AS Team,  " +
                       "DD.AECode AS Dealer,  " +
                       "ISNULL(DD.AEName, '-') AS AEName ,  " +
                       "TCA.TotalAssign ,  " +
                       "ISNULL(MI.Miss, 0) AS Miss, " +
                       "ISNULL(EX.Extra, 0) AS Extra,  " +
                       "ISNULL(ReTradeA, 0) AS ReTradeA,   " +
                       "ISNULL(ReTradeE, 0) AS ReTradeE,   " +
                       "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 + ISNULL(ReTradeA, 0)*2.6 + ISNULL(ReTradeE, 0)*1.6 ) AS Score   " +
                   "FROM  " +
                       "DealerDetail DD WITH (NOLOCK)   " +
                           "INNER JOIN ( " +
                            "SELECT AECode,COUNT(AcctNo) AS TotalAssign  " +
                            "FROM ClientAssign WITH (NOLOCK)  " +
                            "WHERE AssignDate = '" + assignDate + "'" +
                            "GROUP BY AECode" +
                             ") TCA ON  DD.AECode=TCA.AECode " +
                            "LEFT JOIN ( " +
                                           "SELECT  " +
                                               "CA.AECode,  " +
                                               "COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss        " +
                                           "FROM  " +
                                               "ClientAssign CA WITH (NOLOCK)  " +
                                                   "LEFT JOIN ClientContact CC WITH (NOLOCK) ON  " +
                                                       "CA.AECode=CC.ModifiedUser AND  " +
                                                       "CA.AcctNo=CC.AcctNo AND  " +
                                                       "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate       " +
                                           "WHERE  " +
                                               "CA.AssignDate = '" + assignDate + "' AND  " +
                                               "CC.ContactDate IS NULL       " +
                                           "GROUP BY CA.AECode  " +
                                       ") MI ON  " +
                                   "DD.AECode=MI.AECode  " +
                           "LEFT JOIN (        " +
                                           "SELECT  " +
                                               "CC.ModifiedUser,  " +
                                               "COUNT(CC.AcctNo) - COUNT(CA.AcctNo) AS Extra        " +
                                           "FROM  " +
                                               "ClientContact CC WITH (NOLOCK)        " +
                                                   "LEFT JOIN  " +
                                                       "ClientAssign CA WITH (NOLOCK) ON  " +
                                                           "CC.ModifiedUser = CA.AECode AND  " +
                                                           "CC.AcctNo = CA.AcctNo AND  " +
                                                           "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate        " +
                                           "WHERE  " +
                                               "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                               "CC.ContactDate >= '" + assignDate + "'" +
                                               "AND CC.FollowUpStatus!= 'F' " +
                                           "GROUP BY CC.ModifiedUser  " +
                                       ") EX ON  " +
                                   "DD.AECode=EX.ModifiedUser    " +
                           "LEFT JOIN (         " +
                                           "SELECT  " +
                                               "COUNT(CLD1.AcctNo) AS ReTradeE,  " +
                                               "ExtraCall.ModifiedUser         " +
                                           "FROM  " +
                                               "ClientLTradeDate CLD1 WITH (NOLOCK)         " +
                                                   "INNER JOIN (                " +
                                                                   "SELECT  " +
                                                                       "ModifiedUser,  " +
                                                                       "AcctNo,  " +
                                                                       "ContactDate                " +
                                                                   "FROM                " +
                                                                       "( " +
                                                                           "SELECT  " +
                                                                               "CC.ModifiedUser,  " +
                                                                               "CC.AcctNo,  " +
                                                                               "CC.ContactDate,  " +
                                                                               "CA.AssignDate           " +
                                                                           "FROM  " +
                                                                               "ClientContact CC WITH (NOLOCK)            " +
                                                                                   "LEFT JOIN ClientAssign CA WITH (NOLOCK) ON  " +
                                                                                       "CC.ModifiedUser = CA.AECode AND  " +
                                                                                       "CC.AcctNo = CA.AcctNo AND  " +
                                                                                       "CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate   " +
                                                                           "WHERE  " +
                                                                               "(CA.AssignDate = '" + assignDate + "' OR CA.AssignDate IS NULL) AND  " +
                                                                               "CC.ContactDate >= '" + assignDate + "' " +
                                                                       ") TExtra          " +
                                                                   "WHERE  " +
                                                                       "AssignDate IS NULL        " +
                                                                   ") ExtraCall ON  " +
                                                                       "CLD1.AcctNo = ExtraCall.AcctNo  " +
                                                                       "AND CONVERT(VARCHAR(10), CLD1.CreateDate, 120) = CONVERT(VARCHAR(10), ExtraCall.ContactDate, 120)     " +
                                                       "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON  " +
                                                           "CLD1.AcctNo=TCA.AcctNo     " +
                                           "WHERE  " +
                                               "TCA.LastTradeDate > CLD1.LTradeDate     " +
                                           "GROUP BY  " +
                                               "ExtraCall.ModifiedUser  " +
                                       ") TRE ON  " +
                                           "DD.AECode = TRE.ModifiedUser  " +
                           "LEFT JOIN ( " +
                                           "SELECT  " +
                                               "COUNT(TCLD.AcctNo) AS ReTradeA,  " +
                                               "CC.ModifiedUser         " +
                                           "FROM         " +
                                               "(             " +
                                                   "SELECT  " +
                                                       "CLD1.AcctNo,  " +
                                                       "CLD1.CreateDate,  " +
                                                       "LTradeDate             " +
                                                   "FROM  " +
                                                       "ClientLTradeDate CLD1 WITH (NOLOCK)          " +
                                                           "LEFT JOIN ClientAssign CA1 WITH (NOLOCK) ON  " +
                                                               "CLD1.AcctNo = CA1.AcctNo AND  " +
                                                               "CONVERT(VARCHAR(10), CreateDate, 120) BETWEEN CA1.AssignDate AND CA1.CutOffDate     " +
                                                   "WHERE  " +
                                                       "CA1.AssignDate = '" + assignDate + "'  " +
                                               ") TCLD	        " +
                                                   "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON  " +
                                                       "TCLD.AcctNo=TCA.AcctNo         " +
                                                   "INNER JOIN ClientContact CC WITH (NOLOCK) ON  " +
                                                       "TCLD.AcctNo=CC.AcctNo AND  " +
                                                       "CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120)      " +
                                           "WHERE  " +
                                               "TCA.LastTradeDate > TCLD.LTradeDate   " +
                                           "GROUP BY  " +
                                               "CC.ModifiedUser  " +
                                   ") TRA ON  " +
                                       "DD.AECode = TRA.ModifiedUser  " +
                           "WHERE  " +
                               "ISNULL(TCA.AECode, '') <> ''  " +
                   "UNION " +
                   "SELECT  " +
                       "DD.AEGroup AS Team,  " +
                       "DD.AECode AS Dealer,  " +
                       "ISNULL(DD.AEName, '-') AS AEName, " +
                       "TCA.TotalAssign,  " +
                       "ISNULL(MI.Miss, 0) AS Miss,  " +
                       "ISNULL(EX.Extra, 0) AS Extra,  " +
                       "'' AS ReTradeA,    " +
                       "'' AS ReTradeE,    " +
                       "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 ) AS Score    " +
                   "FROM  " +
                       "DealerDetail DD WITH (NOLOCK) " +
                         "INNER JOIN    " +
                         "(   " +
                              "SELECT AECode, COUNT(LeadID) AS TotalAssign   " +
                              "FROM LeadAssign WITH (NOLOCK)    " +
                              "WHERE AssignDate = '" + assignDate + "'   " +
                              "GROUP BY AECode    " +
                         ") TCA ON DD.AECode=TCA.AECode  " +
                         "LEFT JOIN   " +
                         "(   " +
                              "SELECT LA.AECode, COUNT(LA.LeadID)-COUNT(LC.LeadID) AS Miss    " +
                              "FROM LeadAssign LA WITH (NOLOCK) LEFT JOIN LeadContact LC WITH (NOLOCK) ON LA.AECode=LC.AECode   " +
                              "AND LA.LeadID=LC.LeadID AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate   " +
                              "WHERE LA.AssignDate = '" + assignDate + "' " +
                              "AND LC.ContactDate IS NULL       " +
                              "GROUP BY LA.AECode   " +
                         ") MI ON DD.AECode=MI.AECode   " +
                         "LEFT JOIN   " +
                         "(   " +
                               "SELECT LC.AECode, COUNT(LC.LeadID) -  COUNT(LA.LeadID) AS Extra   " +
                               "FROM LeadContact LC WITH (NOLOCK)   " +
                               "LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LC.AECode = LA.AECode   " +
                               "AND LC.LeadID = LA.LeadID   " +
                               "AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate   " +
                               "WHERE (LA.AssignDate IS NOT NULL AND LA.AssignDate = '" + assignDate + "') " +
                               "AND LC.ContactDate >= LA.AssignDate   " +
               // "AND CC.FollowUpStatus!= 'F'" +
                               "GROUP BY LC.AECode   " +
                         ") EX ON DD.AECode=EX.AECode " +
                   ") CallReport  " +
                   "--WHERE CallReport." + Param + "\r\n" +
                   "WHERE CallReport.Team IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') " +
                   "GROUP BY Team, Dealer, AEName ";

           return genericDA.ExecuteQueryForDataTable(sql);
       } 

        //Retrieve all callreport by Client project name
     /*   public DataTable RetrieveCallReportByProject(string projID)
        {
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
                  "     WHERE ProjectID = '" + projID + "' " +
                  "     GROUP BY AECode  " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT CA.AECode, COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss  " +
                  "     FROM ClientAssign CA WITH (NOLOCK) LEFT JOIN ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser " +
                  "     AND CA.AcctNo=CC.AcctNo AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "     WHERE CA.ProjectID = '" + projID + "' " +
                  "     AND CC.ContactDate IS NULL " +
                  "     AND CC.FollowUpStatus!= 'F'" +
                //"     AND GETDATE() > CA.CutOffDate " +
                  "     GROUP BY CA.AECode " +
                  
                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT CC.ModifiedUser, COUNT(CC.AcctNo) -  COUNT(CA.AcctNo) AS Extra " +
                  "      FROM ClientContact CC WITH (NOLOCK) " +
                  "      LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
                  "      AND CC.AcctNo = CA.AcctNo " +
                  "      AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "      WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                ///"      AND CC.ContactDate >= '" + assignDate + "' " +
                  "                 AND CC.ContactDate >= CA.AssignDate " +
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
                 "                 WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                //update sql
                //"                 AND CC.ContactDate >= '" + assignDate + "' " +
                 "                 AND CC.ContactDate >= CA.AssignDate " +
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
                 "           WHERE CA1.ProjectID = '" + projID + "' " +
                 "       ) TCLD	" +
                 "       INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCLD.AcctNo=TCA.AcctNo " +
                 "       INNER JOIN ClientContact CC WITH (NOLOCK) ON TCLD.AcctNo=CC.AcctNo " +
                 "       AND CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120) " +
                 "       WHERE TCA.LastTradeDate > TCLD.LTradeDate " +
                 "       GROUP BY CC.ModifiedUser " +   // GROUP BY CC.ModifiedUser, TCLD.LTradeDate, TCA.LastTradeDate
                 ") TRA ON DD.AECode = TRA.ModifiedUser " +
                 "WHERE ISNULL(TCA.AECode, '') <> '' ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }*/
        public DataTable RetrieveCallReportByProject(string projID, String UserID)
        {
            sql = "SELECT DD.AEGroup AS Team, DD.AECode AS Dealer, ISNULL(DD.AEName, '-') AS AEName " +
                  ", TCA.TotalAssign " +      //Total Assignments for each Dealer
                  ", ISNULL(MI.Miss, 0) AS Miss," +
                  "ISNULL(EX.Extra, 0) AS Extra, ISNULL(ReTradeA, 0) AS ReTradeA,  " +
                  "ISNULL(ReTradeE, 0) AS ReTradeE,  " +
                  "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 + ISNULL(ReTradeA, 0)*2.6 + ISNULL(ReTradeE, 0)*1.6 ) AS Score , " +
                  "tca.AssignDate " +
                //"CONVERT(VARCHAR(10), TCA.AssignDate, 103) AS AssignDate"+
                  "FROM DealerDetail DD WITH (NOLOCK)  " +
                  "INNER JOIN  " +				// -- JOIN to retrieve Assignments and Dealer info to match User entered Assignment Date 
                  "( " +
                  "     SELECT AECode, COUNT(AcctNo) AS TotalAssign, CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDate" +
                  "     FROM ClientAssign WITH (NOLOCK)  " +
                  "     WHERE ProjectID = '" + projID + "' " +
                  "     GROUP BY AECode, CONVERT(VARCHAR(10), AssignDate, 103) " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT CA.AECode, COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss  " +
                  "     FROM ClientAssign CA WITH (NOLOCK) LEFT JOIN ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser " +
                  "     AND CA.AcctNo=CC.AcctNo AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " + /// <Updated by Thet Maung Chaw.  Commented the checking CA.AECode=CC.ModifiedUser.  To be able to retrieve all AECodes>
                  "     WHERE CA.ProjectID = '" + projID + "' " +
                  "     AND CC.ContactDate IS NULL " +
                //"     AND GETDATE() > CA.CutOffDate " +
                  "     GROUP BY CA.AECode " +

                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT CC.ModifiedUser, COUNT(CC.AcctNo) -  COUNT(CA.AcctNo) AS Extra " +
                  "      FROM ClientContact CC WITH (NOLOCK) " +
                  "      LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
                  "      AND CC.AcctNo = CA.AcctNo " +
                  "      AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "      WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                ///"      AND CC.ContactDate >= '" + assignDate + "' " +
                  "                 AND CC.ContactDate >= CA.AssignDate " +
                  "     AND CC.FollowUpStatus!= 'F' " +
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
                 "                 WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                //update sql
                //"                 AND CC.ContactDate >= '" + assignDate + "' " +
                 "                 AND CC.ContactDate >= CA.AssignDate " +
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
                 "           WHERE CA1.ProjectID = '" + projID + "' " +
                 "       ) TCLD	" +
                 "       INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCLD.AcctNo=TCA.AcctNo " +
                 "       INNER JOIN ClientContact CC WITH (NOLOCK) ON TCLD.AcctNo=CC.AcctNo " +
                 "       AND CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120) " +
                 "       WHERE TCA.LastTradeDate > TCLD.LTradeDate " +
                 "       GROUP BY CC.ModifiedUser " +   // GROUP BY CC.ModifiedUser, TCLD.LTradeDate, TCA.LastTradeDate
                 ") TRA ON DD.AECode = TRA.ModifiedUser " +
                 "WHERE ISNULL(TCA.AECode, '') <> '' " +
                 "AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
        public DataTable RetrieveCallReportByProjectByUserOrSupervisor(String projID, String Param)
        {
            sql = "SELECT DD.AEGroup AS Team, DD.AECode AS Dealer, ISNULL(DD.AEName, '-') AS AEName " +
                  ", TCA.TotalAssign " +      //Total Assignments for each Dealer
                  ", ISNULL(MI.Miss, 0) AS Miss," +
                  "ISNULL(EX.Extra, 0) AS Extra, ISNULL(ReTradeA, 0) AS ReTradeA,  " +
                  "ISNULL(ReTradeE, 0) AS ReTradeE,  " +
                  "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 + ISNULL(ReTradeA, 0)*2.6 + ISNULL(ReTradeE, 0)*1.6 ) AS Score , " +
                  "tca.AssignDate " +
                //"CONVERT(VARCHAR(10), TCA.AssignDate, 103) AS AssignDate"+
                  "FROM DealerDetail DD WITH (NOLOCK)  " +
                  "INNER JOIN  " +				// -- JOIN to retrieve Assignments and Dealer info to match User entered Assignment Date 
                  "( " +
                  "     SELECT AECode, COUNT(AcctNo) AS TotalAssign, CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDate" +
                  "     FROM ClientAssign WITH (NOLOCK)  " +
                  "     WHERE ProjectID = '" + projID + "' " +
                  "     GROUP BY AECode, AssignDate " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT CA.AECode, COUNT(CA.AcctNo)- COUNT(CC.AcctNo) AS Miss  " +
                  "     FROM ClientAssign CA WITH (NOLOCK) LEFT JOIN ClientContact CC WITH (NOLOCK) ON CA.AECode=CC.ModifiedUser " +
                  "     AND CA.AcctNo=CC.AcctNo AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "     WHERE CA.ProjectID = '" + projID + "' " +
                  "     AND CC.ContactDate IS NULL " +
                //"     AND GETDATE() > CA.CutOffDate " +
                  "     GROUP BY CA.AECode " +

                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT CC.ModifiedUser, COUNT(CC.AcctNo) -  COUNT(CA.AcctNo) AS Extra " +
                  "      FROM ClientContact CC WITH (NOLOCK) " +
                  "      LEFT JOIN ClientAssign CA WITH (NOLOCK) ON CC.ModifiedUser = CA.AECode " +
                  "      AND CC.AcctNo = CA.AcctNo " +
                  "      AND CC.ContactDate BETWEEN CA.AssignDate AND CA.CutOffDate " +
                  "      WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                ///"      AND CC.ContactDate >= '" + assignDate + "' " +
                  "                 AND CC.ContactDate >= CA.AssignDate " +
                  "     AND CC.FollowUpStatus!= 'F' " +
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
                 "                 WHERE (CA.ProjectID = '" + projID + "' OR CA.AssignDate IS NULL) " +
                //update sql
                //"                 AND CC.ContactDate >= '" + assignDate + "' " +
                 "                 AND CC.ContactDate >= CA.AssignDate " +
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
                 "           WHERE CA1.ProjectID = '" + projID + "' " +
                 "       ) TCLD	" +
                 "       INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCLD.AcctNo=TCA.AcctNo " +
                 "       INNER JOIN ClientContact CC WITH (NOLOCK) ON TCLD.AcctNo=CC.AcctNo " +
                 "       AND CONVERT(VARCHAR(10), TCLD.CreateDate, 120) = CONVERT(VARCHAR(10), CC.ContactDate, 120) " +
                 "       WHERE TCA.LastTradeDate > TCLD.LTradeDate " +
                 "       GROUP BY CC.ModifiedUser " +   // GROUP BY CC.ModifiedUser, TCLD.LTradeDate, TCA.LastTradeDate
                 ") TRA ON DD.AECode = TRA.ModifiedUser " +
                 "WHERE " + Param + " AND ISNULL(TCA.AECode, '') <> ''";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        //Retrieve all call report by Lead project name       
        public DataTable RetrieveCallReportByLeadProject(string projID, String UserID)
        {
            sql = "SELECT DD.AEGroup AS Team, DD.AECode AS Dealer, ISNULL(DD.AEName, '-') AS AEName " +
                  ", TCA.TotalAssign " +      //Total Assignments for each Dealer
                  ", ISNULL(MI.Miss, 0) AS Miss," +
                  " ISNULL(EX.Extra, 0) AS Extra, '' AS ReTradeA,  " +
                  "'' AS ReTradeE,  " +
                  "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 ) AS Score,  " +
                  "tca.AssignDate " +
                  "FROM DealerDetail DD WITH (NOLOCK)  " +
                  "INNER JOIN  " +				// -- JOIN to retrieve Assignments and Dealer info to match User entered Assignment Date 
                  "( " +
                  "     SELECT AECode, COUNT(LeadID) AS TotalAssign ,CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDate " +
                  "     FROM LeadAssign WITH (NOLOCK)  " +
                  "     WHERE ProjectID = '" + projID + "' " +
                  "     GROUP BY AECode , AssignDate " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT LA.AECode, COUNT(LA.LeadID)-COUNT(LC.LeadID) AS Miss  " +
                  "     FROM LeadAssign LA WITH (NOLOCK) LEFT JOIN LeadContact LC WITH (NOLOCK) ON LA.AECode=LC.AECode " +
                  "     AND LA.LeadID=LC.LeadID AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate " +
                  "     WHERE LA.ProjectID = '" + projID + "' " +
                  "     AND LC.ContactDate IS NULL " +
                //"     AND GETDATE() > LA.CutOffDate " +
                  "     GROUP BY LA.AECode " +
                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT LC.AECode, COUNT(LC.LeadID) -  COUNT(LA.LeadID) AS Extra " +
                  "      FROM LeadContact LC WITH (NOLOCK) " +
                  "      LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LC.AECode = LA.AECode " +
                  "      AND LC.LeadID = LA.LeadID " +
                  "      AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate " +
                  "      WHERE (LA.ProjectID = '" + projID + "' OR LA.AssignDate IS NULL) " +
                ///"     AND CC.ContactDate >= '" + assignDate + "' " +
                  "                 AND LC.ContactDate >= LA.AssignDate " +
                  "      GROUP BY LC.AECode " +
                  ") EX ON DD.AECode=EX.AECode WHERE DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";


            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
        public DataTable RetrieveCallReportByLeadProjectByUserOrSupervisor(String projID, String Param)
        {
            sql = "SELECT DD.AEGroup AS Team, DD.AECode AS Dealer, ISNULL(DD.AEName, '-') AS AEName " +
                  ", TCA.TotalAssign " +      //Total Assignments for each Dealer
                  ", ISNULL(MI.Miss, 0) AS Miss," +
                  " ISNULL(EX.Extra, 0) AS Extra, '' AS ReTradeA,  " +
                  "'' AS ReTradeE,  " +
                  "(ISNULL(MI.Miss, 0)*-0.6 + ISNULL(EX.Extra, 0)*1.1 ) AS Score,  " +
                  "tca.AssignDate " +
                  "FROM DealerDetail DD WITH (NOLOCK)  " +
                  "INNER JOIN  " +				// -- JOIN to retrieve Assignments and Dealer info to match User entered Assignment Date 
                  "( " +
                  "     SELECT AECode, COUNT(LeadID) AS TotalAssign ,CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDate " +
                  "     FROM LeadAssign WITH (NOLOCK)  " +
                  "     WHERE ProjectID = '" + projID + "' " +
                  "     GROUP BY AECode , AssignDate " +
                  ") TCA ON DD.AECode=TCA.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve MISS CALL COUNT
                  "( " +
                  "     SELECT LA.AECode, COUNT(LA.LeadID)-COUNT(LC.LeadID) AS Miss  " +
                  "     FROM LeadAssign LA WITH (NOLOCK) LEFT JOIN LeadContact LC WITH (NOLOCK) ON LA.AECode=LC.AECode " +
                  "     AND LA.LeadID=LC.LeadID AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate " +
                  "     WHERE LA.ProjectID = '" + projID + "' " +
                  "     AND LC.ContactDate IS NULL " +
                //"     AND GETDATE() > LA.CutOffDate " +
                  "     GROUP BY LA.AECode " +
                  ") MI ON DD.AECode=MI.AECode " +
                  "LEFT JOIN " +				//-- JOIN to retrieve EXTRA CALL COUNT
                  "( " +
                  "      SELECT LC.AECode, COUNT(LC.LeadID) -  COUNT(LA.LeadID) AS Extra " +
                  "      FROM LeadContact LC WITH (NOLOCK) " +
                  "      LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LC.AECode = LA.AECode " +
                  "      AND LC.LeadID = LA.LeadID " +
                  "      AND LC.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate " +
                  "      WHERE (LA.ProjectID = '" + projID + "' OR LA.AssignDate IS NULL) " +
                //"     AND CC.ContactDate >= '" + assignDate + "' " +
                  "                 AND LC.ContactDate >= LA.AssignDate " +
                  "      GROUP BY LC.AECode " +
                  ") EX ON DD.AECode=EX.AECode " +
                  "WHERE " + Param;

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        //All project name
        public DataTable RetrieveProjectType(string ProjID)
        {
            string sql = "select ProjectType from ProjectDetail where ProjectID ='" + ProjID + "'  ";
            return genericDA.ExecuteQueryForDataTable(sql);
        }
       /* public DataTable RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
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

            if (!String.IsNullOrEmpty(accountNo))
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
            else if (!String.IsNullOrEmpty(dateFrom))
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

            sb.Append("UNION ALL( ");
            sb.Append("SELECT LCT.Contactdate,dd.AEgroup AS Team, LCT.LeadID AS Acctno, Isnull(Leadname, '') AS name,( '' ) AS Sex,");
            sb.Append("Isnull(LCT.Mobileno, '') AS Contactno,Isnull(Content, '') AS Content,( '' ) AS Remarks,''  AS rank, ");
            sb.Append("( '' ) AS PreferA,( '' ) AS PreferB, LCT.Modifieduser AS Dealer, LA2.Assigndate, '' AS Adminid, RankText = 'No Rank' ");
            sb.Append("FROM LEADCONTACT LCT WITH (nolock)");
            sb.Append("LEFT JOIN LEADDETAIL ld WITH (nolock) ON LCT.LeadID = ld.LeadID  ");
            sb.Append("LEFT JOIN DEALERDETAIL DD WITH (nolock) ON LCT.Modifieduser = DD.AEcode ");
            sb.Append("INNER JOIN ( ");
            sb.Append("SELECT LA.Assigndate,LCT1.LeadID,LCT1.Modifieduser,LCT1.Contactdate ");
            sb.Append("FROM LEADCONTACT LCT1 ");
            sb.Append("LEFT JOIN Leadassign LA WITH (nolock) ON LA.LeadID = LCT1.LeadID ");
            sb.Append("AND LA.aecode = LCT1.modifieduser ");
            sb.Append("AND LCT1.contactdate BETWEEN LA.Assigndate AND LA.Cutoffdate ");
            if (!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE LCT1.LeadID = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            //Checking for Contact Date
            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) <= '").Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdatee, 120) <= '").Append(dateTo).Append("' ");
                }
            }

            sb.Append(") LA2 ON LA2.contactdate = LCT.Contactdate ");
            whereFlag = false;


            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LCT.ModifiedUser = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND CONVERT(VARCHAR(10),LCT.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), LCT.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10),LCT.Contactdate, 120) <= '").Append(dateTo).Append("' ");
            }
            if (!String.IsNullOrEmpty(content))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.Content = '").Append(content).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.Content = '").Append(content).Append("' ");
                    whereFlag = true;
                }
            }
            sb.Append(")");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }*/
        public DataTable RetrieveContactHistoryByCriteria(string accountNo, string dealerCode, string dateFrom, string dateTo, string rank,
                          string preference, string content, string teamCode, String UserID)
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

            if (!String.IsNullOrEmpty(accountNo))
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
            else if (!String.IsNullOrEmpty(dateFrom))
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

            sb.Append(" AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ");

            sb.Append("UNION ALL( ");
            sb.Append("SELECT LCT.Contactdate,dd.AEgroup AS Team, LCT.LeadID AS Acctno, Isnull(Leadname, '') AS name,( '' ) AS Sex,");
            sb.Append("Isnull(LCT.Mobileno, '') AS Contactno,Isnull(Content, '') AS Content,( '' ) AS Remarks,''  AS rank, ");
            sb.Append("( '' ) AS PreferA,( '' ) AS PreferB, LCT.AECode AS Dealer, LA2.Assigndate, '' AS Adminid, RankText = 'No Rank' ");
            sb.Append("FROM LEADCONTACT LCT WITH (nolock)");
            sb.Append("LEFT JOIN LEADDETAIL ld WITH (nolock) ON LCT.LeadID = ld.LeadID  ");
            sb.Append("LEFT JOIN DEALERDETAIL DD WITH (nolock) ON LCT.AECode = DD.AEcode ");
            sb.Append("INNER JOIN ( ");
            sb.Append("SELECT LA.Assigndate,LCT1.LeadID,LCT1.AECode,LCT1.Contactdate ");
            sb.Append("FROM LEADCONTACT LCT1 ");
            sb.Append("LEFT JOIN Leadassign LA WITH (nolock) ON LA.LeadID = LCT1.LeadID ");
            sb.Append("AND LA.aecode = LCT1.AECode ");
            sb.Append("AND LCT1.contactdate BETWEEN LA.Assigndate AND LA.Cutoffdate ");
            if (!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE LCT1.LeadID = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            //Checking for Contact Date
            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '")
                        .Append(dateTo).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
                }
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CONVERT(VARCHAR(10), LCT1.Contactdate, 120) <= '").Append(dateTo).Append("' ");
                }
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT1.Contactdatee, 120) <= '").Append(dateTo).Append("' ");
                }
            }

            sb.Append(") LA2 ON LA2.contactdate = LCT.Contactdate ");
            whereFlag = false;


            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LCT.AECode = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND CONVERT(VARCHAR(10),LCT.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), LCT.Contactdate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), LCT.Contactdate, 120) >= '").Append(dateFrom).Append("' ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10),LCT.Contactdate, 120) <= '").Append(dateTo).Append("' ");
            }
            if (!String.IsNullOrEmpty(content))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.Content = '").Append(content).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.Content = '").Append(content).Append("' ");
                    whereFlag = true;
                }
            }

            sb.Append("AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')");

            #region Added by Thet Maung Chaw for Lead Sync

            /// <This query is compied from [dbo].[TS_SPM_spLeadSync] Stored Procedure.  It is the same.>
            if (whereFlag)
            {
                sb.Append(" AND LCT.LeadID NOT IN (SELECT" +
                            "		LD.LeadID" +
                            "	FROM" +
                            "		LeadContact LC" +
                            "		INNER JOIN LeadDetail LD ON LC.LeadId = Ld.LeadID" +
                            "		INNER JOIN CLMAST CM ON (LD.LeadNRIC = CM.NRIC AND CM.NRIC <> '') OR (LD.LeadName = CM.LNAME)" +
                            "   WHERE" +
                            "       EXISTS (SELECT 1 FROM ClientContact WHERE CM.LACCT=ClientContact.AcctNo AND LC.ContactDate=ClientContact.ContactDate)" +
                            "	)");
            }
            else
            {
                sb.Append(" LCT.LeadID NOT IN (SELECT" +
                                "		LD.LeadID" +
                                "	FROM" +
                                "		LeadContact LC" +
                                "		INNER JOIN LeadDetail LD ON LC.LeadId = Ld.LeadID" +
                                "		INNER JOIN CLMAST CM ON (LD.LeadNRIC = CM.NRIC AND CM.NRIC <> '') OR (LD.LeadName = CM.LNAME)" +
                                "   WHERE" +
                                "       EXISTS (SELECT 1 FROM ClientContact WHERE CM.LACCT=ClientContact.AcctNo AND LC.ContactDate=ClientContact.ContactDate)" +
                                "	)");
            }

            sb.Append(")");

            #endregion

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }
        
        
        //Retrieve contacthistory by client project
        public DataTable RetrieveContactHistoryByClientProjName(string accountNo, string dealerCode, string ProjName, string rank,
                            string preference, string content, string teamCode, String UserID)
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

            if (!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE CCT1.AcctNo = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND CLA.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE CLA.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
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

            /*if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND CONVERT(VARCHAR(10), CCT.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                else
                {
                    sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append("' ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) >= '").Append(dateFrom).Append("' ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE CONVERT(VARCHAR(10), CCT.ContactDate, 120) <= '").Append(dateTo).Append("' ");
            }*/

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND CCT.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE CCT.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
                }
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

            sb.Append(" AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        //Retrieve contacthistory by lead project
        /*public DataTable RetrieveContactHistoryByLeadProjName(string accountNo, string dealerCode, string ProjName, string rank,
                            string preference, string content, string teamCode)
        {
            bool whereFlag = false;

            sql = "SELECT LCT.ContactDate, DD.AEGroup AS Team, LCT.LeadID AS AcctNo, ISNULL(LeadName, '') AS Name, " +
                    "( '') AS Sex, ISNULL(LCT.MobileNo, '') AS ContactNo, ISNULL(Content, '') AS Content, " +
                    "( '') AS Remarks, '' as Rank, ('') AS PreferA, " +
                    "('') AS PreferB, LCT.ModifiedUser AS Dealer, LA2.AssignDate, '' AS AdminId " +
                    ", RankText = 'No Rank'" +
                    "FROM LeadContact LCT WITH (NOLOCK) " +
                    "LEFT JOIN LeadDetail LD WITH (NOLOCK) ON LCT.LeadID=LD.LeadID  " +
                    //"LEFT JOIN ClientSex CS WITH (NOLOCK) ON LCT.AcctNo=CS.AcctNo " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON LCT.ModifiedUser=DD.AECode " +
                   // "LEFT JOIN ClientPrefer CP WITH (NOLOCK) ON LCT.AcctNo=CP.AcctNo " +
                    "INNER JOIN " +     //For Extra Call
                    "( " +
                    "    SELECT LA.AssignDate, LCT1.LeadID, LCT1.ModifiedUser, LCT1.ContactDate " +
                    "    FROM LeadContact LCT1 " +
                    "    LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LA.LeadID = LCT1.LeadID " +
                    "    AND LA.AECode = LCT1.ModifiedUser	" +
                    "    AND LCT1.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate ";

            StringBuilder sb = new StringBuilder(sql);

            if (!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE LCT1.LeadID = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND LA.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE LA.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
                }
            }           

            sb.Append(") LA2 ON LA2.ContactDate = LCT.ContactDate ");
            whereFlag = false;


            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LCT.ModifiedUser = '").Append(dealerCode).Append("' ");
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
                    sb.Append(" AND LCT.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }            

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(content))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.Content = '").Append(content).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.Content = '").Append(content).Append("' ");
                    whereFlag = true;
                }
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }*/

        public DataTable RetrieveContactHistoryByLeadProjName(string accountNo, string dealerCode, string ProjName, string rank,
                           string preference, string content, string teamCode, String UserID)
        {
            bool whereFlag = false;

            sql = "SELECT LCT.ContactDate, DD.AEGroup AS Team, LCT.LeadID AS AcctNo, ISNULL(LeadName, '') AS Name, " +
                    "( '') AS Sex, ISNULL(LCT.MobileNo, '') AS ContactNo, ISNULL(Content, '') AS Content, " +
                    "( '') AS Remarks, '' as Rank, ('') AS PreferA, " +
                    "('') AS PreferB, LCT.AECode AS Dealer, LA2.AssignDate, '' AS AdminId " +
                    ", RankText = 'No Rank'" +
                    "FROM LeadContact LCT WITH (NOLOCK) " +
                    "LEFT JOIN LeadDetail LD WITH (NOLOCK) ON LCT.LeadID=LD.LeadID  " +
                //"LEFT JOIN ClientSex CS WITH (NOLOCK) ON LCT.AcctNo=CS.AcctNo " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON LCT.AECode=DD.AECode " +
                // "LEFT JOIN ClientPrefer CP WITH (NOLOCK) ON LCT.AcctNo=CP.AcctNo " +
                    "INNER JOIN " +     //For Extra Call
                    "( " +
                    "    SELECT LA.AssignDate, LCT1.LeadID, LCT1.AECode, LCT1.ContactDate " +
                    "    FROM LeadContact LCT1 " +
                    "    LEFT JOIN LeadAssign LA WITH (NOLOCK) ON LA.LeadID = LCT1.LeadID " +
                    "    AND LA.AECode = LCT1.AECode	" +
                    "    AND LCT1.ContactDate BETWEEN LA.AssignDate AND LA.CutOffDate ";

            StringBuilder sb = new StringBuilder(sql);

            if (!String.IsNullOrEmpty(accountNo))
            {
                //sql = sql + " WHERE CCT1.AcctNo = '0049718' ";

                sb.Append(" WHERE LCT1.LeadID = '").Append(accountNo).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND LA.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE LA.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
                }
            }

            sb.Append(") LA2 ON LA2.ContactDate = LCT.ContactDate ");
            whereFlag = false;


            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LCT.AECode = '").Append(dealerCode).Append("' ");
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
                    sb.Append(" AND LCT.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.ProjectID = '").Append(ProjName).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.ProjectID = '").Append(ProjName).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(content))
            {
                if (whereFlag)
                    sb.Append(" AND LCT.Content = '").Append(content).Append("' ");
                else
                {
                    sb.Append(" WHERE LCT.Content = '").Append(content).Append("' ");
                    whereFlag = true;
                }
            }

            sb.Append(" AND DD.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }
        //Retrieve contactanalysis by client and lead 
        public DataTable RetrieveContactAnalysis(string dealerCode, string accountNo, string dateFrom, string dateTo)
        {
            bool whereFlag = false;

            sql = "SELECT CONVERT(VARCHAR(20),A.ContactDate,103) AS ContactDate, A.DealerCode, A.TeamCode, A.AcctNo, A.ClientName, " +
                    "ISNULL(CONVERT(VARCHAR(20),A.LTradeDate,103),'') AS LTradeDate, " +
                    "ISNULL(CONVERT(VARCHAR(20),A.LastTradeDate,103),'') AS LastTradeDate, " +
                    "A.TotalComm FROM " +
                    "(SELECT CC.ContactDate, CC.ModifiedUser AS DealerCode, TCA.AECode AS TeamCode, CLTD.AcctNo, TCA.ClientName, \r\n" +
                    "CASE WHEN LTradeDate = '1900-01-01' THEN NULL ELSE LTradeDate END AS LTradeDate, \r\n" +
                    "CASE WHEN LastTradeDate = '1900-01-01' THEN NULL ELSE LastTradeDate END AS LastTradeDate, \r\n" +
                    "TCA.TotalComm  \r\n" +
                    "FROM  \r\n" +
                    "( " +
                    "    SELECT DISTINCT AcctNo, MIN(CreateDate) AS CreateDate, LTradeDate \r\n" +
                    "    FROM ClientLTradeDate WITH (NOLOCK) \r\n" +
                    "    GROUP BY AcctNo, LTradeDate \r\n" +
                    ") CLTD \r\n" +
                    "INNER JOIN ClientContact CC WITH (NOLOCK) ON CLTD.AcctNo=CC.AcctNo AND \r\n" +
                    "    CONVERT(VARCHAR(10), CLTD.CreateDate, 120)=CONVERT(VARCHAR(10), CC.ContactDate, 120) \r\n" +
                    "INNER JOIN TmpClientAssign TCA WITH (NOLOCK) ON CLTD.AcctNo=TCA.AcctNo \r\n" +
                    " AND TCA.LastTradeDate > CLTD.LTradeDate ";

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
                    sb.Append(" AND CC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append(" 23:59:59.999' ");
                else
                {
                    sb.Append(" WHERE CC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append(" 23:59:59.999' ");
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
            sb.Append(" union all ( ");
            sb.Append("SELECT LC.ContactDate, LC.AECode AS DealerCode, LA.AECode AS TeamCode, LC.LeadID as AcctNo, '' as Name, NULL as LTradeDate, NULL as LastTradeDate, '' as Totalcolumn ");
            sb.Append("FROM LeadContact LC ");
            sb.Append("INNER JOIN LeadAssign LA WITH (NOLOCK) ON LC.LeadID=LA.LeadID ");

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LC.AECode = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND LC.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LC.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }


            if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
            {
                if (whereFlag)
                    sb.Append(" AND LC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append(" 23:59:59.999' ) ");
                else
                {
                    sb.Append(" WHERE LC.ContactDate BETWEEN '").Append(dateFrom).Append("' AND '").Append(dateTo).Append(" 23:59:59.999' ) ");
                    whereFlag = true;
                }
            }
            else if (!String.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" WHERE LC.ContactDate >= '").Append(dateFrom).Append("'  ");
            }
            else if (!String.IsNullOrEmpty(dateTo))
            {
                sb.Append(" WHERE LC.ContactDate <= '").Append(dateTo).Append("' )");
            }

            sb.Append(" )A");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        //Retrieve contactanalysis by client project
        public DataTable RetrieveContactAnalysisByProjectName(string dealerCode, string accountNo, string ProjID)
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
            //change for the projectname
           if (!String.IsNullOrEmpty(ProjID))
                {
                   if (whereFlag)
                       sb.Append(" AND CC.ProjectID = '").Append(ProjID).Append("' ");
                   else
                   {
                       sb.Append(" WHERE CC.ProjectID = '").Append(ProjID).Append("' ");
                       whereFlag = true;
                   }
               }
            
            /*if ((!String.IsNullOrEmpty(dateFrom)) && (!String.IsNullOrEmpty(dateTo)))
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
            }*/

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }
      // Retrieve contactanalysis ny lead project
        public DataTable RetrieveContactAnalysisByLeadProjectName(string dealerCode, string accountNo, string ProjID)
        {
            bool whereFlag = false;

            sql = "SELECT LC.ContactDate, LC.AECode AS DealerCode, LA.AECode AS TeamCode, LC.LeadID as AcctNo,Ld.LeadName as ClientName, " +
                    "'' as LTradeDate, '' as LastTradeDate, '' as TotalComm " +
                    "FROM LeadContact LC " +                    
                    "INNER JOIN LeadAssign LA WITH (NOLOCK) ON LC.LeadID=LA.LeadID "+
                    "INNER JOIN LeadDetail Ld WITH (NOLOCK) ON LC.LeadID=Ld.LeadID ";     
            StringBuilder sb = new StringBuilder(sql);

           // if (!String.IsNullOrEmpty(dealerCode))
           // {
             //   sb.Append(" WHERE LC.ModifiedUser = '").Append(dealerCode).Append("' ");
            //    whereFlag = true;
           // }

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LC.AECode = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                    sb.Append(" AND LC.LeadID = '").Append(accountNo).Append("' ");
                else
                {
                    sb.Append(" WHERE LC.LeadID = '").Append(accountNo).Append("' ");
                    whereFlag = true;
                }
            }

            if (!String.IsNullOrEmpty(ProjID))
            {
                if (whereFlag)
                    sb.Append(" AND LC.ProjectID = '").Append(ProjID).Append("' ");
                else
                {
                    sb.Append(" WHERE LC.ProjectID = '").Append(ProjID).Append("' ");
                    whereFlag = true;
                }
            }           
            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }


        /// <summary>
        /// Created by:		THA 
        /// </summary>
        /// 

        public DataSet RetrieveCommissionEarnedByClientAcctNo(String acctNo)
        {
            sql = "SELECT " +
                    "CONVERT(CHAR(4), tmpClAssign.LastTradeDate, 100) + '-' + CONVERT(CHAR(4), tmpClAssign.LastTradeDate, 120) AS MonthYear, " +
                    "tmpClAssign.AccServiceType AS AcctSvcType, " +
                    "tmpClAssign.Market_vl AS MarketValue, " +
                    "tmpClAssign.TotalComm AS TotalComm " +
                "from " +
                    "TmpClientAssign tmpClAssign " +
                        "INNER JOIN AccServiceType a ON " +
                            "tmpClAssign.AccServiceType = a.AccServiceType " +
                        "INNER JOIN CLMAST clMast ON " +
			                "clMast.LACCT = tmpClAssign.AcctNo AND " +
			                "(clMast.LACCT = '" + acctNo + "' OR " +
				                "clMast.LACCT in ( " +
					                "SELECT LACCT FROM CLMAST WHERE NRIC in (SELECT NRIC FROM CLMAST WHERE LACCT = '" + acctNo + "') " +
				                ") " +
			                ") " +
                "where " +
                    "tmpClAssign.LastTradeDate between DATEADD(month,-3,tmpClAssign.LastTradeDate) and tmpClAssign.LastTradeDate " ;    
            return genericDA.ExecuteQuery(sql);
        }

        internal DataSet RetrieveCashAndEquivalentByUserAcctNo(string acctNo)
        {
            sql = "SELECT sec_name      AS name, " +
                       "mode          AS mode, " +
                       "currency      AS currency, " +
                       "qty           AS quantity, " +
                       "averageprice  AS avgprice, " +
                       "closingprice  AS closingprice, " +
                       "totalcost     AS totalcost, " +
                       "marketvalue   AS marketvalue,   " +
                       "reportdate    AS reportdate, " +
                       "port          AS port, " +
                       "unrealised_pl AS unrealisedpl, " +
                       "pl            AS profitloss " +
                "FROM   spm.dbo.ClientMMF " +
                "WHERE  AccNo IN ( '" + acctNo + "' ) " +
                "Order By sec_Name ";
                
            return genericDA.ExecuteQuery(sql);
        }

        internal DataSet RetrieveAvailableFundsByUserAcctNo(string acctNo)
        {
            sql = "SELECT out_Bal AS AvailableFunds FROM dbo.TmpClientAssign WHERE AcctNo = '" + acctNo + "'";
            return genericDA.ExecuteQuery(sql);
        }

        internal DataSet RetrieveClientServiceTypeByClientAcctNo(string acctNo)
        {
            sql = "Select " +
			            "af.AccServiceType AS AcctType, ca.AcctNo AS AcctNo,ca.AECode AS AeCode  " +
		            "from  " +
			            "ClientAssign ca  " +
				            "INNER JOIN TmpClientAssign ta ON " +
					            "ca.AcctNo = ta.AcctNo " +
				            "INNER JOIN AccServiceType af ON " +
					            "af.AccServiceType = ta.AccServiceType " +
				            "INNER JOIN CLMAST cl ON " +
					            "ca.AcctNo = cl.LACCT " +
		            "Where  " +
			            "cl.LACCT = '" + acctNo + "' OR " +
			            "cl.LACCT in ( " +
                                "SELECT LACCT FROM CLMAST WHERE NRIC in (SELECT NRIC FROM CLMAST WHERE LACCT = '" + acctNo + "') " +
				            ") ";
            return genericDA.ExecuteQuery(sql);
        }

        internal DataSet PrepareForContactToFollowUp(string userId, String UserRole)
        {
            sql = "SELECT DISTINCT cct.RecId,cla.acctno,clm.nric,cla.aecode,cla.assigndate,cla.cutoffdate,cct.modifieduser,CONVERT(VARCHAR, cct.contactdate, 120) AS contactdate,cct.rank,cct.modifieduser AS dealercode,cs.sex,clm.ltel,clm.lmobile,clm.lofftel,clm.lfax,--clm.lemail,\r\nCONVERT(VARCHAR(10), cltd.overallltd, 103) AS overallltd,ct.totalcall,clm.lname AS clientname,( -tca.out_bal ) AS out_bal,tca.market_vl,tca.status AS clientstatus,cp.phone,cc.aecode AS coredealer,cpr.prefera,cpr.preferb,sk.shortkey,dd.aegroup AS dealerteam,'' AS AccServiceType,--Isnull(clm.accservicetype, '') AS accservicetype,\r\nIsnull(cla.projectid, '') AS projectid " +
                    "FROM   clientassign cla " +
                           "INNER JOIN (SELECT acctno, MAX(assigndate) AS madate " +
                                       "FROM   clientassign WITH (nolock) " +
                                       "WHERE  --aecode = '" + userId + "' AND \r\n " + /// <Updated by Thet Maung Chaw [Remove AECode checking]>
                                              "cutoffdate >= Getdate() " +
                                       "GROUP  BY acctno) mcla " +
                             "ON cla.assigndate = mcla.madate " +
                                "AND cla.acctno = mcla.acctno " +
                                "--AND cla.aecode = '" + userId + "' \r\n" + /// <Updated by Thet Maung Chaw [Remove AECode checking]>
                           "INNER JOIN (SELECT cct1.RecId,cct1.acctno,cct1.modifieduser,cct1.contactdate,cct1.rank " +
                                      "FROM   clientcontact cct1 " +
                                             "INNER JOIN (SELECT acctno, " +
                                                                "MAX(contactdate) AS madate " +
                                                         "FROM   clientcontact WITH (nolock) " +
                                                         "GROUP  BY acctno) mcct " +
                                               "ON cct1.acctno = mcct.acctno " +
                                                  //"AND cct1.contactdate = mcct.madate " +
                                      "WHERE cct1.FollowUpStatus = 'N' " +

                            /// <Comentted and updated by Thet Maung Chaw.>
                            /// <If we search by UserID, other group under the UserID will not come out.
                            /// FollowUpBy is AECode.>
                            "--cct1.FollowUpBy = '" + userId + "' " + "\r\n";

            if (UserRole == "User")
            {
                /// < winms Updated below line by adding UserID = '" + userId + "' >
                sql += "AND cct1.FollowUpBy IN (SELECT AECode FROM DealerDetail WHERE UserID = '" + userId + "' AND AEGRoup IN((SELECT AEGroup FROM SuperAdmin WHERE UserID='" + userId + "')))" + "\r\n";
            }
            else
            {
                sql += "AND cct1.FollowUpBy = '" + userId + "' " + "\r\n";
            }
                            
                            sql+="AND cct1.FollowUpDate is not null " +
                                                  ") cct " +
                             "ON cla.acctno = cct.acctno " +
                           "LEFT JOIN clientsex cs WITH (nolock) " +
                             "ON cla.acctno = cs.acctno " +
                           "LEFT JOIN clmast clm WITH (nolock) " +
                             "ON cla.acctno = clm.lacct " +
                           "LEFT JOIN clientltd cltd WITH (nolock) " +
                             "ON cla.acctno = cltd.acctno " +
                           "LEFT JOIN clienttotal ct WITH (nolock) " +
                             "ON cla.acctno = ct.acctno " +
                           "LEFT JOIN tmpclientassign tca WITH (nolock) " +
                             "ON cla.acctno = tca.acctno AND cla.ModifiedUser = tca.AECode " +
                           "LEFT JOIN coreclient cc WITH (nolock) " +
                             "ON cla.acctno = cc.acctno " +
                           "LEFT JOIN dealerdetail dd WITH(nolock) " +
                             "ON cla.aecode = dd.aecode " +
                           "LEFT JOIN clientprefer cpr WITH (nolock) " +
                             "ON cla.acctno = cpr.acctno " +
                           "LEFT JOIN shortkey sk WITH (nolock) " +
                             "ON cla.acctno = sk.acctno " +
                           "LEFT JOIN clientphone cp WITH (nolock) " +
                             "ON cla.acctno = cp.acctno " +
                //--WHERE  cct.contactdate < cla.assigndate
                      //      --OR cct.contactdate IS NULL
                    "ORDER  BY cla.acctno ";
            return genericDA.ExecuteQuery(sql);
        }
    }
}