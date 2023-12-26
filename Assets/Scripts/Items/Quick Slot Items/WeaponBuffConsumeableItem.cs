using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Consumables/Weapon Buff")]
    public class WeaponBuffConsumeableItem : ConsumableItem
    {
        [Header("EFFECTS")]
        [SerializeField] public WeaponBuffEffect weaponBuffEffect;

        [Header("BUFF SOUND FX")]
        [SerializeField] AudioClip buffTriggerSound;

        public override void AttemptToConsumeItem(PlayerManager player)
        {
            //IF I CAN'T USE THIS ITEM, RETURN WITHOUT DOING ANYTHING
            if (!CanIUseThisItem(player))
                return;

            if (currentItemAmount > 0)
            {
                player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_cannot_cast, true);
            }
        }

        public override void SuccessfullyConsumeItem(PlayerManager player)
        {
            base.SuccessfullyConsumeItem(player);

            player.characterSoundFXManager.PlaySoundFX(buffTriggerSound);

            WeaponBuffEffect weaponBuff = Instantiate(weaponBuffEffect);
            weaponBuff.isRightHandedBuff = true;
            player.playerEffectsManager.rightWeaponBuffEffect = weaponBuff;
            player.playerEffectsManager.ProcessWeaponBuffs();
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (player.playerInventoryManager.currentConsumable.currentItemAmount <= 0)
                return false;

            MeleeWeaponItem meleeWeapon = player.playerInventoryManager.rightWeapon as MeleeWeaponItem;

            if (meleeWeapon != null && meleeWeapon.canBuffed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}