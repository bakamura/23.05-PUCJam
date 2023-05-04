using UnityEngine;

public class Inputs : MonoBehaviour {

    private float _movement = 0;
    public float Movement { get { return _movement; } }
    private float _jump = 0;
    public bool Jump { get { bool isTrue = _jump > 0; _jump = 0; return isTrue; } }
    private float _heal = 0;
    public bool Heal { get { bool isTrue = _heal > 0; _heal = 0; return isTrue; } }
    private float _dodge = 0;
    public bool Dodge { get { bool isTrue = _heal > 0; _heal = 0; return isTrue; } }


}
