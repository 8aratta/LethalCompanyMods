using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FlaskOfFortuneAndFolly.Handlers;
using HarmonyLib;

namespace FlaskOfFortuneAndFolly
{
    [BepInPlugin(GUID, NAME, VER)]

    public class FlaskOfFortuneAndFollyPlugin : BaseUnityPlugin
    {
        private const string GUID = "8_LC_FlaskOfFortuneAndFollyPlugin";
        private const string NAME = "Flask Of Fortune And Folly";
        private const string VER = "1.0.0.0";
        private readonly Harmony harmony = new Harmony("FlaskOfFortuneAndFollyPlugin");
        internal static FlaskOfFortuneAndFollyPlugin? Instance { get; private set; }
        internal static ManualLogSource? mls;
        internal ScrapHandler scrapHandler = new ScrapHandler();

        #region Flask Configs
        /// <summary>
        /// NoEffectChance - Probability for Flask to do nothing
        /// IntoxicationChance - Probability for Flask to intoxicate the HoldingPlayer
        /// PoisoningChance - Probability for Flask to poison the HoldingPlayer
        /// HealingChance - Probability for Flask to heal the HoldingPlayer a certain amount of hp
        /// MaxHealing - Max amount of healing a flask can do
        /// </summary>
        internal static ConfigEntry<int>? NoEffectChance;
        internal static ConfigEntry<int>? IntoxicationChance;
        internal static ConfigEntry<int>? PoisoningChance;
        internal static ConfigEntry<int>? MaxPoison;
        internal static ConfigEntry<int>? HealingChance;
        internal static ConfigEntry<int>? MaxHealing;
        internal static ConfigEntry<int>? InvertedControlsChance;
        internal static ConfigEntry<int>? TeleportationChance;
        internal static ConfigEntry<int>? FatigueChance;
        internal static ConfigEntry<int>? CombustionChance;
        internal static ConfigEntry<int>? BlindnessChance;
        internal static ConfigEntry<int>? DysphasiaChance;
        internal static ConfigEntry<int>? PowerChance;
        internal static ConfigEntry<int>? NightVisionChance;
        internal static ConfigEntry<int>? ScatterChance;
        internal static ConfigEntry<int>? EscapeChance;
        #endregion


        void Awake()
        {
            #region Flask Default Settings
            NoEffectChance = Config.Bind("Flask",
                                         "NoEffectChance",
                                         20,
                                         "Probability of flasks to have of having no effect.");

            IntoxicationChance = Config.Bind("Flask",
                                              "IntoxicationChance",
                                              50,
                                              "Probability of flasks to intoxicate the LocalPlayer.");

            PoisoningChance = Config.Bind("Flask",
                                          "PoisoningChance",
                                           50,
                                           "Probability of flasks to poison the LocalPlayer.");

            MaxPoison = Config.Bind("Flask",
                                    "MaxPoison",
                                    1,
                                    "Health left after being poisoned. 0 Kills the LocalPlayer.");

            HealingChance = Config.Bind("Flask",
                                        "HealingChance",
                                        1,
                                        "Probability of flasks to heal the LocalPlayer.");

            MaxHealing = Config.Bind("Flask",
                                     "MaxHealing",
                                     33,
                                     "Max amount of healing a flask can do. (Based on Scrap Value)");

            InvertedControlsChance = Config.Bind("Flask",
                                                 "InvertedControlsChance",
                                                 40,
                                                 "Probability of flasks to invert the controls of the LocalPlayer.");

            TeleportationChance = Config.Bind("Flask",
                                              "TeleportationChance",
                                              10,
                                              "Probability of flasks to teleport the LocalPlayer to a random location.");

            FatigueChance = Config.Bind("Flask",
                                        "FatigueChance",
                                        50,
                                        "Probability of flasks to cause fatigue in the LocalPlayer.");

            CombustionChance = Config.Bind("Flask",
                                           "CombustionChance",
                                           30,
                                           "Probability of flasks to set the LocalPlayer on fire.");

            BlindnessChance = Config.Bind("Flask",
                                          "BlindnessChance",
                                          50,
                                          "Probability of flasks to cause blindness in the LocalPlayer.");

            DysphasiaChance = Config.Bind("Flask",
                                          "DysphasiaChance",
                                          40,
                                          "Probability of flasks to cause funny voice cracks for the LocalPlayer.");

            PowerChance = Config.Bind("Flask",
                                      "PowerChance",
                                      5,
                                      "Probability of flasks to grant the LocalPlayer increased power.");

            NightVisionChance = Config.Bind("Flask",
                                            "NightVisionChance",
                                            5,
                                            "Probability of flasks to grant the LocalPlayer night vision.");

            ScatterChance = Config.Bind("Flask",
                                        "ScatterChance",
                                        30,
                                        "Probability of flasks to scatter the LocalPlayer's items.");

            EscapeChance = Config.Bind("Flask",
                                       "EscapeChance",
                                       10,
                                       "Probability of flasks to teleport the Player to the ship.");
            #endregion

            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource("Initializing Plugin: Flask Of Fortune And Folly by 8");
            harmony.PatchAll();
            mls.LogInfo("Flask Of Fortune And Folly Plugin loaded successfully.");
        }
    }
}
