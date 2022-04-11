using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    public class Parameter
    {
        // ---------------------------------------------------------------------- implementierte Klassen

        public Klassen.Parameter.GenerierungsParameter ProzentGenerierung = new Klassen.Parameter.GenerierungsParameter();
        public Klassen.Parameter.FischParameter fischParameter = new Klassen.Parameter.FischParameter();
        public Klassen.Parameter.HaiParameter haiParameter = new Klassen.Parameter.HaiParameter();
        public Klassen.Parameter.PflanzenParameter pflanzenParameter = new Klassen.Parameter.PflanzenParameter();
        public Klassen.Parameter.Automatikparameter automatikparameter = new Klassen.Parameter.Automatikparameter();


        // ---------------------------------------------------------------------- Funktionen

        public void ÜberschreibenDerParameter(Parameter NeueParameter)
        {

            // TODO geht das so ?
            ProzentGenerierung = NeueParameter.ProzentGenerierung;
            fischParameter = NeueParameter.fischParameter;
            haiParameter = NeueParameter.haiParameter;
            pflanzenParameter = NeueParameter.pflanzenParameter;
            automatikparameter = NeueParameter.automatikparameter;

        }

        // ---------------------------------------------------------------------- Unterklassen


        public class GenerierungsParameter
        {
            public int ProzentWasser = 85;
            public int ProzentFische = 4;
            public int ProzentHaie = 1;
            public int ProzentPflanzen = 10;
        }

        public class Automatikparameter
        {
            // ---------------------------------------------------------------------- Automatikparameter Variablen

            internal bool Vollautomatik = false;
            internal bool Halbautomatik = false;

            internal bool LineareAbarbeitung = false;
            internal bool ZufälligeAbarbeitung = false;

            // ---------------------------------------------------------------------- Automatikparameter Funktionen

            public void AktiviereVollautomatik()
            {
                Vollautomatik = true;
                Halbautomatik = false;
            }

            public void AktiviereHalbautomatik()
            {
                Vollautomatik = false;
                Halbautomatik = true;
            }

            public void AktiviereLineareAbarbeitung()
            {
                LineareAbarbeitung = true;
                ZufälligeAbarbeitung = false;
            }

            public void AktiviereZufälligeAbarbeitung()
            {
                LineareAbarbeitung = false;
                ZufälligeAbarbeitung = true;
            }


            public bool Abfrage_ModusIstVollAutomatik()
            {
                if ((Vollautomatik && Halbautomatik) || (!Vollautomatik && !Halbautomatik))
                {
                    Vollautomatik = true;
                    Halbautomatik = false;

                    // FEHLER
                }


                if (Vollautomatik)
                {
                    return true;

                }
                else
                {
                    return false;
                
                }
            }


            public bool Abfrage_ModusIstHalbAutomatik()
            {

                if ((Vollautomatik && Halbautomatik) || (!Vollautomatik && !Halbautomatik))
                {
                    Vollautomatik = true;
                    Halbautomatik = false;

                    // FEHLER
                }


                if (Halbautomatik)
                {
                    return true;

                }
                else
                {
                    return false;

                }
            }

            public bool Abfrage_BearbeitungIstLinear()
            {

                if ((LineareAbarbeitung && ZufälligeAbarbeitung) || (!LineareAbarbeitung && !ZufälligeAbarbeitung))
                {
                    LineareAbarbeitung = true;
                    ZufälligeAbarbeitung = false;

                    // FEHLER
                }


                if (LineareAbarbeitung)
                {
                    return true;

                }
                else
                {
                    return false;

                }
            }

            public bool Abfrage_BearbeitungIstZufällig()
            {

                if ((LineareAbarbeitung && ZufälligeAbarbeitung) || (!LineareAbarbeitung && !ZufälligeAbarbeitung))
                {
                    LineareAbarbeitung = true;
                    ZufälligeAbarbeitung = false;

                    // FEHLER
                }


                if (ZufälligeAbarbeitung)
                {
                    return true;

                }
                else
                {
                    return false;

                }
            }



        }

        public class FischParameter
        {
            public int AlterFähigFürFortpflanzung = 0;
            public int AlterSterben = 5;
            public int GeburtenWiederholhunginRunden = 10;


            public int SättigungBeimEssen = 30;
            public int SättigungsVerbrauch = 10;


        }

        public class HaiParameter
        {
            public int AlterFähigFürFortpflanzung = 20;
            public int AlterSterben = 250;
            public int GeburtenWiederholhunginRunden = 30;


            public int SättigungBeimEssen = 40;
            public int SättigungsVerbrauch = 5;

        }

        public class PflanzenParameter
        {

            public int minimalWachstum = 10;
            public int maximalWachstum = 20;

            public int maximalerBewuchsImUmfeld = 200;


        }

        // ---------------------------------------------------------------------- Parameter ENDE
    }





}

