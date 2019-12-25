using UnityEngine;

public class Wall : MonoBehaviour {

    public enum DashReaction {
        Stop,
        Bounce
    }

    [SerializeField] DashReaction dashReaction = DashReaction.Stop;

    public DashReaction DashCollision => dashReaction;
}
