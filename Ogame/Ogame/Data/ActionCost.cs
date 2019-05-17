using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ogame.Models;

namespace Ogame.Data
{
    public class ActionCost
    {
        public float CristalCost { get; set; }
        public float MetalCost { get; set; }
        public float DeuteriumCost { get; set; }
        public float EnergyCost { get; set; }
        public TimeSpan ActionTime { get; set; }

        private const float _mineRessourceCostRate = 0.7f;
        private const float _solarPanelEnergyCostRate = 0.5f;
        private const float _spaceshipEnergyCostRate = 2.0f;
        private const float _defenseEnergyCostRate = 2.0f;

        public static ActionCost UpgradeMineCost(Mine mine)
        {
            ActionCost cost = new ActionCost
            {
                EnergyCost = mine.Caps.Energy_cap,
                ActionTime = (1 + MathF.Pow(mine.Level, 2)) * TemporalActionResolver.CycleDuration
            };

            switch (mine.Ressource)
            {
                case Mine.Ressources.Cristal:
                    cost.CristalCost = mine.Caps.Cristal_cap * _mineRessourceCostRate;
                    cost.MetalCost = mine.Caps.Metal_cap;
                    cost.DeuteriumCost = mine.Caps.Deuterium_cap;
                    break;
                case Mine.Ressources.Deuterium:
                    cost.CristalCost = mine.Caps.Cristal_cap;
                    cost.MetalCost = mine.Caps.Metal_cap;
                    cost.DeuteriumCost = mine.Caps.Deuterium_cap * _mineRessourceCostRate;
                    break;
                case Mine.Ressources.Metal:
                    cost.CristalCost = mine.Caps.Cristal_cap;
                    cost.MetalCost = mine.Caps.Metal_cap * _mineRessourceCostRate;
                    cost.DeuteriumCost = mine.Caps.Deuterium_cap;
                    break;
            }

            return cost;
        }

        public static ActionCost UpgradeSolarPanelCost(SolarPanel solarPanel)
        {
            return new ActionCost
            {
                CristalCost = solarPanel.Caps.Cristal_cap,
                DeuteriumCost = solarPanel.Caps.Deuterium_cap,
                MetalCost = solarPanel.Caps.Metal_cap,
                EnergyCost = solarPanel.Caps.Energy_cap * _solarPanelEnergyCostRate,
                ActionTime = (1 + MathF.Pow(solarPanel.Level, 2)) * TemporalActionResolver.CycleDuration
            };
        }

        public static ActionCost UpgradeSpaceshipActionCost(Spaceship spaceship)
        {
            return new ActionCost
            {
                CristalCost = spaceship.Caps.Cristal_cap,
                DeuteriumCost = spaceship.Caps.Deuterium_cap,
                MetalCost = spaceship.Caps.Metal_cap,
                EnergyCost = spaceship.Caps.Energy_cap * _spaceshipEnergyCostRate,
                ActionTime = (1 + MathF.Pow(spaceship.Level, 2)) * TemporalActionResolver.CycleDuration
            };
        }

        public static ActionCost UpgradeDefenseActionCost(Defense defense)
        {
            return new ActionCost
            {
                CristalCost = defense.Caps.Cristal_cap,
                DeuteriumCost = defense.Caps.Deuterium_cap,
                MetalCost = defense.Caps.Metal_cap,
                EnergyCost = defense.Caps.Energy_cap * _defenseEnergyCostRate,
                ActionTime = (1 + MathF.Pow(defense.Level, 2)) * TemporalActionResolver.CycleDuration
            };
        }
    }
}
