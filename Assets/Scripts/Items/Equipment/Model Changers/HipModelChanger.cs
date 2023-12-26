using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class HipModelChanger : MonoBehaviour
    {
        public List<GameObject> hipsModel;

        private void Awake()
        {
            GetAllHipModels();
        }

        private void GetAllHipModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                hipsModel.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllHipModels()
        {
            foreach (GameObject item in hipsModel)
            {
                item.SetActive(false);
            }
        }

        public void EquipHipModelByName(string helmetName)
        {
            for (int i = 0; i < hipsModel.Count; i++)
            {
                if (hipsModel[i].name == helmetName)
                {
                    hipsModel[i].SetActive(true);
                }
            }
        }
    }
}