using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class FetchImageFromURL: MonoBehaviour
{
    public Image placeholder;
    public string apiURL;
    public string imageURL;
    public GameObject loadingIcon;
    Texture2D texture;

    public void FetchImage()
    {
        loadingIcon.SetActive(true);
        StartCoroutine(GetImageURL(apiURL));
    }

    IEnumerator GetImageURL(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            StartCoroutine(GetImageURL(apiURL));
        } else
        {
            Debug.Log(request.downloadHandler.text);

            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            imageURL = response.url;

            StartCoroutine(DownloadImage(imageURL));
        }   
    }

    IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            StartCoroutine(DownloadImage(imageURL));
        } else
        {
            texture = DownloadHandlerTexture.GetContent(request);
            Sprite tempSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            loadingIcon.SetActive(false);
            placeholder.sprite = tempSprite;
        }
    }

    public class Response
    {
        public string url;
    }
}
