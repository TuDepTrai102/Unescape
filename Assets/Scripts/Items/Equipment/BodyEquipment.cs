using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Equipment/Torso Equipment")]
    public class BodyEquipment : EquipmentItem
    {
        public string torsoModelName;
        public string upperLeftArmModelName;
        public string upperRightArmModelName;
        public string leftShoulderModelName;
        public string rightShoulderModelName;
        public string leftElbowModelName;
        public string rightElbowModelName;
    }
}