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
        // INSTANTIATE CLASS(ES)
        contactProp prop = new contactProp(); // properties class

        // data connection
        static string dataconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        // select data from db (overloaded)
        #region data select
        // dynamic lookup for contactid
        public DataTable Select(int contact_id = 0)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(dataconnstrng);
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

        // dynamic lookup by first / last name(s)
        public DataTable Select(string f_name, string l_name)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(dataconnstrng);
            try
            {
                var t = new Tuple<string, string>(f_name, l_name);

                string sql = "SELECT * FROM vcontact_data_all";
                sql = sql + t.Compare(f_name, l_name);

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
        public bool Insert(contactProp p)
        {
            bool isSuccess = false; // default to fail

            SqlConnection conn = new SqlConnection(dataconnstrng);
            try
            {
                string sql = "INSERT INTO vcontact_data_all (contact_id, first_name, last_name, address, city, state, zip, "
                    + "email_personal, email_work, phone_home, phone_cell, phone_work, website, github) "
                    + "VALUES (@contact_id, @first_name, @last_name, @address, @city, @state, @zip, "
                    + "@email_personal, @email_work, @phone_home, @phone_cell, @phone_work, @website, @github)";

                SqlCommand cmd = new SqlCommand(sql, conn); // build insert
                // add data values from screen to INSERT
                cmd.Parameters.AddWithValue("@contact_id", p.ContactId);
                cmd.Parameters.AddWithValue("@first_name", p.FirstName);
                cmd.Parameters.AddWithValue("@last_name", p.LastName);
                cmd.Parameters.AddWithValue("@address", p.Address);
                cmd.Parameters.AddWithValue("@city", p.City);
                cmd.Parameters.AddWithValue("@state", p.State);
                cmd.Parameters.AddWithValue("@zip", p.Zip);
                cmd.Parameters.AddWithValue("@email_personal", p.EmailPersonal);
                cmd.Parameters.AddWithValue("@email_work", p.EmailWork);
                cmd.Parameters.AddWithValue("@phone_home", p.PhoneHome);
                cmd.Parameters.AddWithValue("@phone_cell", p.PhoneCell);
                cmd.Parameters.AddWithValue("@phone_work", p.PhoneWork);
                cmd.Parameters.AddWithValue("@website", p.Website);
                cmd.Parameters.AddWithValue("@github", p.Github);

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
            return isSuccess; // return status
        }
        #endregion

        // update data in db
        #region data update
        public bool Update(contactProp p, int contact_id = 0)
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
                cmd.Parameters.AddWithValue("@first_name", p.FirstName);
                cmd.Parameters.AddWithValue("@last_name", p.LastName);
                cmd.Parameters.AddWithValue("@address", p.Address);
                cmd.Parameters.AddWithValue("@city", p.City);
                cmd.Parameters.AddWithValue("@state", p.State);
                cmd.Parameters.AddWithValue("@zip", p.Zip);
                cmd.Parameters.AddWithValue("@email_personal", p.EmailPersonal);
                cmd.Parameters.AddWithValue("@email_work", p.EmailWork);
                cmd.Parameters.AddWithValue("@phone_home", p.PhoneHome);
                cmd.Parameters.AddWithValue("@phone_cell", p.PhoneCell);
                cmd.Parameters.AddWithValue("@phone_work", p.PhoneWork);
                cmd.Parameters.AddWithValue("@website", p.Website);
                cmd.Parameters.AddWithValue("@github", p.Github);
                // where param
                cmd.Parameters.AddWithValue("@contact_id", p.ContactId);

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
        #region data delete
        public bool Delete(contactProp p, int contact_id = 0)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(dataconnstrng);

            try
            {
                if (contact_id == 0) { isSuccess = false; return false; }

                string sql = "DELETE FROM vcontact_data_all WHERE contact_id = @contact_id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                // where param
                cmd.Parameters.AddWithValue("@contact_id", p.ContactId);

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

        // fetch next contact_id val
        public int GetNextContactId()
        {
            int contact_id = 0; // default fail value
            int retval;
            string errmess;

            SqlConnection conn = new SqlConnection(dataconnstrng);
            try
            {
                string sql = "spgetNextContactId";

                SqlCommand cmd = new SqlCommand(sql, conn) { CommandType = CommandType.StoredProcedure };
                // set params as outputs
                cmd.Parameters.Add("@contact_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@retval", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@errmess", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                conn.Open(); // open db connection
                cmd.ExecuteNonQuery();

                // capture output vals
                contact_id = Convert.ToInt32(cmd.Parameters["@contact_id"].Value);
                retval = Convert.ToInt32(cmd.Parameters["@retval"].Value);
                errmess = cmd.Parameters["@contact_id"].Value.ToString();

                // alert error if SQL fails
                if (retval <= 0) { MessageBox.Show("ERROR RUNNING spgetNextContactId FROM GetNextContactId()"); }

                prop.ContactId = contact_id; // set prop

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close(); // close db connection
            }
            return contact_id;
        }
    }

    // TUPLE COMPARISON FOR Select() OVERLOAD
    static class CompareTuple
    {
        public static string Compare<T1, T2>(this Tuple<T1, T2> value, T1 v1, T2 v2)
        {
            string where = "";
            if (!string.IsNullOrEmpty(v1.ToString()) && string.IsNullOrEmpty(v2.ToString()))
            {
                return where = string.Format(" WHERE first_name = '{0}'", v1.ToString());
            }
            else if (string.IsNullOrEmpty(v1.ToString()) && !string.IsNullOrEmpty(v2.ToString()))
            {
                return where = string.Format(" WHERE last_name = '{0}'", v2.ToString());
            }
            else if (!string.IsNullOrEmpty(v1.ToString()) && !string.IsNullOrEmpty(v2.ToString()))
            {
                return where = string.Format(" WHERE first_name = '{0}' AND last_name = '{1}'", v1.ToString(), v2.ToString());
            }
            else { return where; }
        }
    }
}
