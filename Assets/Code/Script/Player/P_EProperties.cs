using UnityEngine;

public class P_ : MonoBehaviour, IEntity  {

    private EntityProperties _eProperties;
    public EntityProperties EntityProperties { get { return _eProperties; } }

    private void Start() {
        _eProperties._onFall;
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