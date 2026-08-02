using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace ruba10828tl.Auth
{
    public partial class Login : Page
    {
        private readonly string _conn =
            ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileNumber.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ClientScript.RegisterStartupScript(GetType(), "a", "alert('Enter File Number and Password');", true);
                return;
            }

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE FileNumber=@f AND [Password]=@p;", con))
            {
                cmd.Parameters.AddWithValue("@f", txtFileNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());

                con.Open();
                int ok = (int)cmd.ExecuteScalar();
                if (ok > 0)
                {
                    Response.Redirect("~/Appointment/BookAndview.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "b",
                        "alert('Invalid File Number or Password');", true);
                }
            }
        }
    }
}