using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageClearUI : MonoBehaviour
{
    private string nextStageName;
    

    void Start()
    {
        string previousStage = PlayerPrefs.GetString("PreviousStage", "Stage1"); // 기본값은 Stage1
        nextStageName = GetNextStageName(previousStage);
    }

    string GetNextStageName(string currentStage)
    {
        if (currentStage.StartsWith("Stage"))
        {
            string numberStr = currentStage.Substring(5); // "Stage1" → "1"
            if (int.TryParse(numberStr, out int stageNum))
            {
                if (stageNum == 4)
                    return "Ending";

                int nextStageNum = stageNum + 1;
                return "Stage" + nextStageNum;
            }
        }
        return "Stage1"; // fallback
    }

    public void OnContinueButtonClicked()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.StartFadeOutAndLoad(nextStageName);
        }
        else
        {
            SceneManager.LoadScene(nextStageName);
        }
    }

    public void OnBackToSelectClicked()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.StartFadeOutAndLoad("StageSelect");
        }
        else
        {
            SceneManager.LoadScene("StageSelect");
        }
    }
    public void OnQuitButtonClicked()
    {
        Application.Quit();

    }
}