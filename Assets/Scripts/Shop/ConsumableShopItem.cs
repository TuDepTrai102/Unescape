using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class ConsumableShopItem : MonoBehaviour
    {
        public PlayerManager player;
        public SoulCountBar soul;
        public ShopWindowUI shopWindow;
        public ConsumableItem consumable;
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
                player.playerInventoryManager._consumablesInventory.Add(consumable);
            }
            else
            {
                Debug.Log("NOT ENOUGH MONEY MAN [-_-!!!]");
            }
        }

        public void UpdateInforOfItem()
        {
            shopWindow.itemIcon_consumable.sprite = consumable.itemIcon;
            shopWindow.itemPrice_consumable.text = itemPrice.ToString();
            shopWindow.itemName_consumable.text = consumable.itemName.ToString();
            shopWindow.itemDescription_consumable.text = consumable.itemDescription.ToString();
        }
    }
}