using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public bool enableDamageColliderOnStartUp = false;

        [Header("TEAM I.D")]
        public int teamIDNumber = 0;

        [Header("POISE")]
        public float poiseDamage;
        public float offensivePoiseBonus;

        [Header("DAMAGE")]
        public int physicalDamage;
        public int fireDamage;
        public int _magicDamage;
        public int _lightningDamage;
        public int _darkDamage;
        public int _bleedDamage;

        [Header("GUARD BREAK MODIFIERS")]
        public float guardBreakModifier = 1;

        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;
        protected string currentDamageAnimation;
        public bool _lifeSteal_enable;

        protected Vector3 contactPoint;
        protected float angleHitFrom;

        private List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enableDamageColliderOnStartUp;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            if (charactersDamagedDuringThisCalculation.Count > 0)
            {
                charactersDamagedDuringThisCalculation.Clear();
            }

            damageCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;

                CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();

                if (enemyManager != null)
                {
                    AICharacterManager aiCharacter = enemyManager as AICharacterManager;

                    if (charactersDamagedDuringThisCalculation.Contains(enemyManager))
                        return;

                    charactersDamagedDuringThisCalculation.Add(enemyManager);

                    if (enemyManager.characterStatsManager.teamIDNumber == teamIDNumber)
                        return;

                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);

                    if (hasBeenParried)
                        return;

                    if (shieldHasBeenHit)
                        return;

                    enemyManager.characterStatsManager.poiseResetTimer = enemyManager.characterStatsManager.totalPoiseResetTime;
                    enemyManager.characterStatsManager.totalPoiseDefence = enemyManager.characterStatsManager.totalPoiseDefence - poiseDamage;

                    contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    angleHitFrom = (Vector3.SignedAngle
                        (characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    DealDamage(enemyManager);
                    _CheckForWeaponDamageEffectType();

                    _lifeSteal_enable = true;

                    if (aiCharacter != null)
                    {
                        //IF THE TARGET IS A.I, THE A.I RECEIVES A NEW TARGET, THE PERSON DEALING DAMAGE TO IT
                        aiCharacter.currentTarget = characterManager;
                    }
                }
            }

            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation(enemyManager.characterCombatManager.weaponArt_Parried, true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager)
        {
            Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);

            if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;

                TakeBlockedDamageEffect takeBlockedDamage = Instantiate
                    (WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);
                takeBlockedDamage.physicalDamage = physicalDamage;
                takeBlockedDamage.fireDamage = fireDamage;
                takeBlockedDamage._lightningDamage = _lightningDamage;
                takeBlockedDamage._darkDamage = _darkDamage;
                takeBlockedDamage._magicDamage = _magicDamage;
                takeBlockedDamage._bleedDamage = _bleedDamage;
                takeBlockedDamage.poiseDamage = poiseDamage;
                takeBlockedDamage.staminaDamage = poiseDamage;

                enemyManager.characterEffectsManager.ProcessEffectInstantly(takeBlockedDamage);
            }
        }

        protected virtual void DealDamage(CharacterManager enemyManager)
        {
            float finalPhysicalDamage = physicalDamage;
            float finalFireDamage = fireDamage; 
            float _finalLightningDamage = _lightningDamage;
            float _finalBleedDamage = _bleedDamage;
            float _finalDarkDamage = _darkDamage;
            float _finalMagicDamage = _magicDamage;

            //GET ATTACK TYPE FROM ATTACKING CHARACTER

            //APPLY DAMAGE MULTIPLIERS
            if (characterManager.isUsingRightHand) //IF WE ARE USING RIGHT HAND, WE COMPARE THE RIGHT WEAPON MODIFIERS
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalPhysicalDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    finalFireDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    _finalLightningDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    _finalBleedDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    _finalDarkDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                    _finalMagicDamage *= characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalPhysicalDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    finalFireDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    _finalLightningDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    _finalBleedDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    _finalDarkDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                    _finalMagicDamage *= characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                }
            }
            else if (characterManager.isUsingLeftHand) //IF WE ARE USING LEFT HAND, WE COMPARE THE LEFT WEAPON MODIFIERS
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.light)
                {
                    finalPhysicalDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    finalFireDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    _finalLightningDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    _finalBleedDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    _finalDarkDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                    _finalMagicDamage *= characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.heavy)
                {
                    finalPhysicalDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    finalFireDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    _finalLightningDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    _finalBleedDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    _finalDarkDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                    _finalMagicDamage *= characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                }
            }

            TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            takeDamageEffect.physicalDamage = finalPhysicalDamage;
            takeDamageEffect.fireDamage = finalFireDamage;
            takeDamageEffect._lightningDamage = _finalLightningDamage;
            takeDamageEffect._bleedDamage = _finalBleedDamage;
            takeDamageEffect._darkDamage = _finalDarkDamage;
            takeDamageEffect._magicDamage = _finalMagicDamage;
            takeDamageEffect.poiseDamage = poiseDamage;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.angleHitFrom = angleHitFrom;
            enemyManager.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
        }

        private void _CheckForWeaponDamageEffectType()
        {
            //  EFFECT DAMAGE TYPE OF WEAPON (EX: BLEED EFFECT, CURSE EFFECT,...)

            //  BLEED TYPE
            if (characterManager.characterInventoryManager.GetComponentInChildren<_WeaponEffectDamageType>()._effectWeaponDamageType == _EffectWeaponDamageType._BleedWeaponType)
            {
                foreach (CharacterManager character in charactersDamagedDuringThisCalculation)
                {
                    if (character.characterStatsManager._isBleeded)
                        return;

                    _BleedBuildUpEffect _bleedBuildUp = Instantiate(WorldCharacterEffectsManager.instance._bleedBuildUpEffect);

                    foreach (var effect in character.characterEffectsManager.timedEffects)
                    {
                        if (effect.effectID == _bleedBuildUp.effectID)
                        {
                            return;
                        }
                    }

                    character.characterEffectsManager.timedEffects.Add(_bleedBuildUp);
                }
            }
        }
    }
}