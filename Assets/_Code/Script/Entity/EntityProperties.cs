using UnityEngine;
using UnityEngine.Events;

public class EntityProperties : MonoBehaviour {

    [SerializeField] protected int _healthMax;
    protected int _healthCurrent = 0;
    [SerializeField] protected bool _standing = true;
    public bool Standing { get { return _standing; } }

    [Header("Events")]

    protected UnityEvent _onStand = new UnityEvent();
    public UnityEvent OnStand { get { return _onStand; } }
    protected UnityEvent _onHealed = new UnityEvent();
    public UnityEvent OnHealed { get { return _onHealed; } }
    protected UnityEvent _onDamaged = new UnityEvent();
    public UnityEvent OnDamaged { get { return _onDamaged; } }
    protected UnityEvent _onFallen = new UnityEvent();
    public UnityEvent OnFallen { get { return _onFallen; } }
    protected UnityEvent _onHealthChange = new UnityEvent();
    public UnityEvent OnHealthChange { get { return _onHealthChange; } }

    protected virtual void Start() {
        if (_standing) _healthCurrent = _healthMax;
    }

    public virtual void TakeHeal(int heal) {
        if (heal <= 0) {
            Debug.LogWarning("Heal cannot be 0 or less!");
            return;
        }
        _healthCurrent = Mathf.Clamp(_healthCurrent + heal, 0, _healthMax);
        _onHealed?.Invoke();
        if (_healthCurrent > 0) {
            _onStand?.Invoke();
            _standing = true;
        }
        _onHealthChange?.Invoke();
    }

    public void TakeDamage(int damage) {
        if (_healthCurrent > 0) {
            if (damage <= 0) {
                Debug.LogWarning("Damage cannot be a negative value!");
                return;
            }
            _healthCurrent = Mathf.Clamp(_healthCurrent - damage, 0, _healthMax);
            _onDamaged?.Invoke();
            if (_healthCurrent <= 0) {
                _onFallen?.Invoke();
                _standing = true;
            }
            _onHealthChange?.Invoke();
        }
    }
}