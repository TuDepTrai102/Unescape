using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("ANIMATOR REPLACER")]
        public AnimatorOverrideController weaponController;
        //public string offHandIdleAnimation = "Left Arm Idle";

        [Header("WEAPON TYPE")]
        public WeaponType weaponType;

        [Header("DAMAGE")]
        public int physicalDamage;
        public int fireDamage;
        public int _lightningDamage;
        public int _darkDamage;
        public int _magicDamage;
        public int _bleedDamage;

        [Header("DAMAGE MODIFIERS")]
        public float lightAttackDamageModifier;
        public float heavyAttackDamageModifier;
        //Running Attack Modifier;
        //Jumping Attack Modifier;
        public int criticalDamageMultiplier = 4;
        public float guardBreakModifier = 1;

        [Header("POISE")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("ABSORPTIONS")]
        public float physicalBlockingDamageAbsorption;
        public float fireBlockingDamageAbsorption;
        public float _lightningBlockingDamageAbsorption;
        public float _darkBlockingDamageAbsorption;
        public float _magicBlockingDamageAbsorption;
        public float _bleedBlockingDamageAbsorption;

        [Header("STABILITY")]
        public int stability = 67;

        [Header("STAMINA COSTS")]
        public int baseStaminaCost;
        public float lightAttackStaminaMultiplier;
        public float heavyAttackStaminaMultiplier;

        [Header("ONE HANDED ITEM ACTIONS")]
        public ItemAction oh_hold_E_Action;
        public ItemAction oh_tap_E_Action;
        public ItemAction oh_hold_T_Action;
        public ItemAction oh_tap_T_Action;
        public ItemAction oh_hold_Z_Action;
        public ItemAction oh_tap_Z_Action;
        public ItemAction oh_hold_X_Action;
        public ItemAction oh_tap_X_Action;

        //NEW ACTIONS
        public ItemAction _oh_tap_Q_Action;
        public ItemAction _oh_hold_Q_Action;

        [Header("TWO HANDED ITEM ACTIONS")]
        public ItemAction th_hold_E_Action;
        public ItemAction th_tap_E_Action;
        public ItemAction th_hold_T_Action;
        public ItemAction th_tap_T_Action;
        public ItemAction th_hold_Z_Action;
        public ItemAction th_tap_Z_Action;
        public ItemAction th_hold_X_Action;
        public ItemAction th_tap_X_Action;

        [Header("SOUND FX")]
        public AudioClip[] weaponWhooshes;
        public AudioClip[] blockingNoises;
    }
}