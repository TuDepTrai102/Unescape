using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Dash Attack Action")]
    public class _DashAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();

            if (character.isUsingLeftHand)
            {
                character.characterEffectsManager.PlayWeaponFX(true);
            }
            else if (character.isUsingRightHand)
            {
                character.characterEffectsManager.PlayWeaponFX(false);
            }

            //IF WE CAN PERFORM A RUNNING ATTACK, WE DOT THAT (IF NOT, CONTINUE)
            if (character.isSprinting)
            {
                if (!player._playerSkills._combat_DashAttackSkill._combat_DashAttackUnlocked)
                {
                    Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                }
                else
                {
                    _HandleDashAttack(character);
                    return;
                }
            }

            character.characterCombatManager.currentAttackType = AttackType.heavy;
        }

        private void _HandleDashAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_dash_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_dash_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_dash_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_dash_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_dash_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_dash_attack_01;
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    //NOT HAVE DASH ANIMATION WITH TWO HANDING WEAPON


                }
                else
                {
                    if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_dash_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_dash_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_dash_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_dash_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_dash_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_dash_attack_01;
                    }
                }
            }
        }
    }
}