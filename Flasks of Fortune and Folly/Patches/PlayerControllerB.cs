﻿#region usings
using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine.InputSystem;
#endregion

namespace FlaskOfFortuneAndFolly.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatches
    {
        private static FlaskOfFortuneAndFollyPlugin pluginInstance => FlaskOfFortuneAndFollyPlugin.Instance;

        [HarmonyPatch("GrabObjectServerRpc")]
        [HarmonyPostfix]
        private static void PickUpScrapItem(NetworkObjectReference grabbedObject)
        {
            grabbedObject.TryGet(out var networkObject);
            var scrapItem = networkObject.gameObject.GetComponentInChildren<GrabbableObject>();
            pluginInstance.scrapHandler.RegisterScrapItem(scrapItem);
            FlaskOfFortuneAndFollyPlugin.mls?.LogInfo($"{scrapItem.name} picked up by {scrapItem.playerHeldBy}");
        }

        [HarmonyPatch("ActivateItem_performed")]
        [HarmonyPrefix]
        private static void UseScrapItem(InputAction.CallbackContext context, ref GrabbableObject ___currentlyHeldObjectServer)
        {
            if (___currentlyHeldObjectServer != null)
                pluginInstance.scrapHandler.UseScrapItem(___currentlyHeldObjectServer);
        }

        [HarmonyPatch("InspectItem_performed")]
        [HarmonyPrefix]
        private static void InspectItem_performed(InputAction.CallbackContext context, ref GrabbableObject ___currentlyHeldObjectServer)
        {
            if (___currentlyHeldObjectServer != null)
                pluginInstance.scrapHandler.InspectScrapItem(___currentlyHeldObjectServer);
        }

        [HarmonyPatch("ItemSecondaryUse_performed")]
        [HarmonyPrefix]
        private static void ItemSecondaryUse_performed(InputAction.CallbackContext context, ref GrabbableObject ___currentlyHeldObjectServer)
        {
            if (___currentlyHeldObjectServer != null)
                pluginInstance.scrapHandler.SpecialUse(___currentlyHeldObjectServer);
        }

        //[HarmonyPatch("ItemSold")]
        //[HarmonyPrefix]
        //private static void SellScrapItem(GrabbableObject scrapItemSold)
        //{
        //    // Remove the scrap item from the ScrapHandler when sold
        //    pluginInstance.scrapHandler.RemoveScrapItem(scrapItemSold);

        //    FlaskOfFortuneAndFollyPlugin.mls?.LogInfo($"{scrapItemSold.name} sold.");
        //}
    }
}
