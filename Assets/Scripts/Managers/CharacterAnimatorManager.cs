using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace NT
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected CharacterManager character;

        protected RigBuilder rigBuilder;
        public TwoBoneIKConstraint leftHandConstraint;
        public TwoBoneIKConstraint rightHandConstraint;

        [Header("CHARACTER ACTIONS")]
        //ROLLING ANIMATIONS
        public string animation_rolling_light;
        public string animation_rolling_medium;
        public string animation_rolling_heavy;
        public string animation_rolling_overloaded;

        //BACKSTEP ANIMATIONS
        public string animation_backstep_light;
        public string animation_backstep_medium;
        public string animation_backstep_heavy;
        public string animation_backstep_overloaded;

        //TURN ANIMATIONS
        public string animation_turn_left;
        public string animation_turn_right;
        public string animation_turn_behind_right;
        public string animation_turn_behind_left;

        //ANOTHER ANIMATIONS
        public string animation_dead_01 = "straight_sword_main_death_01";

        public string animation_guard_break;

        public string animation_guard_block_light;
        public string animation_guard_block_medium;
        public string animation_guard_block_heavy;
        public string animation_guard_block_colossal;

        public string animation_cannot_cast;
        public string animation_pick_up_item;
        public string animation_open_chest;
        public string animation_pass_through;
        public string animation_bonfire_activate;
        public string animation_jump;
        public string animation_parry;

        [Header("DAMAGE ANIMATIONS")]
        public string damage_forward_light;
        public string damage_backward_light;
        public string damage_left_light;
        public string damage_right_light;

        public string damage_forward_medium;
        public string damage_backward_medium;
        public string damage_left_medium;
        public string damage_right_medium;

        public string damage_forward_heavy;
        public string damage_backward_heavy;
        public string damage_left_heavy;
        public string damage_right_heavy;

        public string damage_forward_colossal;
        public string damage_backward_colossal;
        public string damage_left_colossal;
        public string damage_right_colossal;

        public List<string> Damage_Animations_Light_Forward = new List<string>();
        public List<string> Damage_Animations_Light_Backward = new List<string>();
        public List<string> Damage_Animations_Light_Left = new List<string>();
        public List<string> Damage_Animations_Light_Right = new List<string>();

        public List<string> Damage_Animations_Medium_Forward = new List<string>();
        public List<string> Damage_Animations_Medium_Backward = new List<string>();
        public List<string> Damage_Animations_Medium_Left = new List<string>();
        public List<string> Damage_Animations_Medium_Right = new List<string>();

        public List<string> Damage_Animations_Heavy_Forward = new List<string>();
        public List<string> Damage_Animations_Heavy_Backward = new List<string>();
        public List<string> Damage_Animations_Heavy_Left = new List<string>();
        public List<string> Damage_Animations_Heavy_Right = new List<string>();

        public List<string> Damage_Animations_Colossal_Forward = new List<string>();
        public List<string> Damage_Animations_Colossal_Backward = new List<string>();
        public List<string> Damage_Animations_Colossal_Left = new List<string>();
        public List<string> Damage_Animations_Colossal_Right = new List<string>();

        bool handIKWeightsReset = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            rigBuilder = GetComponent<RigBuilder>();
        }

        protected virtual void Start()
        {
            Damage_Animations_Light_Forward.Add(damage_forward_light);
            Damage_Animations_Light_Backward.Add(damage_backward_light);
            Damage_Animations_Light_Left.Add(damage_left_light);
            Damage_Animations_Light_Right.Add(damage_right_light);

            Damage_Animations_Medium_Forward.Add(damage_forward_medium);
            Damage_Animations_Medium_Backward.Add(damage_backward_medium);
            Damage_Animations_Medium_Left.Add(damage_left_medium);
            Damage_Animations_Medium_Right.Add(damage_right_medium);

            Damage_Animations_Heavy_Forward.Add(damage_forward_heavy);
            Damage_Animations_Heavy_Backward.Add(damage_backward_heavy);
            Damage_Animations_Heavy_Left.Add(damage_left_heavy);
            Damage_Animations_Heavy_Right.Add(damage_right_heavy);

            Damage_Animations_Colossal_Forward.Add(damage_forward_colossal);
            Damage_Animations_Colossal_Backward.Add(damage_backward_colossal);
            Damage_Animations_Colossal_Left.Add(damage_left_colossal);
            Damage_Animations_Colossal_Right.Add(damage_right_colossal);
        }

        public void PlayTargetAnimation(
            string targetAnim, 
            bool isInteracting, 
            bool canRotate = false, 
            bool mirrorAnim = false,
            bool canRoll = false)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("canRotate", canRotate);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.SetBool("isMirrored", mirrorAnim);
            character.animator.CrossFade(targetAnim, 0.2f);
            character.canRoll = canRoll;
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            character.animator.applyRootMotion = isInteracting;
            character.animator.SetBool("isRotatingWithRootMotion", true);
            character.animator.SetBool("isInteracting", isInteracting);
            character.animator.CrossFade(targetAnim, 0.2f);
        }

        public string GetRandomAnimtionFromList(List<string> animationList)
        {
            int randomValue = Random.Range(0, animationList.Count);

            return animationList[randomValue];
        }

        public virtual void CanRotate()
        {
            character.animator.SetBool("canRotate", true);
        }

        public virtual void StopRotate()
        {
            character.animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableRollCancel()
        {
            character.canRoll = true;
        }

        public virtual void EnableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            character.animator.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            character.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            character.isParrying = false;
        }

        public virtual void EnableCanbeReposted()
        {
            character.canBeRiposted = true;
        }

        public virtual void DisableCanbeReposted()
        {
            character.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage, 0, 0, 0, 0, 0);
            character.pendingCriticalDamage = 0;
        }

        public virtual void SetHandIKForWeapon
            (RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHanding)
        {
            if (isTwoHanding)
            {
                if (rightHandTarget != null)
                {
                    rightHandConstraint.data.target = rightHandTarget.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }

                if (leftHandTarget != null)
                {
                    leftHandConstraint.data.target = leftHandTarget.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
            else
            {
                rightHandConstraint.data.target = null;
                leftHandConstraint.data.target = null;
            }

            rigBuilder.Build();
        }

        public virtual void CheckHandIKWeight
            (RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
        {
            if (character.isInteracting)
                return;

            if (handIKWeightsReset)
            {
                handIKWeightsReset = false;

                if (rightHandConstraint.data.target != null)
                {
                    rightHandConstraint.data.target = rightHandIK.transform;
                    rightHandConstraint.data.targetPositionWeight = 1;
                    rightHandConstraint.data.targetRotationWeight = 1;
                }

                if (leftHandConstraint.data.target != null)
                {
                    leftHandConstraint.data.target = leftHandIK.transform;
                    leftHandConstraint.data.targetPositionWeight = 1;
                    leftHandConstraint.data.targetRotationWeight = 1;
                }
            }
        }

        public virtual void EraseHandIKForWeapon()
        {
            handIKWeightsReset = true;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.targetPositionWeight = 0;
                rightHandConstraint.data.targetRotationWeight = 0;
            }

            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.targetPositionWeight = 0;
                leftHandConstraint.data.targetRotationWeight = 0;
            }
        }

        public virtual void OnAnimatorMove()
        {
            if (character.isInteracting == false)
                return;

            Vector3 velocity = character.animator.deltaPosition;
            character.characterController.Move(velocity);
            character.transform.rotation *= character.animator.deltaRotation;
        }
    }
}