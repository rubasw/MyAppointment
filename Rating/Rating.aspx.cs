using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace ruba10828tl.Auth
{
    public partial class Rating : Page
    {
        private readonly string _conn =
            ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                BindAverages();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                INSERT INTO SiteRatings (FileNumber, Facilities, Usability, Staff, Cleanliness, WaitTime, Comments)
                VALUES (@FileNumber, @Facilities, @Usability, @Staff, @Cleanliness, @WaitTime, @Comments);", con))
            {
                cmd.Parameters.AddWithValue("@FileNumber", (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Facilities", Convert.ToInt32(rblFacilities.SelectedValue));
                cmd.Parameters.AddWithValue("@Usability", Convert.ToInt32(rblUsability.SelectedValue));
                cmd.Parameters.AddWithValue("@Staff", Convert.ToInt32(rblStaff.SelectedValue));
                cmd.Parameters.AddWithValue("@Cleanliness", Convert.ToInt32(rblCleanliness.SelectedValue));
                cmd.Parameters.AddWithValue("@WaitTime", Convert.ToInt32(rblWait.SelectedValue));
                cmd.Parameters.AddWithValue("@Comments", string.IsNullOrWhiteSpace(txtComments.Text) ? (object)DBNull.Value : txtComments.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.CssClass = "ok";
            lblMsg.Text = "Thanks! Your rating has been submitted.";

            // إعادة تحميل
            BindGrid();
            BindAverages();

            // تفريغ الاختيارات
            rblFacilities.ClearSelection();
            rblUsability.ClearSelection();
            rblStaff.ClearSelection();
            rblCleanliness.ClearSelection();
            rblWait.ClearSelection();
            txtComments.Text = string.Empty;
        }

        private void BindGrid()
        {
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(
                @"SELECT TOP 50 CreatedAt, Facilities, Usability, Staff, Cleanliness, WaitTime, Comments
                  FROM SiteRatings ORDER BY CreatedAt DESC", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvRatings.DataSource = dt;
                gvRatings.DataBind();
            }
        }

        private void BindAverages()
        {
            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                @"SELECT 
                    AVG(CAST(Facilities  AS FLOAT)) AS F,
                    AVG(CAST(Usability   AS FLOAT)) AS U,
                    AVG(CAST(Staff       AS FLOAT)) AS S,
                    AVG(CAST(Cleanliness AS FLOAT)) AS C,
                    AVG(CAST(WaitTime    AS FLOAT)) AS W
                  FROM SiteRatings;", con))
            {
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        avgF.Text = FormatAvg(r["F"]);
                        avgU.Text = FormatAvg(r["U"]);
                        avgS.Text = FormatAvg(r["S"]);
                        avgC.Text = FormatAvg(r["C"]);
                        avgW.Text = FormatAvg(r["W"]);
                    }
                }
            }
        }

        private string FormatAvg(object o)
        {
            return o == DBNull.Value ? "-" : string.Format("{0:0.0}", o);
        }
    }
}