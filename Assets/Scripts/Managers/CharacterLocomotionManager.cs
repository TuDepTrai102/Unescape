using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        public Vector3 moveDirection;
        public Vector3 jumpDirection;
        public LayerMask groundLayer;

        [Header("GRAVITY SETTINGS")]
        public float inAirTimer = 0;
        [SerializeField] public Vector3 yVelocity;
        [SerializeField] public float groundYVelocity = -20; //THE FORCE APPLIED TO YOU WHILST GROUND
        [SerializeField] protected float fallStartYVelocity = -7; //THE FORCE APPLIED TO YOU WHEN YOU BEGIN TO FALL (INCREASES OVER TIME)
        [SerializeField] protected float gravityForce = -25;
        [SerializeField] float groundCheckSphereRadius = 1f;
        protected bool fallingVelocitySet = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
            character.animator.SetBool("isGrounded", character.isGrounded);
            HandleGroundCheck();
        }

        public virtual void HandleGroundCheck()
        {
            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocitySet = false;
                    yVelocity.y = groundYVelocity;
                }
            }
            else
            {
                if (!character.isJumping && !fallingVelocitySet)
                {
                    fallingVelocitySet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                yVelocity.y += gravityForce * Time.deltaTime;
            }

            character.animator.SetFloat("inAirTimer", inAirTimer);
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}