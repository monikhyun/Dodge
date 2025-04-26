using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
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
            // 숫자만 뽑기 (Stage5Game → 5)
            Match match = Regex.Match(currentStage.Substring(5), @"^\d+"); 
            if (match.Success && int.TryParse(match.Value, out int stageNum))
            {
                if (stageNum == 6)
                    return "EndingScene";

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