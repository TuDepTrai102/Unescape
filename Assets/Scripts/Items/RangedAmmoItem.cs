using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class RangedAmmoItem : Item
    {
        [Header("AMMOTYPE")]
        public AmmoType ammoType;

        [Header("AMMO VELOCITY")]
        public float forwardVelocity = 550;
        public float upwardVelocity = 0;
        public float ammoMass = 0;
        public bool useGravity = false;

        [Header("AMMO CAPACITY")]
        public int carryLimit = 99;
        public int currentAmount = 99;

        [Header("AMMO BASE DAMAGE")]
        public int physicalDamage = 50;
        public int _fireDamage = 75;
        public int _darkDamage = 90;
        public int _magicDamage = 100;
        public int _lightningDamage = 150;

        [Header("ITEM MODELS/PREFABS")]
        public GameObject loadedItemModel; //the model that is displayed while drawing the bow back
        public GameObject liveItemModel; //the live model that is displayed can damage character
        public GameObject penetratedModel; // the model that is instantiates in to a collider on contact
    }
}