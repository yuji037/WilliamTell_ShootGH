using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum G20_BGMType {
    TITLE,
    INGAME_0,
    INGAME_1,
    INGAME_2,
    CLEAR,
    GAMEOVER,
}

public static class G20_BGMExt {
    public static string GetTypeName(this G20_BGMType _type)
    {
        string retStr = "";
        switch ( _type )
        {
            case G20_BGMType.TITLE:
                retStr = "title";
                break;
            case G20_BGMType.INGAME_0:
                retStr = "stageBGM";
                break;
            case G20_BGMType.INGAME_1:
                retStr = "";
                break;
            case G20_BGMType.INGAME_2:
                retStr = "";
                break;
            case G20_BGMType.CLEAR:
                retStr = "gameclear";
                break;
            case G20_BGMType.GAMEOVER:
                retStr = "gameover";
                break;
        }

        return retStr;
    }
}

public class G20_BGMManager : G20_Singleton<G20_BGMManager> {

    [SerializeField]
    float fadeOutSpan = 1.0f;

    [SerializeField]
    Dictionary<int, AudioClip> bgmClips = new Dictionary<int, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
        foreach ( G20_BGMType i in Enum.GetValues(typeof(G20_BGMType)) )
        {
            string resourcesName = "G20/BGM/" + i.GetTypeName();
            Debug.Log(resourcesName);

            bgmClips.Add((int)i, (AudioClip)Resources.Load(resourcesName, typeof(AudioClip)));
        }
    }

    public void Play(G20_BGMType bgmType)
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClips[(int)bgmType];

        bool isLoopPlay = !( bgmType == G20_BGMType.CLEAR || bgmType == G20_BGMType.GAMEOVER );
        audioSource.loop = isLoopPlay;
        audioSource.Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
        var audioSource = GetComponent<AudioSource>();
        float startVolume = audioSource.volume;

        for (float t = fadeOutSpan; t > 0; t-=Time.deltaTime )
        {
            audioSource.volume = (t / fadeOutSpan) * startVolume;
            yield return null;
        }

        audioSource.volume = 0.0f;
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
