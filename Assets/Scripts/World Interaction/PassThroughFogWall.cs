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

        [Header("FOGWALL INFORMATION")]
        [SerializeField] int fogWallID;
        [SerializeField] bool hasBeenPassedFogWall;

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
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld.ContainsKey(fogWallID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld.Add(fogWallID, false);
            }

            hasBeenPassedFogWall = WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld[fogWallID];

            if (hasBeenPassedFogWall)
            {
                fogWall.SetActive(false);
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
            if (WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld.ContainsKey(fogWallID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld.Remove(fogWallID);
            }

            //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
            WorldSaveGameManager.instance.currentCharacterSaveData.fogWallInWorld.Add(fogWallID, true);

            hasBeenPassedFogWall = true;

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

            Destroy(this);
        }
    }
}