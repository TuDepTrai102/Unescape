using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    [CreateAssetMenu(menuName = "Character Effects/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : CharacterEffect
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager characterCausingDamage; //IF THE DAMAGE IS CAUSED BY CHARACTER, THEY ARE LISTED HERE

        [Header("BASE DAMAGE")]
        public float physicalDamage = 0;
        public float fireDamage = 0;
        public float _lightningDamage = 0;
        public float _darkDamage = 0;
        public float _magicDamage = 0;
        public float _bleedDamage = 0;
        public float staminaDamage = 0;
        public float poiseDamage = 0;

        [Header("ANIMATION")]
        public string blockAnimation;

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
            CalculateStaminaDamage(character);

            //PLAY A DAMAGE ANIMATION
            DecideBlockAnimationBasedOnPoiseDamage(character);

            //PLAY DAMAGE SFX
            PlayBlockSoundFX(character);

            //IF THE CHARACTER IS A.I, ASSIGH THEM THE DAMAGING CHARACTER AS A TARGET
            AssignNewAITarget(character);

            if (character.isDead)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterAnimatorManager.animation_dead_01, true);
            }
            else
            {
                if (character.characterStatsManager.currentStamina <= 0)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterAnimatorManager.animation_guard_break, true);
                    character.canBeRiposted = true;
                    //character.characterSoundFXManager. (PLAY GUARD BREAK SOUND HERE)
                    character.isBlocking = false;
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
                    character.isAttacking = false;
                }
            }
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (character.isDead)
                return;

            if (characterCausingDamage != null)
            {
                //BEFORE WE CALCULATING DAMAGE DEFENSE, WE CHECK THE ATTACKING CHARACTERS DAMAGE MODIFIERS
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

            physicalDamage -= Mathf.RoundToInt(physicalDamage * (character.characterStatsManager.blockingPhysicalDamageAbsorption / 100));
            fireDamage -= Mathf.RoundToInt(fireDamage * (character.characterStatsManager.blockingFireDamageAbsorption / 100));
            _lightningDamage -= Mathf.RoundToInt(_lightningDamage * (character.characterStatsManager._blockingLightningDamageAbsorption / 100));
            _darkDamage -= Mathf.RoundToInt(_darkDamage * (character.characterStatsManager._blockingDarkDamageAbsorption / 100));
            _magicDamage -= Mathf.RoundToInt(_magicDamage * (character.characterStatsManager._blockingMagicDamageAbsorption / 100));
            _bleedDamage -= Mathf.RoundToInt(_bleedDamage * (character.characterStatsManager._blockingBleedDamageAbsorption / 100));

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

            if (character.characterStatsManager.currentHealth <= 0)
            {
                character.characterStatsManager.currentHealth = 0;
                character.isDead = true;
            }
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            float staminaDamageAbsorption = staminaDamage * (character.characterStatsManager.blockingStabilityRating / 100);
            float staminaDamageAfterAbsorption = staminaDamage - staminaDamageAbsorption;
            character.characterStatsManager.currentStamina -= staminaDamageAfterAbsorption;
        }

        private void DecideBlockAnimationBasedOnPoiseDamage(CharacterManager character)
        {
            //ONE HANDED BLOCK ANIMATION
            if (!character.isTwoHandingWeapon)
            {
                //POISE BRACKET < 25      SMALL
                //POISE BRACKET > 25 < 50 MEDIUM
                //POISE BRACKET > 50 < 75 LARGE
                //POISE BRACKET > 75      COLOSSAL

                if (poiseDamage <= 24 && poiseDamage >= 0)
                {
                    blockAnimation = character.characterAnimatorManager.animation_guard_block_light;
                    return;
                }
                else if (poiseDamage <= 49 && poiseDamage >= 25)
                {
                    blockAnimation = character.characterAnimatorManager.animation_guard_block_medium;
                    return;
                }
                else if (poiseDamage <= 74 && poiseDamage >= 50)
                {
                    blockAnimation = character.characterAnimatorManager.animation_guard_block_heavy;
                    return;
                }
                else if (poiseDamage >= 75)
                {
                    blockAnimation = character.characterAnimatorManager.animation_guard_block_colossal;
                    return;
                }
            }
            //TWO HANDED BLOCK ANIMATION
            else
            {
                //TWO HAND ANIMATION BLOCK HIT HERE (IF HAVE)


            }
        }

        private void PlayBlockSoundFX(CharacterManager character)
        {
            //WE ARE BLOCKING WITH OUR RIGHT HANDED WEAPON
            if (character.isTwoHandingWeapon)
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.rightWeapon.blockingNoises);
            }
            //WE ARE BLOCKING WITH OUR OFF (LEFT) HANDED WEAPON
            else
            {
                character.characterSoundFXManager.PlayRandomSoundFXFromArray(character.characterInventoryManager.leftWeapon.blockingNoises);
            }
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