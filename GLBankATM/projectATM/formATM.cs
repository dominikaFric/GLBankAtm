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
        Database db = new Database();

        public enum States
        {
            LANGUAGES,
            CARD_INVALID,
            PIN,
            PIN_OK,
            PIN_WRONG,
            PIN_CHANGED,
            VIEW_BALANCE,
            WITHDRAW_CASH,
            CHANGE_PIN
        }


        public enum Languages
        {
            EN,
            SK,
            FIN,
            SWE,
            RU,
            ESP,
            DE,
            FR//incoming feature
        }

        private long id;
        private States state;
        private Languages language;
        private Messages messages=new Messages();
        private string[] msgList;
        private String pin = "";
        private int wrongPinAttemptCount;

        public formATM(long id)
        {
            InitializeComponent();
            this.id = id;
            state = States.LANGUAGES;
            wrongPinAttemptCount = 0;
            printScreen();
            
        }

        static Bitmap picture = new Bitmap(399, 264);
        Graphics g = Graphics.FromImage(picture);

        private void printScreen()
        {
            
            g.Clear(Color.Black);
            switch (state)
            {
                case States.LANGUAGES:

                    g.DrawString("Choose a language", new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    g.DrawString("SLOVAK", new Font("Consolas", 14), Brushes.White,new Point(10,225));
                    g.DrawString("ENGLISH", new Font("Consolas", 14), Brushes.White, new Point(300, 225));
                    ATMscreen.Image = picture;

                    break;

                case States.CARD_INVALID:
                        g.DrawString(msgList[0], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                        ATMscreen.Image = picture;
                    break;

                case States.PIN:
                        g.DrawString(msgList[1], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                        ATMscreen.Image = picture;
                    break;

                case States.PIN_OK:
                        g.DrawString(msgList[2], new Font("Consolas", 14), Brushes.White, new Point(10, 10));
                        g.DrawString(msgList[4], new Font("Consolas", 14), Brushes.White, new Point(10, 80));
                        g.DrawString(msgList[3], new Font("Consolas", 14), Brushes.White, new Point(10, 150));
                        ATMscreen.Image = picture;
                    break;

                case States.PIN_WRONG:
                    g.DrawString(msgList[6], new Font("Consolas", 14), Brushes.White, new Point(20, 100));
                    ATMscreen.Image = picture;
                    break;

                case States.VIEW_BALANCE:
                    g.DrawString(msgList[3], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    g.DrawString(db.getAccountBalance(Convert.ToInt64(Form1.cardNumber))+ " €", new Font("Consolas", 14), Brushes.White, new Point(100, 150));
                    ATMscreen.Image = picture;
                    break;

                case States.CHANGE_PIN:
                    g.DrawString(msgList[4], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    ATMscreen.Image = picture;
                    break;

                case States.PIN_CHANGED:
                    g.DrawString(msgList[7], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    ATMscreen.Image = picture;
                    break;

                case States.WITHDRAW_CASH:
                    break;
               
            }

        }

        int x = 100;
        int y = 225;
        private void printPinCode()
        {
            if (state == States.PIN)
            {
                g.DrawString(msgList[1], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                g.DrawString("*", new Font("Consolas", 14), Brushes.White, new Point(x, y));
                x = x + 20;
                ATMscreen.Image = picture;
            }
            if (state == States.CHANGE_PIN)
            {
                g.DrawString(msgList[4], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                g.DrawString("*", new Font("Consolas", 14), Brushes.White, new Point(x, y));
                x = x + 20;
                ATMscreen.Image = picture;
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
            if (db.isCardValid(Convert.ToInt64(Form1.cardNumber)))
            {
                state = States.PIN;
                printScreen();
            }
            else
            {
                state = States.CARD_INVALID;
                printScreen();
            }
        }

        private bool checkPinCode()
        {
            bool isPinOk = pin.All(char.IsDigit);
            bool isPinCorrect = db.isPinValid(Convert.ToInt64(Form1.cardNumber), pin);

            if (isPinOk && isPinCorrect)
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
            wrongPinAttemptCount = db.getWrongPinCount(Convert.ToInt64(Form1.cardNumber));
            if (state == States.PIN && pin.Length == 4 && wrongPinAttemptCount<3)
            {
                if (checkPinCode() == true)
                {
                    state = States.PIN_OK;
                    wrongPinAttemptCount = 0;
                    db.updateWrongPinCount(Convert.ToInt64(Form1.cardNumber), wrongPinAttemptCount);
                    printScreen();
                }
                else
                {
                    state = States.PIN;
                    printScreen();
                    pin = "";
                    x = 100;
                    wrongPinAttemptCount++;
                    db.updateWrongPinCount(Convert.ToInt64(Form1.cardNumber), wrongPinAttemptCount);
                }
            }
            if (wrongPinAttemptCount == 3)
            {
                state = States.PIN_WRONG;
                db.blockCard(Convert.ToInt64(Form1.cardNumber));
                printScreen();
            }
            if (state == States.CHANGE_PIN)
            {
                if (pin.All(char.IsDigit))
                {
                    db.updatePin(Convert.ToInt64(Form1.cardNumber),pin);
                    state = States.PIN_CHANGED;
                    printScreen();
                }
              
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        //textless buttons
        private void btnLeft4_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.LANGUAGES:
                    this.language = Languages.SK;
                    msgList = messages.getMessages("slovak");
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
                    msgList = messages.getMessages("english");
                    checkCard();
                    break;
            }

        }

        private void btnLeft3_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.PIN_OK:
                    state = States.VIEW_BALANCE;
                    printScreen();
                    break;
            }
        }

        private void btnRight3_Click(object sender, EventArgs e)
        {
            switch (state)
            {

            }
        }

        private void btnLeft2_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.PIN_OK:
                    state = States.CHANGE_PIN;
                    pin = "";
                    x = 100;
                    printScreen();
                    break;
            }
        }

        private void btnRight2_Click(object sender, EventArgs e)
        {
            switch (state)
            {

            }
        }

        private void btnLeft1_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.PIN_OK:
                    state = States.WITHDRAW_CASH;
                    printScreen();
                    break;
            }
        }

        private void btnRight1_Click(object sender, EventArgs e)
        {
            switch (state)
            {

            }
        }
    
        //keypad
        private void btn1_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN ||state==States.CHANGE_PIN ) && pin.Length < 4)
            {
                pin = pin + '1';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '2';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '3';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '4';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '5';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '6';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '7';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '8';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '9';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if ((state == States.PIN || state == States.CHANGE_PIN) && pin.Length < 4)
            {
                pin = pin + '0';
                printPinCode();
            }
            else
            {
                pin = "";
                printScreen();
                x = 100;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            state = States.PIN_OK;
            printScreen();
        }
    }
}
