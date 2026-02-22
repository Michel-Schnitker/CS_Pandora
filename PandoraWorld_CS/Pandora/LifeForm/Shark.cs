using PandoraWorld_CS.Pandora.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.LifeForm
{
    public class Shark : FormOfLife
    {
        protected override LifeFormConfig Config => GameConfig.SharkConfig;

        public Shark()
        {
            speciesName = "Shark";
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public override void ExecuteLifeForm(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells)
        {
            if (HasActedThisRound)
                return;

            HasActedThisRound = true;
            IncrementAge();

            // Check for death by old age or starvation
            if (ShouldDie || IsStarved)
            {
                gameBoard.RemoveLifeForm(currentCell.selfPosistion);
                return;
            }

            // 1. Hunt fish (always if available - sharks are predators)
            var cellsWithFish = GetCellsWithFish(surroundingCells);
            if (cellsWithFish.Count > 0)
            {
                HuntFish(gameBoard, currentCell, cellsWithFish);
                return;
            }

            // 2. Try to breed if mature
            if (CanBreed && TryBreed(gameBoard, currentCell, surroundingCells))
                return;

            // 3. Random movement
            MoveToRandomEmptyCell(gameBoard, currentCell, surroundingCells);
        }

        /// <summary>
        /// Hunts and eats a fish, gaining energy based on fish's energy.
        /// </summary>
        private void HuntFish(GameBoard gameBoard, BoardCell currentCell, List<BoardCell> cellsWithFish)
        {
            var targetCell = cellsWithFish[_random.Next(cellsWithFish.Count)];
            
            // Get fish energy before removing it
            if (targetCell.lifeFormOnCell is Fish fish)
            {
                int energyGained = (int)(fish.energy * Config.NutrientValue);
                GainEnergy(energyGained);
            }

            // Remove the fish (eaten)
            gameBoard.RemoveLifeForm(targetCell.selfPosistion);

            // Move shark to the target cell
            gameBoard.MoveLifeForm(currentCell.selfPosistion, targetCell.selfPosistion);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private bool TryBreed(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells)
        {
            Shark? partner = FindMaturePartner(surroundingCells);
            if (partner is null || !partner.CanBreed)
                return false;

            var emptyCells = GetEmptyCells(surroundingCells);
            if (emptyCells.Count == 0)
                return false;

            var targetCell = emptyCells[_random.Next(emptyCells.Count)];
            var offspring = new Shark { age = 0, HasActedThisRound = true };
            gameBoard.PlaceLifeForm(targetCell.selfPosistion, offspring);

            ResetBreedingCooldown();
            partner.ResetBreedingCooldown();
            partner.HasActedThisRound = true;

            return true;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private static Shark? FindMaturePartner(BoardCell?[,] surroundingCells)
        {
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    if (surroundingCells[dx, dy]?.lifeFormOnCell is Shark shark && shark.CanBreed)
                        return shark;
                }
            }
            return null;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void MoveToRandomEmptyCell(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells)
        {
            var emptyCells = GetEmptyCells(surroundingCells);
            if (emptyCells.Count > 0)
                gameBoard.MoveLifeForm(currentCell.selfPosistion, emptyCells[_random.Next(emptyCells.Count)].selfPosistion);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private static List<BoardCell> GetCellsWithFish(BoardCell?[,] surroundingCells)
        {
            List<BoardCell> fishCells = [];
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    if (surroundingCells[dx, dy]?.lifeFormOnCell is Fish)
                        fishCells.Add(surroundingCells[dx, dy]!);
                }
            }
            return fishCells;
        }
    }
}
