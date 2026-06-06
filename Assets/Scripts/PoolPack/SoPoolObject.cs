using UnityEngine;

namespace PoolPack
{
    public class SoPoolObject : ScriptableObject
    {
        public T As<T>() where T : SoPoolObject
        {
            return (T)this;
        }
    }
}