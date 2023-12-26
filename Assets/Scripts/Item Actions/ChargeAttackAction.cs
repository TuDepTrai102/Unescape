using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            character._isChargingAttack = true;
            character.characterAnimatorManager.EraseHandIKForWeapon();

            if (character.isUsingLeftHand)
            {
                character.characterEffectsManager.PlayWeaponFX(true);
            }
            else if (character.isUsingRightHand)
            {
                character.characterEffectsManager.PlayWeaponFX(false);
            }

            // if we can do a combo, we combo
            if (character.canDoCombo)
            {
                if (!player._playerSkills._combat_ChargeAttackComboSkill._combat_ChargeAttackComboUnlocked)
                {
                    Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                }
                else
                {
                    character.canDoCombo = true;
                    HandleChargeWeaponCombo(character);
                    character.canDoCombo = false;
                }
            }
            // if not, we perform a regular attack
            else
            {
                if (character.isInteracting)
                    return;

                if (character.canDoCombo)
                    return;

                if (!player._playerSkills._combat_ChargeAttackSkill._combat_ChargeAttackUnlocked)
                {
                    Debug.Log("BRO, WE NOT LEARN THIS SKILL MAN [T.T!!!]");
                }
                else
                {
                    HandleChargeAttack(character);
                }
            }

            character.characterCombatManager.currentAttackType = AttackType.charge;
        }

        private void HandleChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                    character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_01;
                }
                else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                         character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    character.isUsingRightHand = true;
                    character.characterEffectsManager.PlayWeaponFX(false);
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_01, true, false, character.isUsingLeftHand);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_01;
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_01, true, false, character.isUsingLeftHand);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_01;
                    }
                }
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHandingWeapon)
                {
                    if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_charge_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_charge_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_charge_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_charge_attack_01;
                    }
                }
                else
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_01;
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        character.isUsingLeftHand = true;
                        character.characterEffectsManager.PlayWeaponFX(true);
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_01, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_01;
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_01;
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                        {
                            int _random_OH_ChargeAttack = Random.Range(1, 3);

                            if (_random_OH_ChargeAttack == 1)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_charge_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_charge_attack_01;
                            }
                            else if (_random_OH_ChargeAttack == 2)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_charge_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_charge_attack_02;
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_great_sword_charge_attack_03, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_great_sword_charge_attack_03;
                            }
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_01;
                        }
                    }
                }
            }
        }

        private void HandleChargeWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                        character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_charge_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_charge_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_heavy_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_01;
                        }
                    }
                    else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                             character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_charge_attack_01)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_02, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_02;
                        }
                        else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_charge_attack_02)
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_03, true, false, character.isUsingLeftHand);
                        }
                        else
                        {
                            character.isUsingRightHand = true;
                            character.characterEffectsManager.PlayWeaponFX(false);
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_01, true, false, character.isUsingLeftHand);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_01;
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_charge_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_charge_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_charge_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_02, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_charge_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_03, true, false, character.isUsingLeftHand);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_01, true, false, character.isUsingLeftHand);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_01;
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
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_charge_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_charge_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_charge_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_charge_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_straight_sword_charge_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_straight_sword_charge_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_straight_sword_charge_attack_01)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_charge_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.th_spear_charge_attack_02)
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_charge_attack_03, true);
                            }
                            else
                            {
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_spear_charge_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.th_spear_charge_attack_01;
                            }
                        }
                    }
                    else
                    {
                        if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword &&
                            character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_charge_attack_01)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_straight_sword_charge_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_straight_sword_charge_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_straight_sword_charge_attack_01;
                            }
                        }
                        else if (character.characterWeaponSlotManager.leftHandSlot.currentWeapon.weaponType == WeaponType._Spear &&
                                 character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                        {
                            if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_charge_attack_01)
                            {
                                character.isUsingRightHand = true;
                                character.characterEffectsManager.PlayWeaponFX(false);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_02, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_02;
                            }
                            else if (character.characterCombatManager.lastAttack == character.characterCombatManager.dw_spear_charge_attack_02)
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_03, true);
                            }
                            else
                            {
                                character.isUsingLeftHand = true;
                                character.characterEffectsManager.PlayWeaponFX(true);
                                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.dw_spear_charge_attack_01, true);
                                character.characterCombatManager.lastAttack = character.characterCombatManager.dw_spear_charge_attack_01;
                            }
                        }
                        else
                        {
                            if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_charge_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_straight_sword_charge_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_straight_sword_charge_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_straight_sword_charge_attack_01;
                                }
                            }
                            else if (character.characterWeaponSlotManager.rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                            {
                                if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_charge_attack_01)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_02, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_02;
                                }
                                else if (character.characterCombatManager.lastAttack == character.characterCombatManager.oh_spear_charge_attack_02)
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_03, true);
                                }
                                else
                                {
                                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.oh_spear_charge_attack_01, true);
                                    character.characterCombatManager.lastAttack = character.characterCombatManager.oh_spear_charge_attack_01;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}