using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT {
    public class FogWall : MonoBehaviour
    {
        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}
