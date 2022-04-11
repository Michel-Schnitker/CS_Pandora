
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;




namespace Csharp_WPF_PandoraProjekt
{

    public partial class MainWindow : Window
    {
        // TODO Startseite erzeugen
        // TODO ungleiche zahlen in Breit und höhe möglich machen
        const int anzahlZellenBreit = 60;
        const int anzahlZellenHoch = 60;

        Rectangle[,] felderGrund = new Rectangle[anzahlZellenBreit, anzahlZellenHoch];       // Felder Array für Visio Grund
        Rectangle[,] felderWasser = new Rectangle[anzahlZellenBreit, anzahlZellenHoch];       // Felder Array für Visio Wasser                                                                    

        Klassen.Pandora Pandora = new Klassen.Pandora();


        DispatcherTimer timer = new DispatcherTimer();

        Klassen.Parameter TempSpielParameter = new Klassen.Parameter();

        Klassen.PandoraStatistics SpielStatistik = new Klassen.PandoraStatistics();


        // TODO : Datenkapseln
        int statAnzeigenZähler = 0;


        public MainWindow()
        {
            InitializeComponent();

            Pandora.PandoraInitialisieren(anzahlZellenBreit, anzahlZellenHoch); // Spielfeldgröße übertragen und Spiel erstellen

            playground.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            playground.Arrange(new Rect(0.0, 0.0, playground.DesiredSize.Width, playground.DesiredSize.Height));

            // erzeuge Grafische Felder für Grund -> ( Wasser, Pflanzen )
            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = playground.ActualWidth / anzahlZellenBreit + 1 /*-2*/ ;
                    r.Height = playground.ActualHeight / anzahlZellenHoch + 1 /*-2*/  ;

                    playground.Children.Add(r);
                    Canvas.SetLeft(r, j * playground.ActualWidth / anzahlZellenBreit);
                    Canvas.SetTop(r, i * playground.ActualHeight / anzahlZellenHoch);

                    felderGrund[i, j] = r;


                }
            }

            // erzeuge Grafische Felder für Wasser -> ( Fische, Haie, Wasser )
            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = playground.ActualWidth / anzahlZellenBreit + 1 -5 ;
                    r.Height = playground.ActualHeight / anzahlZellenHoch + 1 -5  ;

                    playground.Children.Add(r);
                    Canvas.SetLeft(r, j * playground.ActualWidth / anzahlZellenBreit + 3);
                    Canvas.SetTop(r, i * playground.ActualHeight / anzahlZellenHoch + 3);

