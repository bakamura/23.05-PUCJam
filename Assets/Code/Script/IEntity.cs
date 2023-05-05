using UnityEngine;
using UnityEngine.Events;

public interface IEntity {

    public EntityProperties EntityProperties { get; } // Getter, not member itself

}

[System.Serializable]
public class EntityProperties {

    protected int _healthMax;
    protected int _healthCurrent;
    private UnityEvent _onStand; // Change for a delegate?
    public UnityEvent OnStand { get { return _onStand; } } // Change for a delegate?
    private UnityEvent _onDamaged;
    public UnityEvent OnDamaged;
    private UnityEvent _onFall;
    public UnityEvent OnFall;

    public void TakeHeal(int heal) {
        if (heal < 0) {
            Debug.LogWarning("Heal cannot be a negative value!");
            return;
        }
        if (heal > 0 && _healthCurrent <= 0) _onStand.Invoke();
        _healthCurrent = Mathf.Clamp(_healthCurrent + heal, 0, _healthMax);
    }

    public void TakeDamage(int damage) {
        if (damage < 0) {
            Debug.LogWarning("Damage cannot be a negative value!");
            return;
        }
        _healthCurrent = Mathf.Clamp(_healthCurrent - damage, 0, _healthMax);
        if (_healthCurrent <= 0) _onFall.Invoke();
        _onDamaged.Invoke();
    }

}