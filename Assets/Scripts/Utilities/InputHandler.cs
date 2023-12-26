using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls inputActions;
        PlayerManager player;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool lShift_Input;
        public bool f_Input;
        public bool c_Input;
        public bool y_Input;

        public bool tap_e_Input;
        public bool hold_e_Input;
        public bool tap_t_Input;
        public bool hold_t_Input;
        public bool _tap_q_Input;
        public bool _hold_q_Input;

        public bool x_Input;
        public bool tap_x_Input;
        public bool tap_z_Input;

        public bool space_Input;
        public bool tab_Input;
        public bool lockOn_Input;
        public bool lockOnLeft_Input;
        public bool lockOnRight_Input;

        public bool arrowU_Input;
        public bool arrowD_Input;
        public bool arrowR_Input;
        public bool arrowL_Input;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float leftShiftInputTimer;

        public bool input_Has_Been_Qued;
        public float current_Qued_Input_Timer;
        public float default_Qued_Input_Time;
        public bool qued_E_Input;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

                inputActions.PlayerActions.F1.performed += i => lockOnLeft_Input = true;
                inputActions.PlayerActions.F2.performed += i => lockOnRight_Input = true;

                inputActions.PlayerActions.LockOnTarget.performed += i => lockOn_Input = true;

                inputActions.PlayerActions.E.performed += i => tap_e_Input = true;
                inputActions.PlayerActions.HoldE.performed += i => hold_e_Input = true;
                inputActions.PlayerActions.HoldE.canceled += i => hold_e_Input = false;
                inputActions.PlayerActions.QuedE.performed += i => QueInput(ref qued_E_Input);

                inputActions.PlayerActions.T.performed += i => tap_t_Input = true;
                inputActions.PlayerActions.HoldT.performed += i => hold_t_Input = true;
                inputActions.PlayerActions.HoldT.canceled += i => hold_t_Input = false;

                inputActions.PlayerActions._Q.performed += i => _tap_q_Input = true;
                inputActions.PlayerActions._HoldQ.performed += i => _hold_q_Input = true;
                inputActions.PlayerActions._HoldQ.canceled += i => _hold_q_Input = false;

                inputActions.PlayerActions.Space.performed += i => space_Input = true;
                inputActions.PlayerActions.F.performed += i => f_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.Z.performed += i => tap_z_Input = true;
                inputActions.PlayerActions.C.performed += i => c_Input = true;

                inputActions.PlayerActions.TapX.performed += i => tap_x_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.X.canceled += i => x_Input = false;

                inputActions.PlayerActions.LShift.performed += i => lShift_Input = true;
                inputActions.PlayerActions.LShift.canceled += i => lShift_Input = false;

                inputActions.PlayerActions.Inventory.performed += i => tab_Input = true;

                inputActions.PlayerQuickSlot.RightArrow.performed += i => arrowR_Input = true;
                inputActions.PlayerQuickSlot.LeftArrow.performed += i => arrowL_Input = true;
                inputActions.PlayerQuickSlot.UpArrow.performed += i => arrowU_Input = true;
                inputActions.PlayerQuickSlot.DownArrow.performed += i => arrowD_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput()
        {
            if (player.isDead)
                return;

            HandleInventoryInput();

            if (inventoryFlag)
                return;

            HandleMoveInput();
            HandleLShiftInput();

            _HandleTapQInput();

            HandleHoldEInput();

            HandleTapEInput();

            HandleHoldXInput();
            HandleTapXInput();

            HandleHoldTInput();
            HandleTapTInput();

            HandleTapZInput();

            HandleQuickSlotInput();

            HandleJumpInput();

            HandleLockOnInput();

            HandleTwoHandInput();

            HandleUseConsumableInput();

            HandleQuedInput();
        }

        private void HandleMoveInput()
        {
            if (player.isBusy)
                return;

            if (player.isHoldingArrow || 
                player.playerStatsManager.encumbranceLevel == EncumbranceLevel.Overloaded)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) / 2);

                if (moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }

                if (player.bonfireTeleportUI.bonfireMenu_UI.activeSelf ||
                    player.uiManager.levelUpWindow.activeSelf ||
                    player.uiManager.shopWindow.activeSelf ||
                    player.uiManager.endingWindow.activeSelf ||
                    player.uiManager.selectWindow.activeSelf)
                    return;

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                if (player.bonfireTeleportUI.bonfireMenu_UI.activeSelf ||
                    player.uiManager.levelUpWindow.activeSelf ||
                    player.uiManager.shopWindow.activeSelf ||
                    player.uiManager.endingWindow.activeSelf ||
                    player.uiManager.selectWindow.activeSelf)
                    return;

                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleLShiftInput()
        {
            if (player.isBusy)
                return;

            if (lShift_Input)
            {
                leftShiftInputTimer += Time.deltaTime;

                if (player.playerStatsManager.currentStamina <= 0)
                {
                    lShift_Input = false;
                    player.isSprinting = false;
                }

                if (moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
                {
                    player.isSprinting = true;
                }
            }
            else
            {
                player.isSprinting = false;

                if (leftShiftInputTimer > 0 && leftShiftInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                leftShiftInputTimer = 0;
            }
        }


        private void _HandleTapQInput()
        {
            if (_tap_q_Input)
            {
                _tap_q_Input = false;

                if (player.playerInventoryManager.rightWeapon._oh_tap_Q_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon._oh_tap_Q_Action.PerformAction(player);
                }
            }
        }


        private void HandleTapEInput()
        {
            if (player.isBusy)
                return;

            if (tap_e_Input)
            {
                tap_e_Input = false;

                if (player.playerInventoryManager.rightWeapon.oh_tap_E_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_tap_E_Action.PerformAction(player);
                }
            }
        }

        private void HandleHoldEInput()
        {
            if (player.isBusy)
                return;

            if (hold_e_Input)
            {
                if (player.playerInventoryManager.rightWeapon.oh_hold_E_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_hold_E_Action.PerformAction(player);
                }
            }
        }

        private void HandleHoldTInput()
        {
            if (player.isBusy)
                return;

            player.animator.SetBool("isChargingAttack", hold_t_Input);

            if (hold_t_Input)
            {
                player.UpdateWhichHandCharacterIsUsing(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.th_hold_T_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.th_hold_T_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_T_Action != null)
                    {
                        player.playerInventoryManager.rightWeapon.oh_hold_T_Action.PerformAction(player);
                    }
                }
            }
        }


        private void HandleTapTInput()
        {
            if (player.isBusy)
                return;

            if (tap_t_Input)
            {
                tap_t_Input = false;

                if (player.playerInventoryManager.rightWeapon.oh_tap_T_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.oh_tap_T_Action.PerformAction(player);
                }
            }
        }

        private void HandleTapZInput()
        {
            if (player.isBusy)
                return;

            if (tap_z_Input)
            {
                tap_z_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    // it will be righthanded weapon
                    if (player.playerInventoryManager.rightWeapon.oh_tap_Z_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_Z_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_Z_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_Z_Action.PerformAction(player);
                    }
                }
            }
        }


        private void HandleHoldXInput()
        {
            if (player.isBusy)
                return;

            if (!player.isGrounded ||
                player.isSprinting ||
                player.isFiringSpell)
            {
                x_Input = false;
                return;
            }

            if (x_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_hold_X_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_hold_X_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_hold_X_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_hold_X_Action.PerformAction(player);
                    }
                }
            }
            else if (x_Input == false)
            {
                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.uiManager.crossHair.SetActive(false);
                    player.cameraHandler.ResetAimCameraRotations();
                }

                if (player.isBlocking)
                {
                    player.isBlocking = false;
                    player.isTwoHandingWeapon = false;
                }
            }
        }

        private void HandleTapXInput()
        {
            if (player.isBusy)
                return;

            if (tap_x_Input)
            {
                tap_x_Input = false;

                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.oh_tap_X_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.oh_tap_X_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.leftWeapon.oh_tap_X_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.oh_tap_X_Action.PerformAction(player);
                    }
                }
            }
        }



        private void HandleQuickSlotInput()
        {
            if (player.isBusy)
                return;

            if (arrowR_Input)
            {
                player.playerInventoryManager.ChangeRightWeapon();

                if (player.playerInventoryManager.rightWeapon == player.playerWeaponSlotManager.unarmedWeapon)
                {
                    twoHandFlag = false;
                    player.isTwoHandingWeapon = false;
                }
            }
            else if (arrowL_Input)
            {
                if (player.isTwoHandingWeapon)
                    return;

                player.playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleJumpInput()
        {
            if (player.isBusy)
                return;

            if (space_Input)
            {
                space_Input = true;
            }
        }

        private void HandleInventoryInput()
        {
            if (inventoryFlag)
            {
                player.uiManager.UpdateUI();
            }

            if (tab_Input)
            {
                if (player.bonfireTeleportUI.bonfireMenu_UI.activeSelf ||
                    player.uiManager.levelUpWindow.activeSelf ||
                    player.uiManager.shopWindow.activeSelf ||
                    player.uiManager.endingWindow.activeSelf)
                {
                    player.uiManager.CloseAllInventoryWindow();
                    return;
                }

                inventoryFlag = !inventoryFlag;

                if (inventoryFlag)
                {
                    player.uiManager.OpenSeclectWindow();
                    player.uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    player.uiManager.CloseSeclectWindow();
                    player.uiManager.CloseAllInventoryWindow();
                    player.uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if (player.isBusy)
                return;

            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && lockOnLeft_Input)
            {
                lockOnLeft_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.leftLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockOnTarget;
                }
            }

            if (lockOnFlag && lockOnRight_Input)
            {
                lockOnRight_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.rightLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockOnTarget;
                }
            }

            if (player.cameraHandler != null)
            {
                player.cameraHandler.SetCameraHeight();
            }
        }

        private void HandleTwoHandInput()
        {
            if (player.isBusy)
                return;

            if (y_Input)
            {
                y_Input = false;

                if (player.playerInventoryManager.rightWeapon == player.playerWeaponSlotManager.unarmedWeapon &&
                    player.playerWeaponSlotManager.leftHandSlot.currentWeapon.weaponType != WeaponType.Shield)
                    return;

                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    player.isTwoHandingWeapon = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSLot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(true);
                }
                else
                {
                    player.isTwoHandingWeapon = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSLot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSLot(player.playerInventoryManager.leftWeapon, true);
                    player.playerWeaponSlotManager.LoadTwoHandIKTargets(false);
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if (player.isBusy)
                return;

            if (c_Input)
            {
                c_Input = false;

                if (player.playerInventoryManager.currentConsumable != null)
                {
                    player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player);
                }
                else
                {
                    Debug.Log("DON'T HAVE CONSUMABLE ITEM BRUH");
                }
            }
        }

        private void QueInput(ref bool quedInput)
        {
            //DISABLE ALL OTHER QUED INPUTS
            //QUED_LB_INPUT = FALSE;
            //QUED_RT_INPUT = FALSE;

            //ENABLE THE REFERENCED INPUT BY REFERENCED
            //IF WE ARE INTERACTING, WE CAN QUE AN INPUT, OTHERWISE QUEING NOT NEEDED
            if (player.isInteracting)
            {
                quedInput = true;
                current_Qued_Input_Timer = default_Qued_Input_Time;
                input_Has_Been_Qued = true;
            }
        }

        private void HandleQuedInput()
        {
            if (player.isBusy)
                return;

            if (input_Has_Been_Qued)
            {
                if (current_Qued_Input_Timer > 0)
                {
                    current_Qued_Input_Timer -= Time.deltaTime;

                    //TRY AND PROCESS INPUT
                    ProcessQuedInput();
                }
                else
                {
                    input_Has_Been_Qued = false;
                    current_Qued_Input_Timer = 0;
                }
            }
        }

        private void ProcessQuedInput()
        {
            if (player.isBusy)
                return;

            if (qued_E_Input)
            {
                tap_e_Input = true;
            }
            //IF QUED T INPUT => TAP T INPUT = TRUE;
            //IF QUED X INPUT => TAP X INPUT = TRUE;
        }
    }
}