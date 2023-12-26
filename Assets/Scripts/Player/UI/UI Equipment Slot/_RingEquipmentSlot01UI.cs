using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _RingEquipmentSlot01UI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        RingItem ring;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(RingItem newRing)
        {
            if (newRing != null)
            {
                ring = newRing;
                icon.sprite = ring.itemIcon;
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
            ring = null;
            icon.sprite = null;
            icon.enabled = false;
        }

        public void SelectThisSlot()
        {
            uiManager._ringSlot01Selected = true;
            uiManager.itemStatsWindowUI._UpdateRingItemStats(ring);
        }

        public void _UnEquipRingInThisSlot()
        {
            if (ring != null)
            {
                uiManager.ResetAllSelectedSlot();
                ring.UnEquipRing(uiManager.player);
                uiManager.player.playerInventoryManager._ringsInventory.Add(this.ring);
                uiManager.player.playerInventoryManager.ringSlot01 = null;
                ClearItem();
            }
        }
    }
}