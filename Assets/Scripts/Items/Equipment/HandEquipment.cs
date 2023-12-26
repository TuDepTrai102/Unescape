using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Equipment/Hand Equipment")]
    public class HandEquipment : EquipmentItem
    {
        public string leftHandModelName;
        public string rightHandModelName;
        public string lowerLeftArmModelName;
        public string lowerRightArmModelName;
    }
}