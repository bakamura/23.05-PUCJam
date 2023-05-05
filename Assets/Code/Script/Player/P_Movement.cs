using UnityEngine;

public class P_Movement : Singleton<P_Movement> {

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private Vector2 _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckBox;
    [SerializeField] private LayerMask _groundCheckMask;

    [Header("Cache")]

    private Rigidbody2D _rb;
    public Rigidbody2D RigidBody2D { get { return _rb; } }
    private Vector2 _newVelocity;

    protected override void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        _newVelocity = _rb.velocity; // Deliberate about Vector2.Set(X,Y) usefullness

        // Movement
        _newVelocity[0] = InputHandler.Instance.Movement * _movementSpeed; // Player stops if no input while in the air

        // Jump
        if (Physics2D.OverlapBox((Vector2) transform.position + _groundCheckPoint, _groundCheckBox, 0, _groundCheckMask) && InputHandler.Instance.Jump) _newVelocity[1] = _jumpSpeed; //Order could be changed for performance, but needs handler changes

        _rb.velocity = _newVelocity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Physics2D.OverlapBox((Vector2) transform.position + _groundCheckPoint, _groundCheckBox, 0, _groundCheckMask) ? Color.green : Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + _groundCheckPoint, _groundCheckBox);
    }
#endif
}
