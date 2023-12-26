using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT
{
    public class EndGameInteractable : Interactable
    {
        public PlayerManager playerManager;

        protected override void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            playerManager._worldSaveGameManager.LoadEndingMap();
        }
    }
}