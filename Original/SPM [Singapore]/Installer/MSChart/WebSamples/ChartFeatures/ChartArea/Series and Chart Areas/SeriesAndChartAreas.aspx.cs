using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Drawing2D;
using System.Web.UI.DataVisualization.Charting;

namespace System.Web.UI.DataVisualization.Charting.Samples
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class SeriesAndChartAreas : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Attach the first series to a chart area.
			if( Series1.SelectedItem.Value == "None" )
                Chart1.Series["Series1"].ChartArea = "";
			else
				Chart1.Series["Series1"].ChartArea = Series1.SelectedItem.Value;

			// Attach the second series to a chart area.
			if( Series2.SelectedItem.Value == "None" )
				Chart1.Series["Series2"].ChartArea = "";
			else
				Chart1.Series["Series2"].ChartArea = Series2.SelectedItem.Value;

			// Attach the Third series to a chart area.
			if( Series3.SelectedItem.Value == "None" )
				Chart1.Series["Series3"].ChartArea = "";
			else
				Chart1.Series["Series3"].ChartArea = Series3.SelectedItem.Value;
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		
		
	
	}	
}
