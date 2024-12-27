#region usings
using GameNetcodeStuff;
using FlaskOfFortuneAndFolly.Handlers;
using System.Collections;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
#endregion

namespace FlaskOfFortuneAndFolly.Scrap
{
    internal class Flask : BaseScrapItem
    {
        private System.Random random = new System.Random();
        private string flaskEffect = "None";

        internal Flask(GrabbableObject flask) : base(flask)
        {
            ItemName = "Suspicious Flask";
            // Update Visible Scrap Name
            BaseScrap.gameObject.GetComponentInChildren<ScanNodeProperties>().headerText = ItemName;

            ItemDescription = "Should I really risk drinking the content?";
            RandomizeEffect();
        }

        public override void UpdateItem()
        {
            if (ItemPropertiesDiscovered)
            {
                // Update Visible Scrap Name
                BaseScrap.gameObject.GetComponentInChildren<ScanNodeProperties>().headerText = ItemName;
            }

            // Reset modified state
            ItemModified = false;
        }

        public override void InspectItem()
        {
            if (!BaseScrap.itemUsedUp)
            {
                HUDManager.Instance.DisplayTip($"{ItemName}", $"{ItemDescription}");
            }
            else
                HUDManager.Instance.DisplayTip($"{ItemName}", "... the taste, was not that great.");
        }

        public override void UseItem()
        {
            if (!BaseScrap.itemUsedUp)
            {
                BaseScrap.itemUsedUp = true;

                HoldingPlayer.StartCoroutine(DelayedActivation(3f, () =>
                    {
                        switch (flaskEffect)
                        {
                            default:
                            case "None":
                                NoEffect();
                                break;
                            case "Intoxication":
                                ApplyDrunkEffect(HoldingPlayer);
                                break;
                            case "Poisoning":
                                HoldingPlayer.StartCoroutine(ApplyPoisonEffect(HoldingPlayer));
                                break;
                            case "Healing":
                                HoldingPlayer.StartCoroutine(ApplyHealEffect(HoldingPlayer));
                                break;
                            case "InvertedControls":
                                ApplyInvertedControlsEffect(HoldingPlayer);
                                break;
                            case "Teleportation":
                                ApplyTeleportationEffect(HoldingPlayer);
                                break;
                            case "Fatigue":
                                ApplyFatigueEffect(HoldingPlayer);
                                break;
                            case "Combustion":
                                ApplyCombustionEffect(HoldingPlayer);
                                break;
                            case "Blindness":
                                ApplyBlindnessEffect(HoldingPlayer);
                                break;
                            case "Dysphasia":
                                ApplyDysphasiaEffect(HoldingPlayer);
                                break;
                            case "Power":
                                ApplyPowerEffect(HoldingPlayer);
                                break;
                            case "NightVision":
                                ApplyNightVisionEffect(HoldingPlayer);
                                break;
                            case "Scatter":
                                ApplyScatterEffect(HoldingPlayer);
                                break;
                            case "Escape":
                                ApplyEscapeEffect(HoldingPlayer);
                                break;
                        }
                    }));
                BaseScrap.SetScrapValue(3);
            }
        }

        public override void SecondaryUseItem()
        {
            throw new System.NotImplementedException();
        }

        private static int[] GetEffectProbabilitiesFromConfig()
        {
            int[] probabilities = new int[14]; // incr every time a new effect is added

            // Assign effect probabilities based on configuration
            // Todo check if the config can be null since set in the Awake method.. failsafe here might be obsolete 
            probabilities[0] = FlaskOfFortuneAndFollyPlugin.NoEffectChance?.Value ?? 10;
            probabilities[1] = FlaskOfFortuneAndFollyPlugin.IntoxicationChance?.Value ?? 50;
            probabilities[2] = FlaskOfFortuneAndFollyPlugin.PoisoningChance?.Value ?? 50;
            probabilities[3] = FlaskOfFortuneAndFollyPlugin.HealingChance?.Value ?? 2;
            probabilities[4] = FlaskOfFortuneAndFollyPlugin.InvertedControlsChance?.Value ?? 50;
            probabilities[5] = FlaskOfFortuneAndFollyPlugin.TeleportationChance?.Value ?? 10;
            probabilities[6] = FlaskOfFortuneAndFollyPlugin.FatigueChance?.Value ?? 50;
            probabilities[7] = FlaskOfFortuneAndFollyPlugin.CombustionChance?.Value ?? 10;
            probabilities[8] = FlaskOfFortuneAndFollyPlugin.BlindnessChance?.Value ?? 50;
            probabilities[9] = FlaskOfFortuneAndFollyPlugin.DysphasiaChance?.Value ?? 33;
            probabilities[10] = FlaskOfFortuneAndFollyPlugin.PowerChance?.Value ?? 5;
            probabilities[11] = FlaskOfFortuneAndFollyPlugin.NightVisionChance?.Value ?? 3;
            probabilities[12] = FlaskOfFortuneAndFollyPlugin.ScatterChance?.Value ?? 50;
            probabilities[13] = FlaskOfFortuneAndFollyPlugin.EscapeChance?.Value ?? 1;

            return probabilities;
        }

