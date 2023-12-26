using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Take Damage")]
    public class TakeDamageEffect : CharacterEffect
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager characterCausingDamage; //IF THE DAMAGE IS CAUSED BY CHARACTER, THEY ARE LISTED HERE

        [Header("DAMAGE")]
        public float physicalDamage = 0;
        public float fireDamage = 0;
        public float _lightningDamage = 0;
        public float _darkDamage = 0;
        public float _bleedDamage = 0;
        public float _magicDamage = 0;

        [Header("POISE")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("ANIMATIONS")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SFX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; //EXTRA SFX THAT IS PLAYED WHEN THERE IS ELEMENTAL DAMAGE (FIRE, MAGIC, DARKNESS, LIGHTNING)

        [Header("DIRECTION DAMAGE TAKEN FROM")]
        public float angleHitFrom;
        public Vector3 contactPoint; //WHERE THE DAMAGE STRIKES THE PLAYER ON THEIR BODY

        public override void ProcessEffect(CharacterManager character)
        {
            //IF THE CHARACTER IS DEAD, RETURN WITHOUT RUNNING ANY LOGIC
            if (character.isDead)
                return;

            //IF THE CHARACTER IS INVULNERABLE, NO DAMAGE IS TAKEN
            if (character.isInvulnerable)
                return;

            //CALCULATE TOTAL DAMAGE AFTER DEFENSE
            CalculateDamage(character);

            //CHECK WHICH DIRECTION THE DAMAGE CAME FROM SO WE CAN PLAY THE RIGHT ANIMATION
            CheckWhichDirectionDamageCameFrom(character);

            //PLAY A DAMAGE ANIMATION
            PlayDamageAnimation(character);

            //PLAY DAMAGE SFX
            PlayDamageSoundFX(character);

            //PLAY BLOOD SPLATTER FX
            PlayBloodSplatter(character);

            //IF THE CHARACTER IS A.I, ASSIGH THEM THE DAMAGING CHARACTER AS A TARGET
            AssignNewAITarget(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            //BEFORE WE CALCULATING DAMAGE DEFENSE, WE CHECK THE ATTACKING CHARACTERS DAMAGE MODIFIERS
            if (characterCausingDamage != null)
            {
                physicalDamage = Mathf.RoundToInt(physicalDamage *
                    (characterCausingDamage.characterStatsManager.physicalDamagePercentageModifier / 100));
                fireDamage = Mathf.RoundToInt(fireDamage *
                    (characterCausingDamage.characterStatsManager.fireDamagePercentageModifier / 100));
                _lightningDamage = Mathf.RoundToInt(_lightningDamage *
                    (characterCausingDamage.characterStatsManager._lightningDamagePercentageModifier / 100));
                _darkDamage = Mathf.RoundToInt(_darkDamage *
                    (characterCausingDamage.characterStatsManager._darkDamagePercentageModifier / 100));
                _magicDamage = Mathf.RoundToInt(_magicDamage *
                    (characterCausingDamage.characterStatsManager._magicDamagePercentageModifier / 100));
                _bleedDamage = Mathf.RoundToInt(_bleedDamage *
                    (characterCausingDamage.characterStatsManager._bleedDamagePercentageModifier / 100));
            }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            float totalPhysicalDamageAbsorption = 1 -
                (1 - character.characterStatsManager.physicalDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.physicalDamageAbsorptionHands / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - character.characterStatsManager.fireDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager.fireDamageAbsorptionHands / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            float _totalLightningDamageAbsorption = 1 -
                (1 - character.characterStatsManager._lightningDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager._lightningDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager._lightningDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager._lightningDamageAbsorptionHands / 100);

            _lightningDamage = Mathf.RoundToInt(_lightningDamage - (_lightningDamage * _totalLightningDamageAbsorption));

            float _totalDarkDamageAbsorption = 1 -
                (1 - character.characterStatsManager._darkDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager._darkDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager._darkDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager._darkDamageAbsorptionHands / 100);

            _darkDamage = Mathf.RoundToInt(_darkDamage - (_darkDamage * _totalDarkDamageAbsorption));

            float _totalMagicDamageAbsorption = 1 -
                (1 - character.characterStatsManager._magicDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager._magicDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager._magicDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager._magicDamageAbsorptionHands / 100);

            _magicDamage = Mathf.RoundToInt(_magicDamage - (_magicDamage * _totalMagicDamageAbsorption));

            float _totalBleedDamageAbsorption = 1 -
                (1 - character.characterStatsManager._bleedDamageAbsorptionHead / 100) *
                (1 - character.characterStatsManager._bleedDamageAbsorptionBody / 100) *
                (1 - character.characterStatsManager._bleedDamageAbsorptionLegs / 100) *
                (1 - character.characterStatsManager._bleedDamageAbsorptionHands / 100);

            _bleedDamage = Mathf.RoundToInt(_bleedDamage - (_bleedDamage * _totalBleedDamageAbsorption));

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.physicalAbsorptionPercentageModifier / 100));
            fireDamage -= Mathf.RoundToInt(fireDamage * (character.characterStatsManager.fireAbsorptionPercentageModifier / 100));
            _lightningDamage -= Mathf.RoundToInt(_lightningDamage * (character.characterStatsManager._lightningAbsorptionPercentageModifier / 100));
            _darkDamage -= Mathf.RoundToInt(_darkDamage * (character.characterStatsManager._darkAbsorptionPercentageModifier / 100));
            _magicDamage -= Mathf.RoundToInt(_magicDamage * (character.characterStatsManager._magicAbsorptionPercentageModifier / 100));
            _bleedDamage -= Mathf.RoundToInt(_bleedDamage * (character.characterStatsManager._bleedAbsorptionPercentageModifier / 100));

            float finalDamage = physicalDamage + fireDamage + _lightningDamage + _darkDamage + _magicDamage + _bleedDamage;

            Debug.Log("FINAL DAMAGE: " + finalDamage);
            character.characterStatsManager.currentHealth = Mathf.RoundToInt(character.characterStatsManager.currentHealth - finalDamage);

            PlayerManager player = character as PlayerManager;

            if (characterCausingDamage == player)
            {
                AICharacterManager enemy = character as AICharacterManager;

                if (!enemy.aiCharacterStatsManager.isBoss)
                {
                    character.characterStatsManager.aiCharacterHealthBar.SetHealth(character.characterStatsManager.currentHealth);
                }
                else
                {
                    enemy.aiCharacterBossManager.UpdateBossHealthBar(character.characterStatsManager.currentHealth, character.characterStatsManager.maxHealth);
                }
            }
            else
            {
                character.characterStatsManager.healthBar.SetCurrentHealth(character.characterStatsManager.currentHealth);
            }

            if (character.characterStatsManager.totalPoiseDefence < poiseDamage)
            {
                poiseIsBroken = true;
            }

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }
        }

        private void CheckWhichDirectionDamageCameFrom(CharacterManager character)
        {
            if (manuallySelectDamageAnimation)
                return;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                ChooseDamageAnimationForward(character);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                ChooseDamageAnimationBackward(character);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                ChooseDamageAnimationLeft(character);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                ChooseDamageAnimationRight(character);
            }
        }

        private void ChooseDamageAnimationForward(CharacterManager character)
        {
            //POISE BRACKET < 25      SMALL
            //POISE BRACKET > 25 < 50 MEDIUM
            //POISE BRACKET > 50 < 75 LARGE
            //POISE BRACKET > 75      COLOSSAL

            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Light_Forward);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Medium_Forward);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Heavy_Forward);
                return;
            }
            else if (poiseDamage >= 75)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Colossal_Forward);
                return;
            }
        }

        private void ChooseDamageAnimationBackward(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Light_Backward);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Medium_Backward);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Heavy_Backward);
                return;
            }
            else if (poiseDamage >= 75)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Colossal_Backward);
                return;
            }
        }

        private void ChooseDamageAnimationLeft(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Light_Left);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Medium_Left);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Heavy_Left);
                return;
            }
            else if (poiseDamage >= 75)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Colossal_Left);
                return;
            }
        }

        private void ChooseDamageAnimationRight(CharacterManager character)
        {
            if (poiseDamage <= 24 && poiseDamage >= 0)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Light_Right);
                return;
            }
            else if (poiseDamage <= 49 && poiseDamage >= 25)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Medium_Right);
                return;
            }
            else if (poiseDamage <= 74 && poiseDamage >= 50)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Heavy_Right);
                return;
            }
            else if (poiseDamage >= 75)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimtionFromList
                    (character.characterAnimatorManager.Damage_Animations_Colossal_Right);
                return;
            }
        }

        private void PlayDamageSoundFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayRandomDamageSoundFX();

            if (fireDamage > 0)
            {
                character.characterSoundFXManager.PlaySoundFX(elementalDamageSoundFX);
            }
        }

        private void PlayDamageAnimation(CharacterManager character)
        {
            //IF WE ARE CURRENTLY PLAYING A DAMAGE ANIMATION THAT IS HEAVY AND A LIGHT HITS US
            //WE DO NOT WANT TO PLAY THE LIGHT DAMAGE ANIMATION, WE WANT TO FINISH THE HEAVY ANIMATION
            if (character.isInteracting && character.characterCombatManager.previousPoiseDamageTaken > poiseDamage)
            {
                //IF THE CHARACTER IS INTERACTING && THE PREVIOUS POISE DAMAGE IS ABOVE ZERO, THEY MUST BE IN A DAMAGE ANIMATION
                //IF THE PREVIOUS POISE IS ABOVE THE CURRENT POISE, RETURN, SO WE DONT CHANGE THE DAMAGE ANIMATION TO A LIGHTER ANIMATION
                return;
            }


            if (character.isDead)
            {
                character.characterWeaponSlotManager.CloseDamageCollider();
                character.characterAnimatorManager.PlayTargetAnimation(character.characterAnimatorManager.animation_dead_01, true);
                return;
            }

            //IF THE CHARACTERS POISE IS NOT BROKEN, NO DAMAGE ANIMATION IS PLAYED
            if (!poiseIsBroken)
            {
                return;
            }
            else
            {
                //ENABLE/DISABLE STUN BLOCK

                if (playDamageAnimation)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(damageAnimation, true);
                }
            }
        }

        private void PlayBloodSplatter(CharacterManager character)
        {
            character.characterEffectsManager.PlayerBloodSplatterFX(contactPoint);
        }

        private void AssignNewAITarget(CharacterManager character)
        {
            AICharacterManager aICharacter = character as AICharacterManager;

            if (aICharacter != null && characterCausingDamage != null)
            {
                aICharacter.currentTarget = characterCausingDamage;
            }
        }
    }
}