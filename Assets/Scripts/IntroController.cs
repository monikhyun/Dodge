using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public GameObject fadeManagerPrefab;
    public GameObject bgmPlayerPrefab;
    void Awake()
    {
        if (FadeManager.Instance == null)
        {
            GameObject obj = Instantiate(fadeManagerPrefab);
            DontDestroyOnLoad(obj);
        }
        if (FindObjectOfType<BGMPlayer>() == null) 
        {
            GameObject bgm = Instantiate(bgmPlayerPrefab);
            DontDestroyOnLoad(bgm);
        }
    }

    public void OnStartButtonClicked()
    {
        StartCoroutine(WaitForFadeAndLoad("StageSelect"));
    }

    System.Collections.IEnumerator WaitForFadeAndLoad(string sceneName)
    {
        // 1프레임 대기: Awake → Instance 초기화 완료 기다림
        yield return null;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.StartFadeOutAndLoad(sceneName);
        }
        else
        {
            Debug.LogWarning("FadeManager.Instance is still null. Using fallback.");
            SceneManager.LoadScene(sceneName);
        }
    }
}