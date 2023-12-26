using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class LevelUpInteractable : Interactable
    {
        public override void Interact(PlayerManager playerManager)
        {
            playerManager.uiManager.levelUpWindow.SetActive(true);
            playerManager.interactableUIGameObject.SetActive(false);
            playerManager.itemInteractableGameObject.SetActive(false);
        }
    }
}