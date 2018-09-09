using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum G20_AnimType
{
    Attack,
    Run,
    Death,
    Falter,
    Suicide,
    Idle,
    Jump,
    Stance,
    Dash,
}
public class G20_EnemyAnimation : MonoBehaviour {
    public void PlayAnimation(G20_AnimType anim_type,float? anim_speed=null)
    {
        lastAnim = anim_type;
        float AnimationSpeed = animSpeed;
        if (anim_speed!=null)
        {
            AnimationSpeed = (float)anim_speed;
        }
        animator.SetFloat("Speed",AnimationSpeed);
        animator.CrossFadeInFixedTime(Enum.GetName(typeof(G20_AnimType),anim_type),0.4f);
    }
    public G20_AnimType lastAnim=G20_AnimType.Idle;
    [SerializeField] Animator animator;
    float animSpeed;
    public float AnimSpeed{
        set { animSpeed = value; }
        get { return animSpeed; }
    }
}
