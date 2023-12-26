using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        PlayerManager player;

        public WeaponEquipmentSlotUI[] weaponEquipmentSlotsUI;
        public HeadEquipmentSlotUI headEquipmentSlotsUI;
        public BodyEquipmentSlotUI bodyEquipmentSlotsUI;
        public LegEquipmentSlotUI legEquipmentSlotsUI;
        public HandEquipmentSlotUI handEquipmentSlotsUI;

        public _ConsumableEquipmentSlotUI _consumableEquipmentSlotsUI;
        public _SpellEquipmentSlotUI _spellEquipmentSlotsUI;

        public _RingEquipmentSlot01UI _ringEquipmentSlot01_UI;
        public _RingEquipmentSlot02UI _ringEquipmentSlot02_UI;
        public _RingEquipmentSlot03UI _ringEquipmentSlot03_UI;
        public _RingEquipmentSlot04UI _ringEquipmentSlot04_UI;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlotsUI.Length; i++)
            {
                if (weaponEquipmentSlotsUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                }
                else if (weaponEquipmentSlotsUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                }
                else if (weaponEquipmentSlotsUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                }
                else
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                }
            }
        }

        public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            //CHECK HELMET ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentHelmetEquipment != null)
            {
                headEquipmentSlotsUI.AddItem(playerInventory.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotsUI.ClearItem();
            }

            //CHECK BODY ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentBodyEquipment != null)
            {
                bodyEquipmentSlotsUI.AddItem(playerInventory.currentBodyEquipment);
            }
            else
            {
                bodyEquipmentSlotsUI.ClearItem();
            }

            //CHECK LEG ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentLegEquipment != null)
            {
                legEquipmentSlotsUI.AddItem(playerInventory.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotsUI.ClearItem();
            }

            //CHECK HAND ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentHandEquipment != null)
            {
                handEquipmentSlotsUI.AddItem(playerInventory.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotsUI.ClearItem();
            }
        }

        public void _LoadConsumableOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            //CHECK CONSUMABLE ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentConsumable != null)
            {
                _consumableEquipmentSlotsUI.AddItem(playerInventory.currentConsumable);
            }
            else
            {
                _consumableEquipmentSlotsUI.ClearItem();
            }
        }

        public void _LoadSpellOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            //CHECK SPELL ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.currentSpell != null)
            {
                _spellEquipmentSlotsUI.AddItem(playerInventory.currentSpell);
            }
            else
            {
                _spellEquipmentSlotsUI.ClearItem();
            }
        }

        public void _LoadRingOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            //CHECK RING ITEM, LOAD THIS (IF HAVE)
            if (playerInventory.ringSlot01 != null)
            {
                _ringEquipmentSlot01_UI.AddItem(playerInventory.ringSlot01);
            }
            else
            {
                _ringEquipmentSlot01_UI.ClearItem();
            }

            if (playerInventory.ringSlot02 != null)
            {
                _ringEquipmentSlot02_UI.AddItem(playerInventory.ringSlot02);
            }
            else
            {
                _ringEquipmentSlot02_UI.ClearItem();
            }

            if (playerInventory.ringSlot03 != null)
            {
                _ringEquipmentSlot03_UI.AddItem(playerInventory.ringSlot03);
            }
            else
            {
                _ringEquipmentSlot03_UI.ClearItem();
            }

            if (playerInventory.ringSlot04 != null)
            {
                _ringEquipmentSlot04_UI.AddItem(playerInventory.ringSlot04);
            }
            else
            {
                _ringEquipmentSlot04_UI.ClearItem();
            }
        }
    }
}