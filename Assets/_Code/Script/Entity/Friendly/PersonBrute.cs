using UnityEngine;
using UnityEngine.Events;

public class PersonBrute : EntityProperties {

    [Header("Follow")]

    [SerializeField] private float _movementSpeed;

    [Header("Catch")]

    [SerializeField] private float _detectRange;
    [SerializeField] private float _catchRange;
    [SerializeField] private LayerMask _hostileLayer;
    private GameObject _nearestHostile;

    [Header("Events")]

    private UnityEvent _onCatch;
    public UnityEvent OnCatch { get { return _onCatch; } }

    [Header("Cache")]

    private Rigidbody2D _rb;
    private Collider2D[] _colliderInContact;
    private float _nearestColliderDistance;
    private float _nearestColliderCache;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start() {
        base.Start();

        //_onStand.AddListener();
        _onFallen.AddListener(ReleaseHostile);

        _nearestColliderDistance = _catchRange + 1;
    }

    private void Update() { // can be one liner with ? : maybe
        if (_standing && _nearestColliderDistance > _catchRange && GetNearestHostile()) _rb.velocity = _movementSpeed * Mathf.Sign(_nearestHostile.transform.position.x - transform.position.x) * Vector2.right;
        else _rb.velocity = Vector2.zero;
    }

    // Returns true if there is an enemy in range
    private bool GetNearestHostile() {
        _colliderInContact = Physics2D.OverlapCircleAll(transform.position, _detectRange, _hostileLayer);
        if (_colliderInContact.Length > 0) {
            for (int i = 0; i < _colliderInContact.Length; i++) {
                if (i == 0) {
                    _nearestHostile = _colliderInContact[i].gameObject;
                    _nearestColliderDistance = Vector3.Distance(transform.position, _colliderInContact[i].transform.position);
                }
                else {
                    _nearestColliderCache = Vector3.Distance(transform.position, _colliderInContact[i].transform.position);
                    if (_nearestColliderCache < _nearestColliderDistance) {
                        _nearestHostile = _colliderInContact[i].gameObject;
                        _nearestColliderDistance = _nearestColliderCache;
                    }
                }
            }
            if (_nearestColliderDistance <= _catchRange) Catch();
            return true;
        }
        else {
            _nearestHostile = null;
            return false;
        }
    }

    private void Catch() {
        _nearestHostile.GetComponent<HostileBehaviour>().enabled = false;
        _rb.velocity = Vector2.zero;
    }

    private void ReleaseHostile() {
        if (_nearestHostile != null) {
            _nearestHostile.GetComponent<HostileBehaviour>().enabled = true;
            // Unsubscribe from hostile onfall

            _nearestColliderDistance = _catchRange + 1;
        }
    }
}
