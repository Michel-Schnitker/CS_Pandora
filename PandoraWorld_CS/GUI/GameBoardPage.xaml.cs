using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using PandoraWorld_CS.Pandora.Board;

namespace PandoraWorld_CS.GUI
{
    public partial class GameBoardPage : Page
    {
        private readonly GameBoard _gameBoard;
        private readonly int _size;
        private readonly List<List<Grid>> _cellGrids = new();

        public GameBoardPage(GameBoard gameBoard)
        {
            InitializeComponent();
            _gameBoard = gameBoard;
            _size = _gameBoard.Cells.GetLength(0);
            CreateBoard();
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void CreateBoard()
        {
            BoardGrid.Rows = _size;
            BoardGrid.Columns = _size;

            _cellGrids.Clear();

            for (int x = 0; x < _size; x++)
            {
                var row = new List<Grid>();
                for (int y = 0; y < _size; y++)
                {
                    var grid = CreateCellGrid(_gameBoard.Cells[x, y]);
                    BoardGrid.Children.Add(grid);
                    row.Add(grid);
                }
                _cellGrids.Add(row);
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private Grid CreateCellGrid(BoardCell cell)
        {
            var grid = new Grid();

            var outerRect = new Rectangle
            {
                Fill = GetOuterBrush(cell),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var innerRect = new Rectangle
            {
                Fill = GetInnerBrush(cell),
                Width = 0,
                Height = 0,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            grid.SizeChanged += (s, e) =>
            {
                double min = Math.Min(grid.ActualWidth, grid.ActualHeight);
                innerRect.Width = min * GUIConfig.LifeScaleOfField;
                innerRect.Height = min * GUIConfig.LifeScaleOfField;
            };

            grid.Children.Add(outerRect);
            grid.Children.Add(innerRect);

            // Tag für späteres Update
            grid.Tag = (outerRect, innerRect);

            return grid;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private Brush GetOuterBrush(BoardCell cell)
        {
            // Beispiel: Grün für leere Felder, Blau für Pflanzen
            if (cell.plantPopulation > 0)
                return App.Utils.GetBrush("BoardGreen", new SolidColorBrush(Colors.Green));
            return App.Utils.GetBrush("BoardBlue", new SolidColorBrush(Colors.Blue));
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private Brush GetInnerBrush(BoardCell cell)
        {
            // Rot für Shark, Gelb für Fish, Transparent sonst
            if (cell.lifeFormOnCell is PandoraWorld_CS.Pandora.LifeForm.Shark)
                return App.Utils.GetBrush("BoardRed", new SolidColorBrush(Colors.Red));
            if (cell.lifeFormOnCell is PandoraWorld_CS.Pandora.LifeForm.Fish)
                return App.Utils.GetBrush("BoardYellow", new SolidColorBrush(Colors.Yellow));
            return App.Utils.GetBrush("BoardTransparent", new SolidColorBrush(Colors.Transparent));
        }

        /// <summary>
        /// Update-Funktion, die die Farben der Rechtecke basierend auf dem aktuellen Zustand des GameBoards aktualisiert.
        /// </summary>
        public void UpdateBoard()
        {
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    var grid = _cellGrids[x][y];
                    var (outerRect, innerRect) = ((Rectangle, Rectangle))grid.Tag;
                    var cell = _gameBoard.Cells[x, y];

                    outerRect.Fill = GetOuterBrush(cell);
                    innerRect.Fill = GetInnerBrush(cell);
                }
            }
        }
    }
}