using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _SpellEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        SpellItem item;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(SpellItem spell)
        {
            if (spell != null)
            {
                item = spell;
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
            uiManager._spellSlotSelected = true;
            uiManager.itemStatsWindowUI._UpdateSpellItemStats(item);
        }

        public void _StillChooseSpell()
        {
            if (uiManager.itemStatsWindowUI._quickChangeSpell.activeInHierarchy)
            {
                uiManager.itemStatsWindow.SetActive(false);
            }
        }
    }
}