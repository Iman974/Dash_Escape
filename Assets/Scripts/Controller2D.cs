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
            CastHorizontalRays(deltaMove);
            if (collisions.hitX) {
                deltaMove.x = collisions.hitX.distance * (collisions.right ? 1f : -1f);
            }
        }
        if (!Mathf.Approximately(deltaMove.y, 0f)) {
            CastVerticalRays(deltaMove);
            if (collisions.hitY) {
                deltaMove.y = collisions.hitY.distance * (collisions.above ? 1f : -1f);
            }
        }
        transform.position += (Vector3)deltaMove;
    }

    // This is the exact same function as the one below -> use delegate to make only one function ?
    void CastHorizontalRays(Vector2 deltaMove) {
        float directionX = Mathf.Sign(deltaMove.x);
        float rayLength = Mathf.Abs(deltaMove.x) + RaycastController.SkinWidth;
        const int Left = -1;

        for (int i = 0; i < raycastController.HorizontalRayCount; i++) {
            Vector2 rayOrigin = (int)directionX == Left ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycastController.HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) {
                rayLength = hit.distance;
                hit.distance -= RaycastController.SkinWidth;
                collisions.hitX = hit;
                collisions.left = (int)directionX == Left;
                collisions.right = !collisions.left;
            }
        }
    }

    // This is the exact same function as the one above -> use delegate to make only one function ?
    void CastVerticalRays(Vector2 deltaMove) {
        float directionY = Mathf.Sign(deltaMove.y);
        float rayLength = Mathf.Abs(deltaMove.y) + RaycastController.SkinWidth;
        const int Down = -1;

        for (int i = 0; i < raycastController.VerticalRayCount; i++) {
            Vector2 rayOrigin = (int)directionY == Down ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * ((raycastController.VerticalRaySpacing * i) /*+ deltaMove.x*/);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.blue);

            if (hit) {
                rayLength = hit.distance;
                hit.distance -= RaycastController.SkinWidth;
                collisions.hitY = hit;
                collisions.below = (int)directionY == Down;
                collisions.above = !collisions.below;
            }
        }
    }

    public void CastAllRays(Vector2 deltaMove) {
        CastHorizontalRays(deltaMove);
        CastVerticalRays(deltaMove);
    }

    public struct Collisions {
        public bool above, below;
        public bool left, right;
        public RaycastHit2D hitX;
        public RaycastHit2D hitY;

        public void Reset() {
            above = below = false;
            left = right = false;
            hitX = default;
            hitY = default;
        }

        // Returns the hit which touched a collider. If none returns hitY.
        public RaycastHit2D GetAnyHit() {
            if (hitX) {
                return hitX;
            }
            return hitY;
        }
    }
}
