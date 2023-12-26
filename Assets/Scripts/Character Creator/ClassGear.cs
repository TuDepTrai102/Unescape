using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [System.Serializable]
    public class ClassGear
    {
        [Header("CLASS NAME")]
        public string className;

        [Header("WEAPONS")]
        public WeaponItem primaryWeapon;
        public WeaponItem offHandWeapon;
        //public WeaponItem secondaryWeapon;

        [Header("AMMO ITEM")]
        public RangedAmmoItem ammoItem;

        [Header("ARMOR")]
        public HelmetEquipment headEquipment;
        public BodyEquipment chestEquipment;
        public LegEquipment legEquipment;
        public HandEquipment handEquipment;

        //public SpellItem startingSpell;
    }
}