using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace NT
{
    public class WeaponManager : MonoBehaviour
    {
        [Header("BUFF FX")]
        [SerializeField] GameObject physicalBuffFX;
        [SerializeField] GameObject fireBuffFX;
        [SerializeField] GameObject _lightningBuffFX;
        [SerializeField] GameObject _darkBuffFX;
        [SerializeField] GameObject _magicBuffFX;
        [SerializeField] GameObject _bleedBuffFX;

        [Header("TRAIL FX")]
        [SerializeField] ParticleSystem defaultTrailFX;
        [SerializeField] ParticleSystem fireTrailFX;
        [SerializeField] ParticleSystem _lightningTrailFX;
        [SerializeField] ParticleSystem _darkTrailFX;
        [SerializeField] ParticleSystem _magicTrailFX;
        [SerializeField] ParticleSystem _bleedTrailFX;

        private bool weaponIsBuffed;
        private BuffClass weaponBuffClass;

        [HideInInspector] public MeleeWeaponDamageCollider damageCollider;
        public AudioSource audioSource;

        private void Awake()
        {
            damageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void BuffWeapon(
            BuffClass buffClass, 
            float physicalBuffDamage, 
            float fireBuffDamage, 
            float poiseBuffDamage)
            //float _lightningBuffDamage,
            //float _darkBuffDamage,
            //float _magicBuffDamage,
            //float _bleedBuffDamage)
        {
            //RESET ANY ACTIVE BUFF
            DebuffWeapon();
            weaponIsBuffed = true;
            weaponBuffClass = buffClass;
            audioSource.Play();

            switch (buffClass)
            {
                case BuffClass.Physical: physicalBuffFX.SetActive(true);
                    break;

                case BuffClass.Fire: fireBuffFX.SetActive(true);
                    break;

                //case BuffClass._Lightning:
                //    _lightningBuffFX.SetActive(true);
                //    break;

                //case BuffClass._Dark:
                //    _darkBuffFX.SetActive(true);
                //    break;

                //case BuffClass._Magic:
                //    _magicBuffFX.SetActive(true);
                //    break;

                //case BuffClass._Bleed:
                //    _bleedBuffFX.SetActive(true);
                //    break;

                default: 
                    break;
            }

            damageCollider.physicalBuffDamage = physicalBuffDamage;
            damageCollider.fireBuffDamage = fireBuffDamage;
            damageCollider.poiseBuffDamage = poiseBuffDamage;
            //damageCollider._lightningBuffDamage = _lightningBuffDamage;
            //damageCollider._darkBuffDamage = _darkBuffDamage;
            //damageCollider._magicBuffDamage = _magicBuffDamage;
            //damageCollider._bleedBuffDamage = _bleedBuffDamage;
        }

        public void DebuffWeapon()
        {
            weaponIsBuffed = false;
            audioSource.Stop();
            physicalBuffFX.SetActive(false);
            fireBuffFX.SetActive(false);
            //_lightningBuffFX.SetActive(false);
            //_darkBuffFX.SetActive(false);
            //_magicBuffFX.SetActive(false);
            //_bleedBuffFX.SetActive(false);

            damageCollider.physicalBuffDamage = 0;
            damageCollider.fireBuffDamage = 0;
            damageCollider.poiseBuffDamage = 0;
            //damageCollider._lightningBuffDamage = 0;
            //damageCollider._darkBuffDamage = 0;
            //damageCollider._magicBuffDamage = 0;
            //damageCollider._bleedBuffDamage = 0;
        }

        public void PlayWeaponTrailFX()
        {
            if (weaponIsBuffed)
            {
                switch (weaponBuffClass)
                {
                    //IF OUR WEAPON IS PHYSICALLY BUFFED, PLAY THE DEFAULT TRAIL
                    case BuffClass.Physical:

                        if (defaultTrailFX == null)
                            return;
                        defaultTrailFX.Play();

                        break;

                    //IF OUR WEAPON IS FIRE BUFFED, PLAY THE FIRE TRAIL
                    case BuffClass.Fire: 

                        if (defaultTrailFX == null) 
                            return;
                        fireTrailFX.Play();

                        break;

                    ////IF OUR WEAPON IS FIRE BUFFED, PLAY THE LIGHTNING TRAIL
                    //case BuffClass._Lightning:

                    //    if (defaultTrailFX == null)
                    //        return;
                    //    _lightningTrailFX.Play();

                    //    break;

                    ////IF OUR WEAPON IS FIRE BUFFED, PLAY THE DARK TRAIL
                    //case BuffClass._Dark:

                    //    if (defaultTrailFX == null)
                    //        return;
                    //    _darkTrailFX.Play();

                    //    break;

                    ////IF OUR WEAPON IS FIRE BUFFED, PLAY THE MAGIC TRAIL
                    //case BuffClass._Magic:

                    //    if (defaultTrailFX == null)
                    //        return;
                    //    _magicTrailFX.Play();

                    //    break;

                    ////IF OUR WEAPON IS FIRE BUFFED, PLAY THE BLEED TRAIL
                    //case BuffClass._Bleed:

                    //    if (defaultTrailFX == null)
                    //        return;
                    //    _bleedTrailFX.Play();

                    //    break;

                    default:
                        break;
                }
            }
            else
            {
                defaultTrailFX.Play();
            }
        }
    }
}