using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PassThroughFogWall : Interactable
    {
        WorldEventManager worldEventManager;
        public BoxCollider _collider_wall_BOSS_stage;
        public GameObject fogWall;

        protected override void Awake()
        {
            base.Awake();

            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            StartCoroutine(ActivatedBOSSFight(playerManager));
        }

        IEnumerator ActivatedBOSSFight(PlayerManager playerManager)
        {
            _collider_wall_BOSS_stage.isTrigger = true;

            playerManager.PassThroughFogWallInteraction(transform);

            //  NEED REFACTOR, WHEN PLAYER PRESS FIGHT BOSS, SO UI BOSS ACTIVE (NOT PASSTHROUGH ~.~)
            worldEventManager.ActivateBossFight();

            yield return new WaitForSeconds(0.15f);

            _collider_wall_BOSS_stage.isTrigger = false;

            playerManager.interactableUIGameObject.SetActive(false);
            playerManager.itemInteractableGameObject.SetActive(false);
        }
    }
}