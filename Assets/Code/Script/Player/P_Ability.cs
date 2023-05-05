using System.Collections;
using UnityEngine;

public class P_Ability : Singleton<P_Ability> {

    [Header("Heal")]

    [SerializeField] private int _healPower;

    [Header("Dodge")]

    [SerializeField] private float _dodgeDistance;
    [SerializeField] private float _dodgeIFrameDuration;

    [Header("Cache")]

    private WaitForSeconds _healWait;
    private WaitForSeconds _healRecoilWait;
    private WaitForSeconds _dodgeIFrameWait;
    private WaitForSeconds _dodgeRecoilWait;

    protected override void Awake() {
        base.Awake();

        // Check name after !!!
        _healWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration("P_Heal"));
        _healRecoilWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration("P_Heal") - _dodgeIFrameDuration);
        _dodgeIFrameWait = new WaitForSeconds(_dodgeIFrameDuration);
        _dodgeRecoilWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration("P_Dodge") - _dodgeIFrameDuration);
    }

    private void Update() {
        if (P_Movement.Instance.enabled && InputHandler.Instance.Heal) StartCoroutine(Heal());
        if (P_Movement.Instance.enabled && InputHandler.Instance.Dodge) StartCoroutine(Dodge());
    }

    private IEnumerator Heal() {
        P_Movement.Instance.enabled = false;

        yield return _healWait;

        // Stuff

        yield return _healRecoilWait;

        P_Movement.Instance.enabled = true;
    }

    private IEnumerator Dodge() {
        P_Movement.Instance.enabled = false;
        P_Movement.Instance.RigidBody2D.velocity = (_dodgeDistance / _dodgeIFrameDuration) * (P_Animation.Instance.SpriteRenderer.flipX ? Vector2.left : Vector2.right);

        yield return _dodgeIFrameWait;

        // Stuff
        P_Movement.Instance.RigidBody2D.velocity = Vector2.zero;

        yield return _dodgeRecoilWait;

        P_Movement.Instance.enabled = true;
    }
}
