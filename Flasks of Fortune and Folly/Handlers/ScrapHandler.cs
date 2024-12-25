#region usings
using GameNetcodeStuff;
using FlaskOfFortuneAndFolly;
using FlaskOfFortuneAndFolly.Scrap;
using System.Collections.Generic;
#endregion

namespace FlaskOfFortuneAndFolly.Handlers
{
    public class ScrapHandler
    {
        internal Dictionary<int, BaseScrapItem> scrapItemDictionary = new Dictionary<int, BaseScrapItem>();

        public void RegisterScrapItem(GrabbableObject scrapItem)
        {
            int instanceId = scrapItem.GetInstanceID();
            if (!scrapItemDictionary.ContainsKey(instanceId))
            {
                BaseScrapItem newItem = CreateScrapItem(scrapItem);
                if (newItem != null)
                {
                    scrapItemDictionary.Add(instanceId, newItem);
                    FlaskOfFortuneAndFollyPlugin.mls?.LogInfo($"{scrapItem.name} registered in ScrapHandler.");
                }
            }
            else if (scrapItem.playerHeldBy != null)
            {
                scrapItemDictionary[instanceId].HoldingPlayer = scrapItem.playerHeldBy;
            }
        }

        public void RemoveScrapItem(GrabbableObject scrapItem)
        {
            int instanceId = scrapItem.GetInstanceID();
            if (scrapItemDictionary.ContainsKey(instanceId))
            {
                if (scrapItemDictionary.Remove(instanceId))
                {
                    FlaskOfFortuneAndFollyPlugin.mls?.LogInfo($"{scrapItem.name} removed from ScrapHandler.");
                    return;
                }
            }
            FlaskOfFortuneAndFollyPlugin.mls?.LogError($"{scrapItem.name} not found in ScrapHandler.");
        }

        public void UpdateScrapItem(GrabbableObject scrapItem)
        {
            int scrapID = scrapItem.GetInstanceID();
            if (scrapItemDictionary.ContainsKey(scrapID) && scrapItemDictionary[scrapID].ItemModified)
            {
                scrapItemDictionary[scrapID].UpdateItem();
            }
        }

        public void InspectScrapItem(GrabbableObject scrapItem)
        {
            int instanceId = scrapItem.GetInstanceID();
            if (scrapItemDictionary.ContainsKey(instanceId))
                scrapItemDictionary[instanceId].InspectItem();
        }

        public void UseScrapItem(GrabbableObject scrapItem)
        {
            int instanceId = scrapItem.GetInstanceID();
            if (scrapItemDictionary.ContainsKey(instanceId))
                scrapItemDictionary[instanceId].UseItem();
        }

        public void SpecialUse(GrabbableObject scrapItem)
        {
            int instanceId = scrapItem.GetInstanceID();
            if (scrapItemDictionary.ContainsKey(instanceId) && scrapItemDictionary[instanceId].HasSecondaryUse)
                scrapItemDictionary[instanceId].SecondaryUseItem();

        }

        private BaseScrapItem CreateScrapItem(GrabbableObject scrapItem)
        {
            BaseScrapItem? customScrap = null;

            // Create custom ScrapItems 
            switch (scrapItem.name.Replace("(Clone)", null))
            {
                case "Flask":
                    customScrap = new Flask(scrapItem);
                    break;
                default:
                    FlaskOfFortuneAndFollyPlugin.mls?.LogInfo($"Unsupported scrap item type {scrapItem.name} picked up");
                    return null;
            }

            if (customScrap == null)
                FlaskOfFortuneAndFollyPlugin.mls?.LogError($"Failed to create custom scrap item for {scrapItem.name}");

            return customScrap;
        }
    }
}