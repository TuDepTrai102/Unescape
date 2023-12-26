using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image currentSpellIcon;
        public Image currentConsumableIcon;
        public Text _currentConsumableItemAmount;
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponIconQuickSlots(bool isLeft, WeaponItem weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }

        public void UpdateCurrentSpellIcon(SpellItem spell)
        {
            if (spell.itemIcon != null)
            {
                currentSpellIcon.sprite = spell.itemIcon;
                currentSpellIcon.enabled = true;
            }
            else
            {
                currentSpellIcon.sprite = null;
                currentSpellIcon.enabled = false;
            }
        }

        public void UpdateCurrentConsumableIcon(ConsumableItem consumable)
        {
            if (consumable.itemIcon != null)
            {
                currentConsumableIcon.sprite = consumable.itemIcon;
                currentConsumableIcon.enabled = true;
                _currentConsumableItemAmount.text = consumable.currentItemAmount.ToString();
                _currentConsumableItemAmount.enabled = true;
            }
            else
            {
                currentConsumableIcon.sprite = null;
                currentConsumableIcon.enabled = false;
                _currentConsumableItemAmount.text = "NONE";
                _currentConsumableItemAmount.enabled = false;
            }
        }
    }
}