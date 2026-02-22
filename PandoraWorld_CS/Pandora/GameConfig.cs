using System;
using System.Collections.Generic;
using System.Text;

namespace PandoraWorld_CS.Pandora
{
    internal class GameConfig
    {

        public static int PlantCreatrionMin { get; } = 100;
        public static int PlantCreatrionMax { get; } = 1000;

        // Plant growth configuration
        public static int PlantGrowthMin { get; set; } = 50;
        public static int PlantGrowthMax { get; set; } = 200;
        public static int PlantMaxSize { get; set; } = 5000;
        public static int PlantSpreadThreshold { get; set; } = 2000;
        public static int PlantSurroundedMinNeighbors { get; set; } = 6;
        public static int PlantShrinkMin { get; set; } = 50;
        public static int PlantShrinkMax { get; set; } = 150;

        // Timer configuration
        /// <summary>
        /// Game timer interval in milliseconds. Controls how fast the simulation runs.
        /// </summary>
        public static int GameTimerIntervalMs { get; set; } = 100;

        /// <summary>
        /// Step timer interval in milliseconds. Controls speed in step-by-step mode.
        /// </summary>
        public static int StepTimerIntervalMs { get; set; } = 50;

        /// <summary>
        /// Fish configuration parameters.
        /// </summary>
        public static LifeFormConfig FishConfig { get; set; } = new(
            maxAge: 30, 
            maturityAge: 5, 
            breedingCooldown: 3,
            eatAmountMin: 50,
            eatAmountMax: 150,
            nutrientValue: 0.05,
            energyLossPerRound: 10
        );

        /// <summary>
        /// Shark configuration parameters.
        /// </summary>
        public static LifeFormConfig SharkConfig { get; set; } = new(
            maxAge: 100, 
            maturityAge: 10, 
            breedingCooldown: 8,
            eatAmountMin: 0,      // Not used - sharks eat fish energy
            eatAmountMax: 0,      // Not used - sharks eat fish energy
            nutrientValue: 0.5,   // Fish energy * 0.5 = shark energy gained
            energyLossPerRound: 5
        );


        public static int FishPercentage { get; set; } = 13;
        public static int SharkPercentage { get; set; } = 2;
        public static int PlantPercentage { get; set; } = 30;


        public static void SetGameConfiguration()
        {
            // TODO: load config from file or other source or manipulate here

            validateConfig();
        }

        private static void validateConfig()
        {
            if (FishPercentage + SharkPercentage + PlantPercentage > 100)
            {
                throw new InvalidOperationException("The sum of FishPercentage, SharkPercentage, and PlantPercentage must under 100 Percent.");
            }

            // TODO: check other config values for validity (e.g. non-negative, max > min, etc.)
        }



    }
}
