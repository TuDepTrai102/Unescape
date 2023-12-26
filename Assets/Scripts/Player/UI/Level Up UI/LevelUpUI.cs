using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager player;
        public Button confirmLevelUpButton;

        [Header("PLAYER LEVEL")]
        public int currentPlayerLevel; //THE CURRENT LEVEL WE ARE BEFORE LEVELING UP
        public int projectedPlayerLevel; //THE POSSIBLE LEVEL WE WILL BE IF WE ACCEPT LEVELING UP
        public Text currentPlayerLevelText; //THE UI TEXT NUMBER OF THE CURRENT PLAYER LEVEL      
        public Text projectedPlayerLevelText; //THE UI TEXT PEJECTED PLAYER LEVEL WE WILL BE

        [Header("SOULS")]
        public Text currentSoulsText;
        public Text soulsRequiredToLevelUpText;
        private int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("HEALTH")]
        public Slider healthSlider;
        public Text currentHealthLevelText;
        public Text projectedHealthLevelText;

        [Header("STAMINA")]
        public Slider staminaSlider;
        public Text currentStaminaLevelText;
        public Text projectedStaminaLevelText;

        [Header("FOCUS POINTS")]
        public Slider focusSlider;
        public Text currentFocusLevelText;
        public Text projectedFocusLevelText;

        [Header("POISE")]
        public Slider poiseSlider;
        public Text currentPoiseLevelText;
        public Text projectedPoiseLevelText;

        [Header("STRENGTH")]
        public Slider strengthSlider;
        public Text currentStrengthLevelText;
        public Text projectedStrengthLevelText;

        [Header("DEXTERITY")]
        public Slider dexteritySlider;
        public Text currentDexterityLevelText;
        public Text projectedDexterityLevelText;

        [Header("FAITH")]
        public Slider faithSlider;
        public Text currentFaithLevelText;
        public Text projectedFaithLevelText;

        [Header("INTELLIGENCE / WISDOM POINTS")]
        public Slider intelligenceSlider;
        public Text currentIntelligenceLevelText;
        public Text projectedIntelligenceLevelText;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        //UPDATE ALL OF THE STATS ON THE UI TO THE PLAYER'S CURRENT STATS
        private void OnEnable()
        {
            player.isBusy = true;

            currentPlayerLevel = player.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = player.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = player.playerStatsManager.healthLevel;
            healthSlider.minValue = player.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = player.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = player.playerStatsManager.healthLevel.ToString();

            staminaSlider.value = player.playerStatsManager.staminaLevel;
            staminaSlider.minValue = player.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = player.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = player.playerStatsManager.staminaLevel.ToString();

            focusSlider.value = player.playerStatsManager.focusLevel;
            focusSlider.minValue = player.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;
            currentFocusLevelText.text = player.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = player.playerStatsManager.focusLevel.ToString();

            poiseSlider.value = player.playerStatsManager.poiseLevel;
            poiseSlider.minValue = player.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            currentPoiseLevelText.text = player.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = player.playerStatsManager.poiseLevel.ToString();

            strengthSlider.value = player.playerStatsManager.strengthLevel;
            strengthSlider.minValue = player.playerStatsManager.strengthLevel;
            strengthSlider.maxValue = 99;
            currentStrengthLevelText.text = player.playerStatsManager.strengthLevel.ToString();
            projectedStrengthLevelText.text = player.playerStatsManager.strengthLevel.ToString();

            dexteritySlider.value = player.playerStatsManager.dexterityLevel;
            dexteritySlider.minValue = player.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;
            currentDexterityLevelText.text = player.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = player.playerStatsManager.dexterityLevel.ToString();

            intelligenceSlider.value = player.playerStatsManager.intelligenceLevel;
            intelligenceSlider.minValue = player.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            currentIntelligenceLevelText.text = player.playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = player.playerStatsManager.intelligenceLevel.ToString();

            faithSlider.value = player.playerStatsManager.faithLevel;
            faithSlider.minValue = player.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            currentFaithLevelText.text = player.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = player.playerStatsManager.faithLevel.ToString();

            currentSoulsText.text = player.playerStatsManager.currentSoulCount.ToString();

            UpdateProjectedPlayerLevel();
        }

        //UPDATE THE PLAYER'S STATS TO THE PROJECTED STATS, PROVIDING THEY HAVE ENOUGH SOULS TO CONFIRM
        public void ConfirmPlayerLevelUpStat()
        {
            player.playerStatsManager.playerLevel = projectedPlayerLevel;
            player.playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            player.playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            player.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
            player.playerStatsManager.poiseLevel = Mathf.RoundToInt(poiseSlider.value);
            player.playerStatsManager.strengthLevel = Mathf.RoundToInt(strengthSlider.value);
            player.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            player.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
            player.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);

            player.playerStatsManager.maxHealth = player.playerStatsManager.SetMaxHealthFromHealthLevel();
            player.playerStatsManager.maxStamina = player.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            player.playerStatsManager.maxFocusPoints = player.playerStatsManager.SetMaxFocusPointsFromFocusLevel();

            player.playerStatsManager.currentSoulCount = player.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
            player.uiManager.soulCount.text = player.playerStatsManager.currentSoulCount.ToString();

            player.playerStatsManager.CalculateAndSetMaxEquipLoad();

            player.isBusy = false;
            gameObject.SetActive(false);
        }

        private void CalculateSoulCostToLevelUp()
        {
            for (int i = 0; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp += Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
            }
        }

        //UPDATE THE PROJECTED PLAYER'S TOTAL LEVEL, BY ADDING UP ALL THE PROJECTED LEVEL UP STATS
        private void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (healthSlider.value) - player.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (staminaSlider.value) - player.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (focusSlider.value) - player.playerStatsManager.focusLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (poiseSlider.value) - player.playerStatsManager.poiseLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (strengthSlider.value) - player.playerStatsManager.strengthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (dexteritySlider.value) - player.playerStatsManager.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt
                (faithSlider.value) - player.playerStatsManager.faithLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUp = player.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (player.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = poiseSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = strengthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}