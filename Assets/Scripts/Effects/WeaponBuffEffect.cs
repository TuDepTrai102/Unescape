using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Weapon Buff Effect")]
    public class WeaponBuffEffect : CharacterEffect
    {
        [Header("BUFF INFO")]
        [SerializeField] BuffClass buffClass;
        [SerializeField] float lengthOfBuff;
        public float timeRemainingOnBuff;
        [HideInInspector] public bool isRightHandedBuff;

        [Header("BUFF SOUND FX")]
        [SerializeField] AudioClip buffAmbientSound;
        [SerializeField] float ambientSoundVolume = 0.3f;

        [Header("DAMAGE INFO")]
        [SerializeField] public float buffBaseDamagePercentageMultiplier = 15; //HOW MUCH % OF THE BASE DAMAGE IS MULTIPLED

        [Header("POISE BUFF")]
        [SerializeField] bool buffPoiseDamage;
        [SerializeField] public float buffBasePoiseDamagePercentageMultiplier = 15;

        [Header("GENERAL")]
        [SerializeField] bool buffHasStarted = false;
        private WeaponManager weaponManager;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!buffHasStarted)
            {
                timeRemainingOnBuff = lengthOfBuff;
                buffHasStarted = true;

                weaponManager = character.characterWeaponSlotManager.rightHandDamageCollider.GetComponentInParent<WeaponManager>();
                weaponManager.audioSource.loop = true;
                weaponManager.audioSource.clip = buffAmbientSound;
                weaponManager.audioSource.volume = ambientSoundVolume;

                float baseWeaponDamage =
                    weaponManager.damageCollider.physicalDamage +
                    weaponManager.damageCollider.fireDamage +
                    weaponManager.damageCollider._lightningDamage +
                    weaponManager.damageCollider._darkDamage +
                    weaponManager.damageCollider._magicDamage +
                    weaponManager.damageCollider._bleedDamage;

                float physicalBuffDamage = 0;
                float fireBuffDamage = 0;
                float poiseBuffDamage = 0;
                //float _lightningDamage = 0;
                //float _darkDamage = 0;
                //float _magicDamage = 0;
                //float _bleedDamage = 0;

                if (buffPoiseDamage)
                {
                    poiseBuffDamage = weaponManager.damageCollider.poiseDamage * (buffBasePoiseDamagePercentageMultiplier / 100);
                }

                switch (buffClass)
                {
                    case BuffClass.Physical: 
                        physicalBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                        break;

                    case BuffClass.Fire: 
                        fireBuffDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                        break;

                    //case BuffClass._Lightning: 
                    //    _lightningDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                    //    break;

                    //case BuffClass._Dark:
                    //    _darkDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                    //    break;

                    //case BuffClass._Magic:
                    //    _magicDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                    //    break;

                    //case BuffClass._Bleed:
                    //    _bleedDamage = baseWeaponDamage * (buffBaseDamagePercentageMultiplier / 100);
                    //    break;

                    default:
                        break;
                }

                weaponManager.BuffWeapon(buffClass,
                    physicalBuffDamage,
                    fireBuffDamage,
                    poiseBuffDamage);
                    //_lightningDamage,
                    //_darkDamage,
                    //_magicDamage,
                    //_bleedDamage);
            }

            if (buffHasStarted)
            {
                timeRemainingOnBuff = timeRemainingOnBuff - 1;

                if (timeRemainingOnBuff <= 0)
                {
                    weaponManager.DebuffWeapon();

                    if (isRightHandedBuff)
                    {
                        character.characterEffectsManager.rightWeaponBuffEffect = null;
                    }
                }
            }
        }
    }
}