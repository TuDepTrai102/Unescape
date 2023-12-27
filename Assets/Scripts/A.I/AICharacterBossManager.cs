using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AICharacterBossManager : MonoBehaviour
    {
        public AICharacterManager enemy;

        public string bossName;
        UIBossHealthBar bossHealthBar;
        BossCombatStanceState bossCombatStanceState;

        [Header("SECOND PHASE PARTICLES")]
        public GameObject particleFX;
        public GameObject _activated_second_phase_particle_skill_1;
        public Transform _activated_second_phase_particle_skill_1_transform;

        private void Awake()
        {
            enemy = GetComponent<AICharacterManager>();

            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemy.aiCharacterStatsManager.maxHealth);

            if (enemy.aiCharacterStatsManager.isBoss)
            {
                enemy.aiCharacterStatsManager.armorPoiseBonus = 750;
                enemy.aiCharacterLocomotionManager.yVelocity.y = enemy.aiCharacterLocomotionManager.groundYVelocity;
            }
        }

        public void UpdateBossHealthBar(float currentHealth, float maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);

            if (bossCombatStanceState != null)
            {
                if (currentHealth <= maxHealth / 4 && !bossCombatStanceState.hasPhaseShifted)
                {
                    ShiftToSecondPhase_BOSS_AI();
                }
            }
        }

        public void ShiftToSecondPhase_BOSS_AI()
        {
            enemy.aiCharacterStatsManager.currentHealth = enemy.aiCharacterStatsManager.maxHealth;
            bossCombatStanceState.hasPhaseShifted = true;
            enemy.animator.SetBool("isInvulnerable", true);
            enemy.animator.SetBool("isPhaseShifting", true);
            enemy.aiCharacterAnimatorManager.PlayTargetAnimation(enemy.aiCharacterCombatManager.boss_animation_second_phase, true);

            // EFFECTS
            GameObject skill_01_effect = Instantiate(_activated_second_phase_particle_skill_1, _activated_second_phase_particle_skill_1_transform);
        }
    }
}