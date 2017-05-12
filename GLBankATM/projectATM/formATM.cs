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
        public enum States
        {
            LANGUAGES,
            PIN,
            PIN_OK,
            PIN_WRONG,
            CHOOSE_ACTION,
            WITHDRAW_CASH,
        }


        public enum Languages
        {
            EN,
            SK
        }

        private long id;
        private States state;
        private Languages language;
        private String pin = "";
        private int wrongPinAttemptCount = 0;

        public formATM(long id)
        {
            InitializeComponent();
            this.id = id;
            state = States.LANGUAGES;
            printScreen();
        }

        private void printScreen()
        {
            switch (state)
            {
                case States.LANGUAGES:

                    break;

            }

        }

        private void setState(States state)
        {
            this.state = state;
        }

        private States getState()
        {
            return state;
        }

        private void checkCard()
        {
            //TODO verification,switch to pin after the card is ok,db conn required

        }

        private bool checkPinCode()
        {
            bool isPinOk = pin.All(char.IsDigit);

            if (isPinOk)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void formATM_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length == 4)
            {
                if (checkPinCode() == true)
                    state = States.PIN_OK;
                else
                {
                    state = States.PIN;
                    wrongPinAttemptCount++;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft4_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.LANGUAGES:
                    this.language = Languages.SK;
                    checkCard();
                    break;
            }
        }

        private void btnRight4_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.LANGUAGES:
                    this.language = Languages.EN;
                    checkCard();
                    break;
            }

        }

        private void btnLeft3_Click(object sender, EventArgs e)
        {

        }

        private void btnRight3_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft2_Click(object sender, EventArgs e)
        {

        }

        private void btnRight2_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft1_Click(object sender, EventArgs e)
        {

        }

        private void btnRight1_Click(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '1';
            }
            else
            {
                pin = "";
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '2';
            }
            else
            {
                pin = "";
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '3';
            }
            else
            {
                pin = "";
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '4';
            }
            else
            {
                pin = "";
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '5';
            }
            else
            {
                pin = "";
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '6';
            }
            else
            {
                pin = "";
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '7';
            }
            else
            {
                pin = "";
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '8';
            }
            else
            {
                pin = "";
            }
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '9';
            }
            else
            {
                pin = "";
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if (state == States.PIN && pin.Length < 4)
            {
                pin = pin + '0';
            }
            else
            {
                pin = "";
            }
        }
    }
}
