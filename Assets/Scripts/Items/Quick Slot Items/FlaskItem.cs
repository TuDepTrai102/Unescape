using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Estus Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("FLASK TYPE")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("RECOVERY AMOUNT")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("RECOVERY FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerManager player)
        {
            base.AttemptToConsumeItem(player);
            GameObject flask = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
            player.playerEffectsManager.currentParticleFX = recoveryFX;
            player.playerEffectsManager.amountToBeHealed = healthRecoverAmount;
            player.playerEffectsManager.instantiatedFXModel = flask;
            player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}