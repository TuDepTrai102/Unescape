using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class SoulCountBar : MonoBehaviour
    {
        public int soulCountNumber;
        public Text soulCountText;

        private void Awake()
        {
            soulCountText = GetComponentInChildren<Text>();
        }

        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }
    }
}