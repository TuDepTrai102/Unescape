using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class HeadEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public HelmetEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(HelmetEquipment newItem)
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
            if (uiManager.headEquipmentSlotSelected)
            {
                //add the current equipped helmet (if any) to our helmet inventory
                if (uiManager.player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    uiManager.player.playerInventoryManager.headEquipmentInventory.Add
                        (uiManager.player.playerInventoryManager.currentHelmetEquipment);
                }
                //remove the current equipped helmet & replace it with our new helmet
                uiManager.player.playerInventoryManager.currentHelmetEquipment = item;
                //remove our newly equipped helmet from our inventory
                uiManager.player.playerInventoryManager.headEquipmentInventory.Remove(item);
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