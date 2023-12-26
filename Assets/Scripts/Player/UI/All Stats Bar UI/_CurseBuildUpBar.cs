using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _CurseBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }

        public void _SetCurseBuildUpAmount(int currentCurseBuildUp)
        {
            slider.value = currentCurseBuildUp;

            if (currentCurseBuildUp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
