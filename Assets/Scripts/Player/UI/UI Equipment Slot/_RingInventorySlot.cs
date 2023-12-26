using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _RingInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public RingItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(RingItem newItem)
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
            if (uiManager._ringSlot01Selected)
            {
                //add the current equipped ring (if any) to our ring inventory
                if (uiManager.player.playerInventoryManager.ringSlot01 != null)
                {
                    uiManager.player.playerInventoryManager._ringsInventory.Add
                        (uiManager.player.playerInventoryManager.ringSlot01);
                }

                //remove the current equipped ring & replace it with our new ring
                uiManager.player.playerInventoryManager.ringSlot01 = item;
                //remove our newly equipped ring from our inventory
                uiManager.player.playerInventoryManager._ringsInventory.Remove(item);
            }
            else if (uiManager._ringSlot02Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot02 != null)
                {
                    uiManager.player.playerInventoryManager._ringsInventory.Add
                        (uiManager.player.playerInventoryManager.ringSlot02);
                }

                uiManager.player.playerInventoryManager.ringSlot02 = item;
                uiManager.player.playerInventoryManager._ringsInventory.Remove(item);
            }
            else if (uiManager._ringSlot03Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot03 != null)
                {
                    uiManager.player.playerInventoryManager._ringsInventory.Add
                        (uiManager.player.playerInventoryManager.ringSlot03);
                }

                uiManager.player.playerInventoryManager.ringSlot03 = item;
                uiManager.player.playerInventoryManager._ringsInventory.Remove(item);
            }
            else if (uiManager._ringSlot04Selected)
            {
                if (uiManager.player.playerInventoryManager.ringSlot04 != null)
                {
                    uiManager.player.playerInventoryManager._ringsInventory.Add
                        (uiManager.player.playerInventoryManager.ringSlot04);
                }

                uiManager.player.playerInventoryManager.ringSlot04 = item;
                uiManager.player.playerInventoryManager._ringsInventory.Remove(item);
            }
            else
            {
                return;
            }

            uiManager.player.playerInventoryManager.LoadRingEffects();

            //update the new gear to reflect on the ui/equipment screen
            uiManager.equipmentWindowUI._LoadRingOnEquipmentScreen(uiManager.player.playerInventoryManager);
            uiManager.ResetAllSelectedSlot();
        }

        public void _ItemStats()
        {
            if (item != null)
            {
                uiManager.itemStatsWindow.SetActive(true);
                uiManager.itemStatsWindowUI._UpdateRingItemStats(item);
            }
            else
            {
                uiManager.itemStatsWindow.SetActive(false);
                uiManager.itemStatsWindowUI._UpdateRingItemStats(null);
            }
        }
    }
}