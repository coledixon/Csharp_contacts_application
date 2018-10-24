using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp.Classes
{
    class contactData
    {
        // data connection
        static string dataconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;


        // getters / setters (associated to tables in schema)
            // column names not same as GET/SET props (schema includes _)
        #region get/set
        // contact_main
        public int ContactId { get; set; }
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
        #region data select
        public DataTable Select(int contact_id = 0)
        {
            SqlConnection conn = new SqlConnection(dataconnstrng);

            DataTable dt = new DataTable();

            try
            {
                string sql = "SELECT * FROM vcontact_data_all";
                if (contact_id > 0) { sql = sql + string.Format(" WHERE contact_id = {0}", contact_id); }

                SqlCommand cmd = new SqlCommand(sql, conn); // build query
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                conn.Open(); // open db connection
                adp.Fill(dt); // populate virtual table
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close(); // close db connection
            }
            return dt; // return datatable

        }
        #endregion

        // insert data into db
        #region data insert
        public bool Insert(contactData c)
        {
            bool isSuccess = false; // default to fail

            SqlConnection conn = new SqlConnection(dataconnstrng);
            try
            {
                string sql = "INSERT INTO vcontact_data_all "
                    + "VALUES (@first_name, @last_name, @address, @city, @state, @zip, "
                    + "@email_personal, @email_work, @phone_home, @phone_cell, @phone_work, @website, @github)";

                SqlCommand cmd = new SqlCommand(sql, conn); // build insert
                // add data values from screen to INSERT
                cmd.Parameters.AddWithValue("@first_name", c.FirstName);
                cmd.Parameters.AddWithValue("@last_name", c.LastName);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.Parameters.AddWithValue("@city", c.City);
                cmd.Parameters.AddWithValue("@state", c.State);
                cmd.Parameters.AddWithValue("@zip", c.Zip);
                cmd.Parameters.AddWithValue("@email_personal", c.EmailPersonal);
                cmd.Parameters.AddWithValue("@email_work", c.EmailWork);
                cmd.Parameters.AddWithValue("@phone_home", c.PhoneHome);
                cmd.Parameters.AddWithValue("@phone_cell", c.PhoneCell);
                cmd.Parameters.AddWithValue("@phone_work", c.PhoneWork);
                cmd.Parameters.AddWithValue("@website", c.Website);
                cmd.Parameters.AddWithValue("@github", c.Github);

                conn.Open(); // open db connection

                int rows = cmd.ExecuteNonQuery();
                isSuccess = (rows > 0) ? true : false; // similar logic to @@ROWCOUNT in sql

                if (isSuccess)
                {
                    Select();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close(); // close db connection
            }
            return isSuccess; // return status
        }
        #endregion

        // update data in db
        #region data update
        public bool Update(contactData c, int contact_id = 0)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(dataconnstrng);

            try
            {
                if (contact_id == 0) { isSuccess = false; return false; }

                string sql = "UPDATE vcontact_data_all "
                    + "SET first_name = @first_name, last_name = @last_name, address = @address, city = @city, "
                    + "state = @state, zip = @zip, email_personal = @email_personal, email_work = @email_work, "
                    + "phone_home = @phone_home, phone_cell = @phone_cell, phone_work = @phone_work, website = @website, github = @github "
                    + "WHERE contact_id = @contact_id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                // add data values from screen to UPDATE
                cmd.Parameters.AddWithValue("@first_name", c.FirstName);
                cmd.Parameters.AddWithValue("@last_name", c.LastName);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.Parameters.AddWithValue("@city", c.City);
                cmd.Parameters.AddWithValue("@state", c.State);
                cmd.Parameters.AddWithValue("@zip", c.Zip);
                cmd.Parameters.AddWithValue("@email_personal", c.EmailPersonal);
                cmd.Parameters.AddWithValue("@email_work", c.EmailWork);
                cmd.Parameters.AddWithValue("@phone_home", c.PhoneHome);
                cmd.Parameters.AddWithValue("@phone_cell", c.PhoneCell);
                cmd.Parameters.AddWithValue("@phone_work", c.PhoneWork);
                cmd.Parameters.AddWithValue("@website", c.Website);
                cmd.Parameters.AddWithValue("@github", c.Github);
                // where param
                cmd.Parameters.AddWithValue("@contact_id", c.ContactId);

                conn.Open(); // open db connection

                int rows = cmd.ExecuteNonQuery();
                isSuccess = (rows > 0) ? true : false; // similar logic to @@ROWCOUNT in sql
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close(); // close db connection
            }
            return isSuccess;
        }
        #endregion

        // delete data from db
        #region data update
        public bool Delete(contactData c, string where)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(dataconnstrng);

            try
            {
                if (string.IsNullOrEmpty(where)) { isSuccess = false; return false; }

                string sql = "UPDATE vcontact_data_all "
                    + "SET first_name = @first_name, last_name = @last_name, address = @address, city = @city, "
                    + "state = @state, zip = @zip, email_personal = @email_personal, email_work = @email_work, "
                    + "phone_home = @phone_home, phone_cell = @phone_cell, phone_work = @phone_work, website = @website, github = @github "
                    + "WHERE contact_id = @contact_id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                // add data values from screen to UPDATE
                cmd.Parameters.AddWithValue("@first_name", c.FirstName);
                cmd.Parameters.AddWithValue("@last_name", c.LastName);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.Parameters.AddWithValue("@city", c.City);
                cmd.Parameters.AddWithValue("@state", c.State);
                cmd.Parameters.AddWithValue("@zip", c.Zip);
                cmd.Parameters.AddWithValue("@email_personal", c.EmailPersonal);
                cmd.Parameters.AddWithValue("@email_work", c.EmailWork);
                cmd.Parameters.AddWithValue("@phone_home", c.PhoneHome);
                cmd.Parameters.AddWithValue("@phone_cell", c.PhoneCell);
                cmd.Parameters.AddWithValue("@phone_work", c.PhoneWork);
                cmd.Parameters.AddWithValue("@website", c.Website);
                cmd.Parameters.AddWithValue("@github", c.Github);
                // where param
                cmd.Parameters.AddWithValue("@contact_id", c.ContactId);

                conn.Open(); // open db connection

                int rows = cmd.ExecuteNonQuery();
                isSuccess = (rows > 0) ? true : false; // similar logic to @@ROWCOUNT in sql
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close(); // close db connection
            }
            return isSuccess;
        }
        #endregion
    }
}
