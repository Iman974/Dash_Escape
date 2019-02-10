using UnityEngine;

[System.Serializable]
public class RaycastController {

    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private int verticalRayCount = 4;
    [SerializeField] private LayerMask collisionLayer = new LayerMask();

    private BoxCollider2D collider;

    public const float SkinWidth = 0.015f;
    private const int Down = -1, Left = -1;
    private const int MinRayCount = 2;
    private const int MaxRayCount = 30;

    public int HorizontalRayCount => horizontalRayCount;
    public int VerticalRayCount => verticalRayCount;
    public float HorizontalRaySpacing { get; private set; }
    public float VerticalRaySpacing { get; private set; }
    public LayerMask CollisionLayer => collisionLayer;
    public RaycastOrigins Origins { get; private set; }

    public void Initialize(BoxCollider2D boxCollider) {
        collider = boxCollider;
        Origins = new RaycastOrigins();

        CalculateRaySpacing();
    }

    private void CalculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand(SkinWidth * -2f);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, MinRayCount, MaxRayCount);
        verticalRayCount = Mathf.Clamp(verticalRayCount, MinRayCount, MaxRayCount);

        HorizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        VerticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(SkinWidth * -2f);

        Origins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Origins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        Origins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Origins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public class RaycastOrigins {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
