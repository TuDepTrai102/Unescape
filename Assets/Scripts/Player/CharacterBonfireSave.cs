using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class CharacterBonfireSave : MonoBehaviour
    {
        PlayerManager player;
        public BonfireInteractable interactableBonfire;
        public _BonfireTeleportTransform bonfireTeleportTransform;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        public void SaveBonfireDataToCurrentBonfireSaveData(ref BonfireActivatedData currentBonfireData)
        {
            if (currentBonfireData == null)
                return;

            if (interactableBonfire.bonfire1)
            {
                currentBonfireData.xPosition_bonfire1 = bonfireTeleportTransform.transform.position.x;
                currentBonfireData.yPosition_bonfire1 = bonfireTeleportTransform.transform.position.y;
                currentBonfireData.zPosition_bonfire1 = bonfireTeleportTransform.transform.position.z;

                currentBonfireData.bonfire1_Saved_x = currentBonfireData.xPosition_bonfire1;
                currentBonfireData.bonfire1_Saved_y = currentBonfireData.yPosition_bonfire1;
                currentBonfireData.bonfire1_Saved_z = currentBonfireData.zPosition_bonfire1;
            }

            if (interactableBonfire.bonfire2)
            {
                currentBonfireData.xPosition_bonfire2 = bonfireTeleportTransform.transform.position.x;
                currentBonfireData.yPosition_bonfire2 = bonfireTeleportTransform.transform.position.y;
                currentBonfireData.zPosition_bonfire2 = bonfireTeleportTransform.transform.position.z;

                currentBonfireData.bonfire2_Saved_x = currentBonfireData.xPosition_bonfire2;
                currentBonfireData.bonfire2_Saved_y = currentBonfireData.yPosition_bonfire2;
                currentBonfireData.bonfire2_Saved_z = currentBonfireData.zPosition_bonfire2;
            }

            if (interactableBonfire.bonfire3)
            {
                currentBonfireData.xPosition_bonfire3 = bonfireTeleportTransform.transform.position.x;
                currentBonfireData.yPosition_bonfire3 = bonfireTeleportTransform.transform.position.y;
                currentBonfireData.zPosition_bonfire3 = bonfireTeleportTransform.transform.position.z;

                currentBonfireData.bonfire3_Saved_x = currentBonfireData.xPosition_bonfire3;
                currentBonfireData.bonfire3_Saved_y = currentBonfireData.yPosition_bonfire3;
                currentBonfireData.bonfire3_Saved_z = currentBonfireData.zPosition_bonfire3;
            }

            if (interactableBonfire.bonfire4)
            {
                currentBonfireData.xPosition_bonfire4 = bonfireTeleportTransform.transform.position.x;
                currentBonfireData.yPosition_bonfire4 = bonfireTeleportTransform.transform.position.y;
                currentBonfireData.zPosition_bonfire4 = bonfireTeleportTransform.transform.position.z;

                currentBonfireData.bonfire4_Saved_x = currentBonfireData.xPosition_bonfire4;
                currentBonfireData.bonfire4_Saved_y = currentBonfireData.yPosition_bonfire4;
                currentBonfireData.bonfire4_Saved_z = currentBonfireData.zPosition_bonfire4;
            }

            if (interactableBonfire.bonfire5)
            {
                currentBonfireData.xPosition_bonfire5 = bonfireTeleportTransform.transform.position.x;
                currentBonfireData.yPosition_bonfire5 = bonfireTeleportTransform.transform.position.y;
                currentBonfireData.zPosition_bonfire5 = bonfireTeleportTransform.transform.position.z;

                currentBonfireData.bonfire5_Saved_x = currentBonfireData.xPosition_bonfire5;
                currentBonfireData.bonfire5_Saved_y = currentBonfireData.yPosition_bonfire5;
                currentBonfireData.bonfire5_Saved_z = currentBonfireData.zPosition_bonfire5;
            }
        }

        public void LoadBonfireDataToCurrentBonfireSaveData(ref BonfireActivatedData currentBonfireData)
        {
            if (currentBonfireData == null)
                return;

            currentBonfireData.bonfire1_Saved_x = currentBonfireData.xPosition_bonfire1;
            currentBonfireData.bonfire1_Saved_y = currentBonfireData.yPosition_bonfire1;
            currentBonfireData.bonfire1_Saved_z = currentBonfireData.zPosition_bonfire1;

            currentBonfireData.bonfire2_Saved_x = currentBonfireData.xPosition_bonfire2;
            currentBonfireData.bonfire2_Saved_y = currentBonfireData.yPosition_bonfire2;
            currentBonfireData.bonfire2_Saved_z = currentBonfireData.zPosition_bonfire2;

            currentBonfireData.bonfire3_Saved_x = currentBonfireData.xPosition_bonfire3;
            currentBonfireData.bonfire3_Saved_y = currentBonfireData.yPosition_bonfire3;
            currentBonfireData.bonfire3_Saved_z = currentBonfireData.zPosition_bonfire3;

            currentBonfireData.bonfire4_Saved_x = currentBonfireData.xPosition_bonfire4;
            currentBonfireData.bonfire4_Saved_y = currentBonfireData.yPosition_bonfire4;
            currentBonfireData.bonfire4_Saved_z = currentBonfireData.zPosition_bonfire4;

            currentBonfireData.bonfire5_Saved_x = currentBonfireData.xPosition_bonfire5;
            currentBonfireData.bonfire5_Saved_y = currentBonfireData.yPosition_bonfire5;
            currentBonfireData.bonfire5_Saved_z = currentBonfireData.zPosition_bonfire5;
        }

        public void LoadBonfire_01_Data(ref BonfireActivatedData bonfire1)
        {
            Vector3 loadPosition = new Vector3(
                bonfire1.xPosition_bonfire1,
                bonfire1.yPosition_bonfire1,
                bonfire1.zPosition_bonfire1);

            player.characterController.Move(loadPosition - player.transform.position);
        }

        public void LoadBonfire_02_Data(ref BonfireActivatedData bonfire2)
        {
            Vector3 loadPosition = new Vector3(
                bonfire2.xPosition_bonfire2,
                bonfire2.yPosition_bonfire2,
                bonfire2.zPosition_bonfire2);
            player.characterController.Move(loadPosition - player.transform.position);
        }

        public void LoadBonfire_03_Data(ref BonfireActivatedData bonfire3)
        {
            Vector3 loadPosition = new Vector3(
                bonfire3.xPosition_bonfire3,
                bonfire3.yPosition_bonfire3,
                bonfire3.zPosition_bonfire3);
            player.characterController.Move(loadPosition - player.transform.position);
        }

        public void LoadBonfire_04_Data(ref BonfireActivatedData bonfire4)
        {
            Vector3 loadPosition = new Vector3(
                bonfire4.xPosition_bonfire4,
                bonfire4.yPosition_bonfire4,
                bonfire4.zPosition_bonfire4);
            player.characterController.Move(loadPosition - player.transform.position);
        }

        public void LoadBonfire_05_Data(ref BonfireActivatedData bonfire5)
        {
            Vector3 loadPosition = new Vector3(
                bonfire5.xPosition_bonfire5,
                bonfire5.yPosition_bonfire5,
                bonfire5.zPosition_bonfire5);
            player.characterController.Move(loadPosition - player.transform.position);
        }
    }
}