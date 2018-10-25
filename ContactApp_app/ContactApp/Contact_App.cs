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

        // INSTANTIATE CLASSES
        contactData data = new contactData(); // data class
        contactExt ext = new contactExt(); // method class

        // FORM EVENTS
        #region button events
        private void btnNext_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in this.Controls) // clear all text elements on gen'in new contactid
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = String.Empty;
            }

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
            DialogResult _res = MessageBox.Show("Do you want to clear the form?", "CLEAR", MessageBoxButtons.YesNo);
            if (_res == DialogResult.Yes)
            {
                foreach (Control ctl in this.Controls) // clear all text elements on gen'in new contactid
                {
                    if (ctl.GetType() == typeof(TextBox))
                        ((TextBox)ctl).Text = String.Empty;
                }
                txtContact.ReadOnly = false; // reset readonly
            }
            else { return; }
        }
        #endregion

        #region focus events
        private void txtContact_LostFocus(object sender, EventArgs e)
        {
            // fire Select() txtContact.text
            if (txtContact.Text.Length > 0)
            {
                data.Select();
            }
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
            data.PhoneHome = ext.parsePhoneNumber(txtAreaH.Text, txtPhoneH.Text);
            data.PhoneCell = ext.parsePhoneNumber(txtAreaC.Text, txtPhoneC.Text);
            data.PhoneWork = ext.parsePhoneNumber(txtAreaW.Text, txtPhoneW.Text);
            data.EmailPersonal = ext.parseEmail(txtEmailP.Text);
            data.EmailWork = ext.parseEmail(txtEmailW.Text);
            data.Website = ext.parseWebsite(txtWebsite.Text, false);
            data.Github = ext.parseWebsite(txtGitHub.Text, true);
        }

        private void txtContact_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (txtContact.ReadOnly) { btnAdd.Enabled = false; }
            else { btnAdd.Enabled = true; }
        }
    }
}
