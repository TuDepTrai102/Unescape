using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _RingItemPickUp : Interactable
    {
        //THIS IS UNIQUE ID FOR THIS ITEM SPAWN IN THE GAME WORLD,
        //EACH ITEM YOU PLACE IN YOUR WORLD SHOULD HAVE IT'S OWN UNIQUE ID
        [Header("ITEM INFORMATION")]
        [SerializeField] int itemPickUpID;
        [SerializeField] bool hasBeenLooted;

        [Header("ITEM")]
        public RingItem ring;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            //IF THE SAVE DATA DOES NOT CONTAIN THIS ITEM, WE MUST HAVE NEVER LOOTED IT,
            //SO WE ADD IT TO THE LIST AND LIST IT AS NOT LOOTED
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld.Add(itemPickUpID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld[itemPickUpID];

            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
            if (WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld.Remove(itemPickUpID);
            }

            //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
            WorldSaveGameManager.instance.currentCharacterSaveData.itemInWorld.Add(itemPickUpID, true);

            hasBeenLooted = true;

            //PLACE THE ITEM IN THE PLAYER INVENTORY
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventoryManager playerInventoryManager;
            PlayerLocomotionManager playerLocomotionManager;
            PlayerAnimatorManager playerAnimatorManager;

            playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
            playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = playerManager.GetComponent<PlayerAnimatorManager>();

            playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerAnimatorManager.PlayTargetAnimation(playerManager.characterAnimatorManager.animation_pick_up_item, true);

            if (ring != null)
            {
                playerInventoryManager._ringsInventory.Add(ring);
                playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = ring.itemName;
                playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = ring.itemIcon.texture;
                playerManager.itemInteractableGameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}