using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.IO;
using SPMWebApp.Services;
using System.Data;
using System.Web.UI.WebControls;


namespace SPMWebApp
{
    public class EmailManager
    {

        public const string TEMPLATEID = "1";
        public const string LEADS_TEMPLATEID = "2";
        public const string FOLLOWUP_TEMPLATEID = "3";

        public EmailManager()
        {
        }      

        public string GetSettingValue(string key)
        {
            string settingValue = string.Empty;
            settingValue = System.Configuration.ConfigurationManager.AppSettings.Get(key);
            return settingValue;
       
        }

        public string getAssignmentAnnouncementEmail(string templateID,string dbConnectionStr)
        {
            string EmailContent = "";
            DataSet emailDS = new DataSet();
            EmailTemplateService emailTemplate = new EmailTemplateService(dbConnectionStr);
            emailDS = emailTemplate.RetrieveEmailTemplateByID(templateID);
            if (emailDS.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                DataTable dtEmailTemplate = emailDS.Tables[0];
                if (dtEmailTemplate.Rows.Count == 1)
                {
                    EmailContent += "<html><body>";
                    foreach (DataRow row in dtEmailTemplate.Rows)
                    {
                        EmailContent += row["Contents"].ToString();
                    }
                }
            }
            EmailContent += "</body></html>";
            return EmailContent;
        }

        public void SendPromotionEmail(string FromEmail, string ToEmail, string Subject, String Contents, DataTable Attachment,string pLogo)
        {
            #region Original is commented by Thet Maung Chaw

            //string style = string.Empty;
            //string logoContents = "";

            ///// <Original>
            ////logoContents = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=gb2312'>";
            ////logoContents = logoContents + "    <meta name='GENERATOR' content='Microsoft FrontPage 4.0'><meta name='ProgId' content='FrontPage.Editor.Document'>";            

            ///// <Updated by Thet Maung Chaw>
            ///// <Reason: Not to go to Junk folder in outlook.>
            //logoContents = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=us-ascii'>";
            //logoContents = logoContents + "    <meta name='GENERATOR' content='Microsoft Word 12 (filtered)'>";

            //logoContents = logoContents + "<title></title> </head>";

            //string companyLogoUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + "/images/logo.jpg";
            //logoContents = logoContents + @"<body><img src='https://10.30.2.64/POEMSAdmin/asp/CustSupport/CallCenter/SPM/SpmPhaseII/images\logo.jpg' alt='Phillip Securities Pte Ltd'></body></html>";

            ///// <Commented by Thet Maung Chaw>
            //logoContents = logoContents + "<body><img src='" + companyLogoUrl + "' alt='Phillip Securities Pte Ltd'></body></html>";

            //style = "<style>body, td { font-family:tahoma; font-size:10pt; }</style>";

            //Contents = style + logoContents + Contents;

            #endregion

            //Contents = Contents.Replace("<meta name='Generator' content='Microsoft Word 12'>", "");

            System.Web.Mail.MailMessage objMM = new System.Web.Mail.MailMessage();
            objMM.From = "PhillipCapital <spm@phillip.com.sg>";
           
            for (int i = 0; i < Attachment.Rows.Count; i++)
            {
                string attach = Attachment.Rows[i]["FilePath"].ToString();
                if (!String.IsNullOrEmpty(attach))
                {
                    MailAttachment attachFile = new MailAttachment(HttpContext.Current.Server.MapPath(attach));
                    objMM.Attachments.Add(attachFile);
                }
            }                       

            objMM.BodyFormat = MailFormat.Html;
            objMM.Priority = MailPriority.Normal;
            objMM.Subject = Subject;

            objMM.To = ToEmail;
            //objMM.Body = "This is testing email.  <hr>" + Contents;
            objMM.Body = Contents;
            SmtpMail.SmtpServer = GetSettingValue("SmtpIP");            
            SmtpMail.Send(objMM);

        }

