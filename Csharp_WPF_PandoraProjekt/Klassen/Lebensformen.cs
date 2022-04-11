using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class Lebensformen
    {

        public String TypLebewesen = "Leer";

        public int Sättigung = 0;
        public int Alter = 0;
        public int spielrundenZahl = 0;

        public int NächsteRundeFüreineGeburt = 0;






        public void ersetzeLebensformDurch(Klassen.Lebensformen newLebensform)
        {

            Sättigung = newLebensform.Sättigung;
            Alter = newLebensform.Alter;
            TypLebewesen = newLebensform.TypLebewesen;
            spielrundenZahl = newLebensform.spielrundenZahl;
            NächsteRundeFüreineGeburt = newLebensform.NächsteRundeFüreineGeburt;

        }


        public void LöscheLebewesen()
        {
            TypLebewesen = "Leer";
            Sättigung = 0;
            Alter = 0;
            spielrundenZahl = 0;
            NächsteRundeFüreineGeburt = 0;
        }

        public void ErzeugeFischImFeld(int spielrunde)
        {
            TypLebewesen = "Fisch";
            Sättigung = 100;
            Alter = 0;
            spielrundenZahl = spielrunde;
            NächsteRundeFüreineGeburt = 0;
        }

        public void ErzeugeHaiImFeld(int spielrunde)
        {
            TypLebewesen = "Hai";
            Sättigung = 100;
            Alter = 0;
            spielrundenZahl = spielrunde;
            NächsteRundeFüreineGeburt = 0;
        }





    }
}
