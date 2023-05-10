using System.Collections;
using UnityEngine;

public class P_EProperties : EntityProperties {

    [Header("Keys")]

    private int _keyAmount = 0;
    public int KeyAmount { get { return _keyAmount; } set { _keyAmount = value; } }

    [Header("Cache")]

    private WaitForSeconds _damagedWait;

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
        _onDamaged.Invoke();
        P_Movement.Instance.enabled = false;

        yield return _damagedWait;

        P_Movement.Instance.enabled = true; // Check if is paused before
    }

    private void Fall() {
        _onFallen.Invoke();
        P_Movement.Instance.RigidBody2D.simulated = false;
        // After, UI
    }

    public float CurrentHealthFromTotal() {
        return _healthCurrent / _healthMax;
    }
}