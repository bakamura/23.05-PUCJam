using UnityEngine;

public class Door : Interactable {

    [Header("Door")]

    [SerializeField] private bool _locked;
    public bool Locked { get { return _locked; } }

    [Header("Change Scene")]

    [SerializeField] private int _sceneToGo;

    private void Start() {
        GetComponentInChildren<SpriteRenderer>().flipX = !_locked;
    }

    public override void Interacted() {
        if (_locked) {
            if (P_EProperties.Instance.KeyAmount > 0) {
                P_EProperties.Instance.KeyAmount--;
                _locked = false;
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
        }
        if(!_locked) FindObjectOfType<HUD>().ChangeScene(_sceneToGo);
    }
}
