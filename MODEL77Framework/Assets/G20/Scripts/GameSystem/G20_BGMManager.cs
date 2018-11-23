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
	BOSS,
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
			case G20_BGMType.BOSS:
				retStr = "boss";
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

    float defaultVolume = 1.0f;
    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        foreach ( G20_BGMType i in Enum.GetValues(typeof(G20_BGMType)) )
        {
            string resourcesName = "G20/BGM/" + i.GetTypeName();
            //Debug.Log(resourcesName);

            bgmClips.Add((int)i, (AudioClip)Resources.Load(resourcesName, typeof(AudioClip)));
        }
        defaultVolume = audioSource.volume;
    }

    public void Play(G20_BGMType bgmType)
    {
        if(!audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClips[(int)bgmType];

        bool isLoopPlay = !( bgmType == G20_BGMType.CLEAR || bgmType == G20_BGMType.GAMEOVER );
        audioSource.loop = isLoopPlay;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public IEnumerator FadeOutCoroutine()
    {
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

    public void VolumeDown(float length)
    {
        StartCoroutine(VolumeDownCoroutine(length));
    }

    IEnumerator VolumeDownCoroutine(float length)
    {
        float downFadeTime = 0.1f;
        float fadeTime = 0.4f;
        float downedVolume = 0.2f;
        for(float t = 0; t < downFadeTime; t += Time.deltaTime )
        {
            audioSource.volume = defaultVolume * ( ( downFadeTime - t ) / downFadeTime ) + downedVolume * ( t / downFadeTime );
            yield return null;
        }

        yield return new WaitForSeconds(length);

        for ( float t = 0; t < fadeTime; t += Time.deltaTime )
        {
            audioSource.volume = defaultVolume * ( ( t ) / fadeTime ) + downedVolume * ( ( fadeTime - t ) / fadeTime );
            yield return null;
        }
    }
}
