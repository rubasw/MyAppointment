using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

public partial class Login : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;

        string hashedPassword = HashPassword(password);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"SELECT PatientID, FullName FROM Patients 
                             WHERE Email = @Email AND PasswordHash = @PasswordHash";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Session["PatientID"] = reader["PatientID"];
                Session["FullName"] = reader["FullName"];

                Response.Redirect("Home.aspx");
            }
            else
            {
                lblMessage.Text = "البريد الإلكتروني أو كلمة المرور غير صحيحة.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

            con.Close();
        }
    }

    string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}