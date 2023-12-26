using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class MeleeShopItem : MonoBehaviour
    {
        public PlayerManager player;
        public SoulCountBar soul;
        public ShopWindowUI shopWindow;
        public WeaponItem weapon;
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

        public void UpdateInforOfItem()
        {
            shopWindow.itemIcon_melee.sprite = weapon.itemIcon;
            shopWindow.itemPrice_melee.text = itemPrice.ToString();
            shopWindow.itemName_melee.text = weapon.itemName.ToString();
            shopWindow.itemDescription_melee.text = weapon.itemDescription.ToString();
        }
    }
}