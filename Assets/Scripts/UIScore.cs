using System;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    private int _currentScore;

    private void Awake()
    {
        SetZeroPoints();
        GameManager.OnCardFlipResult += AddScore;
        CardUIManager.OnRestartCards += SetZeroPoints;
    }

    private void OnDestroy()
    {
        GameManager.OnCardFlipResult -= AddScore;
        CardUIManager.OnRestartCards -= SetZeroPoints;
    }

    private void AddScore(bool result)
    {
        if(result == false) return;
        
        _currentScore++;
        _scoreText.text = "Score: " + _currentScore.ToString();
    }

    private void SetZeroPoints()
    {
        _currentScore = 0;
        _scoreText.text = "Score: 0"; 

    }
}