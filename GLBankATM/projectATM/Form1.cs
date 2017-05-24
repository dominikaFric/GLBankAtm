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
        Database db = new Database();
        public static string cardNumber;
        public Form1()
        {
            InitializeComponent();
            lblError.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            bool isCardValid = false;
            cardNumber = cardNoTxt.Text;
            long id;
            if (long.TryParse(cardNumber, out id))
            {
                if (db.doesCardExist(id))
                {
                    formATM formAtm = new formATM(id);
                    formAtm.Show();
                }
                else
                {

                    lblError.Visible = true;
                }
                   
            }



        }
    }
}
