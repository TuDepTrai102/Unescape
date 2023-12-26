using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        CharacterManager character;
        AudioSource audioSource;

        //ATTACKING GRUNTS

        //TAKING DAMAGE GRUNTS

        //TAKING DAMAGE SOUNDS
        [Header("TAKING DAMAGE SOUNDS")]
        public AudioClip[] takingDamageSounds;
        private List<AudioClip> potentialDamageSounds;
        private AudioClip lastDamageSoundPlayer;

        [Header("WEAPON WHOOSHES SOUNDS")]
        private List<AudioClip> potentialWeaponWhooshes;
        private AudioClip lastWeaponWhoosh;

        [Header("FOOT STEP SOUNDS")]
        //FOOD STEP SOUNDS (SPRINT)
        public AudioClip[] footStepSounds_Sprint;
        public float timeBetweenSteps_Sprint = 0.5f;
        private float lastStepTime_Sprint;

        //FOOD STEP SOUNDS (RUN)
        public AudioClip[] footStepSounds_Run;
        public float timeBetweenSteps_Run = 0.5f;
        private float lastStepTime_Run;

        //FOOD STEP SOUNDS (WALK)
        public AudioClip[] footStepSounds_Walk;
        public float timeBetweenSteps_Walk = 0.5f;
        private float lastStepTime_Walk;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            lastStepTime_Sprint = Time.time;
            lastStepTime_Run = Time.time;
            lastStepTime_Walk = Time.time;
        }

        public virtual void PlayRandomDamageSoundFX()
        {
            potentialDamageSounds = new List<AudioClip>();

            foreach (var damageSound in takingDamageSounds)
            {
                //IF THE POTENTIAL DAMAGE SOUND NOT BEEN PLAYED BEFORE,
                //WE ADD IT AS A POTENTIAL (STOP REPEAT DAMAGE SOUNDS)
                if (damageSound != lastDamageSoundPlayer)
                {
                    potentialDamageSounds.Add(damageSound);
                }
            }

            int randomValue = Random.Range(0, potentialDamageSounds.Count);

            lastDamageSoundPlayer = takingDamageSounds[randomValue];
            audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
        }

        public virtual void PlayRandomWeaponWhoosh()
        {
            potentialWeaponWhooshes = new List<AudioClip>();

            if (character.isUsingRightHand)
            {
                foreach (var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);

                lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooshes[randomValue], 0.4f);
            }
            else
            {
                foreach (var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooshes)
                {
                    if (whooshSound != lastWeaponWhoosh)
                    {
                        potentialWeaponWhooshes.Add(whooshSound);
                    }
                }

                int randomValue = Random.Range(0, potentialWeaponWhooshes.Count);

                if (lastWeaponWhoosh != null) //LEFT WEAPON ISSUE (ERROR IF THIS NULL)
                {
                    lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue];
                    audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooshes[randomValue], 0.4f);
                }
            }
        }

        public virtual void PlayRandomFootStep_Sprint()
        {
            if (footStepSounds_Sprint.Length > 0 && Time.time - lastStepTime_Sprint > timeBetweenSteps_Sprint)
            {
                int randomIndex = Random.Range(0, footStepSounds_Sprint.Length);
                audioSource.PlayOneShot(footStepSounds_Sprint[randomIndex]);
                lastStepTime_Sprint = Time.time;
            }
        }

        public virtual void PlayRandomFootStep_Run()
        {
            if (footStepSounds_Run.Length > 0 && Time.time - lastStepTime_Run > timeBetweenSteps_Run)
            {
                int randomIndex = Random.Range(0, footStepSounds_Run.Length);
                audioSource.PlayOneShot(footStepSounds_Run[randomIndex]);
                lastStepTime_Run = Time.time;
            }
        }

        public virtual void PlayRandomFootStep_Walk()
        {
            if (footStepSounds_Walk.Length > 0 && Time.time - lastStepTime_Walk > timeBetweenSteps_Walk)
            {
                int randomIndex = Random.Range(0, footStepSounds_Walk.Length);
                audioSource.PlayOneShot(footStepSounds_Walk[randomIndex]);
                lastStepTime_Walk = Time.time;
            }
        }

        public virtual void PlaySoundFX(AudioClip soundFX)
        {
            audioSource.PlayOneShot(soundFX);
        }

        public virtual void PlayRandomSoundFXFromArray(AudioClip[] soundArray)
        {
            int index = Random.Range(0, soundArray.Length);

            PlaySoundFX(soundArray[index]);
        }
    }
}