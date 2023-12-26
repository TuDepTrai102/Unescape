using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class Talent : MonoBehaviour
    {
        public WorldSaveGameManager worldSaveGameManager;

        public Text countText;
        public int maxCount;
        public int currentCount;

        public Sprite _skillIcon;
        public int _skillID;
        public string _skillName;
        [SerializeField] private bool _hasBeedLearned;

        [SerializeField] private bool unlocked;

        [TextArea] public string _descriptionOfSkill;
        [TextArea] public string _noteDescriptionOfSkill;

        [Header("BOOLS")]
        //COMBAT BOOLS
        public bool _combat_AttackSpeedUnlocked;
        public bool _combat_ComboAttackUnlocked;
        public bool _combat_RunAttackUnlocked;
        public bool _combat_DashAttackUnlocked;
        public bool _combat_JumpAttackUnlocked;
        public bool _combat_CriticalAttackUnlocked;
        public bool _combat_HeavyAttackUnlocked;
        public bool _combat_HeavyComboAttackUnlocked;
        public bool _combat_ChargeAttackUnlocked;
        public bool _combat_ChargeAttackComboUnlocked;

        //PASSIVE BOOLS
        public bool _passive_playerBaseUnlocked;
        public bool _passive_HealthRateUnlocked;
        public bool _passive_StaminaRateUnlocked;
        public bool _passive_FocusRateUnlocked;
        public bool _passive_MovementSpeedUnlocked;
        public bool _passive_DeathDanceUnlocked;
        public bool _passive_DeathOrAliveUnlocked;
        public bool _passive_GodSpeedUnlocked;

        [Header("CHILD TALENTS")]
        [SerializeField] private Talent[] childTalent;

        [Header("COMBAT TALENTS")]
        [SerializeField] private Talent _combat_AttackSpeedTalent;
        [SerializeField] private Talent _combat_ComboAttackTalent;
        [SerializeField] private Talent _combat_RunAttackTalent;
        [SerializeField] private Talent _combat_DashAttackTalent;
        [SerializeField] private Talent _combat_JumpAttackTalent;
        [SerializeField] private Talent _combat_BackstabOrRiposteTalent;
        [SerializeField] private Talent _combat_HeavyAttackTalent;
        [SerializeField] private Talent _combat_HeavyAttackComboTalent;
        [SerializeField] private Talent _combat_ChargeAttackTalent;
        [SerializeField] private Talent _combat_ChargeAttackComboTalent;

        [Header("PASSIVE TALENTS")]
        [SerializeField] private Talent _passive_playerBaseTalent;
        [SerializeField] private Talent _passive_HealthRateTalent;
        [SerializeField] private Talent _passive_StaminaRateTalent;
        [SerializeField] private Talent _passive_FocusRateTalent;
        [SerializeField] private Talent _passive_MovementSpeedTalent;
        [SerializeField] private Talent _passive_DeathDanceTalent;
        [SerializeField] private Talent _passive_DeathOrAliveTalent;
        [SerializeField] private Talent _passive_GodSpeedTalent;

        [SerializeField] private Sprite arrowSpriteLocked;
        [SerializeField] private Sprite arrowSpriteUnlocked;
        [SerializeField] private Image[] arrowImage;
        private Image sprite;

        private void Awake()
        {
            worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();

            sprite = GetComponent<Image>();

            countText.text = $"{currentCount}/{maxCount}";

            if (unlocked)
            {
                Unlock();
            }
        }

        private void OnEnable()
        {
            _InstantiateTalents();
        }

        public void _InstantiateTalents()
        {
            if (!WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer.ContainsKey(_skillID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer.Add(_skillID, false);
            }

            _hasBeedLearned = WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer[_skillID];

            if (_hasBeedLearned)
            {
                Unlock();

                #region COMBAT UNLOCK DATA

                if (_combat_AttackSpeedTalent != null)
                {
                    _combat_AttackSpeedTalent.countText.text = $"{_combat_AttackSpeedTalent.currentCount}/{_combat_AttackSpeedTalent.maxCount}";
                    _combat_AttackSpeedTalent._AttackSpeedUnlock();
                }

                if (_combat_ComboAttackTalent != null)
                {
                    _combat_ComboAttackTalent.countText.text = $"{_combat_ComboAttackTalent.currentCount}/{_combat_ComboAttackTalent.maxCount}";
                    _combat_ComboAttackTalent._ComboUnlock();
                }

                if (_combat_RunAttackTalent != null)
                {
                    _combat_RunAttackTalent.countText.text = $"{_combat_RunAttackTalent.currentCount}/{_combat_RunAttackTalent.maxCount}";
                    _combat_RunAttackTalent._RunAttackUnlock();
                }

                if (_combat_DashAttackTalent != null)
                {
                    _combat_DashAttackTalent.countText.text = $"{_combat_DashAttackTalent.currentCount}/{_combat_DashAttackTalent.maxCount}";
                    _combat_DashAttackTalent._DashAttackUnlock();
                }

                if (_combat_JumpAttackTalent != null)
                {
                    _combat_JumpAttackTalent.countText.text = $"{_combat_JumpAttackTalent.currentCount}/{_combat_JumpAttackTalent.maxCount}";
                    _combat_JumpAttackTalent._JumpAttackUnlock();
                }

                if (_combat_BackstabOrRiposteTalent != null)
                {
                    _combat_BackstabOrRiposteTalent.countText.text = $"{_combat_BackstabOrRiposteTalent.currentCount}/{_combat_BackstabOrRiposteTalent.maxCount}";
                    _combat_BackstabOrRiposteTalent._CriticalUnlock();
                }

                if (_combat_HeavyAttackTalent != null)
                {
                    _combat_HeavyAttackTalent.countText.text = $"{_combat_HeavyAttackTalent.currentCount}/{_combat_HeavyAttackTalent.maxCount}";
                    _combat_HeavyAttackTalent._HeavyAttackUnlock();
                }

                if (_combat_HeavyAttackComboTalent != null)
                {
                    _combat_HeavyAttackComboTalent.countText.text = $"{_combat_HeavyAttackComboTalent.currentCount}/{_combat_HeavyAttackComboTalent.maxCount}";
                    _combat_HeavyAttackComboTalent._HeavyAttackComboUnlock();
                }

                if (_combat_ChargeAttackTalent != null)
                {
                    _combat_ChargeAttackTalent.countText.text = $"{_combat_ChargeAttackTalent.currentCount}/{_combat_ChargeAttackTalent.maxCount}";
                    _combat_ChargeAttackTalent._ChargeAttackUnlock();
                }

                if (_combat_ChargeAttackComboTalent != null)
                {
                    _combat_ChargeAttackComboTalent.countText.text = $"{_combat_ChargeAttackComboTalent.currentCount}/{_combat_ChargeAttackComboTalent.maxCount}";
                    _combat_ChargeAttackComboTalent._ChargeAttackComboUnlock();
                }

                #endregion

                #region PASSIVE UNLOCK DATA

                if (_passive_playerBaseTalent != null)
                {
                    _passive_playerBaseTalent.countText.text = $"{_passive_playerBaseTalent.currentCount}/{_passive_playerBaseTalent.maxCount}";
                    _passive_playerBaseTalent._PlayerBASEUnlock();
                }

                if (_passive_HealthRateTalent != null)
                {
                    _passive_HealthRateTalent.countText.text = $"{_passive_HealthRateTalent.currentCount}/{_passive_HealthRateTalent.maxCount}";
                    _passive_HealthRateTalent._HealthRatePassiveUnlock();
                }

                if (_passive_StaminaRateTalent != null)
                {
                    _passive_StaminaRateTalent.countText.text = $"{_passive_StaminaRateTalent.currentCount}/{_passive_StaminaRateTalent.maxCount}";
                    _passive_StaminaRateTalent._StaminaRatePassiveUnlock();
                }

                if (_passive_FocusRateTalent != null)
                {
                    _passive_FocusRateTalent.countText.text = $"{_passive_FocusRateTalent.currentCount}/{_passive_FocusRateTalent.maxCount}";
                    _passive_FocusRateTalent._FocusRatePassiveUnlock();
                }

                if (_passive_MovementSpeedTalent != null)
                {
                    _passive_MovementSpeedTalent.countText.text = $"{_passive_MovementSpeedTalent.currentCount}/{_passive_MovementSpeedTalent.maxCount}";
                    _passive_MovementSpeedTalent._MovementSpeedPassiveUnlock();
                }

                if (_passive_DeathDanceTalent != null)
                {
                    _passive_DeathDanceTalent.countText.text = $"{_passive_DeathDanceTalent.currentCount}/{_passive_DeathDanceTalent.maxCount}";
                    _passive_DeathDanceTalent._DoubleJumpPassiveUnlock();
                }

                if (_passive_DeathOrAliveTalent != null)
                {
                    _passive_DeathOrAliveTalent.countText.text = $"{_passive_DeathOrAliveTalent.currentCount}/{_passive_DeathOrAliveTalent.maxCount}";
                    _passive_DeathOrAliveTalent._DeathOrAlivePassiveUnlock();
                }

                if (_passive_GodSpeedTalent != null)
                {
                    _passive_GodSpeedTalent.countText.text = $"{_passive_GodSpeedTalent.currentCount}/{_passive_GodSpeedTalent.maxCount}";
                    _passive_GodSpeedTalent._GodSpeedPassiveUnlock();
                }

                #endregion
            }
        }

        public int MyCurrentCount
        {
            get
            {
                return currentCount;
            }

            set
            {
                currentCount = value;
            }
        }

        public bool Click()
        {
            if (currentCount < maxCount && unlocked)
            {
                currentCount++;
                countText.text = $"{currentCount}/{maxCount}";

                //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
                if (WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer.ContainsKey(_skillID))
                {
                    WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer.Remove(_skillID);
                }

                //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
                WorldSaveGameManager.instance.currentCharacterSaveData._talentPlayer.Add(_skillID, true);

                _hasBeedLearned = true;

                if (currentCount == maxCount)
                {
                    if (childTalent != null)
                    {
                        foreach (Talent talent in childTalent)
                        {
                            talent.Unlock();
                        }
                    }
                }

                #region COMBAT

                if (_combat_AttackSpeedTalent != null && _combat_AttackSpeedTalent.currentCount >= 1)
                {
                    _combat_AttackSpeedTalent._AttackSpeedUnlock();
                }

                if (_combat_ComboAttackTalent != null && _combat_ComboAttackTalent.currentCount == maxCount)
                {
                    _combat_ComboAttackTalent._ComboUnlock();
                }

                if (_combat_RunAttackTalent != null && _combat_RunAttackTalent.currentCount >= 1)
                {
                    _combat_RunAttackTalent._RunAttackUnlock();
                }

                if (_combat_DashAttackTalent != null && _combat_DashAttackTalent.currentCount == maxCount)
                {
                    _combat_DashAttackTalent._DashAttackUnlock();
                }

                if (_combat_JumpAttackTalent != null && _combat_JumpAttackTalent.currentCount >= 1)
                {
                    _combat_JumpAttackTalent._JumpAttackUnlock();
                }

                if (_combat_BackstabOrRiposteTalent != null && _combat_BackstabOrRiposteTalent.currentCount == maxCount)
                {
                    _combat_BackstabOrRiposteTalent._CriticalUnlock();
                }

                if (_combat_HeavyAttackTalent != null && _combat_HeavyAttackTalent.currentCount == maxCount)
                {
                    _combat_HeavyAttackTalent._HeavyAttackUnlock();
                }

                if (_combat_HeavyAttackComboTalent != null && _combat_HeavyAttackComboTalent.currentCount == maxCount)
                {
                    _combat_HeavyAttackComboTalent._HeavyAttackComboUnlock();
                }

                if (_combat_ChargeAttackTalent != null && _combat_ChargeAttackTalent.currentCount == maxCount)
                {
                    _combat_ChargeAttackTalent._ChargeAttackUnlock();
                }

                if (_combat_ChargeAttackComboTalent != null && _combat_ChargeAttackComboTalent.currentCount == maxCount)
                {
                    _combat_ChargeAttackComboTalent._ChargeAttackComboUnlock();
                }

                #endregion

                #region PASSIVE

                if (_passive_playerBaseTalent != null && _passive_playerBaseTalent.currentCount >= 1)
                {
                    _passive_playerBaseTalent._PlayerBASEUnlock();
                }

                if (_passive_HealthRateTalent != null && _passive_HealthRateTalent.currentCount >= 1)
                {
                    _passive_HealthRateTalent._HealthRatePassiveUnlock();
                }

                if (_passive_StaminaRateTalent != null && _passive_StaminaRateTalent.currentCount >= 1)
                {
                    _passive_StaminaRateTalent._StaminaRatePassiveUnlock();
                }

                if (_passive_FocusRateTalent != null && _passive_FocusRateTalent.currentCount >= 1)
                {
                    _passive_FocusRateTalent._FocusRatePassiveUnlock();
                }

                if (_passive_MovementSpeedTalent != null && _passive_MovementSpeedTalent.currentCount >= 1)
                {
                    _passive_MovementSpeedTalent._MovementSpeedPassiveUnlock();
                }

                if (_passive_DeathDanceTalent != null && _passive_DeathDanceTalent.currentCount >= 1)
                {
                    _passive_DeathDanceTalent._DoubleJumpPassiveUnlock();
                }

                if (_passive_DeathOrAliveTalent != null && _passive_DeathOrAliveTalent.currentCount >= 1)
                {
                    _passive_DeathOrAliveTalent._DeathOrAlivePassiveUnlock();
                }

                if (_passive_GodSpeedTalent != null && _passive_GodSpeedTalent.currentCount >= 1)
                {
                    _passive_GodSpeedTalent._GodSpeedPassiveUnlock();
                }

                #endregion

                return true;
            }

            return false;
        }

        public void Lock()
        {
            sprite.color = Color.grey;
            countText.color = Color.grey;

            if (arrowImage != null)
            {
                foreach (Image arrow in arrowImage)
                {
                    arrow.sprite = arrowSpriteLocked;
                }
            }

            if (countText != null)
            {
                countText.color = Color.grey;
            }
        }

        public void Unlock()
        {
            sprite.color = Color.white;
            countText.color = Color.white;

            if (arrowImage != null)
            {
                foreach (Image arrow in arrowImage)
                {
                    arrow.sprite = arrowSpriteUnlocked;
                }
            }

            if (countText != null)
            {
                countText.color = Color.white;
            }

            unlocked = true;
        }

        #region COMBAT TALENTS UNLOCK

        private void _AttackSpeedUnlock()
        {
            _combat_AttackSpeedUnlocked = true;
        }

        private void _ComboUnlock()
        {
            _combat_ComboAttackUnlocked = true;
        }

        private void _RunAttackUnlock()
        {
            _combat_RunAttackUnlocked = true;
        }

        private void _DashAttackUnlock()
        {
            _combat_DashAttackUnlocked = true;
        }

        private void _JumpAttackUnlock()
        {
            _combat_JumpAttackUnlocked = true;
        }

        private void _CriticalUnlock()
        {
            _combat_CriticalAttackUnlocked = true;
        }

        private void _HeavyAttackUnlock()
        {
            _combat_HeavyAttackUnlocked = true;
        }

        private void _HeavyAttackComboUnlock()
        {
            _combat_HeavyComboAttackUnlocked = true;
        }

        private void _ChargeAttackUnlock()
        {
            _combat_ChargeAttackUnlocked = true;
        }

        private void _ChargeAttackComboUnlock()
        {
            _combat_ChargeAttackComboUnlocked = true;
        }

        #endregion

        #region PASSIVE TALENTS UNLOCK

        private void _PlayerBASEUnlock()
        {
            _passive_playerBaseUnlocked = true;
        }

        private void _HealthRatePassiveUnlock()
        {
            _passive_HealthRateUnlocked = true;
        }

        private void _StaminaRatePassiveUnlock()
        {
            _passive_StaminaRateUnlocked = true;
        }

        private void _FocusRatePassiveUnlock()
        {
            _passive_FocusRateUnlocked = true;
        }

        private void _MovementSpeedPassiveUnlock()
        {
            _passive_MovementSpeedUnlocked = true;
        }

        private void _DoubleJumpPassiveUnlock()
        {
            _passive_DeathDanceUnlocked = true;
        }

        private void _DeathOrAlivePassiveUnlock()
        {
            _passive_DeathOrAliveUnlocked = true;
        }

        private void _GodSpeedPassiveUnlock()
        {
            _passive_GodSpeedUnlocked = true;
        }

        #endregion
    }
}