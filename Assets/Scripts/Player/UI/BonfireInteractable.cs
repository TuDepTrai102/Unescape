using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class BonfireInteractable : Interactable
    {
        public WorldSaveBonfire saveBonfire;
        public WorldSaveGameManager gameManager;
        _BonfireTeleportUI _bonfireTeleportUI;
        public CharacterBonfireSave characterBonfireSave;

        //LOCATION OF BONFIRE (FOR TELEPORTING)
        [Header("BONFIRE TELEPORT TRANSFORM")]
        public Transform bonfireTeleportTransform;

        [Header("ACTIVATION STATUS")]
        public bool hasBeenActivated;
        public int bonfireID;

        //BONFIRE UNIQUE ID (FOR SAVING WHICH BONFIRES YOU HAVE ACTIVATED)
        [Header("BONFIRE BOOLS")]
        public bool bonfire1;
        public bool bonfire2;
        public bool bonfire3;
        public bool bonfire4;
        public bool bonfire5;

        [Header("BONFIRE FX")]
        public ParticleSystem activationFX;
        public ParticleSystem fireFX;
        public AudioClip bonfireActivationSoundFX;

        AudioSource audioSource;

        protected override void Awake()
        {
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld.ContainsKey(bonfireID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld.Add(bonfireID, false);
            }

            hasBeenActivated = WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld[bonfireID];

            //IF THE BONFIRE HAS ALREADY BEEN ACTIVATED BY PLAYER, PLAY "FIRE FX" WHEN THE BONFIRE LOADED ON THE SCENE 
            if (hasBeenActivated)
            {
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                interactableText = "REST";
            }
            else
            {
                interactableText = "LIGHT BONFIRE";
            }

            audioSource = GetComponent<AudioSource>();
            _bonfireTeleportUI = FindObjectOfType<_BonfireTeleportUI>();
            saveBonfire = FindObjectOfType<WorldSaveBonfire>();
            gameManager = FindObjectOfType<WorldSaveGameManager>();
            characterBonfireSave = FindObjectOfType<CharacterBonfireSave>();
        }

        protected override void Start()
        {
            characterBonfireSave.bonfireTeleportTransform = FindObjectOfType<_BonfireTeleportTransform>();
            characterBonfireSave.interactableBonfire = FindObjectOfType<BonfireInteractable>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            if (hasBeenActivated)
            {
                _bonfireTeleportUI.bonfireMenu_UI.SetActive(true);
                _bonfireTeleportUI.hudWindow.SetActive(false);
            }
            else
            {
                //ACTIVATED BONFIRE
                playerManager.playerAnimatorManager.PlayTargetAnimation(playerManager.playerAnimatorManager.animation_bonfire_activate, true);
                playerManager.uiManager.ActivateBonfirePopUp();

                //NOTIFY THE CHARACTER DATA THIS ITEM HAS BEEN LOOTED FROM THE WORLD, SO IT DOES NOT SPAWN AGAIN
                if (WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld.ContainsKey(bonfireID))
                {
                    WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld.Remove(bonfireID);
                }

                //SAVES THE PICK UP TO OUR SAVE DATA SO IT DOES NOT SPAWN AGAIN WHEN WE RE-LOAD THE AREA
                WorldSaveGameManager.instance.currentCharacterSaveData.bonfireInWorld.Add(bonfireID, true);

                hasBeenActivated = true;
                interactableText = "REST";
                activationFX.gameObject.SetActive(true);
                activationFX.Play();
                saveBonfire.SaveBonfire();
                gameManager.SaveGame();
                fireFX.gameObject.SetActive(true);
                fireFX.Play();
                audioSource.PlayOneShot(bonfireActivationSoundFX);
            }
        }
    }
}