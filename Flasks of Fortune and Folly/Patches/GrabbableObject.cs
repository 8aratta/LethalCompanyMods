using HarmonyLib;

namespace FlaskOfFortuneAndFolly.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class GrabbableObjectPatches
    {
        private static FlaskOfFortuneAndFollyPlugin pluginInstance => FlaskOfFortuneAndFollyPlugin.Instance;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdateBaseScrap(GrabbableObject __instance)
        {
                pluginInstance.scrapHandler.UpdateScrapItem(__instance);
        }
    }
}
