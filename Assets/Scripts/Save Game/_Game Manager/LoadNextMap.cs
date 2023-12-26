using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NT
{
    public class LoadNextMap : Interactable
    {
        PlayerManager player;

        public bool map2;
        public bool map2_back;

        public bool map3;
        public bool map3_back;

        public bool map4;
        public bool map4_back;

        public bool map5;
        public bool map5_back;

        protected override void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        protected override void Start()
        {

        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            if (map2)
            {
                player._worldSaveGameManager.LoadMap2();
            }

            if (map2_back)
            {
                player._worldSaveGameManager.LoadMap2_back();
            }

            if (map3)
            {
                player._worldSaveGameManager.LoadMap3();
            }

            if (map3_back)
            {
                player._worldSaveGameManager.LoadMap3_back();
            }

            if (map4)
            {
                player._worldSaveGameManager.LoadMap4();
            }

            if (map4_back)
            {
                player._worldSaveGameManager.LoadMap4_back();
            }

            if (map5)
            {
                player._worldSaveGameManager.LoadMap5();
            }

            if (map5_back)
            {
                player._worldSaveGameManager.LoadMap5_back();
            }
        }
    }
}