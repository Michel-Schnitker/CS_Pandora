using PandoraWorld_CS.Pandora.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.LifeForm
{
    public abstract class FormOfLife
    {
        protected static readonly Random _random = new();

        public int age { get; set; } = 0;
        public int energy { get; set; } = 100;
        public string speciesName { get; set; } = "Unknown";

        /// <summary>
        /// Rounds since last breeding (0 = can breed if mature).
        /// </summary>
        public int RoundsSinceLastBreeding { get; set; } = 0;

        /// <summary>
        /// Indicates whether this life form has already acted this round.
        /// </summary>
        public bool HasActedThisRound { get; set; } = false;

        /// <summary>
        /// Returns the configuration for this life form species.
        /// </summary>
        protected abstract LifeFormConfig Config { get; }

        /// <summary>
        /// Maximum energy a life form can have.
        /// </summary>
        public const int MaxEnergy = 100;

        /// <summary>
        /// Checks if this life form is hungry and wants to eat.
        /// </summary>
        public bool IsHungry => energy < MaxEnergy;

        /// <summary>
        /// Checks if this life form has starved to death.
        /// </summary>
        public bool IsStarved => energy <= 0;

        /// <summary>
        /// Checks if this life form is mature and can breed.
        /// </summary>
        public bool CanBreed => age >= Config.MaturityAge && RoundsSinceLastBreeding >= Config.BreedingCooldown;

        /// <summary>
        /// Checks if this life form has reached its maximum age and should die.
        /// </summary>
        public bool ShouldDie => age >= Config.MaxAge;

        /// <summary>
        /// Executes the life form's logic for one game round.
        /// </summary>
        /// <param name="gameBoard">Reference to the game board for API access.</param>
        /// <param name="currentCell">The cell this life form is currently on.</param>
        /// <param name="surroundingCells">3x3 matrix of surrounding cells. Center cell [1,1] is null.</param>
        public abstract void ExecuteLifeForm(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells);

        /// <summary>
        /// Increments age, breeding cooldown counter and reduces energy.
        /// </summary>
        protected void IncrementAge()
        {
            age++;
            RoundsSinceLastBreeding++;
            energy = Math.Max(0, energy - Config.EnergyLossPerRound);
        }

        /// <summary>
        /// Adds energy from eating, capped at MaxEnergy.
        /// </summary>
        protected void GainEnergy(int amount)
        {
            energy = Math.Min(MaxEnergy, energy + amount);
        }

        /// <summary>
        /// Resets breeding cooldown after successful reproduction.
        /// </summary>
        public void ResetBreedingCooldown()
        {
            RoundsSinceLastBreeding = 0;
        }

        /// <summary>
        /// Gets all empty cells from surrounding cells.
        /// </summary>
        protected static List<BoardCell> GetEmptyCells(BoardCell?[,] surroundingCells)
        {
            List<BoardCell> emptyCells = [];
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    var cell = surroundingCells[dx, dy];
                    if (cell is not null && cell.lifeFormOnCell is null)
                    {
                        emptyCells.Add(cell);
                    }
                }
            }
            return emptyCells;
        }

        /// <summary>
        /// Gets all cells with plants from surrounding cells.
        /// </summary>
        protected static List<BoardCell> GetCellsWithPlants(BoardCell?[,] surroundingCells)
        {
            List<BoardCell> plantCells = [];
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    var cell = surroundingCells[dx, dy];
                    if (cell is not null && cell.plantPopulation > 0)
                    {
                        plantCells.Add(cell);
                    }
                }
            }
            return plantCells;
        }
    }
}
