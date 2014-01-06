using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SPMWebApp.Services;

namespace SPMWebApp.Utilities
{
    public enum AccessRightType
    {
        Modify,
        Create,
        View,
        Delete
    }

    public class AccessRight
    {
        public AccessRightType accessRightType {get;set;}
        public bool hasAccessRight {get;set;}

        public AccessRight() { }
    }

    public class AccessRightUtilities
    {           
        public AccessRightUtilities() 
        {             
        }

        public static DataTable GetUserMenuTable(String dbConnectionStr, String userId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            DataSet ds = accessControlService.RetrieveUserMenuOptions(userId);            
            return ds.Tables[0];
        }

        public static DataSet RetrieveUserAccessRights(String dbConnectionStr, String userId)
        {
            AccessControlService accessControlService = new AccessControlService(dbConnectionStr);
            return accessControlService.RetrieveUserAccessRights(userId);
        }

        public static Boolean ValidateUserAccessRight(DataTable userAccessRightTable, String FunctionCode,out List<AccessRight> accessRightList)
        {
            accessRightList = new List<AccessRight>();
            try
            {
                var results = from accessRightRow in userAccessRightTable.AsEnumerable()
                              where accessRightRow.Field<String>("Function_Code") == FunctionCode
                              select accessRightRow;
                EnumerableRowCollection<DataRow> drResult = results as EnumerableRowCollection<DataRow>;
                if (drResult.Count() > 0)
                {
                    foreach (DataRow dr in drResult)
                    {
                        accessRightList.Add(new AccessRight()
                        {
                            accessRightType = AccessRightType.Modify,
                            hasAccessRight = dr["ModifyRight"].ToString().Equals("TRUE")
                        });
                        accessRightList.Add(new AccessRight()
                        {
                            accessRightType = AccessRightType.View,
                            hasAccessRight = dr["ViewRight"].ToString().Equals("TRUE")
                        });
                        accessRightList.Add(new AccessRight()
                        {
                            accessRightType = AccessRightType.Delete,
                            hasAccessRight = dr["DeleteRight"].ToString().Equals("TRUE")
                        });
                        accessRightList.Add(new AccessRight()
                        {
                            accessRightType = AccessRightType.Create,
                            hasAccessRight = dr["CreateRight"].ToString().Equals("TRUE")
                        });
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {                
                return false;
            }
        }

        
    }
}
