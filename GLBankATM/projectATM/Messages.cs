using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectATM
{
    class Messages
    {
        private string[] messagesEN = {"Ooops. Card blocked","Enter PIN code","Withdraw Cash",
                                        "Account Balance","Change PIN","Wrong attempt. Enter PIN again",
                                        "3 failed attempts. Card blocked.","Pin changed successfully" };

        private string[] messagesSK = { "Karta zablokovana.", "Zadajte PIN kod", "Vybrat hotovost", "Stav uctu",
                                        "Zmenit PIN","Zly pokus. Zadajte PIN.","3 zle pokusy. Karta zablokovana","Pin bol zmeneny." };

        public Messages()
        {

        }

        public string[] getMessages(string language)
        {
            if (language == "english")
            {
                return messagesEN;
            }
            if (language == "slovak")
            {
                return messagesSK;
            }
            return null;
        }


    }
}
