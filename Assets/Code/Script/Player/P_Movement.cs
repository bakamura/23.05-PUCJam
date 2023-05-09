using UnityEngine;
using UnityEngine.Events;

public class P_Movement : Singleton<P_Movement> {

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private Vector2 _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckBox;
    [SerializeField] private LayerMask _groundCheckMask;

    [Header("Events")]

    private UnityEvent _onJump = new UnityEvent();
    public UnityEvent OnJump { get { return _onJump; } }

    [Header("Cache")]

    private Rigidbody2D _rb;
    public Rigidbody2D RigidBody2D { get { return _rb; } }
    private Vector2 _newVelocity;

    protected override void Awake() {
        base.Awake();

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        _newVelocity = _rb.velocity;

        // Movement
        _newVelocity[0] = InputHandler.Instance.Movement * _movementSpeed; // Player stops if no input while in the air

        // Jump
        if (PlayerGrounded() && InputHandler.Instance.Jump) {
            _newVelocity[1] = _jumpSpeed; //Order could be changed for performance, but needs handler changes
            _onJump.Invoke();
        }

        _rb.velocity = _newVelocity;
    }

    public bool PlayerGrounded() {
        return Physics2D.OverlapBox((Vector2)transform.position + _groundCheckPoint, _groundCheckBox, 0, _groundCheckMask);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Physics2D.OverlapBox((Vector2) transform.position + _groundCheckPoint, _groundCheckBox, 0, _groundCheckMask) ? Color.green : Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + _groundCheckPoint, _groundCheckBox);
    }
#endif
}
