using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Parry Action")]
    public class ParryAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            character.characterAnimatorManager.EraseHandIKForWeapon();

            WeaponItem parryingWeapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;

            //check if parrying weapon is a fast parry or a medium parry weapon
            if (parryingWeapon.weaponType == WeaponType.SmallShield)
            {
                //fast parry anim
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.weaponArt_Parry_Small_Shield, true);
            }
            else if (parryingWeapon.weaponType == WeaponType.Shield)
            {
                //normal parry anim
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.weaponArt_Parry_Normal_Shield, true);
            }
        }
    }
}