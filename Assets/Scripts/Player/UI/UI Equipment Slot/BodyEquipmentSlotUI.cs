using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class BodyEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        BodyEquipment item;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(BodyEquipment bodyEquipment)
        {
            if (bodyEquipment != null)
            {
                item = bodyEquipment;
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
            uiManager.bodyEquipmentSlotSelected = true;
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}