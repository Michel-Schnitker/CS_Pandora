using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class PandoraDataStructs
    {

        public class Suchergebnis
        {
            // TODO : Datenkapseln
            public bool ErgebnisPlausible = false;

            public int AnzahlMöglicherFelder = 0;      // Anzahl aller gefundener Felder passend zur Suche

            public Klassen.PandoraDataStructs.FeldPosition ZufallsFeld = new Klassen.PandoraDataStructs.FeldPosition(); // Gefundene Zufalls Position
            
        }

        public class FeldPosition
        {
            // TODO : Datenkapseln
            public int Pos_i = 0;
            public int Pos_j = 0;

            public void PosLöschen()
            {
                Pos_i = 0;
                Pos_j = 0;
            }
        }

        public class SuchPositionenZwischenMerker
        {
            // TODO : Datenkapseln
            public int AnzahlMöglicherFelder = 0;      // Anzahl aller gefundener Felder passend zur Suche
            public int AnzahlGefundenePositionen = 0; //  Gesamtzahl aller Felder die für die Zufallssuche möglich waren

            public Klassen.PandoraDataStructs.FeldPosition[] GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];


            // TODO : initialisieren bei initíalisieren von instanz class SuchPosMerker
            public void InitGefundenPostionen () // TODO Kann man das schöner machen ?
            {
                for (int j = 0; j < 10; j++)
                {

                    GefundenePositionen[j] = new PandoraDataStructs.FeldPosition();



                }

            }

        }

        public class RundenZähler
        {
            // TODO : Datenkapseln
            public int Wasser = 0;
            public int Pflanzen = 0;
            public int Fische = 0;
            public int Haie = 0;

            public int Runde = 0;

        }

        


    }
}
