using System.Collections;
using System.Collections.Generic;

namespace BulletStorm.Core
{
    public class BulletParamsArray : IEnumerable<BulletParams>
    {
        private readonly BulletParams[] _bulletParams;

        public int Length => _bulletParams.Length;

        public ref BulletParams this[int i] => ref _bulletParams[i];

        public BulletParamsArray(int length) => _bulletParams = new BulletParams[length];

        public IEnumerator<BulletParams> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<BulletParams>
        {
            private readonly BulletParamsArray _array;
            private int _index;

            public Enumerator(BulletParamsArray array)
            {
                _array = array;
                _index = 0;
            }
            
            public bool MoveNext() => ++_index < _array.Length;

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public BulletParams Current => _array[_index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}