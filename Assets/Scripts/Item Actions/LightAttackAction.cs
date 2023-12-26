using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
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
                if (!player._playerSkills._combat_RunAttackSkill._combat_RunAttackUnlocked)
                {
                    Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                }
                else
                {
                    HandleRunningAttack(character);
                    return;
                }
            }

            //IF WE CANDO A COMBO, WE COMBO
            if (character.canDoCombo)
            {
                if (player != null)
                {
                    if (!player._playerSkills._combat_ComboAttackSkill._combat_ComboAttackUnlocked)
                    {
                        Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                    }
                    else
                    {
                        HandleLightWeaponCombo(character);
                    }
                }
                else
                {
                    HandleLightWeaponCombo(character);
                }
            }
            //IF NOT, WE PERFORM A NORMAL ATTACK
            else
            {
                if (character.isInteracting)
                    return;

                if (character.canDoCombo)
                    return;

                HandleLightAttack(character);
            }

            character.characterCombatManager.currentAttackType = AttackType.light;
        }

        private void HandleLightAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                    character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_01;
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                    {
                        int _random_OH_ChargeAttack = Random.Range(1, 4);

                        if (_random_OH_ChargeAttack == 1)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_1, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_1;
                        }
                        else if (_random_OH_ChargeAttack == 2)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_1, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_02_1;
                        }
                        else if (_random_OH_ChargeAttack == 3)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_1, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_1;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_1, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_1;
                        }
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_01;
                    }
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_light_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_light_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_light_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_light_attack_01;
                    }
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_01;
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                        {
                            int _random_greatSword_OH_LightAttack = Random.Range(1, 4);

                            if (_random_greatSword_OH_LightAttack == 1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_1, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_1;
                            }
                            else if (_random_greatSword_OH_LightAttack == 2)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_1, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_02_1;
                            }
                            else if (_random_greatSword_OH_LightAttack == 3)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_1, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_1;
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_1, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_1;
                            }
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_01;
                        }
                    }
                }
            }
        }

        private void HandleRunningAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                    character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_running_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_running_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_01;
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_running_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_running_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_running_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_running_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_running_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_running_attack_01;
                    }
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_running_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_running_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        int random_TH_RunAttack = Random.Range(1, 3);

                        if (random_TH_RunAttack == 1)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_running_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_running_attack_01;
                        }
                        else if (random_TH_RunAttack == 2)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_running_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_running_attack_02;
                        }
                        else if (random_TH_RunAttack == 3)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_running_attack_03, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_running_attack_03;
                        }
                    }
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_running_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_running_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        int random_DW_RunAttack = Random.Range(1, 5);

                        if (random_DW_RunAttack == 1)
                        {
                            character.isUsingLeftHand = true;
                            character.characterEffectsManager.PlayWeaponFX(true);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_01;
                        }
                        else if (random_DW_RunAttack == 2)
                        {
                            character.isUsingLeftHand = true;
                            character.characterEffectsManager.PlayWeaponFX(true);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_02;
                        }
                        else if (random_DW_RunAttack == 3)
                        {
                            character.isUsingLeftHand = true;
                            character.characterEffectsManager.PlayWeaponFX(true);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_03, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_03;
                        }
                        else if (random_DW_RunAttack == 4)
                        {
                            character.isUsingLeftHand = true;
                            character.characterEffectsManager.PlayWeaponFX(true);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_04, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_04;
                        }
                        else
                        {
                            character.isUsingLeftHand = true;
                            character.characterEffectsManager.PlayWeaponFX(true);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_running_attack_05, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_running_attack_05;
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_running_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_running_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_running_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_running_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_running_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_running_attack_01;
                        }
                    }
                }
            }
        }

        private void HandleLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_light_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_light_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_01;
                        }
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_light_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_light_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_light_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_light_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_01_1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_2, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_2;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_01_2)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_3, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_1, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_1;
                            }


                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_02_1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_2, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_02_2;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_02_2)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_3, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_1, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_1;
                            }


                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_03_1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_2, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_2;
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_1, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_1;
                            }


                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_04_1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_2, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_2;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_04_2)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_3, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_1, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_1;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_light_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_light_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_01;
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
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_light_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_light_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_light_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_light_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_light_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_light_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_spear_light_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_light_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_spear_light_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_light_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_light_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_light_attack_01;
                            }
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                            character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_light_attack_01)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_light_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_light_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_light_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                                 character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_light_attack_01)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_light_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_light_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_light_attack_01;
                            }
                        }
                        else
                        {
                            if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_light_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_light_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_light_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_light_attack_01;
                                }
                            }
                            else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_01_1)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_2, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_2;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_01_2)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_3, true);
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_02_1)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_2, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_02_2;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_02_2)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_3, true);
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_03_1)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_2, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_2;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_04_1)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_2, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_2;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_great_sword_light_attack_combo_04_2)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_3, true);
                                }
                                else
                                {
                                    int _random_greatSword_OH_LightAttack = Random.Range(1, 4);

                                    if (_random_greatSword_OH_LightAttack == 1)
                                    {
                                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_01_1, true);
                                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_01_1;
                                    }
                                    else if (_random_greatSword_OH_LightAttack == 2)
                                    {
                                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_02_1, true);
                                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_02_1;
                                    }
                                    else if (_random_greatSword_OH_LightAttack == 3)
                                    {
                                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_03_1, true);
                                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_03_1;
                                    }
                                    else
                                    {
                                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_light_attack_combo_04_1, true);
                                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_light_attack_combo_04_1;
                                    }
                                }
                            }
                            else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_light_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_light_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_light_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_light_attack_01;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}