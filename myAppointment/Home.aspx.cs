using System;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["FullName"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        lblWelcome.Text = Session["FullName"].ToString();
    }

    protected void btnBookAppointment_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookAppointment.aspx");
    }

    protected void btnMyAppointments_Click(object sender, EventArgs e)
    {
        Response.Redirect("AppointmentList.aspx");
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }
}