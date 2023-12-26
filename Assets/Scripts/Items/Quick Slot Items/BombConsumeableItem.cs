using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
    public class BombConsumeableItem : ConsumableItem
    {
        [Header("VELOCITY")]
        public int upwardVelocity = 50;
        public int forwardVelocity = 50;
        public int bombMass = 1;

        [Header("LIVE BOMB MODEL")]
        public GameObject liveBombModel;

        [Header("BASE DAMAGE")]
        public int baseDamage = 200;
        public int explosiveDamage = 75;

        public override void AttemptToConsumeItem(PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
                player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, true);
                GameObject bombModel = Instantiate
                    (itemModel, player.playerWeaponSlotManager.rightHandSlot.transform.position, 
                    Quaternion.identity, player.playerWeaponSlotManager.rightHandSlot.transform);
                player.playerEffectsManager.instantiatedFXModel = bombModel;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_cannot_cast, true);
            }
        }
    }
}