        public void SendPromotionEmail(string FromEmail, string ToEmail, string Subject, String Contents,string pLogo)
        {
            System.Web.Mail.MailMessage objMM = new System.Web.Mail.MailMessage();            
            objMM.From = "PhillipCapital <spm@phillip.com.sg>";
            objMM.BodyFormat = MailFormat.Html;
            objMM.Priority = MailPriority.Normal;
            objMM.Subject = Subject;            
            objMM.To = ToEmail;

            #region Orginal is commented by Thet Maung Chaw

            //string style = string.Empty;
            //string companyLogoUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + "/images/logo.jpg";

            //string logoContents = "";

            ///// <Original>
            ////logoContents = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=gb2312'>";
            ////logoContents = logoContents + "    <meta name='GENERATOR' content='Microsoft FrontPage 4.0'><meta name='ProgId' content='FrontPage.Editor.Document'>";

            ///// <Updated by Thet Maung Chaw>
            ///// <Reason: Not to go to Junk folder in outlook.>
            //logoContents = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=us-ascii'>";
            //logoContents = logoContents + "    <meta name='GENERATOR' content='Microsoft Word 14 (filtered medium)'>";

            //logoContents = logoContents + "<title></title> </head>";

            ////logoContents = logoContents + @"<body><img src='http://10.30.2.64/POEMSAdmin/asp/CustSupport/CallCenter/SPM/SpmPhaseII/images\logo.jpg' alt='Phillip Securities Pte Ltd'></body></html>";            
            //logoContents = logoContents + "<body><img src='" + companyLogoUrl + "' alt='Phillip Securities Pte Ltd'></body></html>";

            //style = "<style>body, td { font-family:tahoma; font-size:10pt; }</style>";
            //Contents = style + logoContents + Contents;

            #endregion

            //objMM.Body = "This is testing email. <hr>" + Contents;
            objMM.Body = Contents;

            SmtpMail.SmtpServer = GetSettingValue("SmtpIP");
            SmtpMail.Send(objMM);
        }


        public void SendEmail(string FromEmail,string ToEmail, string Subject, String Contents)
        {
            string style = string.Empty;
            style = "<style>body, td { font-family:tahoma; font-size:10pt; }</style>";
            Contents = style + Contents;

            System.Web.Mail.MailMessage objMM = new System.Web.Mail.MailMessage();
            objMM.From = "PhillipCapital <" + FromEmail + ">";
            objMM.BodyFormat = MailFormat.Html;
            objMM.Priority = MailPriority.Normal;
            objMM.Subject = Subject;

            objMM.To = ToEmail;
            //objMM.Body = "This is testing email. <hr>" + Contents;
            objMM.Body = Contents;
            SmtpMail.SmtpServer = GetSettingValue("SmtpIP");
            SmtpMail.Send(objMM);
          
        }

        public void SendDTSEmail(string FromEmail, string ToEmail, string Subject, String Contents)
        {
            string style = string.Empty;
            style = "<style>body, td { font-family:tahoma; font-size:10pt; }  table ,tr,th,td { border-color:black; }</style>";
            Contents = style + Contents;

            System.Web.Mail.MailMessage objMM = new System.Web.Mail.MailMessage();
            objMM.From = "PhillipCapital <" + FromEmail + ">";
            objMM.BodyFormat = MailFormat.Html;
            objMM.Priority = MailPriority.Normal;
            objMM.Subject = Subject;

            objMM.To = ToEmail;           
            objMM.Body = Contents;
            SmtpMail.SmtpServer = GetSettingValue("SmtpIP");
            SmtpMail.Send(objMM);

        }

        public string ReadTextFile(string FilePath)
        {
            string fs = "";
            try
            {
                fs = "<html><head><meta http-equiv='Content-Type' content='text/html; charset=us-ascii'><meta name=Generator content='Microsoft Word 12 (filtered)'></head><body>";
                fs = fs + File.ReadAllText(FilePath, System.Text.Encoding.ASCII);
                fs = fs + "</body></html>";               
            }
            catch
            {
                fs = "";
            }
            finally
            {
            }
            return fs;
        }
    
        public string ReplaceSpecialcharacter(string EmailContent,string param1,string param2)
        {
            EmailContent = EmailContent.Replace("&lt;&lt;Client Name&gt;&gt;", param1);
            EmailContent = EmailContent.Replace("&lt;&lt;Account No&gt;&gt;", param2);
            EmailContent = EmailContent.Replace("%NAME%", param1);           
            EmailContent = EmailContent.Replace("***agent/dealer name***", param1);
            return EmailContent;
        }

        /// <Added by Thet Maung Chaw>
        public String ReplaceImage(String EmailContent, String[] param)
        {
            return String.Empty;
        }

        /// <Added by OC>
        public String ReplaceSpecialcharacterForAssignment(String EmailContent, String DealerName, int NoOfClient, String DeadLineDate)
        {
            EmailContent = EmailContent.Replace("***DealerName***", DealerName);
            EmailContent = EmailContent.Replace("***NumOfClients***", NoOfClient.ToString());
            EmailContent = EmailContent.Replace("***CutOffDate***", DateTime.Parse(DeadLineDate).ToString("dd/MM/yyyy"));
            EmailContent = ReplaceSpecialcharacter(EmailContent);
            return EmailContent;
        }

        /// <Added by OC>
        public String ReplaceSpecialcharacterForLead(String EmailContent, String DealerName, int NoOfClient, String DeadLineDate)
        {
            EmailContent = EmailContent.Replace("***DealerName***", DealerName);
            EmailContent = EmailContent.Replace("***NumOfLeads***", NoOfClient.ToString());
            EmailContent = EmailContent.Replace("***CutOffDate***", DateTime.Parse(DeadLineDate).ToString("dd/MM/yyyy"));
            EmailContent = ReplaceSpecialcharacter(EmailContent);
            return EmailContent;
        }

