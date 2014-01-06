﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Collections.Generic;

using SPMWebApp.BasePage;
using SPMWebApp.Services;
using SPMWebApp.Utilities;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class ClientAnalysis : BasePage.BasePage
    {
        private ClientContactService clientContactService;

        /// <summary>
        /// Fetch User Access Right
        /// Call to ValidateUserAccessRight method with FunctionCode
        /// </summary>        
        private void LoadUserAccessRight()
        {
            List<AccessRight> accessRightList;
            if (AccessRightUtilities.ValidateUserAccessRight((DataTable)Session["UserAccessRights"], "ClientAnalysis", out accessRightList))
            {
                foreach (AccessRight accessRight in accessRightList)
                {
                    switch (accessRight.accessRightType)
                    {
                        case AccessRightType.Create:
                            {
                                ViewState["CreateAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.Modify:
                            {
                                ViewState["ModifyAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.View:
                            {
                                ViewState["ViewAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        case AccessRightType.Delete:
                            {
                                ViewState["DeleteAccessRight"] = accessRight.hasAccessRight;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }

        protected void checkAccessRight()
        {
            try
            {
                btnExcel.Enabled = (bool)ViewState["ViewAccessRight"];
                btnSearchClient.Enabled = (bool)ViewState["ViewAccessRight"];
            }
            catch (Exception e) { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                /// <Added by OC>
                CommonService CommonService = new CommonService(base.dbConnectionStr);
                ViewState["dsDealerDetail"] = CommonService.RetrieveDealerCodeAndNameByUserID(Session["UserId"].ToString());

                LoadUserAccessRight();
                checkAccessRight();
                PrepareForClientAnalysis();
            }
        }

        private void PrepareForClientAnalysis()
        {
            string todayDt = DateTime.Now.ToString("dd/MM/yyyy");
            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;

            /// <Updated by OC>
            //ds = clientContactService.PrepareForClientAnalysis();

            DataSet dsDealerDetail = ViewState["dsDealerDetail"] as DataSet;

            if (dsDealerDetail != null && dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "Y")
                {
                    ds = clientContactService.PrepareForClientAnalysis(base.userLoginId);
                }
                else if (dsDealerDetail.Tables[0].Rows[0]["Supervisor"].ToString().Trim().ToUpper() == "S")
                {
                    ds = clientContactService.PrepareForClientAnalysisByUserOrSupervisor("AEGroup = '" + dsDealerDetail.Tables[0].Rows[0]["TeamCode"].ToString() + "' ", base.userLoginId);
                }
                else
                {
                    ds = clientContactService.PrepareForClientAnalysisByUserOrSupervisor("AECode = '" + dsDealerDetail.Tables[0].Rows[0]["AECode"].ToString() + "' ", base.userLoginId);
                }
            }
            else
            {
                divMessage.InnerHtml = dsDealerDetail.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                return;
            }


            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                ddlTeam.Items.Add(new ListItem("--- Select Team ---", ""));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlTeam.Items.Add(new ListItem(ds.Tables[0].Rows[i]["TeamName"].ToString(), ds.Tables[0].Rows[i]["TeamCode"].ToString()));
                }

                /*ddlDealer.Items.Add(new ListItem("--- Select Dealer ---", ""));
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlDealer.Items.Add(new ListItem(ds.Tables[1].Rows[i]["DisplayName"].ToString(), ds.Tables[1].Rows[i]["AECode"].ToString()));
                    //if (ds.Tables[1].Rows[i]["UserID"].ToString().Equals(base.userLoginId))
                    //{
                    //    ddlDealer.SelectedValue = ds.Tables[1].Rows[i]["AECode"].ToString();
                    //}
                }*/
            }

            calAcctOpenDate.DateTextFromValue = todayDt;
            calLastTradeDate.DateTextFromValue = todayDt;
        }

        protected void btnSearchClient_Click(object sender, EventArgs e)
        {
            clientContactService = new ClientContactService(base.dbConnectionStr);
            DataSet ds = null;
            string acctOpenDate = "", lastTradeDate = "";

            if (ValidateSearchForm())
            {                
                acctOpenDate = DateTime.ParseExact(calAcctOpenDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                lastTradeDate = DateTime.ParseExact(calLastTradeDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd");

                ds = clientContactService.RetrieveClientAnalysis("", ddlTeam.SelectedValue, acctOpenDate, lastTradeDate);
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    ViewState["dtClientAnalysis"] = ds.Tables[0];
                    rptClientAnalysis.DataSource = ds.Tables[0];
                    rptClientAnalysis.DataBind();

                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLastTradeAfter")).Text = calLastTradeDate.DateTextFromValue;
                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLastTradeBefore")).Text = calLastTradeDate.DateTextFromValue;

                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLTAfterACAfter")).Text = calAcctOpenDate.DateTextFromValue;
                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLTAfterACBefore")).Text = calAcctOpenDate.DateTextFromValue;
                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLTBeforeACAfter")).Text = calAcctOpenDate.DateTextFromValue;
                    ((Label)rptClientAnalysis.Controls[0].FindControl("lblLTBeforeACBefore")).Text = calAcctOpenDate.DateTextFromValue;
                }
                else
                {
                    ViewState["dtClientAnalysis"] = null;
                    rptClientAnalysis.DataSource = null;
                    rptClientAnalysis.DataBind();
                }

                divMessage.InnerHtml = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
            else
            {
                ViewState["dtClientAnalysis"] = null;
                rptClientAnalysis.DataSource = null;
                rptClientAnalysis.DataBind();
            }
        }

        private bool ValidateSearchForm()
        {
            bool result = false;

            if (String.IsNullOrEmpty(calAcctOpenDate.DateTextFromValue.Trim()))
            {
                divMessage.InnerHtml = "Account Open Date cannot be blank!";
            }
            else if (String.IsNullOrEmpty(calLastTradeDate.DateTextFromValue.Trim()))
            {
                divMessage.InnerHtml = "Last Trade Date cannot be blank!";
            }
            else if (!CommonUtilities.CheckDateFormat(calAcctOpenDate.DateTextFromValue.Trim(), "dd/MM/yyyy"))
            {
                divMessage.InnerHtml = "Account Open Date format should be dd/MM/yyyy";
            }
            else if (!CommonUtilities.CheckDateFormat(calLastTradeDate.DateTextFromValue.Trim(), "dd/MM/yyyy"))
            {
                divMessage.InnerHtml = "Last Trade Date format should be dd/MM/yyyy";
            }
            else if (DateTime.ParseExact(calAcctOpenDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).CompareTo(DateTime.Now) > 0)
            {
                divMessage.InnerHtml = "Account Open Date should not greater than Today!";
            }
            else if (DateTime.ParseExact(calLastTradeDate.DateTextFromValue.Trim(), "dd/MM/yyyy", null).CompareTo(DateTime.Now) > 0)
            {
                divMessage.InnerHtml = "Last Trade Date should not greater than Today!";
            }
            else
            {
                result = true;
            }

            return result;
        }

        private int totActive = 0, totInActAOA = 0, totInActAAOB = 0, totInActBAOA = 0, totInActBAOB = 0, tot3None = 0, totalClient = 0;

        protected void rptClientAnalysis_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)   //e.Item.ItemType == ListItemType.AlternatingItem
            {
                int rpttotalClient = 0;
                int rpttotActive = 0, rpttotInActAOA = 0, rpttotInActAAOB = 0, rpttotInActBAOA = 0, rpttotInActBAOB = 0, rpttot3None = 0;

                Label rptlblTotalClient = (Label)e.Item.FindControl("rptlblTotalClient");

                rpttotActive = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotAct").ToString());
                rpttotInActAOA = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInAfterAOAfter").ToString());
                rpttotInActAAOB = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInAfterAOBefore").ToString());
                rpttotInActBAOA = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInBeforeAOAfter").ToString());
                rpttotInActBAOB = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInBeforeAOBefore").ToString());
                rpttot3None = int.Parse(DataBinder.Eval(e.Item.DataItem, "Tot3None").ToString());

                totActive += rpttotActive;
                totInActAOA += rpttotInActAOA;
                totInActAAOB += rpttotInActAAOB;
                totInActBAOA += rpttotInActBAOA;
                totInActBAOB += rpttotInActBAOB;
                tot3None += rpttot3None;

                rpttotalClient = int.Parse(DataBinder.Eval(e.Item.DataItem, "TotAct").ToString()) 
                   + int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInAfterAOAfter").ToString())
                   + int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInAfterAOBefore").ToString())
                   + int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInBeforeAOAfter").ToString())
                   + int.Parse(DataBinder.Eval(e.Item.DataItem, "TotInBeforeAOBefore").ToString())
                   + int.Parse(DataBinder.Eval(e.Item.DataItem, "Tot3None").ToString());

                rptlblTotalClient.Text = rpttotalClient.ToString();
                totalClient += rpttotalClient;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label rptftlblTotalClient = (Label)e.Item.FindControl("rptftlblTotalClient");
                Label rptftlblTotAct = (Label)e.Item.FindControl("rptftlblTotAct");
                Label rptftlblTotInAfterAOAfter = (Label)e.Item.FindControl("rptftlblTotInAfterAOAfter");
                Label rptftlblTotInAfterAOBefore = (Label)e.Item.FindControl("rptftlblTotInAfterAOBefore");
                Label rptftlblTotInBeforeAOAfter = (Label)e.Item.FindControl("rptftlblTotInBeforeAOAfter");
                Label rptftlblTotInBeforeAOBefore = (Label)e.Item.FindControl("rptftlblTotInBeforeAOBefore");
                Label rptftlblTot3None = (Label)e.Item.FindControl("rptftlblTot3None");

                rptftlblTotAct.Text = totActive.ToString();
                rptftlblTotInAfterAOAfter.Text = totInActAOA.ToString();
                rptftlblTotInAfterAOBefore.Text = totInActAAOB.ToString();
                rptftlblTotInBeforeAOAfter.Text = totInActBAOA.ToString();
                rptftlblTotInBeforeAOBefore.Text = totInActBAOB.ToString();
                rptftlblTot3None.Text = tot3None.ToString();
                rptftlblTotalClient.Text = totalClient.ToString();
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dtClientAnalysis = ViewState["dtClientAnalysis"] as DataTable;

            if ((dtClientAnalysis != null) && (dtClientAnalysis.Rows.Count > 0))
            {
                try
                {
                    StringWriter stringWriter = new StringWriter();
                    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

                    rptClientAnalysis.RenderControl(htmlWriter);
                    //First clean up the response.object
                    Response.Clear();
                    Response.Charset = "";

                    //Set the response mime type for excel
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "filename=ClientAnalysis_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xls");
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
                catch (Exception ex)
                {
                    divMessage.InnerHtml = "Error in generating Excel File! Please try again.";
                }
            }
            else
            {
                divMessage.InnerHtml = "No records to generate Excel File!";
            }
        }
    }
}
