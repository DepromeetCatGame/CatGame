using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class ImageSizeGetter
{
    public static float GetWidth(GameObject gameObject)
    {
        var image = gameObject.GetComponent<Image>();
        if (image != null)
            return image.rectTransform.rect.width;
        else
        {
            var images = gameObject.GetComponentsInChildren<Image>();
            if (images == null)
                return 0;
            else
                return images.Sum(i => i.rectTransform.rect.width);
        }
    }
}