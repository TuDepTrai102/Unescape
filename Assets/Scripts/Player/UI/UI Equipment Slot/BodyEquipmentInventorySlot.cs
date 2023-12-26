using NT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class BodyEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public BodyEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(BodyEquipment newItem)
        {
            if (newItem != null)
            {
                item = newItem;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
        }

        public void ClearInventorySlot()
        {
            if (item != null)
            {
                item = null;
                icon.sprite = null;
                icon.enabled = false;
                gameObject.SetActive(false);
            }
        }

        public void EquipThisItem()
        {
            if (uiManager.bodyEquipmentSlotSelected)
            {
                //add the current equipped torso (if any) to our torso inventory
                if (uiManager.player.playerInventoryManager.currentBodyEquipment != null)
                {
                    uiManager.player.playerInventoryManager.bodyEquipmentInventory.Add
                        (uiManager.player.playerInventoryManager.currentBodyEquipment);
                }
                //remove the current equipped torso & replace it with our new torso
                uiManager.player.playerInventoryManager.currentBodyEquipment = item;
                //remove our newly equipped torso from our inventory
                uiManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(item);
                //load the new gear
                uiManager.player.playerEquipmentManager.EquipAllArmor();
            }
            else
            {
                return;
            }

            //update the new gear to reflect on the ui/equipment screen
            uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uiManager.player.playerInventoryManager);
            uiManager.ResetAllSelectedSlot();
        }

        public void _ItemStats()
        {
            if (item != null)
            {
                uiManager.itemStatsWindow.SetActive(true);
                uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
            }
            else
            {
                uiManager.itemStatsWindow.SetActive(false);
                uiManager.itemStatsWindowUI.UpdateArmorItemStats(null);
            }
        }
    }
}