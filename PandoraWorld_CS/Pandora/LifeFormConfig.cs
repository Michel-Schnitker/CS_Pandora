using System;

namespace PandoraWorld_CS.Pandora
{
    /// <summary>
    /// Configuration for a life form species.
    /// </summary>
    public class LifeFormConfig
    {
        /// <summary>
        /// Maximum age before death.
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Age at which the life form can reproduce.
        /// </summary>
        public int MaturityAge { get; set; }

        /// <summary>
        /// Minimum rounds between breeding.
        /// </summary>
        public int BreedingCooldown { get; set; }

        /// <summary>
        /// Minimum amount of food consumed per eating action.
        /// </summary>
        public int EatAmountMin { get; set; }

        /// <summary>
        /// Maximum amount of food consumed per eating action.
        /// </summary>
        public int EatAmountMax { get; set; }

        /// <summary>
        /// Nutrient value multiplier when eating (food amount * nutrient = energy gained).
        /// </summary>
        public double NutrientValue { get; set; }

        /// <summary>
        /// Energy lost per round.
        /// </summary>
        public int EnergyLossPerRound { get; set; }

        public LifeFormConfig(
            int maxAge,
            int maturityAge,
            int breedingCooldown,
            int eatAmountMin = 50,
            int eatAmountMax = 150,
            double nutrientValue = 0.05,
            int energyLossPerRound = 5)
        {
            MaxAge = maxAge;
            MaturityAge = maturityAge;
            BreedingCooldown = breedingCooldown;
            EatAmountMin = eatAmountMin;
            EatAmountMax = eatAmountMax;
            NutrientValue = nutrientValue;
            EnergyLossPerRound = energyLossPerRound;
        }
    }
}