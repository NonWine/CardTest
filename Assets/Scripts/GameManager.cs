using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [ShowInInspector, ReadOnly] public bool IsFlipState { get; private set; }
    [field: SerializeField] public int CombinationCount { get; private set; }

    private TmpCardContainer _cardContainer;

    public static event Action<bool> OnCardFlipResult;

    public static event Action<bool> OnCardFlipResultAfterDelay;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        
        _cardContainer = new TmpCardContainer();
    }

    public void SetFlipCard(Card card)
    {
        if(_cardContainer.TemporallyCards.Contains(card)) return;
        
        _cardContainer.TemporallyCards.Add(card);
        CheckDoubleFlipCards();
    }

    private async void CheckDoubleFlipCards()
    {
        if(_cardContainer.TemporallyCards.Count < CombinationCount) return;
        
        IsFlipState = true;
        var result = _cardContainer.CheckWinLose();
        
        OnCardFlipResult?.Invoke(result);
        await UniTask.Delay(1000);
        OnCardFlipResultAfterDelay?.Invoke(result);
        
        IsFlipState = false;
    }
}