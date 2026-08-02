using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using ClosedXML.Excel;

namespace ruba10828tl.Auth
{
    public partial class Register : Page
    {
        private readonly string _conn =
            ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAll();
                BindUsersDDL();
            }
        }

        // INSERT (Register)
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileNo.Text))
            { lblMsg.Text = "File Number is required."; return; }

            if (txtPassword.Text.Length <= 5)
            { lblMsg.Text = "Password must be more than 5 characters."; return; }

            if (txtPassword.Text != txtConfirmPassword.Text)
            { lblMsg.Text = "Passwords do not match."; return; }

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                IF EXISTS (SELECT 1 FROM Users WHERE FileNumber=@FileNumber)
                BEGIN
                    RAISERROR('File Number already exists.', 16, 1);
                    RETURN;
                END
                INSERT INTO Users (FileNumber, FullName, Age, Country, Email, Gender, Password)
                VALUES (@FileNumber, @FullName, @Age, @Country, @Email, @Gender, @Password);", con))
            {
                cmd.Parameters.AddWithValue("@FileNumber", txtFileNo.Text.Trim());
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Age", string.IsNullOrWhiteSpace(txtAge.Text) ? (object)DBNull.Value : int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", string.IsNullOrEmpty(rblGender.SelectedValue) ? (object)DBNull.Value : rblGender.SelectedValue);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    lblMsg.Text = "Registered successfully.";
                    ClearInputs();
                    BindAll();
                }
                catch (SqlException ex)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = ex.Message;
                }
            }
        }

        // UPDATE
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileNo.Text))
            { lblMsg.Text = "File Number is required for update."; return; }

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                UPDATE Users SET
                    FullName=@FullName,
                    Age=@Age,
                    Country=@Country,
                    Email=@Email,
                    Gender=@Gender,
                    Password=@Password
                WHERE FileNumber=@FileNumber;", con))
            {
                cmd.Parameters.AddWithValue("@FileNumber", txtFileNo.Text.Trim());
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Age", string.IsNullOrWhiteSpace(txtAge.Text) ? (object)DBNull.Value : int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Gender", string.IsNullOrEmpty(rblGender.SelectedValue) ? (object)DBNull.Value : rblGender.SelectedValue);
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = rows > 0 ? "Updated successfully." : "Record not found.";
                BindAll();
            }
        }

        // DELETE
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileNo.Text))
            { lblMsg.Text = "File Number is required for delete."; return; }

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("DELETE FROM Users WHERE FileNumber=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", txtFileNo.Text.Trim());
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = rows > 0 ? "Deleted." : "Record not found.";
                ClearInputs();
                BindAll();
            }
        }

        // EXPORT TO EXCEL
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetUsersDataTable();

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Users");
                ws.Tables.FirstOrDefault().ShowAutoFilter = false; // يشيل الفلتر
                ws.Row(1).Style.Font.Bold = true;

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    ms.Position = 0;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=users.xlsx");
                    Response.BinaryWrite(ms.ToArray());
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvUsers.SelectedRow;
            txtFileNo.Text = row.Cells[0].Text;
            txtFullName.Text = row.Cells[1].Text;
            txtAge.Text = row.Cells[2].Text;
            ddlCountry.SelectedValue = row.Cells[3].Text;
            txtEmail.Text = row.Cells[4].Text;
            var g = row.Cells[5].Text;
            if (rblGender.Items.FindByValue(g) != null) rblGender.SelectedValue = g;
        }

        private void BindAll()
        {
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(
                "SELECT FileNumber, FullName, Age, Country, Email, Gender, 'Select' AS Action FROM Users ORDER BY FileNumber", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        private DataTable GetUsersDataTable()
        {
            var dt = new DataTable();
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(
                "SELECT FileNumber, FullName, Age, Country, Email, Gender, 'Select' AS Action FROM Users ORDER BY FileNumber", con))
            {
                da.Fill(dt);
            }
            return dt;
        }

        private void ClearInputs()
        {
            txtFileNo.Text = txtFullName.Text = txtAge.Text = txtEmail.Text = txtPassword.Text = txtConfirmPassword.Text = string.Empty;
            ddlCountry.ClearSelection();
            rblGender.ClearSelection();
        }

        private void BindUsersDDL()
        {
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(
                "SELECT FileNumber, FullName FROM Users ORDER BY FullName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "FullName";
                ddlUsers.DataValueField = "FileNumber";
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, "-- Select Intern --");
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (ddlUsers.SelectedIndex <= 0)
            {
                rpUsers.DataSource = null;
                rpUsers.DataBind();
                return;
            }

            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(
                @"SELECT FileNumber, FullName, Age, Country, Email, Gender
                  FROM Users WHERE FileNumber=@id", con))
            {
                da.SelectCommand.Parameters.AddWithValue("@id", ddlUsers.SelectedValue);
                var dt = new DataTable();
                da.Fill(dt);
                rpUsers.DataSource = dt;
                rpUsers.DataBind();
            }
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            var dt = GetUsersDataTable();
            rpUsers.DataSource = dt;
            rpUsers.DataBind();
        }
    }
}