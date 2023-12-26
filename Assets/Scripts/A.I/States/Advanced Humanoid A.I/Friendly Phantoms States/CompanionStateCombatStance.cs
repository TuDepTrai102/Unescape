using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class CompanionStateCombatStance : State
    {
        public ItemBaseAttackAction[] enemyAttacks;

        CompanionStateFollowHost followHostState;
        CompanionStatePursueTarget pursueTargetState;
        CompanionStateAttackTarget attackState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        [Header("STATES FLAGS")]
        public bool willPerformBlock = false;
        public bool willPerformDodge = false;
        public bool willPerformParry = false;
        public bool _willPerformRushAttack = false;

        public bool hasPerformedDodge = false;
        public bool hasPerformedParry = false;
        public bool hasRandomDodgeDirection = false;
        public bool hasAmmoLoaded = false;
        public bool _hasPerformedRushAttack = false;

        public float _rushedInAttackTimer;

        Quaternion targetDodgeDirection;

        private void Awake()
        {
            attackState = GetComponent<CompanionStateAttackTarget>();
            pursueTargetState = GetComponent<CompanionStatePursueTarget>();
            followHostState = GetComponent<CompanionStateFollowHost>();
        }

        public override State Tick(AICharacterManager aiCharacter)
        {
            //IF WE ARE TOO FAR AWAY FROM OUR COMPANION, WE RETURN TO THEM
            if (aiCharacter.distanceFromCompanion > aiCharacter.maxDistanceFromCompanion)
            {
                return followHostState;
            }

            if (aiCharacter.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(aiCharacter);
            }
            else if (aiCharacter.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(aiCharacter);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager aiCharacter)
        {
            //IF THE A.I NOT GROUND OR PERFORM SOMETHING, STOP ALL MOVEMENT
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //IF THE A.I HAS GOTTEN TOO FAR FROM TARGET, RETURN THE A.I TO PURSUE TARGET STATE
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //RANDOMIZES THE WALKING PATTERN OF OUR A.I SO THE CIRCLE THE PLAYER
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                if (aiCharacter.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(aiCharacter);
                    return this;
                }
            }

            if (aiCharacter.allowAIToPerformBlock)
            {
                //ROLL FOR A BLOCK CHANCE
                RollForBlockChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                //ROLL FOR A DODGE CHANCE
                RollForDodgeChance(aiCharacter);
            }

            if (aiCharacter.allowAIToPerformParry)
            {
                //ROLL FOR A PARRY CHANCE
                RollForParryChance(aiCharacter);
            }

            if (aiCharacter._allowAIToPerformRushedIn)
            {
                //ROLL FOR A RUSHED IN ATTACK
                _RollForRushedInChance(aiCharacter);
            }

            if (aiCharacter.currentTarget.isAttacking)
            {
                if (willPerformParry && !hasPerformedParry)
                {
                    ParryCurrentTarget(aiCharacter);
                    return this;
                }
            }

            if (willPerformBlock)
            {
                //BLOCK USING OFF HAND (LEFT HAND)
                BlockUsingOffHand(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
            {
                //IF ENEMY ATTACKING THIS A.I
                Dodge(aiCharacter);
            }

            if (_willPerformRushAttack)
            {
                _RushedInTarget(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (aiCharacter.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                ResetStateFlags();
                return attackState;
            }
            else
            {
                ResetStateFlags();
                GetNewAttack(aiCharacter);
            }

            HandleMovement(aiCharacter);

            return this;
        }

        private State ProcessArcherCombatStyle(AICharacterManager aiCharacter)
        {
            //IF THE A.I NOT GROUND OR PERFORM SOMETHING, STOP ALL MOVEMENT
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Vertical", 0);
                aiCharacter.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //IF THE A.I HAS GOTTEN TOO FAR FROM TARGET, RETURN THE A.I TO PURSUE TARGET STATE
            if (aiCharacter.distanceFromTarget > aiCharacter.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //RANDOMIZES THE WALKING PATTERN OF OUR A.I SO THE CIRCLE THE PLAYER
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(aiCharacter.aiCharacterAnimatorManager);
            }

            if (aiCharacter.allowAIToPerformDodge)
            {
                //ROLL FOR A DODGE CHANCE
                RollForDodgeChance(aiCharacter);
            }

            if (willPerformDodge && aiCharacter.currentTarget.isAttacking)
            {
                //IF ENEMY ATTACKING THIS A.I
                Dodge(aiCharacter);
            }

            HandleRotateTowardsTarget(aiCharacter);

            if (!hasAmmoLoaded)
            {
                DrawArrow(aiCharacter); //DRAW ARROW             
                AimAtTargetBeforeFiring(aiCharacter); //AIM AT TARGET BEFORE FIRING
            }

            if (aiCharacter.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

            if (aiCharacter.isStationaryArcher)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            }
            else
            {
                HandleMovement(aiCharacter);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(AICharacterManager aiCharacter)
        {
            Vector3 direction = aiCharacter.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRotation, aiCharacter.rotationSpeed);
        }

        protected void DecideCirclingAction(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            WalkAroundTarget(aiCharacterAnimatorManager);
        }

        protected void WalkAroundTarget(AICharacterAnimatorManager aiCharacterAnimatorManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBaseAttackAction enemyAttackAction = enemyAttacks[i];

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBaseAttackAction enemyAttackAction = enemyAttacks[i];

                if (aiCharacter.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    aiCharacter.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (aiCharacter.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        aiCharacter.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }

            aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
        }

        //A.I ROLLS
        private void RollForBlockChance(AICharacterManager aiCharacter)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= aiCharacter.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollForDodgeChance(AICharacterManager aiCharacter)
        {
            int dodgeChance = Random.Range(0, 100);

            if (dodgeChance <= aiCharacter.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        private void RollForParryChance(AICharacterManager aiCharacter)
        {
            int parryChance = Random.Range(0, 100);

            if (parryChance <= aiCharacter.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        private void _RollForRushedInChance(AICharacterManager aiCharacter)
        {
            int rushedAttackChance = Random.Range(0, 100);

            if (rushedAttackChance <= aiCharacter._rushLikelyHood)
            {
                _willPerformRushAttack = true;
            }
            else
            {
                _willPerformRushAttack = false;
            }
        }

        //CALLED WHEN EVER WE EXIT THIS STATE, SO WHEN WE RETURN ALL FLAGS ARE RESET AND CAN BE RE-ROLLED
        private void ResetStateFlags()
        {
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            hasAmmoLoaded = false;
            hasPerformedParry = false;
            _hasPerformedRushAttack = false;

            randomDestinationSet = false;

            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
            _willPerformRushAttack = false;
        }

        //A.I ACTIONS
        private void BlockUsingOffHand(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isBlocking == false)
            {
                if (aiCharacter.allowAIToPerformBlock)
                {
                    aiCharacter.isBlocking = true;
                    aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.leftWeapon;
                    aiCharacter.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }

        private void Dodge(AICharacterManager aiCharacter)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler
                        (aiCharacter.transform.eulerAngles.x, randomDodgeDirection, aiCharacter.transform.eulerAngles.z);
                }

                if (aiCharacter.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp
                        (aiCharacter.transform.rotation, targetDodgeDirection, 1f);
                    aiCharacter.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = aiCharacter.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        aiCharacter.transform.rotation = targetDodgeDirection;
                        aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(aiCharacter.aiCharacterAnimatorManager.animation_rolling_light, true);
                    }
                }
            }
        }

        private void _RushedInTarget(AICharacterManager aiCharacter)
        {
            if (!_hasPerformedRushAttack)
            {
                if (_rushedInAttackTimer > 10f)
                {
                    _hasPerformedRushAttack = true;
                    aiCharacter._isRushIn = true;
                    aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
                    aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(aiCharacter.aiCharacterCombatManager.ai_animation_rushed_in_target, true);

                    _rushedInAttackTimer = 0;
                }
            }
        }

        private void DrawArrow(AICharacterManager aiCharacter)
        {
            //WE MUST TWO HAND THE BOW TO FIRE AND LOAD IT
            if (!aiCharacter.isTwoHandingWeapon)
            {
                aiCharacter.isTwoHandingWeapon = true;
                aiCharacter.characterWeaponSlotManager.LoadBothWeaponOnSlots();
            }
            else
            {
                hasAmmoLoaded = true;
                aiCharacter.characterInventoryManager.currentItemBeingUsed = aiCharacter.characterInventoryManager.rightWeapon;
                aiCharacter.characterInventoryManager.rightWeapon.th_hold_E_Action.PerformAction(aiCharacter);
            }
        }

        private void AimAtTargetBeforeFiring(AICharacterManager aiCharacter)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range
                (aiCharacter.minimumTimeToAimAtTarget, aiCharacter.maximumTimeToAimAtTarget);
            aiCharacter.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        private void ParryCurrentTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.currentTarget.canBeParried)
            {
                if (aiCharacter.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    aiCharacter.isParrying = true;
                    aiCharacter.aiCharacterAnimatorManager.PlayTargetAnimation(aiCharacter.aiCharacterAnimatorManager.animation_parry, true);
                }
            }
        }

        private void CheckForRiposte(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isInteracting)
            {
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }

            if (aiCharacter.distanceFromTarget >= 1.0)
            {
                HandleRotateTowardsTarget(aiCharacter);
                aiCharacter.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

            }
            else
            {
                aiCharacter.isBlocking = false;

                if (!aiCharacter.isInteracting &&
                    !aiCharacter.currentTarget.isBeingRiposted &&
                    !aiCharacter.currentTarget.isBeingBackstabbed)
                {
                    aiCharacter.aiRigidbody.velocity = Vector3.zero;
                    aiCharacter.animator.SetFloat("Vertical", 0);
                    aiCharacter.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        private void HandleMovement(AICharacterManager aiCharacter)
        {
            if (aiCharacter.distanceFromTarget <= aiCharacter.stoppingDistance)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }
    }
}