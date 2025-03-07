using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameManagerInstaller : MonoInstaller
{
    
    [SerializeField] private GameManager gameManager;
    
    public override void InstallBindings()
    {
        Container.BindInstance(gameManager).AsSingle();
        
    }
}

