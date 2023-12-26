using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NT
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public List<WeaponItem> weaponItems = new List<WeaponItem>();

        public List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

        public List<RangedAmmoItem> rangedAmmoItems = new List<RangedAmmoItem>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public WeaponItem GetWeaponItemByID(int weaponID)
        {
            return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
        }

        public EquipmentItem GetEquipmentItemByID(int equipmentID)
        {
            return equipmentItems.FirstOrDefault(equipment => equipment.itemID == equipmentID);
        }

        public RangedAmmoItem GetAmmoItemByID(int ammoID)
        {
            return rangedAmmoItems.FirstOrDefault(ammo => ammo.itemID == ammoID);
        }
    }
}