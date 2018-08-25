using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_PlayerDamageEffect : MonoBehaviour {
    [SerializeField]G20_Player player;
    [SerializeField] Animator anim;
    Image image;
	// Use this for initialization
	void Start () {
        player.recvDamageActions += DamageEffect; 
	}
    void DamageEffect(G20_Unit _unit)
    {
        anim.CrossFadeInFixedTime("CameraYure", 0f);
    }
}
