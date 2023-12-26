using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class UIYellowBar : MonoBehaviour
    {
        public Slider slider;
        UIAICharacterHealthBar parentHealthBar;

        public float timer;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            parentHealthBar = GetComponentInParent<UIAICharacterHealthBar>();
        }

        private void OnEnable()
        {
            if (timer <= 0)
            {
                timer = 2f; //HOW LONG YOU WANT THE BAR TO WAIT BEFORE SUBTRACTING/EQUALIZING
            }
        }

        private void OnDisable()
        {
            parentHealthBar.currentDamageTaken = 0;
        }

        public void SetMaxStat(float maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }

        private void Update()
        {
            if (timer <= 0)
            {
                if (slider.value > parentHealthBar.slider.value)
                {
                    slider.value = slider.value - 3f;
                }
                else if (slider.value <= parentHealthBar.slider.value)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                timer = timer - Time.deltaTime;
            }
        }
    }
}