using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp.Classes
{
    class contactExt
    {
        #region parsing methods
        // parse phone numbers (overloads)
        public string parsePhoneNumber(string areaCode, string phoneNumber) // add/update data
        {
                Regex _reg = new Regex("[-. ]"); // remove - and . chars

                string _parsed = "(" + areaCode + ")" + _reg.Replace(phoneNumber, ""); // wrap area code ()
                if (string.IsNullOrEmpty(phoneNumber)) { _parsed = ""; } // eliminate empty () on NULL
                return _parsed;
        }

        public void parsePhoneNumber(string phoneNumber, out string area, out string phone) // select data
        {
            area = (!string.IsNullOrEmpty(phoneNumber)) ? phoneNumber.Substring(1, 3) : "";
            phone = (!string.IsNullOrEmpty(phoneNumber)) ? phoneNumber.Substring(5, 7) : "";
        }

        // parse website
        public string parseWebsite(string site, bool isGit)
        {
            bool isNull = string.IsNullOrEmpty(site);

            // ensure proper website syntax (only validate https on github)
            string _parsed = (!site.ToLower().Contains("https://") && !isNull) ? "https://" + site : site;
            _parsed = (!site.ToLower().Contains(".com") && !isGit && !isNull) ? _parsed + ".com" : _parsed;
            return _parsed;
        }

        // parse email address(es)
        public string parseEmail(string email)
        {
            bool isNull = string.IsNullOrEmpty(email);

            // ensure proper email syntax
            string _parsed = (!email.ToLower().Contains(".com") && !isNull) ? email + ".com" : email;
            return _parsed;
        }
        #endregion
    }
}
