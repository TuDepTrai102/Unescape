using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class AICharacterManager : CharacterManager
    {
        public AICharacterBossManager aiCharacterBossManager;
        public AICharacterLocomotionManager aiCharacterLocomotionManager;
        public AICharacterAnimatorManager aiCharacterAnimatorManager;
        public AICharacterStatsManager aiCharacterStatsManager;
        public AICharacterEffectsManager aiCharacterEffectsManager;
        public AICharacterCombatManager aiCharacterCombatManager;

        public State currentState;
        public CharacterManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody aiRigidbody;
        public WorldEventManager _worldEventManager;
        public SpawnItemWhenDied spawnItem;

        public bool isPerformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("A.I SETTINGS")]
        public float detectionRadius = 20;
        //THE HIGHER, LOWER RESPECTIVELY THESE ANGLES ARE, THE GREATER DETECTION FIELD OF VIEW (BASICLLY LIKE EYE SIGHT)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;
        public float stoppingDistance = 1.2f; //HOW CLOSE WE GET TO OUR TARGET BEFORE STOPPING INFRONT OF THEM, OR HAULTING FORWARD MOVEMENT

        //THESE SETTINGS ONLY EFFECT A.I WITH THE HUMANOID STATES
        [Header("ADVANCED A.I SETTINGS")]
        public bool allowAIToPerformBlock;
        public int blockLikelyHood = 50; //PERCENT DO THIS ACTION (EXAMPLE, IF BLOCK LIKE LY HOOD = 50, WE HAVE 50% PERCENT BLOCK ATTACK ENEMY)
        public bool allowAIToPerformDodge;
        public int dodgeLikelyHood = 50;
        public bool allowAIToPerformParry;
        public int parryLikelyHood = 50;
        public bool _allowAIToPerformRushedIn;
        public int _rushLikelyHood = 50;

        [Header("A.I COMBAT SETTINGS")]
        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;
        public AICombatStyle combatStyle;

        [Header("A.I ARCHERY SETTINGS")]
        public bool isStationaryArcher;
        public float minimumTimeToAimAtTarget = 3;
        public float maximumTimeToAimAtTarget = 6;

        [Header("A.I COMPANION SETTINGS")]
        public float maxDistanceFromCompanion; //MAX DISTANCE WE CAN GO FROM OUR COMPANION
        public float minimumDistanceFromCompanion; //MINIMUON DISTANCE WE HAVE TO BE FROM OUR COMPANION
        public float returnDistanceFromCompanion = 2; //HOW CLOSE WE GET TO OUR COMPANION WHEN WE RETURN TO THEM
        public float distanceFromCompanion;
        public CharacterManager companion;

        [Header("A.I TARGET INFORMATIONS")]
        public Vector3 targetsDirection;
        public float distanceFromTarget;
        public float viewableAngle;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterBossManager = GetComponent<AICharacterBossManager>();
            aiCharacterAnimatorManager = GetComponent<AICharacterAnimatorManager>();
            aiCharacterStatsManager = GetComponent<AICharacterStatsManager>();
            aiCharacterEffectsManager = GetComponent<AICharacterEffectsManager>();       
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiRigidbody = GetComponent<Rigidbody>();
            spawnItem = GetComponent<SpawnItemWhenDied>();
            _worldEventManager = FindObjectOfType<WorldEventManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            aiRigidbody.isKinematic = false;
        }

        protected override void Update()
        {
            base.Update();

            HandleRecoveryTime();
            HandleStateMachine();

            if (isDead && aiCharacterStatsManager.isBoss)
            {
                _worldEventManager.BossHasBeenDefeated();
            }

            if (isDead)
            {
                StartCoroutine(SpawnItem());
            }

            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            animator.SetBool("isDead", isDead);
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isBlocking", isBlocking);

            if (currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetsDirection, transform.forward);

                if (Vector3.Dot(targetsDirection, -transform.right) < 0)
                {
                    viewableAngle = -viewableAngle;
                }
            }

            if (companion != null)
            {
                distanceFromCompanion = Vector3.Distance(companion.transform.position, transform.position);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (isDead)
                return;

            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime < 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        private IEnumerator SpawnItem()
        {
            yield return new WaitForSeconds(5);

            spawnItem.SpawnRandomItem(transform.position);
            Destroy(gameObject);
        }
    }
}