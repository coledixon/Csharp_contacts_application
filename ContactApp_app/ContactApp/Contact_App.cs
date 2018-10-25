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

        // INSTANTIATE CLASS(ES)
        contactData data = new contactData(); // data  class
        contactExt ext = new contactExt(); // method class
        contactProp prop = new contactProp(); // properties class

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
            bool isSuccess = data.Insert(prop);

            if (isSuccess)
            {
                MessageBox.Show("New contact record added for " + prop.FirstName + " " + prop.LastName);
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
            prop.FirstName = txtFname.Text;
            prop.LastName = txtLname.Text;
            prop.Address = txtAddress.Text;
            prop.City = txtCity.Text;
            prop.State = txtState.Text.ToUpper();
            prop.Zip = Convert.ToInt32(txtZip.Text);
            prop.PhoneHome = ext.parsePhoneNumber(txtAreaH.Text, txtPhoneH.Text);
            prop.PhoneCell = ext.parsePhoneNumber(txtAreaC.Text, txtPhoneC.Text);
            prop.PhoneWork = ext.parsePhoneNumber(txtAreaW.Text, txtPhoneW.Text);
            prop.EmailPersonal = ext.parseEmail(txtEmailP.Text);
            prop.EmailWork = ext.parseEmail(txtEmailW.Text);
            prop.Website = ext.parseWebsite(txtWebsite.Text, false);
            prop.Github = ext.parseWebsite(txtGitHub.Text, true);
        }

        private void txtContact_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (txtContact.ReadOnly) { btnAdd.Enabled = false; }
            else { btnAdd.Enabled = true; }
        }
    }
}
