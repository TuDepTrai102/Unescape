using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _ConsumableEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        ConsumableItem item;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(ConsumableItem consumable)
        {
            if (consumable != null)
            {
                item = consumable;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                ClearItem();
            }
        }

        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uiManager._consumableSlotSelected = true;
            uiManager.itemStatsWindowUI._UpdateConsumableItemStats(item);
        }
    }
}