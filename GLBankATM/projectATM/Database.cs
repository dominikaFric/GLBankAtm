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
            }
            return false;

        }


    }
}
