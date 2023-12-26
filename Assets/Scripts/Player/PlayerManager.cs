using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NT
{
    public class PlayerManager : CharacterManager
    {
        [Header("CAMERA")]
        public CameraHandler cameraHandler;

        [Header("INPUTS")]
        public InputHandler inputHandler;

        [Header("UI")]
        public UIManager uiManager;

        [Header("PLAYER")]
        public PlayerStatsManager playerStatsManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerEquipmentManager playerEquipmentManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerLocomotionManager playerLocomotionManager;
        public _PlayerSkills _playerSkills;

        [Header("INTERACTABLES")]
        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        public _BonfireTeleportUI bonfireTeleportUI;

        //WORLD INTERACTIONS
        WorldEventManager _worldEventManager;
        //WORLD SAVE GAME
        public WorldSaveGameManager _worldSaveGameManager;
        public _WorldSaveStartScreen _worldSaveStartScreen;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            bonfireTeleportUI = FindObjectOfType<_BonfireTeleportUI>();
            _worldEventManager = FindObjectOfType<WorldEventManager>();
            _worldSaveGameManager = FindObjectOfType<WorldSaveGameManager>();
            _worldSaveStartScreen = FindObjectOfType<_WorldSaveStartScreen>();

            inputHandler = GetComponent<InputHandler>();

            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            _playerSkills = GetComponent<_PlayerSkills>();

            WorldSaveGameManager.instance.player = this;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (isDead)
            {
                uiManager.repsawnWindow.SetActive(true);
                uiManager.hudWindow.SetActive(false);
            }

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            isPerformingFullyChargedAttack = animator.GetBool("isPerformingFullyChargedAttack");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);

            inputHandler.TickInput();
            playerLocomotionManager.HandleLShiftActions();

            playerLocomotionManager.HandleJumping();

            playerStatsManager.RegenerateStamina();
            playerStatsManager._RegenerateHealth();
            playerStatsManager._RegenerateFocusPoint();
            _playerSkills._Skill_AttackSpeed_Light();
            _playerSkills._Skill_AttackSpeed_Heavy();
            _playerSkills._Skill_AttackSpeed_Charge();
            _playerSkills._Skill_RushPhase();
            _playerSkills._Skill_MovementSpeed();
            _playerSkills._Skill_DeathOrAlive();
            _playerSkills._Skill_DeathDance();

            playerLocomotionManager.HandleGroundedMovement();
            playerLocomotionManager.HandleInAirMovement();
            playerLocomotionManager.HandleRotation();
            playerLocomotionManager.ApplyForwardJumpForceOverTime();

            //TESTING... (FLASH SKILL)
            //if (Input.GetKeyDown(KeyCode.V))
            //{
            //    _playerSkills.TEST_ActivateShadowStep();
            //}

            //UPDATE VALUE OF CONSUMABLE ITEM ~~
            if (playerInventoryManager.currentConsumable != null)
            {
                uiManager.quickSlotsUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumable);
            }

            if (uiManager.shopWindow.activeSelf ||
                uiManager.levelUpWindow.activeSelf ||
                uiManager.endingWindow.activeSelf)
                return;

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!inputHandler.inventoryFlag)
            {
                //PRESS ESC IF YOU WANT TO SEE CURSOR
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (bonfireTeleportUI.bonfireMenu_UI.activeSelf || 
                uiManager.shopWindow.activeSelf ||
                uiManager.endingWindow.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (isDead)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void LateUpdate()
        {
            inputHandler.arrowU_Input = false;
            inputHandler.arrowD_Input = false;
            inputHandler.arrowL_Input = false;
            inputHandler.arrowR_Input = false;
            inputHandler.f_Input = false;
            inputHandler.tab_Input = false;

            if (!isJumping)
            {
                inputHandler.space_Input = false;
            }

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }
        }

        #region PLAYER INTERACTIONS

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward,
                out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.f_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableGameObject != null && inputHandler.f_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandHereWhenOpeningChest)
        {
            playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = playerStandHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation(playerAnimatorManager.animation_open_chest, true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            characterController.GetComponent<CharacterController>().Move(Vector3.zero);

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation(playerAnimatorManager.animation_pass_through, true);
        }

        #endregion

        public void SaveCharacterDataToCurrentSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            if (currentCharacterSaveData == null)
                return;

            //SCENE SAVE
            currentCharacterSaveData.sceneSaved = currentCharacterSaveData.sceneIndex;

            #region STATS & POSITION SAVE

            currentCharacterSaveData.characterAvatar = playerStatsManager._characterAvatar.sprite;
            currentCharacterSaveData.characterName = playerStatsManager.characterName;
            currentCharacterSaveData.characterLevel = playerStatsManager.playerLevel;
            currentCharacterSaveData.characterHealthLevel = playerStatsManager.healthLevel;
            currentCharacterSaveData.characterStaminaLevel = playerStatsManager.staminaLevel;
            currentCharacterSaveData.characterFocusLevel = playerStatsManager.focusLevel;
            currentCharacterSaveData.characterPoiseLevel = playerStatsManager.poiseLevel;
            currentCharacterSaveData.characterDexterityLevel = playerStatsManager.dexterityLevel;
            currentCharacterSaveData.characterIntelligenceLevel = playerStatsManager.intelligenceLevel;
            currentCharacterSaveData.characterFaithLevel = playerStatsManager.faithLevel;
            currentCharacterSaveData.characterStrengthLevel = playerStatsManager.strengthLevel;

            currentCharacterSaveData.characterSoulsCount = playerStatsManager.currentSoulCount;

            currentCharacterSaveData.xPosition = transform.position.x;
            currentCharacterSaveData.yPosition = transform.position.y;
            currentCharacterSaveData.zPosition = transform.position.z;

            #endregion

            #region SKILLS
            //SKILL POINTS COUNT
            currentCharacterSaveData._points = _playerSkills._talentTree.points;

            //COMBAT SKILL
            currentCharacterSaveData._combat_AttackSpeed_point = _playerSkills._combat_AttackSpeedSkill.currentCount;
            currentCharacterSaveData._combat_ComboAttack_point = _playerSkills._combat_ComboAttackSkill.currentCount;
            currentCharacterSaveData._combat_RunAttack_point = _playerSkills._combat_RunAttackSkill.currentCount;
            currentCharacterSaveData._combat_DashAttack_point = _playerSkills._combat_DashAttackSkill.currentCount;
            currentCharacterSaveData._combat_JumpAttack_point = _playerSkills._combat_JumpAttackSkill.currentCount;
            currentCharacterSaveData._combat_CriticalAttack_point = _playerSkills._combat_BackstabOrRiposteSkill.currentCount;
            currentCharacterSaveData._combat_HeavyAttack_point = _playerSkills._combat_HeavyAttackSkill.currentCount;
            currentCharacterSaveData._combat_HeavyComboAttack_point = _playerSkills._combat_HeavyAttackComboSkill.currentCount;
            currentCharacterSaveData._combat_ChargeAttack_point = _playerSkills._combat_ChargeAttackSkill.currentCount;
            currentCharacterSaveData._combat_ChargeAttackCombo_point = _playerSkills._combat_ChargeAttackComboSkill.currentCount;

            //PASSIVE SKILL
            currentCharacterSaveData._passive_playerBase_point = _playerSkills._passive_playerBasePassive.currentCount;
            currentCharacterSaveData._passive_HealthRate_point = _playerSkills._passive_IncreaseHealthRate.currentCount;
            currentCharacterSaveData._passive_StaminaRate_point = _playerSkills._passive_IncreaseStaminaRate.currentCount;
            currentCharacterSaveData._passive_FocusRate_point = _playerSkills._passive_IncreaseFocusPointRate.currentCount;
            currentCharacterSaveData._passive_MovementSpeed_point = _playerSkills._passive_MovementSpeed.currentCount;
            currentCharacterSaveData._passive_DeathOrAlive_point = _playerSkills._passive_DeathOrAlive.currentCount;
            currentCharacterSaveData._passive_DoubleJump_point = _playerSkills._passive_DeathDance.currentCount;
            currentCharacterSaveData._passive_GodSpeed_point = _playerSkills._passive_GodSpeed.currentCount;

            #endregion

            //EQUIPMENT
            #region EQUIPMENT SAVE

            currentCharacterSaveData.currentRightHandWeaponID = playerInventoryManager.rightWeapon.itemID;
            currentCharacterSaveData.currentLeftHandWeaponID = playerInventoryManager.leftWeapon.itemID;

            currentCharacterSaveData.weaponsInRightHandSlots[0] = playerInventoryManager.weaponsInRightHandSlots[0];
            currentCharacterSaveData.weaponsInRightHandSlots[1] = playerInventoryManager.weaponsInRightHandSlots[1];
            currentCharacterSaveData.weaponsInLeftHandSlots[0] = playerInventoryManager.weaponsInLeftHandSlots[0];
            currentCharacterSaveData.weaponsInLeftHandSlots[1] = playerInventoryManager.weaponsInLeftHandSlots[1];

            if (playerInventoryManager.currentAmmo != null)
            {
                currentCharacterSaveData._currentAmmo_Arrow = playerInventoryManager.currentAmmo.itemID;
            }

            //SPELL
            if (playerInventoryManager.currentSpell != null)
            {
                currentCharacterSaveData._currentSpell = playerInventoryManager.currentSpell; 
            }

            //CONSUMABLE
            if (playerInventoryManager.currentConsumable != null)
            {
                currentCharacterSaveData._currentConsumableItem = playerInventoryManager.currentConsumable;
            }

            #endregion

            //INVENTORY
            #region INVENTORY SAVE

            #region WEAPON

            #region SLOT 0 - 9

            if (uiManager.weaponInventorySlots[0].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[0] = uiManager.weaponInventorySlots[0].item;
            }
            if (uiManager.weaponInventorySlots[1].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[1] = uiManager.weaponInventorySlots[1].item;
            }
            if (uiManager.weaponInventorySlots[2].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[2] = uiManager.weaponInventorySlots[2].item;
            }
            if (uiManager.weaponInventorySlots[3].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[3] = uiManager.weaponInventorySlots[3].item;
            }
            if (uiManager.weaponInventorySlots[4].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[4] = uiManager.weaponInventorySlots[4].item;
            }
            if (uiManager.weaponInventorySlots[5].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[5] = uiManager.weaponInventorySlots[5].item;
            }
            if (uiManager.weaponInventorySlots[6].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[6] = uiManager.weaponInventorySlots[6].item;
            }
            if (uiManager.weaponInventorySlots[7].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[7] = uiManager.weaponInventorySlots[7].item;
            }
            if (uiManager.weaponInventorySlots[8].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[8] = uiManager.weaponInventorySlots[8].item;
            }
            if (uiManager.weaponInventorySlots[9].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[9] = uiManager.weaponInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager.weaponInventorySlots[10].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[10] = uiManager.weaponInventorySlots[10].item;
            }
            if (uiManager.weaponInventorySlots[11].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[11] = uiManager.weaponInventorySlots[11].item;
            }
            if (uiManager.weaponInventorySlots[12].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[12] = uiManager.weaponInventorySlots[12].item;
            }
            if (uiManager.weaponInventorySlots[13].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[13] = uiManager.weaponInventorySlots[13].item;
            }
            if (uiManager.weaponInventorySlots[14].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[14] = uiManager.weaponInventorySlots[14].item;
            }
            if (uiManager.weaponInventorySlots[15].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[15] = uiManager.weaponInventorySlots[15].item;
            }
            if (uiManager.weaponInventorySlots[16].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[16] = uiManager.weaponInventorySlots[16].item;
            }
            if (uiManager.weaponInventorySlots[17].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[17] = uiManager.weaponInventorySlots[17].item;
            }
            if (uiManager.weaponInventorySlots[18].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[18] = uiManager.weaponInventorySlots[18].item;
            }
            if (uiManager.weaponInventorySlots[19].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[19] = uiManager.weaponInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager.weaponInventorySlots[20].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[20] = uiManager.weaponInventorySlots[20].item;
            }
            if (uiManager.weaponInventorySlots[21].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[21] = uiManager.weaponInventorySlots[21].item;
            }
            if (uiManager.weaponInventorySlots[22].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[22] = uiManager.weaponInventorySlots[22].item;
            }
            if (uiManager.weaponInventorySlots[23].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[23] = uiManager.weaponInventorySlots[23].item;
            }
            if (uiManager.weaponInventorySlots[24].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[24] = uiManager.weaponInventorySlots[24].item;
            }
            if (uiManager.weaponInventorySlots[25].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[25] = uiManager.weaponInventorySlots[25].item;
            }
            if (uiManager.weaponInventorySlots[26].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[26] = uiManager.weaponInventorySlots[26].item;
            }
            if (uiManager.weaponInventorySlots[27].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[27] = uiManager.weaponInventorySlots[27].item;
            }
            if (uiManager.weaponInventorySlots[28].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[28] = uiManager.weaponInventorySlots[28].item;
            }
            if (uiManager.weaponInventorySlots[29].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[29] = uiManager.weaponInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager.weaponInventorySlots[30].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[30] = uiManager.weaponInventorySlots[30].item;
            }
            if (uiManager.weaponInventorySlots[31].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[31] = uiManager.weaponInventorySlots[31].item;
            }
            if (uiManager.weaponInventorySlots[32].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[32] = uiManager.weaponInventorySlots[32].item;
            }
            if (uiManager.weaponInventorySlots[33].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[33] = uiManager.weaponInventorySlots[33].item;
            }
            if (uiManager.weaponInventorySlots[34].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[34] = uiManager.weaponInventorySlots[34].item;
            }
            if (uiManager.weaponInventorySlots[35].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[35] = uiManager.weaponInventorySlots[35].item;
            }
            if (uiManager.weaponInventorySlots[36].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[36] = uiManager.weaponInventorySlots[36].item;
            }
            if (uiManager.weaponInventorySlots[37].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[37] = uiManager.weaponInventorySlots[37].item;
            }
            if (uiManager.weaponInventorySlots[38].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[38] = uiManager.weaponInventorySlots[38].item;
            }
            if (uiManager.weaponInventorySlots[39].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[39] = uiManager.weaponInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager.weaponInventorySlots[40].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[40] = uiManager.weaponInventorySlots[40].item;
            }
            if (uiManager.weaponInventorySlots[41].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[41] = uiManager.weaponInventorySlots[41].item;
            }
            if (uiManager.weaponInventorySlots[42].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[42] = uiManager.weaponInventorySlots[42].item;
            }
            if (uiManager.weaponInventorySlots[43].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[43] = uiManager.weaponInventorySlots[43].item;
            }
            if (uiManager.weaponInventorySlots[44].item != null)
            {
                currentCharacterSaveData.weaponsInventorySlot[44] = uiManager.weaponInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region CONSUMABLE

            #region SLOT 0 - 9

            if (uiManager._consumableEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[0] = uiManager._consumableEquipmentInventorySlots[0].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[1] = uiManager._consumableEquipmentInventorySlots[1].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[2] = uiManager._consumableEquipmentInventorySlots[2].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[3] = uiManager._consumableEquipmentInventorySlots[3].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[4] = uiManager._consumableEquipmentInventorySlots[4].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[5] = uiManager._consumableEquipmentInventorySlots[5].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[6] = uiManager._consumableEquipmentInventorySlots[6].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[7] = uiManager._consumableEquipmentInventorySlots[7].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[8] = uiManager._consumableEquipmentInventorySlots[8].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[9] = uiManager._consumableEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager._consumableEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[10] = uiManager._consumableEquipmentInventorySlots[10].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[11] = uiManager._consumableEquipmentInventorySlots[11].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[12] = uiManager._consumableEquipmentInventorySlots[12].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[13] = uiManager._consumableEquipmentInventorySlots[13].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[14] = uiManager._consumableEquipmentInventorySlots[14].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[15] = uiManager._consumableEquipmentInventorySlots[15].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[16] = uiManager._consumableEquipmentInventorySlots[16].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[17] = uiManager._consumableEquipmentInventorySlots[17].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[18] = uiManager._consumableEquipmentInventorySlots[18].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[19] = uiManager._consumableEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager._consumableEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[20] = uiManager._consumableEquipmentInventorySlots[20].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[21] = uiManager._consumableEquipmentInventorySlots[21].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[22] = uiManager._consumableEquipmentInventorySlots[22].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[23] = uiManager._consumableEquipmentInventorySlots[23].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[24] = uiManager._consumableEquipmentInventorySlots[24].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[25] = uiManager._consumableEquipmentInventorySlots[25].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[26] = uiManager._consumableEquipmentInventorySlots[26].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[27] = uiManager._consumableEquipmentInventorySlots[27].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[28] = uiManager._consumableEquipmentInventorySlots[28].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[29] = uiManager._consumableEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager._consumableEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[30] = uiManager._consumableEquipmentInventorySlots[30].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[31] = uiManager._consumableEquipmentInventorySlots[31].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[32] = uiManager._consumableEquipmentInventorySlots[32].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[33] = uiManager._consumableEquipmentInventorySlots[33].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[34] = uiManager._consumableEquipmentInventorySlots[34].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[35] = uiManager._consumableEquipmentInventorySlots[35].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[36] = uiManager._consumableEquipmentInventorySlots[36].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[37] = uiManager._consumableEquipmentInventorySlots[37].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[38] = uiManager._consumableEquipmentInventorySlots[38].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[39] = uiManager._consumableEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager._consumableEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[40] = uiManager._consumableEquipmentInventorySlots[40].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[41] = uiManager._consumableEquipmentInventorySlots[41].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[42] = uiManager._consumableEquipmentInventorySlots[42].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[43] = uiManager._consumableEquipmentInventorySlots[43].item;
            }
            if (uiManager._consumableEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.consumablesInventorySlots[44] = uiManager._consumableEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region RING

            #region SLOT 0 - 9

            if (uiManager._ringEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[0] = uiManager._ringEquipmentInventorySlots[0].item;
            }
            if (uiManager._ringEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[1] = uiManager._ringEquipmentInventorySlots[1].item;
            }
            if (uiManager._ringEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[2] = uiManager._ringEquipmentInventorySlots[2].item;
            }
            if (uiManager._ringEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[3] = uiManager._ringEquipmentInventorySlots[3].item;
            }
            if (uiManager._ringEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[4] = uiManager._ringEquipmentInventorySlots[4].item;
            }
            if (uiManager._ringEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[5] = uiManager._ringEquipmentInventorySlots[5].item;
            }
            if (uiManager._ringEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[6] = uiManager._ringEquipmentInventorySlots[6].item;
            }
            if (uiManager._ringEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[7] = uiManager._ringEquipmentInventorySlots[7].item;
            }
            if (uiManager._ringEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[8] = uiManager._ringEquipmentInventorySlots[8].item;
            }
            if (uiManager._ringEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[9] = uiManager._ringEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager._ringEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[10] = uiManager._ringEquipmentInventorySlots[10].item;
            }
            if (uiManager._ringEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[11] = uiManager._ringEquipmentInventorySlots[11].item;
            }
            if (uiManager._ringEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[12] = uiManager._ringEquipmentInventorySlots[12].item;
            }
            if (uiManager._ringEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[13] = uiManager._ringEquipmentInventorySlots[13].item;
            }
            if (uiManager._ringEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[14] = uiManager._ringEquipmentInventorySlots[14].item;
            }
            if (uiManager._ringEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[15] = uiManager._ringEquipmentInventorySlots[15].item;
            }
            if (uiManager._ringEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[16] = uiManager._ringEquipmentInventorySlots[16].item;
            }
            if (uiManager._ringEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[17] = uiManager._ringEquipmentInventorySlots[17].item;
            }
            if (uiManager._ringEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[18] = uiManager._ringEquipmentInventorySlots[18].item;
            }
            if (uiManager._ringEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[19] = uiManager._ringEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager._ringEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[20] = uiManager._ringEquipmentInventorySlots[20].item;
            }
            if (uiManager._ringEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[21] = uiManager._ringEquipmentInventorySlots[21].item;
            }
            if (uiManager._ringEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[22] = uiManager._ringEquipmentInventorySlots[22].item;
            }
            if (uiManager._ringEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[23] = uiManager._ringEquipmentInventorySlots[23].item;
            }
            if (uiManager._ringEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[24] = uiManager._ringEquipmentInventorySlots[24].item;
            }
            if (uiManager._ringEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[25] = uiManager._ringEquipmentInventorySlots[25].item;
            }
            if (uiManager._ringEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[26] = uiManager._ringEquipmentInventorySlots[26].item;
            }
            if (uiManager._ringEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[27] = uiManager._ringEquipmentInventorySlots[27].item;
            }
            if (uiManager._ringEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[28] = uiManager._ringEquipmentInventorySlots[28].item;
            }
            if (uiManager._ringEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[29] = uiManager._ringEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager._ringEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[30] = uiManager._ringEquipmentInventorySlots[30].item;
            }
            if (uiManager._ringEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[31] = uiManager._ringEquipmentInventorySlots[31].item;
            }
            if (uiManager._ringEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[32] = uiManager._ringEquipmentInventorySlots[32].item;
            }
            if (uiManager._ringEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[33] = uiManager._ringEquipmentInventorySlots[33].item;
            }
            if (uiManager._ringEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[34] = uiManager._ringEquipmentInventorySlots[34].item;
            }
            if (uiManager._ringEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[35] = uiManager._ringEquipmentInventorySlots[35].item;
            }
            if (uiManager._ringEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[36] = uiManager._ringEquipmentInventorySlots[36].item;
            }
            if (uiManager._ringEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[37] = uiManager._ringEquipmentInventorySlots[37].item;
            }
            if (uiManager._ringEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[38] = uiManager._ringEquipmentInventorySlots[38].item;
            }
            if (uiManager._ringEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[39] = uiManager._ringEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager._ringEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[40] = uiManager._ringEquipmentInventorySlots[40].item;
            }
            if (uiManager._ringEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[41] = uiManager._ringEquipmentInventorySlots[41].item;
            }
            if (uiManager._ringEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[42] = uiManager._ringEquipmentInventorySlots[42].item;
            }
            if (uiManager._ringEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[43] = uiManager._ringEquipmentInventorySlots[43].item;
            }
            if (uiManager._ringEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.ringsInventorySlots[44] = uiManager._ringEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region HEAD

            #region SLOT 0 - 9

            if (uiManager.headEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[0] = uiManager.headEquipmentInventorySlots[0].item;
            }
            if (uiManager.headEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[1] = uiManager.headEquipmentInventorySlots[1].item;
            }
            if (uiManager.headEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[2] = uiManager.headEquipmentInventorySlots[2].item;
            }
            if (uiManager.headEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[3] = uiManager.headEquipmentInventorySlots[3].item;
            }
            if (uiManager.headEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[4] = uiManager.headEquipmentInventorySlots[4].item;
            }
            if (uiManager.headEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[5] = uiManager.headEquipmentInventorySlots[5].item;
            }
            if (uiManager.headEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[6] = uiManager.headEquipmentInventorySlots[6].item;
            }
            if (uiManager.headEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[7] = uiManager.headEquipmentInventorySlots[7].item;
            }
            if (uiManager.headEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[8] = uiManager.headEquipmentInventorySlots[8].item;
            }
            if (uiManager.headEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[9] = uiManager.headEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager.headEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[10] = uiManager.headEquipmentInventorySlots[10].item;
            }
            if (uiManager.headEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[11] = uiManager.headEquipmentInventorySlots[11].item;
            }
            if (uiManager.headEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[12] = uiManager.headEquipmentInventorySlots[12].item;
            }
            if (uiManager.headEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[13] = uiManager.headEquipmentInventorySlots[13].item;
            }
            if (uiManager.headEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[14] = uiManager.headEquipmentInventorySlots[14].item;
            }
            if (uiManager.headEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[15] = uiManager.headEquipmentInventorySlots[15].item;
            }
            if (uiManager.headEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[16] = uiManager.headEquipmentInventorySlots[16].item;
            }
            if (uiManager.headEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[17] = uiManager.headEquipmentInventorySlots[17].item;
            }
            if (uiManager.headEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[18] = uiManager.headEquipmentInventorySlots[18].item;
            }
            if (uiManager.headEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[19] = uiManager.headEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager.headEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[20] = uiManager.headEquipmentInventorySlots[20].item;
            }
            if (uiManager.headEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[21] = uiManager.headEquipmentInventorySlots[21].item;
            }
            if (uiManager.headEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[22] = uiManager.headEquipmentInventorySlots[22].item;
            }
            if (uiManager.headEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[23] = uiManager.headEquipmentInventorySlots[23].item;
            }
            if (uiManager.headEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[24] = uiManager.headEquipmentInventorySlots[24].item;
            }
            if (uiManager.headEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[25] = uiManager.headEquipmentInventorySlots[25].item;
            }
            if (uiManager.headEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[26] = uiManager.headEquipmentInventorySlots[26].item;
            }
            if (uiManager.headEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[27] = uiManager.headEquipmentInventorySlots[27].item;
            }
            if (uiManager.headEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[28] = uiManager.headEquipmentInventorySlots[28].item;
            }
            if (uiManager.headEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[29] = uiManager.headEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager.headEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[30] = uiManager.headEquipmentInventorySlots[30].item;
            }
            if (uiManager.headEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[31] = uiManager.headEquipmentInventorySlots[31].item;
            }
            if (uiManager.headEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[32] = uiManager.headEquipmentInventorySlots[32].item;
            }
            if (uiManager.headEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[33] = uiManager.headEquipmentInventorySlots[33].item;
            }
            if (uiManager.headEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[34] = uiManager.headEquipmentInventorySlots[34].item;
            }
            if (uiManager.headEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[35] = uiManager.headEquipmentInventorySlots[35].item;
            }
            if (uiManager.headEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[36] = uiManager.headEquipmentInventorySlots[36].item;
            }
            if (uiManager.headEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[37] = uiManager.headEquipmentInventorySlots[37].item;
            }
            if (uiManager.headEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[38] = uiManager.headEquipmentInventorySlots[38].item;
            }
            if (uiManager.headEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[39] = uiManager.headEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager.headEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[40] = uiManager.headEquipmentInventorySlots[40].item;
            }
            if (uiManager.headEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[41] = uiManager.headEquipmentInventorySlots[41].item;
            }
            if (uiManager.headEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[42] = uiManager.headEquipmentInventorySlots[42].item;
            }
            if (uiManager.headEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[43] = uiManager.headEquipmentInventorySlots[43].item;
            }
            if (uiManager.headEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.headEquipmentSlots[44] = uiManager.headEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region BODY

            #region SLOT 0 - 9

            if (uiManager.bodyEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[0] = uiManager.bodyEquipmentInventorySlots[0].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[1] = uiManager.bodyEquipmentInventorySlots[1].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[2] = uiManager.bodyEquipmentInventorySlots[2].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[3] = uiManager.bodyEquipmentInventorySlots[3].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[4] = uiManager.bodyEquipmentInventorySlots[4].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[5] = uiManager.bodyEquipmentInventorySlots[5].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[6] = uiManager.bodyEquipmentInventorySlots[6].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[7] = uiManager.bodyEquipmentInventorySlots[7].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[8] = uiManager.bodyEquipmentInventorySlots[8].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[9] = uiManager.bodyEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager.bodyEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[10] = uiManager.bodyEquipmentInventorySlots[10].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[11] = uiManager.bodyEquipmentInventorySlots[11].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[12] = uiManager.bodyEquipmentInventorySlots[12].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[13] = uiManager.bodyEquipmentInventorySlots[13].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[14] = uiManager.bodyEquipmentInventorySlots[14].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[15] = uiManager.bodyEquipmentInventorySlots[15].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[16] = uiManager.bodyEquipmentInventorySlots[16].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[17] = uiManager.bodyEquipmentInventorySlots[17].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[18] = uiManager.bodyEquipmentInventorySlots[18].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[19] = uiManager.bodyEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager.bodyEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[20] = uiManager.bodyEquipmentInventorySlots[20].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[21] = uiManager.bodyEquipmentInventorySlots[21].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[22] = uiManager.bodyEquipmentInventorySlots[22].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[23] = uiManager.bodyEquipmentInventorySlots[23].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[24] = uiManager.bodyEquipmentInventorySlots[24].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[25] = uiManager.bodyEquipmentInventorySlots[25].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[26] = uiManager.bodyEquipmentInventorySlots[26].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[27] = uiManager.bodyEquipmentInventorySlots[27].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[28] = uiManager.bodyEquipmentInventorySlots[28].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[29] = uiManager.bodyEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager.bodyEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[30] = uiManager.bodyEquipmentInventorySlots[30].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[31] = uiManager.bodyEquipmentInventorySlots[31].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[32] = uiManager.bodyEquipmentInventorySlots[32].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[33] = uiManager.bodyEquipmentInventorySlots[33].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[34] = uiManager.bodyEquipmentInventorySlots[34].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[35] = uiManager.bodyEquipmentInventorySlots[35].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[36] = uiManager.bodyEquipmentInventorySlots[36].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[37] = uiManager.bodyEquipmentInventorySlots[37].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[38] = uiManager.bodyEquipmentInventorySlots[38].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[39] = uiManager.bodyEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager.bodyEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[40] = uiManager.bodyEquipmentInventorySlots[40].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[41] = uiManager.bodyEquipmentInventorySlots[41].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[42] = uiManager.bodyEquipmentInventorySlots[42].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[43] = uiManager.bodyEquipmentInventorySlots[43].item;
            }
            if (uiManager.bodyEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.bodyEquipmentSlots[44] = uiManager.bodyEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region LEG

            #region SLOT 0 - 9

            if (uiManager.legEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[0] = uiManager.legEquipmentInventorySlots[0].item;
            }
            if (uiManager.legEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[1] = uiManager.legEquipmentInventorySlots[1].item;
            }
            if (uiManager.legEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[2] = uiManager.legEquipmentInventorySlots[2].item;
            }
            if (uiManager.legEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[3] = uiManager.legEquipmentInventorySlots[3].item;
            }
            if (uiManager.legEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[4] = uiManager.legEquipmentInventorySlots[4].item;
            }
            if (uiManager.legEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[5] = uiManager.legEquipmentInventorySlots[5].item;
            }
            if (uiManager.legEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[6] = uiManager.legEquipmentInventorySlots[6].item;
            }
            if (uiManager.legEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[7] = uiManager.legEquipmentInventorySlots[7].item;
            }
            if (uiManager.legEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[8] = uiManager.legEquipmentInventorySlots[8].item;
            }
            if (uiManager.legEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[9] = uiManager.legEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager.legEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[10] = uiManager.legEquipmentInventorySlots[10].item;
            }
            if (uiManager.legEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[11] = uiManager.legEquipmentInventorySlots[11].item;
            }
            if (uiManager.legEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[12] = uiManager.legEquipmentInventorySlots[12].item;
            }
            if (uiManager.legEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[13] = uiManager.legEquipmentInventorySlots[13].item;
            }
            if (uiManager.legEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[14] = uiManager.legEquipmentInventorySlots[14].item;
            }
            if (uiManager.legEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[15] = uiManager.legEquipmentInventorySlots[15].item;
            }
            if (uiManager.legEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[16] = uiManager.legEquipmentInventorySlots[16].item;
            }
            if (uiManager.legEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[17] = uiManager.legEquipmentInventorySlots[17].item;
            }
            if (uiManager.legEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[18] = uiManager.legEquipmentInventorySlots[18].item;
            }
            if (uiManager.legEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[19] = uiManager.legEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager.legEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[20] = uiManager.legEquipmentInventorySlots[20].item;
            }
            if (uiManager.legEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[21] = uiManager.legEquipmentInventorySlots[21].item;
            }
            if (uiManager.legEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[22] = uiManager.legEquipmentInventorySlots[22].item;
            }
            if (uiManager.legEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[23] = uiManager.legEquipmentInventorySlots[23].item;
            }
            if (uiManager.legEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[24] = uiManager.legEquipmentInventorySlots[24].item;
            }
            if (uiManager.legEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[25] = uiManager.legEquipmentInventorySlots[25].item;
            }
            if (uiManager.legEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[26] = uiManager.legEquipmentInventorySlots[26].item;
            }
            if (uiManager.legEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[27] = uiManager.legEquipmentInventorySlots[27].item;
            }
            if (uiManager.legEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[28] = uiManager.legEquipmentInventorySlots[28].item;
            }
            if (uiManager.legEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[29] = uiManager.legEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager.legEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[30] = uiManager.legEquipmentInventorySlots[30].item;
            }
            if (uiManager.legEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[31] = uiManager.legEquipmentInventorySlots[31].item;
            }
            if (uiManager.legEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[32] = uiManager.legEquipmentInventorySlots[32].item;
            }
            if (uiManager.legEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[33] = uiManager.legEquipmentInventorySlots[33].item;
            }
            if (uiManager.legEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[34] = uiManager.legEquipmentInventorySlots[34].item;
            }
            if (uiManager.legEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[35] = uiManager.legEquipmentInventorySlots[35].item;
            }
            if (uiManager.legEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[36] = uiManager.legEquipmentInventorySlots[36].item;
            }
            if (uiManager.legEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[37] = uiManager.legEquipmentInventorySlots[37].item;
            }
            if (uiManager.legEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[38] = uiManager.legEquipmentInventorySlots[38].item;
            }
            if (uiManager.legEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[39] = uiManager.legEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager.legEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[40] = uiManager.legEquipmentInventorySlots[40].item;
            }
            if (uiManager.legEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[41] = uiManager.legEquipmentInventorySlots[41].item;
            }
            if (uiManager.legEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[42] = uiManager.legEquipmentInventorySlots[42].item;
            }
            if (uiManager.legEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[43] = uiManager.legEquipmentInventorySlots[43].item;
            }
            if (uiManager.legEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.legEquipmentSlots[44] = uiManager.legEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #region HAND

            #region SLOT 0 - 9

            if (uiManager.handEquipmentInventorySlots[0].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[0] = uiManager.handEquipmentInventorySlots[0].item;
            }
            if (uiManager.handEquipmentInventorySlots[1].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[1] = uiManager.handEquipmentInventorySlots[1].item;
            }
            if (uiManager.handEquipmentInventorySlots[2].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[2] = uiManager.handEquipmentInventorySlots[2].item;
            }
            if (uiManager.handEquipmentInventorySlots[3].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[3] = uiManager.handEquipmentInventorySlots[3].item;
            }
            if (uiManager.handEquipmentInventorySlots[4].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[4] = uiManager.handEquipmentInventorySlots[4].item;
            }
            if (uiManager.handEquipmentInventorySlots[5].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[5] = uiManager.handEquipmentInventorySlots[5].item;
            }
            if (uiManager.handEquipmentInventorySlots[6].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[6] = uiManager.handEquipmentInventorySlots[6].item;
            }
            if (uiManager.handEquipmentInventorySlots[7].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[7] = uiManager.handEquipmentInventorySlots[7].item;
            }
            if (uiManager.handEquipmentInventorySlots[8].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[8] = uiManager.handEquipmentInventorySlots[8].item;
            }
            if (uiManager.handEquipmentInventorySlots[9].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[9] = uiManager.handEquipmentInventorySlots[9].item;
            }

            #endregion

            #region SLOT 10 - 19

            if (uiManager.handEquipmentInventorySlots[10].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[10] = uiManager.handEquipmentInventorySlots[10].item;
            }
            if (uiManager.handEquipmentInventorySlots[11].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[11] = uiManager.handEquipmentInventorySlots[11].item;
            }
            if (uiManager.handEquipmentInventorySlots[12].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[12] = uiManager.handEquipmentInventorySlots[12].item;
            }
            if (uiManager.handEquipmentInventorySlots[13].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[13] = uiManager.handEquipmentInventorySlots[13].item;
            }
            if (uiManager.handEquipmentInventorySlots[14].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[14] = uiManager.handEquipmentInventorySlots[14].item;
            }
            if (uiManager.handEquipmentInventorySlots[15].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[15] = uiManager.handEquipmentInventorySlots[15].item;
            }
            if (uiManager.handEquipmentInventorySlots[16].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[16] = uiManager.handEquipmentInventorySlots[16].item;
            }
            if (uiManager.handEquipmentInventorySlots[17].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[17] = uiManager.handEquipmentInventorySlots[17].item;
            }
            if (uiManager.handEquipmentInventorySlots[18].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[18] = uiManager.handEquipmentInventorySlots[18].item;
            }
            if (uiManager.handEquipmentInventorySlots[19].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[19] = uiManager.handEquipmentInventorySlots[19].item;
            }

            #endregion

            #region SLOT 20 - 29

            if (uiManager.handEquipmentInventorySlots[20].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[20] = uiManager.handEquipmentInventorySlots[20].item;
            }
            if (uiManager.handEquipmentInventorySlots[21].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[21] = uiManager.handEquipmentInventorySlots[21].item;
            }
            if (uiManager.handEquipmentInventorySlots[22].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[22] = uiManager.handEquipmentInventorySlots[22].item;
            }
            if (uiManager.handEquipmentInventorySlots[23].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[23] = uiManager.handEquipmentInventorySlots[23].item;
            }
            if (uiManager.handEquipmentInventorySlots[24].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[24] = uiManager.handEquipmentInventorySlots[24].item;
            }
            if (uiManager.handEquipmentInventorySlots[25].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[25] = uiManager.handEquipmentInventorySlots[25].item;
            }
            if (uiManager.handEquipmentInventorySlots[26].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[26] = uiManager.handEquipmentInventorySlots[26].item;
            }
            if (uiManager.handEquipmentInventorySlots[27].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[27] = uiManager.handEquipmentInventorySlots[27].item;
            }
            if (uiManager.handEquipmentInventorySlots[28].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[28] = uiManager.handEquipmentInventorySlots[28].item;
            }
            if (uiManager.handEquipmentInventorySlots[29].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[29] = uiManager.handEquipmentInventorySlots[29].item;
            }

            #endregion

            #region SLOT 30 - 39

            if (uiManager.handEquipmentInventorySlots[30].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[30] = uiManager.handEquipmentInventorySlots[30].item;
            }
            if (uiManager.handEquipmentInventorySlots[31].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[31] = uiManager.handEquipmentInventorySlots[31].item;
            }
            if (uiManager.handEquipmentInventorySlots[32].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[32] = uiManager.handEquipmentInventorySlots[32].item;
            }
            if (uiManager.handEquipmentInventorySlots[33].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[33] = uiManager.handEquipmentInventorySlots[33].item;
            }
            if (uiManager.handEquipmentInventorySlots[34].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[34] = uiManager.handEquipmentInventorySlots[34].item;
            }
            if (uiManager.handEquipmentInventorySlots[35].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[35] = uiManager.handEquipmentInventorySlots[35].item;
            }
            if (uiManager.handEquipmentInventorySlots[36].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[36] = uiManager.handEquipmentInventorySlots[36].item;
            }
            if (uiManager.handEquipmentInventorySlots[37].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[37] = uiManager.handEquipmentInventorySlots[37].item;
            }
            if (uiManager.handEquipmentInventorySlots[38].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[38] = uiManager.handEquipmentInventorySlots[38].item;
            }
            if (uiManager.handEquipmentInventorySlots[39].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[39] = uiManager.handEquipmentInventorySlots[39].item;
            }

            #endregion

            #region SLOT 40 - 44

            if (uiManager.handEquipmentInventorySlots[40].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[40] = uiManager.handEquipmentInventorySlots[40].item;
            }
            if (uiManager.handEquipmentInventorySlots[41].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[41] = uiManager.handEquipmentInventorySlots[41].item;
            }
            if (uiManager.handEquipmentInventorySlots[42].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[42] = uiManager.handEquipmentInventorySlots[42].item;
            }
            if (uiManager.handEquipmentInventorySlots[43].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[43] = uiManager.handEquipmentInventorySlots[43].item;
            }
            if (uiManager.handEquipmentInventorySlots[44].item != null)
            {
                currentCharacterSaveData.handEquipmentSlots[44] = uiManager.handEquipmentInventorySlots[44].item;
            }

            #endregion

            #endregion

            #endregion

            #region GEARS SAVE
            //GEAR
            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                currentCharacterSaveData.currentHeadGearItemID = playerInventoryManager.currentHelmetEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHeadGearItemID = -1;
            }
            
            if (playerInventoryManager.currentBodyEquipment != null)
            {
                currentCharacterSaveData.currentChestGearItemID = playerInventoryManager.currentBodyEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentChestGearItemID = -1;
            }
            
            if (playerInventoryManager.currentLegEquipment != null)
            {
                currentCharacterSaveData.currentLegGearItemID = playerInventoryManager.currentLegEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentLegGearItemID = -1;
            }
            
            if (playerInventoryManager.currentHandEquipment != null)
            {
                currentCharacterSaveData.currentHandGearItemID = playerInventoryManager.currentHandEquipment.itemID;
            }
            else
            {
                currentCharacterSaveData.currentHandGearItemID = -1;
            }

            #endregion
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData(ref CharacterSaveData currentCharacterSaveData)
        {
            if (currentCharacterSaveData == null)
                return;

            #region STATS PLAYER WILL BE LOAD

            if (currentCharacterSaveData.characterAvatar != null)
            {
                playerStatsManager._characterAvatar.enabled = true;
                playerStatsManager._characterAvatar.sprite = currentCharacterSaveData.characterAvatar;
            }
            else
            {
                playerStatsManager._characterAvatar.enabled = false;
            }

            playerStatsManager._characterAvatar.sprite = currentCharacterSaveData.characterAvatar;
            playerStatsManager.characterName = currentCharacterSaveData.characterName;
            playerStatsManager.playerLevel = currentCharacterSaveData.characterLevel;
            playerStatsManager.healthLevel = currentCharacterSaveData.characterHealthLevel;
            playerStatsManager.staminaLevel = currentCharacterSaveData.characterStaminaLevel;
            playerStatsManager.focusLevel = currentCharacterSaveData.characterFocusLevel;
            playerStatsManager.poiseLevel = currentCharacterSaveData.characterPoiseLevel;
            playerStatsManager.strengthLevel = currentCharacterSaveData.characterStrengthLevel;
            playerStatsManager.dexterityLevel = currentCharacterSaveData.characterDexterityLevel;
            playerStatsManager.intelligenceLevel = currentCharacterSaveData.characterIntelligenceLevel;
            playerStatsManager.faithLevel = currentCharacterSaveData.characterFaithLevel;

            playerStatsManager.SetMaxHealthFromHealthLevel();
            playerStatsManager.currentHealth = playerStatsManager.maxHealth;
            playerStatsManager.healthBar.slider.maxValue = playerStatsManager.maxHealth;
            playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerStatsManager.currentStamina = playerStatsManager.maxStamina;
            playerStatsManager.staminaBar.slider.maxValue = playerStatsManager.maxStamina;
            playerStatsManager.SetMaxFocusPointsFromFocusLevel();
            playerStatsManager.currentFocusPoints = playerStatsManager.maxFocusPoints;
            playerStatsManager.focusPointBar.slider.maxValue = playerStatsManager.maxFocusPoints;

            playerStatsManager.currentSoulCount = currentCharacterSaveData.characterSoulsCount;
            playerStatsManager.soulCountBar.SetSoulCountText(playerStatsManager.currentSoulCount);

            #endregion

            #region POSITION WILL BE LOAD

            if (currentCharacterSaveData.sceneIndex == 0)
            {
                currentCharacterSaveData.sceneIndex = 1;

                Vector3 loadPosition = new Vector3(-83, -5, 43);
                characterController.Move(loadPosition - transform.position);
            }
            else
            {
                if (currentCharacterSaveData.sceneIndex == 1)
                {
                    if (currentCharacterSaveData.xPosition != 0 &&
                        currentCharacterSaveData.yPosition != 0 &&
                        currentCharacterSaveData.zPosition != 0)
                    {
                        Vector3 loadPosition = new Vector3(
                            currentCharacterSaveData.xPosition,
                            currentCharacterSaveData.yPosition,
                            currentCharacterSaveData.zPosition);
                        characterController.Move(loadPosition - transform.position);
                    }
                    else
                    {
                        Vector3 loadPosition = new Vector3(-83, -5, 43);
                        characterController.Move(loadPosition - transform.position);
                    }
                }
                else
                {
                    Vector3 loadPosition = new Vector3(
                        currentCharacterSaveData.xPosition,
                        currentCharacterSaveData.yPosition,
                        currentCharacterSaveData.zPosition);
                    characterController.Move(loadPosition - transform.position);
                }
            }

            #endregion

            //TEST POSITION
            //Vector3 loadPosition = new Vector3(
            //    currentCharacterSaveData.xPosition,
            //    currentCharacterSaveData.yPosition,
            //    currentCharacterSaveData.zPosition);
            //characterController.Move(loadPosition - transform.position);

            #region SKILLS
            //SKILL POINTS COUNT
            _playerSkills._talentTree.points = currentCharacterSaveData._points;
            _playerSkills._talentTree.UpdateTalentPointText();

            //COMBAT SKILLS
            //SKILL 1
            _playerSkills._combat_AttackSpeedSkill.currentCount = currentCharacterSaveData._combat_AttackSpeed_point;

            if (_playerSkills._combat_AttackSpeedSkill.currentCount == _playerSkills._combat_AttackSpeedSkill.maxCount)
            {
                _playerSkills._combat_ComboAttackSkill.Unlock();
                _playerSkills._combat_JumpAttackSkill.Unlock();
                _playerSkills._combat_RunAttackSkill.Unlock();
            }

            //SKILL 2
            _playerSkills._combat_ComboAttackSkill.currentCount = currentCharacterSaveData._combat_ComboAttack_point;

            if (_playerSkills._combat_ComboAttackSkill.currentCount == _playerSkills._combat_ComboAttackSkill.maxCount)
            {
                _playerSkills._combat_BackstabOrRiposteSkill.Unlock();
            }

            //SKILL 3 / 4 / 5
            _playerSkills._combat_RunAttackSkill.currentCount = currentCharacterSaveData._combat_RunAttack_point;
            _playerSkills._combat_DashAttackSkill.currentCount = currentCharacterSaveData._combat_DashAttack_point;
            _playerSkills._combat_JumpAttackSkill.currentCount = currentCharacterSaveData._combat_JumpAttack_point;

            if (_playerSkills._combat_RunAttackSkill.currentCount == _playerSkills._combat_RunAttackSkill.maxCount)
            {
                _playerSkills._combat_DashAttackSkill.Unlock();
            }

            //SKILL 6
            _playerSkills._combat_BackstabOrRiposteSkill.currentCount = currentCharacterSaveData._combat_CriticalAttack_point;

            if (_playerSkills._combat_BackstabOrRiposteSkill.currentCount == _playerSkills._combat_BackstabOrRiposteSkill.maxCount)
            {
                _playerSkills._combat_HeavyAttackSkill.Unlock();
                _playerSkills._combat_ChargeAttackSkill.Unlock();
            }

            //SKILL 7 / 8
            _playerSkills._combat_HeavyAttackSkill.currentCount = currentCharacterSaveData._combat_HeavyAttack_point;
            _playerSkills._combat_HeavyAttackComboSkill.currentCount = currentCharacterSaveData._combat_HeavyComboAttack_point;

            if (_playerSkills._combat_HeavyAttackSkill.currentCount == _playerSkills._combat_HeavyAttackSkill.maxCount)
            {
                _playerSkills._combat_HeavyAttackComboSkill.Unlock();
            }

            //SKILL 9 / 10
            _playerSkills._combat_ChargeAttackSkill.currentCount = currentCharacterSaveData._combat_ChargeAttack_point;
            _playerSkills._combat_ChargeAttackComboSkill.currentCount = currentCharacterSaveData._combat_ChargeAttackCombo_point;

            if (_playerSkills._combat_HeavyAttackSkill.currentCount == _playerSkills._combat_HeavyAttackSkill.maxCount)
            {
                _playerSkills._combat_ChargeAttackComboSkill.Unlock();
            }

            //PASSIVE SKILL
            _playerSkills._passive_playerBasePassive.currentCount = currentCharacterSaveData._passive_playerBase_point;
            _playerSkills._passive_IncreaseHealthRate.currentCount = currentCharacterSaveData._passive_HealthRate_point;
            _playerSkills._passive_IncreaseStaminaRate.currentCount = currentCharacterSaveData._passive_StaminaRate_point;
            _playerSkills._passive_IncreaseFocusPointRate.currentCount = currentCharacterSaveData._passive_FocusRate_point;
            _playerSkills._passive_MovementSpeed.currentCount = currentCharacterSaveData._passive_MovementSpeed_point;
            _playerSkills._passive_DeathOrAlive.currentCount = currentCharacterSaveData._passive_DeathOrAlive_point;
            _playerSkills._passive_DeathDance.currentCount = currentCharacterSaveData._passive_DoubleJump_point;
            _playerSkills._passive_GodSpeed.currentCount = currentCharacterSaveData._passive_GodSpeed_point;

            if (_playerSkills._passive_playerBasePassive.currentCount == _playerSkills._passive_playerBasePassive.maxCount)
            {
                _playerSkills._passive_IncreaseHealthRate.Unlock();
                _playerSkills._passive_IncreaseStaminaRate.Unlock();
                _playerSkills._passive_IncreaseFocusPointRate.Unlock();
                _playerSkills._passive_MovementSpeed.Unlock();
                _playerSkills._passive_DeathOrAlive.Unlock();
                _playerSkills._passive_DeathDance.Unlock();
                _playerSkills._passive_GodSpeed.Unlock();
            }

            #endregion

            //EQUIPMENT
            #region CURRENT EQUIPMENT WILL BE LOAD

            playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID
                (currentCharacterSaveData.currentRightHandWeaponID);
            playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID
                (currentCharacterSaveData.currentLeftHandWeaponID);
            playerWeaponSlotManager.LoadBothWeaponOnSlots();

            playerInventoryManager.weaponsInRightHandSlots[0] = currentCharacterSaveData.weaponsInRightHandSlots[0];
            playerInventoryManager.weaponsInRightHandSlots[1] = currentCharacterSaveData.weaponsInRightHandSlots[1];
            playerInventoryManager.weaponsInLeftHandSlots[0] = currentCharacterSaveData.weaponsInLeftHandSlots[0];
            playerInventoryManager.weaponsInLeftHandSlots[1] = currentCharacterSaveData.weaponsInLeftHandSlots[1];

            playerInventoryManager.currentAmmo = WorldItemDataBase.Instance.GetAmmoItemByID
                (currentCharacterSaveData._currentAmmo_Arrow);

            //SPELL
            if (currentCharacterSaveData._currentSpell != null)
            {
                playerInventoryManager.currentSpell = currentCharacterSaveData._currentSpell;
                uiManager.quickSlotsUI.UpdateCurrentSpellIcon(currentCharacterSaveData._currentSpell);
                uiManager.equipmentWindowUI._spellEquipmentSlotsUI.AddItem(currentCharacterSaveData._currentSpell);
                _playerSkills._spellsSelected._UpdateUISpell(currentCharacterSaveData._currentSpell);
            }

            //CONSUMABLE
            if (currentCharacterSaveData._currentConsumableItem != null)
            {
                playerInventoryManager.currentConsumable = currentCharacterSaveData._currentConsumableItem;
                //uiManager.quickSlotsUI.UpdateCurrentConsumableIcon(currentCharacterSaveData._currentConsumableItem);
                uiManager.equipmentWindowUI._consumableEquipmentSlotsUI.AddItem(currentCharacterSaveData._currentConsumableItem);
            }

            #endregion

            //INVENTORY
            #region INVENTORY

            #region WEAPON INVENT DATA LOAD
            // #WEAPON
            if (currentCharacterSaveData.weaponsInventorySlot[0] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[0]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[1] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[1]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[2] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[2]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[3] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[3]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[4] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[4]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[5] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[5]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[6] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[6]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[7] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[7]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[8] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[8]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[9] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[9]);
            }


            if (currentCharacterSaveData.weaponsInventorySlot[10] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[10]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[11] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[11]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[12] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[12]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[13] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[13]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[14] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[14]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[15] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[15]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[16] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[16]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[17] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[17]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[18] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[18]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[19] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[19]);
            }


            if (currentCharacterSaveData.weaponsInventorySlot[20] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[20]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[21] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[21]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[22] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[22]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[23] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[23]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[24] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[24]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[25] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[25]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[26] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[26]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[27] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[27]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[28] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[28]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[29] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[29]);
            }


            if (currentCharacterSaveData.weaponsInventorySlot[30] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[30]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[31] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[31]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[32] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[32]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[33] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[33]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[34] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[34]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[35] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[35]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[36] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[36]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[37] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[37]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[38] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[38]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[39] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[39]);
            }


            if (currentCharacterSaveData.weaponsInventorySlot[40] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[40]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[41] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[41]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[42] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[42]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[43] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[43]);
            }
            if (currentCharacterSaveData.weaponsInventorySlot[44] != null)
            {
                playerInventoryManager.weaponsInventory.Add(currentCharacterSaveData.weaponsInventorySlot[44]);
            }

            #endregion

            #region CONSUMABLE INVENT DATA LOAD
            // #CONSUMABLE
            if (currentCharacterSaveData.consumablesInventorySlots[0] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[0]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[1] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[1]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[2] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[2]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[3] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[3]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[4] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[4]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[5] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[5]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[6] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[6]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[7] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[7]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[8] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[8]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[9] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[9]);
            }


            if (currentCharacterSaveData.consumablesInventorySlots[10] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[10]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[11] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[11]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[12] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[12]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[13] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[13]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[14] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[14]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[15] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[15]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[16] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[16]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[17] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[17]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[18] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[18]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[19] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[19]);
            }


            if (currentCharacterSaveData.consumablesInventorySlots[20] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[20]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[21] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[21]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[22] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[22]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[23] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[23]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[24] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[24]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[25] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[25]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[26] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[26]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[27] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[27]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[28] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[28]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[29] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[29]);
            }


            if (currentCharacterSaveData.consumablesInventorySlots[30] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[30]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[31] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[31]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[32] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[32]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[33] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[33]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[34] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[34]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[35] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[35]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[36] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[36]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[37] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[37]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[38] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[38]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[39] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[39]);
            }


            if (currentCharacterSaveData.consumablesInventorySlots[40] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[40]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[41] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[41]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[42] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[42]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[43] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[43]);
            }
            if (currentCharacterSaveData.consumablesInventorySlots[44] != null)
            {
                playerInventoryManager._consumablesInventory.Add(currentCharacterSaveData.consumablesInventorySlots[44]);
            }

            #endregion

            #region RING INVENT DATA LOAD
            // #RING
            if (currentCharacterSaveData.ringsInventorySlots[0] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[0]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[1] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[1]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[2] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[2]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[3] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[3]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[4] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[4]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[5] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[5]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[6] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[6]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[7] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[7]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[8] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[8]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[9] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[9]);
            }


            if (currentCharacterSaveData.ringsInventorySlots[10] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[10]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[11] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[11]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[12] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[12]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[13] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[13]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[14] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[14]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[15] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[15]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[16] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[16]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[17] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[17]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[18] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[18]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[19] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[19]);
            }


            if (currentCharacterSaveData.ringsInventorySlots[20] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[20]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[21] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[21]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[22] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[22]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[23] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[23]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[24] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[24]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[25] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[25]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[26] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[26]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[27] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[27]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[28] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[28]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[29] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[29]);
            }


            if (currentCharacterSaveData.ringsInventorySlots[30] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[30]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[31] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[31]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[32] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[32]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[33] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[33]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[34] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[34]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[35] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[35]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[36] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[36]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[37] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[37]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[38] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[38]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[39] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[39]);
            }


            if (currentCharacterSaveData.ringsInventorySlots[40] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[40]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[41] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[41]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[42] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[42]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[43] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[43]);
            }
            if (currentCharacterSaveData.ringsInventorySlots[44] != null)
            {
                playerInventoryManager._ringsInventory.Add(currentCharacterSaveData.ringsInventorySlots[44]);
            }

            #endregion

            #region HELMET INVENT DATA LOAD
            // #HELMET
            if (currentCharacterSaveData.headEquipmentSlots[0] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[0]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[1] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[1]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[2] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[2]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[3] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[3]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[4] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[4]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[5] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[5]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[6] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[6]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[7] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[7]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[8] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[8]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[9] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[9]);
            }


            if (currentCharacterSaveData.headEquipmentSlots[10] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[10]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[11] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[11]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[12] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[12]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[13] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[13]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[14] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[14]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[15] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[15]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[16] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[16]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[17] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[17]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[18] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[18]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[19] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[19]);
            }


            if (currentCharacterSaveData.headEquipmentSlots[20] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[20]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[21] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[21]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[22] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[22]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[23] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[23]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[24] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[24]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[25] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[25]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[26] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[26]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[27] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[27]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[28] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[28]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[29] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[29]);
            }


            if (currentCharacterSaveData.headEquipmentSlots[30] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[30]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[31] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[31]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[32] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[32]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[33] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[33]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[34] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[34]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[35] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[35]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[36] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[36]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[37] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[37]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[38] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[38]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[39] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[39]);
            }


            if (currentCharacterSaveData.headEquipmentSlots[40] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[40]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[41] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[41]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[42] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[42]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[43] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[43]);
            }
            if (currentCharacterSaveData.headEquipmentSlots[44] != null)
            {
                playerInventoryManager.headEquipmentInventory.Add(currentCharacterSaveData.headEquipmentSlots[44]);
            }

            #endregion

            #region BODY INVENT DATA LOAD
            // #BODY
            if (currentCharacterSaveData.bodyEquipmentSlots[0] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[0]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[1] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[1]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[2] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[2]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[3] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[3]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[4] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[4]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[5] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[5]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[6] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[6]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[7] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[7]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[8] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[8]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[9] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[9]);
            }


            if (currentCharacterSaveData.bodyEquipmentSlots[10] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[10]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[11] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[11]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[12] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[12]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[13] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[13]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[14] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[14]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[15] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[15]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[16] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[16]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[17] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[17]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[18] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[18]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[19] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[19]);
            }


            if (currentCharacterSaveData.bodyEquipmentSlots[20] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[20]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[21] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[21]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[22] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[22]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[23] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[23]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[24] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[24]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[25] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[25]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[26] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[26]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[27] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[27]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[28] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[28]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[29] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[29]);
            }


            if (currentCharacterSaveData.bodyEquipmentSlots[30] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[30]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[31] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[31]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[32] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[32]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[33] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[33]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[34] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[34]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[35] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[35]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[36] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[36]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[37] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[37]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[38] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[38]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[39] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[39]);
            }


            if (currentCharacterSaveData.bodyEquipmentSlots[40] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[40]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[41] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[41]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[42] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[42]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[43] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[43]);
            }
            if (currentCharacterSaveData.bodyEquipmentSlots[44] != null)
            {
                playerInventoryManager.bodyEquipmentInventory.Add(currentCharacterSaveData.bodyEquipmentSlots[44]);
            }

            #endregion

            #region LEG INVENT DATA LOAD
            // #LEG
            if (currentCharacterSaveData.legEquipmentSlots[0] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[0]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[1] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[1]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[2] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[2]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[3] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[3]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[4] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[4]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[5] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[5]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[6] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[6]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[7] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[7]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[8] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[8]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[9] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[9]);
            }


            if (currentCharacterSaveData.legEquipmentSlots[10] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[10]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[11] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[11]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[12] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[12]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[13] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[13]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[14] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[14]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[15] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[15]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[16] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[16]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[17] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[17]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[18] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[18]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[19] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[19]);
            }


            if (currentCharacterSaveData.legEquipmentSlots[20] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[20]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[21] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[21]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[22] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[22]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[23] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[23]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[24] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[24]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[25] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[25]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[26] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[26]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[27] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[27]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[28] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[28]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[29] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[29]);
            }


            if (currentCharacterSaveData.legEquipmentSlots[30] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[30]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[31] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[31]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[32] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[32]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[33] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[33]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[34] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[34]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[35] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[35]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[36] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[36]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[37] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[37]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[38] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[38]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[39] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[39]);
            }


            if (currentCharacterSaveData.legEquipmentSlots[40] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[40]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[41] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[41]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[42] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[42]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[43] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[43]);
            }
            if (currentCharacterSaveData.legEquipmentSlots[44] != null)
            {
                playerInventoryManager.legEquipmentInventory.Add(currentCharacterSaveData.legEquipmentSlots[44]);
            }

            #endregion

            #region HAND INVENT DATA LOAD
            // #HAND
            if (currentCharacterSaveData.handEquipmentSlots[0] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[0]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[1] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[1]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[2] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[2]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[3] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[3]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[4] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[4]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[5] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[5]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[6] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[6]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[7] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[7]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[8] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[8]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[9] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[9]);
            }


            if (currentCharacterSaveData.handEquipmentSlots[10] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[10]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[11] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[11]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[12] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[12]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[13] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[13]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[14] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[14]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[15] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[15]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[16] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[16]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[17] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[17]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[18] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[18]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[19] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[19]);
            }


            if (currentCharacterSaveData.handEquipmentSlots[20] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[20]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[21] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[21]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[22] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[22]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[23] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[23]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[24] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[24]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[25] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[25]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[26] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[26]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[27] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[27]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[28] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[28]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[29] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[29]);
            }


            if (currentCharacterSaveData.handEquipmentSlots[30] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[30]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[31] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[31]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[32] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[32]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[33] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[33]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[34] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[34]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[35] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[35]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[36] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[36]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[37] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[37]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[38] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[38]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[39] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[39]);
            }


            if (currentCharacterSaveData.handEquipmentSlots[40] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[40]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[41] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[41]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[42] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[42]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[43] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[43]);
            }
            if (currentCharacterSaveData.handEquipmentSlots[44] != null)
            {
                playerInventoryManager.handEquipmentInventory.Add(currentCharacterSaveData.handEquipmentSlots[44]);
            }

            #endregion

            #endregion

            #region GEARS

            EquipmentItem headEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                (currentCharacterSaveData.currentHeadGearItemID);

            if (headEquipment != null)
            {
                playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
                uiManager.equipmentWindowUI.headEquipmentSlotsUI.AddItem(playerInventoryManager.currentHelmetEquipment);
            }

            EquipmentItem bodyEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                (currentCharacterSaveData.currentChestGearItemID);

            if (bodyEquipment != null)
            {
                playerInventoryManager.currentBodyEquipment = bodyEquipment as BodyEquipment;
                uiManager.equipmentWindowUI.bodyEquipmentSlotsUI.AddItem(playerInventoryManager.currentBodyEquipment);
            }

            EquipmentItem legEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                (currentCharacterSaveData.currentLegGearItemID);

            if (legEquipment != null)
            {
                playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
                uiManager.equipmentWindowUI.legEquipmentSlotsUI.AddItem(playerInventoryManager.currentLegEquipment);
            }

            EquipmentItem handEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                (currentCharacterSaveData.currentHandGearItemID);

            if (handEquipment != null)
            {
                playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
                uiManager.equipmentWindowUI.handEquipmentSlotsUI.AddItem(playerInventoryManager.currentHandEquipment);
            }

            playerEquipmentManager.EquipAllArmor();

            #endregion

            //UI UPDATE
            uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
            uiManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(playerInventoryManager);
        }

        public void LoadMapTEST(ref CharacterSaveData currentCharacterSaveData)
        {
            Vector3 loadPosition = new Vector3(
                currentCharacterSaveData.xPosition,
                currentCharacterSaveData.yPosition,
                currentCharacterSaveData.zPosition);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap1(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 1;

            Vector3 loadPosition = new Vector3(-83, -5, 43);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap2(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 2;

            Vector3 loadPosition = new Vector3(-7, -4.2f, 30);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap2_Back(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 1;

            Vector3 loadPosition = new Vector3(-4.1f, -4.61f, 28.11f);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap3(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 3;

            Vector3 loadPosition = new Vector3(2.5f, 0.71f, 12);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap3_Back(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 2;

            Vector3 loadPosition = new Vector3(2.35f, 0.6f, 17.5f);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap4(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 4;

            Vector3 loadPosition = new Vector3(3.05f, 5.7f, -22f);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap4_Back(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 3;

            Vector3 loadPosition = new Vector3(2.56f, 5.6f, -17.3f);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap5(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 5;

            Vector3 loadPosition = new Vector3(-7.44f, 5.68f, -77.77f);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadMap5_Back(ref CharacterSaveData currentCharacterSaveData)
        {
            currentCharacterSaveData.sceneIndex = 4;

            Vector3 loadPosition = new Vector3(-7.36f, 5.66f, -73);
            characterController.Move(loadPosition - transform.position);
        }

        public void LoadEndScene(ref CharacterSaveData currentCharacterSaveData)
        {
            Vector3 loadPosition = new Vector3(
                currentCharacterSaveData.xPosition = 0,
                currentCharacterSaveData.yPosition = 0,
                currentCharacterSaveData.zPosition = 0);

            characterController.Move(loadPosition - transform.position);
        }
    }
}