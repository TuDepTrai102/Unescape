using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class LeftLegModelChanger : MonoBehaviour
    {
        public List<GameObject> legModels;

        private void Awake()
        {
            GetAllLegModels();
        }

        private void GetAllLegModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                legModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllLegModels()
        {
            foreach (GameObject item in legModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLegModelByName(string legName)
        {
            for (int i = 0; i < legModels.Count; i++)
            {
                if (legModels[i].name == legName)
                {
                    legModels[i].SetActive(true);
                }
            }
        }
    }
}