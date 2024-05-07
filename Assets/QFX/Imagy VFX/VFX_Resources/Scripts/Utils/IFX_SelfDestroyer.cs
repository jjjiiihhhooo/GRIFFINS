// ReSharper disable once CheckNamespace
using UnityEngine;

namespace QFX.IFX
{
    public class IFX_SelfDestroyer : IFX_ControlledObject
    {
        public float LifeTime;

        public GameObject col;

        public override void Run()
        {
            base.Run();

            if (col != null)
            {
                GameObject temp = Instantiate(col, transform.position, Quaternion.identity);
            }
            Destroy(gameObject, LifeTime);
        }
    }
}