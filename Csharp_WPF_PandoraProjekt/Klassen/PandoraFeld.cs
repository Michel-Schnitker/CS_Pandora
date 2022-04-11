using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class PandoraFeld
    {
        // ---------------------------------------------------------------------- variablen für Pflanzen
        // TODO : Datenkapseln
        public int PflanzenRundenZähler = 0;
        public int Pflanzenbewuchs = 0;

        // ---------------------------------------------------------------------- implementierte Klassen

        public Klassen.Lebensformen Lebewesen = new Klassen.Lebensformen();


        // ---------------------------------------------------------------------- Funktionen für Feld

        public void LöscheFeld()
        {

            LöschePflanzenbewuchs();
            Lebewesen.LöscheLebewesen();
        }



        // ---------------------------------------------------------------------- Funktionen für Pflanzen

        public void ErzeugePflanzenbewuchsImFeld (int PflanzenGröße)
        {
            Pflanzenbewuchs = PflanzenGröße;
            PflanzenRundenZähler = 0;
        }

        public void PflanzenWachsenLassen (int PflanzenWachstum)
        {
            Pflanzenbewuchs = Pflanzenbewuchs + PflanzenWachstum;
            //PflanzenRundenZähler++;
        }

        public void BewuchsReduzieren (int PflanzenReduktion)
        {
            Pflanzenbewuchs = Pflanzenbewuchs - PflanzenReduktion;

            if (Pflanzenbewuchs < 0)
            {
                Pflanzenbewuchs = 0;
            }
        }

        public void LöschePflanzenbewuchs ()
        {
            Pflanzenbewuchs = 0;
            PflanzenRundenZähler = 0;
        }

        // ---------------------------------------------------------------------- PandoraFeld ENDE

    }
}
