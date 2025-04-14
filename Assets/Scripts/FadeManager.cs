using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    public CanvasGroup fadeGroup;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            fadeGroup.alpha = 0; // 게임 시작 시 검은 화면 X
            fadeGroup.blocksRaycasts = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFadeOutAndLoad(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeGroup.blocksRaycasts = true;
        fadeGroup.alpha = 1;

        while (fadeGroup.alpha > 0)
        {
            fadeGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        fadeGroup.blocksRaycasts = false;
        fadeGroup.alpha = 0;
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        fadeGroup.blocksRaycasts = true;
        fadeGroup.alpha = 0;

        while (fadeGroup.alpha < 1)
        {
            fadeGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        yield return SceneManager.LoadSceneAsync(sceneName);

        StartCoroutine(FadeIn()); // 씬 전환 후 자동 페이드 인
    }
}