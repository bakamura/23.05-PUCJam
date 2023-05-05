using UnityEngine;

public interface IEntity {

    public EntityProperties EntityProperties { get; } // Getter, not member itself

}

[System.Serializable]
public class EntityProperties {

    protected int _healthMax;
    protected int _healthCurrent;

    public void TakeHeal(int heal) {
        if (heal < 0) {
            Debug.LogWarning("Heal cannot be a negative value!");
            return;
        }
        _healthCurrent = Mathf.Clamp(_healthCurrent + heal, 0, _healthMax);
    }

    public void TakeDamage(int damage) {
        if (damage < 0) {
            Debug.LogWarning("Damage cannot be a negative value!");
            return;
        }
        _healthCurrent = Mathf.Clamp(_healthCurrent - damage, 0, _healthMax);
        if (_healthCurrent <= 0) Fall();
    }

    private void Stand() { }

    private void Fall() { }

}