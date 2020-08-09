using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletStorm.Util
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] [SerializeReference] [HideInInspector]
        private List<TKey> keys;
        [SerializeField] [SerializeReference] [HideInInspector]
        private List<TValue> values;

        public void OnBeforeSerialize()
        {
            throw new NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }
    }
}