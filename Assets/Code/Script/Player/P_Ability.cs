using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class P_Ability : Singleton<P_Ability> {

    [Header("Heal")]

    [SerializeField] private int _healPower;
    [SerializeField] private float _healDelay;
    [SerializeField] private GameObject _healEffect;

    [Header("Dodge")]

    [SerializeField] private float _dodgeDistance;
    [SerializeField] private float _dodgeIFrameDuration;

    [Header("Events")]

    private UnityEvent _onHeal = new UnityEvent();
    public UnityEvent OnHeal { get { return _onHeal; } }
    private UnityEvent _onDodge = new UnityEvent();
    public UnityEvent OnDodge { get { return _onDodge; } }

    [Header("Cache")]

    private WaitForSeconds _healWait;
    private WaitForSeconds _healRecoilWait;
    private WaitForSeconds _dodgeIFrameWait;
    private WaitForSeconds _dodgeRecoilWait;

    private void Start() {
        _healEffect = Instantiate(_healEffect); // Pool of only one
        _healEffect.SetActive(false);

        _healWait = new WaitForSeconds(_healDelay);
        _healRecoilWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration(P_Animation.P_HEAL) - _healDelay);
        _dodgeIFrameWait = new WaitForSeconds(_dodgeIFrameDuration);
        _dodgeRecoilWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration(P_Animation.P_DODGE) - _dodgeIFrameDuration);
    }

    private void Update() {
        if (P_Movement.Instance.enabled && P_Movement.Instance.PlayerGrounded()) {
            if (InputHandler.Instance.Heal) StartCoroutine(Heal());
            if (InputHandler.Instance.Dodge) StartCoroutine(Dodge());
        }
    }

    private IEnumerator Heal() {
        P_Movement.Instance.RigidBody2D.velocity = Vector2.zero;
        P_Movement.Instance.enabled = false;

        yield return _healWait;

        // Stuff
        //_healEffect.GetComponent<Collider2D>().OverlapCollider();

        yield return _healRecoilWait;

        P_Movement.Instance.enabled = true; // Check if game is paused
    }

    private IEnumerator Dodge() {
        P_Movement.Instance.enabled = false;
        P_Movement.Instance.RigidBody2D.velocity = -Physics2D.gravity.y * (_dodgeIFrameDuration / _dodgeDistance) * ((InputHandler.Instance.Movement != 0 ? InputHandler.Instance.Movement < 0 : P_Animation.Instance.SpriteRenderer.flipX) ? Vector2.left : Vector2.right);

        yield return _dodgeIFrameWait;

        // Stuff
        P_Movement.Instance.RigidBody2D.velocity = Vector2.zero;

        yield return _dodgeRecoilWait;

        P_Movement.Instance.enabled = true;
    }
}