using UnityEngine;

public class HostileEntity : EntityProperties {

    [Header("Cache")]
    private HostileBehaviour _behaviour;
    public HostileBehaviour Behaviour { get { return _behaviour; } }

    private void Awake() {
        _behaviour = GetComponent<HostileBehaviour>();
    }

}
