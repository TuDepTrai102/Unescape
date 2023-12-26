using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;

            if (character.isHoldingArrow)
                return;

            if (character.isTwoHandingWeapon)
            {
                //ANIMATE PLAYER
                character.animator.SetBool("isHoldingArrow", true);
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_bow_draw_01, true);

                //INSTANTIATE ARROW
                GameObject loadedArrow = Instantiate
                    (character.characterInventoryManager.currentAmmo.loadedItemModel,
                    character.characterWeaponSlotManager.leftHandSlot.transform);
                character.characterEffectsManager.instantFXModel = loadedArrow;

                //ANIMATE BOW
                Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
                bowAnimator.SetBool("isDrawn", true);
                bowAnimator.Play("Bow_ONLY_Draw_01");
            }           
        }
    }
}