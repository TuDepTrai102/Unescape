using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public ItemBaseAttackAction[] enemyAttacks;
        PursueTargetStateHumanoid pursueTargetState;

        [SerializeField] protected bool randomDestinationSet = false;
        [SerializeField] protected float verticalMovementValue = 0;
        [SerializeField] protected float horizontalMovementValue = 0;

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

        protected virtual void Awake()
        {
            attackState = GetComponent<AttackStateHumanoid>();
            pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }

        public override State Tick(AICharacterManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if(enemy.combatStyle == AICombatStyle.archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }

        private State ProcessSwordAndShieldCombatStyle(AICharacterManager enemy)
        {
            //IF THE A.I NOT GROUND OR PERFORM SOMETHING, STOP ALL MOVEMENT
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //IF THE A.I HAS GOTTEN TOO FAR FROM TARGET, RETURN THE A.I TO PURSUE TARGET STATE
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //RANDOMIZES THE WALKING PATTERN OF OUR A.I SO THE CIRCLE THE PLAYER
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemy);
            }

            if (enemy.allowAIToPerformParry)
            {
                if (enemy.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(enemy);
                    return this;
                }
            }

            if (enemy.allowAIToPerformBlock)
            {
                //ROLL FOR A BLOCK CHANCE
                RollForBlockChance(enemy);
            }

            if (enemy.allowAIToPerformDodge)
            {
                //ROLL FOR A DODGE CHANCE
                RollForDodgeChance(enemy);
            }

            if (enemy.allowAIToPerformParry)
            {
                //ROLL FOR A PARRY CHANCE
                RollForParryChance(enemy);
            }

            if (enemy._allowAIToPerformRushedIn)
            {
                //ROLL FOR A RUSHED IN ATTACK
                _RollForRushedInChance(enemy);
            }

            if (enemy.currentTarget.isAttacking)
            {
                if (willPerformParry && !hasPerformedParry)
                {
                    ParryCurrentTarget(enemy);
                    return this;
                }
            }

            if (willPerformBlock)
            {
                //BLOCK USING OFF HAND (LEFT HAND)
                BlockUsingOffHand(enemy);
            }

            if (willPerformDodge && enemy.currentTarget.isAttacking)
            {
                //IF ENEMY ATTACKING THIS A.I
                Dodge(enemy);
            }

            if (_willPerformRushAttack)
            {
                _RushedInTarget(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            if (enemy.distanceFromTarget <= enemy.stoppingDistance)
            {
                if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
                {
                    return attackState;
                }
                else
                {
                    ResetStateFlags();
                    GetNewAttack(enemy);
                }
            }

            HandleMovement(enemy);

            return this;
        }

        private State ProcessArcherCombatStyle(AICharacterManager enemy)
        {
            //IF THE A.I NOT GROUND OR PERFORM SOMETHING, STOP ALL MOVEMENT
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //IF THE A.I HAS GOTTEN TOO FAR FROM TARGET, RETURN THE A.I TO PURSUE TARGET STATE
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //RANDOMIZES THE WALKING PATTERN OF OUR A.I SO THE CIRCLE THE PLAYER
            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemy);
            }

            if (enemy.allowAIToPerformDodge)
            {
                //ROLL FOR A DODGE CHANCE
                RollForDodgeChance(enemy);
            }

            if (willPerformDodge && enemy.currentTarget.isAttacking)
            {
                //IF ENEMY ATTACKING THIS A.I
                Dodge(enemy);
            }

            HandleRotateTowardsTarget(enemy);

            if (!hasAmmoLoaded)
            {    
                DrawArrow(enemy); //DRAW ARROW             
                AimAtTargetBeforeFiring(enemy); //AIM AT TARGET BEFORE FIRING
            }

            if (enemy.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

            if (enemy.isStationaryArcher)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
            }
            else
            {
                HandleMovement(enemy);
            }

            return this;
        }

        protected void HandleRotateTowardsTarget(AICharacterManager enemy)
        {
            Vector3 direction = enemy.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp
                (transform.rotation, targetRotation, enemy.rotationSpeed);
        }

        protected void DecideCirclingAction(AICharacterManager enemyManager)
        {
            WalkAroundTarget(enemyManager);
        }

        protected void WalkAroundTarget(AICharacterManager enemyManager)
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

            enemyManager.characterSoundFXManager.PlayRandomFootStep_Walk();
        }

        protected virtual void GetNewAttack(AICharacterManager enemy)
        {
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBaseAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
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

            enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
        }

        //A.I ROLLS
        private void RollForBlockChance(AICharacterManager enemy)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= enemy.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollForDodgeChance(AICharacterManager enemy)
        {
            int dodgeChance = Random.Range(0, 100);

            if (dodgeChance <= enemy.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        private void RollForParryChance(AICharacterManager enemy)
        {
            int parryChance = Random.Range(0, 100);

            if (parryChance <= enemy.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        private void _RollForRushedInChance(AICharacterManager enemy)
        {
            int rushedAttackChance = Random.Range(0, 100);

            if (rushedAttackChance <= enemy._rushLikelyHood)
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
        private void BlockUsingOffHand(AICharacterManager enemy)
        {
            if (!enemy.isBlocking)
            {
                if (enemy.allowAIToPerformBlock)
                {
                    enemy.isBlocking = true;
                    enemy.UpdateWhichHandCharacterIsUsing(false);
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                    enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }

        private void Dodge(AICharacterManager enemy)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler
                        (enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
                }

                if (enemy.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp
                        (enemy.transform.rotation, targetDodgeDirection, 1f);
                    enemy.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemy.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if (rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        enemy.transform.rotation = targetDodgeDirection;
                        enemy.aiCharacterAnimatorManager.PlayTargetAnimation(enemy.aiCharacterAnimatorManager.animation_rolling_light, true);
                    }
                }
            }
        }

        private void ParryCurrentTarget(AICharacterManager enemy)
        {
            if (enemy.currentTarget.canBeParried)
            {
                if (enemy.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    enemy.isParrying = true;
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                    enemy.aiCharacterAnimatorManager.PlayTargetAnimation(enemy.aiCharacterAnimatorManager.animation_parry, true);
                }
            }
        }

        private void _RushedInTarget(AICharacterManager enemy)
        {
            if (!_hasPerformedRushAttack)
            {
                if (_rushedInAttackTimer > 10f)
                {
                    _hasPerformedRushAttack = true;
                    enemy._isRushIn = true;
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                    enemy.aiCharacterAnimatorManager.PlayTargetAnimation(enemy.aiCharacterCombatManager.ai_animation_rushed_in_target, true);

                    _rushedInAttackTimer = 0;
                }
            }
        }

        private void DrawArrow(AICharacterManager enemy)
        {
            //WE MUST TWO HAND THE BOW TO FIRE AND LOAD IT
            if (!enemy.isTwoHandingWeapon)
            {
                enemy.isTwoHandingWeapon = true;
                enemy.characterWeaponSlotManager.LoadBothWeaponOnSlots();
            }
            else
            {
                hasAmmoLoaded = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_hold_E_Action.PerformAction(enemy);
            }
        }

        private void AimAtTargetBeforeFiring(AICharacterManager enemy)
        {
            float timeUntilAmmoIsShotAtTarget = Random.Range
                (enemy.minimumTimeToAimAtTarget, enemy.maximumTimeToAimAtTarget);
            enemy.currentRecoveryTime = timeUntilAmmoIsShotAtTarget;
        }

        private void CheckForRiposte(AICharacterManager enemy)
        {
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }

            if (enemy.distanceFromTarget >= 1.0)
            {
                HandleRotateTowardsTarget(enemy);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);

            }
            else
            {
                enemy.isBlocking = false;

                if (!enemy.isInteracting && 
                    !enemy.currentTarget.isBeingRiposted && 
                    !enemy.currentTarget.isBeingBackstabbed)
                {
                    enemy.aiRigidbody.velocity = Vector3.zero;
                    enemy.animator.SetFloat("Vertical", 0);
                    enemy.characterCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }

        private void HandleMovement(AICharacterManager enemy)
        {
            if (enemy.distanceFromTarget <= enemy.stoppingDistance)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }
    }
}