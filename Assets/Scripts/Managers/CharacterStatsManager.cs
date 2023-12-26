using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        //PLAYER HEALTH BAR
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        //A.I HEALTH BAR
        public UIAICharacterHealthBar aiCharacterHealthBar;

        [Header("CHARACTER NAME")]
        public Image _characterAvatar;
        public string characterName;

        [Header("TEAM I.D")]
        public int teamIDNumber = 0;

        public float maxHealth;
        public float currentHealth;

        public float maxStamina;
        public float currentStamina;

        public float maxFocusPoints;
        public float currentFocusPoints;

        public int currentSoulCount = 0;
        public int soulsAwardedOnDeath = 50;
        public int currentPointCount = 0;
        public int pointsAwardedOnDeath = 3;

        [Header("CHARACTER LEVEL")]
        public int playerLevel = 1;

        [Header("STAT LEVELS")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strengthLevel = 10;
        public int dexterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;

        [Header("EQUIPMENT LOAD")]
        public float currentEquipLoad = 0;
        public float maxEquipLoad = 0;
        public EncumbranceLevel encumbranceLevel;

        [Header("POISE")]
        public float totalPoiseDefence; //the TOTAL poise during damage calculation
        public float offensivePoiseBonus; //the poise you GAIN during an attack with a weapon
        public float armorPoiseBonus; //the poise you GAIN form wearing whatever you have equipped
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;

        [Header("ARMOR ABSORPTIONS")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionBody;
        public float fireDamageAbsorptionLegs;
        public float fireDamageAbsorptionHands;

        public float _lightningDamageAbsorptionHead;
        public float _lightningDamageAbsorptionBody;
        public float _lightningDamageAbsorptionLegs;
        public float _lightningDamageAbsorptionHands;

        public float _darkDamageAbsorptionHead;
        public float _darkDamageAbsorptionBody;
        public float _darkDamageAbsorptionLegs;
        public float _darkDamageAbsorptionHands;

        public float _bleedDamageAbsorptionHead;
        public float _bleedDamageAbsorptionBody;
        public float _bleedDamageAbsorptionLegs;
        public float _bleedDamageAbsorptionHands;

        public float _magicDamageAbsorptionHead;
        public float _magicDamageAbsorptionBody;
        public float _magicDamageAbsorptionLegs;
        public float _magicDamageAbsorptionHands;

        [Header("RESISTANCES")]
        public float poisonResistance;
        public float _bleedResistance;
        public float _curseResistance;

        [Header("BLOCKING ABSORPTIONS")]
        public float blockingPhysicalDamageAbsorption;
        public float blockingFireDamageAbsorption;
        public float _blockingLightningDamageAbsorption;
        public float _blockingDarkDamageAbsorption;
        public float _blockingBleedDamageAbsorption;
        public float _blockingMagicDamageAbsorption;
        public float blockingStabilityRating;

        //ANY DAMAGE DEALT BY THIS PLAYER IS MODIFIED BY THESE AMOUNTS
        [Header("DAMAGE TYPE MODIFIERS")]
        public float physicalDamagePercentageModifier = 100;
        public float fireDamagePercentageModifier = 100;
        public float _lightningDamagePercentageModifier = 100;
        public float _darkDamagePercentageModifier = 100;
        public float _bleedDamagePercentageModifier = 100;
        public float _magicDamagePercentageModifier = 100;

        //INCOMING DAMAGE AFTER ARMOR CALCULATION IS MODIFIED BY THESE VALUES
        [Header("DAMAGE ABSORPTION MODIFIERS")]
        public float physicalAbsorptionPercentageModifier = 0;
        public float fireAbsorptionPercentageModifier = 0;
        public float _lightningAbsorptionPercentageModifier = 0;
        public float _darkAbsorptionPercentageModifier = 0;
        public float _bleedAbsorptionPercentageModifier = 0;
        public float _magicAbsorptionPercentageModifier = 0;

        [Header("POISON")]
        public bool isPoisoned;
        public float poisonBuildup = 0; //THE BUILD UP OVER TIME THAT POISONS THE PLAYER AFTER REACHING 100
        public float poisonAmount = 100; //THE AMOUNT OF POISON THE PLAYER HAS TO PROCESS BEFORE BECOMING UNPOISONED

        [Header("BLEED")]
        public bool _isBleeded;
        public float _bleedBuildup = 0;
        public float _bleedAmount = 100;

        [Header("CURSE")]
        public bool _isCursed;
        public float _curseBuildup = 0;
        public float _curseAmount = 100;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        protected virtual void Start()
        {
            totalPoiseDefence = armorPoiseBonus;
            CalculateAndSetMaxEquipLoad();
        }

        public virtual void TakeDamageNoAnimation(
            int physicalDamage, 
            int fireDamage,
            int _lightningDamage,
            int _darkDamage,
            int _bleedDamage,
            int _magicDamage)
        {
            if (character.isDead)
                return;

            float totalPhysicalDamageAbsorption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionLegs / 100) *
                (1 - physicalDamageAbsorptionHands / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));

            float totalFireDamageAbsorption = 1 -
                (1 - fireDamageAbsorptionHead / 100) *
                (1 - fireDamageAbsorptionBody / 100) *
                (1 - fireDamageAbsorptionLegs / 100) *
                (1 - fireDamageAbsorptionHands / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));

            float _totalLightningDamageAbsorption = 1 -
                (1 - _lightningDamageAbsorptionHead / 100) *
                (1 - _lightningDamageAbsorptionBody / 100) *
                (1 - _lightningDamageAbsorptionLegs / 100) *
                (1 - _lightningDamageAbsorptionHands / 100);

            _lightningDamage = Mathf.RoundToInt(_lightningDamage - (_lightningDamage * _totalLightningDamageAbsorption));

            float _totalDarkDamageAbsorption = 1 -
                (1 - _darkDamageAbsorptionHead / 100) *
                (1 - _darkDamageAbsorptionBody / 100) *
                (1 - _darkDamageAbsorptionLegs / 100) *
                (1 - _darkDamageAbsorptionHands / 100);

            _darkDamage = Mathf.RoundToInt(_darkDamage - (_darkDamage * _totalDarkDamageAbsorption));

            float _totalMagicDamageAbsorption = 1 -
                (1 - _magicDamageAbsorptionHead / 100) *
                (1 - _magicDamageAbsorptionBody / 100) *
                (1 - _magicDamageAbsorptionLegs / 100) *
                (1 - _magicDamageAbsorptionHands / 100);

            _magicDamage = Mathf.RoundToInt(_magicDamage - (_magicDamage * _totalMagicDamageAbsorption));

            float _totalBleedDamageAbsorption = 1 -
                (1 - _bleedDamageAbsorptionHead / 100) *
                (1 - _bleedDamageAbsorptionBody / 100) *
                (1 - _bleedDamageAbsorptionLegs / 100) *
                (1 - _bleedDamageAbsorptionHands / 100);

            _bleedDamage = Mathf.RoundToInt(_bleedDamage - (_bleedDamage * _totalBleedDamageAbsorption));

            float finalDamage = physicalDamage + fireDamage + _magicDamage + _lightningDamage + _darkDamage + _bleedDamage;

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                character.isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer() 
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public virtual void DeductStamina(float staminaToDeduct)
        {
            currentStamina = currentStamina - staminaToDeduct;
        }

        public float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 100;
            return maxStamina;
        }

        public float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 5;
            return maxFocusPoints;
        }

        public void CalculateAndSetMaxEquipLoad()
        {
            float totalEquipLoad = 40;

            for (int i = 0; i < staminaLevel; i++)
            {
                //CHANCE RETUNS BASE IN STAMINA LEVEL
                if (i < 25)
                {
                    totalEquipLoad += 1.2f;
                }

                if (i >= 25 && i <= 50)
                {
                    totalEquipLoad += 1.4f;
                }

                if (i > 50)
                {
                    totalEquipLoad += 1;
                }
            }

            maxEquipLoad = totalEquipLoad;
        }

        public void CalculateAndSetCurrentEquipLoad(float equipLoad)
        {
            currentEquipLoad = equipLoad;

            encumbranceLevel = EncumbranceLevel.Light;

            if (currentEquipLoad > (maxEquipLoad * 0.3f))
            {
                encumbranceLevel = EncumbranceLevel.Medium;
            }

            if (currentEquipLoad > (maxEquipLoad * 0.7f))
            {
                encumbranceLevel = EncumbranceLevel.Heavy;
            }

            if (currentEquipLoad > (maxEquipLoad))
            {
                encumbranceLevel = EncumbranceLevel.Overloaded;
            }
        }

        public virtual void HealCharacter(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}