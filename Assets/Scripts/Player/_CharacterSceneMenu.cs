using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace NT
{
    public class _CharacterSceneMenu : MonoBehaviour
    {
        public PlayerManager playerManager;
        public _WorldSaveStartScreen worldSaveStartScreen;
        public NameCharacter nameCharacter;
        public GameObject textFieldName;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            worldSaveStartScreen = FindObjectOfType<_WorldSaveStartScreen>();
            nameCharacter = FindObjectOfType<NameCharacter>();

            _WorldSaveStartScreen.instance.player_Start = this;
        }

        private void Start()
        {
            
        }

        public void SaveCharacterDataToCurrentSaveData_MENU(ref _CharacterStartScreenData currentCharacterSceneData)
        {
            if (currentCharacterSceneData == null)
                return;

            currentCharacterSceneData.characterName = playerManager.playerStatsManager.characterName;

            //EQUIPMENT
            currentCharacterSceneData.currentRightHandWeaponID = playerManager.playerInventoryManager.rightWeapon.itemID;
            currentCharacterSceneData.currentLeftHandWeaponID = playerManager.playerInventoryManager.leftWeapon.itemID;

            if (playerManager.playerInventoryManager.currentHelmetEquipment != null)
            {
                currentCharacterSceneData.currentHeadGearItemID = playerManager.playerInventoryManager.currentHelmetEquipment.itemID;
            }
            else
            {
                currentCharacterSceneData.currentHeadGearItemID = -1;
            }

            if (playerManager.playerInventoryManager.currentBodyEquipment != null)
            {
                currentCharacterSceneData.currentChestGearItemID = playerManager.playerInventoryManager.currentBodyEquipment.itemID;
            }
            else
            {
                currentCharacterSceneData.currentChestGearItemID = -1;
            }

            if (playerManager.playerInventoryManager.currentLegEquipment != null)
            {
                currentCharacterSceneData.currentLegGearItemID = playerManager.playerInventoryManager.currentLegEquipment.itemID;
            }
            else
            {
                currentCharacterSceneData.currentLegGearItemID = -1;
            }

            if (playerManager.playerInventoryManager.currentHandEquipment != null)
            {
                currentCharacterSceneData.currentHandGearItemID = playerManager.playerInventoryManager.currentHandEquipment.itemID;
            }
            else
            {
                currentCharacterSceneData.currentHandGearItemID = -1;
            }
        }

        public void LoadCharacterDataFromCurrentCharacterSaveData_MENU(ref _CharacterStartScreenData currentCharacterSceneData)
        {
            textFieldName.SetActive(false);

            if (currentCharacterSceneData == null)
                return;

            if (nameCharacter != null)
            {
                if (currentCharacterSceneData != null)
                {
                    nameCharacter.nameButtonText.text = currentCharacterSceneData.characterName;
                }
                else
                {
                    nameCharacter.nameButtonText.text = "Nameless";
                }
            }

            //EQUIPMENT
            if (currentCharacterSceneData.currentRightHandWeaponID != 0)
            {
                playerManager.playerInventoryManager.rightWeapon = WorldItemDataBase.Instance.GetWeaponItemByID
                    (currentCharacterSceneData.currentRightHandWeaponID);
            }

            if (currentCharacterSceneData.currentLeftHandWeaponID != 0)
            {
                playerManager.playerInventoryManager.leftWeapon = WorldItemDataBase.Instance.GetWeaponItemByID
                    (currentCharacterSceneData.currentLeftHandWeaponID);
            }

            playerManager.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            #region GEARS

            if (currentCharacterSceneData.currentHeadGearItemID != 0)
            {
                EquipmentItem headEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                    (currentCharacterSceneData.currentHeadGearItemID);

                if (headEquipment != null)
                {
                    playerManager.playerInventoryManager.currentHelmetEquipment = headEquipment as HelmetEquipment;
                    playerManager.uiManager.equipmentWindowUI.headEquipmentSlotsUI.AddItem(playerManager.playerInventoryManager.currentHelmetEquipment);
                }
            }

            if (currentCharacterSceneData.currentChestGearItemID != 0)
            {
                EquipmentItem bodyEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                    (currentCharacterSceneData.currentChestGearItemID);

                if (bodyEquipment != null)
                {
                    playerManager.playerInventoryManager.currentBodyEquipment = bodyEquipment as BodyEquipment;
                    playerManager.uiManager.equipmentWindowUI.bodyEquipmentSlotsUI.AddItem(playerManager.playerInventoryManager.currentBodyEquipment);
                }
            }

            if (currentCharacterSceneData.currentLegGearItemID != 0)
            {
                EquipmentItem legEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                    (currentCharacterSceneData.currentLegGearItemID);

                if (legEquipment != null)
                {
                    playerManager.playerInventoryManager.currentLegEquipment = legEquipment as LegEquipment;
                    playerManager.uiManager.equipmentWindowUI.legEquipmentSlotsUI.AddItem(playerManager.playerInventoryManager.currentLegEquipment);
                }
            }

            if (currentCharacterSceneData.currentHandGearItemID != 0)
            {
                EquipmentItem handEquipment = WorldItemDataBase.Instance.GetEquipmentItemByID
                    (currentCharacterSceneData.currentHandGearItemID);

                if (handEquipment != null)
                {
                    playerManager.playerInventoryManager.currentHandEquipment = handEquipment as HandEquipment;
                    playerManager.uiManager.equipmentWindowUI.handEquipmentSlotsUI.AddItem(playerManager.playerInventoryManager.currentHandEquipment);
                }
            }

            playerManager.playerEquipmentManager.EquipAllArmor();

            #endregion
        }
    }
}