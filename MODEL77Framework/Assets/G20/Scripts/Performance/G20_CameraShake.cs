using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum G20_CameraShakeType {
    DOWN_DIRECTION,
    UP_DIRECTION,
    LEFT_DIRECTION,
    RIGHT_DIRECTION,
    DOWNWARD,
    RANDOM_DIRECTION,
}


public class G20_CameraShake : MonoBehaviour {

    public bool beginShake = false;
    public G20_CameraShakeType ShakeType;
    [Range(0f, 1f)]
    public float Strength = 0.5f;
    [Range(0f, 20f)]
    public float Frequency = 5f;
    [Range(0f, 5f)]
    public float TimeLength = 1f;

    Vector3 defaultPos;

	// Use this for initialization
	void Start () {
        defaultPos = transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = defaultPos;
        if ( beginShake )
        {
            Shake(ShakeType, Strength, Frequency, TimeLength);
            beginShake = false;
        }
    }

    public void Shake(G20_CameraShakeType shakeType, float strength, float frequency, float timeLength)
    {
        float rad = 0f;

        // 振る方向の決定
        switch ( shakeType )
        {
            case G20_CameraShakeType.DOWN_DIRECTION:
                rad = Mathf.PI * 1.5f;
                break;
            case G20_CameraShakeType.UP_DIRECTION:
                rad = Mathf.PI * 0.5f;
                break;
            case G20_CameraShakeType.LEFT_DIRECTION:
                rad = Mathf.PI * 1f;
                break;
            case G20_CameraShakeType.RIGHT_DIRECTION:
                rad = 0f;
                break;
            case G20_CameraShakeType.DOWNWARD:
                rad = Random.Range(Mathf.PI * 1f, Mathf.PI * 2f);
                break;
            case G20_CameraShakeType.RANDOM_DIRECTION:
                rad = Random.Range(0f, Mathf.PI * 2f);
                break;
        }

        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

        StartCoroutine(ShakeDirectionCoroutine(dir, strength, frequency, timeLength));
    }

    public void ShakeDirection(Vector2 dir, float strength, float frequency, float timeLength)
    {
        if ( dir == Vector2.zero ) return;

        dir = dir.normalized;
        StartCoroutine(ShakeDirectionCoroutine(dir, strength, frequency, timeLength));
    }

    IEnumerator ShakeDirectionCoroutine(Vector2 dir, float strength, float freq, float timeLen)
    {
        Vector3 _dir = new Vector3(dir.x, dir.y, 0);

        float fDefaultStrength = strength;
        Vector3 vShake = dir * fDefaultStrength;

        float period = 1f / freq;
        float radRate = 2 * Mathf.PI / period;

        for (float t = TimeLength; t > 0; t -= Time.unscaledDeltaTime )
        {
            float str = fDefaultStrength * t / timeLen;
            vShake = dir * str * Mathf.Sin(( TimeLength - t ) * radRate);
            transform.localPosition += vShake;
            yield return null;
        }
    }
}
