using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Equipment/Leg Equipment")]
    public class LegEquipment : EquipmentItem
    {
        public string hipModelName;
        public string leftLegName;
        public string rightLegName;
        public string leftLegKneeName;
        public string rightLegKneeName;
    }
}