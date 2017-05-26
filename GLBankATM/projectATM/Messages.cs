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
                                        "3 failed attempts. Card blocked.","Pin changed successfully","PIN change failed",
                                        "Enter amount:", "Invalid amount :(","Not enough money","Done. Balance:"};

        private string[] messagesSK = { "Karta zablokovana.", "Zadajte PIN kod", "Vybrat hotovost", "Stav uctu",
                                        "Zmenit PIN","Zly pokus. Zadajte PIN.","3 zle pokusy. Karta zablokovana","Pin bol zmeneny.","Neuspesna zmena pinu",
                                        "Zadajte sumu:","Nevhodna suma :(","Nedostatok penazi","Hotovo. Zostatok:"};

        private string[] messagesFIN = {"Kortti on estetty","Kirjoita PIN-koodi","Nostaa rahaa","Tilin saldo", "Vaihda PIN-koodi", "Väärä yritys",
                                        "Kortti on estetty","Pin muuttui onnistuneesti", "PIN-muutos epäonnistui","Kirjoita määrä:","Virheellinen määrä :(",
                                        "Ei tarpeeksi rahaa","tehty. saldo:"};

        private string[] messagesSWE = {"Kort blockerat.","Ange PIN-kod","Ta ut kontanter","Kontobalans","Ändra PIN-kod","Kort blockerat","Kort blockerat",
                                        "PIN-kod ändrats","PIN-kodändring misslyckades","Ange beloppet","Ogiltigt belopp :(","inte tillräckligt med pengar","Gjort. balans:"};

        private string[] messagesRU = {"Карта заблокирована.","Введите пин-код","снимать наличные","баланс","Изменить пин-код", "Введите пин-код", "Карта заблокирована. Блядь!",
                                        "PIN-код изменен","Сбой изменения пин-кода","введите сумму","Недопустимая сумма :(","недостаточно денег","сделанный. баланс:" };

        private string[] messagesDE = {"Karte blockiert.","PIN-Code eingeben","Geld abheben","Kontostand","PIN ändern", "PIN-Code eingeben", "Karte blockiert",
                                        "PIN-Code geändert","Pin-Änderung fehlgeschlagen","Menge eingeben","ungültige Menge :(","nicht genug Geld","Erledigt. Kontostand:" };



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
            if (language == "finnish")
            {
                return messagesFIN;
            }
            if (language == "swedish")
            {
                return messagesSWE;
            }

            if (language == "russian")
            {
                return messagesRU;
            }
            if (language == "german")
            {
                return messagesDE;
            }
            return null;
        }


    }
}
