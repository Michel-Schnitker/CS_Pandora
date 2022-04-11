using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_WPF_PandoraProjekt.Klassen
{
    class Pandora
    {
        // ---------------------------------------------------------------------- Globale Pandora Veriablen

        // TODO : Datenkapseln

        public int Global_Rundenzähler = 0;

        internal int Global_anzahlZellenBreit = 0;
        internal int Global_anzahlZellenHoch = 0;


        // ---------------------------------------------------------------------- Funktoinsspezifische Variablen

        Klassen.PandoraDataStructs.FeldPosition[] Abarbeitsorder;
        Klassen.PandoraDataStructs.FeldPosition aktuellerFeldZeigerFürSemiAutomatik = new Klassen.PandoraDataStructs.FeldPosition();
        int AbarbeitsorderZähler = 0;

        // ---------------------------------------------------------------------- implementierte Klassen

        Random würfel = new Random();
        public Klassen.PandoraFeld[,] Feld; // wird in der PandoraInitialisierenschleife initialisiert
        public Klassen.Parameter ParameterSatz = new Klassen.Parameter();

        public Klassen.PandoraStatistics PandoraStatistics = new Klassen.PandoraStatistics();


        // ---------------------------------------------------------------------- Spielfeld generienungs Funktionen
        

        public void PandoraInitialisieren(int anzahlZellenBreit, int anzahlZellenHoch)
        {

            if (anzahlZellenBreit > 0 && anzahlZellenHoch > 0)
            {
                Global_anzahlZellenBreit = anzahlZellenBreit;
                Global_anzahlZellenHoch = anzahlZellenHoch;

                Feld = new Klassen.PandoraFeld[Global_anzahlZellenBreit, Global_anzahlZellenHoch];

                Abarbeitsorder = new Klassen.PandoraDataStructs.FeldPosition[Global_anzahlZellenBreit * Global_anzahlZellenHoch];
                

                for (int o = 0; o < (Global_anzahlZellenBreit * Global_anzahlZellenHoch); o++)
                {
                    Abarbeitsorder[o] = new Klassen.PandoraDataStructs.FeldPosition();
                }

                NeueAbarbeitsOrderErstellen();




                for (int i = 0; i < Global_anzahlZellenHoch; i++)
                {
                    for (int j = 0; j < Global_anzahlZellenBreit; j++)
                    {

                        Feld[i, j] = new PandoraFeld();



                    }
                }

                Global_Rundenzähler = 0;


            }
            else
            {
                //FEHLER TODO
            }

        }

        public void GanzesSpielFeldLöschen()
        {

            for (int i = 0; i < Global_anzahlZellenHoch; i++)
            {
                for (int j = 0; j < Global_anzahlZellenBreit; j++)
                {

                    Feld[i, j].LöscheFeld();

                }
            }

            Global_Rundenzähler = 0;


            aktuellerFeldZeigerFürSemiAutomatik.PosLöschen();

        }

        public void EinzelnesFeldZufälligFüllen(int i, int j)
        {
            Feld[i, j].LöscheFeld();

            //TODO random generation überarbeiten


            int Gesamtzahl = 100;

            Gesamtzahl = ParameterSatz.ProzentGenerierung.ProzentWasser + ParameterSatz.ProzentGenerierung.ProzentPflanzen + ParameterSatz.ProzentGenerierung.ProzentFische + ParameterSatz.ProzentGenerierung.ProzentHaie;

                int zufallszahl = würfel.Next(1, Gesamtzahl);

            if (zufallszahl < Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser))
            {
                Feld[i, j].LöscheFeld();
                
            }
            else if (zufallszahl >= Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser) && zufallszahl < Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser + ParameterSatz.ProzentGenerierung.ProzentPflanzen))
            {
                // Feld[i, j].Lebewesen.LöscheLebewesen();

                
                int Pflanzengröße = würfel.Next(ParameterSatz.pflanzenParameter.minimalWachstum, ParameterSatz.pflanzenParameter.maximalWachstum);

                Feld[i, j].ErzeugePflanzenbewuchsImFeld(Pflanzengröße);
            }
            else if (zufallszahl >= Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser + ParameterSatz.ProzentGenerierung.ProzentPflanzen - 1) && zufallszahl < Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser + ParameterSatz.ProzentGenerierung.ProzentPflanzen + ParameterSatz.ProzentGenerierung.ProzentFische))
            {

                Feld[i, j].Lebewesen.ErzeugeFischImFeld(0);
                

            }
            else if (zufallszahl >= Convert.ToInt32(ParameterSatz.ProzentGenerierung.ProzentWasser + ParameterSatz.ProzentGenerierung.ProzentPflanzen + ParameterSatz.ProzentGenerierung.ProzentFische))
            {

                Feld[i, j].Lebewesen.ErzeugeHaiImFeld(0);
                
            }
            else
            {
                // Fehler
            }


        }

        public void GanzesSpielfeldZufälligFüllen()
        {
            

            for (int i = 0; i < Global_anzahlZellenHoch; i++)
            {
                for (int j = 0; j < Global_anzahlZellenBreit; j++)
                {


                    EinzelnesFeldZufälligFüllen(i, j);

                }
            }

            Global_Rundenzähler = 0;

            aktuellerFeldZeigerFürSemiAutomatik.PosLöschen();

        }

        // ---------------------------------------------------------------------- Spiel Runden Funktionen

        public void LebewesenFristZiel (int pos_i, int pos_j, int Zielpos_i, int Zielpos_j)
        {

            if (Feld[pos_i,pos_j].Lebewesen.TypLebewesen == "Fisch")
            {

                if (Feld[Zielpos_i, Zielpos_j].Pflanzenbewuchs > 0)
                {

                    Console.WriteLine("Fisch isst");
                    


                    if (Feld[Zielpos_i, Zielpos_j].Pflanzenbewuchs > ParameterSatz.fischParameter.SättigungBeimEssen)
                    {

                        Feld[Zielpos_i, Zielpos_j].BewuchsReduzieren(ParameterSatz.fischParameter.SättigungBeimEssen);
                        //Feld[Zielpos_i, Zielpos_j].Pflanzenbewuchs =- ParameterSatz.fischParameter.SättigungBeimEssen;

                        Feld[pos_i, pos_j].Lebewesen.Sättigung = Feld[pos_i, pos_j].Lebewesen.Sättigung + ParameterSatz.fischParameter.SättigungBeimEssen;



                    }
                    else if (Feld[Zielpos_i, Zielpos_j].Pflanzenbewuchs <= ParameterSatz.fischParameter.SättigungBeimEssen)
                    {

                        Feld[pos_i, pos_j].Lebewesen.Sättigung = Feld[pos_i, pos_j].Lebewesen.Sättigung + Feld[Zielpos_i, Zielpos_j].Pflanzenbewuchs;

                        Feld[Zielpos_i, Zielpos_j].LöschePflanzenbewuchs();

                    }




                }
                else
                {
                    //TODO : Console.WriteLine("Error keine Pflanzliche Nahung vorhanden");
                    // FEHLER
                }



            }
            else if (Feld[pos_i, pos_j].Lebewesen.TypLebewesen == "Hai")
            {

                if (Feld[Zielpos_i, Zielpos_j].Lebewesen.TypLebewesen == "Fisch")
                {



                    if (Feld[Zielpos_i, Zielpos_j].Lebewesen.Sättigung > ParameterSatz.haiParameter.SättigungBeimEssen)
                    {



                        Feld[pos_i, pos_j].Lebewesen.Sättigung = Feld[pos_i, pos_j].Lebewesen.Sättigung + ParameterSatz.haiParameter.SättigungBeimEssen;



                    }
                    else if (Feld[Zielpos_i, Zielpos_j].Lebewesen.Sättigung <= ParameterSatz.haiParameter.SättigungBeimEssen)
                    {

                        Feld[pos_i, pos_j].Lebewesen.Sättigung = Feld[pos_i, pos_j].Lebewesen.Sättigung + Feld[Zielpos_i, Zielpos_j].Lebewesen.Sättigung;

                    }


                    Feld[Zielpos_i, Zielpos_j].Lebewesen.LöscheLebewesen();


                }
                else
                {
                    // Fehler
                }



            }
            else
            {
                // Fehler
            }



        }


        public void LebewesenVonFeldZuFeldWandern(int von_i, int von_J, int zu_i, int zu_j)
        {

            

            if (Feld[zu_i, zu_j].Lebewesen.TypLebewesen != "Leer" )
            {
                //FEHLER
            }



            Feld[zu_i, zu_j].Lebewesen.ersetzeLebensformDurch(Feld[von_i, von_J].Lebewesen);
       
            Feld[von_i, von_J].Lebewesen.LöscheLebewesen();

            
        }




        public Klassen.PandoraDataStructs.Suchergebnis SucheZufallsFeldNachFloraInteresse(int pos_i, int pos_j, String pointOfInterest, bool SucheFeldOhneLebewesen)
        {

            bool SucheOderErgenbisIstUnlogisch = false;

            Klassen.PandoraDataStructs.SuchPositionenZwischenMerker zwischenMerker = new Klassen.PandoraDataStructs.SuchPositionenZwischenMerker();
            Klassen.PandoraDataStructs.Suchergebnis suchergebnis = new Klassen.PandoraDataStructs.Suchergebnis();

            zwischenMerker.InitGefundenPostionen();





            int iDarüber = pos_i - 1;
            if (iDarüber < 0)
            { iDarüber = Global_anzahlZellenHoch - 1; }

            int iDarunter = pos_i + 1;
            if (iDarunter >= Global_anzahlZellenHoch)
            { iDarunter = 0; }

            int jLinks = pos_j - 1;
            if (jLinks < 0)
            { jLinks = Global_anzahlZellenBreit - 1; }

            int jRechts = pos_j + 1;
            if (jRechts >= Global_anzahlZellenBreit)
            { jRechts = 0; }


            if (pointOfInterest == "Pflanze")
            {

                // feld suchen bei pflanzen
                // suche nach größte Pflanze


                if (Feld[iDarüber, jLinks].Pflanzenbewuchs > 0 && (Feld[iDarüber, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben links
                {

                    // prüfen ob bereits ein feld gefunden
                    // wenn nein eintragen wenn ja vergleichen // zähler hoch zählen
                    // wenn größer überschreiben
                    // wenn gleich dann dazu eintragen // zähler hochzählen

                    zwischenMerker.AnzahlMöglicherFelder++;

                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, jLinks].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        

                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;

                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jLinks;


                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, jLinks].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }



                }

                if (Feld[iDarüber, pos_j].Pflanzenbewuchs > 0 && (Feld[iDarüber, pos_j].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben mittig
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = iDarüber;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = pos_j;
                    //Interesst1_ZählerGefundeneFälder++;
                    zwischenMerker.AnzahlMöglicherFelder++;


                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, pos_j].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[0].Pos_j = pos_j;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, pos_j].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }




                }

                if (Feld[iDarüber, jRechts].Pflanzenbewuchs > 0 && (Feld[iDarüber, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben rechts
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = iDarüber;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = jRechts;
                    //Interesst1_ZählerGefundeneFälder++;


                    zwischenMerker.AnzahlMöglicherFelder++;

                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, jRechts].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jRechts;


                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarüber, jRechts].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }





                }

                if (Feld[pos_i, jRechts].Pflanzenbewuchs > 0 && (Feld[pos_i, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Mitte rechts
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = pos_i;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = jRechts;
                    //Interesst1_ZählerGefundeneFälder++;

                    zwischenMerker.AnzahlMöglicherFelder++;

                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[pos_i, jRechts].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jRechts;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[pos_i, jRechts].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }




                }

                if (Feld[pos_i, jLinks].Pflanzenbewuchs > 0 && (Feld[pos_i, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // mitte links
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = pos_i;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = jLinks;
                    //Interesst1_ZählerGefundeneFälder++;

                    zwischenMerker.AnzahlMöglicherFelder++;


                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[pos_i, jLinks].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jLinks;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[pos_i, jLinks].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }





                }

                if (Feld[iDarunter, jLinks].Pflanzenbewuchs > 0 && (Feld[iDarunter, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten links
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = iDarunter;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = jLinks;
                    //Interesst1_ZählerGefundeneFälder++;


                    zwischenMerker.AnzahlMöglicherFelder++;

                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, jLinks].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jLinks;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, jLinks].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }





                }

                if (Feld[iDarunter, pos_j].Pflanzenbewuchs > 0 && (Feld[iDarunter, pos_j].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten mittig
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = iDarunter;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = pos_j;
                    //Interesst1_ZählerGefundeneFälder++;

                    zwischenMerker.AnzahlMöglicherFelder++;


                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, pos_j].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[0].Pos_j = pos_j;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, pos_j].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }





                }

                if (Feld[iDarunter, jRechts].Pflanzenbewuchs > 0 && (Feld[iDarunter, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten rechts
                {


                    //Interesst1_gefundenePositionenI[Interesst1_ZählerGefundeneFälder] = iDarunter;
                    //Interesst1_gefundenePositionenJ[Interesst1_ZählerGefundeneFälder] = jRechts;
                    //Interesst1_ZählerGefundeneFälder++;


                    zwischenMerker.AnzahlMöglicherFelder++;

                    if (zwischenMerker.AnzahlGefundenePositionen == 0)
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, jRechts].Pflanzenbewuchs > Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        //zwischenMerker.GefundenePositionen = new Klassen.PandoraDataStructs.FeldPosition[10];
                        zwischenMerker.InitGefundenPostionen();

                        zwischenMerker.AnzahlGefundenePositionen = 1;



                        zwischenMerker.GefundenePositionen[0].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[0].Pos_j = jRechts;

                    }
                    else if (zwischenMerker.AnzahlGefundenePositionen >= 1 && (Feld[iDarunter, jRechts].Pflanzenbewuchs == Feld[zwischenMerker.GefundenePositionen[0].Pos_i, zwischenMerker.GefundenePositionen[0].Pos_j].Pflanzenbewuchs))
                    {
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;

                    }
                    else
                    {
                        // nicht größer erwäckt keine aufmerksamkeit
                    }





                }








            }
            else if (pointOfInterest == "Leer")
            {



                if (Feld[iDarüber, jLinks].Pflanzenbewuchs == 0 && (Feld[iDarüber, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben links
                {

                    zwischenMerker.AnzahlMöglicherFelder++;




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;

                }

                if (Feld[iDarüber, pos_j].Pflanzenbewuchs == 0 && (Feld[iDarüber, pos_j].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben mittig
                {

                    zwischenMerker.AnzahlMöglicherFelder++;




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                    zwischenMerker.AnzahlGefundenePositionen++;



                }

                if (Feld[iDarüber, jRechts].Pflanzenbewuchs == 0 && (Feld[iDarüber, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Oben rechts
                {
                    zwischenMerker.AnzahlMöglicherFelder++;




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;




                }

                if (Feld[pos_i, jRechts].Pflanzenbewuchs == 0 && (Feld[pos_i, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // Mitte rechts
                {

                    zwischenMerker.AnzahlMöglicherFelder++;




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;



                }

                if (Feld[pos_i, jLinks].Pflanzenbewuchs == 0 && (Feld[pos_i, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // mitte links
                {
                    zwischenMerker.AnzahlMöglicherFelder++; 




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;



                }

                if (Feld[iDarunter, jLinks].Pflanzenbewuchs == 0 && (Feld[iDarunter, jLinks].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten links
                {
                    zwischenMerker.AnzahlMöglicherFelder++; 





                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;



                }

                if (Feld[iDarunter, pos_j].Pflanzenbewuchs == 0 && (Feld[iDarunter, pos_j].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten mittig
                {

                    zwischenMerker.AnzahlMöglicherFelder++;




                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                    zwischenMerker.AnzahlGefundenePositionen++;



                }

                if (Feld[iDarunter, jRechts].Pflanzenbewuchs == 0 && (Feld[iDarunter, jRechts].Lebewesen.TypLebewesen == "Leer" && SucheFeldOhneLebewesen || !SucheFeldOhneLebewesen)) // unten rechts
                {

                    zwischenMerker.AnzahlMöglicherFelder++;


                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;


                }




            }
            else if (pointOfInterest == "Fisch" || pointOfInterest == "Hai")
            {

                // FEHLER
                SucheOderErgenbisIstUnlogisch = true;
            }
            else
            {
                // was wenn eingabe falsch ?
                SucheOderErgenbisIstUnlogisch = true;
            }




            // TODO: Funktion pürfen ob auch die nullte position genommmen wird
            int zufallsfeld = würfel.Next(0, zwischenMerker.AnzahlGefundenePositionen);


            suchergebnis.ZufallsFeld.Pos_i = zwischenMerker.GefundenePositionen[zufallsfeld].Pos_i;
            suchergebnis.ZufallsFeld.Pos_j = zwischenMerker.GefundenePositionen[zufallsfeld].Pos_j;

            suchergebnis.AnzahlMöglicherFelder = zwischenMerker.AnzahlMöglicherFelder;


            if (suchergebnis.ZufallsFeld.Pos_i == 0 && suchergebnis.ZufallsFeld.Pos_j == 0 && (!(pos_i == 1 || pos_i == 0 || pos_i == Global_anzahlZellenBreit - 1) || !(pos_j == 1 || pos_j == 0 || pos_j == Global_anzahlZellenHoch - 1)))
            {

                ///////////////////////////////
                //  0,0     1,0     n,0     ~
                //  0,1     1,1     n,1     ~
                //  0,n     1,n     n,n     ~
                //  ~~~~~~~~~~~~~~~~~~~~~~~~~

                SucheOderErgenbisIstUnlogisch = true;

            }

            suchergebnis.ErgebnisPlausible = !SucheOderErgenbisIstUnlogisch;

            return suchergebnis;

        }


        public Klassen.PandoraDataStructs.Suchergebnis SucheZufallsFeldNachFaunaInteresse(int pos_i, int pos_j, String pointOfInterest, bool geburtenfähig = false)
        {

            bool SucheOderErgenbisIstUnlogisch = false;

            Klassen.PandoraDataStructs.SuchPositionenZwischenMerker zwischenMerker = new Klassen.PandoraDataStructs.SuchPositionenZwischenMerker();
            Klassen.PandoraDataStructs.Suchergebnis suchergebnis = new Klassen.PandoraDataStructs.Suchergebnis();

            zwischenMerker.InitGefundenPostionen();

            int iDarüber = pos_i - 1;
            if (iDarüber < 0)
            { iDarüber = Global_anzahlZellenHoch - 1; }

            int iDarunter = pos_i + 1;
            if (iDarunter >= Global_anzahlZellenHoch)
            { iDarunter = 0; }

            int jLinks = pos_j - 1;
            if (jLinks < 0)
            { jLinks = Global_anzahlZellenBreit - 1; }

            int jRechts = pos_j + 1;
            if (jRechts >= Global_anzahlZellenBreit)
            { jRechts = 0; }


            if (pointOfInterest == "Pflanze")
            {
                SucheOderErgenbisIstUnlogisch = true;
                // FEHLER

            }
            else if (pointOfInterest == "Leer")
            {

                if (Feld[iDarüber, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // Oben links
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;

                }

                if (Feld[iDarüber, pos_j].Lebewesen.TypLebewesen == pointOfInterest) // Oben mittig
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[iDarüber, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // Oben rechts
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[pos_i, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // Mitte rechts
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[pos_i, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // mitte links
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[iDarunter, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // unten links
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[iDarunter, pos_j].Lebewesen.TypLebewesen == pointOfInterest) // unten mittig
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

                if (Feld[iDarunter, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // unten rechts
                {
                    zwischenMerker.AnzahlMöglicherFelder++;

                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                    zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                    zwischenMerker.AnzahlGefundenePositionen++;
                }

            }
            else if (pointOfInterest == "Fisch" || pointOfInterest == "Hai" )
            {

                if (Feld[iDarüber, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // Oben links
                {



                    if ((Feld[iDarüber, jLinks].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }


                }

                if (Feld[iDarüber, pos_j].Lebewesen.TypLebewesen == pointOfInterest) // Oben mittig
                {


                    if ((Feld[iDarüber, pos_j].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[iDarüber, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // Oben rechts
                {



                    if ((Feld[iDarüber, jRechts].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarüber;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[pos_i, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // Mitte rechts
                {



                    if ((Feld[pos_i, jRechts].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[pos_i, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // mitte links
                {



                    if ((Feld[pos_i, jLinks].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = pos_i;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[iDarunter, jLinks].Lebewesen.TypLebewesen == pointOfInterest) // unten links
                {



                    if ((Feld[iDarunter, jLinks].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jLinks;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[iDarunter, pos_j].Lebewesen.TypLebewesen == pointOfInterest) // unten mittig
                {



                    if ((Feld[iDarunter, pos_j].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = pos_j;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }

                if (Feld[iDarunter, jRechts].Lebewesen.TypLebewesen == pointOfInterest) // unten rechts
                {



                    if ((Feld[iDarunter, jRechts].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler & geburtenfähig) || !geburtenfähig)
                    {
                        zwischenMerker.AnzahlMöglicherFelder++;

                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_i = iDarunter;
                        zwischenMerker.GefundenePositionen[zwischenMerker.AnzahlGefundenePositionen].Pos_j = jRechts;
                        zwischenMerker.AnzahlGefundenePositionen++;
                    }

                }


            }
            else
            {
                SucheOderErgenbisIstUnlogisch = true;
                // was wenn eingabe falsch ?
            }
            

            // TODO: Funktion pürfen ob auch die nullte position genommmen wird
            int zufallsfeld = würfel.Next(0, zwischenMerker.AnzahlGefundenePositionen);


            suchergebnis.ZufallsFeld.Pos_i = zwischenMerker.GefundenePositionen[zufallsfeld].Pos_i;
            suchergebnis.ZufallsFeld.Pos_j = zwischenMerker.GefundenePositionen[zufallsfeld].Pos_j;

            suchergebnis.AnzahlMöglicherFelder = zwischenMerker.AnzahlMöglicherFelder;


            if (suchergebnis.ZufallsFeld.Pos_i == 0 && suchergebnis.ZufallsFeld.Pos_j == 0 && (!(pos_i == 1 || pos_i == 0 || pos_i == Global_anzahlZellenBreit - 1) || !(pos_j == 1 || pos_j == 0 || pos_j == Global_anzahlZellenHoch - 1)))
            {

                ///////////////////////////////
                //  0,0     1,0     n,0     ~
                //  0,1     1,1     n,1     ~
                //  0,n     1,n     n,n     ~
                //  ~~~~~~~~~~~~~~~~~~~~~~~~~

                SucheOderErgenbisIstUnlogisch = true;

            }

            suchergebnis.ErgebnisPlausible = !SucheOderErgenbisIstUnlogisch;

            return suchergebnis;

        }

        // ---------------------------------------------------------------------- Spiel Automatik Funktionen


        // TODO Automatikrunden zusammen fassen zu einer Funktion mit Parametern zusätzlich mit Random möglich machen 




        public void AutomatikRundeMitAllenFeldern()
        {
            
            // TODO Automatikrunde mit Random möglich machen ?


                Global_Rundenzähler++;
                for (int i = 0; i < Global_anzahlZellenHoch; i++)
                {
                    for (int j = 0; j < Global_anzahlZellenBreit; j++)
                    {
                        //TODO Automatikablauf

                    // TODO automatikablauf nur durch führen wenn auch etwas vorhanden ?!

                        AutomatikRundeMitLebewesen(i, j);
                        //
                        AutomatikRundeMitPflanze(i, j);
                    }
                }
            
        }


        public void SemiAutomatikRundeMitAllenFeldern()
        {

            // TODO Semiautomatik auch nach Random möglich machen


                bool SichtbareBewegungDurchgeführt = false;



            // TODO einsprungerlaubnis nur wenn mindestens ein feld beleget , vielleicht bereits gelöst

            for (; !SichtbareBewegungDurchgeführt;)
            {







                   if (aktuellerFeldZeigerFürSemiAutomatik.Pos_i == 0 && aktuellerFeldZeigerFürSemiAutomatik.Pos_j == 0)
                    {
                        // TODO Funktioniert der rundenzähler auch ohne lebewesen im wasser ?
                        Global_Rundenzähler++;
                    }


                    if (Feld[aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j].Lebewesen.TypLebewesen != "Leer")
                    {
                        AutomatikRundeMitLebewesen(aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j);
                        SichtbareBewegungDurchgeführt = true;
                    }

                    if (Feld[aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j].Pflanzenbewuchs > 0)
                    {
                        AutomatikRundeMitPflanze(aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j);
                        SichtbareBewegungDurchgeführt = true;
                    }




                    aktuellerFeldZeigerFürSemiAutomatik.Pos_j++;

                    if (aktuellerFeldZeigerFürSemiAutomatik.Pos_j > (Global_anzahlZellenBreit - 1))
                    {
                        aktuellerFeldZeigerFürSemiAutomatik.Pos_j = 0;
                        aktuellerFeldZeigerFürSemiAutomatik.Pos_i++;
                    }

                    if (aktuellerFeldZeigerFürSemiAutomatik.Pos_i > (Global_anzahlZellenHoch - 1))
                    {
                        aktuellerFeldZeigerFürSemiAutomatik.Pos_i = 0;
                    }



                    if (!SichtbareBewegungDurchgeführt && IstDasSpielfeldLeer())
                    {
                        SichtbareBewegungDurchgeführt = true; // NOTaussprung bei keiner sichtbaren bewegeung

                        aktuellerFeldZeigerFürSemiAutomatik.PosLöschen();
                        

                        // errormeldung kein Lebewesen im wasser


                    }

                }

        }


        public void TESTAutomatikRundeMitAllenFeldern()
        {


            

            if (Abarbeitsorder[0].Pos_i == 0 && Abarbeitsorder[0].Pos_j == 0)
            {
                NeueAbarbeitsOrderErstellen();
            }



            // TODO Semiautomatik auch nach Random möglich machen


            bool SichtbareBewegungDurchgeführt = false;



            // TODO einsprungerlaubnis nur wenn mindestens ein feld beleget , vielleicht bereits gelöst

            for (; !SichtbareBewegungDurchgeführt;)
            {


                if (AbarbeitsorderZähler >=Global_anzahlZellenBreit*Global_anzahlZellenHoch)
                {
                    AbarbeitsorderZähler = 0;

                    SichtbareBewegungDurchgeführt = true;
                }

                aktuellerFeldZeigerFürSemiAutomatik.Pos_i = Abarbeitsorder[AbarbeitsorderZähler].Pos_i;
                    aktuellerFeldZeigerFürSemiAutomatik.Pos_j = Abarbeitsorder[AbarbeitsorderZähler].Pos_j;




                if (AbarbeitsorderZähler == 0)
                {
                    // TODO Funktioniert der rundenzähler auch ohne lebewesen im wasser ?
                    Global_Rundenzähler++;
                }


                if (Feld[aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j].Lebewesen.TypLebewesen != "Leer")
                {
                    AutomatikRundeMitLebewesen(aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j);
                    //SichtbareBewegungDurchgeführt = true;
                }

                if (Feld[aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j].Pflanzenbewuchs > 0)
                {
                    AutomatikRundeMitPflanze(aktuellerFeldZeigerFürSemiAutomatik.Pos_i, aktuellerFeldZeigerFürSemiAutomatik.Pos_j);
                    //SichtbareBewegungDurchgeführt = true;
                }



                if (!SichtbareBewegungDurchgeführt && IstDasSpielfeldLeer())
                {
                    SichtbareBewegungDurchgeführt = true; // NOTaussprung bei keiner sichtbaren bewegeung

                    aktuellerFeldZeigerFürSemiAutomatik.PosLöschen();


                    // errormeldung kein Lebewesen im wasser


                }


                AbarbeitsorderZähler++;

            }


        }



        public void AutomatikRundeMitLebewesen(int i, int j)
        {
            //int[] FeldZwischenmerker = new int[2];
            //int AnzahlFelderZwischenmerker = 0;


            Klassen.PandoraDataStructs.Suchergebnis ErgebnisZwischenMerker = new Klassen.PandoraDataStructs.Suchergebnis();





            if (Feld[i, j].Lebewesen.spielrundenZahl < Global_Rundenzähler)// Wenn die rundenzeit vom lebewesen kleiner ist als die spielrunde
            {

                //
                Feld[i, j].Lebewesen.spielrundenZahl = Global_Rundenzähler;// lebewesen runde ist Rundenzeit
                Feld[i, j].Lebewesen.Alter++;



                if (Feld[i, j].Lebewesen.TypLebewesen == "Fisch")
                {
                    // Fisch bewegen 
                    // Fisch nachrung suchen
                    // Fisch neuen fisch geberen


                    
                    Feld[i, j].Lebewesen.Sättigung -= ParameterSatz.fischParameter.SättigungsVerbrauch;



                    if (Feld[i, j].Lebewesen.Alter > ParameterSatz.fischParameter.AlterSterben || Feld[i, j].Lebewesen.Sättigung < 0)
                    {
                        Feld[i, j].Lebewesen.LöscheLebewesen();
                    }
                    else if ((SucheZufallsFeldNachFaunaInteresse(i, j, "Leer").AnzahlMöglicherFelder > 0 ) && (SucheZufallsFeldNachFaunaInteresse(i, j, "Fisch", true).AnzahlMöglicherFelder > 0) && (Feld[i, j].Lebewesen.Alter > ParameterSatz.fischParameter.AlterFähigFürFortpflanzung) && (Feld[i, j].Lebewesen.NächsteRundeFüreineGeburt  <  Global_Rundenzähler))
                    {


                        




                        // TODO geburtenlogik prüfen // Testreihe starten für den BUG oder abfangstrategie entwickeln







                        Feld[i, j].Lebewesen.NächsteRundeFüreineGeburt = (ParameterSatz.fischParameter.GeburtenWiederholhunginRunden + Global_Rundenzähler);     // setzte Geburten Runde




                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Fisch",true); // suche gepaarten Fisch
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.NächsteRundeFüreineGeburt = (ParameterSatz.fischParameter.GeburtenWiederholhunginRunden + Global_Rundenzähler); // setzte Geburten Runde für gepaarten Fisch
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.spielrundenZahl++;
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.Alter++;


                        if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                        {

                        }







                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Leer");
                        
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.ErzeugeFischImFeld(Global_Rundenzähler);

                        if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                        {

                        }


                    }
                    else if (SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", true).AnzahlMöglicherFelder > 0)
                    {

                        // essen suchen


                        //FeldZwischenmerker = SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", true); // TODO später löschen
                        ErgebnisZwischenMerker = SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", true);

                        // Sättigung erhöhen
                        LebewesenFristZiel(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);

                        LebewesenVonFeldZuFeldWandern(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);

                        if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                        {

                        }
                    }
                    else if (SucheZufallsFeldNachFaunaInteresse(i, j, "Leer").AnzahlMöglicherFelder > 0)
                    {





                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Leer");
                        if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                        {

                        }
                        LebewesenVonFeldZuFeldWandern(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);
                    }
                    else
                    {
                        // nichts möglich ?
                    }


                    
                }
                else if (Feld[i, j].Lebewesen.TypLebewesen == "Hai")
                {
                    // Hai bewegen 
                    // Hai nachrung suchen
                    // Hai neuen Hai geberen


                    Feld[i, j].Lebewesen.Sättigung -= ParameterSatz.haiParameter.SättigungsVerbrauch;



                    if (Feld[i, j].Lebewesen.Alter > ParameterSatz.haiParameter.AlterSterben || Feld[i, j].Lebewesen.Sättigung < 0)
                    {
                        Feld[i, j].Lebewesen.LöscheLebewesen();
                    }
                    else if (SucheZufallsFeldNachFaunaInteresse(i, j, "Leer").AnzahlMöglicherFelder > 0 && SucheZufallsFeldNachFaunaInteresse(i, j, "Hai", true).AnzahlMöglicherFelder > 0 && Feld[i, j].Lebewesen.Alter > ParameterSatz.haiParameter.AlterFähigFürFortpflanzung && (Feld[i, j].Lebewesen.NächsteRundeFüreineGeburt < Global_Rundenzähler))
                    {
                        // TODO geburtenlogik prüfen
                        Feld[i, j].Lebewesen.NächsteRundeFüreineGeburt = (ParameterSatz.haiParameter.GeburtenWiederholhunginRunden + Global_Rundenzähler);     // setzte Geburten Runde


                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Hai", true);  // suche gepaarten Hai
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.NächsteRundeFüreineGeburt = (ParameterSatz.haiParameter.GeburtenWiederholhunginRunden + Global_Rundenzähler); // setzte Geburten Runde für gepaarten Hai
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.spielrundenZahl++;
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.Alter++;


                        if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                        {

                        }


                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Leer");
                        //TODO geburtenfähigkeit wieviele runden ?
                        Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].Lebewesen.ErzeugeHaiImFeld(Global_Rundenzähler);


                    }
                    else if (SucheZufallsFeldNachFaunaInteresse(i, j, "Fisch").AnzahlMöglicherFelder > 0)
                    {
                        // essen suchen



                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Fisch");

                        
                        // TODO testen Sättigung erhöhen
                        LebewesenFristZiel(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);

                        LebewesenVonFeldZuFeldWandern(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);


                    }
                    else if (SucheZufallsFeldNachFaunaInteresse(i, j, "Leer").AnzahlMöglicherFelder > 0)
                    {


                        ErgebnisZwischenMerker = SucheZufallsFeldNachFaunaInteresse(i, j, "Leer");

                        LebewesenVonFeldZuFeldWandern(i, j, ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j);
                    }
                    else
                    {
                        // nichts möglich ?
                    }



                }
                else if (Feld[i, j].Lebewesen.TypLebewesen == "Leer")
                {

                }
                else
                {
                    //Fehler
                }

            }

        }


        public void AutomatikRundeMitPflanze(int i, int j)
        {

            // pflanzenrunde

            //int[] FeldZwischenmerker = new int[2];

            Klassen.PandoraDataStructs.Suchergebnis ErgebnisZwischenMerker = new Klassen.PandoraDataStructs.Suchergebnis();


            if (Feld[i, j].PflanzenRundenZähler < Global_Rundenzähler)// Wenn die rundenzeit vom lebewesen kleiner ist als die spielrunde
            {

                //
                Feld[i, j].PflanzenRundenZähler = Global_Rundenzähler;// lebewesen runde ist Rundenzeit


                // wenn zuviel pflanzen -> eingehen
                // wenn unter oder gleich zuviel pflanzen -> wachsen
                // zufällig ausbreiten oder wachsen ( 1 wachsen 2 ausbreiten 3 beides ??? )


                if (Feld[i, j].Pflanzenbewuchs > 0)
                {

                    if ( SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", false).AnzahlMöglicherFelder > 6)
                    {
                        // TODO ENTFERNEN ODER ÜBERARBEITEN
                        Feld[i, j].LöschePflanzenbewuchs();
                        //
                    }
                    else if (PflanzenbewuchsInUmfeldAuszählen(i, j) > ParameterSatz.pflanzenParameter.maximalerBewuchsImUmfeld)
                    {
                        // reduzieren

                        int Pflanzengröße = würfel.Next(ParameterSatz.pflanzenParameter.minimalWachstum, ParameterSatz.pflanzenParameter.maximalWachstum);

                        Feld[i, j].BewuchsReduzieren(Pflanzengröße);

                    }
                    else if (PflanzenbewuchsInUmfeldAuszählen(i, j) <= ParameterSatz.pflanzenParameter.maximalerBewuchsImUmfeld && SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", false).AnzahlMöglicherFelder < 4)
                    {


                        int zufallsaktion = würfel.Next(1, 4); // 1 wachsen 2 vermehren 3 beides




                        if (zufallsaktion != 2 || (SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", false).AnzahlMöglicherFelder == 8))
                        {
                            // wachsen
                            int Pflanzengröße = würfel.Next(ParameterSatz.pflanzenParameter.minimalWachstum, ParameterSatz.pflanzenParameter.maximalWachstum);


                            Feld[i, j].PflanzenWachsenLassen(Pflanzengröße);


                            if (SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", false).AnzahlMöglicherFelder == 8)
                            {
                                zufallsaktion = 1; // entweder zufall ist 1 oder (3 aber vermehren nicht möglich)
                            }

                        }

                        if (zufallsaktion != 1)
                        {
                            // vermehren



                            int testInt = SucheZufallsFeldNachFloraInteresse(i, j, "Pflanze", false).AnzahlMöglicherFelder;
                            // TODO BUG !!!
                            ErgebnisZwischenMerker = SucheZufallsFeldNachFloraInteresse(i, j, "Leer", false);

                            int Pflanzengröße = würfel.Next(ParameterSatz.pflanzenParameter.minimalWachstum, ParameterSatz.pflanzenParameter.maximalWachstum);

                            Feld[ErgebnisZwischenMerker.ZufallsFeld.Pos_i, ErgebnisZwischenMerker.ZufallsFeld.Pos_j].ErzeugePflanzenbewuchsImFeld(Pflanzengröße);


                            if (ErgebnisZwischenMerker.ErgebnisPlausible == false)
                            {

                            }

                        }


                    }
                    else
                    {
                        // Fehler
                    }


                }

            }

        }


        public void NeueAbarbeitsOrderErstellen()
        {
            int AnzahlFelder = Global_anzahlZellenBreit * Global_anzahlZellenHoch;


            AbarbeitsorderZähler = 0;


            for (int i = 0; i < AnzahlFelder; i++)
            {

                Klassen.PandoraDataStructs.FeldPosition Zufallsfeld = new Klassen.PandoraDataStructs.FeldPosition();
                Zufallsfeld.Pos_i= würfel.Next(0, Global_anzahlZellenBreit );
                Zufallsfeld.Pos_j = würfel.Next(0, Global_anzahlZellenHoch );



                bool PrüfenAufVorhanden = false;

                for (int j = 0; j < AnzahlFelder -1; j++)
                {

                    if (Abarbeitsorder[j].Pos_i == Zufallsfeld.Pos_i && Abarbeitsorder[j].Pos_j == Zufallsfeld.Pos_j)
                    {
                        PrüfenAufVorhanden = true;
                    }



                }

                if (!PrüfenAufVorhanden)
                {
                    Abarbeitsorder[i].Pos_i = Zufallsfeld.Pos_i;
                    Abarbeitsorder[i].Pos_j = Zufallsfeld.Pos_j;
                }
                else
                {
                    i--;
                }


            }


        }


        // ---------------------------------------------------------------------- Sonstige Funktionen

        public int SpielfeldSuche(String gesuchteSache)
        {

            int zähler = 0;


            for (int i = 0; i < Global_anzahlZellenHoch; i++)
            {
                for (int j = 0; j < Global_anzahlZellenBreit; j++)
                {

                    if ((Feld[i, j].Lebewesen.TypLebewesen == gesuchteSache && (gesuchteSache == "Fisch" || gesuchteSache == "Hai")) || (Feld[i, j].Pflanzenbewuchs > 0 && gesuchteSache == "Pflanze"))
                    {
                        zähler++;
                    }

                    // TODO felder zwischen speichern


                }
            }

            return (zähler);
        }

        public bool IstDasSpielfeldLeer()
        {

            if (SpielfeldSuche("Fisch") > 0 || SpielfeldSuche("Hai") > 0 || SpielfeldSuche("Pflanze") > 0)
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }

        public int PflanzenbewuchsInUmfeldAuszählen (int pos_i, int pos_j)
        {


            int GesamtBewuchsZähler = 0;




            int iDarüber = pos_i - 1;
            if (iDarüber < 0)
            { iDarüber = Global_anzahlZellenHoch - 1; }

            int iDarunter = pos_i + 1;
            if (iDarunter >= Global_anzahlZellenHoch)
            { iDarunter = 0; }

            int jLinks = pos_j - 1;
            if (jLinks < 0)
            { jLinks = Global_anzahlZellenBreit - 1; }

            int jRechts = pos_j + 1;
            if (jRechts >= Global_anzahlZellenBreit)
            { jRechts = 0; }



            GesamtBewuchsZähler = 
                Feld[iDarüber, jLinks].Pflanzenbewuchs +
                Feld[iDarüber, pos_j].Pflanzenbewuchs +
                Feld[iDarüber, jRechts].Pflanzenbewuchs +
                Feld[pos_i, jRechts].Pflanzenbewuchs +
                Feld[pos_i, jLinks].Pflanzenbewuchs +
                Feld[iDarunter, jLinks].Pflanzenbewuchs +
                Feld[iDarunter, pos_j].Pflanzenbewuchs +
                Feld[iDarunter, jRechts].Pflanzenbewuchs;



            return GesamtBewuchsZähler;
        }



        // ---------------------------------------------------------------------- Variablenrückgabe Funktionen

        public int AusgabeZellenBreit()
        {
            return (Global_anzahlZellenBreit);
        }

        public int AusgabeZellenHoch()
        {
            return (Global_anzahlZellenHoch);
        }



        // ---------------------------------------------------------------------- Pandora ENDE
    }
}
