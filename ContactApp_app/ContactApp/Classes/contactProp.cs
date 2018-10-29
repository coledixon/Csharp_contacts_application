using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp.Classes
{
    class contactProp
    {
        // column names not same as props (schema includes _)
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
    }
}
