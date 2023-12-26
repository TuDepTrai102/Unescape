using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("UNARMED WEAPON")]
        public WeaponItem unarmedWeapon;

        [Header("WEAPON SLOTS")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("DAMAGE COLLIDERS")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("HAND IK TARGETS")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();
        }

        private void Start()
        {

        }

        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlot = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlot)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }

        public virtual void LoadBothWeaponOnSlots()
        {
            LoadWeaponOnSLot(character.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSLot(character.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSLot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                }
                else
                {
                    if (character.isTwoHandingWeapon)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        //character.characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(character.isTwoHandingWeapon);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;

                if (isLeft)
                {
                    character.characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    //character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    character.characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        protected virtual void LoadLeftWeaponDamageCollider()
        {
            if (leftHandSlot.currentWeapon != null)
            {
                if (leftHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                    leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                    leftHandDamageCollider._lightningDamage = character.characterInventoryManager.leftWeapon._lightningDamage;
                    leftHandDamageCollider._darkDamage = character.characterInventoryManager.leftWeapon._darkDamage;
                    leftHandDamageCollider._magicDamage = character.characterInventoryManager.leftWeapon._magicDamage;
                    leftHandDamageCollider._bleedDamage = character.characterInventoryManager.leftWeapon._bleedDamage;

                    leftHandDamageCollider.characterManager = character;
                    leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                    character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (leftHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                {
                    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                    leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                    leftHandDamageCollider._lightningDamage = character.characterInventoryManager.leftWeapon._lightningDamage;
                    leftHandDamageCollider._darkDamage = character.characterInventoryManager.leftWeapon._darkDamage;
                    leftHandDamageCollider._magicDamage = character.characterInventoryManager.leftWeapon._magicDamage;
                    leftHandDamageCollider._bleedDamage = character.characterInventoryManager.leftWeapon._bleedDamage;

                    leftHandDamageCollider.characterManager = character;
                    leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                    character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (leftHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                    leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                    leftHandDamageCollider._lightningDamage = character.characterInventoryManager.leftWeapon._lightningDamage;
                    leftHandDamageCollider._darkDamage = character.characterInventoryManager.leftWeapon._darkDamage;
                    leftHandDamageCollider._magicDamage = character.characterInventoryManager.leftWeapon._magicDamage;
                    leftHandDamageCollider._bleedDamage = character.characterInventoryManager.leftWeapon._bleedDamage;

                    leftHandDamageCollider.characterManager = character;
                    leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                    character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (leftHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                {
                    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                    leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                    leftHandDamageCollider._lightningDamage = character.characterInventoryManager.leftWeapon._lightningDamage;
                    leftHandDamageCollider._darkDamage = character.characterInventoryManager.leftWeapon._darkDamage;
                    leftHandDamageCollider._magicDamage = character.characterInventoryManager.leftWeapon._magicDamage;
                    leftHandDamageCollider._bleedDamage = character.characterInventoryManager.leftWeapon._bleedDamage;

                    leftHandDamageCollider.characterManager = character;
                    leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                    character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (leftHandSlot.currentWeapon.weaponType == WeaponType.SmallShield)
                {
                    leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
                    leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;
                    leftHandDamageCollider._lightningDamage = character.characterInventoryManager.leftWeapon._lightningDamage;
                    leftHandDamageCollider._darkDamage = character.characterInventoryManager.leftWeapon._darkDamage;
                    leftHandDamageCollider._magicDamage = character.characterInventoryManager.leftWeapon._magicDamage;
                    leftHandDamageCollider._bleedDamage = character.characterInventoryManager.leftWeapon._bleedDamage;

                    leftHandDamageCollider.characterManager = character;
                    leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    leftHandDamageCollider.poiseDamage = character.characterInventoryManager.leftWeapon.poiseBreak;
                    character.characterEffectsManager.leftWeaponManager = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
            }
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            if (rightHandSlot.currentWeapon != null)
            {
                if (rightHandSlot.currentWeapon.weaponType == WeaponType.StraightSword)
                {
                    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                    rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                    rightHandDamageCollider._lightningDamage = character.characterInventoryManager.rightWeapon._lightningDamage;
                    rightHandDamageCollider._darkDamage = character.characterInventoryManager.rightWeapon._darkDamage;
                    rightHandDamageCollider._magicDamage = character.characterInventoryManager.rightWeapon._magicDamage;
                    rightHandDamageCollider._bleedDamage = character.characterInventoryManager.rightWeapon._bleedDamage;

                    rightHandDamageCollider.characterManager = character;
                    rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                    character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (rightHandSlot.currentWeapon.weaponType == WeaponType._GreatSword)
                {
                    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                    rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                    rightHandDamageCollider._lightningDamage = character.characterInventoryManager.rightWeapon._lightningDamage;
                    rightHandDamageCollider._darkDamage = character.characterInventoryManager.rightWeapon._darkDamage;
                    rightHandDamageCollider._magicDamage = character.characterInventoryManager.rightWeapon._magicDamage;
                    rightHandDamageCollider._bleedDamage = character.characterInventoryManager.rightWeapon._bleedDamage;

                    rightHandDamageCollider.characterManager = character;
                    rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                    character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (rightHandSlot.currentWeapon.weaponType == WeaponType._Spear)
                {
                    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                    rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                    rightHandDamageCollider._lightningDamage = character.characterInventoryManager.rightWeapon._lightningDamage;
                    rightHandDamageCollider._darkDamage = character.characterInventoryManager.rightWeapon._darkDamage;
                    rightHandDamageCollider._magicDamage = character.characterInventoryManager.rightWeapon._magicDamage;
                    rightHandDamageCollider._bleedDamage = character.characterInventoryManager.rightWeapon._bleedDamage;

                    rightHandDamageCollider.characterManager = character;
                    rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                    character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (rightHandSlot.currentWeapon.weaponType == WeaponType.Shield)
                {
                    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                    rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                    rightHandDamageCollider._lightningDamage = character.characterInventoryManager.rightWeapon._lightningDamage;
                    rightHandDamageCollider._darkDamage = character.characterInventoryManager.rightWeapon._darkDamage;
                    rightHandDamageCollider._magicDamage = character.characterInventoryManager.rightWeapon._magicDamage;
                    rightHandDamageCollider._bleedDamage = character.characterInventoryManager.rightWeapon._bleedDamage;

                    rightHandDamageCollider.characterManager = character;
                    rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                    character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
                else if (rightHandSlot.currentWeapon.weaponType == WeaponType.SmallShield)
                {
                    rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

                    rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
                    rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;
                    rightHandDamageCollider._lightningDamage = character.characterInventoryManager.rightWeapon._lightningDamage;
                    rightHandDamageCollider._darkDamage = character.characterInventoryManager.rightWeapon._darkDamage;
                    rightHandDamageCollider._magicDamage = character.characterInventoryManager.rightWeapon._magicDamage;
                    rightHandDamageCollider._bleedDamage = character.characterInventoryManager.rightWeapon._bleedDamage;

                    rightHandDamageCollider.characterManager = character;
                    rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

                    rightHandDamageCollider.poiseDamage = character.characterInventoryManager.rightWeapon.poiseBreak;
                    character.characterEffectsManager.rightWeaponManager = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponManager>();
                }
            }
        }

        public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }

        public virtual void OpenDamageCollider()
        {
            character.characterSoundFXManager.PlayRandomWeaponWhoosh();

            if (character.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (character.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }

            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.totalPoiseDefence + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefence = character.characterStatsManager.armorPoiseBonus;
        }
    }
}