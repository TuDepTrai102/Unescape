using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AICharacterStatsManager : CharacterStatsManager
    {
        AICharacterManager aiCharacter;

        public bool isBoss;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
            aiCharacterHealthBar = GetComponentInChildren<UIAICharacterHealthBar>();

            maxHealth = SetEnemyMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        protected override void Start()
        {
            base.Start();

            if (!isBoss)
            {
                aiCharacterHealthBar.SetMaxHealth(maxHealth);
            }
        }

        //need refactor...
        private float SetEnemyMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        //public void BreakGuard()
        //{
        //    aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(aiCharacter.aiCharacterAnimatorManager.animation_guard_break, true);
        //}

        public override void TakeDamageNoAnimation(
            int physicalDamage, 
            int fireDamage,
            int _lightningDamage,
            int _darkDamage,
            int _magicDamage,
            int _bleedDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage, _lightningDamage, _darkDamage, _magicDamage, _bleedDamage);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (aiCharacter.isDead)
                return;

            base.TakePoisonDamage(damage);

            if (!isBoss)
            {
                aiCharacterHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && aiCharacter.aiCharacterBossManager != null)
            {
                aiCharacter.aiCharacterBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            aiCharacter.isDead = true;
            aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(aiCharacter.aiCharacterAnimatorManager.animation_dead_01, true);
        }
    }
}