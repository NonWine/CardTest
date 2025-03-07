using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class AsyncImageLoader
{
    public static async UniTask<List<ImagesData>> LoadImages(JsonDataContainer dataContainer)
    {
        List<UniTask<Texture2D>> textureTasks = new List<UniTask<Texture2D>>();
        foreach (var dataImage in dataContainer.images)
        {
            textureTasks.Add(LoadTextureAsync(dataImage.url));
        }

        Texture2D[] textures = await UniTask.WhenAll(textureTasks);

        List<ImagesData> imagesData = new List<ImagesData>();
        
        for (int i = 0; i < textures.Length; i++)
        {
            if (textures[i] != null)
            {
                imagesData.Add(new ImagesData(textures[i].ConvertToSprite(), dataContainer.images[i].id));
            }
        }

        return imagesData;
    }

    private static async UniTask<Texture2D> LoadTextureAsync(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
    
        await request.SendWebRequest().ToUniTask();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        return texture;
    }
}