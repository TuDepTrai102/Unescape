using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT {
    public class ClassSelector : MonoBehaviour
    {
        PlayerManager player;

        [Header("CLASS INFO UI")]
        [Header("Image (Avatar)")]
        public Sprite nakedAvatar;
        public Sprite knightAvatar;
        public Sprite archerAvatar;
        public Sprite spearerAvatar;

        [Header("Text (Stats)")]
        public Text playerLevel;
        public Text healthStat;
        public Text strengthStat;
        public Text staminaStat;
        public Text focusPointStat;
        //public Text dexteriryStat;
        public Text classDrescription;

        //A SET OF STATS FOR EACH CLASS
        [Header("CLASS STARTING STATS")]
        public ClassStats[] classStats;

        //A SET OF GEAR FOR EACH CLASS
        [Header("CLASS STARTING GEAR")]
        public ClassGear[] classGear;

        //SET THE STATS AND GEAR OF EACH CLASS

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        private void AssignClassStats(int classChoosen)
        {
            //PLAYER LEVEL
            player.playerStatsManager.playerLevel = classStats[classChoosen].classLevel;
            //STRENGTH
            player.playerStatsManager.strengthLevel = classStats[classChoosen].strengthLevel;
            //STAMINA
            player.playerStatsManager.staminaLevel = classStats[classChoosen].staminaLevel;
            //HEALTH
            player.playerStatsManager.healthLevel = classStats[classChoosen].healthLevel;
            //FOCUS POINT
            player.playerStatsManager.focusLevel = classStats[classChoosen].focusPointLevel;
            //OTHER STATS IS 1 ~~
            player.playerStatsManager.poiseLevel = 1;
            player.playerStatsManager.dexterityLevel = 1;
            player.playerStatsManager.intelligenceLevel = 1;
            player.playerStatsManager.faithLevel = 1;

            classDrescription.text = classStats[classChoosen].classDescription;
        }

        public void AssignNakedClass()
        {
            //ASSIGN STATS
            AssignClassStats(0);

            if (nakedAvatar != null)
            {
                player.playerStatsManager._characterAvatar.enabled = true;
                player.playerStatsManager._characterAvatar.sprite = nakedAvatar;
            }
            else
            {
                player.playerStatsManager._characterAvatar.enabled = false;
            }


            //ASSIGN GEAR
            player.playerInventoryManager.currentHelmetEquipment = classGear[0].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGear[0].chestEquipment;
            player.playerInventoryManager.currentLegEquipment = classGear[0].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGear[0].handEquipment;

            player.playerInventoryManager.rightWeapon = classGear[0].primaryWeapon;
            player.playerInventoryManager.leftWeapon = classGear[0].offHandWeapon;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGear[0].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGear[0].offHandWeapon;

            player.playerInventoryManager.currentAmmo = classGear[0].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            playerLevel.text = player.playerStatsManager.playerLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusPointStat.text = player.playerStatsManager.focusLevel.ToString();
            //dexteriryStat.text = player.playerStatsManager.dexterityLevel.ToString();
        }

        public void AssighKnightClass()
        {
            //ASSIGN STATS
            AssignClassStats(1);

            if (knightAvatar != null)
            {
                player.playerStatsManager._characterAvatar.enabled = true;
                player.playerStatsManager._characterAvatar.sprite = knightAvatar;
            }
            else
            {
                player.playerStatsManager._characterAvatar.enabled = false;
            }

            //ASSIGN GEAR
            player.playerInventoryManager.currentHelmetEquipment = classGear[1].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGear[1].chestEquipment;
            player.playerInventoryManager.currentLegEquipment = classGear[1].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGear[1].handEquipment;

            player.playerInventoryManager.rightWeapon = classGear[1].primaryWeapon;
            player.playerInventoryManager.leftWeapon = classGear[1].offHandWeapon;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGear[1].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGear[1].offHandWeapon;

            player.playerInventoryManager.currentAmmo = classGear[1].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            playerLevel.text = player.playerStatsManager.playerLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusPointStat.text = player.playerStatsManager.focusLevel.ToString();
            //dexteriryStat.text = player.playerStatsManager.dexterityLevel.ToString();
        }

        public void AssighArcheryClass()
        {
            //ASSIGN STATS
            AssignClassStats(2);

            if (archerAvatar != null)
            {
                player.playerStatsManager._characterAvatar.enabled = true;
                player.playerStatsManager._characterAvatar.sprite = archerAvatar;
            }
            else
            {
                player.playerStatsManager._characterAvatar.enabled = false;
            }

            //ASSIGN GEAR
            player.playerInventoryManager.currentHelmetEquipment = classGear[2].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGear[2].chestEquipment;
            player.playerInventoryManager.currentLegEquipment = classGear[2].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGear[2].handEquipment;

            player.playerInventoryManager.rightWeapon = classGear[2].primaryWeapon;
            player.playerInventoryManager.leftWeapon = classGear[2].offHandWeapon;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGear[2].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGear[2].offHandWeapon;

            player.playerInventoryManager.currentAmmo = classGear[2].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            playerLevel.text = player.playerStatsManager.playerLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusPointStat.text = player.playerStatsManager.focusLevel.ToString();
            //dexteriryStat.text = player.playerStatsManager.dexterityLevel.ToString();
        }

        public void AssighSpearClass()
        {
            //ASSIGN STATS
            AssignClassStats(3);

            if (spearerAvatar != null)
            {
                player.playerStatsManager._characterAvatar.enabled = true;
                player.playerStatsManager._characterAvatar.sprite = spearerAvatar;
            }
            else
            {
                player.playerStatsManager._characterAvatar.enabled = false;
            }

            //ASSIGN GEAR
            player.playerInventoryManager.currentHelmetEquipment = classGear[3].headEquipment;
            player.playerInventoryManager.currentBodyEquipment = classGear[3].chestEquipment;
            player.playerInventoryManager.currentLegEquipment = classGear[3].legEquipment;
            player.playerInventoryManager.currentHandEquipment = classGear[3].handEquipment;

            player.playerInventoryManager.rightWeapon = classGear[3].primaryWeapon;
            player.playerInventoryManager.leftWeapon = classGear[3].offHandWeapon;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGear[3].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGear[3].offHandWeapon;

            player.playerInventoryManager.currentAmmo = classGear[3].ammoItem;

            player.playerEquipmentManager.EquipAllArmor();
            player.playerWeaponSlotManager.LoadBothWeaponOnSlots();

            playerLevel.text = player.playerStatsManager.playerLevel.ToString();
            strengthStat.text = player.playerStatsManager.strengthLevel.ToString();
            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            focusPointStat.text = player.playerStatsManager.focusLevel.ToString();
            //dexteriryStat.text = player.playerStatsManager.dexterityLevel.ToString();
        }
    }
}