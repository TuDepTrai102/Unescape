using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            if (!character.isTwoHandingWeapon)
                return;

            //CREATE THE LIVE ARROW INSTANTIATION LOCATION
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            //ANIMATE THE BOW FIRING THE ARROW
            Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_ONLY_Fire_01");
            Destroy(character.characterEffectsManager.instantFXModel); //DESTROY THE LOADED MODEL

            //RESET THE PLAYER HOLDING ARROW FLAG
            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.th_bow_fire_01, true);
            character.animator.SetBool("isHoldingArrow", false);

            //FIRE THE ARROW AS A PLAYER CHARACTER
            if (player != null)
            {
                //CREATE AND FIRE THE LIVE ARROW
                GameObject liveArrow = Instantiate
                    (player.playerInventoryManager.currentAmmo.liveItemModel,
                    arrowInstantiationLocation.transform.position,
                    player.cameraHandler.cameraPivotTransform.rotation);
                Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                if (character.isAiming)
                {
                    Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hitPoint;

                    if (Physics.Raycast(ray, out hitPoint, 100.0f))
                    {
                        liveArrow.transform.LookAt(hitPoint.point);
                        Debug.Log(hitPoint.transform.name);
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler
                            (player.cameraHandler.cameraTransform.localEulerAngles.x,
                            player.lockOnTransform.eulerAngles.y, 0);
                    }
                }
                else
                {
                    //GIVE AMMO VELOCITY
                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        //SINCE WHILE LOCKED WE ARE ALWAYS FACING OUR TARGET WE CAN COPY OUR
                        //FACING DIRECTION TO OUR ARROWS FACING DIRECTION WHEN FIRED
                        Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position
                            - liveArrow.gameObject.transform.position);
                        liveArrow.transform.rotation = arrowRotation;
                    }
                    else
                    {
                        liveArrow.transform.rotation = Quaternion.Euler
                            (player.cameraHandler.cameraPivotTransform.eulerAngles.x,
                            player.lockOnTransform.eulerAngles.y, 0);
                    }
                }

                rigidBody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
                rigidBody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity);
                rigidBody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
                rigidBody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                //SET LIVE ARROW DAMAGE
                damageCollider.characterManager = character;
                damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
                damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
            }
            //FIRE THE ARROW AS AN A.I CHARACTER
            else
            {
                AICharacterManager enemy = character as AICharacterManager;

                //CREATE AND FIRE THE LIVE ARROW
                GameObject liveArrow = Instantiate
                    (enemy.characterInventoryManager.currentAmmo.liveItemModel,
                    arrowInstantiationLocation.transform.position,
                    Quaternion.identity);
                Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                //GIVE AMMO VELOCITY
                if (enemy.currentTarget != null)
                {
                    //SINCE WHILE LOCKED WE ARE ALWAYS FACING OUR TARGET WE CAN COPY OUR
                    //FACING DIRECTION TO OUR ARROWS FACING DIRECTION WHEN FIRED
                    Quaternion arrowRotation = Quaternion.LookRotation
                        (enemy.currentTarget.lockOnTransform.position
                        - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }

                rigidBody.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo.forwardVelocity);
                rigidBody.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo.upwardVelocity);
                rigidBody.useGravity = enemy.characterInventoryManager.currentAmmo.useGravity;
                rigidBody.mass = enemy.characterInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                //SET LIVE ARROW DAMAGE
                damageCollider.characterManager = character;
                damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
                damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physicalDamage;
                damageCollider.teamIDNumber = enemy.characterStatsManager.teamIDNumber;
            }
        }
    }
} 