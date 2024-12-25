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
        internal static ConfigEntry<int> NoEffectChance;
        internal static ConfigEntry<int> IntoxicationChance;
        internal static ConfigEntry<int> PoisoningChance;
        internal static ConfigEntry<int> MaxPoison;
        internal static ConfigEntry<int> HealingChance;
        internal static ConfigEntry<int> MaxHealing;
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
            #endregion

            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource("Initializing Plugin: Flask Of Fortune And Folly by 8");
            harmony.PatchAll();
            mls.LogInfo("Flask Of Fortune And Folly Plugin loaded sucessfully.");
        }
    }
}
