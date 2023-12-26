using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class HandEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public HandEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HandEquipment newItem)
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
            if (uiManager.handEquipmentSlotSelected)
            {
                //add the current equipped hand (if any) to our hand inventory
                if (uiManager.player.playerInventoryManager.currentHandEquipment != null)
                {
                    uiManager.player.playerInventoryManager.handEquipmentInventory.Add
                        (uiManager.player.playerInventoryManager.currentHandEquipment);
                }
                //remove the current equipped hand & replace it with our new hand
                uiManager.player.playerInventoryManager.currentHandEquipment = item;
                //remove our newly equipped hand from our inventory
                uiManager.player.playerInventoryManager.handEquipmentInventory.Remove(item);
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