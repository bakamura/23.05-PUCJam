using System.Collections;
using UnityEngine;

public class P_EProperties : EntityProperties {

    [Header("Cache")]

    private WaitForSeconds _damagedWait;

    protected override void Start() {
        base.Start();

        _onDamaged.AddListener(Damaged);
        _onFallen.AddListener(Fall);
        _onHealthChange.AddListener(HealthChange);

        _damagedWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration(P_Animation.P_DAMAGED));
    }

    private void Damaged() {
        StartCoroutine(DamagedCoroutine());
    }

    private IEnumerator DamagedCoroutine() {
        P_Movement.Instance.enabled = false;
        
        yield return _damagedWait;

        P_Movement.Instance.enabled = true; // Check if is paused before
    }

    private void Fall() {
        P_Movement.Instance.RigidBody2D.simulated = false;
        // After, UI
    }

    private void HealthChange() {
        // Update UI
    }
}