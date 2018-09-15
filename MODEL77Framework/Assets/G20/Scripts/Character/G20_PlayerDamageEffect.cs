using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_PlayerDamageEffect : MonoBehaviour {
    [SerializeField]G20_Player player;
    [SerializeField] Animator anim;
    G20_CameraShake cameraShake;
    Image image;
	// Use this for initialization
	void Start () {
        player.recvDamageActions += DamageEffect;
        cameraShake = Camera.main.GetComponent<G20_CameraShake>();
    }
    void DamageEffect(G20_Unit _unit)
    {
        //anim.CrossFadeInFixedTime("CameraYure", 0f);
        if ( cameraShake ) cameraShake.Shake(G20_CameraShakeType.DOWNWARD, 0.2f, 6f, 0.8f);
        G20_SEManager.GetInstance().Play(G20_SEType.PLAYER_DAMAGE, Vector3.zero, false);
    }
}
