using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsessiveAnimation : AnimationHandler {

    [Header("Cache")]

    private const string OBSESSIVE_MOVE = "Obsessive_Move";
    private const string OBSESSIVE_ATTACK = "Obsessive_Attack";
    public const string OBSESSIVE_DODGED = "Obsessive_Dodged";

    private void Start() {
        ObsessiveBehaviour behaviour = GetComponent<ObsessiveBehaviour>();
        behaviour.OnMove.AddListener(MoveAnimation);
        behaviour.OnAttack.AddListener(AttackAnimation);
        behaviour.OnDodged.AddListener(DodgedAnimation);
    }

    private void MoveAnimation() {
        ChangeAnimation(OBSESSIVE_MOVE);
    }

    private void AttackAnimation() {
        ChangeAnimation(OBSESSIVE_ATTACK);
    }

    private void DodgedAnimation() {
        ChangeAnimation(OBSESSIVE_DODGED);
    }
}
