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
        using UnityWebRequest request = UnityWebRequest.Get(imageUrl);

        try
        {
            await request.SendWebRequest().ToUniTask();
        }
        catch (UnityWebRequestException ex)
        {
            LogRequestError(request, imageUrl, ex.Message);
            return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            LogRequestError(request, imageUrl, request.error);
            return null;
        }

        byte[] imageBytes = request.downloadHandler.data;
        if (imageBytes == null || imageBytes.Length == 0)
        {
            Debug.LogError($"Image response was empty: {imageUrl}");
            return null;
        }

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (texture.LoadImage(imageBytes) == false)
        {
            Debug.LogError(
                $"Failed to decode image from '{imageUrl}'. Content-Type: '{request.GetResponseHeader("Content-Type")}', " +
                $"ResponseCode: {request.responseCode}");
            Object.Destroy(texture);
            return null;
        }

        return texture;
    }

    private static void LogRequestError(UnityWebRequest request, string imageUrl, string error)
    {
        string contentType = request.GetResponseHeader("Content-Type");
        string responsePreview = string.Empty;

        if (string.IsNullOrEmpty(contentType) == false && contentType.Contains("text"))
        {
            string text = request.downloadHandler?.text;
            if (string.IsNullOrEmpty(text) == false)
            {
                responsePreview = text.Substring(0, Mathf.Min(200, text.Length));
            }
        }

        Debug.LogError(
            $"Failed to load image from '{imageUrl}'. Error: {error}. ResponseCode: {request.responseCode}. " +
            $"Content-Type: '{contentType}'. Preview: {responsePreview}");
    }
}
