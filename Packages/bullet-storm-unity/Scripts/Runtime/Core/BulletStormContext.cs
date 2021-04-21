using System;
using System.Numerics;

namespace CANStudio.BulletStorm.Core
{
    public class BulletStormContext
    {
        private readonly VariableTable _variables = new VariableTable();
        internal event Action<float> simulate;

        public void SetBool(int hash, bool value)
        {
            _variables.SetBool(hash, value);
        }

        public void SetInt(int hash, int value)
        {
            _variables.SetInt(hash, value);
        }

        public void SetFloat(int hash, float value)
        {
            _variables.SetFloat(hash, value);
        }

        public void SetVector(int hash, Vector3 value)
        {
            _variables.SetVector(hash, value);
        }

        public void ClearVariables()
        {
            _variables.Clear();
        }

        public bool GetBool(int hash)
        {
            if (_variables.TryGetBool(hash, out var value)) return value;
            throw new ArgumentOutOfRangeException(nameof(hash), "No such a boolean variable");
        }

        public int GetInt(int hash)
        {
            if (_variables.TryGetInt(hash, out var value)) return value;
            throw new ArgumentOutOfRangeException(nameof(hash), "No such an integer variable");
        }

        public float GetFloat(int hash)
        {
            if (_variables.TryGetFloat(hash, out var value)) return value;
            throw new ArgumentOutOfRangeException(nameof(hash), "No such a float variable");
        }

        public Vector3 GetVector(int hash)
        {
            if (_variables.TryGetVector(hash, out var value)) return value;
            throw new ArgumentOutOfRangeException(nameof(hash), "No such a vector variable");
        }


        public void Simulate(float deltaTime)
        {
            simulate?.Invoke(deltaTime);
        }

        public static int String2Hash(string str)
        {
            return str.GetHashCode();
        }
    }
}