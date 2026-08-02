using ruba10828tl.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ruba10828tl.demo
{
    public partial class myControls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            populateRblPatients();
        }
        protected void populateRblPatients()
        {
            CRUD myCrud = new CRUD();
            string mySql = "select gender from Patients";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            RblPatients.DataTextField = "gender";
            RblPatients.DataSource = dr;
            RblPatients.DataBind();
        }
      
      protected void RblPatients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}