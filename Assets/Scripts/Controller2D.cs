using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    [SerializeField] private RaycastController raycastController = null;

    private CollisionsInfo collisions;
    private RaycastController.RaycastOrigins raycastOrigins;
    private LayerMask collisionLayer;

    private const int Down = -1, Left = -1;

    public bool IsCollidingBelow => collisions.below;
    public bool IsCollidingAbove => collisions.above;

    private void Start() {
        raycastController.Initialize(GetComponent<BoxCollider2D>());
        raycastOrigins = raycastController.Origins;
        collisionLayer = raycastController.CollisionLayer;
    }

    public void Move(Vector2 deltaMove) {
        raycastController.UpdateRaycastOrigins();
        collisions.Reset();

        if (!Mathf.Approximately(deltaMove.x, 0f)) {
            ProcessHorizontalCollisions(ref deltaMove);
        }
        ProcessVerticalCollisions(ref deltaMove);

        transform.position += (Vector3)deltaMove;
    }

    private void ProcessHorizontalCollisions(ref Vector2 deltaMove) {
        float directionX = Mathf.Sign(deltaMove.x);
        float rayLength = Mathf.Abs(deltaMove.x) + RaycastController.SkinWidth;

        for (int i = 0; i < raycastController.HorizontalRayCount; i++) {
            Vector2 rayOrigin = (int)directionX == Left ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raycastController.HorizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) {
                deltaMove.x = (hit.distance - RaycastController.SkinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = (int)directionX == Left;
                collisions.right = !collisions.left;
            }
        }
    }

    private void ProcessVerticalCollisions(ref Vector2 deltaMove) {
        float directionY = Mathf.Sign(deltaMove.y);
        float rayLength = Mathf.Abs(deltaMove.y) + RaycastController.SkinWidth;

        for (int i = 0; i < raycastController.VerticalRayCount; i++) {
            Vector2 rayOrigin = (int)directionY == Down ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * ((raycastController.VerticalRaySpacing * i) + deltaMove.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.blue);

            if (hit) {
                deltaMove.y = (hit.distance - RaycastController.SkinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = (int)directionY == Down;
                collisions.above = !collisions.below;
            }
        }
    }

    public struct CollisionsInfo {
        public bool above, below;
        public bool left, right;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }
}
