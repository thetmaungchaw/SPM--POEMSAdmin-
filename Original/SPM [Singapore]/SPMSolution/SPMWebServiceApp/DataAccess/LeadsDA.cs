﻿using System;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class LeadsDA
    {
        private GenericDA genericDA;
        private string sql;

        public LeadsDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveDealerByTeamCode(string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM LeadDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'  " +
                        "ORDER BY CurrentAssign, DD.AECode";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet FillLeadsByTeamCode(DataSet ds, string tableName, string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM LeadDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'";

            return genericDA.FillDataSet(ds, tableName, sql);
        }

        public DataTable RetrieveLeadsByEmailId(string emailId)
        {
            sql = "SELECT * FROM LeadsDetail WHERE LeadEmail='" + emailId + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsByDealerCode(string dealerCode)
        {
            sql = "SELECT LeadID, LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender, LeadEmail, Event, " +
                " AEGroup, AECode, CreateDate FROM LeadDetail WHERE AECode='" + dealerCode + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsByLeadID(string leadID)
        {
            sql = "SELECT LeadID, LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender, LeadEmail, Event,  " +
                " AEGroup, AECode,  CreateDate FROM LeadDetail WHERE LeadID='" + leadID + "'";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveAllLeads()
        {
            sql = "SELECT LeadID, LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender, LeadEmail, Event,  " +
                " AEGroup, AECode,  CreateDate FROM LeadDetail ORDER BY AEGroup, AECode";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveLeadsByCriteria(string leadName, string leadNRIC, string leadMobile, string leadHome, string leadGender, string leadEmail, string teamCode, string dealerCode, string dealerName)
        {
            sql = "SELECT LeadId, LeadName,LeadNRIC, LeadMobile,LeadHomeNo, LeadGender, LeadEmail, Event,  AEGroup, AECode,  CreateDate, " +
                    " AECode AS OriginalAECode,  LeadEmail AS OriginalUserID FROM LeadDetail ";

            StringBuilder sb = new StringBuilder(sql);
            bool whereFlag = false;


            if (!String.IsNullOrEmpty(leadEmail))
            {
                if (whereFlag)
                    sb.Append(" AND LeadEmail = '").Append(leadEmail).Append("' ");
                else
                    sb.Append(" WHERE LeadEmail = '").Append(leadEmail).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerCode))
            {
                if (whereFlag)
                    sb.Append(" AND AECode = '").Append(dealerCode).Append("' ");
                else
                    sb.Append(" WHERE AECode = '").Append(dealerCode).Append("' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerName))
            {
                if (whereFlag)
                    sb.Append(" AND AEName LIKE '%").Append(dealerName).Append("%' ");
                else
                    sb.Append(" WHERE AEName LIKE '%").Append(dealerName).Append("%' ");

                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(teamCode))
            {
                if (whereFlag)
                    sb.Append(" AND AEGroup = '").Append(teamCode).Append("' ");
                else
                    sb.Append(" WHERE AEGroup = '").Append(teamCode).Append("' ");

                whereFlag = true;
            }


            sb.Append(" ORDER BY AEGroup, AECode ");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public int DeleteLeads(string leadId)
        {
            int result = -1;

            OleDbCommand cmdArchive = null, cmdDelete = null;
            OleDbTransaction sqlTransaction = null;

            string sqlArchive = "IF EXISTS (SELECT LA.* FROM LeadAssign AS LA " +
            "LEFT JOIN DealerDetail AS DD ON LA.AECode = DD.AECode " +
            "WHERE DD.UserType NOT LIKE 'FAR%' AND LA.LeadID = '" + leadId + "') " +

            "BEGIN " +
                "RAISERROR('2', 16, 1, 103) " +
                "RETURN " +
            "END " + "\r\n" +

            "INSERT INTO LeadDetailArchive " +
                                "SELECT LeadID,LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender,LeadEmail,Event,AEGroup,AECode,CreateDate,InputType,GETDATE() " +
                                "FROM LeadDetail where LeadID='" + leadId + "' ";

            sql = "DELETE FROM LeadDetail WHERE LeadID = '" + leadId + "' ";

            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();


            //Create Insert command
            cmdArchive = genericDA.GetSqlCommand();
            cmdArchive.Transaction = sqlTransaction;
            cmdArchive.CommandText = sqlArchive;

            //Create Delete command
            cmdDelete = genericDA.GetNewSqlCommand();
            cmdDelete.Transaction = sqlTransaction;
            cmdDelete.CommandText = sql;


            try
            {
                result = cmdArchive.ExecuteNonQuery();

                if (result > 0)
                {
                    cmdDelete.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                result = -1;

                if (e.Message == "2")
                {
                    result = 2;
                }
            }
            finally
            {
                try
                {
                    cmdArchive.Dispose();
                    cmdDelete.Dispose();
                }
                catch (Exception ex) { }
            }

            return result;



            //============== before not to save Archive =====

            //sql = "DELETE FROM LeadDetail WHERE LeadID = '" + leadId + "'";

            //return genericDA.ExecuteNonQuery(sql);
        }



        public int InsertLeads(string LeadID, string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent, string teamCode, string dealerCode, string inputType)
        {
            //string LeadID="";
            string insertedIdStr = "";
            int result = -1;
            OleDbCommand cmd = null;

            sql = " INSERT INTO LeadDetail (LeadID, LeadName, LeadNRIC, LeadMobile, LeadHomeNo, LeadGender, LeadEmail, Event, AEGroup, AECode, InputType,CreateDate) "
                    + " VALUES('" + LeadID + "','" + LeadName + "','" + LeadNRIC + "','" + LeadMobile + "','" + LeadHome + "','" + LeadGender + "',"
                    + "LOWER('" + LeadEmail + "'),'" + LeadEvent + "',UPPER('" + teamCode + "'),UPPER('" + dealerCode + "'),'" + inputType + "', GetDate())";
            // + ";SELECT SCOPE_IDENTITY();";

            //sql = sql + ";SELECT @@IDENTITY AS InsertedID";


            cmd = genericDA.GetSqlCommand();
            cmd.CommandText = sql;
            insertedIdStr = cmd.ExecuteNonQuery().ToString();

            //insertedIdStr =  cmd.ExecuteScalar().ToString();

            if (!String.IsNullOrEmpty(insertedIdStr))
            {
                result = int.Parse(insertedIdStr);
            }

            //OleDbCommand cmdGetIdentity = genericDA.GetNewSqlCommand();
            //cmdGetIdentity.CommandText = "SELECT SCOPE_IDENTITY()";

            //result = genericDA.ExecuteNonQuery(sql);

            ////Get Inserted ID
            //if(result > 0)
            //    result = int.Parse(cmdGetIdentity.ExecuteScalar().ToString());

            //cmdGetIdentity.Dispose();

            return result;
        }

        public DataSet CheckLeadsExist(string LeadName, string LeadMobile, string LeadNRIC)
        {
            if (!String.IsNullOrEmpty(LeadNRIC))
            {
                sql = " SELECT * FROM LeadDetail WHERE LeadNRIC='" + LeadNRIC + "'";
            }
            else
            {
                sql = " SELECT * FROM LeadDetail WHERE LeadName ='" + LeadName + "' AND LeadMobile = '" + LeadMobile + "' ";
            }

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveMaxLeadsID()
        {
            sql = "SELECT Max(LeadID) AS MaxLeadID " +
                  "FROM dbo.LeadDetail ";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveExistingLeadsInfo(string LeadName, string LeadNRIC)
        {
            //sql= "SELECT * FROM LeadDetail LD LEFT JOIN LeadAssign LA ON LD.LeadID=LA.LeadID AND LD.AECode=LA.AECode " +
            //    "LEFT JOIN LeadContact LC ON LA.LeadID=LC.LeadID AND LA.AECode=LC.DealerCode WHERE "; //where LeadNRIC='My NRIC'

            sql = " SELECT A.*, A.AECode UploadedBy, A.CreateDate UploadedDate, LC.AECode LastCallDealer, LC.ContactDate LastCallDate, LA.AECode LastAssignDealer, LA.AssignDate LastAssignDate FROM  (" +
                " SELECT LD.LeadID,LeadName, LeadNRIC,LeadMobile, LeadHomeNo, LeadGender,LD.LeadEmail ,Event,LD.AEGroup,LD.AECODE,CreateDate," +
                " DD.AEGroup + ' - ' + AEName AS TeamName,AEName " +
                //" CASE WHEN LeadGender  = 'F' THEN 'Female' ELSE 'Male' END AS LGender " +
                " FROM LeadDetail LD JOIN DealerDetail DD  ON LD.AECode=DD.AECode)A " +
                " LEFT JOIN LeadAssign LA ON A.LeadID=LA.LeadID AND A.AECode=LA.AECode " +
                " LEFT JOIN LeadContact LC ON LA.LeadID=LC.LeadID AND LA.AECode=LC.AECode WHERE "; //where LeadNRIC='My NRIC' --AND UserID='cqdemo1'

            if (!String.IsNullOrEmpty(LeadNRIC))
            {
                sql = sql + " LeadNRIC='" + LeadNRIC + "'";
            }
            else
            {
                //sql = sql + " LeadName LIKE '%" + LeadName + "%' ";
                sql = sql + " LeadName='" + LeadName + "' ";
            }

            return genericDA.ExecuteQuery(sql);
        }

        public int UpdateLeads(string LeadID, string LeadName, string LeadNRIC, string LeadMobile, string LeadHome, string LeadGender, string LeadEmail,
            string LeadEvent, string teamCode, string dealerCode)
        {
            sql = " UPDATE SPM.dbo.LeadDetail SET LeadName = '" + LeadName + "', LeadNRIC = '" + LeadNRIC + "',LeadMobile='" + LeadMobile + "'," +
                    " LeadHomeNo = '" + LeadHome + "', LeadGender ='" + LeadGender + "', LeadEmail = '" + LeadEmail + "', Event = '" + LeadEvent + "'," +
                    " AEGroup = UPPER('" + teamCode + "'), " +
                    " AECode = UPPER('" + dealerCode + "'), " +
                    " CreateDate = GETDATE() " +
                    " WHERE LeadID = '" + LeadID + "'";

            return genericDA.ExecuteNonQuery(sql);
        }

        public DataTable RetrieveClientInfoByAccountNo(string AccountNo)
        {
            sql = "SELECT Distinct * FROM CLMAST WHERE LACCT='" + AccountNo + "'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public int MoveToLeadArchive(String syncType, String strCondition)
        {
            int result = -1;

            OleDbCommand cmd = null;
            OleDbTransaction sqlTransaction = null;
            String sqlLeadDetailArchive = String.Empty;

            if (syncType=="NRIC")
            {
                // Insert into LeadDetailArchive
                sqlLeadDetailArchive = "INSERT INTO LeadDetailArchive " + "\r\n";
                sqlLeadDetailArchive += "SELECT LeadID,LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender,LeadEmail,Event,AEGroup,AECode,CreateDate,InputType,GETDATE()" + "\r\n";
                sqlLeadDetailArchive += "FROM LeadDetail where LeadNRIC='" + strCondition + "'";
            }

            if (syncType=="AccNo")
            {
                // Insert into LeadDetailArchive
                sqlLeadDetailArchive = "INSERT INTO LeadDetailArchive " + "\r\n";
                sqlLeadDetailArchive += "SELECT LeadID,LeadName,LeadNRIC,LeadMobile,LeadHomeNo,LeadGender,LeadEmail,Event,AEGroup,AECode,CreateDate,InputType,GETDATE()" + "\r\n";
                sqlLeadDetailArchive += "FROM LeadDetail where LeadName='" + strCondition + "'";
            }

            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

            //Create Insert command
            cmd = genericDA.GetSqlCommand();
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = sqlLeadDetailArchive;

            try
            {
                result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    cmd.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {

                sqlTransaction.Rollback();
                result = -1;
            }
            finally
            {
                try
                {
                    cmd.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex) { }
            }

            return result;
        }


        public int LeadsDataSync(string syncType, string strCondition)
        {
            int result = 1; DataTable dtClient = null; string ClientNRIC = null, ClientName = null;


            OleDbCommand cmd = null, cmdDeleteLA = null, cmdDeleteLD = null, cmdDeleteLC = null;
            string sqlLC = "", sqlLA = "", sqlLD = "";
            OleDbTransaction sqlTransaction = null;

            if (syncType == "AccNo")
            {
                dtClient = RetrieveClientInfoByAccountNo(strCondition);

                if (dtClient == null)
                {
                    return -1;
                }
                else if (dtClient.Rows.Count == 0)
                {
                    return 0;
                }

                if (!String.IsNullOrEmpty(dtClient.Rows[0]["NRIC"].ToString()))
                {
                    ClientNRIC = dtClient.Rows[0]["NRIC"].ToString();
                }

                if (!String.IsNullOrEmpty(dtClient.Rows[0]["LNAME"].ToString()))
                {
                    ClientName = dtClient.Rows[0]["LNAME"].ToString();
                }
            }

            if (syncType == "NRIC")
            {
                ClientNRIC = strCondition;
            }
            if (!String.IsNullOrEmpty(ClientNRIC))
            {
                //Insert to Client Contact First
                sql = "INSERT INTO ClientContact " +
                      "SELECT ContactDate,LACCT,CASE " +
                                       "WHEN PreferMode='Mobile No' THEN MobileNo " +
                                       "WHEN PreferMode='Home No' THEN HomeNo " +
                                       "END AS ContactPhone,Content,'From Leads Data' As Remarks, " +
                                       "0 AS Rank,ModifiedUser,ModifiedDate,'Y' AS Keep,'' AS AdminId," +
                                       "NeedFollowUp,LC.AECode,FollowUpDate,ProjectID " + /// <Updated by OC "DealerCode" to "AECode">
                   " FROM LeadContact LC INNER JOIN LeadDetail LD ON LC.LeadId=Ld.LeadID " +
                   " INNER JOIN CLMAST CM ON (LD.LeadNRIC=CM.NRIC AND LD.LeadNRIC='" + ClientNRIC + "' AND CM.NRIC<>'') OR  LD.LeadName=CM.LNAME " +
                   " WHERE DATEDIFF(YEAR,LCRDATE,GETDATE())<25";

                // -- ------------------------------- Delete From Leads Contact After Checking AccountNo------------------------------------------ --
                sqlLC = "DELETE From LeadContact WHERE LeadId IN " +
                      "(" +
                            " SELECT leadid FROM LeadDetail LD INNER JOIN CLMAST CM " +
                            " ON (LD.LeadNRIC=CM.NRIC OR Ld.LeadName=cm.LNAME) " +
                            " WHERE LD.LeadNRIC='" + ClientNRIC + "' OR CM.NRIC='" + ClientNRIC + "' " +
                            " AND DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                        ")";

                //-- ------------------------------- Delete From Leads Assign After Checking AccountNo------------------------------------------ --

                sqlLA = "DELETE From LeadAssign WHERE LeadId IN " +
                        "(" +
                            " SELECT leadid FROM LeadDetail LD INNER JOIN CLMAST CM " +
                            " ON (LD.LeadNRIC=CM.NRIC OR Ld.LeadName=cm.LNAME) " +
                            " WHERE LD.LeadNRIC='" + ClientNRIC + "' OR CM.NRIC='" + ClientNRIC + "' " +
                            " AND DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                        ")";


                //-- ------------------------------- Delete From Leads Detail After Checking AccountNo------------------------------------------ --
                sqlLD = "DELETE From LeadDetail WHERE LeadId IN " +
                           "(" +
                                " SELECT leadid FROM LeadDetail LD INNER JOIN CLMAST CM " +
                           " ON (LD.LeadNRIC=CM.NRIC OR Ld.LeadName=cm.LNAME) " +
                           " WHERE LD.LeadNRIC='" + ClientNRIC + "' OR CM.NRIC='" + ClientNRIC + "' " +
                           " AND DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                           ")";
            }
            else if (String.IsNullOrEmpty(ClientNRIC)) //if NRIC is blank, to check with Name
            {
                //Insert to Client Contact First
                sql = "INSERT INTO ClientContact " +
                      "SELECT ContactDate,LACCT,CASE " +
                                       "WHEN PreferMode='Mobile No' THEN MobileNo " +
                                       "WHEN PreferMode='Home No' THEN HomeNo " +
                                       "END AS ContactPhone,Content,'From Leads Data' As Remarks, " +
                                       "0 AS Rank,ModifiedUser,ModifiedDate,'Y' AS Keep,'' AS AdminId," +
                                       "NeedFollowUp,LC.AECode,FollowUpDate,ProjectID " + /// <Updated by OC "DealerCode" to "AECode">
                   " FROM LeadContact LC INNER JOIN LeadDetail LD ON LC.LeadId=Ld.LeadID " +
                    //" INNER JOIN CLMAST CM ON (LD.LeadNRIC=CM.NRIC AND LD.LeadNRIC='" + strCondition + "')" +// AND CM.NRIC<>'') OR  LD.LeadName=CM.LNAME"
                   " INNER JOIN CLMAST CM ON LD.LeadName=CM.LNAME AND LeadName ='" + ClientName + "'" +
                   " WHERE DATEDIFF(YEAR,LCRDATE,GETDATE())<25";

                // -- ------------------------------- Delete From Leads Contact After Checking AccountNo------------------------------------------ --
                sqlLC = "DELETE From LeadContact WHERE LeadId IN " +
                      "(" +
                            "SELECT leadid FROM LeadDetail LD INNER JOIN CLMAST CM " +
                            " ON LD.LeadName=CM.LNAME AND LeadName ='" + ClientName + "'" +
                    //"ON (LD.LeadNRIC=CM.NRIC AND LD.LeadNRIC ='" + strCondition + "' )" +
                            " WHERE DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                        ")";

                //-- ------------------------------- Delete From Leads Assign After Checking AccountNo------------------------------------------ --

                sqlLA = "DELETE From LeadAssign WHERE LeadId IN " +
                                "(" +
                                    "SELECT leadid FROM LeadDetail LD " +
                                    "INNER JOIN CLMAST CM " +
                                    "ON LD.LeadName=CM.LNAME AND LeadName ='" + ClientName + "'" +
                    //"ON (LD.LeadNRIC=CM.NRIC AND LD.LeadNRIC='" + strCondition + "')" +// <>'' AND CM.NRIC<>'' )	 
                                    " WHERE DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                                ")";


                //-- ------------------------------- Delete From Leads Detail After Checking AccountNo------------------------------------------ --
                sqlLD = "DELETE From LeadDetail WHERE LeadId IN " +
                            "(" +
                                "SELECT leadid FROM LeadDetail LD " +
                                "INNER JOIN CLMAST CM " +
                                "ON LD.LeadName=CM.LNAME AND LeadName ='" + ClientName + "'" +
                    // "ON (LD.LeadNRIC=CM.NRIC AND LD.LeadNRIC='" + strCondition + "')" +// <>'' AND CM.NRIC<>'' )
                                " WHERE DATEDIFF(YEAR,LCRDATE,GETDATE())<25 " +
                            ")";
            }

            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

            //Create Insert command
            cmd = genericDA.GetSqlCommand();
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = sql;

            //Create Delete Lead Contact command
            cmdDeleteLC = genericDA.GetNewSqlCommand();
            cmdDeleteLC.Transaction = sqlTransaction;
            cmdDeleteLC.CommandText = sqlLC;

            //Create Delete Lead Assign command
            cmdDeleteLA = genericDA.GetNewSqlCommand();
            cmdDeleteLA.Transaction = sqlTransaction;
            cmdDeleteLA.CommandText = sqlLA;

            //Create Delete Lead Deatail command
            cmdDeleteLD = genericDA.GetNewSqlCommand();
            cmdDeleteLD.Transaction = sqlTransaction;
            cmdDeleteLD.CommandText = sqlLD;

            try
            {
                result = cmd.ExecuteNonQuery();
                result = cmdDeleteLC.ExecuteNonQuery();
                result = cmdDeleteLA.ExecuteNonQuery();
                result = cmdDeleteLD.ExecuteNonQuery();

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                result = -1;
            }
            finally
            {
                try
                {
                    cmd.Dispose();
                    cmdDeleteLC.Dispose();
                    cmdDeleteLA.Dispose();
                    cmdDeleteLD.Dispose();
                }
                catch (Exception ex) { }
            }
            //}


            return result;
        }
    }
}