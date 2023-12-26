using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class RangedShopItem : MonoBehaviour
    {
        public PlayerManager player;
        public SoulCountBar soul;
        public ShopWindowUI shopWindow;
        public WeaponItem weapon;
        public RangedAmmoItem ammo;
        public int itemPrice;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
            soul = FindObjectOfType<SoulCountBar>();
            shopWindow = FindObjectOfType<ShopWindowUI>();
        }

        public void Buy()
        {
            if (player.playerStatsManager.currentSoulCount >= itemPrice)
            {
                player.playerStatsManager.currentSoulCount -= itemPrice;
                soul.SetSoulCountText(player.playerStatsManager.currentSoulCount);
                player.playerInventoryManager.weaponsInventory.Add(weapon);
            }
            else
            {
                Debug.Log("NOT ENOUGH MONEY MAN [-_-!!!]");
            }
        }

        public void Buy_Arrow()
        {
            if (player.playerStatsManager.currentSoulCount >= itemPrice)
            {
                player.playerStatsManager.currentSoulCount -= itemPrice;
                soul.SetSoulCountText(player.playerStatsManager.currentSoulCount);
                player.playerInventoryManager.currentAmmo = ammo;
            }
            else
            {
                Debug.Log("NOT ENOUGH MONEY MAN [-_-!!!]");
            }
        }

        public void UpdateInforOfItem()
        {
            shopWindow.itemIcon_ranged.sprite = weapon.itemIcon;
            shopWindow.itemPrice_ranged.text = itemPrice.ToString();
            shopWindow.itemName_ranged.text = weapon.itemName.ToString();
            shopWindow.itemDescription_ranged.text = weapon.itemDescription.ToString();
        }

        public void UpdateInforOfItem_Arrow()
        {
            shopWindow.itemIcon_ranged.sprite = ammo.itemIcon;
            shopWindow.itemPrice_ranged.text = itemPrice.ToString();
            shopWindow.itemName_ranged.text = ammo.itemName.ToString();
            shopWindow.itemDescription_ranged.text = ammo.itemDescription.ToString();
        }
    }
}