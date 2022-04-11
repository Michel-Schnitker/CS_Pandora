using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class Ringspeicher
    {

        public Object[] TestRingspeicher;


            public Klassen.PandoraDataStructs.RundenZähler[] RundenRingSpeicher;


        public void InitialRundenRingspeicher(int Speichergröße, Object KlasseDesRingSpeichers)
        {


            for (int i = 0; i < Speichergröße; i++)
            {


                TestRingspeicher[i] = KlasseDesRingSpeichers;


            }


        }


            // TODO : schöner schreiben
        public void InitialRundenRingspeicher(int Speichergröße)
        {
            RundenRingSpeicher = new Klassen.PandoraDataStructs.RundenZähler[Speichergröße];

            for (int j = 0; j < Speichergröße; j++)
            {

                RundenRingSpeicher[j] = new Klassen.PandoraDataStructs.RundenZähler();

            }

        }

        public void WertInRingSpeicherÜbernehmen(Klassen.PandoraDataStructs.RundenZähler neuerStand)
        {
            // TODO: abfrage ob RundenRingSpeicher ungleich NULL wenn NULL dann mit standart initialisieren
            for (int i = RundenRingSpeicher.Length - 1; i >= 0; i--)
            {
                // TODO: testen



                if (i > 0)
                {
                    RundenRingSpeicher[i] = RundenRingSpeicher[i - 1];

                    RundenRingSpeicher[i - 1] = new Klassen.PandoraDataStructs.RundenZähler();
                }
                else if (i == 0)
                {
                    int tmp_Fische = neuerStand.Fische;
                    int tmp_Haie = neuerStand.Haie;
                    int tmp_Pflanzen = neuerStand.Pflanzen;
                    int tmp_Runde = neuerStand.Runde;
                    int tmp_Wasser = neuerStand.Wasser;

                    RundenRingSpeicher[0].Fische = tmp_Fische;
                    RundenRingSpeicher[0].Haie = tmp_Haie;
                    RundenRingSpeicher[0].Pflanzen = tmp_Pflanzen;
                    RundenRingSpeicher[0].Runde = tmp_Runde;
                    RundenRingSpeicher[0].Wasser = tmp_Wasser;

                    //RundenRingSpeicher[0] = neuerStand;

                }


            }


        }


        public Klassen.PandoraDataStructs.RundenZähler[] RückgabaRingspeicher()
        {
            // TODO: abfrage ob RundenRingSpeicher ungleich NULL
            return RundenRingSpeicher;
        }


    }
}
