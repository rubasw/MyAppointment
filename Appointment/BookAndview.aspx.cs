using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;

using WebListItem = System.Web.UI.WebControls.ListItem;

namespace ruba10828tl.Appointment
{
    public partial class BookAndview : System.Web.UI.Page
    {
        private readonly string _conn =
            ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDepartments();
                LoadDoctors();
                BindGrid();
            }
        }

        /* ========= Lists ========= */

        private void LoadDepartments()
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new WebListItem("--Select Department--", ""));
            ddlDepartment.Items.Add(new WebListItem("Cardiology", "Cardiology"));
            ddlDepartment.Items.Add(new WebListItem("Orthopedics", "Orthopedics"));
            ddlDepartment.Items.Add(new WebListItem("Dermatology", "Dermatology"));
            ddlDepartment.Items.Add(new WebListItem("Neurology", "Neurology"));
            ddlDepartment.Items.Add(new WebListItem("Pediatrics", "Pediatrics"));
            ddlDepartment.Items.Add(new WebListItem("ENT", "ENT"));
        }

        private void LoadDoctors()
        {
            ddlDoctor.Items.Clear();
            ddlDoctor.Items.Add(new WebListItem("--Select Doctor--", ""));
            ddlDoctor.Items.Add(new WebListItem("Dr. Smith", "Dr. Smith"));
            ddlDoctor.Items.Add(new WebListItem("Dr. Johnson", "Dr. Johnson"));
            ddlDoctor.Items.Add(new WebListItem("Dr. Brown", "Dr. Brown"));
            ddlDoctor.Items.Add(new WebListItem("Dr. Taylor", "Dr. Taylor"));
            ddlDoctor.Items.Add(new WebListItem("Dr. Wilson", "Dr. Wilson"));
        }

        /* ========= Data ========= */

        private DataTable GetAppointments()
        {
            using (var con = new SqlConnection(_conn))
            using (var da = new SqlDataAdapter(@"
                SELECT 
                    AppointmentID,
                    FileNumber,
                    Department,
                    Doctor,
                    CONVERT(varchar(10), [Date], 120) AS [Date],   -- yyyy-MM-dd
                    CONVERT(varchar(5),  [Time], 108) AS [Time],   -- HH:mm
                    History
                FROM Appointments
                ORDER BY AppointmentID DESC;", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void BindGrid()
        {
            gvAppointments.DataSource = GetAppointments();
            gvAppointments.DataBind();
        }

        /* ========= Helpers ========= */

        private string GetHistoryCsv()
        {
            var selected = chkHistory.Items.Cast<System.Web.UI.WebControls.ListItem>()
                                           .Where(i => i.Selected)
                                           .Select(i => i.Text);
            return string.Join(",", selected);
        }

        private void SetHistoryFromCsv(string csv)
        {
            foreach (System.Web.UI.WebControls.ListItem li in chkHistory.Items) li.Selected = false;
            if (string.IsNullOrWhiteSpace(csv)) return;

            foreach (var p in csv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => s.Trim()))
            {
                var it = chkHistory.Items.FindByText(p);
                if (it != null) it.Selected = true;
            }
        }

        private void SetDropDownSafe(DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            if (string.IsNullOrWhiteSpace(value)) return;

            var item = ddl.Items.FindByValue(value) ?? ddl.Items.FindByText(value);
            if (item == null)
            {
                ddl.Items.Insert(0, new WebListItem(value, value));
                ddl.SelectedIndex = 0;
            }
            else
            {
                item.Selected = true;
            }
        }

        private void ClearForm()
        {
            hfAppointmentID.Value = "";
            txtFileNumber.Text = "";
            ddlDepartment.ClearSelection();
            ddlDoctor.ClearSelection();
            txtDate.Text = "";
            txtTime.Text = "";
            foreach (System.Web.UI.WebControls.ListItem li in chkHistory.Items) li.Selected = false;
            lblMsg.Text = "";
        }

        private bool ValidateInputs(out DateTime d, out TimeSpan t)
        {
            d = default; t = default; lblMsg.Text = "";

            if (string.IsNullOrWhiteSpace(txtFileNumber.Text))
            { lblMsg.Text = "File Number is required."; return false; }

            if (string.IsNullOrEmpty(ddlDepartment.SelectedValue))
            { lblMsg.Text = "Please choose a Department."; return false; }

            if (string.IsNullOrEmpty(ddlDoctor.SelectedValue))
            { lblMsg.Text = "Please choose a Doctor."; return false; }

            if (!DateTime.TryParse(txtDate.Text, out d))
            { lblMsg.Text = "Invalid Date."; return false; }

            if (!TimeSpan.TryParse(txtTime.Text, out t))
            { lblMsg.Text = "Invalid Time (HH:mm)."; return false; }

            return true;
        }

        /* ========= CRUD ========= */

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var d, out var t)) return;

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                INSERT INTO Appointments (FileNumber, Department, Doctor, [Date], [Time], History)
                VALUES (@f, @dep, @doc, @date, @time, @hist);", con))
            {
                cmd.Parameters.AddWithValue("@f", txtFileNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@dep", ddlDepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@doc", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@date", d.Date);
                cmd.Parameters.AddWithValue("@time", t);
                cmd.Parameters.AddWithValue("@hist", GetHistoryCsv());

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Inserted.";
            BindGrid();
            ClearForm();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfAppointmentID.Value))
            { lblMsg.Text = "Select a row first."; return; }

            if (!ValidateInputs(out var d, out var t)) return;

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                UPDATE Appointments
                   SET FileNumber=@f,
                       Department=@dep,
                       Doctor=@doc,
                       [Date]=@date,
                       [Time]=@time,
                       History=@hist
                 WHERE AppointmentID=@id;", con))
            {
                cmd.Parameters.AddWithValue("@id", int.Parse(hfAppointmentID.Value));
                cmd.Parameters.AddWithValue("@f", txtFileNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@dep", ddlDepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@doc", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@date", d.Date);
                cmd.Parameters.AddWithValue("@time", t);
                cmd.Parameters.AddWithValue("@hist", GetHistoryCsv());

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = rows > 0 ? "Updated." : "Record not found.";
            }

            BindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfAppointmentID.Value))
            { lblMsg.Text = "Select a row first."; return; }

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(
                "DELETE FROM Appointments WHERE AppointmentID=@id;", con))
            {
                cmd.Parameters.AddWithValue("@id", int.Parse(hfAppointmentID.Value));
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = rows > 0 ? "Deleted." : "Record not found.";
            }

            BindGrid();
            ClearForm();
        }

        /* ========= Grid Select ========= */

        protected void gvAppointments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvAppointments.SelectedDataKey == null) return;

            var id = (int)gvAppointments.SelectedDataKey.Value;
            hfAppointmentID.Value = id.ToString();

            using (var con = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(@"
                SELECT AppointmentID, FileNumber, Department, Doctor, [Date], [Time], History
                FROM Appointments WHERE AppointmentID=@id;", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;

                    txtFileNumber.Text = r["FileNumber"].ToString();
                    SetDropDownSafe(ddlDepartment, r["Department"].ToString());
                    SetDropDownSafe(ddlDoctor, r["Doctor"].ToString());

                    if (DateTime.TryParse(r["Date"].ToString(), out var d))
                        txtDate.Text = d.ToString("yyyy-MM-dd");
                    else
                        txtDate.Text = string.Empty;

                    var tObj = r["Time"];
                    string timeText = "";
                    if (tObj is TimeSpan ts)
                        timeText = ts.ToString(@"hh\:mm");
                    else if (TimeSpan.TryParse(tObj.ToString(), out var ts2))
                        timeText = ts2.ToString(@"hh\:mm");
                    else if (DateTime.TryParse(tObj.ToString(), out var dt))
                        timeText = dt.ToString("HH:mm");
                    txtTime.Text = timeText;

                    SetHistoryFromCsv(r["History"] == DBNull.Value ? "" : r["History"].ToString());
                    lblMsg.Text = "";
                }
            }
        }

        /* ========= Export to PDF ========= */

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            var dt = GetAppointments();

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var title = new Paragraph("Appointments", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14));
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10f;
                doc.Add(title);

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;

                // Header
                foreach (DataColumn col in dt.Columns)
                {
                    var cell = new PdfPCell(new Phrase(col.ColumnName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Padding = 4f;
                    table.AddCell(cell);
                }

                // Rows
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        var txt = row[col] == DBNull.Value ? "" : row[col].ToString();
                        var cell = new PdfPCell(new Phrase(txt, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        cell.Padding = 3f;
                        table.AddCell(cell);
                    }
                }

                doc.Add(table);
                doc.Close();

                var bytes = ms.ToArray();
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=appointments.pdf");
                Response.BinaryWrite(bytes);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}