using UnityEngine;

public class P_ : MonoBehaviour, IEntity  {

    private EntityProperties _eProperties;
    public EntityProperties EntityProperties { get { return _eProperties; } }

    private void Start() {
        _eProperties.OnStand.AddListener(Stand);
        _eProperties.OnDamaged.AddListener(Damaged);
        _eProperties.OnFall.AddListener(Fall);
    }

    private void Stand() {
        // Impossible?
    }

    private void Damaged() {
        // Lose control for mseconds?
    }

    private void Fall() {
        // Wait for Animation
        // Show UI
    }
}