using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp.Classes
{
    class contactDirty
    {
        // track if form has changes
        public Form _formTrack { get; set; }
        public bool _isDirty { get; set; }

        public contactDirty(Form frm)
        {
            _formTrack = frm;
            checkDirtyState(frm.Controls);
        }

        // TRACK FORM CHANGES (DIRTY)
        private void checkDirtyState(Control.ControlCollection coll)
        {
            foreach (Control c in coll)
            {
                if (c is TextBox)
                    (c as TextBox).TextChanged += new EventHandler(DirtyState_TextChanged);
            }
        }

        private void DirtyState_TextChanged(object sender, EventArgs e)
        {
            SetDirty();
        }


        // SET DIRTY STATE(s)
        public void SetDirty()
        {
            _isDirty = true;
        }

        public void SetClean()
        {
            _isDirty = false;
        }
    }
}
