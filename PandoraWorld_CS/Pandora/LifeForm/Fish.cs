using PandoraWorld_CS.Pandora.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora.LifeForm
{
    public class Fish : FormOfLife
    {
        protected override LifeFormConfig Config => GameConfig.FishConfig;

        public Fish()
        {
            speciesName = "Fish";
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

            // 1. Flee from sharks (highest priority)
            BoardCell? sharkCell = FindFirstShark(surroundingCells);
            if (sharkCell is not null)
            {
                FleeFromShark(gameBoard, currentCell, surroundingCells, sharkCell);
                return;
            }

            // 2. Try to breed if mature
            if (CanBreed && TryBreed(gameBoard, currentCell, surroundingCells))
                return;

            // 3. Eat plants if hungry
            if (IsHungry && TryEatPlants(surroundingCells))
                return;

            // 4. Random movement
            MoveToRandomEmptyCell(gameBoard, currentCell, surroundingCells);
        }

        /// <summary>
        /// Attempts to eat plants from a neighboring cell.
        /// </summary>
        private bool TryEatPlants(BoardCell?[,] surroundingCells)
        {
            var plantCells = GetCellsWithPlants(surroundingCells);
            if (plantCells.Count == 0)
                return false;

            // Pick a random cell with plants
            var targetCell = plantCells[_random.Next(plantCells.Count)];

            // Calculate how much to eat
            int eatAmount = _random.Next(Config.EatAmountMin, Config.EatAmountMax + 1);
            eatAmount = Math.Min(eatAmount, targetCell.plantPopulation);

            // Reduce plant population
            targetCell.plantPopulation -= eatAmount;

            // Gain energy based on nutrient value
            int energyGained = (int)(eatAmount * Config.NutrientValue);
            GainEnergy(energyGained);

            return true;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private bool TryBreed(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells)
        {
            Fish? partner = FindMaturePartner(surroundingCells);
            if (partner is null || !partner.CanBreed)
                return false;

            var emptyCells = GetEmptyCells(surroundingCells);
            if (emptyCells.Count == 0)
                return false;

            var targetCell = emptyCells[_random.Next(emptyCells.Count)];
            var offspring = new Fish { age = 0, HasActedThisRound = true };
            gameBoard.PlaceLifeForm(targetCell.selfPosistion, offspring);

            ResetBreedingCooldown();
            partner.ResetBreedingCooldown();
            partner.HasActedThisRound = true;

            return true;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private static Fish? FindMaturePartner(BoardCell?[,] surroundingCells)
        {
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    if (surroundingCells[dx, dy]?.lifeFormOnCell is Fish fish && fish.CanBreed)
                        return fish;
                }
            }
            return null;
        }

        /// <summary>
        /// TODO:
        /// </summary>
        private void FleeFromShark(GameBoard gameBoard, BoardCell currentCell, BoardCell?[,] surroundingCells, BoardCell sharkCell)
        {
            int sharkDx = sharkCell.selfPosistion.X - currentCell.selfPosistion.X;
            int sharkDy = sharkCell.selfPosistion.Y - currentCell.selfPosistion.Y;

            int oppositeDx = Math.Clamp(1 - sharkDx, 0, 2);
            int oppositeDy = Math.Clamp(1 - sharkDy, 0, 2);

            var oppositeCell = surroundingCells[oppositeDx, oppositeDy];
            if (oppositeCell is not null && oppositeCell.lifeFormOnCell is null)
            {
                gameBoard.MoveLifeForm(currentCell.selfPosistion, oppositeCell.selfPosistion);
                return;
            }

            MoveToRandomEmptyCell(gameBoard, currentCell, surroundingCells);
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
        private static BoardCell? FindFirstShark(BoardCell?[,] surroundingCells)
        {
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 3; dy++)
                {
                    if (surroundingCells[dx, dy]?.lifeFormOnCell is Shark)
                        return surroundingCells[dx, dy];
                }
            }
            return null;
        }
    }
}
