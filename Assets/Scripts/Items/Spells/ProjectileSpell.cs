using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;

        Rigidbody rigidBody;

        public override void AttemptToCashSpell(CharacterManager character)
        {
            base.AttemptToCashSpell(character);

            if (character.isUsingLeftHand)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate
                    (spellWarmUpFX, character.characterWeaponSlotManager.leftHandSlot.transform);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
            else
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate
                    (spellWarmUpFX, character.characterWeaponSlotManager.rightHandSlot.transform);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
        }

        public override void SuccessfullyCashSpell(CharacterManager character)
        {
            base.SuccessfullyCashSpell(character);

            PlayerManager player = character as PlayerManager;

            //HANDLE THE PROCESS IF THE CASTER IS A PLAYER
            if (player != null)
            {
                if (character.isUsingLeftHand)
                {
                    GameObject instantiatedSpellFX = Instantiate
                        (spellCastFX,
                        player.playerWeaponSlotManager.leftHandSlot.transform.position,
                        player.cameraHandler.cameraPivotTransform.rotation);

                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                    rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();


                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler
                            (player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                    }

                    rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                    rigidBody.useGravity = isEffectedByGravity;
                    rigidBody.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                    //add an instantiate location on the caster weapon itself
                }
                else
                {
                    GameObject instantiatedSpellFX = Instantiate
                        (spellCastFX,
                        player.playerWeaponSlotManager.rightHandSlot.transform.position,
                        player.cameraHandler.cameraPivotTransform.rotation);

                    SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                    spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                    rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();


                    if (player.cameraHandler.currentLockOnTarget != null)
                    {
                        instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                    }
                    else
                    {
                        instantiatedSpellFX.transform.rotation = Quaternion.Euler
                            (player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                    }

                    rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                    rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                    rigidBody.useGravity = isEffectedByGravity;
                    rigidBody.mass = projectileMass;
                    instantiatedSpellFX.transform.parent = null;
                }
            }
            //HANDLE THE PROCESS IF THE CASTER IS AN A.I
            else
            {

            }
        }
    }
}