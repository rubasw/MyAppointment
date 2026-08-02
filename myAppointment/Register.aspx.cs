using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

public partial class Register : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (!chkTerms.Checked)
        {
            Response.Write("<script>alert('يجب الموافقة على الشروط');</script>");
            return;
        }

        string fullName = txtFullName.Text.Trim();
        string gender = rblGender.SelectedValue;
        string dob = txtDOB.Text;
        string phone = txtPhone.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;
        bool notifications = chkNotifications.Checked;

        string hashedPassword = HashPassword(password);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO Patients (FullName, Gender, DateOfBirth, Phone, Email, PasswordHash, Notifications, CreatedAt)
                             VALUES (@FullName, @Gender, @DOB, @Phone, @Email, @PasswordHash, @Notifications, @CreatedAt)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@DOB", dob);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
            cmd.Parameters.AddWithValue("@Notifications", notifications);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        Response.Redirect("Login.aspx");
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