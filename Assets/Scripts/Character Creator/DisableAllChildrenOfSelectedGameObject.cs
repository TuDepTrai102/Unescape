using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllChildrenOfSelectedGameObject : MonoBehaviour
{
    public GameObject parentGameObject;

    public void DisableAllChildren()
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            var child = parentGameObject.transform.GetChild(i).gameObject;

            if (child != null)
            {
                child.SetActive(false);
            }
        }
    }
}
