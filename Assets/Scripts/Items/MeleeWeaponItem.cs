using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Melee Weapon Item")]
    public class MeleeWeaponItem : WeaponItem
    {
        public bool canBuffed = true;
    }
}