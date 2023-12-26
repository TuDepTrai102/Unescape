using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _ConsumableInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public ConsumableItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(ConsumableItem newItem)
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
            if (uiManager._consumableSlotSelected)
            {
                //add the current equipped consumable (if any) to our consumable inventory
                if (uiManager.player.playerInventoryManager.currentConsumable != null)
                {
                    uiManager.player.playerInventoryManager._consumablesInventory.Add
                        (uiManager.player.playerInventoryManager.currentConsumable);
                }
                //remove the current equipped consumable & replace it with our new consumable
                uiManager.player.playerInventoryManager.currentConsumable = item;
                //remove our newly equipped consumable from our inventory
                uiManager.player.playerInventoryManager._consumablesInventory.Remove(item);
            }
            else
            {
                return;
            }

            if (uiManager.player.playerInventoryManager.currentConsumable != null)
            {
                uiManager.quickSlotsUI.UpdateCurrentConsumableIcon(uiManager.player.playerInventoryManager.currentConsumable);
            }
            else
            {
                uiManager.quickSlotsUI.currentConsumableIcon.sprite = null;
                uiManager.quickSlotsUI.currentConsumableIcon.enabled = false;
            }

            //update the new gear to reflect on the ui/equipment screen
            uiManager.equipmentWindowUI._LoadConsumableOnEquipmentScreen(uiManager.player.playerInventoryManager);
            uiManager.ResetAllSelectedSlot();
        }

        public void _ItemStats()
        {
            if (item != null)
            {
                uiManager.itemStatsWindow.SetActive(true);
                uiManager.itemStatsWindowUI._UpdateConsumableItemStats(item);
            }
            else
            {
                uiManager.itemStatsWindow.SetActive(false);
                uiManager.itemStatsWindowUI._UpdateConsumableItemStats(null);
            }
        }
    }
}