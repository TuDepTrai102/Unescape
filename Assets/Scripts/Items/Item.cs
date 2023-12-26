using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class Item : ScriptableObject
    {
        [Header("ITEM INFORMATION")]
        public Sprite itemIcon;
        public string itemName;
        public int itemID;

        [Header("DESCRIPTION ITEM")]
        [TextArea] public string itemDescription;
    }
}