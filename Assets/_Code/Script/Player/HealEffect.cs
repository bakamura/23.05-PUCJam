using System.Collections;
using UnityEngine;

public class HealEffect : MonoBehaviour {

    private float _currentDuration = 0;
    private Vector3 _initialPos;

    private void OnEnable() {
        StartCoroutine(Activate());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        EntityProperties properties = collision.GetComponent<EntityProperties>();
        if (properties != null) properties.TakeHeal(P_Ability.Instance.HealPower);

    }

    private IEnumerator Activate() {
        _currentDuration = 1;
        _initialPos = (Vector2) P_Movement.Instance.transform.position + P_Ability.Instance.HealEffectSpawnOffset * (P_Ability.Instance.HealEffectRenderer.flipX ? -1f : 1f);
        transform.position = _initialPos;
         
        while (true) {
            transform.position = _initialPos + (P_Ability.Instance.HealEffectRenderer.flipX ? Vector3.left : Vector3.right) * Mathf.Log(_currentDuration);

            _currentDuration += Time.deltaTime / P_Ability.Instance.HealEffectDuration;
            if (_currentDuration >= 2) break;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
