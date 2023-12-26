using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        public Animator animator;
        public CharacterAnimatorManager characterAnimatorManager;
        public CharacterWeaponSlotManager characterWeaponSlotManager;
        public CharacterStatsManager characterStatsManager;
        public CharacterInventoryManager characterInventoryManager;
        public CharacterEffectsManager characterEffectsManager;
        public CharacterSoundFXManager characterSoundFXManager;
        public CharacterCombatManager characterCombatManager;

        [Header("LOCK ON TRANSFORM")]
        public Transform lockOnTransform;

        [Header("RAY CASTS")]
        public Transform criticalAttackRaycastStartPoint;

        [Header("INTERACTIONS")]
        public bool isInteracting;

        [Header("STATUS")]
        public bool isDead;

        [Header("COMBAT FLAGS")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool canDoCombo;
        public bool canRoll = true;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isHoldingArrow;
        public bool isAiming;
        public bool isTwoHandingWeapon;
        public bool isPerformingFullyChargedAttack;
        public bool isAttacking;
        public bool isBeingBackstabbed;
        public bool isBeingRiposted;
        public bool isPerformingBackstab;
        public bool isPerformingRiposte;
        public bool isJumping;
        public bool isBusy;
        public bool _isRushIn;
        public bool _isChargingAttack;

        [Header("MOVEMENT FLAGS")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isGrounded;

        [Header("SPELL FLAGS")]
        public bool isFiringSpell;

        //DAMAGE WILL BE INFLICTED DURING AN ANIMATION EVENT
        //USED IN BACKSTAB OR RIPOSTE ANIMATIONS
        public int pendingCriticalDamage;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            characterAnimatorManager.CheckHandIKWeight(
                characterWeaponSlotManager.rightHandIKTarget, 
                characterWeaponSlotManager.leftHandIKTarget,
                isTwoHandingWeapon);
        }

        protected virtual void Update()
        {
            characterEffectsManager.ProcessAllTimedEffects();
        }

        public virtual void UpdateWhichHandCharacterIsUsing(bool usingRightHand)
        {
            if (usingRightHand)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingLeftHand = true;
                isUsingRightHand = false;
            }
        }
    }
}