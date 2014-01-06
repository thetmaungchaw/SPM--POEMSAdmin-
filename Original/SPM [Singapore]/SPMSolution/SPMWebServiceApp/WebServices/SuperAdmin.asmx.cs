﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

using System.Data;
using SPMWebServiceApp.Services;

namespace SPMWebServiceApp.WebServices
{
    /// <summary>
    /// Summary description for SuperAdmin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SuperAdmin : WebService
    {
        [WebMethod]
        public DataSet SuperAdmin_GetByFilters(String UserID, String AEGroup, int Start, int PageLength, String dbConnectionStr)
        {
            SuperAdminService superAdminService = new SuperAdminService(dbConnectionStr);
            return superAdminService.SuperAdmin_GetByFilters(UserID, AEGroup, Start, PageLength);
        }

        [WebMethod]
        public DataSet SuperAdmin_Getddl(String UserID, String AEGroup, String rdbtnUserIDOption, String rdbtnAEGroupOption, String dbConnectionStr)
        {
            SuperAdminService superAdminService = new SuperAdminService(dbConnectionStr);
            return superAdminService.SuperAdmin_Getddl(UserID, AEGroup, rdbtnUserIDOption, rdbtnAEGroupOption);
        }

        [WebMethod]
        public DataSet SuperAdmin_Delete(String UserID, String AEGroup, String dbConnectionStr)
        {
            SuperAdminService superAdminService = new SuperAdminService(dbConnectionStr);
            return superAdminService.SuperAdmin_Delete(UserID, AEGroup);
        }

        [WebMethod]
        public DataSet SuperAdmin_Insert(String UserID, String AEGroup, String dbConnectionStr)
        {
            SuperAdminService superAdminService = new SuperAdminService(dbConnectionStr);
            return superAdminService.SuperAdmin_Insert(UserID, AEGroup);
        }
    }
}