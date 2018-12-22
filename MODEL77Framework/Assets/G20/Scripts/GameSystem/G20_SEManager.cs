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
    GESSLER_FLIGHT,
    PLAYER_DAMAGE,
    SMALL_APPLE_ATTACK,
    WALK_WEAK,
    WALK_STRONG,
    WALK_STOP,
    FOREST,
    BARRIER,
    HIGH_SCORE,
	HIT_HEAD_B,
	BARRIER_EXTINCTION,
	APPLE_VOICE_SMALL,
	APPLE_VOICE_NORMAL,
	APPLE_VOICE_NORMAL2,
	HIT_GOLDHEAD_2,
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
    VOICE20,
    VOICE21,
    VOICE22,
    CHAIN1,
    CHAIN2,
    CHAIN3,
    FLIGHTBOSS,

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
            case G20_SEType.GESSLER_FLIGHT:
                retStr = "gesura-flight";
                break;
            case G20_SEType.PLAYER_DAMAGE:
                retStr = "player_damage";
                break;
            case G20_SEType.SMALL_APPLE_ATTACK:
                retStr = "smallapple_attack";
                break;
            case G20_SEType.WALK_WEAK:
                retStr = "walk_weak";
                break;
            case G20_SEType.WALK_STRONG:
                retStr = "walk_strong";
                break;
            case G20_SEType.WALK_STOP:
                retStr = "walk_stop";
                break;
            case G20_SEType.FOREST:
                retStr = "forest_BGM";
                break;
            case G20_SEType.BARRIER:
                retStr = "barrier_pattern2";
                break;
            case G20_SEType.HIGH_SCORE:
                retStr = "score";
                break;
			case G20_SEType.HIT_HEAD_B:
				retStr = "hit_headB";
				break;
			case G20_SEType.BARRIER_EXTINCTION:
				retStr = "barrier_Extinction";
				break;
			case G20_SEType.APPLE_VOICE_SMALL:
				retStr = "apple_voice_small";
				break;
			case G20_SEType.APPLE_VOICE_NORMAL:
				retStr = "apple_voice_normal";
				break;
			case G20_SEType.APPLE_VOICE_NORMAL2:
				retStr = "apple_voice_normal2";
				break;
			case G20_SEType.HIT_GOLDHEAD_2:
				retStr = "hit_gold_2nd";
				break;
			case G20_SEType.TEST_VOICE:
                retStr = "test_voice";
                break;
            case G20_SEType.CHAIN1:
                retStr = "Chain_1";
                break;
            case G20_SEType.CHAIN2:
                retStr = "Chain_2";
                break;
            case G20_SEType.CHAIN3:
                retStr = "Chain_3";
                break;
            case G20_SEType.FLIGHTBOSS:
                retStr = "flight_boss";
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
            //Debug.Log(resourcesName);

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
            Debug.Log("エラー：SE音量設定ミス"+(int)seType);
            audioSource.volume = 1f;
        }

        // 自動消滅時間の設定
        var autoDestroy = obj.GetComponent<G20_AutoDestroy>();
        if ( autoDestroy ) autoDestroy.destroyTime = clip.length + 0.5f;

        // ボイスは必ず鳴って欲しい
        if ( seType >= G20_SEType.TEST_VOICE
            && seType <= G20_SEType.VOICE22 ) audioSource.priority = 118;

        audioSource.Play();

        return audioSource;
    }

    public float GetClipLength(G20_SEType seType)
    {
        return seClips[(int)seType].length;
    }

	public void Fadeout(AudioSource se)
	{
		StartCoroutine( FadeoutCoroutine( se ) );
	}

	IEnumerator FadeoutCoroutine(AudioSource se)
	{
		float defVolume = se.volume;
		for(float t = 0; t < 1f; t += Time.deltaTime )
		{
			se.volume =
				defVolume * ( 1f - t );
			yield return null;
		}
		se.volume = 0f;
		Debug.Log( "フェードアウト完了 : " + se.clip.name );
		se.Stop();
	}
}
