using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _CurseAmountBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 100;
            gameObject.SetActive(false);
        }

        public void _SetCurseAmount(int curseAmount)
        {
            if (curseAmount > 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }

            slider.value = curseAmount;
        }
    }
}
