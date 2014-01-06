using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SPMWebApp
{
    public partial class PagingControl : System.Web.UI.UserControl
    {
        private int startRowPerPage;
        private int endRowPerPage;
        private int rowPerPageIncrement;
        private int pageCount;
        //private int currentRowPerPage;

        public event EventHandler<EventArgs<Int32>> PageNoChanged;
        public event EventHandler<EventArgs<Int32>> RowPerPageChanged;

        //Only for calcuation
        private int changedPageNo;
        
        public int StartRowPerPage
        {
            get { return startRowPerPage; }
            set { startRowPerPage = value; }
        }        

        public int EndRowPerPage
        {
            get { return endRowPerPage; }
            set { endRowPerPage = value; }
        }

        public int RowPerPageIncrement
        {
            get { return rowPerPageIncrement; }
            set { rowPerPageIncrement = value; }
        }

        public int PageCount
        {
            get { return pageCount; }
            set 
            { 
                pageCount = value;
                hdfPageCount.Value = pageCount.ToString();
            }
        }

        public string CurrentPageNo
        {
            get { return ddlPageNo.SelectedValue; }
        }

        public string CurrentRowPerPage
        {
            get { return ddlRowPerPage.SelectedValue; }
            set { ddlRowPerPage.SelectedValue = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillRowPerPage();
            }
        }

        public void DisplayPaging()
        {
            lblTotalPage.Text = hdfPageCount.Value;
            ddlPageNo.Items.Clear();
            for (int i = 1; i <= pageCount; i++)
            {
                ddlPageNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ShowPagingLink(1);
        }        

        private void FillRowPerPage()
        {
            for (int i = startRowPerPage; i <= endRowPerPage; i = i + rowPerPageIncrement)
            {
                ddlRowPerPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void ShowPagingLink(int pageNo)
        {
            //int maxIndex = gvAssignments.PageCount - 1;
            pageCount = int.Parse(hdfPageCount.Value);

            lbtnFirst.Visible = true;
            lbtnNext.Visible = true;
            lbtnPrev.Visible = true;
            lbtnLast.Visible = true;

            if (pageNo == 1)
            {
                //First Page
                lbtnPrev.Visible = false;
                lbtnFirst.Visible = false;
                if (pageCount < 3)
                    lbtnLast.Visible = false;
            }
            else if (pageNo == pageCount)
            {
                //Last Page
                lbtnNext.Visible = false;
                lbtnLast.Visible = false;
                if (pageCount < 3)
                    lbtnFirst.Visible = false;
            }

            if (pageCount == 1)
            {
                lbtnNext.Visible = false;
            }
        }

        private void RaisePageNoChanged(int pageNo)
        {           
            //Raise PageNo Change event
            if (PageNoChanged != null)
                PageNoChanged(this, new EventArgs<Int32>(pageNo));
        }

        protected void lbtnFirst_Click(object sender, EventArgs e)
        {
            //lblPage.Text = "First : 0";
            ddlPageNo.SelectedValue = "1";

            ShowPagingLink(1);
            RaisePageNoChanged(1);
        }

        protected void lbtnPrev_Click(object sender, EventArgs e)
        {
            changedPageNo = int.Parse(ddlPageNo.SelectedValue) - 1;
            //lblPage.Text = "Previous : " + changedPageNo;

            ddlPageNo.SelectedValue = changedPageNo.ToString();
            ShowPagingLink(changedPageNo);
            RaisePageNoChanged(changedPageNo);
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlPageNo.SelectedValue))
            {
                changedPageNo = int.Parse(ddlPageNo.SelectedValue) + 1;
                //lblPage.Text = "Next : " + changedPageNo;

                ddlPageNo.SelectedValue = changedPageNo.ToString();
                ShowPagingLink(changedPageNo);
                RaisePageNoChanged(changedPageNo);
            }            
        }

        protected void lbtnLast_Click(object sender, EventArgs e)
        {
            changedPageNo = int.Parse(hdfPageCount.Value);
            //lblPage.Text = "Last : " + changedPageNo;

            ddlPageNo.SelectedValue = changedPageNo.ToString();
            ShowPagingLink(changedPageNo);
            RaisePageNoChanged(changedPageNo);
        }

        protected void ddlPageNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblPage.Text = "Page No Change : " + ddlPageNo.SelectedValue;
            changedPageNo = int.Parse(ddlPageNo.SelectedValue);
            ShowPagingLink(changedPageNo);
            RaisePageNoChanged(changedPageNo);
        }

        protected void ddlRowPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            changedPageNo = int.Parse(ddlRowPerPage.SelectedValue);
            if (RowPerPageChanged != null)
                RowPerPageChanged(this, new EventArgs<Int32>(changedPageNo));
        }
    }

    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;

        public T Value
        {
            get { return m_value; }
        }
    }

    //Creating Custom EventArgs class
    /*
    //Custom EventArgs class
    public class PagingEvent : EventArgs
    {
        private int pageNo;

        public int PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }

        public PagingEvent(int pageNo)
        {
            this.pageNo = pageNo;
        }                
    }
    
    //Declare EventHandler
    public event EventHandler<PagingEvent> PageNoChanged;
    
    //Raise Event
    private void RaisePageNoChanged(int pageNo)
    {            
        //Raise PageNo Change event
        if (PageNoChanged != null)
            PageNoChanged(this, new PagingEvent(pageNo));
    }
    
    //Event Handler
    protected void PageNoChangedHandler(object src, PagingEvent pe)
    {
        Console.WriteLine(pe.PageNo);
    } 
    */
}