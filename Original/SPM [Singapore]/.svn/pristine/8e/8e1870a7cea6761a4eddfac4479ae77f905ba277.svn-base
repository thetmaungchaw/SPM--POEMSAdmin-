using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using SPMWebApp.Utilities;

namespace SPMWebApp.Services
{
    public class SuperAdminService
    {
        private string dbConnectionStr;
        private SPMWebApp.SuperAdmin.SuperAdmin SuperAdmin;
        private DataTable dtReturn;

        public SuperAdminService()
        {
            SuperAdmin = new SPMWebApp.SuperAdmin.SuperAdmin();
            dbConnectionStr = "";
        }

        public SuperAdminService(string cnStr) : this()
        {
            this.dbConnectionStr = cnStr;
        }

        public DataSet SuperAdmin_GetByFilters(String UserID, String AEGroup, int Start, int PageLength)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = SuperAdmin.SuperAdmin_GetByFilters(UserID, AEGroup, Start, PageLength, dbConnectionStr);
            }
            catch (Exception ex)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!" + ex.Message);
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet SuperAdmin_Getddl(String UserID, String AEGroup, String rdbtnUserIDOption, String rdbtnAEGroupOption)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = SuperAdmin.SuperAdmin_Getddl(UserID, AEGroup, rdbtnUserIDOption, rdbtnAEGroupOption, dbConnectionStr);
            }
            catch (Exception ex)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!" + ex.Message);
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet SuperAdmin_Delete(String UserID, String AEGroup)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = SuperAdmin.SuperAdmin_Delete(UserID, AEGroup, dbConnectionStr);
            }
            catch (Exception ex)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!" + ex.Message);
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet SuperAdmin_Insert(String UserID, String AEGroup)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = SuperAdmin.SuperAdmin_Insert(UserID, AEGroup, dbConnectionStr);
            }
            catch (Exception ex)
            {
                dtReturn = CommonUtilities.CreateReturnTable("-3", "Problem in WebService Connection!" + ex.Message);
                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
    }
}