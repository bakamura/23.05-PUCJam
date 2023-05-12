using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarAnimation : AnimationHandler {

    [Header("Cache")]

    private BoarBehaviour _behaviour;
    private const string BOAR_IDLE = "Boar_Idle";
    private const string BOAR_RUN = "Boar_Run";

    private void Start() {
        _behaviour= GetComponent<BoarBehaviour>();
        _behaviour.OnIdle.AddListener(IdleAnimation);
        _behaviour.OnRun.AddListener(RunAnimation);
    }

    private void Update() {
        if (_behaviour.Rigidbody2D.velocity.magnitude == 0) _sr.flipX = (P_Movement.Instance.transform.position.x - transform.position.x) < 0;
        else _sr.flipX = _behaviour.Rigidbody2D.velocity.x < 0;
    }

    private void IdleAnimation() {
        ChangeAnimation(BOAR_IDLE);
    }

    private void RunAnimation() {
        ChangeAnimation(BOAR_RUN);
    }
}
