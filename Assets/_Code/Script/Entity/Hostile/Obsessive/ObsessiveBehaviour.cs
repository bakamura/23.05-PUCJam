using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObsessiveBehaviour : HostileBehaviour {

    [Header("Movement")]

    [SerializeField] private float _movementSpeed;

    [Header("Attack")]

    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _hitBox;
    [SerializeField] private LayerMask _playerLayer;
    private bool _hasHit;

    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _attackMotion;
    [SerializeField] private float _attackMotionSpeed;

    [Header("State")]

    [SerializeField] private float _recoilDuration;

    private enum ObsessiveState {
        Follow,
        Attack,
        Recoil
    }
    private ObsessiveState _state;

    [Header("Events")]

    private UnityEvent _onMove = new UnityEvent();
    public UnityEvent OnMove { get { return _onMove; } }
    private UnityEvent _onAttack = new UnityEvent();
    public UnityEvent OnAttack { get { return _onAttack; } }
    private UnityEvent _onDodged = new UnityEvent();
    public UnityEvent OnDodged { get { return _onDodged; } }


    [Header("Cache")]

    private Rigidbody2D _rb;
    private Coroutine _currentRoutine;
    private WaitForSeconds _attackDelayWait;
    private WaitForSeconds _attackMotionWait;
    private WaitForSeconds _recoilWait;
    private WaitForSeconds _deactivateWait;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        _attackDelayWait = new WaitForSeconds(_attackDelay);
        _attackMotionWait = new WaitForSeconds(_attackMotion / _attackMotionSpeed);
        _recoilWait = new WaitForSeconds(_recoilDuration);
        _deactivateWait = new WaitForSeconds(GetComponent<ObsessiveAnimation>().GetAnimationDuration(ObsessiveAnimation.OBSESSIVE_DODGED));
    }

    private void Start() {
        Collider2D[] tilemapCols = FindObjectsOfType<CompositeCollider2D>();
        foreach (Collider2D col in tilemapCols) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
    }

    private void OnEnable() {
        _currentRoutine = StartCoroutine(ChangeState());
    }

    private void FixedUpdate() {
        switch (_state) {
            case ObsessiveState.Follow:
                _rb.velocity = (P_EProperties.Instance.transform.position - transform.position).normalized * _movementSpeed;
                break;
            case ObsessiveState.Attack:
                if (!_hasHit) {
                    if (Physics2D.OverlapBox(transform.position, _hitBox, 0, LayerMask.NameToLayer("Ignore Raycast"))) gameObject.SetActive(false);
                    else if (Physics2D.OverlapBox(transform.position, _hitBox, 0, _playerLayer)) { // Check if player is dodging
                        P_EProperties.Instance.TakeDamage(_damage);
                        _hasHit = true;
                    }
                }
                break;
            case ObsessiveState.Recoil:
                _rb.velocity = Vector2.zero;
                break;
        }
    }

    private IEnumerator ChangeState() {
        while (gameObject.activeInHierarchy) {
            _onMove?.Invoke();
            _state = ObsessiveState.Follow;
            _hasHit = false;

            yield return null; // Wait 1 frame so start runs

            while (true) {
                if (Vector3.Distance(P_EProperties.Instance.transform.position, transform.position) <= _attackRange) {
                    _onAttack?.Invoke();
                    _state = ObsessiveState.Recoil;

                    yield return _attackDelayWait;

                    P_Ability.Instance.OnDodge.AddListener(GotDodged);
                    _state = ObsessiveState.Attack;
                    _rb.velocity = (P_EProperties.Instance.transform.position - transform.position).normalized * _attackMotionSpeed;
                    break;
                }

                yield return null;
            }

            yield return _attackMotionWait;

            P_Ability.Instance.OnDodge.RemoveListener(GotDodged);
            
            _state = ObsessiveState.Recoil;

            yield return _recoilWait;
        }
    }

    private void GotDodged() {
        _onDodged?.Invoke();
        StopCoroutine(_currentRoutine);
        StartCoroutine(DeactivateSelf());
    }

    private IEnumerator DeactivateSelf() {
        yield return _deactivateWait;

        gameObject.SetActive(false);
    }
}