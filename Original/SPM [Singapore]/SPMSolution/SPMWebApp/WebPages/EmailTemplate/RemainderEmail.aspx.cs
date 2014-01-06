﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPMWebApp.Services;
using System.Data;
using System.Text;

using SPMWebApp.Utilities;
using System.Configuration;

namespace SPMWebApp
{
    public partial class RemainderEmail : System.Web.UI.Page
    {
        private ClientAssignmentService clientAssignmentService;

        protected void Page_Load(object sender, EventArgs e)
        {
            String dbConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString.ToString();
            AutomatedRemainderEmailByFormula(dbConnectionString);
            AutomatedRemainderEmailByFollowUpDate(dbConnectionString);
        }

        #region Test
        protected void Button1_Click(object sender, EventArgs e)
        {
            String dbConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString.ToString();
            AutomatedRemainderEmailByFormula(dbConnectionString);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            String dbConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString.ToString();
            //AutomatedRemainderEmailByFollowUpDate();
        }
        #endregion

        private void AutomatedRemainderEmailByFormula(String dbConnectionStr)
        {
            /*(1) Retrieve all dealers information
             *(2) RetrieveCallReportDetail 
             *     (a) IF(GetDate()<='CutOffDate') =>
             *     (DateDiff(AssignDate-CutOffDate)/2 ) HalfofProjectPeriod
             *      CutOff-GetDate() = No of Days left for Project Period
             *     (b)No of Days Left <= Half of Project =>  Calculate BaseFigure and No of Assign Call Left
             *     (c)Sent automated email to dealer
             *      
             *(3) Sent automated email to dealer
                        
            */
            try
            {
                clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
                DataSet ds = clientAssignmentService.RetrieveAllDealer("");

                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string DealerName = ds.Tables[0].Rows[i]["DisplayName"].ToString();
                        string AECode = ds.Tables[0].Rows[i]["AECode"].ToString();

                        DataSet dsAssignReport = clientAssignmentService.RetrieveAssignReportByDealer(AECode);
                        if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            DataTable dtReport = null;
                            dtReport = CalculateReport(dsAssignReport.Tables[0]);
                            if (dtReport.Rows.Count > 0)
                            {
                                SentReminderByFormula(dbConnectionStr, dtReport, AECode, DealerName);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.divMessage1.InnerHtml = this.divMessage1.InnerHtml + " " + e.ToString() + " <br/>";
            }
            finally
            {
                this.divMessage1.InnerHtml = this.divMessage1.InnerHtml + " " + "Successful Sending Remainder email by Calls left" + "<br/>";
            }
        }

        private void AutomatedRemainderEmailByFollowUpDate(String dbConnectionStr)
        {
            try
            {
                //(1) Retrieve all dealers RetrieveAllDealer()
                clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
                DataSet ds = clientAssignmentService.RetrieveAllDealer("");
                if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string DealerName = ds.Tables[0].Rows[i]["DisplayName"].ToString();
                        string AECode = ds.Tables[0].Rows[i]["AECode"].ToString();

                        DataSet dsAssignReport = clientAssignmentService.RetrieveAssignReportByFollowUpDate(AECode);
                        if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            if (dsAssignReport.Tables[0].Rows.Count > 0)
                            {
                                SentReminderByFollowUp(dbConnectionStr, dsAssignReport.Tables[0], AECode, DealerName);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.divMessage2.InnerHtml = this.divMessage2.InnerHtml + " " + e.ToString() + " <br/>";
            }
            finally
            {
                this.divMessage2.InnerHtml = this.divMessage2.InnerHtml + " " + "Successful Sending Remainder email by Calls left" + "<br/>";
            }
        }

        private void SentReminderByFollowUp(String dbConnectionStr, DataTable dtReport, string dealerCode, string DealerName)
        {
            StringBuilder sb = new StringBuilder("Dear ").Append(DealerName).Append(",<br><br>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0' width='850'><tr height='50px'><th>Project Name</th><th>Cut Off Date Time</th><th>Number of Calls Left</th>");
            sb.Append("<th>Number of Calls to</br> Follow-Up</th><th>Follow-up Date</th></tr>");
            for (int i = 0; i < dtReport.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<td width=30% align='left'>").Append(dtReport.Rows[i]["ProjectName"].ToString()).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=20% align='center'>").Append(ConvertDateTimeFormat(Convert.ToDateTime(dtReport.Rows[i]["CutOffDateTime"].ToString()))).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=15% align='center'>").Append(dtReport.Rows[i]["NoCallsLeft"].ToString()).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=15% align='center'>").Append(dtReport.Rows[i]["NoCallsToFollowUp"].ToString()).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=20% align='center'>").Append(ConvertDateTimeFormat(Convert.ToDateTime(dtReport.Rows[i]["FollowUpDate"].ToString()))).Append("&nbsp;").Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append("<br><br><br>");
            sb.Append("Thank you.");
            SentEmail(dbConnectionStr, dealerCode, sb);
        }

        private void SentReminderByFormula(String dbConnectionStr, DataTable dtReport, string dealerCode, string DealerName)
        {
            StringBuilder sb = new StringBuilder("Dear ").Append(DealerName).Append(",<br><br>");
            sb.Append("<table border='1' cellspacing='0' cellpadding='0' width='850'><tr><th>Project Name</th><th>Cut Off Time</th><th>Number of Calls Left</th>");
            sb.Append("<th>Number of Calls to</br> Follow Up</th><th>Follow-up Date</th></tr>");
            for (int i = 0; i < dtReport.Rows.Count; i++)
            {
                sb.Append("<tr>");
                sb.Append("<td width=30% align='left'>").Append(dtReport.Rows[i]["ProjectName"].ToString()).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=20% align='center'>").Append(ConvertDateTimeFormat(Convert.ToDateTime(dtReport.Rows[i]["CutOffDateTime"].ToString()))).Append("&nbsp;").Append("</td>");
                sb.Append("<td width=15% align='center'>").Append(dtReport.Rows[i]["NoCallsLeft"].ToString()).Append("&nbsp;").Append("</td>");

                clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
                DataSet dsFollowUpDate = clientAssignmentService.RetrieveFollowUpDateByProjectID(dtReport.Rows[i]["ProjectID"].ToString(), dealerCode);
                if (dsFollowUpDate.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                {
                    DataTable dtFollowUpDate = dsFollowUpDate.Tables[0];
                    sb.Append("<td width=15% align='center'>");
                    for (int j = 0; j < dtFollowUpDate.Rows.Count; j++)
                    {
                        sb.Append(dtFollowUpDate.Rows[j]["NoCallsToFollowUp"].ToString()).Append("<br>");

                    }
                    sb.Append("&nbsp;").Append("</td>");
                    sb.Append("<td width=20% align='center'>");
                    for (int j = 0; j < dtFollowUpDate.Rows.Count; j++)
                    {
                        sb.Append(dtFollowUpDate.Rows[j]["FollowUpDate"].ToString()).Append("<br>");
                    }
                    sb.Append("&nbsp;").Append("</td>");
                }
                else
                {
                    sb.Append("<td width='200px' align='center'>&nbsp;</td>");
                    sb.Append("<td width='200px' align='center'>&nbsp;</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append("<br><br>");
            sb.Append("Thank you.");
            SentEmail(dbConnectionStr, dealerCode, sb);
        }

        private String ConvertDateTimeFormat(DateTime datetime)
        {
            if (datetime != null)
            {
                return String.Format("{0:dd/MM/yyyy}", datetime);
            }
            else
            {
                return string.Empty;
            }
        }

        private void SentEmail(String dbConnectionStr, String dealerCode, StringBuilder sb)
        {
            string dealerEmail = "";
            EmailManager emailSender = new EmailManager();
            CommonUtilities common = new CommonUtilities();
            dealerEmail = emailSender.GetDealerEmailByDealerCode(dealerCode, dbConnectionStr);
            // dealerEmail = "thetmc@cyberquote.com.sg";
            if (!String.IsNullOrEmpty(dealerEmail))
            {
                if (common.isEmail(dealerEmail.Trim()))
                {
                    string EmailSubject = "Reminder on Call Report Status";
                    emailSender.SendDTSEmail("spm@phillip.com.sg", dealerEmail, EmailSubject, sb.ToString());
                    clientAssignmentService.InsertEmailLog("spm@phillip.com.sg", dealerEmail, EmailSubject, sb.ToString());
                }
            }
        }

        private DataTable CalculateReport(DataTable dataTable)
        {
            DataTable dtReport = new DataTable("dtReport");
            dtReport.Columns.Add("ProjectID", String.Empty.GetType());
            dtReport.Columns.Add("ProjectName", String.Empty.GetType());
            dtReport.Columns.Add("CutOffDateTime", String.Empty.GetType());
            dtReport.Columns.Add("NoCallsLeft", String.Empty.GetType());
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                int TotalAssign = 0;
                int NoOfAssignDays = 0;
                int RemainingAssignedDays = 0;
                int basicFigure = 0;
                int AssignCallLeft = 0;
                int TotalCalls = 0;

                TotalAssign = Convert.ToInt32(dataTable.Rows[i]["TotalAssign"].ToString());
                NoOfAssignDays = Convert.ToInt32(dataTable.Rows[i]["AssignDays"].ToString());
                RemainingAssignedDays = Convert.ToInt32(dataTable.Rows[i]["AssignedDayLeft"].ToString());
                TotalCalls = Convert.ToInt32(dataTable.Rows[i]["TotalCalls"].ToString());
                basicFigure = calculateBasicFigure(TotalAssign, NoOfAssignDays, RemainingAssignedDays);
                AssignCallLeft = TotalAssign - TotalCalls;
                if (AssignCallLeft > basicFigure)
                {
                    DataRow drNewContact = null;
                    drNewContact = dtReport.NewRow();
                    drNewContact["ProjectID"] = dataTable.Rows[i]["ProjectID"].ToString();
                    drNewContact["ProjectName"] = dataTable.Rows[i]["ProjectName"].ToString();
                    drNewContact["CutOffDateTime"] = dataTable.Rows[i]["CutOffDate"].ToString();
                    drNewContact["NoCallsLeft"] = AssignCallLeft;
                    dtReport.Rows.Add(drNewContact);
                }
            }
            return dtReport;
        }

        private int calculateBasicFigure(int TotalAssign, int NoOfAssignDays, int RemainingAssignedDays)
        {
            decimal result = 0;
            result = (TotalAssign / NoOfAssignDays) * RemainingAssignedDays;
            int value = 0;
            value = Convert.ToInt32(Math.Ceiling(result));
            return value;
        }
    }
}