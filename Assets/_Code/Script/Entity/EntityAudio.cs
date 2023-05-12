using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudio : AudioHandler {

    public const string E_HEALED = "E_Healed";
    public const string E_DAMAGED = "E_Damaged";

    protected virtual void Start() {
        EntityProperties properties = GetComponent<EntityProperties>();
        properties.OnHealed.AddListener(HealedSFX);
        properties.OnHealed.AddListener(DamagedSFX);
    }

    private void HealedSFX() {
        PlaySound(E_HEALED);
    }

    private void DamagedSFX() {
        PlaySound(E_DAMAGED);
    }
}
