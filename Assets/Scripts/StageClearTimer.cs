using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // 추가

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
    
    public PlayerController player;

    public String nextSceneName = "StageClear";
    
    public FadeManager fadeManager;
    
    private bool isTimerPaused = false;
    
    void Start()
    {
        currentTime = 0f;
        progressBar.value = 0f;

        if (clearUI != null)
            clearUI.SetActive(false);
    }

    void Update()
    {
        if (stageCleared || isPlayerDead || isTimerPaused) return;

        currentTime += Time.deltaTime;

        float percent = Mathf.Clamp01(currentTime / clearTime);
        progressBar.value = percent;

        if (percentText != null)
            percentText.text = (percent * 100f).ToString("F0") + "%";
        
        if (percent >= 1f)
        {
            stageCleared = true;
            if (player != null)
                player.SetInvincibleTrue();
            StartCoroutine(StageClearSequence()); // 코루틴 시작
        }
    }

    private IEnumerator StageClearSequence()
    {
        if (clearUI != null)
            clearUI.SetActive(true);
        if (clearSound != null)
            AudioSource.PlayClipAtPoint(clearSound, Camera.main.transform.position);

        PlayerPrefs.SetString("PreviousStage", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(2f); // 2초 대기

        if (FadeManager.Instance != null)
            FadeManager.Instance.StartFadeOutAndLoad(nextSceneName);
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    
    public void PauseTimer()
    {
        isTimerPaused = true;
    }

    public void ResumeTimer()
    {
        isTimerPaused = false;
    }
}