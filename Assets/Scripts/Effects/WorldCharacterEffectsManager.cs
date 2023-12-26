using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("DAMAGE")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;

        [Header("POISON")]
        public PoisonBuildUpEffect poisonBuildUpEffect;
        public PoisonedEffect poisonedEffect;
        public GameObject poisonFX;
        public AudioClip poisonSFX;

        [Header("BLEED")]
        public _BleedBuildUpEffect _bleedBuildUpEffect;
        public _BleededEffect _bleededEffect;
        public GameObject _bleedFX;
        public AudioClip _bleedSFX;

        [Header("CURSE")]
        public _CurseBuildUpEffect _curseBuildUpEffect;
        public _CursedEffect _cursedEffect;
        public GameObject _curseFX;
        public AudioClip _curseSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}