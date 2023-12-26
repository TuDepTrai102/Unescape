using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("COMBAT TRANSFORMS")]
        public Transform backStabReceiverTransform;
        public Transform riposteReceiverTransform;

        public LayerMask characterLayer;
        public float criticalAttackRange = 0.7f;

        [Header("LAST AMOUNT OF POISE DAMAGE TAKEN")]
        public int previousPoiseDamageTaken;

        [Header("ATTACK TYPE")]
        public AttackType currentAttackType;

        [Header("CRITICAL ANIMATIONS (BACKSTAB && RIPOSTE)")]
        //STAB ANIMATION
        public string animation_backstab;
        public string animation_riposte;

        //STABBED ANIMATION
        public string animation_backstab_stabbed;
        public string animation_riposte_stabbed;

        [Header("NORMAL ATTACK ANIMATIONS")]

        [Header("STRAIGHT SWORD")]
        [Header("One Handed Weapon")]
        public string oh_straight_sword_light_attack_01;
        public string oh_straight_sword_light_attack_02;
        public string oh_straight_sword_light_attack_03;

        public string oh_straight_sword_heavy_attack_01;
        public string oh_straight_sword_heavy_attack_02;
        public string oh_straight_sword_heavy_attack_03;

        public string oh_straight_sword_running_attack_01;
        public string oh_straight_sword_running_attack_02;
        public string oh_straight_sword_running_attack_03;

        public string oh_straight_sword_jumping_attack_01;
        public string oh_straight_sword_jumping_attack_02;
        public string oh_straight_sword_jumping_attack_03;

        public string oh_straight_sword_charge_attack_01;
        public string oh_straight_sword_charge_attack_02;
        public string oh_straight_sword_charge_attack_03;

        public string oh_straight_sword_dash_attack_01;

        //GREAT SWORD (ONE HAND)
        public string oh_great_sword_light_attack_combo_01_1;
        public string oh_great_sword_light_attack_combo_01_2;
        public string oh_great_sword_light_attack_combo_01_3;

        public string oh_great_sword_light_attack_combo_02_1;
        public string oh_great_sword_light_attack_combo_02_2;
        public string oh_great_sword_light_attack_combo_02_3;

        public string oh_great_sword_light_attack_combo_03_1;
        public string oh_great_sword_light_attack_combo_03_2;

        public string oh_great_sword_light_attack_combo_04_1;
        public string oh_great_sword_light_attack_combo_04_2;
        public string oh_great_sword_light_attack_combo_04_3;

        public string oh_great_sword_running_attack_01;

        public string oh_great_sword_charge_attack_01;
        public string oh_great_sword_charge_attack_02;
        public string oh_great_sword_charge_attack_03;

        public string oh_great_sword_dash_attack_01;

        [Header("Two Handed Weapon")]
        public string th_straight_sword_light_attack_01;
        public string th_straight_sword_light_attack_02;
        public string th_straight_sword_light_attack_03;

        public string th_straight_sword_heavy_attack_01;
        public string th_straight_sword_heavy_attack_02;
        public string th_straight_sword_heavy_attack_03;

        public string th_straight_sword_running_attack_01;
        public string th_straight_sword_running_attack_02;
        public string th_straight_sword_running_attack_03;

        public string th_straight_sword_jumping_attack_01;
        public string th_straight_sword_jumping_attack_02;
        public string th_straight_sword_jumping_attack_03;

        public string th_straight_sword_charge_attack_01;
        public string th_straight_sword_charge_attack_02;
        public string th_straight_sword_charge_attack_03;

        [Header("Duel Weapon")]
        public string dw_straight_sword_light_attack_01;
        public string dw_straight_sword_light_attack_02;
        public string dw_straight_sword_light_attack_03;

        public string dw_straight_sword_heavy_attack_01;
        public string dw_straight_sword_heavy_attack_02;
        public string dw_straight_sword_heavy_attack_03;

        public string dw_straight_sword_running_attack_01;
        public string dw_straight_sword_running_attack_02;
        public string dw_straight_sword_running_attack_03;

        public string dw_straight_sword_jumping_attack_01;
        public string dw_straight_sword_jumping_attack_02;
        public string dw_straight_sword_jumping_attack_03;

        public string dw_straight_sword_charge_attack_01;
        public string dw_straight_sword_charge_attack_02;
        public string dw_straight_sword_charge_attack_03;

        [Header("SPEAR")]
        [Header("One Handed Weapon")]
        public string oh_spear_light_attack_01;
        public string oh_spear_light_attack_02;
        public string oh_spear_light_attack_03;

        public string oh_spear_heavy_attack_01;
        public string oh_spear_heavy_attack_02;
        public string oh_spear_heavy_attack_03;

        public string oh_spear_running_attack_01;
        public string oh_spear_running_attack_02;
        public string oh_spear_running_attack_03;

        public string oh_spear_jumping_attack_01;
        public string oh_spear_jumping_attack_02;
        public string oh_spear_jumping_attack_03;

        public string oh_spear_charge_attack_01;
        public string oh_spear_charge_attack_02;
        public string oh_spear_charge_attack_03;

        public string oh_spear_dash_attack_01;

        [Header("Two Handed Weapon")]
        public string th_spear_light_attack_01;
        public string th_spear_light_attack_02;
        public string th_spear_light_attack_03;

        public string th_spear_heavy_attack_01;
        public string th_spear_heavy_attack_02;
        public string th_spear_heavy_attack_03;

        public string th_spear_running_attack_01;
        public string th_spear_running_attack_02;
        public string th_spear_running_attack_03;

        public string th_spear_jumping_attack_01;
        public string th_spear_jumping_attack_02;
        public string th_spear_jumping_attack_03;

        public string th_spear_charge_attack_01;
        public string th_spear_charge_attack_02;
        public string th_spear_charge_attack_03;

        [Header("Duel Weapon")]
        public string dw_spear_light_attack_01;
        public string dw_spear_light_attack_02;
        public string dw_spear_light_attack_03;

        public string dw_spear_heavy_attack_01;
        public string dw_spear_heavy_attack_02;
        public string dw_spear_heavy_attack_03;

        public string dw_spear_running_attack_01;
        public string dw_spear_running_attack_02;
        public string dw_spear_running_attack_03;
        public string dw_spear_running_attack_04;
        public string dw_spear_running_attack_05;

        public string dw_spear_jumping_attack_01;
        public string dw_spear_jumping_attack_02;
        public string dw_spear_jumping_attack_03;

        public string dw_spear_charge_attack_01;
        public string dw_spear_charge_attack_02;
        public string dw_spear_charge_attack_03;

        [Header("BOW ANIMATIONS")]
        public string th_bow_draw_01;
        public string th_bow_fire_01;

        [Header("CHARACTER ACTIONS")]
        public string weaponArt_Parry_Small_Shield;
        public string weaponArt_Parry_Normal_Shield;
        public string weaponArt_Parried;

        [Header("A.I COMBAT MODE ANIMATIONS")]
        public string boss_animation_second_phase;
        public string ai_animation_rushed_in_target;

        [Header("CURRENT ATTACK")]
        public string lastAttack;

        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetBlockingAbsorptionsFromBlockingWeapon()
        {
            if (character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption =
                    character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption =
                    character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating =
                    character.characterInventoryManager.rightWeapon.stability;
                character.characterStatsManager._blockingLightningDamageAbsorption =
                    character.characterInventoryManager.rightWeapon._lightningBlockingDamageAbsorption;
                character.characterStatsManager._blockingDarkDamageAbsorption =
                    character.characterInventoryManager.rightWeapon._darkBlockingDamageAbsorption;
                character.characterStatsManager._blockingMagicDamageAbsorption =
                    character.characterInventoryManager.rightWeapon._magicBlockingDamageAbsorption;
                character.characterStatsManager._blockingBleedDamageAbsorption =
                    character.characterInventoryManager.rightWeapon._bleedBlockingDamageAbsorption;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorption =
                    character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorption;
                character.characterStatsManager.blockingFireDamageAbsorption =
                    character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorption;
                character.characterStatsManager.blockingStabilityRating =
                    character.characterInventoryManager.leftWeapon.stability;
                character.characterStatsManager._blockingLightningDamageAbsorption =
                    character.characterInventoryManager.leftWeapon._lightningBlockingDamageAbsorption;
                character.characterStatsManager._blockingDarkDamageAbsorption =
                    character.characterInventoryManager.leftWeapon._darkBlockingDamageAbsorption;
                character.characterStatsManager._blockingMagicDamageAbsorption =
                    character.characterInventoryManager.leftWeapon._magicBlockingDamageAbsorption;
                character.characterStatsManager._blockingBleedDamageAbsorption =
                    character.characterInventoryManager.leftWeapon._bleedBlockingDamageAbsorption;
            }
        }

        public virtual void DrainStaminaBaseOnAttackType()
        {
            //IF YOU WANT AI TO LOSE STAMINA DURING ATTACKS, PLACE THE CODE ON THE BASE CLASS (HERE)
        }

        private void SuccesfullyCastSpell()
        {
            character.characterInventoryManager.currentSpell.SuccessfullyCashSpell(character);
        }

        IEnumerator ForceMoveCharacterToEnemyBackStabPosition(CharacterManager characterPerformingBackStab)
        {
            for (float timer = 0.05f; timer < 0.05f; timer = timer + 0.05f)
            {
                Quaternion backstabRotation = Quaternion.LookRotation(characterPerformingBackStab.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
                transform.parent = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform;
                transform.localPosition = characterPerformingBackStab.characterCombatManager.backStabReceiverTransform.localPosition;
                transform.parent = null;

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator ForceMoveCharacterToEnemyRipostePosition(CharacterManager characterPerformingRiposte)
        {
            for (float timer = 0.05f; timer < 0.05f; timer = timer + 0.05f)
            {
                Quaternion riposteRotation = Quaternion.LookRotation(-characterPerformingRiposte.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, riposteRotation, 1);
                transform.parent = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform;
                transform.localPosition = characterPerformingRiposte.characterCombatManager.riposteReceiverTransform.localPosition;
                transform.parent = null;

                yield return new WaitForSeconds(0.05f);
            }
        }

        public void GetBackStabbed(CharacterManager characterPerformingBackStab)
        {
            //PLAY SOUND FX
            character.isBeingBackstabbed = true;

            //FORCE LOCK POSITION
            StartCoroutine(ForceMoveCharacterToEnemyBackStabPosition(characterPerformingBackStab));

            character.characterAnimatorManager.PlayTargetAnimation(animation_backstab_stabbed, true);
        }

        public void GetRiposted(CharacterManager characterPerformingRiposte)
        {
            //PLAY SOUND FX
            character.isBeingRiposted = true;

            //FORCE LOCK POSITION
            StartCoroutine(ForceMoveCharacterToEnemyRipostePosition(characterPerformingRiposte));

            character.characterAnimatorManager.PlayTargetAnimation(animation_riposte_stabbed, true);
        }

        public void AttemptBackStabOrRiposte()
        {
            if (character.isInteracting)
                return;

            if (character.characterStatsManager.currentStamina <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast
                (character.criticalAttackRaycastStartPoint.transform.position, 
                character.transform.TransformDirection(Vector3.forward), 
                out hit, criticalAttackRange, characterLayer))
            {
                CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
                Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;
                float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward); 

                if (enemyCharacter.canBeRiposted)
                {
                    if (dotValue <= 1.2f && dotValue >= 0.6f)
                    {
                        //ATTEMPT RIPOSTE
                        AttemptRiposte(hit);
                        return;
                    }
                }

                if (dotValue >= -0.7f && dotValue <= -0.6f)
                {
                    //ATTEMPT BACKSTAB
                    AttemptBackStab(hit);
                }
            }
        }

        private void AttemptBackStab(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
                {
                    //WE MAKE IT SO THE ENEMY CANNOT BE DAMAGE WHISLT BEING CRITICALLY DAMAGE (BACKSTAB OR RIPOSTE)
                    EnableIsInvulnerable();
                    character.isPerformingBackstab = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation(animation_backstab, true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier *
                        (character.characterInventoryManager.rightWeapon.physicalDamage +
                        character.characterInventoryManager.rightWeapon.fireDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetBackStabbed(character);
                }
            }
        }

        private void AttemptRiposte(RaycastHit hit)
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

            if (enemyCharacter != null)
            {
                if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
                {
                    //WE MAKE IT SO THE ENEMY CANNOT BE DAMAGE WHISLT BEING CRITICALLY DAMAGE (BACKSTAB OR RIPOSTE)
                    EnableIsInvulnerable();
                    character.isPerformingRiposte = true;
                    character.characterAnimatorManager.EraseHandIKForWeapon();

                    character.characterAnimatorManager.PlayTargetAnimation(animation_riposte, true);

                    float criticalDamage = (character.characterInventoryManager.rightWeapon.criticalDamageMultiplier *
                        (character.characterInventoryManager.rightWeapon.physicalDamage +
                        character.characterInventoryManager.rightWeapon.fireDamage));

                    int roundedCriticalDamage = Mathf.RoundToInt(criticalDamage);
                    enemyCharacter.characterCombatManager.pendingCriticalDamage = roundedCriticalDamage;
                    enemyCharacter.characterCombatManager.GetRiposted(character);
                }
            }
        }

        private void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        public void ApplyPendingDamage()
        {
            character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage, 0, 0, 0, 0, 0);
        }

        public void EnableCanBeParried()
        {
            character.canBeParried = true;
        }

        public void DisableCanBeParried()
        {
            character.canBeParried = false;
        }

        public void EnableCanBeRiposted()
        {
            character.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            character.canBeRiposted = false;
        }
    }
}