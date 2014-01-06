﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebServiceApp.DataAccess
{
    public class CommonServiceDA
    {
        private GenericDA genericDA;
        private string sql;

        public CommonServiceDA(GenericDA da)
        {
            genericDA = da;
        }

        public DataSet RetrieveTeamCodeAndName()
        {
            sql = "SELECT RTRIM(LTRIM(DD.AEGroup)) AS TeamCode, DD.AEGroup + ' - ' + AEL.AE_Name AS TeamName " +
                        "FROM DealerDetail DD WITH (NOLOCK) " +
                        "LEFT JOIN AEList AEL ON DD.AEGroup = AEL.GroupName " +
                        "GROUP BY DD.AEGroup, AEL.AE_Name " +
                        "ORDER BY DD.AEGroup ";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveTeamAndNameByDealerCode(string dealerCode)
        {
            sql = "SELECT RTRIM(LTRIM(DD.AEGroup)) AS TeamCode, DD.AEGroup + ' - ' + DD.AEName AS TeamName " +
                  "FROM DealerDetail DD WHERE DD.AECode='" + dealerCode + "' " +
                  "UNION " +
                  "SELECT RTRIM(LTRIM(CA.AEGroup)) AS TeamCode, CA.AEGroup + ' - ' + DD.AEName AS TeamName from CommAECode CA " +
                  "LEFT JOIN DealerDetail DD WITH (NOLOCK) " +
                  "ON CA.AECode = DD.AECode where CA.AECode='" + dealerCode + "' ";

            return genericDA.ExecuteQuery(sql);
        }


        public DataSet RetrieveTeamCodeAndNameByTSeries(String UserID)
        {
            /// <Modified by Thet Maung Chaw not to include FAR Records.>
            //sql = "Select AE_CD_Sec AS TeamCode, AE_CD_Sec + ' - ' + AE_Name AS TeamName from AEList AE " +
            //        "Inner Join  (select AECode,AEGroup from DealerDetail where AECode like ('t%') or AECode like ('sfr%')) AS DD  " +
            //        "ON AE.GroupName=DD.AEGroup  " +
            //        "GROUP BY AE_CD_Sec, AE_Name " +
            //        "order by 1  ";

            sql = "SELECT AE_CD_Sec                   AS TeamCode," + "\r\n";
            sql += "       AE_CD_Sec + ' - ' + AE_Name AS TeamName" + "\r\n";
            sql += "FROM   AEList_20130910 AE" + "\r\n";
            sql += "WHERE  AE.RefMonthYr = (SELECT MAX(RefMonthYr)" + "\r\n";
            sql += "                        FROM   AEList_20130910)" + "\r\n";
            sql += "       AND AE_CD_Sec NOT IN (SELECT DISTINCT AECode" + "\r\n";
            sql += "                             FROM   DealerDetail" + "\r\n";
            sql += "                             WHERE  UserType LIKE 'FAR%')" + "\r\n";
            sql += "       AND AE_CD_Sec IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";
            sql += "GROUP  BY AE_CD_Sec," + "\r\n";
            sql += "          AE_Name" + "\r\n";
            sql += "ORDER  BY 1";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveTeamCodeAndNameByNonTSeries(String UserID)
        {
            /// <Modified by Thet Maung Chaw not to include FAR Records.>
            //sql = "SELECT AE_CD_Sec AS TeamCode, AE_CD_Sec + ' - ' + AE_Name AS TeamName FROM AEList AE " +
            //        "Inner Join  (SELECT AECode,AEGroup FROM DealerDetail WHERE UserType NOT LIKE 'FAR%' --AECode not like ('t%') and AECode not like ('sfr%') \r\n" +
            //        ") AS DD " + 
            //        "ON AE.GroupName=DD.AEGroup " + 
            //        "GROUP BY AE_CD_Sec, AE_Name " + 
            //        "ORDER BY 1  ";

            sql = "SELECT" + "\r\n";
            sql += "    AE_CD_Sec AS TeamCode," + "\r\n";
            sql += "    AE_CD_Sec + ' - ' + AE_Name AS TeamName" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    AEList" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    AE_CD_Sec NOT IN" + "\r\n";
            sql += "    (" + "\r\n";
            sql += "        SELECT DISTINCT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%'" + "\r\n";
            sql += "    )" + "\r\n";
            sql += "    AND AEList.RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList)" + "\r\n";
            sql += "    AND AE_CD_Sec IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    AE_CD_Sec," + "\r\n";
            sql += "    AE_Name" + "\r\n";
            sql += "ORDER BY" + "\r\n";
            sql += "    1";

            return genericDA.ExecuteQuery(sql);
        }

        public DataTable RetrieveTeamCodeAndNameByDataTable()
        {
            //sql = "SELECT RTRIM(LTRIM(DD.AEGroup)) AS TeamCode, DD.AEGroup + ' - ' + AEL.AE_Name AS TeamName " +
            //            "FROM DealerDetail DD WITH (NOLOCK) " +
            //            "LEFT JOIN AEList AEL ON DD.AEGroup = AEL.GroupName " +
            //            "GROUP BY DD.AEGroup, AEL.AE_Name " +
            //            "ORDER BY DD.AEGroup ";

            sql = "SELECT" + "\r\n";
            sql += "    AE_CD_Sec AS TeamCode," + "\r\n";
            sql += "    AE_CD_Sec + ' - ' + AE_Name AS TeamName" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    AEList" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    AE_CD_Sec NOT IN" + "\r\n";
            sql += "    (" + "\r\n";
            sql += "        SELECT DISTINCT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%'" + "\r\n";
            sql += "    )" + "\r\n";
            sql += "    AND AEList.RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList)" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    AE_CD_Sec," + "\r\n";
            sql += "    AE_Name" + "\r\n";
            sql += "ORDER BY 1";

            return genericDA.ExecuteQueryForDataTable(sql, "Teams");
        }

        /// <Added by OC>
        public DataTable RetrieveTeamCodeAndNameByDataTableByUserOrSupervisor(String Param, String UserID)
        {
            //sql = "SELECT RTRIM(LTRIM(DD.AEGroup)) AS TeamCode, DD.AEGroup + ' - ' + AEL.AE_Name AS TeamName " +
            //            "FROM DealerDetail DD WITH (NOLOCK) " +
            //            "LEFT JOIN AEList AEL ON DD.AEGroup = AEL.GroupName " +
            //            "WHERE DD." + Param +
            //            "GROUP BY DD.AEGroup, AEL.AE_Name " +
            //            "ORDER BY DD.AEGroup ";

            sql = "SELECT" + "\r\n";
            sql += "    AE_CD_Sec AS TeamCode," + "\r\n";
            sql += "    AE_CD_Sec + ' - ' + AE_Name AS TeamName" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    AEList" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    (AE_CD_Sec NOT IN" + "\r\n";
            sql += "    (" + "\r\n";
            sql += "        SELECT DISTINCT AECode FROM DealerDetail WHERE " + Param + "\r\n";
            sql += "    )" + "\r\n";
            sql += "    AND AE_CD_Sec IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "'))" + "\r\n";
            sql += "    AND AEList.RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList)" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    AE_CD_Sec," + "\r\n";
            sql += "    AE_Name" + "\r\n";
            sql += "ORDER BY 1";

            return genericDA.ExecuteQueryForDataTable(sql, "Teams");
        }

        public DataTable RetrieveAllTeamCodeAndName(String UserID)
        {
            /// <Modified by Thet Maung Chaw not to include FAR.>
            //sql = "SELECT AE_CD_Sec AS TeamCode, AE_CD_Sec + ' - ' + AE_Name AS TeamName " +
            //            " FROM  dbo.AEList " +
            //            " GROUP BY AE_CD_Sec, AE_Name " +
            //            " order by 1 ";

            sql = "SELECT" + "\r\n";
            sql += "    AE_CD_Sec AS TeamCode," + "\r\n";
            sql += "    AE_CD_Sec + ' - ' + AE_Name AS TeamName" + "\r\n";
            sql += "FROM" + "\r\n";
            sql += "    AEList" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    (AE_CD_Sec NOT IN" + "\r\n";
            sql += "    (" + "\r\n";
            sql += "        SELECT DISTINCT AECode FROM DealerDetail WHERE UserType LIKE 'FAR%'" + "\r\n";
            sql += "    )" + "\r\n";

            if(UserID!="")
                sql += "    AND AE_CD_Sec IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')" + "\r\n";

            sql += " )   AND AEList.RefMonthYr = (SELECT MAX(RefMonthYr) FROM AEList)" + "\r\n";
            sql += "GROUP BY" + "\r\n";
            sql += "    AE_CD_Sec," + "\r\n";
            sql += "    AE_Name" + "\r\n";
            sql += "ORDER BY 1";

            return genericDA.ExecuteQueryForDataTable(sql, "Teams");
        }

        public DataSet RetrieveDealerCodeAndNameByTeam(string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM DealerDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE CutOffDate > GETDATE() " +                      //"WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'  AND DD.Enable = 1" +
                        "ORDER BY DD.AECode";

            return genericDA.ExecuteQuery(sql);
        }

        public DataSet RetrieveDealerCodeAndNameByTeamNLoginID(string teamCode, string loginid)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM DealerDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM ClientAssign WITH (NOLOCK) " +
                        "WHERE CutOffDate > GETDATE() " +                      //"WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup = '" + teamCode + "'  AND DD.Enable = 1 AND DD.UserID = '" + loginid + "'" + 
                        "ORDER BY DD.AECode";

            return genericDA.ExecuteQuery(sql);
        }


        public DataTable RetrieveAllDealer(String UserID)
        {
            if (UserID != String.Empty)
            {
                sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
                        " FROM DealerDetail " +
                        " WHERE UserType NOT LIKE 'FAR%' " +
                        "AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') " +
                        " GROUP BY UserID, AECode, AEName, AEGroup " +
                        " ORDER BY AEName ";
            }
            else
            {
                sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
                        " FROM DealerDetail " +
                        " WHERE UserType NOT LIKE 'FAR%' " +
                        " GROUP BY UserID, AECode, AEName, AEGroup " +
                        " ORDER BY AEName ";
            }
            

            return genericDA.ExecuteQueryForDataTable(sql, "Dealer");
        }

        /// <Added by OC>
        public DataTable RetrieveAllDealerByUserOrSupervisor(String Param, String UserID)
        {
            sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
                    " FROM DealerDetail " +
                    " WHERE DealerDetail." + Param +
                    " AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "') " +
                    " GROUP BY UserID, AECode, AEName, AEGroup " +
                    " ORDER BY AEName ";

            return genericDA.ExecuteQueryForDataTable(sql, "Dealer");
        }

        public DataTable RetrieveCrossEnableDealer()
        {
            sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
                    " FROM DealerDetail " +
                    " WHERE Enable = 1 AND CrossGroup = 'Y' AND UserType NOT LIKE 'FAR%' " +
                    " GROUP BY UserID, AECode, AEName, AEGroup " +
                    " ORDER BY AEGroup, AECode ";
            
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable RetrievePreferenceCodeAndName()
        {
            sql = "SELECT OptionNo, OptionContent, (OptionNo + ' - ' + OptionContent) AS OptionDisplay FROM PreferenceList ORDER BY OptionNo";

            return genericDA.ExecuteQueryForDataTable(sql, "dtPreference");
        }

        /*** for SPM III by Yin Mon Win
         *   16 September 2011
         * 
         **/

        public DataTable  getAccessRight(string PageFunction,string userId)
        {
            sql = "SELECT UserID,TYPE,Functions,CreateRight,ViewRight,ModifyRight,DeleteRight,RoleID,UserRight  " +
                  " FROM AccessRight WITH (NOLOCK) WHERE UserID='" + userId + "' AND Functions='" + PageFunction + "' AND UserRight='Y' ";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataTable dtRetrieveDealerCodeAndNameByUserID(string userId)
        {
            // sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
            sql = "SELECT UserID,  RTRIM(LTRIM(AEGroup)) AS TeamCode, AEName, AECode, AEGroup + ' - ' + AEName AS TeamName " +
                  "FROM DealerDetail WITH (NOLOCK) WHERE UserID='" + userId + "' AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + userId + "') ORDER BY AEName ";

            return genericDA.ExecuteQueryForDataTable(sql);
        }

        public DataSet RetrieveDealerCodeAndNameByUserID(string userId)
        {
            // sql = "SELECT UserID, AECode, AEName, AEGroup, AEName + ' - ' + AECode AS DisplayName " +
            sql = "SELECT UserID,  RTRIM(LTRIM(AEGroup)) AS TeamCode, AEName, AECode, AEGroup + ' - ' + AEName AS TeamName, Supervisor " +
                  "FROM DealerDetail WITH (NOLOCK) WHERE UserID='" + userId + "' AND AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + userId + "') ORDER BY AEName ";

            return genericDA.ExecuteQuery(sql);
        }

        //Add for SPM III multiple selection
        public DataSet RetrieveMultipleDealerCodeAndNameByTeam(string teamCode)
        {
            sql = "SELECT DD.AECode, DD.AEName, DD.AEGroup, ISNULL(CA.CurrentAssign, 0) AS CurrentAssign " +
                        "FROM DealerDetail DD " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT AECode, COUNT(AECode) AS CurrentAssign " +
                        "FROM LeadAssign WITH (NOLOCK) " +
                        "WHERE CutOffDate > GETDATE() " +                      //"WHERE AssignDate < CutOffDate " +
                        "GROUP BY AECode " +
                        ") CA ON DD.AECode = CA.AECode " +
                        "WHERE DD.AEGroup IN (" + teamCode + ")  AND DD.Enable = 1" +
                        "ORDER BY DD.AECode";

            return genericDA.ExecuteQuery(sql);
        }

        /// <summary>
        /// SPM Phase III
        /// Retrieving the Projects Information by User ID
        /// Filtering by ProjectName will be added if "filterByProjName" is not null.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchByProjName"></param>
        /// <returns></returns>
        public DataSet RetrieveProjectByUserId(String userId, String filterByProjName)
        {
            sql = "SELECT DISTINCT " +
                        "p.ProjectID, " +
                        "p.ProjectName, " +
                        "p.ProjectType, " +
                        "p.AssignDate, " +
                        "p.CutOffDate " +
                    "FROM ProjectDetail p " +
                        "INNER JOIN dbo.ClientAssign c ON " +
                            "p.ProjectID = c.ProjectID " +
                        "INNER JOIN dbo.DealerDetail d ON " +
                            "d.AECode = c.AECode " +
                    "WHERE d.UserID = '" + userId + "'";

            //sql = "SELECT DISTINCT " +
            //            "p.ProjectID AS ProjectID, " +
            //            "p.ProjectName AS ProjectName, " +
            //            "p.ProjectType AS ProjectType, " +
            //            "p.AssignDate AS AssignDate, " +
            //            "p.CutOffDate AS CutOffDate, " +
            //            "TotalComm.TotalCommission AS TotalCommission " +
            //        "FROM ProjectDetail p " +
            //            "INNER JOIN dbo.ClientAssign c ON " +
            //                "p.ProjectID = c.ProjectID " +
            //            "INNER JOIN dbo.DealerDetail d ON " +
            //                "d.AECode = c.AECode " +
            //            "INNER JOIN " +
            //                "( " +
            //                    "SELECT " +
            //                        "ca.ProjectId, SUM(t.TotalComm) AS TotalCommission " +
            //                    "FROM " +
            //                        "TmpClientAssign t " +
            //                            "INNER JOIN ClientAssign ca ON " +
            //                                "t.AECode = ca.AECode AND " +
            //                                "t.AcctNo = ca.AcctNo " +
            //                            "INNER JOIN ProjectDetail pd ON " +
            //                                "ca.ProjectID = pd.ProjectID " +
            //                    "GROUP BY  " +
            //                        "ca.ProjectID " +
            //                    ") AS TotalComm ON " +
            //                        "TotalComm.ProjectID = p.ProjectID	 " +
            //        "WHERE d.UserID = '" + userId + "'";

            if (!String.IsNullOrEmpty(filterByProjName))
            {
                sql = sql + " AND ProjectName LIKE '%" + filterByProjName + "%'";
            }

            return genericDA.ExecuteQuery(sql);
        }

        /// <summary>
        /// Created by:		Thet Su Mon 
        /// </summary>
        /// 
        public DataTable RetrieveAllProjectByProjectName(String ProjName)
        {
            //sql = "SELECT ProjectID,ProjectName FROM [SPM].[dbo].[ProjectDetail]where ProjectName like '%" + ProjName + "%'";
            sql = "SELECT ProjectID,ProjectName FROM [SPM].[dbo].[ProjectDetail]where ProjectName like '%" + ProjName + "%' ORDER BY ProjectName";
            return genericDA.ExecuteQueryForDataTable(sql);
        }

        /// <Added by OC>
        public DataTable RetrieveAllProjectByProjectNameByUserOrSupervisor(String Param, String UserID)
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
            sql += "	ProjectDetail" + "\r\n";
            sql += "    INNER JOIN LeadAssign ON ProjectDetail.ProjectID = LeadAssign.ProjectID" + "\r\n";
            sql += "    INNER JOIN DealerDetail ON DealerDetail.AECode = LeadAssign.AECode" + "\r\n";
            sql += "WHERE" + "\r\n";
            sql += "    DealerDetail." + Param + "\r\n";
            sql += "    AND DealerDetail.AEGroup IN (SELECT AEGroup FROM SuperAdmin WHERE UserID='" + UserID + "')";

            return genericDA.ExecuteQueryForDataTable(sql);
        }
       

        /// <summary>
        /// Retrieving TotalCommission for each Individual and Team
        /// Filter by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
       internal DataSet RetrieveTotalCommissionForUserId(string userId)
       {
           sql = "SELECT individual.ProjectId AS ProjectId, individual.ProjectName AS ProjectName, individual.TotalComm AS TotalIndividualComm, Team.TotalComm AS TotalTeamComm " +
                        "FROM " +
                            "( " +
		                        "SELECT  " +
			                        "temp.ProjectID, temp.ProjectName, SUM(t.TotalComm) AS TotalComm " +
		                        "FROM " +
			                        "TmpClientAssign t " +
				                        "INNER JOIN " +
					                        "( " +
						                        "Select " +
							                        "ca.AcctNo, ca.AECode, ca.ProjectID, p.ProjectName " +
						                        "From " +
							                        "ClientAssign ca " +
								                        "INNER JOIN DealerDetail d ON " +
									                        "ca.AECode = d.AECode " +
                                                            "AND d.AECode IN (SELECT AECode FROM DealerDetail WHERE UserID = 'ittest') " +
								                        "INNER JOIN ProjectDetail p ON " +
                                                            "ca.ProjectID = p.ProjectID AND ca.CutOffDate >= GETDATE()" +
						                        "Where p.ProjectID IN  " +
											                        "( " +
												                        "SELECT  " +
													                        "ca.ProjectId  " +
												                        "From " + 
													                        "ClientAssign CA " + 
												                        "WHERE AECode IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "') " +
											                        ") " +
					                        ") AS temp ON " +
						                        "temp.AECode = t.AECode " +
						                        "AND temp.AcctNo = t.AcctNo " +
		                        "GROUP BY temp.ProjectID, temp.ProjectName " +
	                        ") AS individual " +
		                        "INNER JOIN " +
			                        "( " +
				                        "SELECT " +
					                        "temp.ProjectID, temp.ProjectName, SUM(t.TotalComm) AS TotalComm " +
				                        "FROM " +
					                        "TmpClientAssign t " +
						                        "INNER JOIN " +
							                        "( " +
								                        "Select " +
									                        "ca.AcctNo, ca.AECode, ca.ProjectID, p.ProjectName " +
								                        "From " +
									                        "ClientAssign ca " +
										                        "INNER JOIN DealerDetail d ON " +
											                        "ca.AECode = d.AECode " +
											                        "AND d.AEGroup IN (SELECT AEGroup FROM DealerDetail WHERE UserID = '"+ userId +"') " +
										                        "INNER JOIN ProjectDetail p ON " +
                                                                    "ca.ProjectID = p.ProjectID AND ca.CutOffDate >= GETDATE()" +
								                        "Where p.ProjectID IN ( " +
													                        "SELECT " +
														                        "ca.ProjectId " +
													                        "From " +
														                        "ClientAssign CA " +
                                                                            "WHERE AECode IN (SELECT AECode FROM DealerDetail WHERE UserID='" + userId + "') " +
													                        ") " +
							                        ") AS temp ON " +
								                        "temp.AECode = t.AECode " +
								                        "AND temp.AcctNo = t.AcctNo " +
				                        "GROUP BY temp.ProjectID, temp.ProjectName " +
			                        ") AS Team ON " +
				                        "individual.ProjectID = Team.ProjectID ";
           return genericDA.ExecuteQuery(sql);
       }

       internal DataSet RetrieveProjectTotalCommByProjectId(string projectId)
       {
           sql = "SELECT " +
	                "SUM(t.TotalComm) AS TotalCommission " +
                "FROM " +
	                "TmpClientAssign t " +
		                "INNER JOIN ClientAssign ca ON " +
			                "t.AECode = ca.AECode AND " +
			                "t.AcctNo = ca.AcctNo " +
		                "INNER JOIN ProjectDetail pd ON " +
                            "ca.ProjectID = pd.ProjectID AND " +
			                "pd.ProjectID = '" + projectId + "'";
           return genericDA.ExecuteQuery(sql);
       }
    }

}