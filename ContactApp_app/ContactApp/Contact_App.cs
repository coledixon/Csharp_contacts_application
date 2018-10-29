﻿using ContactApp.Classes;
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
            SetProps(); // ensure all class properties are set
            bool isSuccess = data.Insert(prop);

            if (isSuccess) { MessageBox.Show("New contact record added for " + prop.FirstName + " " + prop.LastName, "ADD"); Clear(); }
            else { MessageBox.Show("ERROR SAVING RECORD TO db_contacts", "ERROR"); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SetProps(); // ensure all class propeties are set
            bool isSuccess = data.Update(prop, Convert.ToInt32(txtContact.Text));

            if (isSuccess) { MessageBox.Show("Record updated for " + prop.FirstName + " " + prop.LastName, "UPDATE"); Clear(); }
            else { MessageBox.Show("ERROR SAVING RECORD TO db_contacts", "ERROR"); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete()
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // form clear functionality
            DialogResult _res = MessageBox.Show("Do you want to clear the form?", "CLEAR", MessageBoxButtons.YesNo);

            if (_res == DialogResult.Yes) { Clear(); }
            else { return; }
        }
        #endregion

        #region focus events
        private void txtContact_LostFocus(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtContact.Text, @"[a-zA-Z]+$")) { MessageBox.Show("Contact Id entered is not numeric.", "ERROR"); Clear(); return; }

            // fire Select() txtContact.text
            if (txtContact.Text.Length > 0 && !txtContact.ReadOnly)
            {
                DataTable dt = new DataTable();
                dt = data.Select(Convert.ToInt32(txtContact.Text));
                SetReturnValues(dt);
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

        private void txtContact_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (txtContact.ReadOnly) { btnNext.Enabled = false; }
            else { btnNext.Enabled = true; }
        }

        // FORM METHODS
        // set properties
        private void SetProps()
        {
            prop.ContactId = Convert.ToInt32(txtContact.Text);
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

        // set form select() values
        public void SetReturnValues(DataTable dt)
        {
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string area; string phone;
                    // strongly typed values
                    txtFname.Text = dr["first_name"].ToString();
                    txtLname.Text = dr["last_name"].ToString();
                    txtAddress.Text = dr["address"].ToString();
                    txtCity.Text = dr["city"].ToString();
                    txtState.Text = dr["state"].ToString();
                    txtZip.Text = dr["zip"].ToString();
                    // reparse phone numbers to fit form fields
                    ext.parsePhoneNumber(dr["phone_home"].ToString(), out area, out phone);
                    txtAreaH.Text = area;
                    txtPhoneH.Text = phone;
                    ext.parsePhoneNumber(dr["phone_cell"].ToString(), out area, out phone);
                    txtAreaC.Text = area;
                    txtPhoneC.Text = phone;
                    ext.parsePhoneNumber(dr["phone_work"].ToString(), out area, out phone);
                    txtAreaW.Text = area;
                    txtPhoneW.Text = phone;
                    // end reparse
                    txtEmailP.Text = dr["email_personal"].ToString();
                    txtEmailW.Text = dr["email_work"].ToString();
                    txtWebsite.Text = dr["website"].ToString();
                    txtGitHub.Text = dr["github"].ToString();
                    txtContact.ReadOnly = true; // disable contactid field
                }
            }
            else if (dt.Rows.Count > 1) { MessageBox.Show("contactData.Select() returned more than 1 row from database", "ERROR"); Clear(); }
            else { MessageBox.Show("Contact Id does not exist.", "ERROR"); Clear(); }
        }

        private void Clear()
        {
            foreach (Control ctl in this.Controls) // clear all text elements on gen'in new contactid
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = String.Empty;
            }
            txtContact.ReadOnly = false; // reset readonly
        }

    }
}
