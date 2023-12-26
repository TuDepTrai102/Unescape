using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Ranged Weapon Item")]
    public class RangedWeaponItem : WeaponItem
    {
        public bool canBuffed = true;
    }
}