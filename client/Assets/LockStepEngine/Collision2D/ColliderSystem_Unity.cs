using UnityEngine;

namespace LockStepEngine.Collision2D
{
    public partial class CollisionSystem
    {
        public static ColliderPrefab CreateColliderPrefab(GameObject fab, ColliderData data)
        {
            GLog.Error("CreateColliderPrefab " + fab.name);
            CBaseShape collider = null;
            if (data == null)
            {
                Debug.LogError(fab.name + " Miss ColliderDataMono ");
                return null;
            }

            if (data.radius > 0)
            {
                //circle
                collider = new CCircle(data.radius);
            }
            else
            {
                //obb
                collider = new COBB(data.size, data.deg);
            }

            GLog.Info($"{fab.name} !!!CreateCollider  deg: {data.deg} up:{data.size} radius:{data.radius}");
            var colFab = new ColliderPrefab();
            colFab.parts.Add(new ColliderPart()
            {
                transform = new CTransform2D(LVector2.zero),
                collider = collider
            });
            return colFab;
        }
    }
}