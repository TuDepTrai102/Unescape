using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

namespace NT
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        //STATIC EFFECTS (RINGS EFFECTS, ARMOR EFFECTS ETC)
        [Header("STATIC EFFECTS")]
        [SerializeField] public List<StaticCharacterEffect> staticCharacterEffects;

        //TIMED / STATUS EFFECTS (POISON BUILD UP, CURSE, TOXIC ETC)
        [Header("TIMED EFFECTS")]
        public List<CharacterEffect> timedEffects;
        [SerializeField] float effectTickTimer = 0;

        //INSTANCE EFFECT (TAKING DAMAGE, ADD BUILD UP, ETC)
        [Header("TIMED EFFECT VISUAL FX")]
        public List<GameObject> timedEffectParticles;

        [Header("CURRENT FX")]
        public GameObject instantFXModel; //INSTANTIATEDFXMODEL

        [Header("DAMAGE FX")]
        public GameObject bloodSplatterFX;

        [Header("WEAPON FX")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        [Header("RIGHT WEAPON BUFF")]
        public WeaponBuffEffect rightWeaponBuffEffect;

        [Header("POISON FX")]
        public Transform buildUpTransform; //THE LOCATION BUILD UP PARTICLE FX WILL SPAWN

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        public virtual void ProcessEffectInstantly(CharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

        public virtual void ProcessAllTimedEffects()
        {
            effectTickTimer += Time.deltaTime;

            if (effectTickTimer >= 1)
            {
                effectTickTimer = 0;

                ProcessWeaponBuffs();

                //  PROCESSES ALL ACTIVE EFFECTS OVER GAME TIME
                for (int i = timedEffects.Count - 1; i > -1; i--)
                {
                    timedEffects[i].ProcessEffect(character);
                }

                //  DECAYS BUILD UP EFFECTS OVER GAME TIME
                ProcessBuildUpDecay();
            }
        }

        public void ProcessWeaponBuffs()
        {
            if (rightWeaponBuffEffect != null)
            {
                rightWeaponBuffEffect.ProcessEffect(character);
            }
        }

        public void AddStaticEffect(StaticCharacterEffect effect)
        {
            //CHECK THE LIST TO MAKE SURE WE DONT DUPLICATE ADD EFFECT

            StaticCharacterEffect staticEffect;

            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effect.effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        //WE REMOVE THE ACTUAL EFFECT FROM OUR CHARACTER
                        staticEffect.RemoveStaticEffect(character);
                        //WE THE REMOVE THE EFFFECT FROM THE LIST OF ACTIVE EFFECTS
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }

            //WE ADD THE EFFECT TO OUR LIST OF ACTIVE EFFECTS
            staticCharacterEffects.Add(effect);
            //WE ADD THE ACTUAL EFFECT TO OUR CHARACTER
            effect.AddStaticEffect(character);

            //CHECK THE LIST FOR NULL ITEMS AND REMOVE THEM
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }
        }

        public void RemoveStaticEffect(int effectID)
        {
            StaticCharacterEffect staticEffect;

            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] != null)
                {
                    if (staticCharacterEffects[i].effectID == effectID)
                    {
                        staticEffect = staticCharacterEffects[i];
                        //WE REMOVE THE ACTUAL EFFECT FROM OUR CHARACTER
                        staticEffect.RemoveStaticEffect(character);
                        //WE THE REMOVE THE EFFFECT FROM THE LIST OF ACTIVE EFFECTS
                        staticCharacterEffects.Remove(staticEffect);
                    }
                }
            }

            //CHECK THE LIST FOR NULL ITEMS AND REMOVE THEM
            for (int i = staticCharacterEffects.Count - 1; i > -1; i--)
            {
                if (staticCharacterEffects[i] == null)
                {
                    staticCharacterEffects.RemoveAt(i);
                }
            }
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if (!isLeft)
            {
                if (rightWeaponManager != null && character.isUsingRightHand)
                {
                    rightWeaponManager.PlayWeaponTrailFX();
                }
            }

            if (isLeft)
            {
                if (leftWeaponManager != null && character.isUsingLeftHand)
                {
                    leftWeaponManager.PlayWeaponTrailFX();
                }
            }
        }

        public virtual void PlayerBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void InterruptEffect()
        {
            //CAN BE USED TO DESTROY EFFECTS MODELS (DRINKING ESTUS, HAVING ARROW DRAWN, ETC.ETC...)
            if (instantFXModel != null)
            {
                Destroy(instantFXModel);
            }

            //FIRES THE CHARACTERS BOW AND REMOVES THE ARROW IF THEY ARE CURRENTLY HOLDING AN ARROW
            if (character.isHoldingArrow)
            {
                character.animator.SetBool("isHoldingArrow", false);
                Animator rangedWeaponAnimator = 
                    character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();

                if (rangedWeaponAnimator != null)
                {
                    rangedWeaponAnimator.SetBool("isDrawn", false);
                    rangedWeaponAnimator.Play("Bow_ONLY_Fire_01");
                }
            }

            //REMOVES PLAYER FROM AIMING STATE IF THEY AIR CURRENTLY AIMING
            if (character.isAiming)
            {
                character.animator.SetBool("isAiming", false);
            }
        }

        protected virtual void ProcessBuildUpDecay()
        {
            if (character.characterStatsManager.poisonBuildup > 0)
            {
                character.characterStatsManager.poisonBuildup -= 1;
            }

            if (character.characterStatsManager._bleedBuildup > 0)
            {
                character.characterStatsManager._bleedBuildup -= 1;
            }

            if (character.characterStatsManager._curseBuildup > 0)
            {
                character.characterStatsManager._curseBuildup -= 1;
            }
        }

        public virtual void AddTimedEffectParticle(GameObject effect)
        {
            GameObject effectGameObject = Instantiate(effect, buildUpTransform);
            timedEffectParticles.Add(effectGameObject);
        }

        public virtual void RemoveTimedEffectParticle(EffectParticleType effectType)
        {
            for (int i = timedEffectParticles.Count - 1; i > -1; i--)
            {
                if (timedEffectParticles[i].GetComponent<EffectParticle>().effectType == effectType)
                {
                    Destroy(timedEffectParticles[i]);
                    timedEffectParticles.RemoveAt(i);
                }
            }
        }
    }
}