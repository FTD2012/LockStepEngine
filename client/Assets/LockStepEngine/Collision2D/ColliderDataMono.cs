using System;
using UnityEngine;

namespace LockStepEngine.Collision2D
{
    public class ColliderDataMono : UnityEngine.MonoBehaviour
    {
        public ColliderData colliderData;
    }
    [Serializable]
    public  class ColliderData : IComponent
    {
        [Header("Offset")]
        public LFloat y;

        public LVector2 pos;
        [Header("Collider data")]
        public LFloat high;

        public LFloat radius;
        public LVector2 size;
        public LVector2 up;
        public LFloat deg;
    }
}