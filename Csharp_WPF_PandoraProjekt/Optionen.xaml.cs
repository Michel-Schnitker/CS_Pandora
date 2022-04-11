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
using System.Windows.Shapes;

namespace Csharp_WPF_PandoraProjekt
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            prozentanzeigenfüllen();

        }

        private void saveOptionen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseOptionen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HeufigkeitWasser_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prozentanzeigenfüllen();
        }

        private void HeufigkeitPlankton_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prozentanzeigenfüllen();
        }

        private void HeufigkeitFische_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prozentanzeigenfüllen();
        }

        private void HeufigkeitHaie_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            prozentanzeigenfüllen();
        }

        private void prozentanzeigenfüllen()
        {

            //anzeigeProzentWasser.Content = Math.Round(HeufigkeitWasser.Value, 0);

            if (HeufigkeitWasser.IsInitialized && HeufigkeitPlankton.IsInitialized && HeufigkeitFische.IsInitialized && HeufigkeitHaie.IsInitialized)
            {



                int gesamtmenge = Convert.ToInt32(HeufigkeitWasser.Value + HeufigkeitPlankton.Value + HeufigkeitFische.Value + HeufigkeitHaie.Value);
                if (gesamtmenge > 0)
                {
                    anzeigeProzentWasser.Content = Convert.ToInt32(HeufigkeitWasser.Value / gesamtmenge * 100) + " %";
                    anzeigeProzentPlankton.Content = Convert.ToInt32(HeufigkeitPlankton.Value / gesamtmenge * 100) + " %";
                    anzeigeProzentFische.Content = Convert.ToInt32(HeufigkeitFische.Value / gesamtmenge * 100) + " %";
                    anzeigeProzentHaie.Content = Convert.ToInt32(HeufigkeitHaie.Value / gesamtmenge * 100) + " %";
                }

            }
        }





    }
}
