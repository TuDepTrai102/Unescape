using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTrigger : MonoBehaviour
{
    BoxCollider[] boxCollider;

    private void Awake()
    {
        boxCollider = GetComponentsInChildren<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TriggerCollider());
    }

    private IEnumerator TriggerCollider()
    {
        yield return new WaitForSeconds(1);

        foreach (var boxCollider in boxCollider)
        {
            boxCollider.isTrigger = false;
        }
    }
}
