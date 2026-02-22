using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace PandoraWorld_CS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public float LifeScaleOfField { get; set; } = 0.4f;


        public class Utils
        {
            public static Brush GetBrush(string resourceKey, Brush fallback)
            {
                var res = Application.Current.TryFindResource(resourceKey) as Brush;
                return res ?? fallback;
            }
        }



    }

}
