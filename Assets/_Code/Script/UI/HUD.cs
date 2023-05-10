using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : UI {

    [Header("UI")]

    [SerializeField] private CanvasGroup _hudCanvas;
    [SerializeField] private CanvasGroup _pauseCanvas;
    [SerializeField] private CanvasGroup _settingsCanvas;
    [SerializeField] private CanvasGroup _confirmQuitCanvas;

    [Header("HUD")]

    [SerializeField] private Image _healthImage;

    [Header("Pause")]

    [SerializeField] private LayerMask _pauseLayers;

    [Header("Cache")]

    private List<MonoBehaviour> _behavioursToPause = new List<MonoBehaviour>();
    private P_EProperties _playerProperties;

    private void Start() {
        _playerProperties = FindObjectOfType<P_EProperties>();
        _playerProperties.OnHealthChange.AddListener(UpdateHealthBar);

        Collider2D[] objects = Physics2D.OverlapCircleAll(Vector3.zero, Mathf.Infinity, _pauseLayers);
        MonoBehaviour[] behaviours;
        foreach (Collider2D obj in objects) {
            behaviours = obj.GetComponents<MonoBehaviour>();
            if(behaviours.Length > 0) foreach (MonoBehaviour behaviour in behaviours) if (behaviour.GetType() != typeof(Transform) && behaviour.GetType() != typeof(SpriteRenderer) &&
                behaviour.GetType() != typeof(Rigidbody2D) && behaviour.GetType() != typeof(Collider2D)) _behavioursToPause.Add(behaviour);
        }
    }

    public void PauseGame(bool isPaused) {
        Time.timeScale = isPaused ? 0f : 1f;
        foreach (MonoBehaviour behaviour in _behavioursToPause) behaviour.enabled = !isPaused;

        ToggleUI(_hudCanvas, !isPaused);
        ToggleUI(_pauseCanvas, isPaused);
    }

    public void ToggleSettings(bool open) {
        ToggleUI(_pauseCanvas, !open);
        ToggleUI(_settingsCanvas, open);
    }

    public void ToggleConfirmQuit(bool open) {
        ToggleUI(_confirmQuitCanvas, open);
        ToggleUI(_pauseCanvas, !open); // Not needed if toggle interactable to false, think wich is best
    }

    public void ConfirmQuit() {
        SceneManager.LoadScene(0);
    }

    private void UpdateHealthBar() {
        _healthImage.fillAmount = _playerProperties.CurrentHealthFromTotal();
    }
}
