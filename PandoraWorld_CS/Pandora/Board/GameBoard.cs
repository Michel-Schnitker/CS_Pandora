using PandoraWorld_CS.Pandora.LifeForm;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.Board
{
    /// <summary>
    /// Represents the game board with a toroidal topology where edges wrap around.
    /// </summary>
    public class GameBoard
    {
        /// <summary>
        /// The maximum size of the game board.
        /// </summary>
        private Position gameMaxSize;

        /// <summary>
        /// Gets the size (width/height) of the square game board.
        /// </summary>
        public int Size => gameMaxSize.X;

        /// <summary>
        /// Gets the 2D array of board cells.
        /// </summary>
        public BoardCell[,] Cells { get; }

        /// <summary>
        /// Initializes a new game board with the specified size.
        /// </summary>
        /// <param name="size">The width and height of the square board.</param>
        public GameBoard(int size)
        {
            this.gameMaxSize = new Position(size, size);

            // Create and initialize the board cells
            this.Cells = new BoardCell[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    this.Cells[x, y] = new BoardCell(BoardCell.CellCreationCode.Random, new Position(x, y));
                }
            }
        }

        /// <summary>
        /// Resets all cells on the board using the specified creation code.
        /// </summary>
        /// <param name="creationCode">The cell creation code determining how cells are initialized.</param>
        public void Reset(BoardCell.CellCreationCode creationCode)
        {
            for (int x = 0; x < gameMaxSize.X; x++)
            {
                for (int y = 0; y < gameMaxSize.Y; y++)
                {
                    this.Cells[x, y] = new BoardCell(creationCode, new Position(x, y));
                }
            }
        }

        #region LifeForm API

        /// <summary>
        /// Moves a life form from one cell to another.
        /// </summary>
        /// <param name="from">Source position.</param>
        /// <param name="to">Target position.</param>
        /// <returns>True if move was successful, false otherwise.</returns>
        public bool MoveLifeForm(Position from, Position to)
        {
            var sourceCell = GetCell(from);
            var targetCell = GetCell(to);

            if (sourceCell?.lifeFormOnCell is null)
                return false;

            // Move the life form
            targetCell.lifeFormOnCell = sourceCell.lifeFormOnCell;
            sourceCell.lifeFormOnCell = default!;

            return true;
        }

        /// <summary>
        /// Removes the life form from a cell.
        /// </summary>
        /// <param name="position">Position of the cell.</param>
        /// <returns>The removed life form, or null if cell was empty.</returns>
        public FormOfLife? RemoveLifeForm(Position position)
        {
            var cell = GetCell(position);
            var lifeForm = cell.lifeFormOnCell;
            cell.lifeFormOnCell = default!;
            return lifeForm;
        }

        /// <summary>
        /// Places a life form on a cell, replacing any existing one.
        /// </summary>
        /// <param name="position">Target position.</param>
        /// <param name="lifeForm">The life form to place.</param>
        /// <returns>The previous life form on this cell, or null if it was empty.</returns>
        public FormOfLife? PlaceLifeForm(Position position, FormOfLife lifeForm)
        {
            var cell = GetCell(position);
            var previousLifeForm = cell.lifeFormOnCell;
            cell.lifeFormOnCell = lifeForm;
            return previousLifeForm;
        }

        /// <summary>
        /// Checks if a cell has a life form.
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <returns>True if cell contains a life form.</returns>
        public bool HasLifeForm(Position position)
        {
            return GetCell(position).lifeFormOnCell is not null;
        }

        /// <summary>
        /// Checks if a cell is empty (no life form).
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <returns>True if cell is empty.</returns>
        public bool IsEmpty(Position position)
        {
            return GetCell(position).lifeFormOnCell is null;
        }

        /// <summary>
        /// Gets the life form at a specific position.
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <returns>The life form or null.</returns>
        public FormOfLife? GetLifeForm(Position position)
        {
            return GetCell(position).lifeFormOnCell;
        }

        #endregion

        #region Cell Access

        /// <summary>
        /// Gets a cell at a position with wrapping (toroidal).
        /// </summary>
        public BoardCell GetCell(Position position)
        {
            return GetCell(position.X, position.Y);
        }

        /// <summary>
        /// Gets a cell at coordinates with wrapping (toroidal).
        /// </summary>
        public BoardCell GetCell(int x, int y)
        {
            int wrappedX = WrapCoordinate(x, gameMaxSize.X);
            int wrappedY = WrapCoordinate(y, gameMaxSize.Y);
            return Cells[wrappedX, wrappedY];
        }

        /// <summary>
        /// Wraps a coordinate to stay within bounds (toroidal topology).
        /// </summary>
        private int WrapCoordinate(int value, int max)
        {
            return ((value % max) + max) % max;
        }

        #endregion

        #region Game Loop

        /// <summary>
        /// Resets the HasActedThisRound flag for all life forms on the board.
        /// </summary>
        private void ResetAllActedFlags()
        {
            int size = gameMaxSize.X;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var lifeForm = Cells[x, y].lifeFormOnCell;
                    if (lifeForm is not null)
                    {
                        lifeForm.HasActedThisRound = false;
                    }
                }
            }
        }

        /// <summary>
        /// Executes one game round by iterating through all cells from [0,0] to [n,n].
        /// </summary>
        public void NextRound()
        {
            ResetAllActedFlags();

            int size = gameMaxSize.X;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    BoardCell?[,] surroundingCells = GetSurroundingCells(x, y, size);
                    Cells[x, y].ExecuteCell(this, surroundingCells);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that executes one cell at a time for step-by-step debugging.
        /// </summary>
        /// <returns>Enumerator yielding the position after each cell execution.</returns>
        public IEnumerable<Position> NextRoundStepByStep()
        {
            ResetAllActedFlags();

            int size = gameMaxSize.X;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    BoardCell?[,] surroundingCells = GetSurroundingCells(x, y, size);
                    Cells[x, y].ExecuteCell(this, surroundingCells);
                    yield return new Position(x, y);
                }
            }
        }

        /// <summary>
        /// Gets a 3x3 matrix of surrounding cells with wrapping at edges (toroidal topology).
        /// The center cell [1,1] is null as it represents the current cell.
        /// </summary>
        private BoardCell?[,] GetSurroundingCells(int centerX, int centerY, int size)
        {
            BoardCell?[,] surrounding = new BoardCell?[3, 3];

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int arrayX = dx + 1;
                    int arrayY = dy + 1;

                    // Center cell is null (the cell itself)
                    if (dx == 0 && dy == 0)
                    {
                        surrounding[arrayX, arrayY] = null;
                        continue;
                    }

                    surrounding[arrayX, arrayY] = GetCell(centerX + dx, centerY + dy);
                }
            }

            return surrounding;
        }

        #endregion
    }
}
