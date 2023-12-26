using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Equipment/Helmet Equipment")]
    public class HelmetEquipment : EquipmentItem
    {
        public string helmetModelName;

        //HIDE HAIR
        public bool hideHairFeatures = true;
        //HIDE BEARD
        public bool hideFacialFeatures = true;
        //HIDE EYEBROWS
        public bool hideEyebrowsFeatures = true;
        //ETC ETC
    }
}