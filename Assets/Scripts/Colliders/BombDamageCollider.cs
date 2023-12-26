using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage & Radius")]
        public int eplosiveRadius = 1;
        public int explosionDamage;
        public int explosionSplashDamage;
        // magic exp damage
        // lightning exp damage,etc etc

        public Rigidbody bombRigidBody;
        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                
                Explode();

                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, eplosiveRadius);

            foreach (Collider objectsInExplosion in characters)
            {
                CharacterManager character = objectsInExplosion.GetComponent<CharacterManager>();

                if (character != null)
                {
                    if (character.characterStatsManager.teamIDNumber != teamIDNumber)
                    {
                        TakeDamageEffect takeDamageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                        takeDamageEffect.physicalDamage = physicalDamage;
                        takeDamageEffect.fireDamage = fireDamage;
                        takeDamageEffect._lightningDamage = _lightningDamage;
                        takeDamageEffect._darkDamage = _darkDamage;
                        takeDamageEffect._magicDamage = _magicDamage;
                        takeDamageEffect._bleedDamage = _bleedDamage;
                        takeDamageEffect.poiseDamage = poiseDamage;
                        takeDamageEffect.contactPoint = contactPoint;
                        takeDamageEffect.angleHitFrom = angleHitFrom;
                        character.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
                    }
                }
            }
        }
    }
}
