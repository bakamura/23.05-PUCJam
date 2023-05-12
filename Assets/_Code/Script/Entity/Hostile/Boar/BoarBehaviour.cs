using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BoarBehaviour : HostileBehaviour {

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;
    private bool _movementDirection; // True when right

    [Header("Attack")]

    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _hitBox;
    [SerializeField] private LayerMask _playerLayer;
    private bool _hasHit;

    [Header("State")]

    [SerializeField] private float _idleDuration;
    [SerializeField] private float _movementDuration;
    private enum BoarState {
        Idle,
        Movement
    }
    private BoarState _state;

    [Header("Events")]

    private UnityEvent _onIdle = new UnityEvent();
    public UnityEvent OnIdle { get { return _onIdle; } }
    private UnityEvent _onRun = new UnityEvent();
    public UnityEvent OnRun { get { return _onRun; } }

    [Header("Cache")]

    private Rigidbody2D _rb;
    public Rigidbody2D Rigidbody2D { get { return _rb; } }
    private WaitForSeconds _idleWait;
    private WaitForSeconds _movementWait;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        _idleWait = new WaitForSeconds(_idleDuration);
        _movementWait = new WaitForSeconds(_movementDuration);
    }

    private void OnEnable() {
        StartCoroutine(ChangeState());
    }

    private void FixedUpdate() {
        switch (_state) {
            case BoarState.Idle:
                break;
            case BoarState.Movement:
                // Doesn't consider it can fall
                _rb.velocity = _movementSpeed * (_movementDirection ? Vector2.right : Vector2.left);
                if (!_hasHit && Physics2D.OverlapBox(transform.position, _hitBox, 0, _playerLayer)) {
                    P_EProperties.Instance.TakeDamage(_damage);
                    _hasHit = true;
                }
                break;
        }
    }

    private IEnumerator ChangeState() {
        while (gameObject.activeInHierarchy) {
            _state = BoarState.Idle;
            _rb.velocity = Vector2.zero;
            _hasHit = false;

            yield return _idleWait;

            _state = BoarState.Movement;
            _movementDirection = P_Movement.Instance.transform.position.x - transform.position.x > 0;

            yield return _movementWait;
        }
    }
}