        /// <Added by OC>
        public String ReplaceSpecialcharacterForFollowUp(String EmailContent, String DealerName, String AcctNo)
        {
            EmailContent = EmailContent.Replace("***DealerName***", DealerName);
            EmailContent = EmailContent.Replace("***AcctNo***", AcctNo);
            EmailContent = ReplaceSpecialcharacter(EmailContent);
            return EmailContent;
        }

        /// <Added by OC>
        public string ReplaceSpecialcharacterForNewLine(String EmailContent)
        {
            EmailContent = EmailContent.Replace("***NewLine***", "</br>");
            return EmailContent;
        }

        /// <Added by Thet Maung Chaw>
        public String ReplacePhotos(String EmailContent, String[] Photos)
        {
            String ImageSrc = String.Empty;

            foreach (String Photo in Photos)
            {
                ImageSrc = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + "/images/";

                //if (ImageSrc.Contains("https"))
                //{
                //    ImageSrc = ImageSrc.Replace("https", "http");
                //}

                EmailContent = EmailContent.Replace("***image_" + Photo.Substring(0, Photo.Length - 4).Trim() + "_image***", "<img src='" + ImageSrc + Photo + "'>");
            }

            return EmailContent;
        }

        /// <Added by Thet Maung Chaw>
        public void RemoveUnNecessaryText(String readPath, String writePath)
        {
            Boolean ContinueReading = true;
            Boolean Style = false;

            String line;
            StreamReader sr = new StreamReader(readPath);
            StreamWriter sw = new StreamWriter(writePath);

            while ((line = sr.ReadLine()) != null)
            {
                if ((!line.StartsWith("<!--") || line.Substring(line.Length - 3, 3) == "-->" ||
                    line.StartsWith("<style>") || line.EndsWith("</style>") || Style == true) &&
                    !line.StartsWith("<link") &&
                    ContinueReading == true)
                {
                    sw.WriteLine(line);
                    ContinueReading = true;

                    if (line.StartsWith("<style>"))
                    {
                        Style = true;
                    }
                    else if (line.StartsWith("</style>"))
                    {
                        Style = false;
                    }
                }
                else
                {
                    if (line.Substring(line.Length - 3, 3) == "-->" || Style == true)
                    {
                        ContinueReading = true;
                    }
                    else
                    {
                        ContinueReading = false;
                    }
                }
            }

            sw.Close();
            sr.Close();
        }

        public String ReplaceUnNecessaryText(String Content)
        {
            int start;
            int end;

            start = Content.IndexOf("<html");
            end = Content.IndexOf(">");

            Content = Content.Remove(start, end + 1);
            Content = "<html>" + Content;

            return Content = Content.Replace("Microsoft Word 12", "Microsoft Word 12 (filtered)");
        }

        public string ReplaceAcctNo(string EmailContent, string param)
        {
            EmailContent = EmailContent.Replace("%ACCTNO%", param);
            return EmailContent;
        }

        public string ReplaceSpecialcharacter(string EmailContent)
        {
            EmailContent = EmailContent.Replace("\n", "</br>");
            EmailContent = EmailContent.Replace("\r\n", "<br>");
            return EmailContent;
        }

        public string ReplaceDeadlineDate(string EmailContent, string param1)
        {
            EmailContent = EmailContent.Replace("***dd/mm/yyyy***", param1);
            return EmailContent;
        }

        public string ReplaceTotalClient(string EmailContentsInfo, int noOfClient)
        {
            EmailContentsInfo = EmailContentsInfo.Replace("*****", noOfClient.ToString());
            return EmailContentsInfo;
        }

        public string GetDealerEmailByDealerCode(string dealerCode,string dbConnectionStr)
        {
            string DealerEmail = "";
            DataSet DealerDs = new DataSet();
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            DealerDs = clientAssignmentService.RetrieveDealerEmailByDealerCode(dealerCode);
            if (DealerDs.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                foreach (DataRow dr in DealerDs.Tables[0].Rows)
                {
                    DealerEmail = dr["Email"].ToString();
                }
            }
            return DealerEmail;
        }

        public string GetTeamEmail(string teamCode, string dbConnectionStr)
        {
            string teamEmail = "";
            DataSet DealerDs = new DataSet();
            ClientAssignmentService clientAssignmentService = new ClientAssignmentService(dbConnectionStr);
            DealerDs = clientAssignmentService.RetrieveTeamEmailByTeamCode(teamCode);
            if (DealerDs.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
            {
                foreach (DataRow dr in DealerDs.Tables[0].Rows)
                {
                    teamEmail = dr["Email"].ToString();
                }
            }
            return teamEmail;
        }
    }
}
