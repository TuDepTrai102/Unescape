using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NT
{
    [System.Serializable]
    public class SerializbleDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Tkey> keys = new List<Tkey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        //CALLED RIGHT BEFORE SERIALIZATION
        //SAVES THE DICTIONARY TO LISTS
        public void OnBeforeSerialize() 
        {
            keys.Clear();
            values.Clear();

            foreach (KeyValuePair<Tkey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        //CALLED RIGHT AFTER SERIALIZATION
        //LOAD THE DICTIONARY FROM THE LISTS
        public void OnAfterDeserialize() 
        {
            Clear();

            if (keys.Count != values.Count)
            {
                Debug.LogError("BRO, WE TRIED TO DESERIALIZE THE DICTIONARY, THE AMOUNT OF KEYS DOES NOT MATCH THE AMOUNT OF VALUES");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}