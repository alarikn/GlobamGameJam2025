using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Animator baseScoreAnimator;
    [SerializeField] private Animator multiScoreAnimator;

    [SerializeField] private TextMeshProUGUI baseScoreTMP;
    [SerializeField] private TextMeshProUGUI multiScoreTMP;

    [SerializeField] private Animator finalScoreAnimator;
    [SerializeField] private GameObject finalScorePanel;
    [SerializeField] private TextMeshProUGUI finalScoreTMP;

    private int baseScore = 0;
    private int multiScore = 1;
    private int finalScore = 0;

    private int scoreTemp;
    private int multiTemp;
    private int finalTemp;


    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
        finalScorePanel.SetActive(false);
    }

    public void ResetScoring()
    {
        baseScore = 0;
        multiScore = 1;
        finalScore = 0;
        baseScoreAnimator.Play("ResetScore", 0, 0);
        multiScoreAnimator.Play("ResetScore", 0, 0);
        baseScoreTMP.text = baseScore.ToString();
        multiScoreTMP.text = "X" + multiScore.ToString();
        finalScoreTMP.text = finalScore.ToString();
    }

    public void AddScoring(int scoreAmount, int multiAmount)
    {
        AddToBaseScore(scoreAmount);
        AddToMulti(multiAmount);
    }

    public void AddToBaseScore(int amount)
    {
        if (amount == 0) return;
        scoreTemp = baseScore;
        baseScore += amount;
        baseScoreAnimator.Play("AddScore", 0, 0);
        StartCoroutine(AddScoreBehaviour());
    }

    public void AddToMulti(int amount)
    {
        if (amount == 0) return;
        multiTemp = multiScore;
        multiScore += amount;
        multiScoreAnimator.Play("AddScore", 0, 0);
        StartCoroutine(AddMultiBehaviour());
    }

    private IEnumerator AddScoreBehaviour()
    {
        int currentScore = scoreTemp;
        while (currentScore < baseScore)
        {
            currentScore++;
            baseScoreTMP.text = currentScore.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        baseScoreTMP.text = baseScore.ToString();
    }

    private IEnumerator AddMultiBehaviour()
    {
        int currentMulti = multiTemp;
        while (currentMulti < multiScore)
        {
            currentMulti++;
            multiScoreTMP.text = "X" + currentMulti.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        multiScoreTMP.text = "X" + multiScore.ToString();
    }

    public void AddScoreToFinal()
    {
        baseScoreAnimator.Play("ResetScore",0,0);
        finalScoreAnimator.Play("AddScore", 0, 0);
        baseScoreTMP.text = 0.ToString();
        finalScore = baseScore;
        StartCoroutine(AddFinalBehaviour());
    }

    public void AddMultiToFinal()
    {
        multiScoreAnimator.Play("ResetScore", 0, 0);
        finalScoreAnimator.Play("AddScore", 0, 0);
        multiScoreTMP.text = "X" + 1.ToString();
        finalScore = baseScore * multiScore;
        StartCoroutine(AddFinalBehaviour());
    }

    public void AddPreferenceMulti(float multiplier)
    {
        finalScore = (int)(finalScore * multiplier);
        finalScoreAnimator.Play("AddScore", 0, 0);
        StartCoroutine(AddFinalBehaviour());
    }

    private IEnumerator AddFinalBehaviour()
    {
        int currentFinal = finalTemp;
        while (currentFinal < finalScore)
        {
            var addition = 1;
            var diff = finalScore - currentFinal;
            if (diff > 10) addition = 5;
            if (diff > 50) addition = 20;
            if (diff > 100) addition = 50;

            currentFinal += addition;
            finalScoreTMP.text = currentFinal.ToString();
            yield return new WaitForSeconds(0.02f);
        }
        finalScoreTMP.text = finalScore.ToString();
    }

    public void ShowFinalScoring(bool value)
    {
        finalScorePanel.SetActive(value);
    }
}
