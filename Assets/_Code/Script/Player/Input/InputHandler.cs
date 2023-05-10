using UnityEngine;

public class InputHandler : Singleton<InputHandler> {

    private float _movement = 0;
    public float Movement { get { return _movement; } }
    private float _jump = 0;
    public bool Jump { get { isTrue = _jump > 0; if(isTrue) _jump = 0; return isTrue; } } // Not Future Proof, maybe put a bool to set to 0 on end of frame
    private float _heal = 0;
    public bool Heal { get { isTrue = _heal > 0; if (isTrue) _heal = 0; return isTrue; } }
    private float _dodge = 0;
    public bool Dodge { get { isTrue = _dodge > 0; if (isTrue) _dodge = 0; return isTrue; } }
    private float _interact = 0;
    public bool Interact { get { isTrue = _interact > 0; if (isTrue) _interact = 0; return isTrue; } }

    [SerializeField] private InputKeys _keys;
    [SerializeField] private float _rememberKeyPress;

    [Header("Cache")]

    private bool isTrue;

    private void Update() {
        GetKeyDown();
        ReduceKeyRemember();
    }

    private void GetKeyDown() {
        _movement = (Input.GetKey(_keys.LeftMovement) ? -1 : 0) + (Input.GetKey(_keys.RightMovement) ? 1 : 0);
        if (Input.GetKeyDown(_keys.Jump)) _jump = _rememberKeyPress;
        if (Input.GetKeyDown(_keys.Heal)) _heal = _rememberKeyPress;
        if (Input.GetKeyDown(_keys.Dodge)) _dodge = _rememberKeyPress;
        if (Input.GetKeyDown(_keys.Interact)) _interact= _rememberKeyPress;
    }

    private void ReduceKeyRemember() {
        if(_jump > 0) _jump -= Time.deltaTime;
        if(_heal > 0) _heal -= Time.deltaTime;
        if(_dodge > 0) _dodge -= Time.deltaTime;
        if(_interact > 0) _interact -= Time.deltaTime;
    }
}
