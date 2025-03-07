using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class GoogleDriveDataController : MonoBehaviour
{
    [SerializeField] private string _urlJson;
    
    [ShowInInspector] public bool IsInitialized { get; private set; }
    
    [ReadOnly]  [ShowInInspector]  public DataManager DataManager { get; private set; }
    
    private async void Awake()
    {
        await LoadDataFromGoogle(_urlJson);
        ProjectContext.Instance.Container.BindInstance(DataManager);
        IsInitialized = true;
    }

    private async UniTask LoadDataFromGoogle(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return;
        }

        JsonDataContainer data = JsonUtility.FromJson<JsonDataContainer>(request.downloadHandler.text);
        
        var imagesData = await AsyncImageLoader.LoadImages(data);
        DataManager = new DataManager(imagesData);
        Debug.Log("Init From Server");

    }
}
