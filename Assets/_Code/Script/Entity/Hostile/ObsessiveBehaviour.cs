using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("Cache")]

    private Rigidbody2D _rb;
    private P_EProperties _playerProperties;
    private WaitForSeconds _attackDelayWait;
    private WaitForSeconds _attackMotionWait;
    private WaitForSeconds _recoilWait;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();

        _attackDelayWait = new WaitForSeconds(_attackDelay);
        _attackMotionWait = new WaitForSeconds(_attackMotion / _attackMotionSpeed);
        _recoilWait = new WaitForSeconds(_recoilDuration);
    }

    private void Start() {
        _playerProperties = P_Movement.Instance.GetComponent<P_EProperties>();

        Collider2D[] tilemapCols = FindObjectsOfType<TilemapCollider2D>();
        foreach (TilemapCollider2D col in tilemapCols) Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
    }

    private void OnEnable() {
        StartCoroutine(ChangeState());
    }

    private void FixedUpdate() {
        switch (_state) {
            case ObsessiveState.Follow:
                _rb.velocity = (_playerProperties.transform.position - transform.position).normalized * _movementSpeed;
                break;
            case ObsessiveState.Attack:
                if (!_hasHit) {
                    if (Physics2D.OverlapBox(transform.position, _hitBox, 0, LayerMask.NameToLayer("Ignore Raycast"))) gameObject.SetActive(false);
                    else if (Physics2D.OverlapBox(transform.position, _hitBox, 0, _playerLayer)) { // Check if player is dodging
                        _playerProperties.TakeDamage(_damage);
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
            _state = ObsessiveState.Follow;

            yield return null; // Wait 1 frame so start runs

            while (true) {
                if (Vector3.Distance(_playerProperties.transform.position, transform.position) <= _attackRange) {
                    _rb.velocity = Vector2.zero;

                    yield return _attackDelayWait;

                    _state = ObsessiveState.Attack;
                    _rb.velocity = (_playerProperties.transform.position - transform.position).normalized * _attackMotionSpeed;
                    break;
                }

                yield return null;
            }

            yield return _attackMotionWait;

            _state = ObsessiveState.Recoil;

            yield return _recoilWait;
        }
    }
}