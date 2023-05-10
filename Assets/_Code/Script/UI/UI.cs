using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI : MonoBehaviour {

    protected void ToggleUI(CanvasGroup group, bool open) {
        group.alpha = open ? 1 : 0;
        group.interactable = open;
        group.blocksRaycasts = open;
    }

}
