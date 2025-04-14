using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearTimer : MonoBehaviour
{
    [SerializeField] private AudioClip clearSound;
    public float clearTime = 30f; // 클리어까지 걸리는 시간
    private float currentTime;

    public TextMeshProUGUI percentText;
    public Slider progressBar;
    public GameObject clearUI; // 스테이지 클리어 시 보여줄 UI

    public bool isPlayerDead = false;
    private bool stageCleared = false;

    public String nextSceneName = "StageClear";
    
    public FadeManager fadeManager;
    void Start()
    {
        currentTime = 0f;
        progressBar.value = 0f;

        if (clearUI != null)
            clearUI.SetActive(false);
    }

    void Update()
    {
        if (stageCleared || isPlayerDead) return;

        currentTime += Time.deltaTime;

        float percent = Mathf.Clamp01(currentTime / clearTime);
        progressBar.value = percent;

        if (percentText != null)
            percentText.text = (percent * 100f).ToString("F0") + "%";
        
        if (percent >= 1f)
        {
            stageCleared = true;

            if (clearUI != null)
                clearUI.SetActive(true);
            if (clearSound != null)
                AudioSource.PlayClipAtPoint(clearSound, Camera.main.transform.position);
            PlayerPrefs.SetString("PreviousStage", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
            
            
            if (FadeManager.Instance != null)
                FadeManager.Instance.StartFadeOutAndLoad(nextSceneName);
            else
            {
                Debug.LogWarning("FadeManager.Instance is null. Fallback to SceneManager.LoadScene");
                SceneManager.LoadScene(nextSceneName);
            }
// 혹시 모를 예외 처리
        }

    }
}