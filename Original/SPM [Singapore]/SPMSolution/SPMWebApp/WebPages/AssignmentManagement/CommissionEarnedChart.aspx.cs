using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace SPMWebApp.WebPages.AssignmentManagement
{
    public partial class CommissionEarnedChart : System.Web.UI.Page
    {
        private const String XChartValue = "Category";
        private const String YChartValue = "CommissionEarned";

        protected void Page_Load(object sender, EventArgs e)
        {
            //chart_CommissionEarned.ImageStorageMode = System.Web.UI.DataVisualization.Charting.ImageStorageMode.UseImageLocation;
            if (!IsPostBack)
            {
                if (Session["CommissionEarnedHistory"] != null)
                {
                    DataSet dsCommissionEarnedHistory = (DataSet)Session["CommissionEarnedHistory"];
                    if (dsCommissionEarnedHistory.Tables.Count > 0)
                    {
                        if (dsCommissionEarnedHistory.Tables["ReturnTable"].Rows[0]["ReturnCode"].ToString() == "1")
                        {
                            this.chart_CommissionEarned.Visible = true;
                            BindCommissionEarnedTable(dsCommissionEarnedHistory.Tables["CommissionEarned"]);
                            BindCommissionEarnedChart(dsCommissionEarnedHistory.Tables["CommissionEarned"]);
                        }
                        else
                        {
                            divMessage.InnerHtml = dsCommissionEarnedHistory.Tables["ReturnTable"].Rows[0]["ReturnMessage"].ToString();
                            this.chart_CommissionEarned.Visible = false;
                        }
                    }
                }
            }
        }

        private void BindCommissionEarnedChart(DataTable commissionEarnedTable)
        {
            chart_CommissionEarned.DataSource = GetChartDataTable(commissionEarnedTable);

            // set series members names for the X and Y values 
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"].XValueMember = XChartValue;
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"].YValueMembers = YChartValue;
            chart_CommissionEarned.Series["chartSeries_CommissionEarned"]["PointWidth"] = Convert.ToString(0.6);


            // data bind to the selected data source     
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.Title = "Total";
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.TitleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisY.TitleForeColor = Color.Red;

            chart_CommissionEarned.ChartAreas["chartArea_CommissionEarned"].AxisX.ArrowStyle = System.Web.UI.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chart_CommissionEarned.DataBind();
        }

        private void BindCommissionEarnedTable(DataTable commissionEarnedTable)
        {
            this.gvCommissionEarned.DataSource = GetFormattedDataTable(commissionEarnedTable);
            this.gvCommissionEarned.DataBind();
                
        }

        public DataTable GetFormattedDataTable(DataTable source)
        {
            DataTable dest = new DataTable(source.TableName);
            dest.Columns.Add("Title");
            foreach (DataColumn c in source.Columns)
            {
                dest.Columns.Add(c.ToString());
            }

            for (int i = 0; i < source.Rows.Count; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            for (int r = 0; r < dest.Rows.Count; r++)
            {
                for (int col = 0; col < dest.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        dest.Rows[r][col] = "Total Earned";
                    }
                    else
                    {
                        String value = !String.IsNullOrEmpty(source.Rows[r][col - 1].ToString()) ? source.Rows[r][col - 1].ToString() : "0.00";
                        dest.Rows[r][col] = String.Format("{0:00.00}", Convert.ToDouble(value));
                    }
                }
            }

            dest.AcceptChanges();
            return dest;
        }

        public DataTable GetChartDataTable(DataTable source)
        {
            DataTable dest = new DataTable(source.TableName);
            dest.Columns.Add(XChartValue);
            dest.Columns.Add(YChartValue);

            for (int i = 0; i < source.Columns.Count; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            for (int r = 0; r < dest.Rows.Count; r++)
            {
                for (int c = 0; c < dest.Columns.Count; c++)
                {
                    if (c == 0)
                    {
                        String colName = source.Columns[r].ColumnName;
                        String caption = string.Empty;
                        switch (colName)
                        {
                            case "CashTrading":
                                caption = "CA";
                                break;
                            case "CFD":
                                caption = "CFD";
                                break;
                            case "Custodian":
                                caption = "CU";
                                break;
                            case "CashManagement":
                                caption = "KC";
                                break;
                            case "PhillipMargin":
                                caption = "M";
                                break;
                            case "PhillipFinancial":
                                caption = "PFN";
                                break;
                            case "DiscretionaryAccounts":
                                caption = "S2";
                                break;
                            case "UnitTrustNonWrap":
                                caption = "UT";
                                break;
                            case "AdvisoryAccounts":
                                caption = "UTW";
                                break;
                            default: break;
                        }

                        dest.Rows[r][c] = caption;
                    }
                    else
                    {
                        dest.Rows[r][c] = source.Rows[c - 1][r];
                    }
                }
            }

            dest.AcceptChanges();
            return dest;
        }

        protected void gvCommissionEarned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double total = 0;
                foreach (Object obj in (e.Row.DataItem as DataRowView).Row.ItemArray)
                {
                    double amt;
                    if (double.TryParse(obj.ToString(), out amt))
                    {
                        total += amt;
                    }
                }
                e.Row.Cells[10].Text = String.Format("{0:00.00}", total);
            }
        }
    }
}
