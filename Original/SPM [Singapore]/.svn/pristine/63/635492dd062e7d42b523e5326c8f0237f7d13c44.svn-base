using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SPMWebApp.WebPages.ContactManagement
{
    public partial class AvailableFundsBalance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // MMF column is "Y"
                if (Session["CashAndEquivalents"] != null)
                {
                    DataSet dsCashAndEquivalents = (DataSet)Session["CashAndEquivalents"];
                    if (dsCashAndEquivalents != null)
                    {
                        if (dsCashAndEquivalents.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            this.gvCashAndEquivalents.DataSource = dsCashAndEquivalents.Tables[0];
                            this.gvCashAndEquivalents.DataBind();
                        }
                        else
                        {
                            divMessage.InnerHtml = dsCashAndEquivalents.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                        }
                    }

                    divAvailableFundsBalance.Visible = false;
                    divCashAndEquivanents.Visible = true;
                }
                // MMF column is "N"
                else if (Session["AvailableFunds"] != null)
                {
                    DataSet dsAvailableFunds = (DataSet)Session["AvailableFunds"];
                    if (dsAvailableFunds != null)
                    {
                        if (dsAvailableFunds.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            divAvailableFundsBalance.InnerHtml = "Available Cash: " + dsAvailableFunds.Tables[0].Rows[0]["AvailableFunds"].ToString();
                        }
                        else
                        {
                            divMessage.InnerHtml = dsAvailableFunds.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                        }
                    }

                    divAvailableFundsBalance.Visible = true;
                    divCashAndEquivanents.Visible = false;
                }
                else
                {
                    divMessage.InnerHtml = "No Records Found!";
                    divAvailableFundsBalance.Visible = false;
                    divCashAndEquivanents.Visible = false;
                }
            }
        }

        private double totalcostTotal = 0.0;
        private double marketvalueTotal = 0.0;
        private double portTotal = 0.0;
        private double unrealisedplTotal = 0.0;
        private double profitlossTotal = 0.0;

        protected void gvCashAndEquivalents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double totalcostRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalcost"));
                totalcostTotal = totalcostTotal + totalcostRowTotal;
                double marketvalueRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "marketvalue"));
                marketvalueTotal = marketvalueTotal + marketvalueRowTotal;
                double portRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "port"));
                portTotal = portTotal + portRowTotal;
                double unrealisedplRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "unrealisedpl"));
                unrealisedplTotal = unrealisedplTotal + unrealisedplRowTotal;
                double profitlossRowTotal = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "profitloss"));
                profitlossTotal = profitlossTotal + profitlossRowTotal;
                
                e.Row.Cells[3].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "quantity").ToString()));
                e.Row.Cells[4].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "avgprice").ToString()));
                e.Row.Cells[5].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "closingprice").ToString()));
                e.Row.Cells[6].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalcost").ToString()));
                e.Row.Cells[7].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "marketvalue").ToString()));
                e.Row.Cells[8].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "port").ToString()));
                e.Row.Cells[9].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "unrealisedpl").ToString()));
                e.Row.Cells[10].Text = String.Format("{0:00.00}", Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "profitloss").ToString()));                
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {                
                e.Row.Cells[6].Text = String.Format("{0:0.00}", totalcostTotal);
                e.Row.Cells[7].Text = String.Format("{0:0.00}", marketvalueTotal);
                e.Row.Cells[8].Text = String.Format("{0:0.00}", portTotal);
                e.Row.Cells[9].Text = String.Format("{0:0.00}", unrealisedplTotal);
                e.Row.Cells[10].Text = String.Format("{0:0.00}", profitlossTotal);
            }
        }

    }
}
