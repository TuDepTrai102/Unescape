using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public override void LoadWeaponOnSLot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    player.uiManager.quickSlotsUI.UpdateWeaponIconQuickSlots(true, weaponItem);
                    //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (player.inputHandler.twoHandFlag && !player.isBlocking)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        //player.playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.uiManager.quickSlotsUI.UpdateWeaponIconQuickSlots(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    player.playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    player.uiManager.quickSlotsUI.UpdateWeaponIconQuickSlots(true, weaponItem);
                    //player.playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    player.playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    player.uiManager.quickSlotsUI.UpdateWeaponIconQuickSlots(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        public void SuccessfullyThrowFireBomb()
        {
            Destroy(player.playerEffectsManager.instantiatedFXModel);
            BombConsumeableItem fireBombItem = player.playerInventoryManager.currentConsumable as BombConsumeableItem;

            GameObject activeModelBomb = Instantiate
                (fireBombItem.liveBombModel, 
                rightHandSlot.transform.position,
                player.cameraHandler.cameraPivotTransform.rotation);
            activeModelBomb.transform.rotation = Quaternion.Euler
                (player.cameraHandler.cameraPivotTransform.eulerAngles.x, 
                player.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosionDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            damageCollider.bombRigidBody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
            LoadWeaponOnSLot(player.playerInventoryManager.rightWeapon, false);
        }
    }
}