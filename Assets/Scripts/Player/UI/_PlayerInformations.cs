using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _PlayerInformations : MonoBehaviour
    {
        PlayerManager player;

        [Header("PLAYER")]
        [Header("Avatar")]
        public Image characterAvatar;
        public Text characterName;

        [Header("Stats Text")]
        public Text healthText;
        public Text staminaText;
        public Text focusText;
        public Text poiseText;
        public Text strengthText;
        public Text intelligenceText;
        public Text dexterityText;
        public Text faithText;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void _StatInforUI()
        {
            if (player.playerStatsManager._characterAvatar != null)
            {
                characterAvatar.enabled = true;
                characterAvatar.sprite = player.playerStatsManager._characterAvatar.sprite;
            }
            else
            {
                characterAvatar.enabled = false;
            }

            characterName.text = player.playerStatsManager.characterName.ToString();

            healthText.text = player.playerStatsManager.healthLevel.ToString();
            staminaText.text = player.playerStatsManager.staminaLevel.ToString();
            focusText.text = player.playerStatsManager.focusLevel.ToString();
            dexterityText.text = player.playerStatsManager.dexterityLevel.ToString();
            intelligenceText.text = player.playerStatsManager.poiseLevel.ToString();
            faithText.text = player.playerStatsManager.faithLevel.ToString();
            poiseText.text = player.playerStatsManager.poiseLevel.ToString();
            strengthText.text = player.playerStatsManager.strengthLevel.ToString();
        }
    }
}