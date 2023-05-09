using UnityEngine;

public class PersonNormal : EntityProperties {

    [SerializeField] private bool _hasKey;
    [SerializeField] private GameObject _attachedEntity;

    protected override void Start() {
        base.Start();

        if (_hasKey) _onStand.AddListener(GiveKey);
        if (_attachedEntity != null) {
            _attachedEntity = Instantiate(_attachedEntity);
            _attachedEntity.SetActive(!_standing);
        }
    }

    private void GiveKey() {
        Debug.Log("Key received");
    }
}
