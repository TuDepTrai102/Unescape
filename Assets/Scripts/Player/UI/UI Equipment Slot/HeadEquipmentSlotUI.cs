using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class HeadEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        HelmetEquipment item;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(HelmetEquipment helmetEquipment)
        {
            if (helmetEquipment != null)
            {
                item = helmetEquipment;
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
            uiManager.headEquipmentSlotSelected = true;
            uiManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}