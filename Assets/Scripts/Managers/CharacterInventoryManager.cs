using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("CURRENT ITEM BEING USED")]
        public Item currentItemBeingUsed;

        [Header("QUICK SLOT ITEMS")]
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumable;
        public RangedAmmoItem currentAmmo;

        [Header("CURRENT ARMOR EQUIPMENT")]
        public HelmetEquipment currentHelmetEquipment;
        public BodyEquipment currentBodyEquipment;
        public LegEquipment currentLegEquipment;
        public HandEquipment currentHandEquipment;
        public RingItem ringSlot01;
        public RingItem ringSlot02;
        public RingItem ringSlot03;
        public RingItem ringSlot04;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            character.characterWeaponSlotManager.LoadBothWeaponOnSlots();
            LoadRingEffects();
        }

        //CALL IN YOUR FUNCTION AFTER LOADING EQUIPMENT
        public virtual void LoadRingEffects()
        {
            if (ringSlot01 != null)
            {
                ringSlot01.EquipRing(character);
            }

            if (ringSlot02 != null)
            {
                ringSlot02.EquipRing(character);
            }

            if (ringSlot03 != null)
            {
                ringSlot03.EquipRing(character);
            }

            if (ringSlot04 != null)
            {
                ringSlot04.EquipRing(character);
            }
        }
    }
}