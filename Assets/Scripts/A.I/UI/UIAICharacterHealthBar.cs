using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class UIAICharacterHealthBar : MonoBehaviour
    {
        public Slider slider;
        private float timeUntilBarIsHidden = 0;
        [SerializeField] UIYellowBar yellowBar;
        [SerializeField] float yellowBarTimer = 2.5f;
        [SerializeField] Text damageText;
        public int currentDamageTaken;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(float health)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true); //TRIGGERS THE ONENABLE FUNCTION

                yellowBar.timer = yellowBarTimer; //EVERYTIME WE GET HIT, WE RENEW THE TIMER

                if (health > slider.value)
                {
                    yellowBar.slider.value = health;
                }
            }

            currentDamageTaken = currentDamageTaken + Mathf.RoundToInt(slider.value - health);
            damageText.text = currentDamageTaken.ToString();

            slider.value = health;
            timeUntilBarIsHidden = 3;
        }

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                transform.LookAt(transform.position + Camera.main.transform.forward);

                timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

                if (slider != null)
                {
                    if (timeUntilBarIsHidden <= 0)
                    {
                        timeUntilBarIsHidden = 0;
                        slider.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!slider.gameObject.activeInHierarchy)
                        {
                            slider.gameObject.SetActive(true);
                        }
                    }

                    if (slider.value <= 0)
                    {
                        Destroy(slider.gameObject);
                    }
                }
            }
        }
    }
}