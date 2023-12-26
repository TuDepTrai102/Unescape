using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        public PlayerManager player;
        public SoulCountBar soulCountBar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenerationAmountWhilstBlocking = 0.1f;
        public float staminaRengenTimer;

        public float _healthRegenerationAmount = 1;
        public float _healthRegenerationAmountWhilstBlocking = 0.1f;
        public float _healthRengenTimer;

        public float _focusPtRegenerationAmount = 1;
        public float _focusPtRegenerationAmountWhilstBlocking = 0.1f;
        public float _focusPtRengenTimer;

        private float sprintingTimer = 0;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            soulCountBar = FindObjectOfType<SoulCountBar>();
        }

        protected override void Start()
        {
            base.Start();

            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoint(maxFocusPoints);
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !player.isInteracting)
            {
                totalPoiseDefence = armorPoiseBonus;
            }
        }

        public override void TakeDamageNoAnimation(
            int physicalDamage, 
            int fireDamage,
            int _lightningDamage,
            int _darkDamage,
            int _bleedDamage,
            int _magicDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage, _lightningDamage, _darkDamage, _bleedDamage, _magicDamage);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead)
                return;

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                _HandlePlayerDeath();
            }
        }

        public void _HandlePlayerDeath()
        {
            currentHealth = 0;
            player.isDead = true;
            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_dead_01, true);
        }

        public override void DeductStamina(float staminaToDeduct)
        {
            base.DeductStamina(staminaToDeduct);
            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }

        public void DeductSprintingStamina(float staminaToDeduct)
        {
            if (player.isSprinting)
            {
                sprintingTimer += Time.deltaTime;

                if (sprintingTimer > 0.1f)
                {
                    //RESET TIMER
                    sprintingTimer = 0;
                    //DEDUCT STAMINA
                    currentStamina = currentStamina - staminaToDeduct;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
            else
            {
                sprintingTimer = 0;
            }
        }

        public void RegenerateStamina()
        {
            //DO NOT REGENERATE STAMINA IF WE ARE INTERACTING OR SPRINTING
            if (player.isInteracting || player.isSprinting)
            {
                staminaRengenTimer = 0;
            }
            else
            {
                staminaRengenTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRengenTimer > 1f)
                {
                    if (player.isBlocking)
                    {
                        currentStamina += staminaRegenerationAmountWhilstBlocking * Time.deltaTime;
                        staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 1 && 
                            player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.1f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 2 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.2f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 3 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.3f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 4 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.4f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 5 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.5f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 6 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.6f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 7 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.7f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 8 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.8f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 9 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 1.9f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else if (player._playerSkills._passive_IncreaseStaminaRate.currentCount == 10 &&
                                 player._playerSkills._passive_IncreaseStaminaRate._passive_StaminaRateUnlocked)
                        {
                            currentStamina += staminaRegenerationAmount * 2.33f * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                        else
                        {
                            currentStamina += staminaRegenerationAmount * Time.deltaTime;
                            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                        }
                    }
                }
            }           
        }

        public void _RegenerateHealth()
        {
            //DO NOT REGENERATE STAMINA IF WE ARE INTERACTING OR SPRINTING
            if (player.isInteracting || player.isSprinting)
            {
                _healthRengenTimer = 0;
            }
            else
            {
                _healthRengenTimer += Time.deltaTime;

                if (currentHealth < maxHealth && _healthRengenTimer > 1f)
                {
                    if (player.isBlocking)
                    {
                        currentHealth += _healthRegenerationAmountWhilstBlocking * Time.deltaTime;
                        healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                    }
                    else
                    {
                        if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 1 &&
                            player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.1f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 2 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.2f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 3 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.3f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 4 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.5f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 5 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.5f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 6 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.6f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 7 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.7f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 8 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.8f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 9 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 1.9f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else if (player._playerSkills._passive_IncreaseHealthRate.currentCount == 10 &&
                                 player._playerSkills._passive_IncreaseHealthRate._passive_HealthRateUnlocked)
                        {
                            currentHealth += _healthRegenerationAmount * 2.33f * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                        else
                        {
                            currentHealth += _healthRegenerationAmount * Time.deltaTime;
                            healthBar.SetCurrentHealth(Mathf.RoundToInt(currentHealth));
                        }
                    }
                }
            }
        }

        public void _RegenerateFocusPoint()
        {
            //DO NOT REGENERATE STAMINA IF WE ARE INTERACTING OR SPRINTING
            if (player.isInteracting || player.isSprinting)
            {
                _focusPtRengenTimer = 0;
            }
            else
            {
                _focusPtRengenTimer += Time.deltaTime;

                if (currentFocusPoints < maxFocusPoints && _healthRengenTimer > 1f)
                {
                    if (player.isBlocking)
                    {
                        currentFocusPoints += _focusPtRegenerationAmountWhilstBlocking * Time.deltaTime;
                        focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                    }
                    else
                    {
                        if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 1 &&
                            player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.1f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 2 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.2f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 3 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.3f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 4 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.4f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 5 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.5f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 6 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.6f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 7 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.7f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 8 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.8f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 9 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 1.9f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else if (player._playerSkills._passive_IncreaseFocusPointRate.currentCount == 10 &&
                                 player._playerSkills._passive_IncreaseFocusPointRate._passive_FocusRateUnlocked)
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * 2.33f * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                        else
                        {
                            currentFocusPoints += _focusPtRegenerationAmount * Time.deltaTime;
                            focusPointBar.SetCurrentFocusPoint(Mathf.RoundToInt(currentFocusPoints));
                        }
                    }
                }
            }
        }

        public override void HealCharacter(int healAmount)
        {
            base.HealCharacter(healAmount);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints = currentFocusPoints - focusPoints;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            currentSoulCount = currentSoulCount + souls;
        }

        public void AddPoints(int points)
        {
            currentPointCount = currentPointCount + points;
        }
    }
}