        private void RandomizeEffect()
        {
            int totalProbability = 0;
            int[] probabilities = GetEffectProbabilitiesFromConfig();

            for (int i = 0; i < probabilities.Length; i++)
            {
                totalProbability += probabilities[i];
            }

            // If no effects are enabled, set NoEffect as default
            if (totalProbability == 0)
                return;

            // Generate a random number within the total probability space
            int randomNum = random.Next(totalProbability);

            // Determine the selected effect based on the random number
            int cumulativeProbability = 0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomNum < cumulativeProbability)
                {
                    // Effect i is selected
                    switch (i)
                    {
                        case 0:
                            flaskEffect = "NoEffect";
                            ItemName = "Clear Flask";
                            ItemDescription = "The liquid inside the flask is clear...";
                            break;
                        case 1:
                            flaskEffect = "Intoxication";
                            ItemName = "Amber Flask";
                            ItemDescription = "A sweet aroma is oozing out...";
                            break;
                        case 2:
                            flaskEffect = "Poisoning";
                            ItemName = "Green Flask";
                            ItemDescription = "Inside there is a greenish tar like substance...";
                            break;
                        case 3:
                            flaskEffect = "Healing";
                            ItemName = "Warm Flask";
                            ItemDescription = "It feels warm to the touch...";
                            break;
                        case 4:
                            flaskEffect = "InvertedControls";
                            ItemName = "Twisted Flask";
                            ItemDescription = "The liquid swirls in a strange pattern...";
                            break;
                        case 5:
                            flaskEffect = "Teleportation";
                            ItemName = "Mystic Flask";
                            ItemDescription = "The liquid shimmers with an otherworldly glow...";
                            break;
                        case 6:
                            flaskEffect = "Fatigue";
                            ItemName = "Dull Flask";
                            ItemDescription = "The liquid looks murky and unappealing...";
                            break;
                        case 7:
                            flaskEffect = "Combustion";
                            ItemName = "Fiery Flask";
                            ItemDescription = "The liquid inside seems to be boiling...";
                            break;
                        case 8:
                            flaskEffect = "Blindness";
                            ItemName = "Dark Flask";
                            ItemDescription = "The liquid is pitch black...";
                            break;
                        case 9:
                            flaskEffect = "Dysphasia";
                            ItemName = "Confusing Flask";
                            ItemDescription = "The liquid changes color constantly...";
                            break;
                        case 10:
                            flaskEffect = "Power";
                            ItemName = "Strong Flask";
                            ItemDescription = "The liquid radiates a powerful aura...";
                            break;
                        case 11:
                            flaskEffect = "NightVision";
                            ItemName = "Luminous Flask";
                            ItemDescription = "The liquid glows faintly in the dark...";
                            break;
                        case 12:
                            flaskEffect = "Scatter";
                            ItemName = "Chaotic Flask";
                            ItemDescription = "The liquid bubbles erratically...";
                            break;
                        case 13:
                            flaskEffect = "Escape";
                            ItemName = "Swift Flask";
                            ItemDescription = "The liquid moves rapidly within the flask...";
                            break;
                    }
                    break;
                }
            }
        }

        private void NoEffect()
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Nothing", "Well now we are certain that it does nothing...");
            else
                HUDManager.Instance.DisplayTip("Nothing", "Huh... nothing happened...");
        }

        private void ApplyDrunkEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Intoxication", "Don't drink and drive!");
            else
                HUDManager.Instance.DisplayTip("Intoxication", "You're feeling a bit dizzy...");

            // Make the HoldingPlayer drunk
            AudioHandler.PlaySound(player, "Scrap\\Flask\\Intoxication.mp3");
            player.drunkness = 1f;
        }

        private IEnumerator ApplyPoisonEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("You've been poisoned", "I don't know what you expected to happen...", true);
            else
                HUDManager.Instance.DisplayTip("Poisoning Effect", "You feel a burning sensation...");

            float elapsedTime = 0f;

            while (player.health > FlaskOfFortuneAndFollyPlugin.MaxPoison.Value)
            {
                // Decrement HoldingPlayer's health gradually
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 3f) // Adjust the duration as needed
                {
                    player.DamagePlayer(1, false);
                    elapsedTime = 0f;

                    // Update UI
                    HUDManager.Instance.UpdateHealthUI(player.health, true);
                }

                yield return null; // Wait for the next frame
            }
        }

        private IEnumerator ApplyHealEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Healing", "You feel rejuvenated...");
            else
                HUDManager.Instance.DisplayTip("Healing", "You somewhat feel better...");

            const int minScrapValue = 16;
            const int maxScrapValue = 44;
            int minHealing = 1;
            int maxHealing = FlaskOfFortuneAndFollyPlugin.MaxHealing.Value;
            int healing;

            if (BaseScrap.scrapValue <= minScrapValue)
                healing = minHealing;
            else if (BaseScrap.scrapValue >= maxScrapValue)
                healing = maxHealing;
            else
            {
                float percentage = (float)(BaseScrap.scrapValue - minScrapValue) / (maxScrapValue - minScrapValue);
                healing = Mathf.RoundToInt(Mathf.Lerp(minHealing, maxHealing, percentage));
            }

            int targetHealth = player.health + healing;

            float elapsedTime = 0f;

            while (player.health < targetHealth && player.health < 100)
            {
                // Increment HoldingPlayer's health gradually
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 3f)
                {
                    player.health++;
                    elapsedTime = 0f;

                    // Ensure health does not exceed the target health
                    player.health = Mathf.Min(player.health, targetHealth);

                    // Update UI
                    HUDManager.Instance.UpdateHealthUI(player.health, false);
                }
                yield return null; // Wait for the next frame
            }
        }

        private void ApplyInvertedControlsEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Inverted Controls", "Your controls are inverted!");
            else
                HUDManager.Instance.DisplayTip("Inverted Controls", "Something feels off...");

            // Implement the effect logic here
        }

        private void ApplyTeleportationEffect(PlayerControllerB player)
        {

            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Teleportation", "You have been teleported!");
            else
                HUDManager.Instance.DisplayTip("Teleportation", "You feel disoriented...");

            // Find possible teleportation destinations
            List<Vector3> possibleDestinations = new List<Vector3>();

            // Differenciate between inside and outside teleportation
            if (player.isInsideFactory)
            {
                FlowermanAI bracken = StartOfRound.FindAnyObjectByType<FlowermanAI>();
                EnemyAI insideMonster = StartOfRound.FindObjectOfType<EnemyAI>();
                GameObject secretRoom = GameObject.Find("SmallRoom2(Clone)");


                if (secretRoom != null)
                    possibleDestinations.Add(secretRoom.transform.position); // Adds Secrect Room as possible destination

                if (bracken != null && bracken.movingTowardsTargetPlayer)
                    possibleDestinations.Add(bracken.favoriteSpot.position); // Adds Bracken's favorite spot as possible destination

                if (insideMonster != null && !insideMonster.isOutside)
                    possibleDestinations.Add(insideMonster.ChooseClosestNodeToPosition(insideMonster.transform.position, true, 5).position); // Adds Inside Monster's closest node as possible destination

                possibleDestinations.Add(RoundManager.FindMainEntrancePosition(true)); // Adds Main Entrance as possible destination

            }
            else
            {
                possibleDestinations.Add(RoundManager.FindMainEntrancePosition(true, true)); // Adds Main Entrance as possible destination
                possibleDestinations.Add(RoundManager.FindAnyObjectByType<ItemDropship>().itemSpawnPositions[1].position); // Adds Dropship as possible destination
                possibleDestinations.Add(RoundManager.Instance.shipSpawnPathPoints[0].position); // Adds Ship as possible destination
            }

            // Randomly select a teleport target
            Vector3 selectedTarget = possibleDestinations[random.Next(possibleDestinations.Count)];
            player.TeleportPlayer(selectedTarget);


            player.SpawnPlayerAnimation();

            // TODO:
            // - Make teleport destination configurable
            // - Add a teleportation sound
            // - Add a teleportation visual effect
            // - Add a teleportation to random location
            // -------------------------------------------------
            // TOTEST:
            // - Teleportation to secret room
            // - Teleportation to Bracken's favorite spot
            // - Teleportation to Inside Monster's closest node
            // - Teleportation to Main Entrance
            // -------------------------------------------------
            // TOFIX:
            // - Backroom Teleportation spawns you inside of wall of the room
            // - Teleportation inside makes the player not targetable 
            // --> Differenciate between inside and outside teleportation
        }

        private void ApplyFatigueEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Fatigue", "You feel extremely tired...");
            else
                HUDManager.Instance.DisplayTip("Fatigue", "You feel a bit sluggish...");

            // Implement the effect logic here
        }

        private void ApplyCombustionEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Combustion", "You are on fire!");
            else
                HUDManager.Instance.DisplayTip("Combustion", "You feel a sudden heat...");

            // Implement the effect logic here
        }

        private void ApplyBlindnessEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Blindness", "You can't see anything!");
            else
                HUDManager.Instance.DisplayTip("Blindness", "Your vision is fading...");

            // Implement the effect logic here
        }

        private void ApplyDysphasiaEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Dysphasia", "You can't speak properly!");
            else
                HUDManager.Instance.DisplayTip("Dysphasia", "Your words are jumbled...");

            // Implement the effect logic here
        }

        private void ApplyPowerEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Power", "You feel incredibly strong!");
            else
                HUDManager.Instance.DisplayTip("Power", "You feel a surge of energy...");

            // Implement the effect logic here
        }

        private void ApplyNightVisionEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Night Vision", "You can see in the dark!");
            else
                HUDManager.Instance.DisplayTip("Night Vision", "Your vision adapts to the darkness...");

            // Implement the effect logic here
        }

        private void ApplyScatterEffect(PlayerControllerB player)
        {
            if (ItemPropertiesDiscovered)
                HUDManager.Instance.DisplayTip("Scatter", "You feel scattered!");
            else
                HUDManager.Instance.DisplayTip("Scatter", "You feel disoriented...");

            player.DropAllHeldItemsAndSync();
        }
    }
}
