using System;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class BookAppointment : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDoctors();
            LoadHospitals();
        }
    }

    private void LoadDoctors()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT DoctorID, FullName FROM Doctors ORDER BY FullName";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            var reader = cmd.ExecuteReader();
            ddlDoctors.DataSource = reader;
            ddlDoctors.DataTextField = "FullName";
            ddlDoctors.DataValueField = "DoctorID";
            ddlDoctors.DataBind();
            con.Close();
        }
        ddlDoctors.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- اختر طبيب --", "0"));
    }

    private void LoadHospitals()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "SELECT HospitalID, HospitalName FROM Hospitals ORDER BY HospitalName";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            var reader = cmd.ExecuteReader();
            ddlHospitals.DataSource = reader;
            ddlHospitals.DataTextField = "HospitalName";
            ddlHospitals.DataValueField = "HospitalID";
            ddlHospitals.DataBind();
            con.Close();
        }
        ddlHospitals.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- اختر مستشفى --", "0"));
    }

    protected void btnBook_Click(object sender, EventArgs e)
    {
        lblMessage.ForeColor = System.Drawing.Color.Red;

        if (ddlDoctors.SelectedValue == "0")
        {
            lblMessage.Text = "يرجى اختيار الطبيب.";
            return;
        }

        if (ddlHospitals.SelectedValue == "0")
        {
            lblMessage.Text = "يرجى اختيار المستشفى.";
            return;
        }

        DateTime appointmentDate;
        if (!DateTime.TryParse(txtAppointmentDate.Text, out appointmentDate))
        {
            lblMessage.Text = "يرجى إدخال تاريخ ووقت صحيح للموعد.";
            return;
        }

        if (appointmentDate < DateTime.Now)
        {
            lblMessage.Text = "لا يمكن حجز موعد بتاريخ سابق.";
            return;
        }

        int patientID = Convert.ToInt32(Session["PatientID"]);
        bool notifications = chkNotifications.Checked;

        // التحقق من وجود المريض
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) FROM Patients WHERE PatientID=@PatientID", con);
            cmdCheck.Parameters.AddWithValue("@PatientID", patientID);
            int exists = (int)cmdCheck.ExecuteScalar();
            if (exists == 0)
            {
                lblMessage.Text = "المريض غير موجود في النظام. يرجى تسجيل المريض أولاً.";
                return;
            }
        }

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO Appointments (PatientID, DoctorID, HospitalID, AppointmentDate, Notifications)
                             VALUES (@PatientID, @DoctorID, @HospitalID, @AppointmentDate, @Notifications)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@PatientID", patientID);
            cmd.Parameters.AddWithValue("@DoctorID", int.Parse(ddlDoctors.SelectedValue));
            cmd.Parameters.AddWithValue("@HospitalID", int.Parse(ddlHospitals.SelectedValue));
            cmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
            cmd.Parameters.AddWithValue("@Notifications", notifications);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // إنشاء PDF تأكيد الحجز
        string pdfPath = Server.MapPath("~/AppointmentPDFs/") + $"Appointment_{DateTime.Now.Ticks}.pdf";
        Directory.CreateDirectory(Server.MapPath("~/AppointmentPDFs/"));
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
        doc.Open();
        doc.Add(new Paragraph("تأكيد موعد"));
        doc.Add(new Paragraph($"المريض: {Session["FullName"]}"));
        doc.Add(new Paragraph($"الطبيب: {ddlDoctors.SelectedItem.Text}"));
        doc.Add(new Paragraph($"المستشفى: {ddlHospitals.SelectedItem.Text}"));
        doc.Add(new Paragraph($"تاريخ ووقت الموعد: {appointmentDate:yyyy-MM-dd HH:mm}"));
        doc.Close();

        // إرسال البريد عبر Outlook/Office 365
       
        lblMessage.ForeColor = System.Drawing.Color.Green;
        lblMessage.Text = "تم حجز الموعد بنجاح. تم إرسال التأكيد بالبريد الإلكتروني.";

        ddlDoctors.SelectedIndex = 0;
        ddlHospitals.SelectedIndex = 0;
        txtAppointmentDate.Text = "";
        chkNotifications.Checked = false;
    }
}