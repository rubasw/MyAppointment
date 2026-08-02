using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class AppointmentList : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["ruba10828tlConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindAppointments();
    }

    private void BindAppointments()
    {
        int patientID = Convert.ToInt32(Session["PatientID"]);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = @"
                SELECT a.AppointmentID,
                       p.FullName AS PatientName,
                       d.FullName AS DoctorName,
                       h.HospitalName,
                       a.AppointmentDate
                FROM Appointments a
                INNER JOIN Patients p ON a.PatientID = p.PatientID
                INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
                INNER JOIN Hospitals h ON a.HospitalID = h.HospitalID
                WHERE a.PatientID=@PatientID
                ORDER BY a.AppointmentDate";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.SelectCommand.Parameters.AddWithValue("@PatientID", patientID);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvAppointments.DataSource = dt;
            gvAppointments.DataBind();
        }
    }

    protected void gvAppointments_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        gvAppointments.EditIndex = e.NewEditIndex;
        BindAppointments();
    }

    protected void gvAppointments_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        int appointmentID = Convert.ToInt32(gvAppointments.DataKeys[e.RowIndex].Value);
        string newDate = ((System.Web.UI.WebControls.TextBox)gvAppointments.Rows[e.RowIndex].Cells[4].Controls[0]).Text;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "UPDATE Appointments SET AppointmentDate=@Date WHERE AppointmentID=@ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Date", newDate);
            cmd.Parameters.AddWithValue("@ID", appointmentID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        gvAppointments.EditIndex = -1;
        BindAppointments();
    }

    protected void gvAppointments_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        int appointmentID = Convert.ToInt32(gvAppointments.DataKeys[e.RowIndex].Value);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Appointments WHERE AppointmentID=@ID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ID", appointmentID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        BindAppointments();
    }

    protected void gvAppointments_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        gvAppointments.EditIndex = -1;
        BindAppointments();
    }
}