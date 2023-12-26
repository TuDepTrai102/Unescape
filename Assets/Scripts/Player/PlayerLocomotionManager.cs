using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace NT
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [Header("MOVEMENT STATS")]
        [SerializeField] float walkingSpeed = 1.5f;
        [SerializeField] public float runningSpeed = 5f;
        [SerializeField] public float sprintingSpeed = 7f;
        [SerializeField] float rotationSpeed = 10f;
        [SerializeField] float jumpHeight = 4f;

        [Header("STAMINA COSTS")]
        [SerializeField] int rollStaminaCost = 15;
        [SerializeField] int backStepStaminaCost = 12;
        [SerializeField] public int sprintStaminaCost = 1;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {

        }

        public void HandleRotation()
        {
            if (player.canRotate)
            {
                if (player.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotaion = Quaternion.Slerp
                        (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotaion;
                }
                else
                {
                    if (player.inputHandler.lockOnFlag)
                    {
                        if (player.isSprinting || player.inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
                            targetDirection += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
                            targetDirection.Normalize();

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp
                                (transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp
                                (transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = player.inputHandler.moveAmount;

                        targetDir = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                        targetDir += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                        {
                            targetDir = transform.forward;
                        }

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                }
            }     
        }

        public void HandleGroundedMovement()
        {
            player.playerAnimatorManager._ResetSpeedAnimation();

            if (player.inputHandler.rollFlag ||
                player.isInteracting ||
                !player.isGrounded)
                return;

            moveDirection = player.cameraHandler.transform.forward * player.inputHandler.vertical;
            moveDirection = moveDirection + player.cameraHandler.transform.right * player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting && player.inputHandler.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
                player.characterSoundFXManager.PlayRandomFootStep_Sprint();
                player.playerStatsManager.DeductSprintingStamina(sprintStaminaCost);
            }
            else
            {
                if (player.inputHandler.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                    player.characterSoundFXManager.PlayRandomFootStep_Run();
                }
                else if (player.inputHandler.moveAmount <= 0.5f && player.inputHandler.moveAmount > 0)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                    player.characterSoundFXManager.PlayRandomFootStep_Walk();
                }
            }

            if (player.inputHandler.lockOnFlag && player.isSprinting == false)
            {
                player.playerAnimatorManager.UpdateAnimatorValues
                    (player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorValues
                    (player.inputHandler.moveAmount, 0, player.isSprinting);
            }
        }

        public void HandleInAirMovement()
        {
            if (player.isGrounded)
                return;

            moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
            moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting && player.inputHandler.moveAmount > 0.5f)
            {
                moveDirection = moveDirection * sprintingSpeed * Time.deltaTime;
                player.characterController.Move(moveDirection / 3);
            }
            else
            {
                if (player.inputHandler.moveAmount > 0.5f)
                {
                    moveDirection = moveDirection * runningSpeed * Time.deltaTime;
                    player.characterController.Move(moveDirection / 3);
                }
                else if (player.inputHandler.moveAmount <= 0.5f)
                {
                    moveDirection = moveDirection * walkingSpeed * Time.deltaTime;
                    player.characterController.Move(moveDirection / 3);
                }
            }
        }

        public void HandleLShiftActions()
        {
            player.playerAnimatorManager._ResetSpeedAnimation();

            if (player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.inputHandler.rollFlag)
            {
                player.inputHandler.rollFlag = false;

                if (!player.canRoll)
                    return;

                moveDirection = player.cameraHandler.cameraObject.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.cameraObject.transform.right * player.inputHandler.horizontal;

                if (player.inputHandler.moveAmount > 0)
                {
                    switch (player.playerStatsManager.encumbranceLevel)
                    {
                        case EncumbranceLevel.Light:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_rolling_light, true);
                            break;
                        case EncumbranceLevel.Medium:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_rolling_medium, true);
                            break;
                        case EncumbranceLevel.Heavy:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_rolling_heavy, true);
                            break;
                        case EncumbranceLevel.Overloaded:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_rolling_overloaded, true);
                            break;
                    }

                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = rollRotation;
                    player.playerStatsManager.DeductStamina(rollStaminaCost);
                }
                else
                {
                    switch (player.playerStatsManager.encumbranceLevel)
                    {
                        case EncumbranceLevel.Light:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_backstep_light, true);
                            break;
                        case EncumbranceLevel.Medium:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_backstep_medium, true);
                            break;
                        case EncumbranceLevel.Heavy:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_backstep_heavy, true);
                            break;
                        case EncumbranceLevel.Overloaded:
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_backstep_overloaded, true);
                            break;
                    }

                    player.playerAnimatorManager.EraseHandIKForWeapon();
                    player.playerStatsManager.DeductStamina(backStepStaminaCost);
                }
            }
        }

        public void HandleJumping()
        {
            player.playerAnimatorManager._ResetSpeedAnimation();

            if (player.isJumping ||
                player.playerStatsManager.currentStamina <= 0)
                return;

            if (player.inputHandler.space_Input)
            {
                player.isJumping = true;
                player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_jump, true);
                player.playerAnimatorManager.EraseHandIKForWeapon();

                moveDirection = player.cameraHandler.transform.forward * player.inputHandler.vertical;
                moveDirection += player.cameraHandler.transform.right * player.inputHandler.horizontal;
                moveDirection.y = 0;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = jumpRotation;

                    if (player.isSprinting)
                    {
                        jumpDirection = moveDirection;
                    }
                    else if (player.inputHandler.moveAmount > 0.5)
                    {
                        jumpDirection = moveDirection * 0.5f;
                    }
                    else if (player.inputHandler.moveAmount <= 0.5)
                    {
                        jumpDirection = moveDirection * 0.25f;
                    }
                }
                else
                {
                    jumpDirection = Vector3.zero;
                }
            }
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

        public void ApplyForwardJumpForceOverTime()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * runningSpeed * Time.deltaTime);
            }
        }
    }
}