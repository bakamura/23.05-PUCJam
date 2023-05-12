using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBehaviour : MonoBehaviour {

    protected EntityProperties _properties;

    protected virtual void Awake() {
        _properties = GetComponent<EntityProperties>();
    }
}
