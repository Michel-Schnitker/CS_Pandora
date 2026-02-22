using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PandoraWorld_CS.Pandora;

namespace PandoraWorld_CS.GUI
{
    public partial class StatistikPage : Page
    {
        private Statistics? _statistics;
        private MainWindow? _mainWindow;
        private int _displayCount = 100;

        public StatistikPage()
        {
            InitializeComponent();
            Loaded += StatistikPage_Loaded;
            Unloaded += StatistikPage_Unloaded;
            SizeChanged += StatistikPage_SizeChanged;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void StatistikPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get statistics from MainWindow and subscribe to events
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                _mainWindow = mainWindow;
                _statistics = mainWindow.GameStatistics;
                _mainWindow.RoundCompleted += OnRoundCompleted;
            }

            RefreshDisplay();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void StatistikPage_Unloaded(object sender, RoutedEventArgs e)
        {
            // Unsubscribe to prevent memory leaks
            if (_mainWindow is not null)
            {
                _mainWindow.RoundCompleted -= OnRoundCompleted;
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void OnRoundCompleted(object? sender, EventArgs e)
        {
            RefreshDisplay();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void StatistikPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshChart();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void CmbHistoryCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbHistoryCount.SelectedItem is ComboBoxItem item && 
                int.TryParse(item.Content?.ToString(), out int count))
            {
                _displayCount = count;
                RefreshChart();
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void RefreshDisplay()
        {
            UpdateCurrentStats();
            RefreshChart();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void UpdateCurrentStats()
        {
            if (_statistics is null)
                return;

            var latest = _statistics.GetLatest();
            if (latest is null)
            {
                TxtRound.Text = "0";
                TxtFishCount.Text = "0";
                TxtSharkCount.Text = "0";
                TxtPlantCount.Text = "0";
                return;
            }

            TxtRound.Text = latest.Value.RoundNumber.ToString("N0");
            TxtFishCount.Text = latest.Value.FishCount.ToString("N0");
            TxtSharkCount.Text = latest.Value.SharkCount.ToString("N0");
            TxtPlantCount.Text = latest.Value.PlantCellCount.ToString("N0");
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void RefreshChart()
        {
            if (_statistics is null || ChartCanvas is null)
                return;

            ChartCanvas.Children.Clear();
            YAxisLabels.Children.Clear();

            var data = _statistics.GetLast(_displayCount).ToList();
            if (data.Count < 2)
            {
                DrawNoDataMessage();
                return;
            }

            double width = ChartCanvas.ActualWidth;
            double height = ChartCanvas.ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            // Calculate max value for scaling
            int maxValue = data.Max(d => Math.Max(d.FishCount, Math.Max(d.SharkCount, d.PlantCellCount)));
            maxValue = Math.Max(maxValue, 10); // Minimum scale

            // Add some padding to max
            maxValue = (int)(maxValue * 1.1);

            // Draw grid lines and Y-axis labels
            DrawGridLines(width, height, maxValue);

            // Draw data lines
            DrawDataLine(data, width, height, maxValue, d => d.FishCount, 
                (Brush)FindResource("BoardBlue"), 2);
            DrawDataLine(data, width, height, maxValue, d => d.SharkCount, 
                (Brush)FindResource("BoardRed"), 2);
            DrawDataLine(data, width, height, maxValue, d => d.PlantCellCount, 
                (Brush)FindResource("BoardGreen"), 2);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void DrawNoDataMessage()
        {
            var text = new TextBlock
            {
                Text = "Keine Daten vorhanden.\nStarten Sie das Spiel, um Statistiken zu sammeln.",
                Foreground = Brushes.LightGray,
                FontSize = 16,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Canvas.SetLeft(text, ChartCanvas.ActualWidth / 2 - 150);
            Canvas.SetTop(text, ChartCanvas.ActualHeight / 2 - 20);
            ChartCanvas.Children.Add(text);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void DrawGridLines(double width, double height, int maxValue)
        {
            int gridLines = 5;
            
            for (int i = 0; i <= gridLines; i++)
            {
                double y = height - (height * i / gridLines);
                int labelValue = maxValue * i / gridLines;

                // Grid line
                var line = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = width,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255)),
                    StrokeThickness = 1
                };
                ChartCanvas.Children.Add(line);

                // Y-axis label
                var label = new TextBlock
                {
                    Text = labelValue.ToString("N0"),
                    Foreground = Brushes.LightGray,
                    FontSize = 10,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Width = 40,
                    TextAlignment = TextAlignment.Right
                };
                
                // Position label relative to grid
                double labelY = (height * (gridLines - i) / gridLines) - 8;
                label.Margin = new Thickness(0, Math.Max(0, labelY), 0, 0);
                YAxisLabels.Children.Add(label);
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void DrawDataLine(
            List<RoundStatistics> data, 
            double width, 
            double height, 
            int maxValue,
            Func<RoundStatistics, int> valueSelector,
            Brush stroke,
            double strokeThickness)
        {
            if (data.Count < 2)
                return;

            var polyline = new Polyline
            {
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                StrokeLineJoin = PenLineJoin.Round
            };

            double xStep = width / (data.Count - 1);

            for (int i = 0; i < data.Count; i++)
            {
                double x = i * xStep;
                double value = valueSelector(data[i]);
                double y = height - (value / maxValue * height);

                polyline.Points.Add(new Point(x, y));
            }

            ChartCanvas.Children.Add(polyline);
        }
    }
}