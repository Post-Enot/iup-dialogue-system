using System;
using IUP.Toolkits.SerializableCollections;
using UnityEngine;

namespace IUP.Toolkits.DialogueSystem.Builder
{
    [Serializable]
    public sealed class Blackboard
    {
        [SerializeField] private SK_SV_Dictionary<string, bool> _boolValues = new();
        [SerializeField] private SK_SV_Dictionary<string, int> _intValue = new();
        [SerializeField] private SK_SV_Dictionary<string, float> _floatValue = new();
        [SerializeField] private SK_SV_Dictionary<string, string> _stringValue = new();

        public bool Contains(string key)
        {
            return _boolValues.Value.ContainsKey(key) ||
                _intValue.Value.ContainsKey(key) ||
                _floatValue.Value.ContainsKey(key) ||
                _stringValue.Value.ContainsKey(key);
        }

        //public bool Remove(string key);

        //public string GetString(string key);

        //public void SetString(string key, string value);

        //public int GetInt(string key);

        //public void SetInt(string key, int value);

        //public float GetFloat(string key);

        //public void SetFloat(string key, float value);

        //public bool GetBool(string key);

        //public void SetBool(string key, bool value);
    }
}
