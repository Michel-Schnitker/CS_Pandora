using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class PandoraStatistics
    {

        // ---------------------------------------------------------------------- Globale Statistik Veriablen

        public int StatisticRundenzähler = 0;

        public Klassen.PandoraDataStructs.RundenZähler AktuellStatisticZähler = new Klassen.PandoraDataStructs.RundenZähler();



        public Klassen.Ringspeicher ringspeicher = new Klassen.Ringspeicher();







        public void FeldAuszählen (Klassen.Pandora PandoraFeld)
        {
            

            int Zwischenzähler_Pflanzen = 0;
            int Zwischenzähler_Fische = 0;
            int Zwischenzähler_Haie = 0;
            int Zwischenzähler_Wasser = 0;

            for (int i = 0; i < PandoraFeld.AusgabeZellenHoch(); i++)
            {
                for (int j = 0; j < PandoraFeld.AusgabeZellenBreit(); j++)
                {

                    //TODO im gesamten überarbeiten was bedeutet was ? ist ein feld mit pflanzen aber ohne lebewesen leer ?


                    if (PandoraFeld.Feld[i, j].Pflanzenbewuchs > 0)
                    {
                        Zwischenzähler_Pflanzen++;
                        
                    }

                    if (PandoraFeld.Feld[i, j].Lebewesen.TypLebewesen == "Fisch")
                    {
                        Zwischenzähler_Fische++;
                    }
                    else if (PandoraFeld.Feld[i, j].Lebewesen.TypLebewesen == "Hai")
                    {
                        Zwischenzähler_Haie++;
                    }
                    else if (PandoraFeld.Feld[i, j].Lebewesen.TypLebewesen == "Leer" /*&& PandoraFeld.Feld[i, j].Pflanzenbewuchs == 0*/)
                    {
                        Zwischenzähler_Wasser++;
                    }

                }
            }


            AktuellStatisticZähler.Fische = Zwischenzähler_Fische;
            AktuellStatisticZähler.Haie = Zwischenzähler_Haie;
            AktuellStatisticZähler.Wasser = Zwischenzähler_Wasser;
            AktuellStatisticZähler.Pflanzen = Zwischenzähler_Pflanzen;
            AktuellStatisticZähler.Runde = PandoraFeld.Global_Rundenzähler;

            StatisticRundenzähler = PandoraFeld.Global_Rundenzähler;


        }



    }
}
