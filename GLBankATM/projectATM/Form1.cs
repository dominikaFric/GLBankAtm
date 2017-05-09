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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isCardValid = false;
            String cardNumber = cardNoTxt.Text;
            long id;
            if (long.TryParse(cardNumber, out id))
            {
                formATM formAtm = new formATM(id);
                formAtm.Show();
            }



        }
    }
}
