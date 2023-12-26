using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterManager spellTarget;
        Rigidbody rigidBody;

        Vector3 impactNormal;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.enabled = enableDamageColliderOnStartUp;
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            projectileParticles = Instantiate
                (projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles)
            {
                muzzleParticles = Instantiate
                    (muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                spellTarget = collision.transform.GetComponent<CharacterManager>();

                if (spellTarget != null && spellTarget.characterStatsManager.teamIDNumber != teamIDNumber)
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
                    spellTarget.characterEffectsManager.ProcessEffectInstantly(takeDamageEffect);
                }

                hasCollided = true;
                impactParticles = Instantiate
                    (impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 5f);
                Destroy(gameObject);
            }
        }
    }
}