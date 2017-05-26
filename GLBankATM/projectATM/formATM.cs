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
            PIN_CHANGE_FAILED,
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
            DE
        }

        private long id;
        private States state;
        private Languages language;
        private Messages messages=new Messages();
        private string[] msgList;
        private String pin = "";
        private string amount;
        private int wrongPinAttemptCount;
        private float accBalance;

        public formATM(long id)
        {
            InitializeComponent();
            this.id = id;
            state = States.LANGUAGES;
            wrongPinAttemptCount = 0;
            printScreen();
            accBalance = db.getAccountBalance(Convert.ToInt64(Form1.cardNumber));
        }

        static Bitmap picture = new Bitmap(399, 264);
        Graphics g = Graphics.FromImage(picture);

        private void printScreen()
        {
            
            g.Clear(Color.Black);
            switch (state)
            {
                case States.LANGUAGES:

                    g.DrawString("Choose a language", new Font("Consolas", 14), Brushes.White, new Point(100, 30));
                    g.DrawString("SVK", new Font("Consolas", 14), Brushes.White,new Point(10,225));
                    g.DrawString("FIN", new Font("Consolas", 14), Brushes.White, new Point(10, 150));
                    g.DrawString("SWE", new Font("Consolas", 14), Brushes.White, new Point(340, 150));
                    g.DrawString("ENG", new Font("Consolas", 14), Brushes.White, new Point(340, 225));
                    g.DrawString("RUS", new Font("Consolas", 14), Brushes.White, new Point(10, 80));
                    g.DrawString("DEU", new Font("Consolas", 14), Brushes.White, new Point(340, 80));
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
                    g.DrawString(accBalance+ " €", new Font("Consolas", 14), Brushes.White, new Point(100, 150));
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

                case States.PIN_CHANGE_FAILED:
                    g.DrawString(msgList[8], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    ATMscreen.Image = picture;
                    break;

                case States.WITHDRAW_CASH:
                    g.DrawString(msgList[9], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                    ATMscreen.Image = picture;
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

        private void printAmount()
        {
            if (state == States.WITHDRAW_CASH)
            {
                x = 100;
                g.DrawString(msgList[9], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                g.DrawString(amount+ " €", new Font("Consolas", 14), Brushes.White, new Point(x, y));
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
                    if(state!=States.CHANGE_PIN)
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
                    Console.WriteLine(pin);
                    int newPin = 0;
                    if (Int32.TryParse(pin, out newPin))
                    {
                        int result=db.updatePin(Convert.ToInt64(Form1.cardNumber), newPin);
                        if (result == 1)
                        {
                            state = States.PIN_CHANGED;
                            printScreen();
                        }
                        else
                        {
                            state = States.PIN_CHANGE_FAILED;
                            printScreen();
                        }
                            
                    }
                }
            }
            if(state == States.WITHDRAW_CASH)
            {
                if (amount.All(char.IsDigit))
                {
                    int toWithdraw = 0;
                    if (Int32.TryParse(amount, out toWithdraw))
                    {
                        if (toWithdraw % 5 == 0)
                        {
                            float newBalance = accBalance - toWithdraw;
                            if (newBalance > 0)
                            {
                                Console.WriteLine("new balance:" + newBalance);
                                db.withdrawCash(Convert.ToInt64(Form1.cardNumber), newBalance,toWithdraw);
                                accBalance = newBalance;
                                g.Clear(Color.Black);
                                g.DrawString(msgList[12]+" "+newBalance, new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                                ATMscreen.Image = picture;
                            }
                            else
                            {
                                Console.WriteLine("low on money");
                                g.Clear(Color.Black);
                                g.DrawString(msgList[11], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                                ATMscreen.Image = picture;
                            }
                        }
                        else
                        {
                            Console.WriteLine("invalid amount");
                            g.Clear(Color.Black);
                            g.DrawString(msgList[10], new Font("Consolas", 14), Brushes.White, new Point(100, 100));
                            ATMscreen.Image = picture;
                        }
                        
                    }
                    Console.WriteLine("failed parsing");
                    
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
                case States.LANGUAGES:
                    this.language = Languages.FIN;
                    msgList = messages.getMessages("finnish");
                    checkCard();
                    break;
            }
        }

        private void btnRight3_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.LANGUAGES:
                    this.language = Languages.SWE;
                    msgList = messages.getMessages("swedish");
                    checkCard();
                    break;
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
                case States.LANGUAGES:
                    this.language = Languages.RU;
                    msgList = messages.getMessages("russian");
                    checkCard();
                    break;
            }
        }

        private void btnRight2_Click(object sender, EventArgs e)
        {
            switch (state)
            {
                case States.LANGUAGES:
                    this.language = Languages.DE;
                    msgList = messages.getMessages("german");
                    checkCard();
                    break;
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '1';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '2';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '3';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '4';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '5';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '6';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '7';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '8';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '9';
                printAmount();
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
            if (state == States.WITHDRAW_CASH)
            {
                amount = amount + '0';
                printAmount();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (state != States.LANGUAGES&&state!=States.PIN)
            {
                state = States.PIN_OK;
                amount = "";
                printScreen();
            }
           
        }
    }
}
