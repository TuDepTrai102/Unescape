using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpConsumeableItem : ConsumableItem
    {
        [Header("RECOVERY FX")]
        public GameObject clumpConsumeFX;

        [Header("CURE")]
        public bool curePoison;
        public bool _cureBleed;
        public bool _cureCurse;

        public override void AttemptToConsumeItem(PlayerManager player)
        {
            base.AttemptToConsumeItem(player);
            GameObject clump = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
            player.playerEffectsManager.currentParticleFX = clumpConsumeFX;
            player.playerEffectsManager.instantiatedFXModel = clump;

            if (curePoison)
            {
                player.playerStatsManager.poisonBuildup = 0;
                player.playerStatsManager.poisonAmount = 100;
                player.playerStatsManager.isPoisoned = false;

                player.playerEffectsManager.poisonAmountBar.SetPoisonAmount(0);
            }

            if (_cureBleed)
            {
                player.playerStatsManager._bleedBuildup = 0;
                player.playerStatsManager._bleedAmount = 100;
                player.playerStatsManager._isBleeded = false;

                player.playerEffectsManager._bleedAmountBar._SetBleedAmount(0);
            }

            if (_cureCurse)
            {
                player.playerStatsManager._curseBuildup = 0;
                player.playerStatsManager._curseAmount = 100;
                player.playerStatsManager._isCursed = false;

                player.playerEffectsManager._curseAmountBar._SetCurseAmount(0);
            }

            player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}