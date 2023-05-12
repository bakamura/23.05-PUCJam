using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Image[] _healthImages;
    [SerializeField] private Sprite _healthFull;
    [SerializeField] private Sprite _healthHalf;

    [Header("Pause")]

    [SerializeField] private LayerMask _pauseLayers;

    [Header("Fade")]

    [SerializeField] private CanvasGroup _fadeCanvas;
    [SerializeField] private float _fadeHalfDuration;

    [Header("Fallen Screen")]

    [SerializeField] private CanvasGroup _fallenCanvas;

    [Header("Cache")]

    private List<MonoBehaviour> _behavioursToPause = new List<MonoBehaviour>();
    private P_EProperties _playerProperties;
    private Color _transparent = new Color(0, 0, 0, 0);
    private WaitForSeconds _fallenWait;

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
        _fallenWait = new WaitForSeconds(P_Animation.Instance.GetAnimationDuration(P_Animation.P_FALLEN));
        P_EProperties.Instance.OnFallen.AddListener(FadeFallen);
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
        int playerHealth = _playerProperties.HealthCurrent;
        for (int i = 0; i < _healthImages.Length; i++) {
            if (playerHealth > 1) {
                playerHealth -= 2;
                _healthImages[i].sprite = _healthFull;
                _healthImages[i].color = Color.white;
            }
            else if (playerHealth > 0) {
                playerHealth -= 1;
                _healthImages[i].sprite = _healthHalf;
                _healthImages[i].color = Color.white;
            }
            else _healthImages[i].color = _transparent;
        }
    }

    public void ChangeScene(int id) {
        StartCoroutine(FadeChangeScene(id));
        GameManager.currentRespawn = P_Movement.Instance.transform.position;
    }

    private IEnumerator FadeChangeScene(int sceneID) {
        P_Movement.Instance.enabled = false;
        P_Movement.Instance.RigidBody2D.velocity = Vector2.zero;
        P_Ability.Instance.enabled = false;

        while (_fadeCanvas.alpha < 1) {
            _fadeCanvas.alpha += Time.deltaTime / _fadeHalfDuration;
         
            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
        Scene sceneToUnload = SceneManager.GetActiveScene();

        while (!asyncLoad.isDone) yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneID));
        SceneManager.UnloadSceneAsync(sceneToUnload);

        while (_fadeCanvas.alpha > 0) {
            _fadeCanvas.alpha -= Time.deltaTime / _fadeHalfDuration;

            yield return null;
        }

        P_Movement.Instance.enabled = true;
        P_Ability.Instance.enabled = true;
    }

    private void FadeFallen() {
        StartCoroutine(FadeFallenRoutine());
    }

    private IEnumerator FadeFallenRoutine() {
        yield return _fallenWait;

        while (_fadeCanvas.alpha < 1) { //
            _fadeCanvas.alpha += Time.deltaTime / _fadeHalfDuration; //

            yield return null;
        }

        P_Movement.Instance.transform.position = GameManager.currentRespawn;
        P_EProperties.Instance.TakeHeal(10);

        while (_fadeCanvas.alpha > 0) { //
            _fadeCanvas.alpha -= Time.deltaTime / _fadeHalfDuration; //

            yield return null;
        }

        P_Movement.Instance.enabled = true;
    }
}
