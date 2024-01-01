using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NT
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public PlayerManager player;
        public ItemStatsWindowUI itemStatsWindowUI;
        public EquipmentWindowUI equipmentWindowUI;
        public QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public GameObject crossHair;
        public Text soulCount;

        [Header("UI WINDOWS")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject _quickChangeSpellWindow;
        public GameObject _consumableInventoryWindow;
        public GameObject weaponInventoryWindow;
        public GameObject _headInventoryWindow;
        public GameObject _bodyInventoryWindow;
        public GameObject _legInventoryWindow;
        public GameObject _handInventoryWindow;
        public GameObject _ringInventoryWindow;
        public GameObject itemStatsWindow;
        public GameObject levelUpWindow;
        public GameObject _skillTreeWindow;
        public GameObject _playerInforWindow;
        public GameObject _OptionsWindow;
        public GameObject _NOTE_BoardWindow;
        public GameObject _bonfireTeleportWindow;
        public GameObject repsawnWindow;
        public GameObject shopWindow;
        public GameObject endingWindow;

        [Header("EQUIPMENT WINDOW SLOT SELECTED")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        public bool headEquipmentSlotSelected;
        public bool bodyEquipmentSlotSelected;
        public bool legEquipmentSlotSelected;
        public bool handEquipmentSlotSelected;

        public bool _consumableSlotSelected;
        public bool _spellSlotSelected;

        public bool _ringSlot01Selected;
        public bool _ringSlot02Selected;
        public bool _ringSlot03Selected;
        public bool _ringSlot04Selected;

        [Header("POP UPS")]
        BonfireLitPopUpUI bonfireLitPopUpUI;

        [Header("WEAPON INVENTORY")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        public WeaponInventorySlot[] weaponInventorySlots;

        [Header("HEAD EQUIPMENT INVENTORY")]
        public GameObject headEquipmentInventorySlotPrefab;
        public Transform headEquipmentInventorySlotsParent;
        public HeadEquipmentInventorySlot[] headEquipmentInventorySlots;

        [Header("BODY EQUIPMENT INVENTORY")]
        public GameObject bodyEquipmentInventorySlotPrefab;
        public Transform bodyEquipmentInventorySlotsParent;
        public BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;

        [Header("LEG EQUIPMENT INVENTORY")]
        public GameObject legEquipmentInventorySlotPrefab;
        public Transform legEquipmentInventorySlotsParent;
        public LegEquipmentInventorySlot[] legEquipmentInventorySlots;

        [Header("HAND EQUIPMENT INVENTORY")]
        public GameObject handEquipmentInventorySlotPrefab;
        public Transform handEquipmentInventorySlotsParent;
        public HandEquipmentInventorySlot[] handEquipmentInventorySlots;

        [Header("CONSUMABLE EQUIPMENT INVENTORY")]
        public GameObject _consumableEquipmentInventorySlotPrefab;
        public Transform _consumableEquipmentInventorySlotsParent;
        public _ConsumableInventorySlot[] _consumableEquipmentInventorySlots;

        [Header("RING EQUIPMENT INVENTORY")]
        public GameObject _ringEquipmentInventorySlotPrefab;
        public Transform _ringEquipmentInventorySlotsParent;
        public _RingInventorySlot[] _ringEquipmentInventorySlots;

        private void Awake()
        {
            //if (instance == null)
            //{
            //    instance = this;
            //}
            //else
            //{
            //    Destroy(gameObject);
            //}

            player = FindObjectOfType<PlayerManager>();

            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();

            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();

            headEquipmentInventorySlots = 
                headEquipmentInventorySlotsParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            bodyEquipmentInventorySlots = 
                bodyEquipmentInventorySlotsParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            legEquipmentInventorySlots = 
                legEquipmentInventorySlotsParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            handEquipmentInventorySlots = 
                handEquipmentInventorySlotsParent.GetComponentsInChildren<HandEquipmentInventorySlot>();

            _consumableEquipmentInventorySlots = _consumableEquipmentInventorySlotsParent.GetComponentsInChildren<_ConsumableInventorySlot>();
            _ringEquipmentInventorySlots = _ringEquipmentInventorySlotsParent.GetComponentsInChildren<_RingInventorySlot>();

            bonfireLitPopUpUI = GetComponentInChildren<BonfireLitPopUpUI>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            equipmentWindowUI.LoadWeaponsOnEquipmentScreen(player.playerInventoryManager);
            equipmentWindowUI.LoadArmorOnEquipmentScreen(player.playerInventoryManager);

            if (player.playerInventoryManager.currentSpell != null)
            {
                quickSlotsUI.UpdateCurrentSpellIcon(player.playerInventoryManager.currentSpell);
            }

            if (player.playerInventoryManager.currentConsumable != null)
            {
                quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
            }

            soulCount.text = player.playerStatsManager.currentSoulCount.ToString();
        }

        public void UpdateUI()
        {
            //WEAPON INVENTORY SLOTS
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < player.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }

                    weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            //HEAD EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < headEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.headEquipmentInventory.Count)
                {
                    if (headEquipmentInventorySlots.Length < 
                        player.playerInventoryManager.headEquipmentInventory.Count)
                    {
                        Instantiate(headEquipmentInventorySlotsParent, headEquipmentInventorySlotsParent);
                        headEquipmentInventorySlots = headEquipmentInventorySlotsParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                    }

                    headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
                }
                else
                {
                    headEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //BODY EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
                {
                    if (headEquipmentInventorySlots.Length <
                        player.playerInventoryManager.bodyEquipmentInventory.Count)
                    {
                        Instantiate(bodyEquipmentInventorySlotsParent, bodyEquipmentInventorySlotsParent);
                        bodyEquipmentInventorySlots = bodyEquipmentInventorySlotsParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                    }

                    bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
                }
                else
                {
                    bodyEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //LEG EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.legEquipmentInventory.Count)
                {
                    if (legEquipmentInventorySlots.Length <
                        player.playerInventoryManager.legEquipmentInventory.Count)
                    {
                        Instantiate(legEquipmentInventorySlotsParent, legEquipmentInventorySlotsParent);
                        legEquipmentInventorySlots = legEquipmentInventorySlotsParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                    }

                    legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
                }
                else
                {
                    legEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //HAND EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < handEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.handEquipmentInventory.Count)
                {
                    if (handEquipmentInventorySlots.Length <
                        player.playerInventoryManager.handEquipmentInventory.Count)
                    {
                        Instantiate(handEquipmentInventorySlotsParent, handEquipmentInventorySlotsParent);
                        handEquipmentInventorySlots = handEquipmentInventorySlotsParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
                    }

                    handEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.handEquipmentInventory[i]);
                }
                else
                {
                    handEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //CONSUMABLE EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < _consumableEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager._consumablesInventory.Count)
                {
                    if (_consumableEquipmentInventorySlots.Length <
                        player.playerInventoryManager._consumablesInventory.Count)
                    {
                        Instantiate(_consumableEquipmentInventorySlotsParent, _consumableEquipmentInventorySlotsParent);
                        _consumableEquipmentInventorySlots = _consumableEquipmentInventorySlotsParent.GetComponentsInChildren<_ConsumableInventorySlot>();
                    }

                    _consumableEquipmentInventorySlots[i].AddItem(player.playerInventoryManager._consumablesInventory[i]);
                }
                else
                {
                    _consumableEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //RING EQUIPMENT INVENTORY SLOTS
            for (int i = 0; i < _ringEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager._ringsInventory.Count)
                {
                    if (_ringEquipmentInventorySlots.Length <
                        player.playerInventoryManager._ringsInventory.Count)
                    {
                        Instantiate(_ringEquipmentInventorySlotsParent, _ringEquipmentInventorySlotsParent);
                        _ringEquipmentInventorySlots = _ringEquipmentInventorySlotsParent.GetComponentsInChildren<_RingInventorySlot>();
                    }

                    _ringEquipmentInventorySlots[i].AddItem(player.playerInventoryManager._ringsInventory[i]);
                }
                else
                {
                    _ringEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }
        }

        public void OpenSeclectWindow()
        {
            if (_bonfireTeleportWindow.activeSelf || 
                levelUpWindow.activeSelf)
                return;

            selectWindow.SetActive(true);
        }

        public void CloseSeclectWindow()
        {
            if (_bonfireTeleportWindow.activeSelf || 
                levelUpWindow.activeSelf)
                return;

            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindow()
        {
            ResetAllSelectedSlot();
            hudWindow.SetActive(true);
            weaponInventoryWindow.SetActive(false);
            _consumableInventoryWindow.SetActive(false);
            _headInventoryWindow.SetActive(false);
            _bodyInventoryWindow.SetActive(false);
            _legInventoryWindow.SetActive(false);
            _handInventoryWindow.SetActive(false);
            _ringInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            _quickChangeSpellWindow.SetActive(false);
            itemStatsWindow.SetActive(false);
            _skillTreeWindow.SetActive(false);
            _playerInforWindow.SetActive(false);
            _OptionsWindow.SetActive(false);
            _NOTE_BoardWindow.SetActive(false);
            _bonfireTeleportWindow.SetActive(false);
            levelUpWindow.SetActive(false);
            shopWindow.SetActive(false);
            endingWindow.SetActive(false);
            player.isBusy = false;
        }

        public void ResetAllSelectedSlot()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;

            headEquipmentSlotSelected = false;
            bodyEquipmentSlotSelected = false;
            legEquipmentSlotSelected = false;
            handEquipmentSlotSelected = false;

            _consumableSlotSelected = false;
            _spellSlotSelected = false;

            _ringSlot01Selected = false;
            _ringSlot02Selected = false;
            _ringSlot03Selected = false;
            _ringSlot04Selected = false;
        }

        public void ActivateBonfirePopUp()
        {
            bonfireLitPopUpUI.DisplayBonfireLitPopUp();
        }
    }
}