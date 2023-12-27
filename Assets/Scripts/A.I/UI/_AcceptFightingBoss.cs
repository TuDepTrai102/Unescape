using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _AcceptFightingBoss : Interactable
    {
        public WorldEventManager worldEventManager;
        public bool _acceptFightingBool;

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

            StartCoroutine(_BossFightInteract(playerManager));
        }

        public IEnumerator _BossFightInteract(PlayerManager playerManager)
        {
            _acceptFightingBool = true;

            worldEventManager.bossHasBeenAwakened = true;

            yield return new WaitForSeconds(0.35f);

            worldEventManager.StartBossMusic();

            if (worldEventManager.bossHasBeenAwakened)
            {
                worldEventManager.bossHealthBar.SetUIHealthBarToActive();
            }

            playerManager.interactableUIGameObject.SetActive(false);
            playerManager.itemInteractableGameObject.SetActive(false);
        }
    }
}