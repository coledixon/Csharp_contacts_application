using ContactApp.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp
{
    public partial class Contact_App : Form
    {
        public Contact_App()
        {
            InitializeComponent();
        }

        // instantiate class(es)
        contactData data = new contactData();

        #region button events
        private void btnNext_Click(object sender, EventArgs e)
        {
            txtContact.Text = data.GetNextContactId().ToString();
            txtContact.ReadOnly = true; // set to readonly
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetProps(); // enaure all class values are set
            bool isSuccess = data.Insert(data);

            if (isSuccess)
            {
                MessageBox.Show("New contact record added for " + data.FirstName + " " + data.LastName);
                txtContact.ReadOnly = false;
            }
            else { MessageBox.Show("Error saving record to db_contacts"); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Update()
            txtContact.ReadOnly = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete()
            txtContact.ReadOnly = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // build form clear functionality
            txtContact.ReadOnly = false;
        }
        #endregion

        #region focus events
        private void txtContact_LostFocus(object sender, EventArgs e)
        {
            // fire Select() txtContact.text
        }

        private void txtFname_LostFocus(object sender, EventArgs e)
        {
            // dynamic lookup by first name (last name required)
        }

        private void txtLname_LostFocus(object sender, EventArgs e)
        {
            // dynamic lookup by last name (first name required)
        }

        // testing mouse hover for ToolTip functionality
        private void btnAdd_MouseHover(object sender, EventArgs e)
        {
            // ToolTip ??
        }
        #endregion

        // set properties
        private void SetProps()
        {
            data.FirstName = txtFname.Text;
            data.LastName = txtLname.Text;
            data.Address = txtAddress.Text;
            data.City = txtCity.Text;
            data.State = txtState.Text.ToUpper();
            data.Zip = Convert.ToInt32(txtZip.Text);
            data.PhoneHome = parsePhoneNumber(txtAreaH.Text, txtPhoneH.Text);
            data.PhoneCell = parsePhoneNumber(txtAreaC.Text, txtPhoneC.Text);
            data.PhoneWork = parsePhoneNumber(txtAreaW.Text, txtPhoneW.Text);
            data.EmailPersonal = parseEmail(txtEmailP.Text);
            data.EmailWork = parseEmail(txtEmailW.Text);
            data.Website = parseWebsite(txtWebsite.Text, false);
            data.Github = parseWebsite(txtGitHub.Text, true);
        }

        #region parsing methods
        // parse phone number
        private string parsePhoneNumber(string areaCode, string phoneNumber)
        {
            Regex _reg = new Regex("[-. ]"); // remove - and . chars
            string _parsed = "(" + areaCode + ")" + _reg.Replace(phoneNumber, "");
            if (string.IsNullOrEmpty(phoneNumber)) { _parsed = null; } // eliminate empty () on parse
            return _parsed;
        }

        // parse website
        private string parseWebsite(string site, bool isGit)
        {
            // ensure proper website syntax (only validate https on github)
            string _parsed = (!site.ToLower().Contains("https://")) ? "https://" + site : site;
            _parsed = (!site.ToLower().Contains(".com") && !isGit) ? _parsed + ".com" : _parsed;
            return _parsed;
        }

        private string parseEmail(string email)
        {
            // ensure proper email syntax
            string _parsed = (!email.ToLower().Contains(".com")) ? email + ".com" : email;
            return _parsed;
        }
        #endregion

    }
}
