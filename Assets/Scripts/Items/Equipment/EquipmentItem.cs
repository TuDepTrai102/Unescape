using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class EquipmentItem : Item
    {
        [Header("DEFENSE BONUS")]
        public float physicalDefense;
        public float magicDefense;
        //FIRE DEF
        public float _fireDefense;
        //LIGHTNING DEF
        public float _lightningDefense;
        //DARKNESS DEF
        public float _darkDefense;
        public float _bleedDefense;

        [Header("WEIGHT")]
        public float weight = 0;

        [Header("RESISTANCES")]
        public float poisonResistance;
        public float _bleedResistance;
        public float _curseResistance;
    }
}