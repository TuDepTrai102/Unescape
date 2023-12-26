using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class _BonfireTeleportUI : MonoBehaviour
    {
        public WorldSaveBonfire worldSaveBonfire;

        [Header("BONFIRE TELEPORT UI")]
        public GameObject bonfireMenu_UI;
        public GameObject hudWindow;

        private void Awake()
        {
            worldSaveBonfire = FindObjectOfType<WorldSaveBonfire>();
        }

        public void TeleportMap1()
        {
            if (worldSaveBonfire.currentBonfireSaveData.bonfire1_Saved_x != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire1_Saved_y != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire1_Saved_z != 0)
            {
                worldSaveBonfire.LoadBonfire_01();
            }
        }

        public void TeleportMap2()
        {
            if (worldSaveBonfire.currentBonfireSaveData.bonfire2_Saved_x != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire2_Saved_y != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire2_Saved_z != 0)
            {
                worldSaveBonfire.LoadBonfire_02();
            }
        }

        public void TeleportMap3()
        {
            if (worldSaveBonfire.currentBonfireSaveData.bonfire3_Saved_x != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire3_Saved_y != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire3_Saved_z != 0)
            {
                worldSaveBonfire.LoadBonfire_03();
            }
        }

        public void TeleportMap4()
        {
            if (worldSaveBonfire.currentBonfireSaveData.bonfire4_Saved_x != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire4_Saved_y != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire4_Saved_z != 0)
            {
                worldSaveBonfire.LoadBonfire_04();
            }
        }

        public void TeleportMap5()
        {
            if (worldSaveBonfire.currentBonfireSaveData.bonfire5_Saved_x != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire5_Saved_y != 0 &&
                worldSaveBonfire.currentBonfireSaveData.bonfire5_Saved_z != 0)
            {
                worldSaveBonfire.LoadBonfire_05();
            }
        }
    }
}