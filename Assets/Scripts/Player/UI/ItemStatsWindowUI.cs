using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class ItemStatsWindowUI : MonoBehaviour
    {
        public Text itemNameText;
        public Image itemIconImage;

        [Header("EQUIPMENT STATS WINDOW")]
        public GameObject weaponStats;
        public GameObject armorStats;
        public GameObject _consumableStats;
        public GameObject _spellStats;
        public GameObject _ringStats;
        public GameObject _quickChangeSpell;

        [Header("WEAPON STATS")]
        public Text physicalDamageText;
        public Text _fireDamageText;
        public Text _darkDamageText;
        public Text _lightningDamageText;
        public Text magicDamageText;
        public Text _bleedDamageText;

        public Text physicalAbsorptionText;
        public Text _fireAbsorptionText;
        public Text _darkAbsorptionText;
        public Text _lightningAbsorptionText;
        public Text magicAbsorptionText;
        public Text _bleedAbsorptionText;

        [Header("ARMOR STATS")]
        public Text armorPhysicalAbsorptionText;
        public Text _armorFireAbsorptionText;
        public Text _armorDarkAbsorptionText;
        public Text _armorLightningAbsorptionText;
        public Text armorMagicAbsorptionText;
        public Text _armorBleedAbsorptionText;
        public Text armorPoisonResistanceText;

        [Header("CONSUMABLE STATS")]
        public Text _consumableAmountText;

        [Header("SPELL STATS")]
        public Text _spellAbilityText;

        [Header("RING STATS")]
        public Text _ringEffectInformationText;

        [Header("DESCRIPTION OF ITEM")]
        public Text weaponDescriptionText;
        public Text armorDescriptionText;
        public Text _consumableDescriptionText;
        public Text _spellDescriptionText;
        public Text _ringDescriptionText;

        //UPDATE WEAPON ITEM STATS
        public void UpdateWeaponItemStats(WeaponItem weapon)
        {
            CloseAllStatWindows();

            if (weapon != null)
            {
                if (weapon.itemName != null)
                {
                    itemNameText.text = weapon.itemName;
                }
                else
                {
                    itemNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (weapon.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = weapon.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                physicalDamageText.text = weapon.physicalDamage.ToString();
                physicalAbsorptionText.text = weapon.physicalBlockingDamageAbsorption.ToString();

                _fireDamageText.text = weapon.fireDamage.ToString();
                _fireAbsorptionText.text = weapon.fireBlockingDamageAbsorption.ToString();

                _darkDamageText.text = weapon._darkDamage.ToString();
                _darkAbsorptionText.text = weapon._darkBlockingDamageAbsorption.ToString();

                _lightningDamageText.text = weapon._lightningDamage.ToString();
                _lightningAbsorptionText.text = weapon._lightningBlockingDamageAbsorption.ToString();

                //MAGIC DAMAGE
                magicDamageText.text = weapon._magicDamage.ToString();
                //MAGIC ABSORPTION
                magicAbsorptionText.text = weapon._magicBlockingDamageAbsorption.ToString();

                _bleedDamageText.text = weapon._bleedDamage.ToString();
                _bleedAbsorptionText.text = weapon._bleedBlockingDamageAbsorption.ToString();

                weaponDescriptionText.text = weapon.itemDescription.ToString();

                weaponStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                weaponStats.SetActive(false);
            }
        }

        //UPDATE ARMOR ITEM STATS
        public void UpdateArmorItemStats(EquipmentItem armor)
        {
            CloseAllStatWindows();

            if (armor != null)
            {
                if (armor.itemName != null)
                {
                    itemNameText.text = armor.itemName;
                }
                else
                {
                    itemNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (armor.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = armor.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                armorPhysicalAbsorptionText.text = armor.physicalDefense.ToString();
                _armorFireAbsorptionText.text = armor._fireDefense.ToString();
                _armorDarkAbsorptionText.text = armor._darkDefense.ToString();
                _armorLightningAbsorptionText.text = armor._lightningDefense.ToString();
                armorMagicAbsorptionText.text = armor.magicDefense.ToString();
                _armorBleedAbsorptionText.text = armor._bleedDefense.ToString();
                armorPoisonResistanceText.text = armor.poisonResistance.ToString();

                armorDescriptionText.text = armor.itemDescription.ToString();

                armorStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                armorStats.SetActive(false);
            }
        }

        //UPDATE CONSUMABLE ITEM STATS
        public void _UpdateConsumableItemStats(ConsumableItem consumable)
        {
            CloseAllStatWindows();

            if (consumable != null)
            {
                if (consumable.itemName != null)
                {
                    itemNameText.text = consumable.itemName;
                }
                else
                {
                    itemNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (consumable.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = consumable.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                _consumableAmountText.text = consumable.maxItemAmount.ToString();

                _consumableDescriptionText.text = consumable.itemDescription.ToString();

                _consumableStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                _consumableStats.SetActive(false);
            }
        }

        //UPDATE SPELL ITEM STATS
        public void _UpdateSpellItemStats(SpellItem spell)
        {
            CloseAllStatWindows();

            if (spell != null)
            {
                if (spell.itemName != null)
                {
                    itemNameText.text = spell.itemName;
                }
                else
                {
                    itemNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (spell.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = spell.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                _spellAbilityText.text = spell.spellDescription.ToString();

                _spellDescriptionText.text = spell.itemDescription.ToString();

                _spellStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                _spellStats.SetActive(false);
            }
        }

        //UPDATE RING ITEM STATS
        public void _UpdateRingItemStats(RingItem ring)
        {
            CloseAllStatWindows();

            if (ring != null)
            {
                if (ring.itemName != null)
                {
                    itemNameText.text = ring.itemName;
                }
                else
                {
                    itemNameText.text = ""; //or itemNameText.text = string.Empty;
                }

                if (ring.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.enabled = true;
                    itemIconImage.sprite = ring.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.enabled = false;
                    itemIconImage.sprite = null;
                }

                _ringEffectInformationText.text = ring.itemEffectInformation.ToString();

                _ringDescriptionText.text = ring.itemDescription.ToString();

                _ringStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.enabled = false;
                itemIconImage.sprite = null;
                _ringStats.SetActive(false);
            }
        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
            armorStats.SetActive(false);
            _consumableStats.SetActive(false);
            _spellStats.SetActive(false);
            _ringStats.SetActive(false);
        }
    }
}