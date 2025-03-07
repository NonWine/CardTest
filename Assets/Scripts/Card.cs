using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Card : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _mainObj;
    [Inject] private GameManager _gameManager;

    public bool isALive { get; private set; }
    
    [ShowInInspector, ReadOnly] public int Id { get; private set; }
    [ShowInInspector, ReadOnly] public bool IsFlipped { get; private set; }
    
    private void Awake()
    {
        _button.onClick.AddListener(ShowCard);
    }

    private void OnDestroy()
    {
        _button.onClick.AddListener(ShowCard);
    }
    
    public void Initialize(ImagesData imagesData)
    {
        Id = imagesData.Id;
        _img.sprite = imagesData.Sprite;
    }

    public void HideCard()
    {
        _mainObj.DORotate(new Vector3(0f, -180f, 0f), 0.25f).SetEase(Ease.Linear).SetDelay(0.5f);
        IsFlipped = false;
    }

    public void RemoveCard()
    {
        _mainObj.DOShakeRotation(0.25f, 40f, 5,0).OnComplete(() =>
        {
            Invoke(nameof(OffObj), 0.15f);
        }).SetDelay(0.2f).SetEase(Ease.OutBack);
    }
    
    public void ResetCard() => gameObject.SetActive(true);
    
    private void ShowCard()
    {
        if(IsFlipped || _gameManager.IsFlipState) return;
        
        _mainObj.DORotate(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.OutBack); 
        IsFlipped = true;
        _gameManager.SetFlipCard(this);
    }

    private void OffObj() => gameObject.SetActive(false);
}