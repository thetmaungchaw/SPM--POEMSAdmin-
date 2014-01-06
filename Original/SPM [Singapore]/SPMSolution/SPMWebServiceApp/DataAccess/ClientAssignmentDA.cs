/* 
 * Purpose:         Assignment Management Webservices data access layer
 * Created By:      Than Htike Tun
 * Date:            03/03/2010
 * 
 * Change History:
 * --------------------------------------------------------------------------------
 * Modified By      Date        Purpose
 * --------------------------------------------------------------------------------
 * Than Htike Tun   15/03/2010  Add in data access method for Assignment History
 * Than Htike Tun   22/03/2010  Add in data access method for Assigned Client Info
 * Than Htike Tun   04/04/2010  Add in data access method for Cross Team Assignment
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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SPMWebServiceApp.DataAccess
{
    public class ClientAssignmentDA
    {
        private string sql;
        private GenericDA genericDA;

        public ClientAssignmentDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveClientsForAssignment(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                    string accountFromDate, string accountToDate, bool contHisFlag, string contHisMonth, bool trustAccFlag,
                    string trustAccBalance, bool MMFFlag, string MMFBalance, bool TPeriod, string periodMonth, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            #region Updated by Thet Maung Chaw

            //DataSet ds = null;
            //string andClause = " ", orderClause = "AND (Rank !=1 OR Rank is null) ORDER BY CLM.LCRDATE DESC ";

            //string contHisFlagSql = "";
            //string conHistSql = "";
            ////Changes for Sorting
            ////CONVERT(VARCHAR, CLM.LCRDATE, 103) AS ACOpen
            ////CONVERT(VARCHAR, TCA.LastTradeDate, 103) AS LastTradeDate
            ////CONVERT(VARCHAR, CLA.AssignDate, 103) AS AssignDate
            ////CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate, CONVERT(VARCHAR, CLA.CutOffDate, 120) AS LastCutOffDate
            ////CONVERT(VARCHAR, CCT.LastCallDate, 120) AS LastCallDate

            ///// <Added by OC>
            //String AccountTypesSql = String.Empty;
            //if (AccountTypes != String.Empty)
            //{
            //    AccountTypesSql = " AND CLM.AccServiceType IN ('" + AccountTypes + "') ";
            //}

            //sql = "SELECT TCA.AcctNo, TCA.ClientName, TCA.AECode AS TeamCode, CEILING(ABS(TCA.out_Bal)) AS Balance, " +
            //    "(TCA.out_Bal + TCA.Market_vl) AS TwoNClient, " +
            //    "CEILING(ABS(TCA.Market_vl)) AS StockMarket, CEILING(ABS((CEILING(ABS(TCA.out_Bal)) + CEILING(ABS(TCA.Market_vl))))) AS Equity, TCA.AUM, TCA.MMF,TCA.AltAECode," +
            //    "TCA.LastTradeDate, TCA.Status, TCA.TotalComm," +
            //    "CLM.LCRDATE AS ACOpen, CLA.AECode AS LastAssignDealer, CONVERT(VARCHAR, CLA.AssignDate, 103) AS LastAssignDate, " +
            //    "CLA.CutOffDate, CCT.LastCallDate, CCT.ModifiedUser AS LastCallDealer,CCT.Rank, ISNULL(CT.TotalCall, 0) as TotalCall," +
            //    "CC.AECode AS CoreAECode, 't' AS AssignmentType, 's' AS AssignmentStatus, " +
            //    "CLA.AECode AS AssignDealer, CLA.AssignDate, " +
            //    "CLA.CutOffDate AS LastCutOffDate " +
            //    ", CLA.ModifiedUser, CLA.ModifiedDate ,TCA.Email,CLM.LEMAIL " +       //New added for AuditLog
            //    "FROM TmpClientAssign TCA " +
            //    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo = CLM.LACCT AND TCA.AccServiceType = CLM.AccServiceType " +
            //    "LEFT JOIN	" +         // JOIN for retrieving MAX Assingment Record
            //    "(" +
            //    "    SELECT DISTINCT CA.AECode, CA.AcctNo, MDT.MDate AS AssignDate, MDT.CutOffDate AS CutOffDate " +
            //    "    , CA.ModifiedUser, CA.ModifiedDate " +                 //New added for AuditLog
            //    "    FROM ClientAssign CA WITH (NOLOCK) " +
            //    "    INNER JOIN " +
            //    "    ( " +
            //    "        SELECT AcctNo, MAX(AssignDate) AS MDate, MAX(CutOffDate) AS CutOffDate " +
            //    "        FROM ClientAssign WITH (NOLOCK) GROUP BY AcctNo " +
            //    "    ) MDT ON CA.AcctNo = MDT.AcctNo AND CA.AssignDate = MDT.MDate " +
            //    ") CLA ON TCA.AcctNo = CLA.AcctNo " +
            //    "LEFT JOIN " +	        // JOIN for retrieving MAX Contact Record
            //    "(" +
            //    "    SELECT CCT.AcctNo, Rank, ModifiedUser, Max(ContactDate) AS LastCallDate " +
            //    "    FROM ClientContact CCT WITH (NOLOCK) " +
            //    "    LEFT JOIN " +
            //    "    ( " +
            //    "        SELECT AcctNo, Max(ContactDate) AS MDate " +
            //    "        FROM ClientContact WITH (NOLOCK) GROUP BY AcctNo " +
            //    "    ) MDT ON CCT.AcctNo=MDT.AcctNo " +
            //    "    WHERE CCT.ContactDate=MDT.MDate " +
            //    "    GROUP BY CCT.AcctNo, Rank, ModifiedUser " +
            //    ") " +
            //    "CCT ON TCA.AcctNo = CCT.AcctNo " +
            //    "LEFT JOIN ClientTotal CT ON TCA.AcctNo = CT.AcctNo " +
            //    "LEFT JOIN CoreClient CC ON TCA.AcctNo = CC.AcctNo " +
            //    "WHERE CLM.LCRDATE BETWEEN '" + accountFromDate + "' AND '" + accountToDate + "' ";

            //#region Updated by Thet Maung Chaw

            ////"AND TCA.AECode = '" + teamCode + "' " +

            //if (AccountTypes.Contains("S2"))
            //{
            //    sql += " AND TCA.AltAECode IN (SELECT AltAECode FROM DealerDetail WHERE AEGroup='" + teamCode + "' AND (AltAECode IS NOT NULL AND AltAECode <> '')) ";
            //}
            //else
            //{
            //    sql += " AND TCA.AECode = '" + teamCode + "' ";
            //}

            //#endregion

            //sql += AccountTypesSql;

            ////WHERE CLM.LCRDATE BETWEEN '2000-01-01' AND '2010-03-03' AND TCA.AECode = 'TA'




            ////if (_2NFlag)
            ////{
            ////    andClause = " AND TCA.Status='I' AND ((TCA.out_Bal + TCA.Market_vl) > 0) "
            ////        + "AND DateDiff(MM, ISNULL(TCA.LastTradeDate, '1900-01-01'), GETDATE()) >= 3";
            ////}

            //if (emailFlag)
            //{
            //    andClause = " AND TCA.Email = 'Y'";
            //}

            //if (mobileFlag)
            //{
            //    andClause = " AND TCA.Mobile = 'Y'";
            //}

            ////Changes for AccuntFilter


            //if (contHisFlag)
            //{
            //    contHisFlagSql = " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";

            //    sql = sql + " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";
            //}

            //if (trustAccFlag)
            //{
            //    sql = sql + " AND  ABS(TCA.out_Bal)>=" + trustAccBalance + " and TCA.MMF='N'";
            //}

            //if (MMFFlag)
            //{
            //    sql = sql + " AND  ABS(TCA.out_Bal)>=" + MMFBalance + " and TCA.MMF='Y'";
            //}

            //// Test OK
            //if (TPeriod)
            //{
            //    sql = sql + " AND DATEDIFF(mm,TCA.LastTradeDate,getdate())>= " + periodMonth + "";
            //}

            //// Test OK
            //if (SMarketValue)
            //{
            //    sql = sql + " AND TCA.Market_vl >= " + MarketValue + "";
            //}

            //if (contHisFlag)
            //{
            //    conHistSql = sql;
            //    conHistSql = conHistSql.Replace(contHisFlagSql, "AND CCT.LastCallDate is null");
            //    conHistSql = conHistSql + andClause + orderClause;
            //    sql = sql + " UNION " + conHistSql + "";
            //    ds = genericDA.ExecuteQuery(sql);
            //}
            //else
            //{
            //    sql = sql + andClause + orderClause;
            //    //Retrive without setting Row No from Program Code
            //    ds = genericDA.ExecuteQuery(sql);
            //}

            ////To set Row No from Program Code
            ////SqlCommand cmd = genericDA.GetSqlCommand();
            ////SqlDataReader dr = cmd.EndExecuteReader();

            //return ds;

            #endregion

            String SQL = String.Empty;
            String rankClause = " AND (Rank !=1 OR Rank is null) ";
            String orderClause = " ORDER BY CLM.LCRDATE DESC ";

            if (!AccountTypes.Contains("S2")) /// <Without S2>
            {
                SQL = PrepareSQLStatementForAssignmentWithoutS2(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate, accountToDate, contHisFlag, contHisMonth, trustAccFlag, trustAccBalance, MMFFlag, MMFBalance, TPeriod, periodMonth, SMarketValue, MarketValue, AccountTypes);

                //if (!contHisFlag)
                    SQL += rankClause + orderClause;
            }
            else if (AccountTypes.Contains("S2") && AccountTypes.Length > 4) /// <Mix with S2 and other account types>
            {
                SQL = PrepareSQLStatementForAssignmentWithoutS2(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate, accountToDate, contHisFlag, contHisMonth, trustAccFlag, trustAccBalance, MMFFlag, MMFBalance, TPeriod, periodMonth, SMarketValue, MarketValue, AccountTypes);

                //if (!contHisFlag)
                    SQL += rankClause;

                SQL += "\r\n\r\n" + "UNION ALL" + "\r\n\r\n";

                SQL += PrepareSQLStatementForAssignmentOnlyS2(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate, accountToDate, contHisFlag, contHisMonth, trustAccFlag, trustAccBalance, MMFFlag, MMFBalance, TPeriod, periodMonth, SMarketValue, MarketValue, "S2");

                //if (!contHisFlag)
                    SQL += rankClause + orderClause;
            }
            else
            {
                SQL = PrepareSQLStatementForAssignmentOnlyS2(teamCode, _2NFlag, emailFlag, mobileFlag, accountFromDate, accountToDate, contHisFlag, contHisMonth, trustAccFlag, trustAccBalance, MMFFlag, MMFBalance, TPeriod, periodMonth, SMarketValue, MarketValue, "S2");

                //if (!contHisFlag)
                    SQL += rankClause + orderClause;
            }

            DataSet ds;
            return ds = genericDA.ExecuteQuery(SQL);
        }

        private String PrepareSQLStatementForAssignmentOnlyS2(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                    string accountFromDate, string accountToDate, bool contHisFlag, string contHisMonth, bool trustAccFlag,
                    string trustAccBalance, bool MMFFlag, string MMFBalance, bool TPeriod, string periodMonth, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            string andClause = " ", orderClause = "AND (Rank !=1 OR Rank is null) ORDER BY CLM.LCRDATE DESC ";

            string contHisFlagSql = "";
            string conHistSql = "";
            //Changes for Sorting
            //CONVERT(VARCHAR, CLM.LCRDATE, 103) AS ACOpen
            //CONVERT(VARCHAR, TCA.LastTradeDate, 103) AS LastTradeDate
            //CONVERT(VARCHAR, CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate, CONVERT(VARCHAR, CLA.CutOffDate, 120) AS LastCutOffDate
            //CONVERT(VARCHAR, CCT.LastCallDate, 120) AS LastCallDate

            /// <Added by OC>
            String AccountTypesSql = String.Empty;
            if (AccountTypes != String.Empty)
            {
                AccountTypesSql = " AND CLM.AccServiceType IN ('" + AccountTypes + "') ";
            }

            sql = "SELECT TCA.AcctNo, TCA.ClientName, TCA.AECode AS TeamCode, CEILING(ABS(TCA.out_Bal)) AS Balance, " +
                "(TCA.out_Bal + TCA.Market_vl) AS TwoNClient, " +
                "CEILING(ABS(TCA.Market_vl)) AS StockMarket, CEILING(ABS((CEILING(ABS(TCA.out_Bal)) + CEILING(ABS(TCA.Market_vl))))) AS Equity, TCA.AUM, TCA.MMF,TCA.AltAECode," +
                "TCA.LastTradeDate, TCA.Status, TCA.TotalComm," +
                "CLM.LCRDATE AS ACOpen, CLA.AECode AS LastAssignDealer, CONVERT(VARCHAR, CLA.AssignDate, 103) AS LastAssignDate, " +
                "CLA.CutOffDate, CCT.LastCallDate, CCT.ModifiedUser AS LastCallDealer,CCT.Rank, ISNULL(CT.TotalCall, 0) as TotalCall," +
                "CC.AECode AS CoreAECode, 't' AS AssignmentType, 's' AS AssignmentStatus, " +
                "CLA.AECode AS AssignDealer, CLA.AssignDate, " +
                "CLA.CutOffDate AS LastCutOffDate " +
                ", CLA.ModifiedUser, CLA.ModifiedDate ,TCA.Email,CLM.LEMAIL " +       //New added for AuditLog
                "FROM TmpClientAssign TCA " +
                "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo = CLM.LACCT AND TCA.AccServiceType = CLM.AccServiceType " +
                "LEFT JOIN	" +         // JOIN for retrieving MAX Assingment Record
                "(" +
                "    SELECT DISTINCT CA.AECode, CA.AcctNo, MDT.MDate AS AssignDate, MDT.CutOffDate AS CutOffDate " +
                "    , CA.ModifiedUser, CA.ModifiedDate " +                 //New added for AuditLog
                "    FROM ClientAssign CA WITH (NOLOCK) " +
                "    INNER JOIN " +
                "    ( " +
                "        SELECT AcctNo, MAX(AssignDate) AS MDate, MAX(CutOffDate) AS CutOffDate " +
                "        FROM ClientAssign WITH (NOLOCK) GROUP BY AcctNo " +
                "    ) MDT ON CA.AcctNo = MDT.AcctNo AND CA.AssignDate = MDT.MDate " +
                ") CLA ON TCA.AcctNo = CLA.AcctNo " +
                "LEFT JOIN " +	        // JOIN for retrieving MAX Contact Record
                "(" +
                "    SELECT CCT.AcctNo, Rank, ModifiedUser, Max(ContactDate) AS LastCallDate " +
                "    FROM ClientContact CCT WITH (NOLOCK) " +
                "    LEFT JOIN " +
                "    ( " +
                "        SELECT AcctNo, Max(ContactDate) AS MDate " +
                "        FROM ClientContact WITH (NOLOCK) GROUP BY AcctNo " +
                "    ) MDT ON CCT.AcctNo=MDT.AcctNo " +
                "    WHERE CCT.ContactDate=MDT.MDate " +
                "    GROUP BY CCT.AcctNo, Rank, ModifiedUser " +
                ") " +
                "CCT ON TCA.AcctNo = CCT.AcctNo " +
                "LEFT JOIN ClientTotal CT ON TCA.AcctNo = CT.AcctNo " +
                "LEFT JOIN CoreClient CC ON TCA.AcctNo = CC.AcctNo " +
                "WHERE CLM.LCRDATE BETWEEN '" + accountFromDate + "' AND '" + accountToDate + "' ";

            #region Updated by Thet Maung Chaw

            //"AND TCA.AECode = '" + teamCode + "' " +

            sql += " AND TCA.AltAECode IN (SELECT AltAECode FROM DealerDetail WHERE AEGroup='" + teamCode + "' AND (AltAECode IS NOT NULL AND AltAECode <> '')) ";            

            #endregion

            sql += AccountTypesSql;

            //WHERE CLM.LCRDATE BETWEEN '2000-01-01' AND '2010-03-03' AND TCA.AECode = 'TA'




            //if (_2NFlag)
            //{
            //    andClause = " AND TCA.Status='I' AND ((TCA.out_Bal + TCA.Market_vl) > 0) "
            //        + "AND DateDiff(MM, ISNULL(TCA.LastTradeDate, '1900-01-01'), GETDATE()) >= 3";
            //}

            if (emailFlag)
            {
                andClause = " AND TCA.Email = 'Y'";
            }

            if (mobileFlag)
            {
                andClause = " AND TCA.Mobile = 'Y'";
            }

            //Changes for AccuntFilter


            if (contHisFlag)
            {
                contHisFlagSql = " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";

                sql = sql + " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";
            }

            if (trustAccFlag)
            {
                sql = sql + " AND  ABS(TCA.out_Bal)>=" + trustAccBalance + " and TCA.MMF='N'";
            }

            if (MMFFlag)
            {
                sql = sql + " AND  ABS(TCA.out_Bal)>=" + MMFBalance + " and TCA.MMF='Y'";
            }

            // Test OK
            if (TPeriod)
            {
                sql = sql + " AND DATEDIFF(mm,TCA.LastTradeDate,getdate())>= " + periodMonth + "";
            }

            // Test OK
            if (SMarketValue)
            {
                sql = sql + " AND TCA.Market_vl >= " + MarketValue + "";
            }

            if (contHisFlag)
            {
                conHistSql = sql;
                conHistSql = conHistSql.Replace(contHisFlagSql, "AND CCT.LastCallDate is null");
                conHistSql = conHistSql + andClause;// +orderClause;
                sql = sql + " UNION " + conHistSql + "";
            }
            else
            {
                sql = sql + andClause;// +orderClause; // Commented by Thet Maung Chaw to be able to union all
            }

            //To set Row No from Program Code
            //SqlCommand cmd = genericDA.GetSqlCommand();
            //SqlDataReader dr = cmd.EndExecuteReader();

            return sql;
        }

        private String PrepareSQLStatementForAssignmentWithoutS2(string teamCode, bool _2NFlag, bool emailFlag, bool mobileFlag,
                    string accountFromDate, string accountToDate, bool contHisFlag, string contHisMonth, bool trustAccFlag,
                    string trustAccBalance, bool MMFFlag, string MMFBalance, bool TPeriod, string periodMonth, bool SMarketValue, string MarketValue, String AccountTypes)
        {
            string andClause = " ", orderClause = "AND (Rank !=1 OR Rank is null) ORDER BY CLM.LCRDATE DESC ";

            string contHisFlagSql = "";
            string conHistSql = "";
            //Changes for Sorting
            //CONVERT(VARCHAR, CLM.LCRDATE, 103) AS ACOpen
            //CONVERT(VARCHAR, TCA.LastTradeDate, 103) AS LastTradeDate
            //CONVERT(VARCHAR, CLA.AssignDate, 103) AS AssignDate
            //CONVERT(VARCHAR, CLA.CutOffDate, 120) AS CutOffDate, CONVERT(VARCHAR, CLA.CutOffDate, 120) AS LastCutOffDate
            //CONVERT(VARCHAR, CCT.LastCallDate, 120) AS LastCallDate

            /// <Added by OC>
            String AccountTypesSql = String.Empty;
            if (AccountTypes != String.Empty)
            {
                AccountTypesSql = " AND CLM.AccServiceType IN ('" + AccountTypes + "') ";
            }

            sql = "SELECT TCA.AcctNo, TCA.ClientName, TCA.AECode AS TeamCode, CEILING(ABS(TCA.out_Bal)) AS Balance, " +
                "(TCA.out_Bal + TCA.Market_vl) AS TwoNClient, " +
                "CEILING(ABS(TCA.Market_vl)) AS StockMarket, CEILING(ABS((CEILING(ABS(TCA.out_Bal)) + CEILING(ABS(TCA.Market_vl))))) AS Equity, TCA.AUM, TCA.MMF,TCA.AltAECode," +
                "TCA.LastTradeDate, TCA.Status, TCA.TotalComm," +
                "CLM.LCRDATE AS ACOpen, CLA.AECode AS LastAssignDealer, CONVERT(VARCHAR, CLA.AssignDate, 103) AS LastAssignDate, " +
                "CLA.CutOffDate, CCT.LastCallDate, CCT.ModifiedUser AS LastCallDealer,CCT.Rank, ISNULL(CT.TotalCall, 0) as TotalCall," +
                "CC.AECode AS CoreAECode, 't' AS AssignmentType, 's' AS AssignmentStatus, " +
                "CLA.AECode AS AssignDealer, CLA.AssignDate, " +
                "CLA.CutOffDate AS LastCutOffDate " +
                ", CLA.ModifiedUser, CLA.ModifiedDate ,TCA.Email,CLM.LEMAIL " +       //New added for AuditLog
                "FROM TmpClientAssign TCA " +
                "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON TCA.AcctNo = CLM.LACCT AND TCA.AccServiceType = CLM.AccServiceType " +
                "LEFT JOIN	" +         // JOIN for retrieving MAX Assingment Record
                "(" +
                "    SELECT DISTINCT CA.AECode, CA.AcctNo, MDT.MDate AS AssignDate, MDT.CutOffDate AS CutOffDate " +
                "    , CA.ModifiedUser, CA.ModifiedDate " +                 //New added for AuditLog
                "    FROM ClientAssign CA WITH (NOLOCK) " +
                "    INNER JOIN " +
                "    ( " +
                "        SELECT AcctNo, MAX(AssignDate) AS MDate, MAX(CutOffDate) AS CutOffDate " +
                "        FROM ClientAssign WITH (NOLOCK) GROUP BY AcctNo " +
                "    ) MDT ON CA.AcctNo = MDT.AcctNo AND CA.AssignDate = MDT.MDate " +
                ") CLA ON TCA.AcctNo = CLA.AcctNo " +
                "LEFT JOIN " +	        // JOIN for retrieving MAX Contact Record
                "(" +
                "    SELECT CCT.AcctNo, Rank, ModifiedUser, Max(ContactDate) AS LastCallDate " +
                "    FROM ClientContact CCT WITH (NOLOCK) " +
                "    LEFT JOIN " +
                "    ( " +
                "        SELECT AcctNo, Max(ContactDate) AS MDate " +
                "        FROM ClientContact WITH (NOLOCK) GROUP BY AcctNo " +
                "    ) MDT ON CCT.AcctNo=MDT.AcctNo " +
                "    WHERE CCT.ContactDate=MDT.MDate " +
                "    GROUP BY CCT.AcctNo, Rank, ModifiedUser " +
                ") " +
                "CCT ON TCA.AcctNo = CCT.AcctNo " +
                "LEFT JOIN ClientTotal CT ON TCA.AcctNo = CT.AcctNo " +
                "LEFT JOIN CoreClient CC ON TCA.AcctNo = CC.AcctNo " +
                "WHERE CLM.LCRDATE BETWEEN '" + accountFromDate + "' AND '" + accountToDate + "' ";

            #region Updated by Thet Maung Chaw

            //"AND TCA.AECode = '" + teamCode + "' " +
            sql += " AND TCA.AECode = '" + teamCode + "' ";

            #endregion

            sql += AccountTypesSql;

            //WHERE CLM.LCRDATE BETWEEN '2000-01-01' AND '2010-03-03' AND TCA.AECode = 'TA'




            //if (_2NFlag)
            //{
            //    andClause = " AND TCA.Status='I' AND ((TCA.out_Bal + TCA.Market_vl) > 0) "
            //        + "AND DateDiff(MM, ISNULL(TCA.LastTradeDate, '1900-01-01'), GETDATE()) >= 3";
            //}

            if (emailFlag)
            {
                andClause = " AND TCA.Email = 'Y'";
            }

            if (mobileFlag)
            {
                andClause = " AND TCA.Mobile = 'Y'";
            }

            //Changes for AccuntFilter


            if (contHisFlag)
            {
                contHisFlagSql = " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";

                sql = sql + " AND DATEDIFF(mm,CCT.LastCallDate,getdate())>= " + contHisMonth + "";
            }

            if (trustAccFlag)
            {
                sql = sql + " AND  ABS(TCA.out_Bal)>=" + trustAccBalance + " and TCA.MMF='N'";
            }

            if (MMFFlag)
            {
                sql = sql + " AND  ABS(TCA.out_Bal)>=" + MMFBalance + " and TCA.MMF='Y'";
            }

            // Test OK
            if (TPeriod)
            {
                sql = sql + " AND DATEDIFF(mm,TCA.LastTradeDate,getdate())>= " + periodMonth + "";
            }

            // Test OK
            if (SMarketValue)
            {
                sql = sql + " AND TCA.Market_vl >= " + MarketValue + "";
            }

            if (contHisFlag)
            {
                conHistSql = sql;
                conHistSql = conHistSql.Replace(contHisFlagSql, "AND CCT.LastCallDate is null");
                conHistSql = conHistSql + andClause;// +orderClause;

                sql = sql + " UNION " + conHistSql + " ";
            }
            else
            {
                sql = sql + andClause; // +orderClause; // Commented by Thet Maung Chaw to be able to union all
            }

            //To set Row No from Program Code
            //SqlCommand cmd = genericDA.GetSqlCommand();
            //SqlDataReader dr = cmd.EndExecuteReader();

            return sql;
        }

        public DataTable RetrieveAssignmentHistoryByCriteria(string dealerCode, string accountNo, string nric,
                            string fromDate, string toDate, bool retradeFlag, String Param, String UserID)
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

            sql = "SELECT DISTINCT CLA.AcctNo, CLA.AECode AS Dealer, CLA.AssignDate AS AssignDate, " +
                    "CLM.LCRDATE AS AcctCreateDate, CLM.NRIC, CLM.LNAME AS ClientName, " +
                    "CLM.LRNER as TeamCode, TAssignLTD.CTradeDate AS AssignLTD, " +
                    "LTD.OverallLTD AS CurrentLTD, MCC.LastCallDate, " +
                    "CLA.CutOffDate AS CutOffDate, " +
                    "DATEDIFF(mm, TAssignLTD.CTradeDate, LTD.OverallLTD) AS MonthDiff, " +
                    "ISNULL(TCA.TotalComm, 0) AS TotalComm, " +
                    "ISNULL(TCA.NumTrades, 0) AS NoOfTrades " +
                    "FROM ClientAssign CLA " +
                    "INNER JOIN DealerDetail ON CLA.AECode = DealerDetail.AECode " +
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
                if (whereFlag)
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
                    sb.Append(" AND CLA.AssignDate BETWEEN '").Append(fromDate).Append("' AND '").Append(toDate).Append(" 23:59:59.999' ");
                }
                else
                {
                    sb.Append(" WHERE CLA.AssignDate BETWEEN '").Append(fromDate).Append("' AND '").Append(toDate).Append(" 23:59:59.999' ");
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
                    sb.Append(" AND CLA.AssignDate <= '").Append(toDate).Append(" 23:59:59.999' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.AssignDate <= '").Append(toDate).Append(" 23:59:59.999' ");
                }
            }

            /// <Added by Thet Maung Chaw not to include FAR records.>
            if (Param != String.Empty)
            {
                if (whereFlag)
                {
                    //sb.Append(" AND " + Param);
                    sb.Append(" AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ");
                    whereFlag = true;
                }
                else
                {
                    //sb.Append(" WHERE " + Param);
                    sb.Append(" WHERE AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') ");
                    whereFlag = true;
                }
            }

            //New added for retrieving LastTradeDate only
            if (retradeFlag)
            {
                //sb.Append(" AND TCA.LastTradeDate > TAssignLTD.LTradeDate ");
                //sb.Append(" AND (DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) > 0 OR  Datediff(dd, tassignltd.ltradedate, ltd.overallltd) IS NULL OR Datediff(dd, tassignltd.ltradedate, ltd.overallltd)='')");
                sb.Append(" AND (DATEDIFF(dd , ISNULL(TAssignLTD.LTradeDate, '1900-01-01'), ISNULL(LTD.OverallLTD, '1900-01-01')) >  0) ");
            }

            sb.Append(GetSQLForLeadByAdministrator(dealerCode, fromDate, toDate, Param, accountNo, nric, UserID));

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public int InsertAssignment(DataSet ds)
        {
            int result = 1;

            // Keep in mind the sequence in which you are adding the parameters should be same as the sequence they appear in the query
            OleDbParameter[] sqlParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@AECode", OleDbType.VarChar), 
                                            new OleDbParameter("@AcctNo", OleDbType.VarChar),
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
                                            new OleDbParameter("@AcctNo", OleDbType.VarChar),
                                            new OleDbParameter("@AssignDate", OleDbType.Date) 
                                        };

            //Parameters for AuditLog
            OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                                            new OleDbParameter("@istrAcctNo", OleDbType.VarChar),
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

            string auditLogSql = "SPM_spAssignmentAuditIns";

            //Parameterized Query for OLEDB Parameters
            string updateSql = "UPDATE ClientAssign SET AECode = ?, " +
                " CutOffDate = ?, ModifiedUser = ?, ModifiedDate = CONVERT(VARCHAR, GetDate(), 120) " +
                " WHERE AECode = ? AND AcctNo = ? AND AssignDate = ?";

            
            sql = "INSERT INTO ClientAssign(AECode, AcctNo, AssignDate, CutOffDate, ModifiedUser, ModifiedDate,ProjectID) " +
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
                        cmd.Parameters["@AcctNo"].Value = dtClientAssign.Rows[i]["AcctNo"].ToString();

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
                        cmdUpdate.Parameters["@AcctNo"].Value = dtClientAssign.Rows[i]["AcctNo"].ToString();

                        cmdUpdate.Parameters["@AssignDate"].Value = dtClientAssign.Rows[i]["AssignDate"].ToString();
                        //Correct Date Format
                        //cmdUpdate.Parameters["@AssignDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["AssignDate"].ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");


                        //cmd.Parameters["@CutOffDate"].Value = DateTime.ParseExact(dtClientAssign.Rows[i]["CutOffDate"].ToString(), "dd/MM/yyyy", null);
                        cmdUpdate.Parameters["@CutOffDate"].Value = dtClientAssign.Rows[i]["CutOffDate"].ToString();
                        cmdUpdate.Parameters["@ModifiedUser"].Value = dtClientAssign.Rows[i]["ModifiedUser"].ToString();

                        result = cmdUpdate.ExecuteNonQuery();

                        if (result > 0)
                        {
                            cmdAuditLog.Parameters["@istrAECode"].Value = dtClientAssign.Rows[i]["LastAssignDealer"].ToString();
                            cmdAuditLog.Parameters["@istrAcctNo"].Value = dtClientAssign.Rows[i]["AcctNo"].ToString();
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
                    }
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
                    cmdUpdate.Dispose();
                    cmdAuditLog.Dispose();
                }
                catch (Exception ex) { }
            }
            
            return result;
        }
        
        public int DeleteClientAssignment(string dealerCode, string accountNumber, string assignDate, string cutOffDate, string modifiedUser,
                    string modifiedDate, string newModifiedUser)
        {
            int result = -1;

            //WHERE AECode = 'T11_PIC2' AND AcctNo = '0336021' AND AssignDate = '2010-03-08'
            sql = "DELETE FROM ClientAssign WHERE AECode = '" + dealerCode + "' AND AcctNo = '" + accountNumber
                    + "' AND AssignDate = '" + assignDate + "'";

            result = genericDA.ExecuteNonQuery(sql);
            if (result > 0)
            {
                //string auditLogSql = "INSERT INTO ClientAssignAudit(OldAECode, OldAcctNo, OldAssignDate, OldCutOffDate, OldModifiedUser, OldModifiedDate," +
                //        "Action, ActionBy, ActionDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, CONVERT(VARCHAR, GetDate(), 120))";


                string auditLogSql = "SPM_spAssignmentAuditIns";
                OleDbCommand cmdAuditLog = genericDA.GetSqlCommand();

                //Parameters for AuditLog
                OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                                            new OleDbParameter("@istrAcctNo", OleDbType.VarChar),
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
                cmdAuditLog.Parameters["@istrAcctNo"].Value = accountNumber;
                cmdAuditLog.Parameters["@idtAssignDate"].Value = assignDate;
                cmdAuditLog.Parameters["@idtCutOffDate"].Value = cutOffDate;
                cmdAuditLog.Parameters["@istrModifiedUser"].Value = modifiedUser;
                cmdAuditLog.Parameters["@idtModifiedDate"].Value = modifiedDate;

                cmdAuditLog.Parameters["@istrActionCode"].Value = "D";
                cmdAuditLog.Parameters["@istrActionBy"].Value = newModifiedUser;

                cmdAuditLog.ExecuteNonQuery();

                cmdAuditLog.Dispose();
            }

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
                  "      WHERE DD.Enable='1' AND (DD.CrossGroup = 'Y' OR DD.CrossGroup = '1') AND ISNULL(CA.AEGroup, '') <> '' AND UserType NOT LIKE 'FAR%' " +
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
                        "    SELECT AECode, COUNT(AECode) AS TotAss FROM ClientAssign WITH (NOLOCK) " +
                        "    WHERE CutOffDate > GETDATE() " +
                        "    GROUP BY AECode " +
                        ") " +
                        "CAT ON DD.AECode=CAT.AECode " +
                        "WHERE DD.AECode<>'CB910' AND Enable='1' AND CrossGroup = 'Y' " +
                        "AND CAC.AEGroup = '" + crossTeamCode + "'" +
                        "ORDER BY CAC.AECode ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveAssignmentForExtraCall(string accountNo, string dealerCode, string contactDate)
        {
            sql = "SELECT CLA.AssignDate, CLA.AcctNo, CLA.AECode " +
                        "FROM ClientAssign CLA " +
                        "WHERE CLA.AcctNo = '" + accountNo + "' "  +
                        "AND CLA.AECode = '" + dealerCode + "' " +
                        "AND '" + contactDate + "' BETWEEN CLA.AssignDate AND CLA.CutOffDate";

            return genericDA.ExecuteQueryForDataTable(sql);
        }
        
        //public DataTable RetrieveAssignmentDateByDateRange(string fromDate, string toDate)
        //{
        //    sql = "SELECT CONVERT(VARCHAR(10), AssignDate, 103) AS AssignDateStr " +
        //            "FROM ClientAssign " +
        //            "WHERE AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + "' " +
        //            "GROUP BY AssignDate " +
        //            "ORDER BY AssignDate DESC ";

        //    return genericDA.ExecuteQueryForDataTable(sql);
        //}

        #region SPM III

        /// <summary>
        /// Created by:		Thet Su Mon (CyberQuote)
        /// </summary>
        /// 

        //All the client and  lead info by searching by assign date
        public DataTable RetrieveAssignedClientInfo(string assignDate)
        {
                 sql = "SELECT DISTINCT CA.AECode AS Dealer, LRNER AS Team, CA.AcctNo, ISNULL(LName, '') AS ClientName, CLTD.OverallLTD AS LTD, " +
                  "LMOBILE AS Mobile, ISNULL(LEMAIL, '') AS Email, ISNULL((LADDR1+' ' +LADDR2+' '+LADDR3), '') AS Address " +
                //", ISNULL(LADDR1, '') AS LADDR1, ISNULL(LADDR2, '') AS LADDR2, ISNULL(LADDR3, '') AS LADDR3 " +
                   "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
                   "LEFT JOIN SPM.dbo.ClientLTD CLTD WITH (NOLOCK) ON CA.AcctNo=CLTD.AcctNo " +
                    "LEFT JOIN  " +
                    "( " +
                    "SELECT GroupName, AECodeGp FROM SPM..DealerIDTable WITH (NOLOCK) WHERE GroupType='Dealer'  " +
                    "AND RefMonthYr = (SELECT LEFT(Max(LastUpdate), 6) AS MaxDay FROM SPM.dbo.LastUpdateTable WITH (NOLOCK)) " +
                    ") " +
                    "DID ON CA.AECode=DID.AECodeGp  " +
                    "LEFT JOIN SPM.dbo.CLMAST CL WITH (NOLOCK) ON CA.AcctNo=CL.LACCT  " +
                     "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = CA.AECode " +      //To serach with Team
                    "WHERE CA.AssignDate = '" + assignDate + "'" +
                    "UNION ALL " +
                    "( " +
                    " SELECT DISTINCT LA.AECode AS Dealer," +
                    "LD.AEGroup AS Team, " +
                    "LA.LeadID AS AcctNo, " +
                    "ISNULL(LeadName, '') " +
                    "AS ClientName, " +
                    "'' AS LTD, " +
                    "LeadMobile AS Mobile," +
                    "ISNULL(LeadEmail, '') AS Email," +
                    "'' AS Address " +
                    "FROM SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                    "LEFT JOIN SPM.dbo.LeadDetail LD WITH (NOLOCK) ON LA.LeadID =LD.LeadID " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = LA.LeadID " +
                    "WHERE LA.AssignDate= '" + assignDate + "' " +
                    ")";

            StringBuilder sb = new StringBuilder(sql);
            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        /// <Added by OC>
        public DataTable RetrieveAssignedClientInfoByUserOrSupervisor(String assignDate, String Param)
        {
            sql = "SELECT DISTINCT CA.AECode AS Dealer, LRNER AS Team, CA.AcctNo, ISNULL(LName, '') AS ClientName, CLTD.OverallLTD AS LTD, " +
             "LMOBILE AS Mobile, ISNULL(LEMAIL, '') AS Email, ISNULL((LADDR1+' ' +LADDR2+' '+LADDR3), '') AS Address " +
                //", ISNULL(LADDR1, '') AS LADDR1, ISNULL(LADDR2, '') AS LADDR2, ISNULL(LADDR3, '') AS LADDR3 " +
              "FROM SPM.dbo.ClientAssign CA WITH (NOLOCK) " +
              "LEFT JOIN SPM.dbo.ClientLTD CLTD WITH (NOLOCK) ON CA.AcctNo=CLTD.AcctNo " +
               "LEFT JOIN  " +
               "( " +
               "SELECT GroupName, AECodeGp FROM SPM..DealerIDTable WITH (NOLOCK) WHERE GroupType='Dealer'  " +
               "AND RefMonthYr = (SELECT LEFT(Max(LastUpdate), 6) AS MaxDay FROM SPM.dbo.LastUpdateTable WITH (NOLOCK)) " +
               ") " +
               "DID ON CA.AECode=DID.AECodeGp  " +
               "LEFT JOIN SPM.dbo.CLMAST CL WITH (NOLOCK) ON CA.AcctNo=CL.LACCT  " +
                "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = CA.AECode " +      //To serach with Team
               "WHERE CA.AssignDate = '" + assignDate + "' AND DD." + Param +
               "UNION ALL " +
               "( " +
               " SELECT DISTINCT LA.AECode AS Dealer," +
               "LD.AEGroup AS Team, " +
               "LA.LeadID AS AcctNo, " +
               "ISNULL(LeadName, '') " +
               "AS ClientName, " +
               "'' AS LTD, " +
               "LeadMobile AS Mobile," +
               "ISNULL(LeadEmail, '') AS Email," +
               "'' AS Address " +
               "FROM SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
               "LEFT JOIN SPM.dbo.LeadDetail LD WITH (NOLOCK) ON LA.LeadID =LD.LeadID " +
               "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = LA.LeadID " +
               "WHERE LA.AssignDate= '" + assignDate + "' AND DD. " + Param +
               ")";

            StringBuilder sb = new StringBuilder(sql);
            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }


        //All the client info searching by Project Name
        public DataTable RetrieveAssignedClientInfoByProj(string ProjID)
        {
            //Removed Column for Excel Report
            //GroupName, AssignDate

            sql = "SELECT DISTINCT CA.AECode AS Dealer, LRNER AS Team, CA.AcctNo, ISNULL(LName, '') AS ClientName, CLTD.OverallLTD AS LTD, " +
                    "LMOBILE AS Mobile, ISNULL(LEMAIL, '') AS Email, ISNULL((LADDR1+' ' +LADDR2+' '+LADDR3), '') AS Address " +
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
                    "WHERE CA.ProjectID = '" + ProjID + "' ";

            StringBuilder sb = new StringBuilder(sql);           

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        /// <Added by OC>
        public DataTable RetrieveAssignedClientInfoByProjByUserOrSupervisor(String ProjID, String Param)
        {
            //Removed Column for Excel Report
            //GroupName, AssignDate

            sql = "SELECT DISTINCT CA.AECode AS Dealer, LRNER AS Team, CA.AcctNo, ISNULL(LName, '') AS ClientName, CLTD.OverallLTD AS LTD, " +
                    "LMOBILE AS Mobile, ISNULL(LEMAIL, '') AS Email, ISNULL((LADDR1+' ' +LADDR2+' '+LADDR3), '') AS Address " +
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
                    "WHERE CA.ProjectID = '" + ProjID + "' AND DD." + Param;

            StringBuilder sb = new StringBuilder(sql);

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        //All the lead info searching by the lead project name
        public DataTable RetrieveAssignedLeadInfoByProj(string ProjID)
        {

            sql = "SELECT DISTINCT LA.AECode AS Dealer,LD.AEGroup AS Team, LA.LeadID AS AcctNo, ISNULL(LeadName, '') AS ClientName, '' AS LTD, " +
                    "LeadMobile AS Mobile, ISNULL(LeadEmail, '') AS Email, '' AS Address " +
                    "FROM SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                    "LEFT JOIN  " +
                    "( " +
                    "    SELECT GroupName, AECodeGp FROM SPM..DealerIDTable WITH (NOLOCK) WHERE GroupType='Dealer'  " +
                    "    AND RefMonthYr = (SELECT LEFT(Max(LastUpdate), 6) AS MaxDay FROM SPM.dbo.LastUpdateTable WITH (NOLOCK)) " +
                    ") " +
                    "DID ON LA.AECode=DID.AECodeGp  " +
                    "LEFT JOIN SPM.dbo.LeadDetail LD WITH (NOLOCK) ON LA.LeadID =LD.LeadID  " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = LA.LeadID " +      //To serach with Team
                    "WHERE LA.ProjectID = '" + ProjID + "' ";

            StringBuilder sb = new StringBuilder(sql);

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        /// <Added by OC>
        public DataTable RetrieveAssignedLeadInfoByProjByUserOrSupervisor(String ProjID, String Param)
        {
            sql = "SELECT DISTINCT LA.AECode AS Dealer,LD.AEGroup AS Team, LA.LeadID AS AcctNo, ISNULL(LeadName, '') AS ClientName, '' AS LTD, " +
                    "LeadMobile AS Mobile, ISNULL(LeadEmail, '') AS Email, '' AS Address " +
                    "FROM SPM.dbo.LeadAssign LA WITH (NOLOCK) " +
                    "LEFT JOIN  " +
                    "( " +
                    "    SELECT GroupName, AECodeGp FROM SPM..DealerIDTable WITH (NOLOCK) WHERE GroupType='Dealer'  " +
                    "    AND RefMonthYr = (SELECT LEFT(Max(LastUpdate), 6) AS MaxDay FROM SPM.dbo.LastUpdateTable WITH (NOLOCK)) " +
                    ") " +
                    "DID ON LA.AECode=DID.AECodeGp  " +
                    "LEFT JOIN SPM.dbo.LeadDetail LD WITH (NOLOCK) ON LA.LeadID =LD.LeadID  " +
                    "LEFT JOIN DealerDetail DD WITH (NOLOCK) ON DD.AECode = LA.LeadID " +      //To serach with Team
                    "WHERE LA.ProjectID = '" + ProjID + "' AND DD." + Param;

            StringBuilder sb = new StringBuilder(sql);

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }


        public DataTable RetrieveAssignmentHistoryByProjName(string dealerCode, string accountNo, string nric,
                        string ProjName, bool retradeFlag)
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

            sql = "SELECT DISTINCT CLA.AcctNo, CLA.AECode AS Dealer, CLA.AssignDate AS AssignDate, " +
                    "CLM.LCRDATE AS AcctCreateDate, CLM.NRIC, CLM.LNAME AS ClientName, " +
                    "CLM.LRNER as TeamCode, TAssignLTD.CTradeDate AS AssignLTD, " +
                    "LTD.OverallLTD AS CurrentLTD, MCC.LastCallDate, " +
                    "CLA.CutOffDate AS CutOffDate, " +
                    "DATEDIFF(mm, TAssignLTD.CTradeDate, LTD.OverallLTD) AS MonthDiff, " +
                    "ISNULL(TCA.TotalComm, 0) AS TotalComm," +
                    "ISNULL(TCA.NumTrades, 0) AS NoOfTrades " +
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
                if (whereFlag)
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

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.ProjectID = '").Append(ProjName).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.ProjectID = '").Append(ProjName).Append("' ");
                }
            }

            /*  if ((!String.IsNullOrEmpty(fromDate)) && (!String.IsNullOrEmpty(toDate)))
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
              }*/

            //New added for retrieving LastTradeDate only
            if (retradeFlag)
            {
                //sb.Append(" AND TCA.LastTradeDate > TAssignLTD.LTradeDate ");
                //sb.Append(" AND DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) >= 0 ");
                //sb.Append(" AND (DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) > 0 OR  Datediff(dd, tassignltd.ltradedate, ltd.overallltd) IS NULL OR Datediff(dd, tassignltd.ltradedate, ltd.overallltd)='')");
                sb.Append(" AND (DATEDIFF(dd , ISNULL(TAssignLTD.LTradeDate, '1900-01-01'), ISNULL(LTD.OverallLTD, '1900-01-01')) > 0) ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }

        public DataTable RetrieveAssignmentHistoryByLeadProjName(string dealerCode, string accountNo, string nric,
                        string ProjName, bool retradeFlag)
        {

            bool whereFlag = false;

            sql = "SELECT LA.LeadID as AcctNo, LA.AECode AS Dealer, LA.AssignDate AS AssignDate, " +
                    "LD.CreateDate AS AcctCreateDate, LD.LeadNRIC as NRIC, LD.LeadName AS ClientName, " +
                    "LD.AECode as TeamCode, '' AS AssignLTD, " +
                    "'' AS CurrentLTD, LCC.LastCallDate, " +
                    "LA.CutOffDate AS CutOffDate, " +
                    "'' AS MonthDiff, " +
                    "'' AS TotalComm," +
                    "'' AS NoOfTrades " +
                    "FROM LeadAssign LA " +
                    "LEFT JOIN LeadDetail LD WITH (NOLOCK) ON LA.LeadID = LD.LeadID  " +
                // "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCA.AcctNo = CLA.AcctNO " +
                // "LEFT JOIN " +
                // "(	" +
                // "    SELECT DISTINCT CLTD.AcctNo, MinCDate, LTradeDate " +              //LTradeDate is to use in WHERE clause to get Retrade Client
                // "       , CTradeDate = CASE CONVERT(VARCHAR(10), LTradeDate, 103) " +   //CTradeDate is to calculate NULL value for MonthDiff
                // "       WHEN '01/01/1900' THEN NULL ELSE LTradeDate END " +
                //  "    FROM SPM..ClientLTradeDate CLTD WITH (NOLOCK) " +
                // "    LEFT JOIN " +
                //  "    (	" +
                // "        SELECT DISTINCT AcctNo, MIN(CreateDate) AS MinCDate " +
                //  "        FROM SPM.dbo.ClientLTradeDate " +
                // --WHERE CreateDate BETWEEN '" & pDate & "' AND '" & pDate2 & "' "
                /* "        GROUP BY AcctNo " +
                 "    ) TMDate ON CLTD.AcctNo=TMDate.AcctNo " +
                 "    WHERE CreateDate=MinCDate " +
                 ") TAssignLTD ON CLA.AcctNo=TAssignLTD.AcctNo " +
                 "LEFT JOIN ClientLTD LTD WITH (NOLOCK) ON CLA.AcctNo=LTD.AcctNo " + //-- AND CA.AECode='" & pAECode AND CA.AcctNo='" & pAcctNo & "' "*/
                    "INNER JOIN " +
                    "( " +
                    "    SELECT la.AECode, la.LeadID, la.AssignDate, la.CutOffDate, MAX(lc.ContactDate) as LastCallDate " +
                    "    FROM LeadAssign la " +
                    "    LEFT JOIN " +
                    "    ( " +
                    "        SELECT LeadID, ContactDate " +
                    "        FROM LeadContact   ";

            if (!String.IsNullOrEmpty(accountNo))
            {
                sql = sql + " WHERE LeadID = '" + accountNo + "' ";        //--WHERE AcctNo = '0520259'
            }

            sql = sql + " ) lc ON lc.LeadID = la.LeadID AND lc.ContactDate BETWEEN la.AssignDate AND la.CutOffDate ";

            //--WHERE cla.AcctNo = '0520259' AND AECode = 'TA_CQ'
            if (!String.IsNullOrEmpty(accountNo))
            {
                sql = sql + " WHERE la.LeadID = '" + accountNo + "' ";
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(dealerCode))
            {
                if (whereFlag)
                {
                    sql = sql + " AND AECode = '" + dealerCode + "' ";
                }
                else
                {
                    sql = sql + " WHERE AECode = '" + dealerCode + "' ";
                }
            }

            sql = sql + " GROUP BY la.AECode, la.LeadID, la.AssignDate, la.CutOffDate " +
                            " ) LCC ON LA.LeadID = LCC.LeadID AND LA.AssignDate = LCC.AssignDate ";


            StringBuilder sb = new StringBuilder(sql);
            whereFlag = false;

            if (!String.IsNullOrEmpty(dealerCode))
            {
                sb.Append(" WHERE LA.AECode = '").Append(dealerCode).Append("' ");
                whereFlag = true;
            }

            if (!String.IsNullOrEmpty(accountNo))
            {
                if (whereFlag)
                {
                    sb.Append(" AND LA.LeadID = '").Append(accountNo).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE LA.LeadID = '").Append(accountNo).Append("' ");
                }
            }

            /*if (!String.IsNullOrEmpty(nric))
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
            }*/

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                {
                    sb.Append(" AND LA.ProjectID = '").Append(ProjName).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE LA.ProjectID = '").Append(ProjName).Append("' ");
                }
            }


            //New added for retrieving LastTradeDate only
            /*if (retradeFlag)
            {
                //sb.Append(" AND TCA.LastTradeDate > TAssignLTD.LTradeDate ");
                sb.Append(" AND DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) >= 0 ");
            }*/

            if (retradeFlag)
            {
                sb.Append(" AND (DATEDIFF(dd , ISNULL(TAssignLTD.LTradeDate, '1900-01-01'), ISNULL(LTD.OverallLTD, '1900-01-01')) >  0) ");
            }

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }       

        //to bind the dropdownlist with project and client date
        public DataTable RetrieveAssignmentDateByDateRange(String UserID, string fromDate, string toDate)
        {
            /// <Modified by Thet Maung Chaw not to include FAR records.>
            //sql = "SELECT temp.AssignDateStr " +
            //      "From " +
            //      "( " +
            //      "SELECT AssignDate AS AssignDateStr " +
            //       "FROM ClientAssign " +
            //       "WHERE AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999' " +
            //       "union all " +
            //       "SELECT AssignDate AS AssignDateStr " +
            //       "FROM LeadAssign " +
            //       "WHERE AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999' " +
            //       ")" +
            //       "as temp " +
            //       "GROUP BY temp.AssignDateStr " +
            //       "ORDER BY temp.AssignDateStr DESC ";

            sql = "SELECT" + "\r\n";
            sql += "    Temp.AssignDateStr" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "	(" + "\r\n";
            sql += "		SELECT" + "\r\n";
            sql += "			AssignDate AS AssignDateStr, AECode" + "\r\n";
            sql += "		FROM" + "\r\n";
            sql += "			ClientAssign" + "\r\n";
            sql += "		WHERE" + "\r\n";
            sql += "			AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999'" + "\r\n\r\n";

            sql += "		UNION ALL" + "\r\n\r\n";

            sql += "		SELECT" + "\r\n";
            sql += "			AssignDate AS AssignDateStr, AECode" + "\r\n";
            sql += "		FROM" + "\r\n";
            sql += "			LeadAssign" + "\r\n";
            sql += "		WHERE" + "\r\n";
            sql += "			AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999'" + "\r\n";
            sql += "	)AS Temp" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON Temp.AECode = DealerDetail.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    --DealerDetail.UserType NOT LIKE 'FAR%' AND Temp.AECode IN (SELECT DISTINCT DD.AECode FROM SuperAdmin SA INNER JOIN DealerDetail DD ON DD.UserID=SA.UserID AND DD.AEGroup=SA.AEGroup WHERE DD.UserID='" + UserID + "')" + "\r\n";
            sql += "    DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID = '" + UserID + "')" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    Temp.AssignDateStr" + "\r\n";
            sql += "ORDER BY" + "\r\n";
            sql += "    Temp.AssignDateStr DESC";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
        public DataTable RetrieveAssignmentDateByDateRangeByUserOrSupervisor(String UserID, String fromDate, String toDate, String Param)
        {
            sql = "SELECT" + "\r\n";
            sql += "    Temp.AssignDateStr" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "	(" + "\r\n";
            sql += "		SELECT" + "\r\n";
            sql += "			AssignDate AS AssignDateStr, AECode" + "\r\n";
            sql += "		FROM" + "\r\n";
            sql += "			ClientAssign" + "\r\n";
            sql += "		WHERE" + "\r\n";
            sql += "			AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999'" + "\r\n\r\n";

            sql += "		UNION ALL" + "\r\n\r\n";

            sql += "		SELECT" + "\r\n";
            sql += "			AssignDate AS AssignDateStr, AECode" + "\r\n";
            sql += "		FROM" + "\r\n";
            sql += "			LeadAssign" + "\r\n";
            sql += "		WHERE" + "\r\n";
            sql += "			AssignDate BETWEEN '" + fromDate + "' AND '" + toDate + " 23:59:59.999'" + "\r\n";

            #region Added by Thet Maung Chaw for Lead Sync

            sql += "            AND LeadAssign.LeadID" + SPMWebServiceApp.Utilities.CommonUtilities.SearchSyncRecords();

            #endregion

            sql += "	)AS Temp" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON Temp.AECode = DealerDetail.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail." + Param + " --AND Temp.AECode IN (SELECT DISTINCT DD.AECode FROM SuperAdmin SA INNER JOIN DealerDetail DD ON DD.UserID=SA.UserID AND DD.AEGroup=SA.AEGroup WHERE DD.UserID='" + UserID + "')" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    Temp.AssignDateStr" + "\r\n";
            sql += "ORDER BY" + "\r\n";
            sql += "    Temp.AssignDateStr DESC";

            return genericDA.ExecuteQueryForDataTable(sql);
        }
       
        /// <summary>
        /// SPM Phase III - Fetching Commission Earned for the Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet RetrieveCommissionEarnedByProjecId(String projectId)
        {
            sql = "SELECT " +
                        "t.AccServiceType AS AcctSvcType, SUM(t.TotalComm) AS TotalComm " +
                    "FROM " +
                        "TmpClientAssign t  " +
                            "INNER JOIN AccServiceType acctFilter ON " +
                                "acctFilter.AccServiceType = t.AccServiceType " +
                            "INNER JOIN " +
                                "( " +
                                    "Select  " +
                                        "ca.AcctNo, ca.AECode, ca.ProjectID " +
                                    "From " +
                                        "ClientAssign ca  " +
                                            "INNER JOIN DealerDetail d ON " +
                                                "ca.AECode = d.AECode  " +
                                                //--AND d.AEGroup IN (SELECT AEGroup FROM DealerDetail WHERE UserID = 'tsertayhx')
                                            "INNER JOIN ProjectDetail p ON " +
                                                "ca.ProjectID = p.ProjectID " +
                                    "Where p.ProjectID = '" + projectId + "' " +
                                ") AS temp ON " +
                                    "temp.AECode = t.AECode " +
                                    "AND temp.AcctNo = t.AcctNo " +
                    "GROUP BY t.AccServiceType ";
            return genericDA.ExecuteQuery(sql);
        }

        /// <summary>
        /// SPM Phase III - Fetching Client Calls for the Project 
        /// Returning No Of Client Calls left and No Of Client Follow-up
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// 
        public DataSet RetrieveClientCallsByProjectId(String userId, String projectId)
        {
            bool isAdmin = false; // if userId is not admin, then supervisor only.
            CommonServiceDA commonService = new CommonServiceDA(this.genericDA);
            DataSet dsDealer = commonService.RetrieveDealerCodeAndNameByUserID(userId);
            if (dsDealer != null && dsDealer.Tables[0].Rows.Count > 0)
            {
                if (dsDealer.Tables[0].Rows[0]["Supervisor"].ToString().Equals("Y"))
                {
                    isAdmin = true;
                }
            }
            else return null;

            sql = "SELECT * FROM( " +
                        "SELECT  " +
                            "clienttotalassign.aecode AS aecode,  " +
                            "clienttotalassign.aename AS aename,  " +
                            "( clienttotalassign.assignedclientno - ISNULL(noofclientcontact.totalclientcontact,0) ) AS noofcallsleft,  " +
                            "ISNULL(noofclientfollowup.nooffollowup,0) AS noofcallsfollowup  " +
                        "FROM    " +
                            "( " +
                                "SELECT  " +
                                    "ca.aecode AS aecode, d.AEName AS AeName, ca.projectid, COUNT(ca.projectid) AS assignedclientno  " +
                                "FROM    " +
                                    "dbo.clientassign ca  " +
                                        "INNER JOIN dealerdetail d  ON   ";
            if (!isAdmin)
            {
                sql = sql + "d.aecode IN (SELECT aecode FROM   dealerdetail WHERE aegroup = (SELECT aegroup FROM   dealerdetail WHERE  userid = '" + userId + "')) AND ";
            }
            sql = sql + "ca.aecode = d.aecode  " +
                                "WHERE   " +
                                    "ca.projectid = '" + projectId + "' AND  " +
                                    "ca.cutoffdate >= Getdate()  " +
                                "GROUP  BY ca.aecode, d.AEName, ca.projectid " +
                                "UNION " +
                                "SELECT  " +
                                    "la.aecode AS aecode, d.AEName AS AeName, la.projectid, COUNT(la.projectid) AS assignedclientno  " +
                                "FROM    " +
                                    "dbo.LeadAssign la  " +
                                        "INNER JOIN dealerdetail d  ON   ";
            if (!isAdmin)
            {
                sql = sql + "d.aecode IN (SELECT aecode FROM   dealerdetail WHERE aegroup = (SELECT aegroup FROM   dealerdetail WHERE  userid = '" + userId + "')) AND ";
            }
            sql = sql + "la.aecode = d.aecode  " +
                                "WHERE   " +
                                    "la.projectid = '" + projectId + "' AND  " +
                                    "la.cutoffdate >= Getdate()  " +
                                "GROUP  BY la.aecode, d.AEName, la.projectid	 " +
                            ") AS clienttotalassign  " +
                                "LEFT JOIN  " +
                                    "( " +
                                        "SELECT  " +
                                            "ca.aecode AS aecode,  " +
                                            "d.aename AS aename,  " +
                                            "ca.projectid AS projectid,  " +
                                            "COUNT(cc.recid) AS totalclientcontact  " +
                                        "FROM    " +
                                            "dbo.clientcontact cc  " +
                                                "INNER JOIN clientassign ca ON  " +
                                                    "cc.acctno = ca.acctno AND  " +
                                                    "cc.projectid = '" + projectId + "' AND  " +
                                                    "ca.cutoffdate >= Getdate()  " +
                                                "INNER JOIN  " +
                                                    "dealerdetail d ON  ";
            if (!isAdmin)
            {
                sql = sql + "d.aecode IN (SELECT aecode FROM   dealerdetail WHERE aegroup = (SELECT aegroup FROM   dealerdetail WHERE  userid = '" + userId + "')) AND " ;
            }
            sql = sql + "d.aecode = ca.aecode  " +
                                        "WHERE   " +
                                            "cc.followupby NOT IN ('F')   " +
                                        "GROUP BY ca.aecode, d.aename, ca.projectid " +
                                        "UNION " +
                                        "SELECT  " +
                                            "ca.aecode AS aecode,  " +
                                            "d.aename AS aename,  " +
                                            "ca.projectid AS projectid,  " +
                                            "COUNT(cc.recid) AS totalclientcontact  " +
                                        "FROM    " +
                                            "dbo.LeadContact cc  " +
                                                "INNER JOIN LeadAssign ca ON  " +
                                                    "cc.LeadID = ca.LeadID AND  " +
                                                    "cc.projectid = '" + projectId + "' AND  " +
                                                    "ca.cutoffdate >= Getdate()  " +
                                                "INNER JOIN  " +
                                                    "dealerdetail d ON  ";
            if (!isAdmin)
            {
                sql = sql + "d.aecode IN (SELECT aecode FROM   dealerdetail WHERE aegroup = (SELECT aegroup FROM   dealerdetail WHERE  userid = '" + userId + "')) AND ";
            }
            sql = sql + "d.aecode = ca.aecode  " +
				                        "WHERE   " +
					                        "cc.NeedFollowUp NOT IN ('Y')   " +
				                        "GROUP BY ca.aecode, d.aename, ca.projectid " +
                        				
			                        ") AS noofclientcontact ON  " +
				                        "clienttotalassign.aecode = noofclientcontact.aecode  " +
		                        "LEFT JOIN " + 
			                        "( " +
				                        "SELECT  " +
					                        "followupby AS followupby,  " +
					                        "COUNT(recid) AS nooffollowup  " +
				                        "FROM    " +
					                        "dbo.clientcontact  " +
				                        "WHERE   " +
					                        "followupstatus = 'N' AND  " +
					                        "followupby IS NOT NULL AND  " +
					                        "followupdate IS NOT NULL AND  " +
                                            "projectid = '" + projectId + "'  " +
				                        "GROUP BY followupby " +
				                        "UNION " +
				                        "SELECT  " +
					                        "ModifiedUser AS followupby,  " +
					                        "COUNT(recid) AS nooffollowup  " +
				                        "FROM    " +
					                        "dbo.LeadContact  " +
				                        "WHERE   " +
					                        "NeedFollowUp = 'Y' AND 		 " +			
					                        "followupdate IS NOT NULL AND  " +
                                            "projectid = '" + projectId + "' " + 
				                        "GROUP BY ModifiedUser " +
			                        ") AS noofclientfollowup ON  " +
				                        "clienttotalassign.aecode = noofclientfollowup.followupby) A " +
                        "WHERE A.noofcallsleft >= 0 ";
            return genericDA.ExecuteQuery(sql);
        }

        //public DataSet RetrieveAssignedProjectByUserId(String userId)
        //{
        //    sql = "SELECT totalassigned.projectid AS projectid, " +
        //               "totalassigned.projectname AS projectname, " +
        //               "totalassigned.assigndate AS assigneddate, " +
        //               "totalassigned.cutoffdate AS cutoffdate, " +
        //               "(totalassigned.assignedclient - ISNULL(totalcontact.totalcontacted,0) ) AS callsleft, " +
        //               "ISNULL(totalcallstofollowup.nooffollowup, 0) AS callstofollowup, " +
        //               "totalcallstofollowup.followupdate AS followupdate " +
        //        "FROM   (SELECT p.projectid, " +
        //                       "p.projectname, " +
        //                       "p.assigndate, " +
        //                       "p.cutoffdate, " +
        //                       "d.aecode, " +
        //                       "COUNT(p.projectid) AS assignedclient " +
        //                "FROM   clientassign ca " +
        //                       "INNER JOIN projectdetail p " +
        //                         "ON ca.projectid = p.projectid " +
        //                       "INNER JOIN dealerdetail d " +
        //                         "ON ca.aecode = d.aecode and d.usertype not like 'FAR%' " +
        //                "WHERE  d.userid = '" + userId +"' " +
        //                       "AND p.projecttype = 'C'  " +
        //                       "AND ca.CutOffDate >= GETDATE() " +
        //                "GROUP  BY p.projectid, " +
        //                          "p.projectname, " +
        //                          "p.assigndate, " +
        //                          "p.cutoffdate, " +
        //                          "d.aecode) AS totalassigned " +
        //               "LEFT JOIN (SELECT cc.projectid, " +
        //                                  "COUNT(cc.projectid) AS totalcontacted " +
        //                           "FROM   clientcontact cc " +
        //                                  "INNER JOIN clientassign ca " +
        //                                    "ON cc.projectid = ca.projectid " +
        //                                       "AND cc.acctno = ca.acctno " +
        //                                       "AND ca.CutOffDate >= GETDATE() " +
        //                                  "INNER JOIN projectdetail p " +
        //                                    "ON p.projectid = cc.projectid " +
        //                                  "INNER JOIN dealerdetail d " +
        //                                    "ON d.aecode = ca.aecode AND d.usertype not like 'FAR%' " +
        //                           "WHERE  d.userid = '" + userId + "'  " +
        //                                  "AND p.projecttype = 'C' " +
        //                                  "AND cc.FollowUpStatus NOT IN ('F') " +
        //                           "GROUP  BY cc.projectid) AS totalcontact " +
        //                 "ON totalassigned.projectid = totalcontact.projectid " +
        //               "LEFT JOIN (SELECT cc.projectid    AS projectid, " +
        //                                  "cc.followupby   AS followupby, " +
        //                                  "followupdatetbl.followupdate, " +
        //                                  "COUNT(cc.recid) AS nooffollowup " +
        //                           "FROM   dbo.clientcontact cc " +
        //                                  "INNER JOIN dealerdetail d " +
        //                                    "ON cc.followupby = d.aecode AND d.usertype not like 'FAR%' " +
        //                                       "AND d.userid = '" + userId + "' \r\n" +
        //                                  "--INNER JOIN clientassign ca \r\n" +
        //                                    "--ON cc.projectid = ca.projectid \r\n" +
        //                                       "--AND ca.aecode = d.aecode \r\n" +
        //                                       "--AND cc.acctno = ca.acctno \r\n" +
        //                                  "INNER JOIN (SELECT cc.projectid         AS projectid, " +
        //                                                     "MAX(cc.followupdate) AS " +
        //                                                     "followupdate " +
        //                                              "FROM   clientcontact cc " +
        //                                                     "INNER JOIN dealerdetail d " +
        //                                                       "ON cc.followupby = d.aecode and d.usertype not like 'FAR%' " +
        //                                                          "AND d.userid = '" + userId + "' \r\n" +
        //                                                     "--INNER JOIN clientassign ca \r\n" +
        //                                                       "--ON cc.projectid = ca.projectid \r\n" +
        //                                                          "--AND ca.aecode = d.aecode \r\n" +
        //                                                          "--AND ca.acctno = cc.acctno \r\n" +
        //                                              "WHERE  cc.followupstatus = 'N' " +
        //                                                     "AND cc.followupby IS NOT NULL " +
        //                                                     "AND cc.followupdate IS NOT NULL " +
        //                                              "GROUP  BY cc.projectid) AS followupdatetbl " +
        //                                    "ON followupdatetbl.projectid = cc.projectid " +
        //                           "WHERE  cc.followupstatus = 'N' " +
        //                                  "AND cc.followupby IS NOT NULL " +
        //                                  "AND cc.followupdate IS NOT NULL " +
        //                           "GROUP  BY cc.projectid, " +
        //                                     "cc.followupby, " +
        //                                     "followupdatetbl.followupdate) AS " +
        //                          "totalcallstofollowup " +
        //                 "ON totalcallstofollowup.projectid = totalassigned.projectid   ";
            
        //    return genericDA.ExecuteQuery(sql);
        //}

        public DataSet RetrieveAssignedProjectByUserId(String userId)
        {
            sql = "-- Assign Not In (Contact) " + "\r\n\r\n" +


"-- Assign " + "\r\n" +
"SELECT PD.ProjectID, PD.ProjectName, COUNT(CA.AcctNo) AS CallsLeft, 0 AS CallsToFollowUp, NULL AS FollowUpDate, PD.AssignDate, PD.CutOffDate " + "\r\n" +
"FROM ClientAssign CA " + "\r\n" +
"INNER JOIN ProjectDetail PD ON CA.ProjectID=PD.ProjectID " + "\r\n" +
"WHERE AECode IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "' AND UserType NOT LIKE 'FAR%') AND CA.CutoffDate>=GETDATE() " + "\r\n" +
"AND PD.ProjectID NOT IN ( " + "\r\n\r\n" +

"-- Contact " + "\r\n" +
"SELECT PD.ProjectID " + "\r\n" +
"FROM ClientContact CC " + "\r\n" +
"INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID " + "\r\n" +
"INNER JOIN ClientAssign CA ON CC.AcctNo=CA.AcctNo AND PD.ProjectID=CA.ProjectID " + "\r\n" +
"WHERE CC.ModifiedUser IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "' AND UserType NOT LIKE 'FAR%') " + "\r\n" +
"AND PD.ProjectType='C' AND CC.FollowUpStatus <> 'F' AND CA.CutOffDate >= GETDATE() " + "\r\n" +
"GROUP BY PD.ProjectID, PD.ProjectName " + "\r\n\r\n" +

") " + "\r\n" +
"GROUP BY PD.ProjectID, PD.ProjectName, PD.AssignDate, PD.CutOffDate " + "\r\n\r\n\r\n\r\n" +



"UNION ALL " + "\r\n\r\n\r\n\r\n" +


"-- (Assign-Contact) " + "\r\n" +
"SELECT A.ProjectID, A.ProjectName, A.Assigned - B.Contacted AS CallsLeft, 0 AS CallsToFollowUp, NULL AS FollowUpDate, A.AssignDate, A.CutOffDate FROM " + "\r\n" +
"-- Assign " + "\r\n" +
"( " + "\r\n" +
"SELECT PD.ProjectID, PD.ProjectName, COUNT(CA.AcctNo) AS Assigned, 0 AS CallsToFollowUp, NULL AS FollowUpDate, PD.AssignDate, PD.CutOffDate " + "\r\n" +
"FROM ClientAssign CA " + "\r\n" +
"INNER JOIN ProjectDetail PD ON CA.ProjectID=PD.ProjectID " + "\r\n" +
"WHERE AECode IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "' AND UserType NOT LIKE 'FAR%') AND CA.CutoffDate>=GETDATE() " + "\r\n" +
"GROUP BY PD.ProjectID, PD.ProjectName, PD.AssignDate, PD.CutOffDate)A " + "\r\n\r\n" +

"INNER JOIN " + "\r\n\r\n" +

"-- Contact " + "\r\n" +
"(SELECT PD.ProjectID, PD.ProjectName, COUNT(CC.AcctNo) AS Contacted, 0 AS CallsToFollowUp, NULL AS FollowUpDate, PD.AssignDate, PD.CutOffDate " + "\r\n" +
"FROM ClientContact CC " + "\r\n" +
"INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID " + "\r\n" +
"INNER JOIN ClientAssign CA ON CC.AcctNo=CA.AcctNo AND PD.ProjectID=CA.ProjectID " + "\r\n" +
"WHERE CC.ModifiedUser IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "' AND UserType NOT LIKE 'FAR%') " + "\r\n" +
"AND PD.ProjectType='C' AND CC.FollowUpStatus <> 'F' AND CA.CutOffDate >= GETDATE() " + "\r\n" +
"GROUP BY PD.ProjectID, PD.ProjectName, PD.AssignDate, PD.CutOffDate)B ON A.ProjectID=B.ProjectID" + "\r\n\r\n\r\n\r\n" +








"UNION ALL " + "\r\n\r\n\r\n\r\n" +

"-- Only FollowUp " + "\r\n" +
"SELECT PD.ProjectID, PD.ProjectName, 0 AS CallsLeft, COUNT(CC.AcctNo) AS CallsToFollowUp, CC.FollowUpDate, PD.AssignDate, PD.CutOffDate " + "\r\n" +
"FROM ClientContact CC " + "\r\n" +
"INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID " + "\r\n" +
"INNER JOIN ClientAssign CA ON CC.AcctNo=CA.AcctNo AND PD.ProjectID=CA.ProjectID " + "\r\n" +
"WHERE FollowUpBy IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "' AND UserType NOT LIKE 'FAR%') " + "\r\n" +
"AND PD.ProjectType='C' AND CC.FollowUpStatus = 'N' AND (CC.FollowUpBy IS NOT NULL AND CC.FollowUpDate IS NOT NULL) AND CA.CutOffDate>=GETDATE() " + "\r\n" +
"GROUP BY PD.ProjectID, PD.ProjectName, CC.FollowUpDate, PD.AssignDate, PD.CutOffDate";

            return genericDA.ExecuteQuery(sql);
        }

        public DataTable RetrieveProjectByProjectName(String ProjName, String UserID)
        {
            // sql = "SELECT ProjectID,ProjectName FROM [SPM].[dbo].[ProjectDetail]where ProjectName like '%" + ProjName + "%'";

            /// <Modified by Thet Maung Chaw not to include FAR records.>
            //sql = "SELECT ProjectID,ProjectName, AssignDate, CutoffDate FROM [dbo].[ProjectDetail]where ProjectName like '%" + ProjName + "%' ORDER BY ProjectName";

            sql = "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ProjectDetail.ProjectName," + "\r\n";
            sql += "    ProjectDetail.ProjectType," + "\r\n";
            sql += "    ProjectDetail.AssignDate," + "\r\n";
            sql += "    ProjectDetail.CutoffDate" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "	ProjectDetail" + "\r\n";
            sql += "    INNER JOIN ClientAssign ON ProjectDetail.ProjectID = ClientAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON DealerDetail.AECode = ClientAssign.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail.UserType NOT LIKE 'FAR%'" + "\r\n";
            sql += "    AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'" + "\r\n";
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" +  UserID + "')" + "\r\n\r\n";

            sql += "UNION ALL" + "\r\n\r\n";

            sql += "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ProjectDetail.ProjectName," + "\r\n";
            sql += "    ProjectDetail.ProjectType," + "\r\n";
            sql += "    ProjectDetail.AssignDate," + "\r\n";
            sql += "    ProjectDetail.CutoffDate" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    ProjectDetail" + "\r\n";
            sql += "    INNER JOIN LeadAssign ON ProjectDetail.ProjectID = LeadAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON DealerDetail.AECode = LeadAssign.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail.UserType NOT LIKE 'FAR%'" + "\r\n";
            sql += "    AND ProjectDetail.ProjectName LIKE '%" + ProjName + "%'" + "\r\n";
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
        public DataTable RetrieveProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
        {
            sql = "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ProjectDetail.ProjectName," + "\r\n";
            sql += "    ProjectDetail.ProjectType," + "\r\n";
            sql += "    ProjectDetail.AssignDate," + "\r\n";
            sql += "    ProjectDetail.CutoffDate" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "	ProjectDetail" + "\r\n";
            sql += "    INNER JOIN ClientAssign ON ProjectDetail.ProjectID = ClientAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON DealerDetail.AECode = ClientAssign.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail." + Param + "\r\n";
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')" + "\r\n\r\n";

            sql += "UNION ALL" + "\r\n\r\n";

            sql += "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ProjectDetail.ProjectName," + "\r\n";
            sql += "    ProjectDetail.ProjectType," + "\r\n";
            sql += "    ProjectDetail.AssignDate," + "\r\n";
            sql += "    ProjectDetail.CutoffDate" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    ProjectDetail" + "\r\n";
            sql += "    INNER JOIN LeadAssign ON ProjectDetail.ProjectID = LeadAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON DealerDetail.AECode = LeadAssign.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail." + Param;
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";

            return genericDA.ExecuteQueryForDataTable(sql);
        }
        
      
        // "LEFT JOIN SPM.dbo.ClientLTD CLTD WITH (NOLOCK) ON CA.AcctNo=CLTD.AcctNo " +
       

        public DataTable RetrieveProjectType(string ProjID)
        {
            string sql = "select ProjectType from [SPM].[dbo].[ProjectDetail] where ProjectID ='" + ProjID + "'  ";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <summary>
        /// Created by:		THA (CyberQuote)
        /// </summary>
        /// 
        public DataTable RetrieveCommissionEarnedHistoryByCriteria(string dealerCode, string accountNo, string nric,
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

            sql = "SELECT " +
                    "acctFilter.AccServiceType AS AccServiceType, SUM(TCA.TotalComm) AS TotalComm " +
                    "FROM ClientAssign CLA " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON CLA.AcctNo = CLM.LACCT " +
                    "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCA.AcctNo = CLA.AcctNO " +
                    "INNER JOIN AccServiceType acctFilter WITH (NOLOCK) ON TCA.AccServiceType = acctFilter.AccServiceType " +
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
                if (whereFlag)
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

            sb.Append(" GROUP BY acctFilter.AccServiceType ");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }
        
        public DataTable RetrieveCommissionEarnedHistoryByProjName(string dealerCode, string accountNo, string nric,
                        string ProjName, bool retradeFlag)
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

            sql = "SELECT " +
                    "acctFilter.AccServiceType AS AccServiceType, SUM(TCA.TotalComm) AS TotalComm " +
                    "FROM ClientAssign CLA " +
                    "LEFT JOIN CLMAST CLM WITH (NOLOCK) ON CLA.AcctNo = CLM.LACCT " +
                    "LEFT JOIN TmpClientAssign TCA WITH (NOLOCK) ON TCA.AcctNo = CLA.AcctNO " +
                    "INNER JOIN AccServiceType acctFilter WITH (NOLOCK) ON TCA.AccServiceType = acctFilter.AccServiceType " +
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
                if (whereFlag)
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

            if (!String.IsNullOrEmpty(ProjName))
            {
                if (whereFlag)
                {
                    sb.Append(" AND CLA.ProjectID = '").Append(ProjName).Append("' ");
                }
                else
                {
                    whereFlag = true;
                    sb.Append(" WHERE CLA.ProjectID = '").Append(ProjName).Append("' ");
                }
            }

            /*  if ((!String.IsNullOrEmpty(fromDate)) && (!String.IsNullOrEmpty(toDate)))
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
              }*/

            //New added for retrieving LastTradeDate only
            if (retradeFlag)
            {
                //sb.Append(" AND TCA.LastTradeDate > TAssignLTD.LTradeDate ");
                sb.Append(" AND DATEDIFF(dd , TAssignLTD.LTradeDate , LTD.OverallLTD) >= 0 ");
            }

            sb.Append(" GROUP BY acctFilter.AccServiceType ");

            return genericDA.ExecuteQueryForDataTable(sb.ToString());
        }
        
        /// <summary>
        /// Created by:		Thiri (CyberQuote)
        /// </summary>
        /// 

        private string GetFollowUpQuery(string dealerCode)
        {
            string query = "";
            query = "SELECT ATC.ProjectID,ATC.ProjectName As ProjectName, ISNULL((ATC.TotalAssign)-(TTC.TotalCalls),0) As NoCallsLeft," +
                       "ISNULL((ATC.TotalAssign)-(CTC.FollowUpCalls),0) As NoCallsToFollowUp,FollowUpDate,CutOffDate As CutOffDateTime " +
                       "FROM (SELECT ProjectID,IsNull(COUNT(AcctNo),0) As FollowUpCalls,FollowUpDate FROM ClientContact WHERE DATEDIFF(dd,GetDate(),FollowUpDate)in(2,3,4)  " +
                       "AND FollowUpStatus='N' AND FollowUpDate IS NOT NULL AND FollowUpBy='" + dealerCode + "'  GROUP BY ProjectID,FollowUpDate ) CTC " +
                       "INNER JOIN (SELECT AECode,PD.ProjectID,PD.ProjectName,PD.CutOffDate As CutOffDate,IsNull(COUNT(CA.AcctNo),0) AS TotalAssign " +
                       "FROM ClientAssign CA INNER JOIN ProjectDetail PD ON PD.ProjectID=CA.ProjectID " +
                       "WHERE AECode='" + dealerCode + "'   GROUP BY AECode,PD.AssignDate,PD.CutOffDate,PD.ProjectID,PD.ProjectName ) ATC " +
                       "ON CTC.ProjectID=ATC.ProjectID " +
                       "INNER JOIN (SELECT CC.ProjectID,IsNull(COUNT(CC.AcctNo),0) As TotalCalls FROM ClientContact CC INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID WHERE ModifiedUser='" + dealerCode + "'" +
                       "GROUP BY CC.ProjectID) TTC ON ATC.ProjectID=TTC.ProjectID ";
            return query;
        }

        private string GetFollowUpQuery(string dealerCode, int daydiff)
        {
            string query = "";
            query = "SELECT ATC.ProjectID,ATC.ProjectName As ProjectName, ISNULL((ATC.TotalAssign)-(TTC.TotalCalls),0) As NoCallsLeft," +
                       "ISNULL((ATC.TotalAssign)-(CTC.FollowUpCalls),0) As NoCallsToFollowUp,FollowUpDate,CutOffDate As CutOffDateTime " +
                       "FROM (SELECT ProjectID,IsNull(COUNT(AcctNo),0) As FollowUpCalls,FollowUpDate FROM ClientContact WHERE DATEDIFF(dd,GetDate(),FollowUpDate)= " + daydiff + "  " +
                       "AND FollowUpStatus='N' AND FollowUpDate IS NOT NULL AND FollowUpBy='" + dealerCode + "'  GROUP BY ProjectID,FollowUpDate ) CTC " +
                       "INNER JOIN (SELECT AECode,PD.ProjectID,PD.ProjectName,PD.CutOffDate As CutOffDate,IsNull(COUNT(CA.AcctNo),0) AS TotalAssign " +
                       "FROM ClientAssign CA INNER JOIN ProjectDetail PD ON PD.ProjectID=CA.ProjectID " +
                       "WHERE AECode='" + dealerCode + "'   GROUP BY AECode,PD.AssignDate,PD.CutOffDate,PD.ProjectID,PD.ProjectName ) ATC " +
                       "ON CTC.ProjectID=ATC.ProjectID " +
                       "INNER JOIN (SELECT CC.ProjectID,IsNull(COUNT(CC.AcctNo),0) As TotalCalls FROM ClientContact CC INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID WHERE ModifiedUser='" + dealerCode + "'" +
                       "GROUP BY CC.ProjectID) TTC ON ATC.ProjectID=TTC.ProjectID ";
            return query;
        }        

        public DataTable BatchDeleteClientAssignment(DataTable dtAssignDelete)
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("DataRowIndex", String.Empty.GetType());
            dtResult.Columns.Add("dealerCode", String.Empty.GetType());
            dtResult.Columns.Add("accountNumber", String.Empty.GetType());
            dtResult.Columns.Add("assignDate", String.Empty.GetType());
            dtResult.Columns.Add("result", String.Empty.GetType());

            int result = -1;

            for (int i = 0; i < dtAssignDelete.Rows.Count; i++)
            {
                string dataRowIndex = "";
                string dealerCode = "";
                string accountNumber = "";
                string assignDate = "";
                string cutOffDate = "";
                string modifiedUser = "";
                string modifiedDate = "";
                string newModifiedUser = "";
                DataRow drResult = null;

                dataRowIndex = dtAssignDelete.Rows[i]["DataRowIndex"].ToString();
                dealerCode = dtAssignDelete.Rows[i]["dealerCode"].ToString();
                accountNumber = dtAssignDelete.Rows[i]["accountNumber"].ToString();
                assignDate = dtAssignDelete.Rows[i]["assignDate"].ToString();
                cutOffDate = dtAssignDelete.Rows[i]["cutOffDate"].ToString();
                modifiedUser = dtAssignDelete.Rows[i]["modifiedUser"].ToString();
                modifiedDate = dtAssignDelete.Rows[i]["modifiedDate"].ToString();
                newModifiedUser = dtAssignDelete.Rows[i]["newModifiedUser"].ToString();

                sql = "DELETE FROM ClientAssign WHERE AECode = '" + dealerCode + "' AND AcctNo = '" + accountNumber
                        + "' AND AssignDate = '" + assignDate + "'";

                result = genericDA.ExecuteNonQuery(sql);
                if (result > 0)
                {

                    string auditLogSql = "SPM_spAssignmentAuditIns";
                    OleDbCommand cmdAuditLog = genericDA.GetSqlCommand();

                    //Parameters for AuditLog
                    OleDbParameter[] sqlAuditLogParams = new OleDbParameter[] 
                                            { 
                                                new OleDbParameter("@istrAECode", OleDbType.VarChar),   
                                                new OleDbParameter("@istrAcctNo", OleDbType.VarChar),
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
                    cmdAuditLog.Parameters["@istrAcctNo"].Value = accountNumber;
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
                drResult["accountNumber"] = accountNumber;
                drResult["assignDate"] = assignDate;
                drResult["result"] = result;
                dtResult.Rows.Add(drResult);
            }
            return dtResult;
        }

        public DataTable RetrieveProjectAttachment(string projectID)
        {
            sql = "SELECT RecID,FilePath,FileName,FileExtension,FileSize,ProjectID " +
                  "FROM ProjectAttachment WHERE ProjectID='" + projectID + "' ORDER BY RECID ";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataSet RetrieveAssignReportByDealer(string dealerCode)
        {
            sql = "SELECT AECode,ISNULL(COUNT(CA.AcctNo),0) AS TotalAssign,PD.AssignDate,PD.CutOffDate,PD.ProjectID,PD.ProjectName," +
                "ISNULL(DATEDIFF(dd,PD.AssignDate,PD.CutOffDate),0) AS AssignDays," +
                "ISNULL(CEILING(DATEDIFF(dd,PD.AssignDate,PD.CutOffDate)/2),0) As AssignPeriodHalf," +
                "ISNULL(DATEDIFF(dd,GetDate(),PD.CutOffDate),0) As AssignedDayLeft,ISNULL(TC.TotalCalls,0) AS TotalCalls " +
                "FROM ClientAssign CA " +
                "INNER JOIN ProjectDetail PD ON CA.ProjectID=PD.ProjectID " +
                "LEFT JOIN (SELECT CC.ProjectID,ISNULL(COUNT(CC.AcctNo),0) As TotalCalls " +
                "FROM ClientContact CC " +
                "INNER JOIN ProjectDetail PD ON CC.ProjectID=PD.ProjectID  " +
                "WHERE ModifiedUser='" + dealerCode + "' " +
                "GROUP BY CC.ProjectID ) TC ON TC.ProjectID=PD.ProjectID " +
                "WHERE GETDATE() <= PD.CutOffDate " +
                "and (DATEDIFF(dd,GetDate(),PD.CutOffDate)) <= (DATEDIFF(dd,PD.AssignDate,PD.CutOffDate)/2) " +
                "and AECode='" + dealerCode + "' " +
                "GROUP BY AECode,PD.AssignDate,PD.CutOffDate,PD.ProjectID,TC.TotalCalls,PD.ProjectName  " +
                "ORDER by AssignDate ";
            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveFollowUpDateByProjectID(string projectID, string dealerCode)
        {
            sql = "SELECT FC.ProjectID,ISNULL((TA.TotalAssign-FC.FollowUpCalls),0) AS NoCallsToFollowUp,ISNULL(TA.TotalAssign,0) As TotalAssign,FC.FollowUpCalls,FC.FollowUpDate " +
            "FROM (SELECT ProjectID,ISNULL(COUNT(AcctNo),0) As FollowUpCalls,FollowUpDate FROM ClientContact " +
            "WHERE FollowUpStatus='Y' AND FollowUpBy='" + dealerCode + "' AND ProjectID='" + projectID + "' " +
            "GROUP BY ProjectID,FollowUpDate) FC " +
            "LEFT JOIN ( SELECT ISNULL(COUNT(AcctNo),0) AS TotalAssign,ProjectID FROM ClientAssign WHERE ProjectID='" + projectID + "' " +
            "GROUP BY ProjectID) TA ON FC.ProjectID=TA.ProjectID ";
            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveAssignReportByFollowUpDate(string dealerCode)
        {

            /* if GetDate() equals Monday, TuesDays, Wednesday= > Sent ( Wednesday, Thursday, Friday ) FollowUpDate
             * if GetDate() equals Thursday => Sent ( Saturday, Sunday ,Monday ) FollowUpDate  -2,3,4
             * if GetDate() equals Friday => Sent ( Tuesday ) FollowupDate
             **/

            sql = "SELECT DATENAME(dw,GETDATE())";
            DataTable dtToday = genericDA.ExecuteQueryForDataTable(sql);
            string todayDate = dtToday.Rows[0][0].ToString();
            sql = "";          
            if (todayDate == "Thursday")
            {
                sql += GetFollowUpQuery(dealerCode);
            }
            else if (todayDate == "Friday")
            {
                sql = GetFollowUpQuery(dealerCode, 4);
            }
            else
            {
                sql += GetFollowUpQuery(dealerCode, 2);
            }
            return genericDA.ExecuteQuery(sql);
        }

        public string InsertProjectInfo(string projectName, string projectObj, string projectType, string filePath, DateTime assignDate, DateTime cutOffDate)
        {
            string result = "";
            string projectID = "";
            projectID = GenerateProjectID();
            if (projectID != "")
            {
               OleDbParameter[] sqlParams = new OleDbParameter[] 
                                        { 
                                            new OleDbParameter("@ProjectID", OleDbType.VarChar), 
                                            new OleDbParameter("@ProjectName", OleDbType.VarChar),
                                            new OleDbParameter("@ProjectObjective", OleDbType.VarChar),
                                            new OleDbParameter("@ProjectType", OleDbType.VarChar),
                                            new OleDbParameter("@AssignDate", OleDbType.Date),
                                            new OleDbParameter("@CutoffDate", OleDbType.Date),
                                            new OleDbParameter("@FilePath", OleDbType.VarChar)
                                        };

                OleDbCommand cmd1 = null;
                OleDbTransaction sqlTransaction = null;

                sql = "INSERT INTO ProjectDetail(ProjectID,ProjectName, ProjectObjective, ProjectType,AssignDate,CutoffDate,FilePath)" +
                            "VALUES (?, ?, ?, ?, ?, ?, ?)";
                //Start Transaction
                sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

                //Create Insert command
                cmd1 = genericDA.GetSqlCommand();
                cmd1.Transaction = sqlTransaction;
                cmd1.CommandText = sql;
                cmd1.Parameters.AddRange(sqlParams);
                try
                {
                    cmd1.Parameters["@ProjectID"].Value = projectID;
                    cmd1.Parameters["@ProjectName"].Value = projectName;
                    cmd1.Parameters["@ProjectObjective"].Value = projectObj;
                    cmd1.Parameters["@ProjectType"].Value = projectType;
                    cmd1.Parameters["@AssignDate"].Value = assignDate;
                    cmd1.Parameters["@CutoffDate"].Value = cutOffDate;
                    cmd1.Parameters["@FilePath"].Value = filePath;
                    cmd1.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception e)
                {
                    sqlTransaction.Rollback();
                    result = "";
                }
                finally
                {

                }
                result = projectID;
            }
            return result;
        }

        private string GenerateProjectID()
        {
            int generateID = 0000001;
            string projectID = "";
            OleDbCommand cmd = null;
            string insertedIdStr = "";
            try
            {
                projectID = "P";
                sql = " SELECT MAX(ProjectID) from ProjectDetail";
                cmd = genericDA.GetSqlCommand();
                cmd.CommandText = sql;
                insertedIdStr = cmd.ExecuteScalar().ToString();
                if (!String.IsNullOrEmpty(insertedIdStr))
                {
                    insertedIdStr = insertedIdStr.Substring(2);
                    generateID = Convert.ToInt32(insertedIdStr) + 1;
                    if (generateID.ToString().Length < 6)
                    {
                        for (int i = 0; i < (6 - (generateID.ToString().Length)); i++)
                        {
                            projectID = projectID + "0";
                        }
                    }
                    projectID = projectID + generateID.ToString();
                }
                else
                {
                    projectID = projectID + "000001";
                }
            }
            catch
            {
                projectID = "";
            }
            finally
            {
            }
            return projectID;
        }

        public string InsertProjectAttachment(string filePath, string fileName, string fileExtension, string fileSize, string projectID)
        {
            string result = "";

            OleDbParameter[] sqlParams = new OleDbParameter[] 
                                    { 
                                        new OleDbParameter("@filePath", OleDbType.VarChar), 
                                        new OleDbParameter("@fileName", OleDbType.VarChar),
                                        new OleDbParameter("@fileExtension", OleDbType.VarChar),
                                        new OleDbParameter("@fileSize", OleDbType.Double),
                                        new OleDbParameter("@projectID", OleDbType.VarChar)
                                    };

            OleDbCommand cmd1 = null;
            OleDbTransaction sqlTransaction = null;

            sql = "INSERT INTO ProjectAttachment(FilePath,FileName,FileExtension,FileSize,ProjectID)" +
                        " VALUES (?, ?, ?, ?, ?)";
            //Start Transaction
            sqlTransaction = genericDA.GetSqlConnection().BeginTransaction();

            //Create Insert command
            cmd1 = genericDA.GetSqlCommand();
            cmd1.Transaction = sqlTransaction;
            cmd1.CommandText = sql;
            cmd1.Parameters.AddRange(sqlParams);
            try
            {
                cmd1.Parameters["@filePath"].Value = filePath;
                cmd1.Parameters["@fileName"].Value = fileName;
                cmd1.Parameters["@fileExtension"].Value = fileExtension;
                cmd1.Parameters["@fileSize"].Value = fileSize;
                cmd1.Parameters["@projectID"].Value = projectID;

                cmd1.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                result = "";
            }
            finally
            {

            }
            return result;
        }

        public DataTable RetrieveAllProjectInfo()
        {
            //sql = "  SELECT ProjectID,ISNULL(ProjectName,'') As ProjectName,ISNULL(ProjectObjective,'') As ProjectObjective,ProjectType,ISNULL(FilePath,'') As FilePath FROM [SPM].[dbo].[ProjectDetail]";

            sql = "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ISNULL(ProjectName, '') AS ProjectName," + "\r\n";
            sql += "    ISNULL(ProjectObjective, '') AS ProjectObjective," + "\r\n";
            sql += "    ProjectType," + "\r\n";
            sql += "    ISNULL(FilePath, '') AS FilePath" + "\r\n";
            sql += "FROM [ProjectDetail]" + "\r\n";
            sql += "    INNER JOIN ClientAssign ON ProjectDetail.ProjectID = ClientAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON ClientAssign.AECode = DealerDetail.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail.UserType NOT LIKE 'FAR%'" + "\r\n\r\n";

            sql += "UNION ALL" + "\r\n\r\n";

            sql += "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ISNULL(ProjectName, '') AS ProjectName," + "\r\n";
            sql += "    ISNULL(ProjectObjective, '') AS ProjectObjective," + "\r\n";
            sql += "    ProjectType," + "\r\n";
            sql += "    ISNULL(FilePath, '') AS FilePath" + "\r\n";
            sql += "FROM [ProjectDetail]" + "\r\n";
            sql += "    INNER JOIN LeadAssign ON ProjectDetail.ProjectID = LeadAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON LeadAssign.AECode = DealerDetail.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail.UserType NOT LIKE 'FAR%'";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrieveContactEntryProjectInfo(String UserID)
        {
            /// <Modified by Thet Maung Chaw not to include FAR records.>
            //sql = "  SELECT ProjectID,ISNULL(ProjectName,'') As ProjectName,ISNULL(ProjectObjective,'') As ProjectObjective,ProjectType,ISNULL(FilePath,'') As FilePath FROM [SPM].[dbo].[ProjectDetail] WHERE ProjectType='C' ORDER BY ProjectName";

            sql = "SELECT DISTINCT" + "\r\n";
            sql += "    ProjectDetail.ProjectID," + "\r\n";
            sql += "    ISNULL(ProjectName, '') AS ProjectName," + "\r\n";
            sql += "    ISNULL(ProjectObjective, '') AS ProjectObjective," + "\r\n";
            sql += "    ProjectType," + "\r\n";
            sql += "    ISNULL(FilePath, '') AS FilePath" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    ProjectDetail" + "\r\n";
            sql += "    INNER JOIN ClientAssign ON ProjectDetail.ProjectID = ClientAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON ClientAssign.AECode = DealerDetail.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail.UserType NOT LIKE 'FAR%'" + "\r\n";
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')" + "\r\n";
            sql += "ORDER BY" + "\r\n";
            sql += "    ProjectName";

            return genericDA.ExecuteQueryForDataTable(sql, "dtProjectInfo");
        }

        public DataTable RetrieveAccountTypeValues(string[] accList)
        {
            string param = null;
            param = "(";
            for (int i = 0; i < accList.Length; i++)
            {
                param = param + "'" + accList[i] + "'";
                if (i < accList.Length - 1)
                {
                    param = param + ",";
                }
            }
            param = param + ")";
            //sql = "SELECT AccServiceType,AccServiceDesp,AllowConHist,AllowTrustBal,AllowMMFBal,AllowTPeriod,AllowSMarketVal " +
            //            "FROM AccountFilter " +
            //            "WHERE AccServiceType IN " + param + "";
            sql = "SELECT AccServiceType,AccServiceDesc,[AllowConhist], [AllowTrustBal], [AllowMMFBal], [AllowTPeriod], [AllowSMarketVal] " +
                    "FROM  " +
                    "(SELECT a.AccServiceType, a.AccServiceDesc, f.SearchCriteria, f.Allow " +
                    "FROM AccServiceType a " +
                        "INNER JOIN AccServiceTypeFilter f ON " +
                            "a.AccServiceType = f.AccServiceType " +
                    "WHERE a.AccServiceType IN " + param + ") AS PivotTbl " +
                    "PIVOT " +
                    "( " +
                    "MAX(Allow) " +
                    "FOR SearchCriteria IN ([AllowConhist], [AllowMMFBal], [AllowSMarketVal], [AllowTPeriod], [AllowTrustBal])   " +
                    ") AS PivotedTable ";
            return genericDA.ExecuteQueryForDataTable(sql);
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
                                            new OleDbParameter("@Contents", OleDbType.VarChar)
                                        };

            OleDbCommand cmd = null; 
            OleDbTransaction sqlTransaction = null;

            sql = "INSERT INTO EmailLog(FromEmail, ToEmail, LogDate, Subject, Contents) " +
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
                cmd.Parameters["@FromEmail"].Value = "PhillipCapital <" + fromEmail + ">";
                cmd.Parameters["@ToEmail"].Value = toEmail;
                cmd.Parameters["@LogDate"].Value = System.DateTime.Now;
                cmd.Parameters["@Subject"].Value = subject;
                cmd.Parameters["@Contents"].Value = emailContent;
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

        #endregion

        public String GetSQLForLead(String DealerCode, String FromDate, String ToDate, String AcctNo, String NRIC)
        {
            String SQL = String.Empty;

            SQL = "UNION ALL" + "\r\n\r\n";

            SQL += "SELECT LA.LeadID as AcctNo, LA.AECode AS Dealer, LA.AssignDate AS AssignDate, LD.CreateDate AS AcctCreateDate, LD.LeadNRIC as NRIC," + "\r\n";
            SQL += "LD.LeadName AS ClientName, LD.AECode as TeamCode, '' AS AssignLTD, '' AS CurrentLTD, LCC.LastCallDate, LA.CutOffDate AS CutOffDate," + "\r\n";
            SQL += "'' AS MonthDiff, '' AS TotalComm,'' AS NoOfTrades" + "\r\n";
            SQL += "FROM LeadAssign LA" + "\r\n";
            SQL += "LEFT JOIN LeadDetail LD WITH (NOLOCK) ON LA.LeadID = LD.LeadID" + "\r\n";
            SQL += "INNER JOIN" + "\r\n";
            SQL += "(" + "\r\n";
            SQL += "	SELECT la.AECode, la.LeadID, la.AssignDate, la.CutOffDate, MAX(lc.ContactDate) as LastCallDate" + "\r\n";
            SQL += "	FROM LeadAssign la" + "\r\n";
            SQL += "	LEFT JOIN" + "\r\n";
            SQL += "	(" + "\r\n";
            SQL += "		SELECT LeadID, ContactDate         FROM LeadContact" + "\r\n";
            SQL += "	) lc ON lc.LeadID = la.LeadID AND lc.ContactDate BETWEEN la.AssignDate AND la.CutOffDate" + "\r\n";

            if (DealerCode != String.Empty)
            {
                SQL += "	WHERE AECode = '" + DealerCode + "'" + "\r\n";
            }

            SQL += "	GROUP BY la.AECode, la.LeadID, la.AssignDate, la.CutOffDate" + "\r\n";
            SQL += ") LCC ON LA.LeadID = LCC.LeadID AND LA.AssignDate = LCC.AssignDate" + "\r\n";
            SQL += "WHERE LA.AssignDate BETWEEN  '" + FromDate + "' AND '" + ToDate + " 23:59:59.999'" + "\r\n";

            if (DealerCode != String.Empty)
            {
                SQL += "AND LA.AECode = '" + DealerCode + "'" + "\r\n";
            }

            if (AcctNo != String.Empty)
            {
                SQL += "AND LA.LeadID = '" + AcctNo + "'" + "\r\n";
            }

            if (NRIC != String.Empty)
            {
                SQL += "AND LD.LeadNRIC = '" + NRIC + "'";
            }

            return SQL;
        }

        public String GetSQLForLeadByAdministrator(String DealerCode, String FromDate, String ToDate, String AEGroup, String AcctNo, String NRIC, String UserID)
        {
            String SQL = String.Empty;

            SQL = "UNION ALL" + "\r\n\r\n";

            SQL += "SELECT LA.LeadID as AcctNo, LA.AECode AS Dealer, LA.AssignDate AS AssignDate, LD.CreateDate AS AcctCreateDate, LD.LeadNRIC as NRIC," + "\r\n";
            SQL += "LD.LeadName AS ClientName, LD.AECode as TeamCode, '' AS AssignLTD, '' AS CurrentLTD, LCC.LastCallDate, LA.CutOffDate AS CutOffDate," + "\r\n";
            SQL += "'' AS MonthDiff, '' AS TotalComm,'' AS NoOfTrades" + "\r\n";
            SQL += "FROM LeadAssign LA" + "\r\n";
            SQL += "LEFT JOIN LeadDetail LD WITH (NOLOCK) ON LA.LeadID = LD.LeadID" + "\r\n";
            SQL += "INNER JOIN" + "\r\n";
            SQL += "(" + "\r\n";
            SQL += "	SELECT la.AECode, la.LeadID, la.AssignDate, la.CutOffDate, MAX(lc.ContactDate) as LastCallDate" + "\r\n";
            SQL += "	FROM LeadAssign la" + "\r\n";
            SQL += "	LEFT JOIN" + "\r\n";
            SQL += "	(" + "\r\n";
            SQL += "		SELECT LeadID, ContactDate         FROM LeadContact" + "\r\n";
            SQL += "	) lc ON lc.LeadID = la.LeadID AND lc.ContactDate BETWEEN la.AssignDate AND la.CutOffDate" + "\r\n";

            if (DealerCode != String.Empty)
            {
                SQL += "	WHERE AECode = '" + DealerCode + "'" + "\r\n";
            }

            SQL += "	GROUP BY la.AECode, la.LeadID, la.AssignDate, la.CutOffDate" + "\r\n";
            SQL += ") LCC ON LA.LeadID = LCC.LeadID AND LA.AssignDate = LCC.AssignDate" + "\r\n";
            SQL += "WHERE LA.AssignDate BETWEEN  '" + FromDate + "' AND '" + ToDate + " 23:59:59.999'" + "\r\n";

            if (DealerCode != String.Empty)
            {
                SQL += "AND LA.AECode = '" + DealerCode + "'" + "\r\n";
            }

            //SQL += "AND LA.AECode IN (SELECT AECode FROM DealerDetail WHERE AEGroup LIKE '" + AEGroup + "%')" + "\r\n";
            if (AEGroup != String.Empty)
            {
                //SQL += "AND LA.AECode IN (SELECT AECode FROM DealerDetail WHERE " + AEGroup + ")" + "\r\n";
                SQL += "AND LA.AECode IN (SELECT AECode FROM DealerDetail WHERE AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "'))" + "\r\n";
            }

            if (AcctNo != String.Empty)
            {
                SQL += "AND LA.LeadID = '" + AcctNo + "'" + "\r\n";
            }

            if (NRIC != String.Empty)
            {
                SQL += "AND LD.LeadNRIC = '" + NRIC + "'";
            }

            return SQL;
        }
    }    
}