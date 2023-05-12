using System.Collections;
using UnityEngine;

public class P_EProperties : EntityProperties {

    // Singleton because can't inherit from 2 classes
    private static P_EProperties _instance;
    public static P_EProperties Instance { get { return _instance; } }

    [Header("Keys")]

    private int _keyAmount = 0;
    public int KeyAmount { get { return _keyAmount; } set { _keyAmount = value; } }

    // Accessors

    public int HealthCurrent { get { return _healthCurrent; } }

    [Header("Cache")]

    private WaitForSeconds _damagedWait;

    private void Awake() {
        if (_instance == null) _instance = this;
        else if (_instance != this) {
            Debug.LogWarning("Multiple P_EProperties instances found, deleted duplicate"); // Check grammar
            Destroy(gameObject);
        }
    }

    protected override void Start() {
        base.Start();

        _onDamaged.AddListener(Damaged);
        _onFallen.AddListener(Fall);

        _damagedWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration(P_Animation.P_DAMAGED));
    }

    private void Damaged() {
        StartCoroutine(DamagedCoroutine());
    }

    private IEnumerator DamagedCoroutine() {
        P_Movement.Instance.enabled = false;
        P_Movement.Instance.RigidBody2D.velocity = Vector2.zero;

        yield return _damagedWait;

        P_Movement.Instance.enabled = true; // Check if is paused before
    }

    private void Fall() {
        P_Movement.Instance.RigidBody2D.simulated = false;
        // After, UI
    }
}