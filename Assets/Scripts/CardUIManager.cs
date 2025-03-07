using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CardUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private Card _cardPrefab;
    [Inject] private DiContainer _diContainer;
    [Inject] private DataManager _dataManager;
    [Inject] private GameManager _gameManager;
    
    [ShowInInspector, ReadOnly] private List<Card> _currentCards = new List<Card>();

    public static event Action OnRestartCards;
    
    private void Awake()
    {
        GameManager.OnCardFlipResultAfterDelay += CheckCards;
    }

    private void Start()
    {
        CreateCards();
        SortRandomCards();
        HideCards();
    }

    private void OnDestroy()
    {
        GameManager.OnCardFlipResultAfterDelay -= CheckCards;
    }

    private void CreateCards()
    {
        _currentCards.Clear();
        
        foreach (var image in _dataManager.Images)
        {
            for (int i = 0; i < _gameManager.CombinationCount; i++)
            {
                var card = _diContainer.InstantiatePrefabForComponent<Card>(_cardPrefab, _container);
                card.Initialize(image);
                _currentCards.Add(card);
            }
        }
    }

    private void CheckCards(bool result)
    {
        if(result == false) return;
        
        bool hasCards = _currentCards.All(x => !x.gameObject.activeSelf);

        if (hasCards)
        {
            Debug.Log("Restart");
            OnRestartCards?.Invoke();
            SortRandomCards();
            HideCards();
        }
    }

    private void SortRandomCards()
    {
        for (int i = _currentCards.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (_currentCards[i], _currentCards[j]) = (_currentCards[j], _currentCards[i]);
        }

        for (var i = 0; i < _currentCards.Count; i++)
        {
            _currentCards[i].transform.SetSiblingIndex(i);
        }
    }

    private void HideCards()
    {
        foreach (var currentCard in _currentCards)
        {
            currentCard.ResetCard();
            currentCard.HideCard();
        }
    }
}