using UnityEngine;

public class P_Animation : AnimationHandler {

    private static P_Animation _instance;
    public static P_Animation Instance { get { return _instance; } }

    protected override void Awake() {
        base.Awake();

        if (_instance == null) _instance = this;
        else if (_instance != this) {
            Debug.LogWarning("Multiple P_Animation instances found, deleted duplicate"); // Check grammar
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (InputHandler.Instance.Movement != 0) {
            _sr.flipX = InputHandler.Instance.Movement < 0;
        }
    }

}
