using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _restartingScene = false;

    public bool RestartCurrentScene() {
        if (_restartingScene) return false;
        _restartingScene = true;
        _RestartScene();
        return true;
    }

    public bool RestartCurrentScene(float waitTime) {
        if (_restartingScene) return false;
        _restartingScene = true;
        StartCoroutine(_RestartScene(waitTime));
        return true;
    }


    private void _RestartScene() {
        Debug.Log("Restarting scene");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private IEnumerator _RestartScene(float waitTime) {
        Debug.Log("Restarting scene after " + waitTime.ToString() + " seconds...");
        yield return new WaitForSeconds(waitTime);
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
