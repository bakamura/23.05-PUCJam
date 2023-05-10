using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T _instance;
    public static T Instance { get { return _instance; } }

    // Use base.Awake() when creating more functionality for Awake in inheriting class
    protected virtual void Awake() {
        if (_instance == null) _instance = this as T;
        else if (_instance != this) {
            Destroy(gameObject);
            Debug.LogWarning("Multiple " + typeof(T).Name + " instances exist, duplicate deleted");
        }
    }

}
