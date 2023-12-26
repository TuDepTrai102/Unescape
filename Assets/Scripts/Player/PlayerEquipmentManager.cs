using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("EQUIPMENT MODEL CHANGERS")]
        //HELMET EQUIPMENT
        HelmetModelChanger helmetModelChanger;
        //TORSO EQUIPMENT
        TorsoModelChanger torsoModelChanger;
        UpperLeftArmModelChanger upperLeftArmModelChanger;
        UpperRightArmModelChanger upperRightArmModelChanger;
        LeftShoulderModelChanger leftShoulderModelChanger;
        RightShoulderModelChanger rightShoulderModelChanger;
        LeftElbowModelChanger leftElbowModelChanger;
        RightElbowModelChanger rightElbowModelChanger;
        //LEG EQUIPMENT
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        LeftKneeModelChanger leftKneeModelChanger;
        RightKneeModelChanger rightKneeModelChanger;
        //HAND EQUIPMENT
        LowerLeftArmModelChanger lowerLeftArmModelChanger;
        LowerRightArmModelChanger lowerRightArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightHandModelChanger rightHandModelChanger;

        [Header("FACIAL FEATURES")]
        public GameObject[] hairFeatures;
        public GameObject[] facialFeatures;
        public GameObject[] eyebrowsFeatures;

        [Header("DEFAULT NAKED MODELS")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;
        public string nakedHipModel;
        public string nakedLeftLeg;
        public string nakedRightLeg;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();

            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
            upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>() ;
            lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
            lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
            leftShoulderModelChanger = GetComponentInChildren<LeftShoulderModelChanger>();
            rightShoulderModelChanger = GetComponentInChildren<RightShoulderModelChanger>();
            leftElbowModelChanger = GetComponentInChildren<LeftElbowModelChanger>();
            rightElbowModelChanger = GetComponentInChildren<RightElbowModelChanger>();
            leftKneeModelChanger = GetComponentInChildren<LeftKneeModelChanger>();
            rightKneeModelChanger = GetComponentInChildren<RightKneeModelChanger>();
        }

        private void Start()
        {
            EquipAllArmor();
        }

        public void EquipAllArmor()
        {
            float poisonResistance = 0;
            float bleedResistance = 0;
            float curseResistance = 0;
            float totalEquipmentLoad = 0;

            #region HELMET EQUIPMENT

            helmetModelChanger.UnEquipAllHelmetModels();

            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                if (player.playerInventoryManager.currentHelmetEquipment.hideHairFeatures)
                {
                    foreach (var feature in hairFeatures)
                    {
                        feature.SetActive(false);
                    }
                }

                if (player.playerInventoryManager.currentHelmetEquipment.hideFacialFeatures)
                {
                    foreach (var feature in facialFeatures)
                    {
                        feature.SetActive(false);
                    }
                }

                if (player.playerInventoryManager.currentHelmetEquipment.hideEyebrowsFeatures)
                {
                    foreach (var feature in eyebrowsFeatures)
                    {
                        feature.SetActive(false);
                    }
                }

                nakedHeadModel.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);

                player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
                player.playerStatsManager.fireDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment._fireDefense;
                player.playerStatsManager._darkDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment._darkDefense;
                player.playerStatsManager._lightningDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment._lightningDefense;
                player.playerStatsManager._magicDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.magicDefense;
                player.playerStatsManager._bleedDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment._bleedDefense;

                poisonResistance += player.playerInventoryManager.currentHelmetEquipment.poisonResistance;
                bleedResistance += player.playerInventoryManager.currentHelmetEquipment._bleedResistance;
                curseResistance += player.playerInventoryManager.currentHelmetEquipment._curseResistance;
                totalEquipmentLoad += player.playerInventoryManager.currentHelmetEquipment.weight;
            }
            else
            {
                nakedHeadModel.SetActive(true);
                player.playerStatsManager.physicalDamageAbsorptionHead = 0;
                player.playerStatsManager.fireDamageAbsorptionHead = 0;
                player.playerStatsManager._darkDamageAbsorptionHead = 0;
                player.playerStatsManager._lightningDamageAbsorptionHead = 0;
                player.playerStatsManager._magicDamageAbsorptionHead = 0;
                player.playerStatsManager._bleedDamageAbsorptionHead = 0;

                foreach (var feature in hairFeatures)
                {
                    feature.SetActive(true);
                }

                foreach (var feature in facialFeatures)
                {
                    feature.SetActive(true);
                }

                foreach (var feature in eyebrowsFeatures)
                {
                    feature.SetActive(true);
                }
            }

            #endregion

            #region BODY EQUIPMENT

            torsoModelChanger.UnEquipAllTorsoModels();
            upperLeftArmModelChanger.UnEquipAllModels();
            upperRightArmModelChanger.UnEquipAllModels();
            leftShoulderModelChanger.UnEquipAllModels();
            rightShoulderModelChanger.UnEquipAllModels();
            leftElbowModelChanger.UnEquipAllModels();
            rightElbowModelChanger.UnEquipAllModels();

            if (player.playerInventoryManager.currentBodyEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
                upperLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
                upperRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
                leftShoulderModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.leftShoulderModelName);
                rightShoulderModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.rightShoulderModelName);
                leftElbowModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.leftElbowModelName);
                rightElbowModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.rightElbowModelName);

                player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
                player.playerStatsManager.fireDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment._fireDefense;
                player.playerStatsManager._darkDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment._darkDefense;
                player.playerStatsManager._lightningDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment._lightningDefense;
                player.playerStatsManager._magicDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment.magicDefense;
                player.playerStatsManager._bleedDamageAbsorptionBody = player.playerInventoryManager.currentBodyEquipment._bleedDefense;

                poisonResistance += player.playerInventoryManager.currentBodyEquipment.poisonResistance;
                bleedResistance += player.playerInventoryManager.currentBodyEquipment._bleedResistance;
                curseResistance += player.playerInventoryManager.currentBodyEquipment._curseResistance;
                totalEquipmentLoad += player.playerInventoryManager.currentBodyEquipment.weight;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
                upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
                player.playerStatsManager.physicalDamageAbsorptionBody = 0;
                player.playerStatsManager.fireDamageAbsorptionBody = 0;
                player.playerStatsManager._darkDamageAbsorptionBody = 0;
                player.playerStatsManager._lightningDamageAbsorptionBody = 0;
                player.playerStatsManager._magicDamageAbsorptionBody = 0;
                player.playerStatsManager._bleedDamageAbsorptionBody = 0;
            }

            #endregion

            #region LEG EQUIPMENT

            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();
            leftKneeModelChanger.UnEquipAllModels();
            rightKneeModelChanger.UnEquipAllModels();

            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
                leftKneeModelChanger.EquipModelByName(player.playerInventoryManager.currentLegEquipment.leftLegKneeName);
                rightKneeModelChanger.EquipModelByName(player.playerInventoryManager.currentLegEquipment.rightLegKneeName);

                player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
                player.playerStatsManager.fireDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment._fireDefense;
                player.playerStatsManager._darkDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment._darkDefense;
                player.playerStatsManager._lightningDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment._lightningDefense;
                player.playerStatsManager._magicDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.magicDefense;
                player.playerStatsManager._bleedDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment._bleedDefense;

                poisonResistance += player.playerInventoryManager.currentLegEquipment.poisonResistance;
                bleedResistance += player.playerInventoryManager.currentLegEquipment._bleedResistance;
                curseResistance += player.playerInventoryManager.currentLegEquipment._curseResistance;
                totalEquipmentLoad += player.playerInventoryManager.currentLegEquipment.weight;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
                player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
                player.playerStatsManager.fireDamageAbsorptionLegs = 0;
                player.playerStatsManager._darkDamageAbsorptionLegs = 0;
                player.playerStatsManager._lightningDamageAbsorptionLegs = 0;
                player.playerStatsManager._magicDamageAbsorptionLegs = 0;
                player.playerStatsManager._bleedDamageAbsorptionLegs = 0;
            }

            #endregion

            #region HAND EQUIPMENT

            lowerLeftArmModelChanger.UnEquipAllModels();
            lowerRightArmModelChanger.UnEquipAllModels();
            leftHandModelChanger.UnEquipAllModels();
            rightHandModelChanger.UnEquipAllModels();

            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                lowerLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                lowerRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);

                player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
                player.playerStatsManager.fireDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment._fireDefense;
                player.playerStatsManager._darkDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment._darkDefense;
                player.playerStatsManager._lightningDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment._lightningDefense;
                player.playerStatsManager._magicDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.magicDefense;
                player.playerStatsManager._bleedDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment._bleedDefense;

                poisonResistance += player.playerInventoryManager.currentHandEquipment.poisonResistance;
                bleedResistance += player.playerInventoryManager.currentHandEquipment._bleedResistance;
                curseResistance += player.playerInventoryManager.currentHandEquipment._curseResistance;
                totalEquipmentLoad += player.playerInventoryManager.currentHandEquipment.weight;
            }
            else
            {
                lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
                lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipModelByName(nakedLeftHand);
                rightHandModelChanger.EquipModelByName(nakedRightHand);
                player.playerStatsManager.physicalDamageAbsorptionHands = 0;
                player.playerStatsManager.fireDamageAbsorptionHands = 0;
                player.playerStatsManager._darkDamageAbsorptionHands = 0;
                player.playerStatsManager._lightningDamageAbsorptionHands = 0;
                player.playerStatsManager._magicDamageAbsorptionHands = 0;
                player.playerStatsManager._bleedDamageAbsorptionHands = 0;
            }

            #endregion

            player.playerStatsManager.poisonResistance = poisonResistance;
            player.playerStatsManager._bleedResistance = bleedResistance;
            player.playerStatsManager._curseResistance = curseResistance;
            player.playerStatsManager.CalculateAndSetCurrentEquipLoad(totalEquipmentLoad);
        }
    }
}