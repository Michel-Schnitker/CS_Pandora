using PandoraWorld_CS.Pandora.LifeForm;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.Board
{
    public class BoardCell
    {

        public int plantPopulation { get; set; } = 0;
        public FormOfLife lifeFormOnCell { get; set; } = null;

        public Position selfPosistion { get; }

        private static readonly Random _random = new Random();

        /// <summary>
        /// TODO:
        /// </summary>
        public BoardCell(CellCreationCode creationCode, Position pos)
        {
            this.selfPosistion = pos.Copy();
            Random random = new Random();
            switch (creationCode)
            {
                case CellCreationCode.Empty:
                    break;
                case CellCreationCode.Random:
                    // approximately based on percentages
                    int roll = random.Next(0, 100); // 0-99
                    int fishLimit = GameConfig.FishPercentage;
                    int sharkLimit = fishLimit + GameConfig.SharkPercentage;
                    int plantLimit = sharkLimit + GameConfig.PlantPercentage;

                    if (roll < fishLimit)
                    {
                        lifeFormOnCell = new Fish();
                    }
                    else if (roll < sharkLimit)
                    {
                        lifeFormOnCell = new Shark();
                    }
                    else if (roll < plantLimit)
                    {
                        plantPopulation = random.Next(GameConfig.PlantCreatrionMin, GameConfig.PlantCreatrionMax);
                    }
                    else
                    {
                        // empty
                    }
                    break;
                case CellCreationCode.Fish:
                    lifeFormOnCell = new Fish();
                    break;
                case CellCreationCode.Shark:
                    lifeFormOnCell = new Shark();
                    break;
                case CellCreationCode.OnlyPlants:
                    plantPopulation = random.Next(GameConfig.PlantCreatrionMin, GameConfig.PlantCreatrionMax);
                    break;
            }
        }

        /// <summary>
        /// Executes the cell logic for one game round.
        /// </summary>
        /// <param name="gameBoard">Reference to the game board for API access.</param>
        /// <param name="surroundingCells">3x3 matrix of surrounding cells. Center cell [1,1] is null (this cell).</param>
        public void ExecuteCell(GameBoard gameBoard, BoardCell?[,] surroundingCells)
        {
            // Execute plant growth logic
            ExecutePlantGrowth(gameBoard, surroundingCells);

            // Execute life form logic
            lifeFormOnCell?.ExecuteLifeForm(gameBoard, this, surroundingCells);
        }

        /// <summary>
        /// Executes plant growth logic for this cell.
        /// </summary>
        private void ExecutePlantGrowth(GameBoard gameBoard, BoardCell?[,] surroundingCells)
        {
            if (plantPopulation <= 0)
                return;

            // Count surrounding plants
            int plantNeighborCount = CountPlantNeighbors(surroundingCells);

            // If surrounded by n+ plants, shrink instead of grow
            if (plantNeighborCount >= GameConfig.PlantSurroundedMinNeighbors)
            {
                int shrinkAmount = _random.Next(GameConfig.PlantShrinkMin, GameConfig.PlantShrinkMax + 1);
                plantPopulation = Math.Max(0, plantPopulation - shrinkAmount);
                return;
            }

            // Only grow if below maximum size
            if (plantPopulation < GameConfig.PlantMaxSize)
            {
                int growthAmount = _random.Next(GameConfig.PlantGrowthMin, GameConfig.PlantGrowthMax + 1);
                plantPopulation = Math.Min(plantPopulation + growthAmount, GameConfig.PlantMaxSize);
            }

            // Check if plant should spread to neighboring cell
            if (plantPopulation >= GameConfig.PlantSpreadThreshold)
            {
                TrySpreadToEmptyNeighbor(surroundingCells);
            }
        }

        /// <summary>
        /// Counts how many neighboring cells have plants.
        /// </summary>
        private int CountPlantNeighbors(BoardCell?[,] surroundingCells)
        {
            int count = 0;
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    // Skip center (this cell)
                    if (dx == 1 && dy == 1)
                        continue;

                    var neighbor = surroundingCells[dx, dy];
                    if (neighbor?.plantPopulation > 0)
                        count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Attempts to spread the plant to an empty neighboring cell.
        /// </summary>
        private void TrySpreadToEmptyNeighbor(BoardCell?[,] surroundingCells)
        {
            // Find all empty neighbors (no life form and no plants)
            var emptyNeighbors = new List<BoardCell>();

            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    // Skip center (this cell)
                    if (dx == 1 && dy == 1)
                        continue;

                    var neighbor = surroundingCells[dx, dy];
                    if (neighbor is not null && neighbor.plantPopulation == 0 && neighbor.lifeFormOnCell is null)
                    {
                        emptyNeighbors.Add(neighbor);
                    }
                }
            }

            // Spread to a random empty neighbor if available
            if (emptyNeighbors.Count > 0)
            {
                var targetCell = emptyNeighbors[_random.Next(emptyNeighbors.Count)];

                // Transfer half the population to the new cell
                int transferAmount = plantPopulation / 2;
                targetCell.plantPopulation = transferAmount;
                plantPopulation -= transferAmount;
            }
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public enum CellCreationCode
        {
            Empty,
            Random,
            Fish,
            Shark,
            OnlyPlants
        }

    }
}
