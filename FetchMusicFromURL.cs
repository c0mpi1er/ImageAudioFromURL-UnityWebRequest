using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FetchMusicFromURL : MonoBehaviour
{
    public AudioSource musicSource;
    AudioClip clip;
    public string musicURL;
    public GameObject initStatus;
    public GameObject completeStatus;

    private void Start()
    {
        initStatus.SetActive(true);
        StartCoroutine(DownloadMusic());
    }

    IEnumerator DownloadMusic()
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(musicURL, AudioType.MPEG);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            StartCoroutine(DownloadMusic());
        }
        else
        {
            initStatus.SetActive(false);
            completeStatus.SetActive(true);

            clip = DownloadHandlerAudioClip.GetContent(request);
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}
