using System;
using BulletStorm.Util;
using UnityEngine;

namespace BulletStorm.Examples.Scripts
{
    public class Test : MonoBehaviour
    {
        public Test2 t;
        
        private void Start()
        {
            Debug.Log("I am son.");
        }
    }

    [Serializable]
    public class Test2
    {
        public Test3 t;
    }

    [Serializable]
    public class Test3
    {
        public Target target;
    }
}