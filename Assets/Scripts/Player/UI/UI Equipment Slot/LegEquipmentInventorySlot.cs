using NT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class LegEquipmentInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public LegEquipment item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(LegEquipment newItem)
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
            if (uiManager.legEquipmentSlotSelected)
            {
                //add the current equipped leg (if any) to our leg inventory
                if (uiManager.player.playerInventoryManager.currentLegEquipment != null)
                {
                    uiManager.player.playerInventoryManager.legEquipmentInventory.Add
                        (uiManager.player.playerInventoryManager.currentLegEquipment);
                }
                //remove the current equipped leg & replace it with our new leg
                uiManager.player.playerInventoryManager.currentLegEquipment = item;
                //remove our newly equipped leg from our inventory
                uiManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
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