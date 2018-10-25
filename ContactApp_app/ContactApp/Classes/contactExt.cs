using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactApp.Classes
{
    class contactExt
    {
        #region parsing methods
        // parse phone numbers
        public string parsePhoneNumber(string areaCode, string phoneNumber)
        {
            Regex _reg = new Regex("[-. ]"); // remove - and . chars
            string _parsed = "(" + areaCode + ")" + _reg.Replace(phoneNumber, ""); // wrap area code ()
            if (string.IsNullOrEmpty(phoneNumber)) { _parsed = ""; } // eliminate empty () on NULL
            return _parsed;
        }

        // parse website
        public string parseWebsite(string site, bool isGit)
        {
            // ensure proper website syntax (only validate https on github)
            string _parsed = (!site.ToLower().Contains("https://")) ? "https://" + site : site;
            _parsed = (!site.ToLower().Contains(".com") && !isGit) ? _parsed + ".com" : _parsed;
            return _parsed;
        }

        // parse email address(es)
        public string parseEmail(string email)
        {
            // ensure proper email syntax
            string _parsed = (!email.ToLower().Contains(".com")) ? email + ".com" : email;
            return _parsed;
        }
        #endregion
    }
}
