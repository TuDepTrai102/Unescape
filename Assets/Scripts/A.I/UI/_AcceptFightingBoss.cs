using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _AcceptFightingBoss : Interactable
    {
        public WorldEventManager worldEventManager;
        public GameObject theBOSS_prebab;
        public GameObject theBOSS_collider;

        public bool _acceptFightingBool;

        protected override void Awake()
        {
            base.Awake();

            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        protected override void Start()
        {
            base.Start();

            //IF THE SAVE DATA DOES NOT CONTAIN THIS ITEM, WE MUST HAVE NEVER LOOTED IT,
            //SO WE ADD IT TO THE LIST AND LIST IT AS NOT LOOTED
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.ContainsKey(worldEventManager.bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Add(worldEventManager.bossID, false);
            }

            worldEventManager.hasBeenDefeatedBOSS = WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld[worldEventManager.bossID];

            if (worldEventManager.hasBeenDefeatedBOSS)
            {
                theBOSS_prebab.SetActive(false);
                theBOSS_collider.SetActive(false);
                worldEventManager.BossHasBeenDefeated();
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
            if (WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.ContainsKey(worldEventManager.bossID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Remove(worldEventManager.bossID);
            }

            //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
            WorldSaveGameManager.instance.currentCharacterSaveData.bossInWorld.Add(worldEventManager.bossID, true);

            worldEventManager.hasBeenDefeatedBOSS = true;

            StartCoroutine(_BossFightInteract(playerManager));
        }

        public IEnumerator _BossFightInteract(PlayerManager playerManager)
        {
            _acceptFightingBool = true;
            worldEventManager.bossHasBeenAwakened = true;

            yield return new WaitForSeconds(0.35f);

            worldEventManager.StartBossMusic();
            playerManager.interactableUIGameObject.SetActive(false);

            if (worldEventManager.bossHasBeenAwakened)
            {
                worldEventManager.bossHealthBar.SetUIHealthBarToActive();
            }

            Destroy(this);
        }
    }
}