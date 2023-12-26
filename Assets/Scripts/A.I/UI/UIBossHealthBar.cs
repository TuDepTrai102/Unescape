using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public Text bossName;
        public Image healthFrame;
        public Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<Text>();
            healthFrame = GetComponentInChildren<Image>();
        }

        private void Start()
        {
            SetHealthBarInactive();
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
            healthFrame.gameObject.SetActive(true);
        }

        public void SetHealthBarInactive()
        {
            slider.gameObject.SetActive(false);
            healthFrame.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetBossCurrentHealth(float currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}