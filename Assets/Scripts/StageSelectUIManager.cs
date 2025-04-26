using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectUIManager : MonoBehaviour
{
    void Update()
    {
    }
    // 이 메서드는 버튼의 OnClick에 연결해서 스테이지 이름을 넘겨줄 수 있음
    public void OnStageSelected(string stageName)
    {
        // 선택한 스테이지 이름 저장
        PlayerPrefs.SetString("PreviousStage", stageName);
        PlayerPrefs.Save();

        // 페이드로 씬 전환
        if (FadeManager.Instance != null)
            FadeManager.Instance.StartFadeOutAndLoad(stageName);
        else
            SceneManager.LoadScene(stageName);
    }

    // 선택 씬에서 뒤로가기 버튼 눌렀을 때
    public void OnBackToIntro()
    {
        if (FadeManager.Instance != null)
            FadeManager.Instance.StartFadeOutAndLoad("Intro");
        else
            SceneManager.LoadScene("Intro");
    }
    
    public void OnGuide()
    {
        if (FadeManager.Instance != null)
            FadeManager.Instance.StartFadeOutAndLoad("Guide");
        else
            SceneManager.LoadScene("Guide");
    }
}