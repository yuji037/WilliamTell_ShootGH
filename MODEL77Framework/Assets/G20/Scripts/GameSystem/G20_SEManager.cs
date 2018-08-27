using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum G20_SEType {
    HIT_HEAD,
    HIT_GOLDHEAD,
    HIT_BODY,
    SUMMON_APPLE,
    BOMB,
    APPLE_FALL,
    TEST_VOICE,
}

public static class G20_SEExt {
    public static string GetTypeName(this G20_SEType _type)
    {
        string retStr = "";
        switch ( _type )
        {
            case G20_SEType.HIT_HEAD:
                retStr = "hit_head";
                break;
            case G20_SEType.HIT_GOLDHEAD:
                retStr = "hit_goldhead";
                break;
            case G20_SEType.HIT_BODY:
                retStr = "hit_body";
                break;
            case G20_SEType.SUMMON_APPLE:
                retStr = "enemy";
                break;
            case G20_SEType.BOMB:
                retStr = "bomb";
                break;
            case G20_SEType.APPLE_FALL:
                retStr = "apple_fall";
                break;
            case G20_SEType.TEST_VOICE:
                retStr = "test_voice";
                break;
        }

        return retStr;
    }
}

public class G20_SEManager : G20_Singleton<G20_SEManager> {
    [SerializeField]
    GameObject sePlayPrefab;

    [SerializeField]
    Dictionary<int, AudioClip> seClips = new Dictionary<int, AudioClip>();

    [SerializeField, Range(0f, 1f)]
    float[] seVolumes;

    protected override void Awake()
    {
        base.Awake();
        foreach ( G20_SEType i in Enum.GetValues(typeof(G20_SEType)) )
        {
            string resourcesName = "G20/SE/" + i.GetTypeName();
            Debug.Log(resourcesName);

            seClips.Add((int)i, (AudioClip)Resources.Load(resourcesName, typeof(AudioClip)));
        }
    }

    public void Play(G20_SEType seType, Vector3 position, bool playIn3DVolume = true)
    {
        var obj = Instantiate(sePlayPrefab, transform);
        obj.transform.position = position;
        var audioSource = obj.GetComponent<AudioSource>();
        audioSource.clip = seClips[(int)seType];

        if ( !playIn3DVolume )
        {
            // 3D的なボリューム調節をしない
            audioSource.spatialBlend = 0.0f;
        }

        if ( (int)seType < seVolumes.Length )
        {
            audioSource.volume = seVolumes[(int)seType];
        }
        else
        {
            Debug.Log("エラー：SE音量設定ミス");
            audioSource.volume = 1f;
        }

        audioSource.Play();
    }
}
