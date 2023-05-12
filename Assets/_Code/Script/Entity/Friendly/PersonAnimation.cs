using UnityEngine;

public class PersonAnimation : AnimationHandler {

    private const string PERSON_STANDING = "Person_Standing";
    private const string PERSON_FALLEN = "Person_Fallen";

    private void Start() {
        EntityProperties properties = GetComponent<EntityProperties>();
        properties.OnStand.AddListener(StandAnimation);
        properties.OnFallen.AddListener(FallenAnimation);
    }

    private void StandAnimation() {
        ChangeAnimation(PERSON_STANDING);
    }

    private void FallenAnimation() {
        ChangeAnimation(PERSON_FALLEN);
    }
}
