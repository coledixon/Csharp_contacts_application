using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Classes
{
    class contactData
    {
        // data connection
        static string dataconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;


        // getters / setters (associated to tables in schema)
        #region GetSet
        // contact_main
        public int ContactId { get; set; };
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // contact_address
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }

        // contact_email
        public string EmailPersonal { get; set; }
        public string EmailWork { get; set; }

        // contact_phone
        public string PhoneHome { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneWork { get; set; }

        // contact_website
        public string Website { get; set; }
        public string Github { get; set; }
        #endregion

        // select data from db
        #region data
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(dataconnstrng);

            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT * FROM vcontact_data_all";
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

        }
        #endregion

    }
}
