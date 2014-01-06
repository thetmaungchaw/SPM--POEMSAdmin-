using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SPMWebApp.Services;
using System.Text;

namespace SPMWebApp.WebPages.AccessControl
{
    public partial class SuperAdmin : BasePage.BasePage
    {
        SuperAdminService superAdminService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// <Initialize the paging>
                Paging(0);

                
                ddl_DataBind();
                dl_DataBind();


                // Capture Params from querystring
                try
                {
                    string userid = Request.QueryString["uid"];
                    string name = Request.QueryString["nm"];
                    string tab = Request.QueryString["tb"];
                    if(tab!="")
                        tcMainContainer.ActiveTabIndex = int.Parse(tab);
                    if (userid != "")
                        ddlUserID.SelectedValue = userid;
                    if ((name != "") && (tab=="1"))
                    {
                        txtUserIDSearch.Text = name;
                        SearchUser();   
                    }
                }
                catch (Exception)
                {
                }

                gvSuperAdmin_DataBind();

            }
        }

        private void gvSuperAdmin_DataBind()
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            ds = superAdminService.SuperAdmin_GetByFilters(ddlUserID.SelectedValue, ddlAEGroup.SelectedValue, StartPage, int.Parse(ddlRowPerPage.SelectedValue));

            if (ds.Tables[0].Rows.Count == 0)
                Paging(0);
            else
                Paging(int.Parse(ds.Tables[0].Rows[0]["Cnt"].ToString()));

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                gvSuperAdmin.DataSource = ds.Tables[0];
                gvSuperAdmin.DataBind();
            }
            else
            {
                labError.Text = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        private void ddl_DataBind()
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            ds = superAdminService.SuperAdmin_Getddl("", "", "", "");

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                ddlAEGroup.DataSource = ds.Tables["dtAEGroup"];
                ddlAEGroup.DataBind();
                ddlAEGroup.Items.Insert(0, new ListItem("-- Select --", ""));

                ddlUserID.DataSource = ds.Tables["dtUserID"];
                ddlUserID.DataBind();
                ddlUserID.Items.Insert(0, new ListItem("-- Select --", ""));
            }
        }

        private void dl_DataBind()
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            ds = superAdminService.SuperAdmin_Getddl("", "", "", "");

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                dlUserID.DataSource = ds.Tables["dtUserID"];
                dlUserID.DataBind();

                dlAEGroup.DataSource = ds.Tables["dtAEGroup"];
                dlAEGroup.DataBind();
            }
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            gvSuperAdmin_DataBind();
            labError.Text = "";
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            CheckBox chkIDelete;
            String UserID;
            String AEGroup;
            DataSet ds;

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            foreach (GridViewRow gvr in gvSuperAdmin.Rows)
            {
                chkIDelete = gvr.FindControl("chkIDelete") as CheckBox;

                if (chkIDelete.Checked == true)
                {
                    UserID = (gvr.FindControl("labUserID") as Label).Text;
                    AEGroup = (gvr.FindControl("labAEGroup") as Label).Text;

                    ds = superAdminService.SuperAdmin_Delete(UserID, AEGroup);

                    if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
                    {
                        gvSuperAdmin_DataBind();
                        labError.Text = "Deleted !";
                    }
                    else
                    {

                    }
                }
            }
            
        }

        protected void btnSearchAEGroup_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            String rdbtnAEGroupOption = String.Empty;

            if (rdbtnAEGroupStartWith.Checked)
            {
                rdbtnAEGroupOption = "S";
            }
            else if (rdbtnAEGroupEndWith.Checked)
            {
                rdbtnAEGroupOption = "E";
            }
            else
            {
                rdbtnAEGroupOption = "C";
            }

            ds = superAdminService.SuperAdmin_Getddl(txtUserIDSearch.Text.Trim(), txtSearchAEGroup.Text.Trim(), "", rdbtnAEGroupOption);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                dlAEGroup.DataSource = ds.Tables["dtAEGroup"];
                dlAEGroup.DataBind();
            }
            else
            {
            
            }

            if (ds.Tables["dtAEGroup"].Rows.Count == 0)
            {
                labAEGroupSelectAll.Enabled = false;
                chkAEGroupSelectAll.Enabled = false;
            }
            else
            {
                labAEGroupSelectAll.Enabled = true;
                chkAEGroupSelectAll.Enabled = true;
            }
        }

        protected void btnUserIDSearch_Click(object sender, EventArgs e)
        {
            SearchUser();            
        }

        private void SearchUser()
        {
            DataSet ds = new DataSet();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            String rdbtnUserIDOption = String.Empty;

            if (rdbtnUserIDStartWith.Checked)
            {
                rdbtnUserIDOption = "S";
            }
            else if (rdbtnUserIDEndWith.Checked)
            {
                rdbtnUserIDOption = "E";
            }
            else
            {
                rdbtnUserIDOption = "C";
            }

            ds = superAdminService.SuperAdmin_Getddl(txtUserIDSearch.Text.Trim(), txtSearchAEGroup.Text.Trim(), rdbtnUserIDOption, "");

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                dlUserID.DataSource = ds.Tables["dtUserID"];
                dlUserID.DataBind();
            }
            else
            {

            }

            if (ds.Tables["dtUserID"].Rows.Count == 0)
            {
                labUserIDSelectAll.Enabled = false;
                chkUserIDSelectAll.Enabled = false;
            }
            else
            {
                labUserIDSelectAll.Enabled = true;
                chkUserIDSelectAll.Enabled = true;
            }
        }

        protected void dlUserID_PreRender(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;

            foreach (DataListItem item in dlUserID.Items)
            {
                CheckBox chkUserID = item.FindControl("chkUserID") as CheckBox;
                cs.RegisterArrayDeclaration("chkUserID", String.Concat("'", chkUserID.ClientID, "'"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String UserID;
            String AEGroup;
            CheckBox chkUserID;
            CheckBox chkAEGroup;
            DataSet ds;
            StringBuilder sb = new StringBuilder();

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            foreach (DataListItem itemUserID in dlUserID.Items)
            {
                chkUserID = itemUserID.FindControl("chkUserID") as CheckBox;

                if (chkUserID.Checked == true)
                {
                    UserID = (itemUserID.FindControl("labUserID") as Label).Text;

                    foreach (DataListItem itemAEGroup in dlAEGroup.Items)
                    {
                        chkAEGroup = itemAEGroup.FindControl("chkAEGroup") as CheckBox;

                        if (chkAEGroup.Checked == true)
                        {
                            AEGroup = (itemAEGroup.FindControl("labAEGroup") as Label).Text;

                            ds = superAdminService.SuperAdmin_Insert(UserID, AEGroup);

                            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() != "1")
                            {
                                sb.Append(ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString() + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            

            if (sb.ToString() != String.Empty)
            {
                labError.Text = sb.ToString();
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Window", "alert('" + sb.ToString() + "');", true);
            }
            else
            {
                labError.Text = "Save Successful !";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "Window", "alert('Save Successful !');", true);
            }

            gvSuperAdmin_DataBind();            

        }

        protected void dlAEGroup_PreRender(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;

            foreach (DataListItem item in dlAEGroup.Items)
            {
                CheckBox chkAEGroup = item.FindControl("chkAEGroup") as CheckBox;
                cs.RegisterArrayDeclaration("chkAEGroup", String.Concat("'", chkAEGroup.ClientID, "'"));
            }
        }

        #region Paging

        public Boolean PagingButton
        {
            get
            {
                return Boolean.Parse(ViewState["PagingButton"].ToString());
            }
            set
            {
                ViewState["PagingButton"] = value;
            }
        }

        public int StartPage
        {
            get
            {
                if (ViewState["StartPage"] != null)
                {
                    if (PagingButton == true)
                    {
                        return int.Parse(ViewState["StartPage"].ToString());
                    }
                    else
                    {
                        ViewState["StartPage"] = 1;
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                ViewState["StartPage"] = value;
            }
        }

        public int RowPerPage
        {
            get
            {
                if (ViewState["RowPerPage"] != null)
                {
                    return int.Parse(ViewState["RowPerPage"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["RowPerPage"] = value;
            }
        }

        public int NoOfPage
        {
            get
            {
                if (ViewState["NoOfPage"] != null)
                {
                    return int.Parse(ViewState["NoOfPage"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["NoOfPage"] = value;
            }
        }

        public int TotalRecord
        {
            get
            {
                if (ViewState["TotalRecord"] != null)
                {
                    return int.Parse(ViewState["TotalRecord"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["TotalRecord"] = value;
            }
        }

        public int PageCheck
        {
            get
            {
                if (ViewState["PageCheck"] != null)
                    return int.Parse(ViewState["PageCheck"].ToString());
                else
                    return 0;
            }
            set { ViewState["PageCheck"] = value; }
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage = 1;
            gvSuperAdmin_DataBind();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage--;
            gvSuperAdmin_DataBind();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage++;
            gvSuperAdmin_DataBind();
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage = NoOfPage;
            gvSuperAdmin_DataBind();
        }

        protected void ddlRowPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage = 1;
            gvSuperAdmin_DataBind();
        }

        protected void ddlNoOfPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagingButton = true;
            StartPage = int.Parse(ddlNoOfPage.SelectedValue);
            gvSuperAdmin_DataBind();
        }

        private void Paging(int TotalRecord)
        {
            if (!String.IsNullOrEmpty(ddlRowPerPage.SelectedValue))
                RowPerPage = int.Parse(ddlRowPerPage.SelectedValue);

            PageCheck = TotalRecord % RowPerPage;

            if (PageCheck == 0)
            {
                NoOfPage = TotalRecord / RowPerPage;

                if (NoOfPage != 0)
                {
                    ddlNoOfPage.Items.Clear();

                    for (int i = 1; i <= NoOfPage; i++)
                    {
                        ddlNoOfPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                }
            }
            else
            {
                NoOfPage = TotalRecord / RowPerPage + 1;

                ddlNoOfPage.Items.Clear();

                for (int i = 1; i <= NoOfPage; i++)
                {
                    ddlNoOfPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }

            ddlNoOfPage.SelectedValue = StartPage.ToString();
            labTotalRecord.Text = TotalRecord.ToString();
            labTotalPage.Text = NoOfPage.ToString();

            if (NoOfPage == 0)
            {
                divPaging2.Visible = false;
            }
            else
            {
                divPaging2.Visible = true;
            }

            if (NoOfPage == 1)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;

                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            else if (StartPage == 1)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;

                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else if (StartPage != NoOfPage)
            {
                btnPrevious.Enabled = true;
                btnFirst.Enabled = true;

                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else if (StartPage == NoOfPage)
            {
                btnPrevious.Enabled = true;
                btnFirst.Enabled = true;

                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
        }

        #endregion

        protected void gvSuperAdmin_PreRender(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;

            foreach (GridViewRow gvr in gvSuperAdmin.Rows)
            {
                CheckBox chkIDelete = gvr.FindControl("chkIDelete") as CheckBox;
                cs.RegisterArrayDeclaration("chkIDelete", String.Concat("'", chkIDelete.ClientID, "'"));
            }
        }

        protected void btnDeleteByUserID_Click(object sender, EventArgs e)
        {
            DataSet ds;

            superAdminService = new SuperAdminService(base.dbConnectionStr);

            ds = superAdminService.SuperAdmin_Delete(ddlUserID.SelectedValue, String.Empty);

            if (ds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString().Equals("1"))
            {
                gvSuperAdmin_DataBind();
                labError.Text = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
            else
            {
                labError.Text = ds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
            }
        }

        
    }

    
}