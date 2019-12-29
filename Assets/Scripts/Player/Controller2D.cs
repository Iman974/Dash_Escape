using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour {

    [SerializeField] RaycastController raycastController = null;

    public Collisions collisions;

    RaycastController.RaycastOrigins raycastOrigins;
    LayerMask collisionLayer;

    void Start() {
        raycastController.Initialize(GetComponent<Collider2D>());
        raycastOrigins = raycastController.Origins;
        collisionLayer = raycastController.CollisionLayer;
    }

    public void Move(Vector2 deltaMove) {
        raycastController.UpdateRaycastOrigins();
        collisions.Reset();

        if (!Mathf.Approximately(deltaMove.x, 0f)) {
            RaycastHit2D closestHit = CastHorizontalRays(deltaMove);
            if (closestHit) {
                deltaMove.x = closestHit.distance * (collisions.right ? 1f : -1f);
                collisions.latestHit = closestHit;
            }
        }
        if (!Mathf.Approximately(deltaMove.y, 0f)) {
            RaycastHit2D closestHit = CastVerticalRays(deltaMove);
            if (closestHit) {
                deltaMove.y = closestHit.distance * (collisions.above ? 1f : -1f);
                collisions.latestHit = closestHit;
            }
        }
        transform.position += (Vector3)deltaMove;
    }

    RaycastHit2D CastHorizontalRays(Vector2 deltaMove) {
        float directionX = Mathf.Sign(deltaMove.x);
        float rayLength = Mathf.Abs(deltaMove.x) + RaycastController.SkinWidth;
        const int Left = -1;

        RaycastHit2D closestHit = default;
        for (int i = 0; i < raycastController.HorizontalRayCount; i++) {
            Vector2 rayOrigin = (int)directionX == Left ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycastController.HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) {
                rayLength = hit.distance;
                hit.distance -= RaycastController.SkinWidth;
                closestHit = hit;
                collisions.left = (int)directionX == Left;
                collisions.right = !collisions.left;
            }
        }
        return closestHit;
    }

    RaycastHit2D CastVerticalRays(Vector2 deltaMove) {
        float directionY = Mathf.Sign(deltaMove.y);
        float rayLength = Mathf.Abs(deltaMove.y) + RaycastController.SkinWidth;
        const int Down = -1;

        RaycastHit2D closestHit = default;
        for (int i = 0; i < raycastController.VerticalRayCount; i++) {
            Vector2 rayOrigin = (int)directionY == Down ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * ((raycastController.VerticalRaySpacing * i) + deltaMove.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.blue);

            if (hit) {
                rayLength = hit.distance;
                hit.distance -= RaycastController.SkinWidth;
                closestHit = hit;
                collisions.below = (int)directionY == Down;
                collisions.above = !collisions.below;
            }
        }
        return closestHit;
    }

    public bool CastAllRays(Vector2 deltaMove) {
        RaycastHit2D hit = CastHorizontalRays(deltaMove);
        if (hit) {
            return true;
        }
        hit = CastVerticalRays(deltaMove);
        return hit;
    }

    public struct Collisions {
        public bool above, below;
        public bool left, right;
        public RaycastHit2D latestHit;

        public void Reset() {
            above = below = false;
            left = right = false;
            latestHit = default;
        }
    }
}
