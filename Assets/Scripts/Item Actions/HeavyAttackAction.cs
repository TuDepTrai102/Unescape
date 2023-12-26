using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
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
                if (!player._playerSkills._combat_JumpAttackSkill._combat_JumpAttackUnlocked)
                {
                    Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                }
                else
                {
                    HandleJumpingAttack(character);
                    return;
                }
            }

            //IF WE CANDO A COMBO, WE COMBO
            if (character.canDoCombo)
            {
                if (player != null)
                {
                    if (!player._playerSkills._combat_HeavyAttackComboSkill._combat_HeavyComboAttackUnlocked)
                    {
                        Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                    }
                    else
                    {
                        HandleHeavyWeaponCombo(character);
                    }
                }
                else
                {
                    HandleHeavyWeaponCombo(character);
                }
            }
            //IF NOT, WE PERFORM A NORMAL ATTACK
            else
            {
                if (character.isInteracting)
                    return;

                if (character.canDoCombo)
                    return;

                if (player != null)
                {
                    if (!player._playerSkills._combat_HeavyAttackSkill._combat_HeavyAttackUnlocked)
                    {
                        Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                    }
                    else
                    {
                        HandleHeavyAttack(character);
                    }
                }
                else
                {
                    HandleHeavyAttack(character);
                }
            }

            character.characterCombatManager.currentAttackType = AttackType.heavy;
        }

        private void HandleHeavyAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                    character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_01;
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_01;
                    }
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_heavy_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_heavy_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_heavy_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_heavy_attack_01;
                    }
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_01;
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_01;
                        }
                    }
                }
            }
        }

        private void HandleJumpingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                    character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_jumping_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_jumping_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_jumping_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_jumping_attack_01;
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_jumping_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_jumping_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_jumping_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_jumping_attack_01;
                    }
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_jumping_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_jumping_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_jumping_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_jumping_attack_01;
                    }
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_jumping_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_jumping_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_jumping_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_jumping_attack_01;
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_jumping_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_jumping_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_jumping_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_jumping_attack_01;
                        }
                    }
                }
            }
        }

        private void HandleHeavyWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_heavy_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_heavy_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_01;
                        }
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_heavy_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_heavy_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_heavy_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_heavy_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_heavy_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_heavy_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_01;
                            }
                        }
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHandingWeapon)
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_heavy_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_heavy_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_heavy_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_heavy_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_heavy_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_heavy_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_spear_heavy_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_heavy_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_spear_heavy_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_heavy_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_heavy_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_heavy_attack_01;
                            }
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                            character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_heavy_attack_01)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_heavy_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_heavy_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                                 character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_heavy_attack_01)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_heavy_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_heavy_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_heavy_attack_01;
                            }
                        }
                        else
                        {
                            if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_heavy_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_heavy_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_heavy_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_heavy_attack_01;
                                }
                            }
                            else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_heavy_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_heavy_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_heavy_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_heavy_attack_01;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}