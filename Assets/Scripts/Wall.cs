using UnityEngine;

public class Wall : MonoBehaviour, IDashable {

    [SerializeField] DashReaction dashReaction = DashReaction.Stop;

    public DashReaction DashReaction => dashReaction;
}
