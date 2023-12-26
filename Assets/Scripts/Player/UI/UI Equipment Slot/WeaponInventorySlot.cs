using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        public WeaponItem item;

        private void Awake()
        {
            uiManager = GetComponentInParent<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
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
            if (uiManager.rightHandSlot01Selected)
            {
                uiManager.player.playerInventoryManager.weaponsInventory.Add
                    (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
                uiManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = item;
                uiManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                Debug.Log(uiManager.player.playerInventoryManager.weaponsInventory.Remove(item).ToString());
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                uiManager.player.playerInventoryManager.weaponsInventory.Add
                    (uiManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
                uiManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = item;
                uiManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                Debug.Log(uiManager.player.playerInventoryManager.weaponsInventory.Remove(item).ToString());
            }
            else if (uiManager.leftHandSlot01Selected)
            {
                uiManager.player.playerInventoryManager.weaponsInventory.Add
                    (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = item;
                uiManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                Debug.Log(uiManager.player.playerInventoryManager.weaponsInventory.Remove(item).ToString());
            }
            else if (uiManager.leftHandSlot02Selected)
            {
                uiManager.player.playerInventoryManager.weaponsInventory.Add
                    (uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                uiManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = item;
                uiManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                Debug.Log(uiManager.player.playerInventoryManager.weaponsInventory.Remove(item).ToString());
            }
            else
            {
                return;
            }

            uiManager.player.playerInventoryManager.rightWeapon = 
                uiManager.player.playerInventoryManager.weaponsInRightHandSlots
                [uiManager.player.playerInventoryManager.currentRightWeaponIndex];

            uiManager.player.playerInventoryManager.leftWeapon = 
                uiManager.player.playerInventoryManager.weaponsInLeftHandSlots
                [uiManager.player.playerInventoryManager.currentLeftWeaponIndex];


            uiManager.player.playerWeaponSlotManager.LoadWeaponOnSLot(uiManager.player.playerInventoryManager.rightWeapon, false);
            uiManager.player.playerWeaponSlotManager.LoadWeaponOnSLot(uiManager.player.playerInventoryManager.leftWeapon, true);

            ClearInventorySlot();
            uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(uiManager.player.playerInventoryManager);
            uiManager.ResetAllSelectedSlot();
        }

        public void _ItemStats()
        {
            if (item != null)
            {
                uiManager.itemStatsWindow.SetActive(true);
                uiManager.itemStatsWindowUI.UpdateWeaponItemStats(item);
            }
            else
            {
                uiManager.itemStatsWindow.SetActive(false);
                uiManager.itemStatsWindowUI.UpdateWeaponItemStats(null);
            }
        }
    }
}