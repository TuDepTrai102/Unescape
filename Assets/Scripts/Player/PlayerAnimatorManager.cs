using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < 0)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (isSprinting && player.inputHandler.moveAmount > 0.5f)
            {
                v = 2;
                h = horizontalMovement;
            }

            player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DisableCollision()
        {
            player.characterController.enabled = false;
        }

        public void EnableCollision()
        {
            player.characterController.enabled = true;
        }

        public virtual void SuccessfullyUseCurrentConsumable()
        {
            if (character.characterInventoryManager.currentConsumable != null)
            {
                character.characterInventoryManager.currentConsumable.SuccessfullyConsumeItem(player);
            }
        }

        public void _ResetSpeedAnimation()
        {
            if (player.isAttacking || player._isChargingAttack)
                return;

            player.animator.speed = 1.0f;
        }
    }
}
