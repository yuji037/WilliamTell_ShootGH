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
    VOICE0,
    VOICE1,
    VOICE2,
    VOICE3,
    VOICE4,
    VOICE5,
    VOICE6,
    VOICE7,
    VOICE8,
    VOICE9,
    VOICE10,
    VOICE11,
    VOICE12,
    VOICE13,
    VOICE14,
    VOICE15,
    VOICE16,
    VOICE17,
    VOICE18,
    VOICE19,
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
            default:
                retStr = "voice" + ((int)( _type - G20_SEType.VOICE0 )).ToString();
                break;
        }

        return retStr;
    }
}

public class G20_SEManager : G20_Singleton<G20_SEManager> {
    [SerializeField]
    GameObject sePlayPrefab;

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

    public AudioSource Play(G20_SEType seType, Vector3 position, bool playIn3DVolume = true)
    {
        var obj = Instantiate(sePlayPrefab, transform);
        obj.transform.position = position;
        var audioSource = obj.GetComponent<AudioSource>();
        var clip = seClips[(int)seType];
        if ( clip == null ) return audioSource;
        audioSource.clip = clip;

        if ( !playIn3DVolume )
        {
            // 3D的なボリューム調節をしない
            audioSource.spatialBlend = 0f;
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

        // 自動消滅時間の設定
        var autoDestroy = obj.GetComponent<G20_AutoDestroy>();
        if ( autoDestroy ) autoDestroy.destroyTime = clip.length + 0.5f;

        // ボイスは必ず鳴って欲しい
        if ( seType >= G20_SEType.TEST_VOICE
            && seType <= G20_SEType.VOICE19 ) audioSource.priority = 118;

        audioSource.Play();

        return audioSource;
    }
}
