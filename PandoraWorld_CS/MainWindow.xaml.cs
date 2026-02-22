using PandoraWorld_CS.GUI;
using PandoraWorld_CS.Pandora;
using PandoraWorld_CS.Pandora.Board;
using System.Collections.Generic;
using System.Text;
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

namespace PandoraWorld_CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isRunning = false;
        private GameBoard? _gameBoard;
        private GameBoardPage? _gameBoardPage;
        private DispatcherTimer _gameTimer;
        private Statistics _statistics = new(5000);

        // Event for round completion
        public event EventHandler? RoundCompleted;

        // Step-by-step debugging
        private IEnumerator<Position>? _stepEnumerator;
        private DispatcherTimer? _stepTimer;

        // Optional: Property to access statistics from StatistikPage
        public Statistics GameStatistics => _statistics;

        public MainWindow()
        {
            InitializeComponent();
            initializeProgram();
        }

        private void initializeProgram()
        {
            try
            {
                GameConfig.SetGameConfiguration();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Fehler bei der Initialisierung der Spielkonfiguration: " + ex.Message, ex);
                //todo: logging of Exception
            }

            try
            {
                GUIConfig.SetGUIConfiguration();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Fehler bei der Initialisierung der GUI-Konfiguration: " + ex.Message, ex);
                //todo: logging of Exception
            }

            this._gameBoard = new GameBoard(50);
            this._gameBoardPage = new GameBoardPage(this._gameBoard);
            MainFrame.Content = this._gameBoardPage;

            // Initialize game timer with configured interval
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameConfig.GameTimerIntervalMs)
            };
            _gameTimer.Tick += GameTimer_Tick;

            // Initialize step timer for step-by-step mode with configured interval
            _stepTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(GameConfig.StepTimerIntervalMs)
            };
            _stepTimer.Tick += StepTimer_Tick;
        }

        /// <summary>
        /// Updates the timer intervals for game and step timers.
        /// </summary>
        /// <param name="gameTimerMs">Game timer interval in milliseconds.</param>
        /// <param name="stepTimerMs">Step timer interval in milliseconds.</param>
        public void UpdateTimerIntervals(int gameTimerMs, int stepTimerMs)
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(gameTimerMs);
            if (_stepTimer is not null)
            {
                _stepTimer.Interval = TimeSpan.FromMilliseconds(stepTimerMs);
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            if (_gameBoard is null || _gameBoardPage is null)
                return;

            _gameBoard.NextRound();
            _statistics.RecordRound(_gameBoard);
            _gameBoardPage.UpdateBoard();
            
            // Notify subscribers
            RoundCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void StepTimer_Tick(object? sender, EventArgs e)
        {
            ExecuteNextStep();
        }

        /// <summary>
        /// Executes the next single step in step-by-step mode.
        /// </summary>
        private void ExecuteNextStep()
        {
            if (_gameBoard is null || _gameBoardPage is null)
                return;

            // Initialize enumerator if not started
            if (_stepEnumerator is null)
            {
                _stepEnumerator = _gameBoard.NextRoundStepByStep().GetEnumerator();
            }

            // Execute next step
            if (_stepEnumerator.MoveNext())
            {
                _gameBoardPage.UpdateBoard();
            }
            else
            {
                // Round complete - reset for next round
                _stepEnumerator.Dispose();
                _stepEnumerator = null;
                _stepTimer?.Stop();
                
                _statistics.RecordRound(_gameBoard);
                
                // Notify subscribers
                RoundCompleted?.Invoke(this, EventArgs.Empty);

                // Update status
                StatusIndicator.Fill = App.Utils.GetBrush("BoardRed", new SolidColorBrush(Colors.Red));
                StatusIndicator.ToolTip = "Runde beendet";
            }
        }

        /// <summary>
        /// Starts step-by-step execution with automatic progression.
        /// </summary>
        public void StartStepByStep()
        {
            if (_gameBoard is null || _gameBoardPage is null)
                return;

            _stepEnumerator?.Dispose();
            _stepEnumerator = _gameBoard.NextRoundStepByStep().GetEnumerator();

            StatusIndicator.Fill = App.Utils.GetBrush("BoardYellow", new SolidColorBrush(Colors.Yellow));
            StatusIndicator.ToolTip = "Einzelschritt läuft";

            _stepTimer?.Start();
        }

        /// <summary>
        /// Executes a single step manually (for button click).
        /// </summary>
        public void SingleStep()
        {
            if (_gameBoard is null || _gameBoardPage is null)
                return;

            // Stop automatic timers
            _gameTimer.Stop();
            _stepTimer?.Stop();
            _isRunning = false;
            BtnStart.Content = "▶ Start";

            // Initialize enumerator if not started
            if (_stepEnumerator is null)
            {
                _stepEnumerator = _gameBoard.NextRoundStepByStep().GetEnumerator();
                StatusIndicator.Fill = App.Utils.GetBrush("BoardYellow", new SolidColorBrush(Colors.Yellow));
                StatusIndicator.ToolTip = "Einzelschritt-Modus";
            }

            ExecuteNextStep();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public void SetGameBoard(GameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new StatistikPage();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ConfigPage();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnGameBoard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _gameBoardPage;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            // Reset step enumerator when starting normal mode
            _stepEnumerator?.Dispose();
            _stepEnumerator = null;
            _stepTimer?.Stop();

            _isRunning = !_isRunning;

            if (_isRunning)
            {
                StatusIndicator.Fill = App.Utils.GetBrush("BoardGreen", new SolidColorBrush(Colors.LimeGreen));
                StatusIndicator.ToolTip = "Läuft";
                BtnStart.Content = "⏹ Stop";
                _gameTimer.Start();
            }
            else
            {
                StatusIndicator.Fill = App.Utils.GetBrush("BoardRed", new SolidColorBrush(Colors.Red));
                StatusIndicator.ToolTip = "Gestoppt";
                BtnStart.Content = "▶ Start";
                _gameTimer.Stop();
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _isRunning = false;
            _gameTimer.Stop();
            _stepTimer?.Stop();
            _stepEnumerator?.Dispose();
            _stepEnumerator = null;
            _statistics.Reset();

            StatusIndicator.Fill = App.Utils.GetBrush("BoardRed", new SolidColorBrush(Colors.Red));
            StatusIndicator.ToolTip = "Gestoppt";
            BtnStart.Content = "▶ Start";

            _gameBoard?.Reset(BoardCell.CellCreationCode.Random);
            _gameBoardPage?.UpdateBoard();
            
            // Notify subscribers about reset
            RoundCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void BtnStep_Click(object sender, RoutedEventArgs e)
        {
            SingleStep();
        }


    }
}