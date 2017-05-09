using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projectATM
{
    public partial class formATM : Form
    {

        private long id;

        public formATM(long id)
        {
            InitializeComponent();
            this.id = id;
            
        }

        private void formATM_Load(object sender, EventArgs e)
        {

        }
    }
}
