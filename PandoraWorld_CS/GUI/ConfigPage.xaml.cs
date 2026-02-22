using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PandoraWorld_CS.Pandora;

namespace PandoraWorld_CS.GUI
{
    public partial class ConfigPage : Page
    {
        private LifeFormConfig? _currentLifeFormConfig;

        public ConfigPage()
        {
            InitializeComponent();
            LoadAllValues();
        }

        #region Load Values
        
        /// <summary>
        /// TODO:
        /// </summary>
        private void LoadAllValues()
        {
            LoadPercentages();
            LoadTimerConfig();
            LoadPlantConfig();
            LoadLifeFormConfig();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void LoadPercentages()
        {
            TxtFishPercentage.Text = GameConfig.FishPercentage.ToString();
            TxtSharkPercentage.Text = GameConfig.SharkPercentage.ToString();
            TxtPlantPercentage.Text = GameConfig.PlantPercentage.ToString();
            UpdatePercentageSum();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void LoadTimerConfig()
        {
            TxtGameTimerInterval.Text = GameConfig.GameTimerIntervalMs.ToString();
            TxtStepTimerInterval.Text = GameConfig.StepTimerIntervalMs.ToString();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void LoadPlantConfig()
        {
            TxtPlantGrowthMin.Text = GameConfig.PlantGrowthMin.ToString();
            TxtPlantGrowthMax.Text = GameConfig.PlantGrowthMax.ToString();
            TxtPlantMaxSize.Text = GameConfig.PlantMaxSize.ToString();
            TxtPlantSpreadThreshold.Text = GameConfig.PlantSpreadThreshold.ToString();
            TxtPlantSurroundedMin.Text = GameConfig.PlantSurroundedMinNeighbors.ToString();
            TxtPlantShrinkMin.Text = GameConfig.PlantShrinkMin.ToString();
            TxtPlantShrinkMax.Text = GameConfig.PlantShrinkMax.ToString();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void LoadLifeFormConfig()
        {
            if (TxtMaxAge == null) return; // Controls not yet initialized
            
            bool isFish = CmbLifeForm.SelectedIndex == 0;
            _currentLifeFormConfig = isFish ? GameConfig.FishConfig : GameConfig.SharkConfig;

            TxtMaxAge.Text = _currentLifeFormConfig.MaxAge.ToString();
            TxtMaturityAge.Text = _currentLifeFormConfig.MaturityAge.ToString();
            TxtBreedingCooldown.Text = _currentLifeFormConfig.BreedingCooldown.ToString();
            TxtEatAmountMin.Text = _currentLifeFormConfig.EatAmountMin.ToString();
            TxtEatAmountMax.Text = _currentLifeFormConfig.EatAmountMax.ToString();
            TxtNutrientValue.Text = _currentLifeFormConfig.NutrientValue.ToString("F2", CultureInfo.InvariantCulture);
            TxtEnergyLossPerRound.Text = _currentLifeFormConfig.EnergyLossPerRound.ToString();

            // Update info text
            if (isFish)
            {
                TxtLifeFormInfo.Text = "Fish fressen Pflanzen. Essen Min/Max bestimmt die Pflanzenmenge pro Mahlzeit.";
                TxtEatAmountMin.IsEnabled = true;
                TxtEatAmountMax.IsEnabled = true;
            }
            else
            {
                TxtLifeFormInfo.Text = "Sharks fressen Fish. Die Energie des Fish wird mit dem Nährwert multipliziert.";
                TxtEatAmountMin.IsEnabled = false;
                TxtEatAmountMax.IsEnabled = false;
            }
        }

        #endregion

        #region Apply Values

        /// <summary>
        /// TODO:
        /// </summary>
        private void ApplyAllValues()
        {
            try
            {
                ApplyPercentages();
                ApplyTimerConfig();
                ApplyPlantConfig();
                ApplyLifeFormConfig();

                GameConfig.SetGameConfiguration();

                // Update MainWindow timers
                UpdateMainWindowTimers();

                MessageBox.Show("Konfiguration erfolgreich angewendet!", "Erfolg", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Anwenden der Konfiguration:\n{ex.Message}", "Fehler",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void ApplyPercentages()
        {
            GameConfig.FishPercentage = ParseInt(TxtFishPercentage.Text, nameof(GameConfig.FishPercentage));
            GameConfig.SharkPercentage = ParseInt(TxtSharkPercentage.Text, nameof(GameConfig.SharkPercentage));
            GameConfig.PlantPercentage = ParseInt(TxtPlantPercentage.Text, nameof(GameConfig.PlantPercentage));
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void ApplyTimerConfig()
        {
            int gameTimer = ParseInt(TxtGameTimerInterval.Text, "Spiel-Timer");
            int stepTimer = ParseInt(TxtStepTimerInterval.Text, "Schritt-Timer");

            if (gameTimer < 10)
                throw new ArgumentException("Spiel-Timer muss mindestens 10ms sein.");
            if (stepTimer < 10)
                throw new ArgumentException("Schritt-Timer muss mindestens 10ms sein.");

            GameConfig.GameTimerIntervalMs = gameTimer;
            GameConfig.StepTimerIntervalMs = stepTimer;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void ApplyPlantConfig()
        {
            GameConfig.PlantGrowthMin = ParseInt(TxtPlantGrowthMin.Text, nameof(GameConfig.PlantGrowthMin));
            GameConfig.PlantGrowthMax = ParseInt(TxtPlantGrowthMax.Text, nameof(GameConfig.PlantGrowthMax));
            GameConfig.PlantMaxSize = ParseInt(TxtPlantMaxSize.Text, nameof(GameConfig.PlantMaxSize));
            GameConfig.PlantSpreadThreshold = ParseInt(TxtPlantSpreadThreshold.Text, nameof(GameConfig.PlantSpreadThreshold));
            GameConfig.PlantSurroundedMinNeighbors = ParseInt(TxtPlantSurroundedMin.Text, nameof(GameConfig.PlantSurroundedMinNeighbors));
            GameConfig.PlantShrinkMin = ParseInt(TxtPlantShrinkMin.Text, nameof(GameConfig.PlantShrinkMin));
            GameConfig.PlantShrinkMax = ParseInt(TxtPlantShrinkMax.Text, nameof(GameConfig.PlantShrinkMax));
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void ApplyLifeFormConfig()
        {
            if (_currentLifeFormConfig is null)
                return;

            _currentLifeFormConfig.MaxAge = ParseInt(TxtMaxAge.Text, "Max Alter");
            _currentLifeFormConfig.MaturityAge = ParseInt(TxtMaturityAge.Text, "Geschlechtsreife");
            _currentLifeFormConfig.BreedingCooldown = ParseInt(TxtBreedingCooldown.Text, "Paarungs-Cooldown");
            _currentLifeFormConfig.EatAmountMin = ParseInt(TxtEatAmountMin.Text, "Essen Min");
            _currentLifeFormConfig.EatAmountMax = ParseInt(TxtEatAmountMax.Text, "Essen Max");
            _currentLifeFormConfig.NutrientValue = ParseDouble(TxtNutrientValue.Text, "Nährwert");
            _currentLifeFormConfig.EnergyLossPerRound = ParseInt(TxtEnergyLossPerRound.Text, "Energie-Verlust");
        }

        /// <summary>
        /// Updates the timer intervals in the MainWindow.
        /// </summary>
        private void UpdateMainWindowTimers()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateTimerIntervals(
                    GameConfig.GameTimerIntervalMs,
                    GameConfig.StepTimerIntervalMs);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// TODO:
        /// </summary>
        private static int ParseInt(string text, string fieldName)
        {
            if (int.TryParse(text, out int result))
                return result;
            throw new FormatException($"'{fieldName}' muss eine ganze Zahl sein.");
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private static double ParseDouble(string text, string fieldName)
        {
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                return result;
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
                return result;
            throw new FormatException($"'{fieldName}' muss eine Dezimalzahl sein.");
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void UpdatePercentageSum()
        {
            int.TryParse(TxtFishPercentage.Text, out int fish);
            int.TryParse(TxtSharkPercentage.Text, out int shark);
            int.TryParse(TxtPlantPercentage.Text, out int plant);

            int sum = fish + shark + plant;
            TxtPercentageSum.Text = $"Summe: {sum}%";
            TxtPercentageSum.Foreground = sum > 100 
                ? new SolidColorBrush(Colors.Red) 
                : new SolidColorBrush(Colors.Gray);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// TODO:
        /// </summary>
        private void CmbLifeForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Save current config before switching
            if (_currentLifeFormConfig is not null && IsLoaded)
            {
                try
                {
                    ApplyLifeFormConfig();
                }
                catch
                {
                    // Ignore validation errors during switch
                }
            }

            LoadLifeFormConfig();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyAllValues();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset to default values
            GameConfig.FishPercentage = 13;
            GameConfig.SharkPercentage = 2;
            GameConfig.PlantPercentage = 30;

            GameConfig.GameTimerIntervalMs = 100;
            GameConfig.StepTimerIntervalMs = 50;

            GameConfig.PlantGrowthMin = 50;
            GameConfig.PlantGrowthMax = 200;
            GameConfig.PlantMaxSize = 5000;
            GameConfig.PlantSpreadThreshold = 2000;
            GameConfig.PlantSurroundedMinNeighbors = 6;
            GameConfig.PlantShrinkMin = 50;
            GameConfig.PlantShrinkMax = 150;

            GameConfig.FishConfig = new LifeFormConfig(
                maxAge: 50,
                maturityAge: 5,
                breedingCooldown: 3,
                eatAmountMin: 50,
                eatAmountMax: 150,
                nutrientValue: 0.05,
                energyLossPerRound: 3
            );

            GameConfig.SharkConfig = new LifeFormConfig(
                maxAge: 80,
                maturityAge: 10,
                breedingCooldown: 8,
                eatAmountMin: 0,
                eatAmountMax: 0,
                nutrientValue: 0.5,
                energyLossPerRound: 5
            );

            LoadAllValues();
            UpdateMainWindowTimers();

            MessageBox.Show("Konfiguration auf Standardwerte zurückgesetzt!", "Zurückgesetzt",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}