                    felderWasser[i, j] = r;


                }
            }


            TranslateStatsToPlayBoard();

            Prozentanzeigenfüllen();

            timer.Interval = TimeSpan.FromSeconds(0.15);
            timer.Tick += Timer_Tick;


            StartStop.IsEnabled = true;


            SpielStatistik.ringspeicher.InitialRundenRingspeicher(40);


        }

        // ---------------------------------------------------------------------- Buttons

        private void StartStop_Click(object sender, RoutedEventArgs e)
        {
            Pandora.ParameterSatz.ÜberschreibenDerParameter(TempSpielParameter);

            AutoStartStop();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {

            Pandora.GanzesSpielFeldLöschen();
            
            

            StatPanelFüllen();
            TranslateStatsToPlayBoard();
        }

        private void FillRandom_Click(object sender, RoutedEventArgs e)
        {

            //Füllen der ProzentWerte
            Pandora.ParameterSatz.ÜberschreibenDerParameter(TempSpielParameter);

            

            Pandora.GanzesSpielfeldZufälligFüllen();


            StatPanelFüllen();
            TranslateStatsToPlayBoard();


            
        }

        // ---------------------------------------------------------------------- slider

        private void HeufigkeitWasser_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Prozentanzeigenfüllen();
        }

        private void HeufigkeitPlankton_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Prozentanzeigenfüllen();
        }

        private void HeufigkeitFische_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Prozentanzeigenfüllen();
        }

        private void HeufigkeitHaie_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Prozentanzeigenfüllen();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


            testfeld1.Content = Math.Round(slider1.Value, 2) + " s";

            timer.Interval = TimeSpan.FromSeconds(slider1.Value);


        }

        private void FischAlter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


            if (FischAlter_label.IsInitialized)
            {
                TempSpielParameter.fischParameter.AlterFähigFürFortpflanzung = Convert.ToInt32(FischAlter.Value);
                FischAlter_label.Content = TempSpielParameter.fischParameter.AlterFähigFürFortpflanzung;
            }
            
        }

        private void FischMaxAlter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.fischParameter.AlterSterben = Convert.ToInt32(FischMaxAlter.Value);

            if (FischMaxAlter_label.IsInitialized)
            {
                FischMaxAlter_label.Content = TempSpielParameter.fischParameter.AlterSterben;
            }
            
        }

        private void FischGeburtenWied_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.fischParameter.GeburtenWiederholhunginRunden = Convert.ToInt32(FischGeburtenWied.Value);

            if (FischGeburtenWied_label.IsInitialized)
            {
                FischGeburtenWied_label.Content = TempSpielParameter.fischParameter.GeburtenWiederholhunginRunden;
            }
            
        }

        private void FischSättigungEssen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.fischParameter.SättigungBeimEssen = Convert.ToInt32(FischSättigungEssen.Value);

            if (FischSättigungEssen_label.IsInitialized)
            {
                FischSättigungEssen_label.Content = TempSpielParameter.fischParameter.SättigungBeimEssen;
            }
            
        }

        private void FischSättigungVerbrauch_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.fischParameter.SättigungsVerbrauch = Convert.ToInt32(FischSättigungVerbrauch.Value);

            if (FischSättigungVerbrauch_label.IsInitialized)
            {
                FischSättigungVerbrauch_label.Content = TempSpielParameter.fischParameter.SättigungsVerbrauch;
            }
            
        }

        
        private void HaieAlter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.haiParameter.AlterFähigFürFortpflanzung = Convert.ToInt32(HaieAlter.Value);

            if (HaieAlter_label.IsInitialized)
            {
                HaieAlter_label.Content = TempSpielParameter.haiParameter.AlterFähigFürFortpflanzung;
            }
            
        }

        private void HaiMaxAlter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.haiParameter.AlterSterben = Convert.ToInt32(HaiMaxAlter.Value);

            if (HaiMaxAlter_label.IsInitialized)
            {
                HaiMaxAlter_label.Content = TempSpielParameter.haiParameter.AlterSterben;
            }
            
        }

        private void HaiGeburtenWied_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.haiParameter.GeburtenWiederholhunginRunden = Convert.ToInt32(HaiGeburtenWied.Value);

            if (HaiGeburtenWied_label.IsInitialized)
            {
                HaiGeburtenWied_label.Content = TempSpielParameter.haiParameter.GeburtenWiederholhunginRunden;
            }
            
        }

        private void HaiSättigungEssen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.haiParameter.SättigungBeimEssen = Convert.ToInt32(HaiSättigungEssen.Value);

            if (HaiSättigungEssen_label.IsInitialized)
            {
                HaiSättigungEssen_label.Content = TempSpielParameter.haiParameter.SättigungBeimEssen;
            }
            
        }

        private void HaiSättigungVerbrauch_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.haiParameter.SättigungsVerbrauch = Convert.ToInt32(HaiSättigungVerbrauch.Value);

            if (HaiSättigungVerbrauch_label.IsInitialized)
            {
                HaiSättigungVerbrauch_label.Content = TempSpielParameter.haiParameter.SättigungsVerbrauch;
            }
            
        }


        private void PflanzeMinWachstum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.pflanzenParameter.minimalWachstum = Convert.ToInt32(PflanzeMinWachstum.Value);

            if (PflanzeMinWachstum_label.IsInitialized)
            {
                PflanzeMinWachstum_label.Content = TempSpielParameter.pflanzenParameter.minimalWachstum;
            }
        }

        private void PflanzeMaxWachstum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.pflanzenParameter.maximalWachstum = Convert.ToInt32(PflanzeMaxWachstum.Value);

            if (PflanzeMaxWachstum_label.IsInitialized)
            {
                PflanzeMaxWachstum_label.Content = TempSpielParameter.pflanzenParameter.maximalWachstum;
            }
        }

        private void PflanzeMaxBewuchs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TempSpielParameter.pflanzenParameter.maximalerBewuchsImUmfeld = Convert.ToInt32(PflanzeMaxBewuchs.Value);

            if (PflanzeMaxBewuchs_label.IsInitialized)
            {
                PflanzeMaxBewuchs_label.Content = TempSpielParameter.pflanzenParameter.maximalerBewuchsImUmfeld;
            }
        }


        // ---------------------------------------------------------------------- Mouse Event

        private void playground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (timer.IsEnabled == false)
            {

                // TODO: Funktion optimieren und analysieren
                System.Windows.Point position = e.GetPosition(this);
                int Y = Convert.ToInt32((position.X - playground.Margin.Left) / (playground.ActualWidth / anzahlZellenBreit));
                int X = Convert.ToInt32((position.Y - playground.Margin.Top) / (playground.ActualHeight / anzahlZellenHoch));


                // TODO PFlanzen erzeugen Manuell

                if (Pandora.Feld[X, Y].Lebewesen.TypLebewesen == "Leer" && Pandora.Feld[X, Y].Pflanzenbewuchs > 0)
                {
                    // Wenn Pflanze dann lösche Feld
                    Pandora.Feld[X, Y].LöscheFeld();
                }
                else if (Pandora.Feld[X, Y].Lebewesen.TypLebewesen == "Leer")
                {
                    // Wenn leer dann erzeuge Fisch
                    Pandora.Feld[X, Y].Lebewesen.ErzeugeFischImFeld(0);
                }
                else if (Pandora.Feld[X, Y].Lebewesen.TypLebewesen == "Fisch")
                {
                    // Wenn Fisch dann erzeuge Hai
                    Pandora.Feld[X, Y].Lebewesen.ErzeugeHaiImFeld(0);
                }
                else if (Pandora.Feld[X, Y].Lebewesen.TypLebewesen == "Hai")
                {
                    // Wenn Hai dann erzeuge Pflanzen

                    Pandora.Feld[X, Y].Lebewesen.LöscheLebewesen();

                    //TODO Randomzahl einfügen für pflanzen
                    Pandora.Feld[X, Y].ErzeugePflanzenbewuchsImFeld(10);

                }

                TranslateStatsToPlayBoard();


            }
            
        }

        private void playground_MouseMove(object sender, MouseEventArgs e)
        {

            //TODO: Funktion optimieren und analysieren
            System.Windows.Point position = e.GetPosition(this);
            int Y = Convert.ToInt32((position.X - playground.Margin.Left) / (playground.ActualWidth / anzahlZellenBreit));
            int X = Convert.ToInt32((position.Y - playground.Margin.Top) / (playground.ActualHeight / anzahlZellenHoch));

            if (X > anzahlZellenHoch - 1)
            {
                X = anzahlZellenHoch - 1;
            }
            if (X < 0)
            {
                X = 0;
            }
            if (Y > anzahlZellenBreit - 1)
            {
                Y = anzahlZellenBreit - 1;
            }
            if (Y < 0)
            {
                Y = 0;
            }


            Focus_XAchse.Content = X;
            Focus_YAchse.Content = Y;


            Focus_Typ.Content = Pandora.Feld[X, Y].Lebewesen.TypLebewesen;
            Focus_Alter.Content = Pandora.Feld[X, Y].Lebewesen.Alter;
            Focus_Sättigung.Content = Pandora.Feld[X, Y].Lebewesen.Sättigung;
            Focus_Runde.Content = Pandora.Feld[X, Y].Lebewesen.spielrundenZahl;

            Focus_PfalnzenRunde.Content = Pandora.Feld[X, Y].PflanzenRundenZähler;
            Focus_Pfalnzenbewuchs.Content = Pandora.Feld[X, Y].Pflanzenbewuchs;

            felderWasser[X, Y].Fill = Brushes.Gray;
            //felderGrund[X, Y].Fill = Brushes.Gray;

        }

        // ---------------------------------------------------------------------- Automatik Routine

        void Timer_Tick(object sender, EventArgs e)
        {

            Pandora.AutomatikRundeMitAllenFeldern();
            //Pandora.SemiAutomatikRundeMitAllenFeldern();
            //Pandora.TESTAutomatikRundeMitAllenFeldern();
            
            TranslateStatsToPlayBoard();

            StatPanelFüllen();

            SpielStatistik.ringspeicher.WertInRingSpeicherÜbernehmen(SpielStatistik.AktuellStatisticZähler);

            // TODO option für semimanuell einfügen

            //AutoStartStop("stop");

        }

        // ---------------------------------------------------------------------- Funktions

        void AutoStartStop (string anweisung = "toggle")
        {
            if (anweisung == "toggle")  // bei "toggle" bestimmen was ausgeführt werden soll
            {
                if (timer.IsEnabled)
                {

                    anweisung = "stop";
                    
                }
                else
                {

                    anweisung = "start";

                }
            }

            if (anweisung == "start")
            {
                timer.Start();
                StartStop.Content = "Stop";

                Clean.IsEnabled = false;
                FillRandom.IsEnabled = false;
            }
            else if (anweisung == "stop")
            {
                timer.Stop();
                StartStop.Content = "Start";

                Clean.IsEnabled = true;
                FillRandom.IsEnabled = true;

                statAnzeigenZähler = statAnzeigenZähler + 100;

            }
            else
            {
                //Error
            }
            
        }


        public void TranslateStatsToPlayBoard()
        {
            
            for (int i = 0; i < anzahlZellenHoch; i++)
            {
                for (int j = 0; j < anzahlZellenBreit; j++)
                {


                    if (Pandora.Feld[i, j].Pflanzenbewuchs == 0)
                    {
                        felderGrund[i, j].Fill = Brushes.DarkBlue;
                    }
                    else if (Pandora.Feld[i, j].Pflanzenbewuchs > 0 && Pandora.Feld[i, j].Pflanzenbewuchs < 100)
                    {
                        felderGrund[i, j].Fill = Brushes.DarkGreen;
                    }
                    else if (Pandora.Feld[i, j].Pflanzenbewuchs >= 100 )
                    {
                        felderGrund[i, j].Fill = Brushes.DarkGreen;
                    }


                    if (Pandora.Feld[i, j].Lebewesen.TypLebewesen == "Leer")
                    {
                        felderWasser[i, j].Fill = felderGrund[i, j].Fill;
                    }
                    else if (Pandora.Feld[i, j].Lebewesen.TypLebewesen == "Fisch")
                    {
                        felderWasser[i, j].Fill = Brushes.Orange;
                    }
                    else if (Pandora.Feld[i, j].Lebewesen.TypLebewesen == "Hai")
                    {
                        felderWasser[i, j].Fill = Brushes.Red;
                    }


                    if (Pandora.Feld[i, j].Lebewesen.TypLebewesen == "" || Pandora.Feld[i, j].Pflanzenbewuchs < 0)
                    {
                        felderWasser[i, j].Fill = Brushes.Gray;
                        felderGrund[i, j].Fill = Brushes.Gray;
                    }


                }
            }

        }

        public void StatPanelFüllen()
        {

            SpielStatistik.FeldAuszählen(Pandora);

            Stat_Plankton.Content = SpielStatistik.AktuellStatisticZähler.Pflanzen;
            Stat_Fische.Content = SpielStatistik.AktuellStatisticZähler.Fische;
            Stat_Haie.Content = SpielStatistik.AktuellStatisticZähler.Haie;

            Stat_Runde.Content = SpielStatistik.StatisticRundenzähler;

        }


        private void Prozentanzeigenfüllen()
        {
            
            //anzeigeProzentWasser.Content = Math.Round(HeufigkeitWasser.Value, 0);

            if (HeufigkeitWasser.IsInitialized && HeufigkeitPlankton.IsInitialized && HeufigkeitFische.IsInitialized && HeufigkeitHaie.IsInitialized)
            {

                int SliderFische = Convert.ToInt32(HeufigkeitFische.Value);
                int SliderHaie = Convert.ToInt32(HeufigkeitHaie.Value);
                int SliderPflanze = Convert.ToInt32(HeufigkeitPlankton.Value);
                int SliderWasser = Convert.ToInt32(HeufigkeitWasser.Value);

                int gesamtmenge = (SliderWasser + SliderPflanze + SliderFische + SliderHaie);

                if (gesamtmenge > 0)
                {

                    TempSpielParameter.ProzentGenerierung.ProzentFische = ((SliderFische * 100) / gesamtmenge );
                    TempSpielParameter.ProzentGenerierung.ProzentHaie = ((SliderHaie * 100) / gesamtmenge );
                    TempSpielParameter.ProzentGenerierung.ProzentPflanzen = ((SliderPflanze * 100) / gesamtmenge );
                    TempSpielParameter.ProzentGenerierung.ProzentWasser = ((SliderWasser * 100) / gesamtmenge );

                    anzeigeProzentFische.Content = TempSpielParameter.ProzentGenerierung.ProzentFische + " %";
                    anzeigeProzentHaie.Content = TempSpielParameter.ProzentGenerierung.ProzentHaie + " %";
                    anzeigeProzentPlankton.Content = TempSpielParameter.ProzentGenerierung.ProzentPflanzen + " %";
                    anzeigeProzentWasser.Content = TempSpielParameter.ProzentGenerierung.ProzentWasser + " %";

                }

            }
            
        }


        private void ShowOptionen_Click_1(object sender, RoutedEventArgs e)
        {
            Window1 optiFenster = new Window1();

            optiFenster.ShowDialog();
        }


        // ------------------------------------------------------------------------------ PRIO 1

        // TODO Errormeldungen einbauen ( Möglichkeit die Aktionen über die Console mit zu schreiben ?!=?!=!= )

        // TODO : Statistik erweitern für operationen ( fisch gestorben , hai frisst Fisch )

        // TODO : Extra Form für Statistik möglich machen
        // TODO : Test felder für ringspeicher einbauen
        
        // TODO : zugriffsmodifizierer einbauen // Datenkappslung prüfen 

        // TODO : anzeige erweitern für das Feld ( Pflanzen werte )
        // TODO option für semimanuell einfügen

        // TODO Automatikrunden zusammen fassen zu einer Funktion mit Parametern zusätzlich mit Random möglich machen 

        // ------------------------------------------------------------------------------ PRIO 2

        // TODO : von i und j auf XY umrüsten ???
        

        // ------------------------------------------------------------------------------ PRIO 3

        //TODO Statistik exportieren möglich machen




        //TODO menü überarbeiten // da init bereits beim erstellen kann die stat bedingung überarbeitet werden
        //TODO Optionenfenster hinzufügen
        //TODO Optionen speicherbar machen
        //TODO Menüführung
        //TODO Statistikfenster hinzufügen


    }
}
