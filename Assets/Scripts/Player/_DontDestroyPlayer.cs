using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class _DontDestroyPlayer : MonoBehaviour
    {
        public static _DontDestroyPlayer instance;

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
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}