using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable] [InlineEditor] [HideReferenceObjectPicker]
public class ImagesData
{
    [PreviewField] public readonly Sprite Sprite;
    public readonly int Id;

    public ImagesData(Sprite sprite, int id)
    {
        Sprite = sprite;
        Id = id;
    }
}