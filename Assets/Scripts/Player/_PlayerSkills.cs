using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NT
{
    public class _PlayerSkills : MonoBehaviour
    {
        PlayerManager player;

        [Header("SKILL TREE/TALENTS/SPELLS")]
        public TalentTree _talentTree;
        public _SpellsSelected _spellsSelected;

        [Header("Combat")]
        public Talent _combat_AttackSpeedSkill;
        public Talent _combat_ComboAttackSkill;
        public Talent _combat_RunAttackSkill;
        public Talent _combat_DashAttackSkill;
        public Talent _combat_JumpAttackSkill;
        public Talent _combat_BackstabOrRiposteSkill;
        public Talent _combat_HeavyAttackSkill;
        public Talent _combat_HeavyAttackComboSkill;
        public Talent _combat_ChargeAttackSkill;
        public Talent _combat_ChargeAttackComboSkill;

        [Header("Passive")]
        public Talent _passive_playerBasePassive;
        public Talent _passive_IncreaseHealthRate;
        public Talent _passive_IncreaseStaminaRate;
        public Talent _passive_IncreaseFocusPointRate;
        public Talent _passive_MovementSpeed;
        public Talent _passive_DeathDance;
        public Talent _passive_DeathOrAlive;
        public Talent _passive_GodSpeed;

        [Header("ACTIVATED SKILL (COMBAT)")]
        [Header("Attack Speed Skill")]
        //LIGHT ATTACK
        public float _light_atk_speed_original = 1.0f;
        public float _light_atk_speed_lv_1 = 1.05f;
        public float _light_atk_speed_lv_2 = 1.1f;
        public float _light_atk_speed_lv_3 = 1.15f;
        public float _light_atk_speed_lv_4 = 1.2f;
        public float _light_atk_speed_lv_5 = 1.25f;

        [Header("Heavy Attack Skill")]
        //HEAVY ATTACK
        public float _heavy_atk_speed_lv_1 = 1.05f;
        public float _heavy_atk_speed_lv_2 = 1.1f;
        public float _heavy_atk_speed_lv_3 = 1.15f;

        [Header("Charge Attack Skill")]
        //CHARGE ATTACK
        public float _charge_atk_speed_lv_1 = 1.05f;
        public float _charge_atk_speed_lv_2 = 1.1f;
        public float _charge_atk_speed_lv_3 = 1.15f;

        [Header("ACTIVATED SKILL (PASSIVE)")]
        [Header("Death Or Alive")]
        //DEATH OR ALIVE SKILL
        public bool _skill_DeathOrAlive_Activated = false;
        public GameObject _particle_DeathOrAlive_Activated;
        public float _deathOrAlive_Threshold_Activation = 0.25f;
        public float _health_Recovery_Amount = 0.55f;

        //DEATH OR ALIVE COOL DOWN
        public float _coolDown_DeathOrAlive = 300f;
        public float _reset_CoolDown_DeathOrAlive;

        [Header("Wish You God Speed")]
        //RUSH PHASE (GOD SPEED) SKILL
        public bool _skill_RushPhase_Activated = false;
        public GameObject _particle_RushPhase_Activated;
        public float _rushPhase_Threshold_Activation = 0.35f; //% HEALTH FOR ACTIVE THIS SKILL
        public float _boost_Duaration_Rush_Phase = 3f; //TIME VALIDITY
        public float _boost_Multiplier_Rush_Phase = 1.5f; //SPEED INCREASE
        public float _boost_Timer_Rush_Phase;
        public float _speed_Buff_Sprinting;
        public float _speed_Buff_Running;
        public float _speed_Running_Original;
        public float _speed_Sprinting_Original;

        //RUSH PHASE COOL DOWN
        public float _coolDown_RushPhase = 120f;
        public float _reset_CoolDown_RushPhase;

        [Header("Death Dance")]
        //DEATH DANCE
        public bool _skill_DeathDance_Activated = false;
        public GameObject _particle_DeathDance_Activated;
        public float _deathDance_Threshold_Activation = 0.15f; //% HEALTH FOR ACTIVE THIS SKILL
        public float _boost_Duration_Death_Dance = 20f;
        public float _boost_Damage_Multiplier_Death_Dance = 2.5f;
        public float _boost_physical_Life_Steal_Death_Dance = 1.5f;
        public float _boost_Timer_Death_Dance;

        //DEATH DANCE COOL DOWN
        public bool _deathDance_Is_CoolDown;
        public float _coolDown_DeathDance = 600f;
        public float _reset_CoolDown_DeathDance;

        //TESTING SKILL HERE
        public float _shadowStepDistance = 5f;
        public GameObject _particle_shadowStep_Activated;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {

        }

        public void _Skill_AttackSpeed_Light()
        {
            if (player.isAttacking && player.playerCombatManager.currentAttackType == AttackType.light)
            {
                if (player._playerSkills._combat_AttackSpeedSkill._combat_AttackSpeedUnlocked)
                {
                    if (player._playerSkills._combat_AttackSpeedSkill.currentCount == 1)
                    {
                        player.animator.speed = _light_atk_speed_lv_1;
                    }

                    if (player._playerSkills._combat_AttackSpeedSkill.currentCount == 2)
                    {
                        player.animator.speed = _light_atk_speed_lv_2;
                    }

                    if (player._playerSkills._combat_AttackSpeedSkill.currentCount == 3)
                    {
                        player.animator.speed = _light_atk_speed_lv_3;
                    }

                    if (player._playerSkills._combat_AttackSpeedSkill.currentCount == 4)
                    {
                        player.animator.speed = _light_atk_speed_lv_4;
                    }

                    if (player._playerSkills._combat_AttackSpeedSkill.currentCount == 5)
                    {
                        player.animator.speed = _light_atk_speed_lv_5;
                    }
                }
                else
                {
                    player.animator.speed = _light_atk_speed_original;
                }
            }
        }

        public void _Skill_AttackSpeed_Heavy()
        {
            if (player.isAttacking && player.playerCombatManager.currentAttackType == AttackType.heavy)
            {
                if (player._playerSkills._combat_HeavyAttackSkill._combat_HeavyAttackUnlocked)
                {
                    if (player._playerSkills._combat_HeavyAttackSkill.currentCount == 1)
                    {
                        player.animator.speed = _heavy_atk_speed_lv_1;
                    }

                    if (player._playerSkills._combat_HeavyAttackSkill.currentCount == 2)
                    {
                        player.animator.speed = _heavy_atk_speed_lv_2;
                    }

                    if (player._playerSkills._combat_HeavyAttackSkill.currentCount == 3)
                    {
                        player.animator.speed = _heavy_atk_speed_lv_3;
                    }
                }
            }
        }

        public void _Skill_AttackSpeed_Charge()
        {
            if (player.isAttacking && player.playerCombatManager.currentAttackType == AttackType.charge)
            {
                if (player._playerSkills._combat_ChargeAttackSkill._combat_ChargeAttackUnlocked)
                {
                    if (player._playerSkills._combat_ChargeAttackSkill.currentCount == 1)
                    {
                        player.animator.speed = _charge_atk_speed_lv_1;
                    }

                    if (player._playerSkills._combat_ChargeAttackSkill.currentCount == 2)
                    {
                        player.animator.speed = _charge_atk_speed_lv_2;
                    }

                    if (player._playerSkills._combat_ChargeAttackSkill.currentCount == 3)
                    {
                        player.animator.speed = _charge_atk_speed_lv_3;
                    }
                }
            }
        }

        public void _Skill_MovementSpeed()
        {
            if (player.isSprinting)
            {
                if (player._playerSkills._passive_MovementSpeed._passive_MovementSpeedUnlocked)
                {
                    if (player._playerSkills._passive_MovementSpeed.currentCount == 1)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.2f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 2)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.25f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 3)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.3f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 4)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.35f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 5)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.4f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 6)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.45f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 7)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.5f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 8)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.55f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 9)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.6f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 10)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.sprintingSpeed * 0.65f * Time.deltaTime);
                        player.playerStatsManager.DeductSprintingStamina(player.playerLocomotionManager.sprintStaminaCost);
                    }
                }
            }
            else
            {
                if (player._playerSkills._passive_MovementSpeed._passive_MovementSpeedUnlocked)
                {
                    if (player._playerSkills._passive_MovementSpeed.currentCount == 1)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.2f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 2)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.25f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 3)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.3f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 4)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.35f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 5)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.4f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 6)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.45f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 7)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.5f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 8)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.55f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 9)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.6f * Time.deltaTime);
                    }
                    else if (player._playerSkills._passive_MovementSpeed.currentCount == 10)
                    {
                        player.characterController.Move(player.playerLocomotionManager.moveDirection * player.playerLocomotionManager.runningSpeed * 0.65f * Time.deltaTime);
                    }
                }
            }
        }

        public void _Skill_DeathOrAlive()
        {
            if (_passive_DeathOrAlive._passive_DeathOrAliveUnlocked)
            {
                if (!_skill_DeathOrAlive_Activated)
                {
                    if (player.playerStatsManager.currentHealth <= player.playerStatsManager.maxHealth * _deathOrAlive_Threshold_Activation)
                    {
                        _reset_CoolDown_DeathOrAlive = _coolDown_DeathOrAlive;

                        _skill_DeathOrAlive_Activated = true;

                        //PARTICLE FOR MAKE SURE SKILL ACTIVATED
                        GameObject _DeathOrAlive_Activated = Instantiate(_particle_DeathOrAlive_Activated, player.transform);

                        player.playerStatsManager.currentHealth += player.playerStatsManager.maxHealth * _health_Recovery_Amount;

                        if (player.playerStatsManager.currentHealth > player.playerStatsManager.maxHealth)
                        {
                            player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
                        }

                        player.playerStatsManager.healthBar.SetCurrentHealth(player.playerStatsManager.currentHealth);

                        Destroy(_DeathOrAlive_Activated, 2f);
                    }
                }
            }

            if (_skill_DeathOrAlive_Activated)
            {
                _coolDown_DeathOrAlive -= 1 * Time.deltaTime;

                if (_coolDown_DeathOrAlive <= 0)
                {
                    _skill_DeathOrAlive_Activated = false;
                    _coolDown_DeathOrAlive = _reset_CoolDown_DeathOrAlive;
                }
            }
        }

        public void _Skill_RushPhase()
        {
            if (_passive_GodSpeed._passive_GodSpeedUnlocked)
            {
                if (!_skill_RushPhase_Activated)
                {
                    if (player.playerStatsManager.currentHealth <= player.playerStatsManager.maxHealth * _rushPhase_Threshold_Activation)
                    {
                        _skill_RushPhase_Activated = true;

                        //PARTICLE EFFECT
                        GameObject _RushPhase_Activated = Instantiate(_particle_RushPhase_Activated, player.transform);

                        _reset_CoolDown_RushPhase = _coolDown_RushPhase;
                        _boost_Timer_Rush_Phase = _boost_Duaration_Rush_Phase;

                        _speed_Buff_Sprinting = player.playerLocomotionManager.sprintingSpeed;
                        _speed_Buff_Running = player.playerLocomotionManager.runningSpeed;

                        _speed_Sprinting_Original = player.playerLocomotionManager.sprintingSpeed;
                        _speed_Running_Original = player.playerLocomotionManager.runningSpeed;

                        _speed_Buff_Sprinting *= _boost_Multiplier_Rush_Phase;
                        _speed_Buff_Running *= _boost_Multiplier_Rush_Phase;

                        player.playerLocomotionManager.sprintingSpeed = _speed_Buff_Sprinting;
                        player.playerLocomotionManager.runningSpeed = _speed_Buff_Running;

                        Destroy(_RushPhase_Activated, _boost_Duaration_Rush_Phase);
                    }
                }
                else
                {
                    _boost_Timer_Rush_Phase -= Time.deltaTime;

                    if (_boost_Timer_Rush_Phase <= 0f)
                    {
                        _boost_Timer_Rush_Phase = 0f;

                        player.playerLocomotionManager.sprintingSpeed = _speed_Sprinting_Original;
                        player.playerLocomotionManager.runningSpeed = _speed_Running_Original;
                    }
                }
            }

            if (_skill_RushPhase_Activated)
            {
                _coolDown_RushPhase -= 1 * Time.deltaTime;

                if (_coolDown_RushPhase <= 0)
                {
                    _skill_RushPhase_Activated = false;
                    _coolDown_RushPhase = _reset_CoolDown_RushPhase;
                }
            }
        }

        public void _Skill_DeathDance()
        {
            if (_passive_DeathDance._passive_DeathDanceUnlocked)
            {
                if (!_skill_DeathDance_Activated && !_deathDance_Is_CoolDown)
                {
                    if (player.playerStatsManager.currentHealth <= player.playerStatsManager.maxHealth * _deathDance_Threshold_Activation)
                    {
                        _skill_DeathDance_Activated = true;

                        GameObject _DeathDance_Ativated = Instantiate(_particle_DeathDance_Activated, player.transform);

                        _boost_Timer_Death_Dance = _boost_Duration_Death_Dance;
                        _reset_CoolDown_DeathDance = _coolDown_DeathDance;

                        Destroy(_DeathDance_Ativated, _boost_Duration_Death_Dance);
                    }
                }

                if (_skill_DeathDance_Activated)
                {
                    _boost_Timer_Death_Dance -= Time.deltaTime;

                    if (_boost_Timer_Death_Dance <= 0)
                    {
                        _boost_Timer_Death_Dance = 0;
                        _deathDance_Is_CoolDown = true;

                        if (player.isUsingLeftHand)
                        {
                            player.playerWeaponSlotManager.leftHandDamageCollider.physicalDamage =
                                player.playerInventoryManager.leftWeapon.physicalDamage;
                            player.playerWeaponSlotManager.leftHandDamageCollider.fireDamage =
                                player.playerInventoryManager.leftWeapon.fireDamage;
                            player.playerWeaponSlotManager.leftHandDamageCollider._magicDamage =
                                player.playerInventoryManager.leftWeapon._magicDamage;
                            player.playerWeaponSlotManager.leftHandDamageCollider._lightningDamage =
                                player.playerInventoryManager.leftWeapon._lightningDamage;
                            player.playerWeaponSlotManager.leftHandDamageCollider._darkDamage =
                                player.playerInventoryManager.leftWeapon._darkDamage;
                            player.playerWeaponSlotManager.leftHandDamageCollider._bleedDamage =
                                player.playerInventoryManager.leftWeapon._bleedDamage;
                        }

                        if (player.isUsingRightHand)
                        {
                            player.playerWeaponSlotManager.rightHandDamageCollider.physicalDamage =
                                player.playerInventoryManager.rightWeapon.physicalDamage;
                            player.playerWeaponSlotManager.rightHandDamageCollider.fireDamage =
                                player.playerInventoryManager.rightWeapon.fireDamage;
                            player.playerWeaponSlotManager.rightHandDamageCollider._magicDamage =
                                player.playerInventoryManager.rightWeapon._magicDamage;
                            player.playerWeaponSlotManager.rightHandDamageCollider._lightningDamage =
                                player.playerInventoryManager.rightWeapon._lightningDamage;
                            player.playerWeaponSlotManager.rightHandDamageCollider._darkDamage =
                                player.playerInventoryManager.rightWeapon._darkDamage;
                            player.playerWeaponSlotManager.rightHandDamageCollider._bleedDamage =
                                player.playerInventoryManager.rightWeapon._bleedDamage;
                        }
                    }

                    if (_deathDance_Is_CoolDown)
                    {
                        _coolDown_DeathDance -= 1 * Time.deltaTime;

                        if (_coolDown_DeathDance <= 0)
                        {
                            _skill_DeathDance_Activated = false;
                            _deathDance_Is_CoolDown = false;
                            _coolDown_DeathDance = _reset_CoolDown_DeathDance;
                        }
                    }

                    if (_boost_Timer_Death_Dance == 0)
                        return;

                    if (player.isUsingLeftHand)
                    {
                        if (player.playerWeaponSlotManager.leftHandSlot.currentWeapon != null)
                        {
                            player.playerWeaponSlotManager.leftHandDamageCollider.physicalDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon.physicalDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.leftHandDamageCollider.fireDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon.fireDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.leftHandDamageCollider._magicDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon._magicDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.leftHandDamageCollider._lightningDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon._lightningDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.leftHandDamageCollider._darkDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon._darkDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.leftHandDamageCollider._bleedDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.leftWeapon._bleedDamage * _boost_Damage_Multiplier_Death_Dance);

                            if (player.playerWeaponSlotManager.leftHandDamageCollider._lifeSteal_enable &&
                                !_deathDance_Is_CoolDown)
                            {
                                player.playerStatsManager.currentHealth += _boost_physical_Life_Steal_Death_Dance;
                                player.playerStatsManager.healthBar.SetCurrentHealth(player.playerStatsManager.currentHealth);
                            }

                            player.playerWeaponSlotManager.leftHandDamageCollider._lifeSteal_enable = false;
                        }

                    }

                    if (player.isUsingRightHand)
                    {
                        if (player.playerWeaponSlotManager.rightHandSlot.currentWeapon != null)
                        {
                            player.playerWeaponSlotManager.rightHandDamageCollider.physicalDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon.physicalDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider.physicalDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon.physicalDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider.fireDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon.fireDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider._magicDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon._magicDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider._lightningDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon._lightningDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider._darkDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon._darkDamage * _boost_Damage_Multiplier_Death_Dance);
                            player.playerWeaponSlotManager.rightHandDamageCollider._bleedDamage =
                                Mathf.RoundToInt(player.playerInventoryManager.rightWeapon._bleedDamage * _boost_Damage_Multiplier_Death_Dance);

                            if (player.playerWeaponSlotManager.rightHandDamageCollider._lifeSteal_enable &&
                                !_deathDance_Is_CoolDown)
                            {
                                player.playerStatsManager.currentHealth += _boost_physical_Life_Steal_Death_Dance;
                                player.playerStatsManager.healthBar.SetCurrentHealth(player.playerStatsManager.currentHealth);
                            }

                            player.playerWeaponSlotManager.rightHandDamageCollider._lifeSteal_enable = false;
                        }
                    }
                }
            }
        }

        public void TEST_ActivateShadowStep()
        {
            Vector3 shadowStepPosition = transform.position + transform.forward * _shadowStepDistance;

            GameObject _shadowStep_Activated = Instantiate(_particle_shadowStep_Activated, player.transform);

            player.characterController.Move(shadowStepPosition - transform.position);
        }
    }
}