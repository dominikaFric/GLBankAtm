using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace projectATM
{
    class Database
    {
        private string server;
        private string uid;
        private string password;
        private string database;

        private static MySqlConnection connection;
     
        public Database()
        {
            server = "localhost";
            uid = "root";
            password = "";
            database = "glbank";
        }

        private  MySqlConnection openConnection()
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MySqlConnection c = new MySqlConnection(connectionString);
            c.Open();
            return c;
        }

        public bool doesCardExist(long idCard)
        {
            string query = "select * from cards where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                    return true;
                else
                    Console.WriteLine("Card doesn't exist.");
                r.Close();

                c.Close();
            }
            return false;
        }

        public bool isCardValid(long idCard)
        {

            string query = "select * from cards where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    string blocked = r["blocked"].ToString();
                    if (blocked == "N")
                        return true;
                    else
                        return false;
                }
                else
                    Console.WriteLine("Card is blocked.");
                r.Close();
                c.Close();
            }
            return false;

        }

        public bool isPinValid(long idCard,string pin)
        {

            string query = "select * from cards where cardnumber=" + idCard + " and pin=" + pin;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    return true;
                }
                else
                    r.Close();
                c.Close();
                return false;
            }
            return false;            
        }

        public int getWrongPinCount(long idCard)
        {
            string query = "select * from cards where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            int count=-1;
            
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                   count = r.GetInt32("wrongpinattempts");
                }
                else
                {
                    r.Close();
                }
            }
            c.Close();
            return count;
        }

        public void updateWrongPinCount(long idCard, int count)
        {
            string query = "update cards set wrongpinattempts="+count+" where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.ExecuteNonQuery();
                c.Close();
            }
        }

        public void blockCard(long idCard)
        {
            string query = "update cards set blocked=\"Y\" where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.ExecuteNonQuery();
                c.Close();
            }
        }

        public float getAccountBalance(long idCard)
        {
            string query = "select balance from cards inner join accounts on cards.idacc = accounts.idacc where cardnumber=" + idCard;
            float balance = 0;
            MySqlConnection c = openConnection();

            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    balance = r.GetFloat("balance");
                }
                else
                {
                    r.Close();
                }
            }
            c.Close();
            return balance;
        }

        public int updatePin(long idCard,int pin)
        {
            string query = "update cards set pin="+pin+" where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            int result = 0;
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                result = cmd.ExecuteNonQuery();
                c.Close();

            }
            return result;
        }

        public void withdrawCash(long idCard,float newBalance,float amount)
        {
            MySqlConnection c = openConnection();
            MySqlTransaction t = c.BeginTransaction();
            try
            {
                withdraw(idCard, newBalance,t);
                logWithdrawal(idCard, amount,t);
                t.Commit();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e);
                t.Rollback();
            }
            c.Close();
        }

        private int logWithdrawal(long idCard,float amount,MySqlTransaction t)
        {
            string query = "insert into atmwithdrawals(amount,idatm,cardnumber) values(" + amount + ",1," + idCard + ")";
            MySqlConnection c = openConnection();
            int result=0;
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.Transaction = t;
                result=cmd.ExecuteNonQuery();
                c.Close();

            }
            Console.WriteLine("log "+result);
            return result;
        }

        private void withdraw(long idCard,float newBalance, MySqlTransaction t)
        {
            string query = "update accounts inner join cards on cards.idacc = accounts.idacc set balance=" + newBalance + " where cardnumber=" + idCard;
            MySqlConnection c = openConnection();
            if (c != null)
            {
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.Transaction = t;
                cmd.ExecuteNonQuery();
                c.Close();

            }
        }


    }
}
