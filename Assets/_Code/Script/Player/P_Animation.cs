using UnityEngine;

public class P_Animation : AnimationHandler {

    // Singleton because class can't inherit from 2 base classes
    private static P_Animation _instance;
    public static P_Animation Instance { get { return _instance; } }

    [Header("Cache")]

    private const string P_IDLE = "P_Idle";
    private const string P_RUN = "P_Run";
    private const string P_JUMP = "P_Jump";
    private const string P_FALL = "P_Fall";
    public const string P_DAMAGED = "P_Damaged";
    private const string P_FALLEN = "P_Fallen";
    public const string P_HEAL = "P_Heal";
    public const string P_DODGE = "P_Dodge";
    public const string P_INTERACT = "P_Interact";
    private const string P_ENTERDOOR = "P_EnterDoor";

    protected override void Awake() {
        base.Awake();

        if (_instance == null) _instance = this;
        else if (_instance != this) {
            Debug.LogWarning("Multiple P_Animation instances found, deleted duplicate"); // Check grammar
            Destroy(gameObject);
        }
    }

    private void Start() {
        GetComponent<P_Movement>().OnJump.AddListener(JumpAnimation);
        P_EProperties properties = GetComponent<P_EProperties>();
        properties.OnDamaged.AddListener(DamagedAnimation);
        properties.OnFallen.AddListener(FallenAnimation);
        P_Ability abilities = GetComponent<P_Ability>();
        abilities.OnHeal.AddListener(HealAnimation);
        abilities.OnDodge.AddListener(DodgeAnimation);
    }

    private void Update() {
        if(InputHandler.Instance.Movement != 0) _sr.flipX = InputHandler.Instance.Movement < 0;
        if (P_Movement.Instance.PlayerGrounded() && _animation[_currentAnimation].name != P_JUMP) {
            if (InputHandler.Instance.Movement == 0) ChangeAnimation(P_IDLE);
            else ChangeAnimation(P_RUN);
        }
        else if (P_Movement.Instance.RigidBody2D.velocity.y <= 0 ) ChangeAnimation(P_FALL);
    }

    private void JumpAnimation() { // Could be substituted by using UnityEvent<String>
        ChangeAnimation(P_JUMP);
    }

    private void DamagedAnimation() {
        ChangeAnimation(P_DAMAGED);
    }

    private void FallenAnimation() {
        ChangeAnimation(P_FALLEN);
    }

    private void HealAnimation() {
        ChangeAnimation(P_HEAL);
    }

    private void DodgeAnimation() {
        ChangeAnimation(P_DODGE);
    }

    private void InteractAnimation() { // NEEDS TO LISTEN TO EVENT
        ChangeAnimation(P_INTERACT);
    }

    private void EnterDoorAnimation() { // NEEDS TO LISTEN TO EVENT
        ChangeAnimation(P_ENTERDOOR);
    }
}