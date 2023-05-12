using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayBtn() {
        StartCoroutine(EnterGame());
    }

    private IEnumerator EnterGame() { //provisry
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        yield return null;
    }

    public void QuitBtn() {
        Application.Quit();
    }
}