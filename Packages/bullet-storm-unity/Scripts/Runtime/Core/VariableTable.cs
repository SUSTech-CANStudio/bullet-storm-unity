using System.Collections.Generic;
using System.Numerics;

namespace CANStudio.BulletStorm.Core
{
    internal class VariableTable
    {
        private readonly Dictionary<int, bool> _booleans = new Dictionary<int, bool>();
        private readonly Dictionary<int, float> _floats = new Dictionary<int, float>();
        private readonly Dictionary<int, int> _integers = new Dictionary<int, int>();
        private readonly Dictionary<int, Vector3> _vectors = new Dictionary<int, Vector3>();

        public void Clear()
        {
            _booleans.Clear();
            _integers.Clear();
            _floats.Clear();
            _vectors.Clear();
        }

        public void SetBool(int hash, bool value)
        {
            _booleans[hash] = value;
        }

        public void SetInt(int hash, int value)
        {
            _integers[hash] = value;
        }

        public void SetFloat(int hash, float value)
        {
            _floats[hash] = value;
        }

        public void SetVector(int hash, Vector3 value)
        {
            _vectors[hash] = value;
        }

        public bool TryGetBool(int hash, out bool value)
        {
            return _booleans.TryGetValue(hash, out value);
        }

        public bool TryGetInt(int hash, out int value)
        {
            return _integers.TryGetValue(hash, out value);
        }

        public bool TryGetFloat(int hash, out float value)
        {
            return _floats.TryGetValue(hash, out value);
        }

        public bool TryGetVector(int hash, out Vector3 value)
        {
            return _vectors.TryGetValue(hash, out value);
        }
    }
}