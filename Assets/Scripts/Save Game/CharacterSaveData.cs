using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("SCENE WORLD")]
        //SCENE WOLRD WILL LOAD THE NEXT LOG IN...
        public int sceneIndex;
        public int sceneSaved;

        public Sprite characterAvatar;
        public string characterName;

        [Header("STATS")]
        public int characterLevel;
        public int characterHealthLevel;
        public int characterStaminaLevel;
        public int characterFocusLevel;
        public int characterStrengthLevel;
        public int characterIntelligenceLevel;
        public int characterFaithLevel;
        public int characterDexterityLevel;
        public int characterPoiseLevel;

        public int characterSoulsCount;

        [Header("EQUIPMENT")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;

        public WeaponItem[] weaponsInRightHandSlots;
        public WeaponItem[] weaponsInLeftHandSlots;

        public int currentHeadGearItemID;
        public int currentChestGearItemID;
        public int currentLegGearItemID;
        public int currentHandGearItemID;

        //SPELL
        public SpellItem _currentSpell;
        //CONSUMABLE
        public ConsumableItem _currentConsumableItem;
        //AMMO
        public RangedAmmoItem _currentAmmo;
        public int _currentAmmo_Arrow;

        [Header("INVENTORY SAVE")]
        public List<WeaponItem> weaponsInventorySlot;
        public List<ConsumableItem> consumablesInventorySlots;
        public List<RingItem> ringsInventorySlots;
        public List<HelmetEquipment> headEquipmentSlots;
        public List<BodyEquipment> bodyEquipmentSlots;
        public List<LegEquipment> legEquipmentSlots;
        public List<HandEquipment> handEquipmentSlots;
        //public RangedAmmoItem[]

        [Header("WORLD COORDINATES")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("SKILL TREE/TALENTS")]
        public SerializbleDictionary<int, bool> _talentPlayer;

        public int _points;

        //COMBAT ID
        public int _combat_AttackSpeed_point;
        public int _combat_ComboAttack_point;
        public int _combat_RunAttack_point;
        public int _combat_DashAttack_point;
        public int _combat_JumpAttack_point;
        public int _combat_CriticalAttack_point;
        public int _combat_HeavyAttack_point;
        public int _combat_HeavyComboAttack_point;
        public int _combat_ChargeAttack_point;
        public int _combat_ChargeAttackCombo_point;

        //PASSIVE ID
        public int _passive_playerBase_point;
        public int _passive_HealthRate_point;
        public int _passive_StaminaRate_point;
        public int _passive_FocusRate_point;
        public int _passive_MovementSpeed_point;
        public int _passive_DoubleJump_point;
        public int _passive_DeathOrAlive_point;
        public int _passive_GodSpeed_point;

        [Header("ITEMS LOOTED FROM WORLD")]
        public SerializbleDictionary<int, bool> itemInWorld; //THE INT IS THE WORLD ITEM ID, THE BOOL IS IF THE ITEM HAS BEEN LOOTED

        [Header("BOSS DEFEAT FROM WORLD")]
        public SerializbleDictionary<int, bool> bossInWorld;
        public SerializbleDictionary<int, bool> fogWallInWorld;

        [Header("BONFIRE FROM WORLD")]
        public SerializbleDictionary<int, bool> bonfireInWorld;

        public CharacterSaveData()
        {
            itemInWorld = new SerializbleDictionary<int, bool>();
            _talentPlayer = new SerializbleDictionary<int, bool>();
            bossInWorld = new SerializbleDictionary<int, bool>();
            fogWallInWorld = new SerializbleDictionary<int, bool>();
            bonfireInWorld = new SerializbleDictionary<int, bool>();
        }
    }
}