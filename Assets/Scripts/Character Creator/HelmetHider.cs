using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class HelmetHider : MonoBehaviour
    {
        PlayerManager player;
        HelmetEquipment helmet;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void HideHelmet()
        {
            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                helmet = player.playerInventoryManager.currentHelmetEquipment;
                player.playerInventoryManager.currentHelmetEquipment = null;
                player.playerEquipmentManager.EquipAllArmor();
            }
        }

        public void UnHideHelmet()
        {
            if (helmet != null)
            {
                player.playerInventoryManager.currentHelmetEquipment = helmet;
                player.playerEquipmentManager.EquipAllArmor();
            }
        }
    }
}