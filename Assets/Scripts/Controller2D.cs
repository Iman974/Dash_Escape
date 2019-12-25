using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour {

    [SerializeField] RaycastController raycastController = null;

    public CollisionsInfo collisions;

    RaycastController.RaycastOrigins raycastOrigins;
    LayerMask collisionLayer;

    const int Down = -1, Left = -1;

    void Start() {
        raycastController.Initialize(GetComponent<Collider2D>());
        raycastOrigins = raycastController.Origins;
        collisionLayer = raycastController.CollisionLayer;
        dashVelocity = dashDistance / dashSpeed;
    }

    public void Move(Vector2 deltaMove) {
        raycastController.UpdateRaycastOrigins();
        collisions.Reset();

        if (!Mathf.Approximately(deltaMove.x, 0f)) {
            ProcessHorizontalCollisions(ref deltaMove);
        }
        if (!Mathf.Approximately(deltaMove.y, 0f)) {
            ProcessVerticalCollisions(ref deltaMove);
        }
        transform.position += (Vector3)deltaMove;
    }

    [SerializeField] float dashDistance = 4f;
    [SerializeField] float dashSpeed = 0.14f;

    Vector2 dashDirection;
    Vector2 dashStartPosition;
    float remainingDashDistance;
    float dashVelocity;

    public void InitDash() {
        dashStartPosition = transform.position;
        remainingDashDistance = dashDistance;
        dashDirection = (InputUtility.GetWorldMousePosition() - dashStartPosition).normalized;
    }

    public bool Dash() {
        if ((dashStartPosition - (Vector2)transform.position).sqrMagnitude >= remainingDashDistance *
        remainingDashDistance) {
            return false;
        }
        if (collisions.right || collisions.left || collisions.above || collisions.below) {
            Wall wall = collisions.hitCollider.GetComponent<Wall>();
            Vector2 normal = collisions.normal;
            if (wall.DashCollision == Wall.DashReaction.Bounce) {
                dashDirection = Vector2.Reflect(dashDirection, normal);
                remainingDashDistance -= (dashStartPosition - (Vector2)transform.position).magnitude;
                dashStartPosition = transform.position;
            } else if (wall.DashCollision == Wall.DashReaction.Stop) {
                return false;
            }
        }
        return true;
    }

    void ProcessHorizontalCollisions(ref Vector2 deltaMove) {
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
                collisions.hitCollider = hit.collider;
                collisions.normal = hit.normal;
            }
        }
    }

    void ProcessVerticalCollisions(ref Vector2 deltaMove) {
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
                collisions.hitCollider = hit.collider;
                collisions.normal = hit.normal;
            }
        }
    }

    public struct CollisionsInfo {
        public bool above, below;
        public bool left, right;
        public Collider2D hitCollider;
        public Vector2 normal;

        public void Reset() {
            above = below = false;
            left = right = false;
            hitCollider = null;
        }
    }
}
