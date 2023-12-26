using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _BleedBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }

        public void _SetBleedBuildUpAmount(int currentBleedBuildUp)
        {
            slider.value = currentBleedBuildUp;

            if (currentBleedBuildUp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}