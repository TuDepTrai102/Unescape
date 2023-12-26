using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        public AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(aiCharacter.aiCharacterStatsManager.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.currentSoulCount);
                }
            }
        }

        public void AwardPointsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();

            if (playerStats != null)
            {
                playerStats.AddPoints(aiCharacter.aiCharacterStatsManager.pointsAwardedOnDeath);

                if (playerStats.player._playerSkills._talentTree != null)
                {
                    //playerStats.player._playerSkills._talentTree.points = playerStats.currentPointCount;
                    //playerStats.player._playerSkills._talentTree.UpdateTalentPointText();
                }
            }
        }

        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(aiCharacter.aiCharacterBossManager.particleFX ,bossFXTransform.transform);
        }

        public void PlayWeaponTrailFX()
        {
            aiCharacter.aiCharacterEffectsManager.PlayWeaponFX(false);
        }

        public override void OnAnimatorMove()
        {
            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);

            if (aiCharacter.isRotatingWithRootMotion)
            {
                character.transform.rotation *= character.animator.deltaRotation;
            }
        }
    }
}