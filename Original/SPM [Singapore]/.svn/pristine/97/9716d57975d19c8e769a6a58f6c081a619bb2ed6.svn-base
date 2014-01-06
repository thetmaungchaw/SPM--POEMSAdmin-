using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using SPMWebServiceApp.DataAccess;
using SPMWebServiceApp.Utilities;

namespace SPMWebServiceApp.Services
{
    public class SuperAdminService
    {
        private GenericDA genericDA;
        private SuperAdminDA SuperAdminDA;

        public SuperAdminService()
        {
            genericDA = new GenericDA();
            SuperAdminDA = new SuperAdminDA(genericDA);            
        }

        public SuperAdminService(String dbConnStr) : this()
        {
            genericDA.SetConnectionString(dbConnStr);
        }

        public DataSet SuperAdmin_GetByFilters(String UserID, String AEGroup, int Start, int PageLength)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(SuperAdminDA.SuperAdmin_GetByFilters(UserID, AEGroup, Start, PageLength));

                ds.Tables.Add(dtReturn);

                genericDA.CloseConnection();
            }
            catch (Exception ex)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";
                
                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving SuperAdmin! Please try again. Exception Message: " + ex.Message;

                ds.Tables.Add(dtReturn);

                genericDA.CloseConnection();
            }

            return ds;
        }

        public DataSet SuperAdmin_Getddl(String UserID, String AEGroup, String rdbtnUserIDOption, String rdbtnAEGroupOption)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;

            try
            {
                dtReturn = CommonUtilities.CreateReturnTable("1", "");

                genericDA.OpenConnection();

                ds.Tables.Add(SuperAdminDA.SuperAdmin_GetAEGroup(AEGroup, rdbtnAEGroupOption));

                ds.Tables.Add(SuperAdminDA.SuperAdmin_GetAECode());

                ds.Tables.Add(SuperAdminDA.SuperAdmin_GetUserID(UserID, rdbtnUserIDOption));

                ds.Tables.Add(dtReturn);

                genericDA.CloseConnection();
            }
            catch (Exception ex)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";

                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving SuperAdmin! Please try again. Exception Message: " + ex.Message;

                ds.Tables.Add(dtReturn);

                genericDA.CloseConnection();
            }

            return ds;
        }

        public DataSet SuperAdmin_Delete(String UserID, String AEGroup)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;

            String returnstr;

            try
            {
                returnstr = SuperAdminDA.SuperAdmin_Delete(UserID, AEGroup);

                if (returnstr != "1")
                {
                    dtReturn = CommonUtilities.CreateReturnTable("-1", returnstr);
                    ds.Tables.Add(dtReturn);
                }
                else
                {
                    dtReturn = CommonUtilities.CreateReturnTable("1", "Delete Successful !");
                    ds.Tables.Add(dtReturn);
                }
            }
            catch (Exception ex)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";

                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving SuperAdmin! Please try again. Exception Message: " + ex.Message;

                ds.Tables.Add(dtReturn);
            }

            return ds;
        }

        public DataSet SuperAdmin_Insert(String UserID, String AEGroup)
        {
            DataSet ds = new DataSet();

            DataTable dtReturn = null;

            String returnstr;

            try
            {
                returnstr = SuperAdminDA.SuperAdmin_Insert(UserID, AEGroup);

                if (returnstr != "1")
                {
                    dtReturn = CommonUtilities.CreateReturnTable("-1", returnstr);
                    ds.Tables.Add(dtReturn);
                }
                else
                {
                    dtReturn = CommonUtilities.CreateReturnTable("1", "Save Successful !");
                    ds.Tables.Add(dtReturn);
                }
            }
            catch (Exception ex)
            {
                dtReturn.Rows[0]["ReturnCode"] = "-1";

                dtReturn.Rows[0]["ReturnMessage"] = "Error in retrieving SuperAdmin! Please try again. Exception Message: " + ex.Message;

                ds.Tables.Add(dtReturn);
            }

            return ds;
        }
    }
}