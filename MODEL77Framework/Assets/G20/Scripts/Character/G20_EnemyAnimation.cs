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
    DynamicDeath,
    PauseDeath,
    FrontDeath,
    SmashDeath,
    RageDeath,
}

public class G20_EnemyAnimation : MonoBehaviour
{
    public bool isEnding;
    public void PlayAnimation(G20_AnimType[] anim_type, float? anim_speed = null, Action onEndAction = null)
    {
        var max = anim_type.Length;
        var rand=UnityEngine.Random.Range(0,max);
        PlayAnimation(anim_type[rand],anim_speed,onEndAction);
    }
    public void PlayAnimation(G20_AnimType anim_type, float? anim_speed = null,Action onEndAction=null)
    {
        if (isEnding) return;
        lastAnim = anim_type;
        float AnimationSpeed = animSpeed;
        if (anim_speed != null)
        {
            AnimationSpeed = (float)anim_speed;
        }
        animator.SetFloat("Speed", AnimationSpeed);
        var animName = Enum.GetName(typeof(G20_AnimType), anim_type);
        animator.CrossFadeInFixedTime(animName, 0.4f);
        G20_EnemyAnimAdjuster.GetInstance().AdjustEnemy(anim_type,transform,onEndAction);
    }
    [NonSerialized] public G20_AnimType lastAnim = G20_AnimType.Idle;
    [SerializeField] Animator animator;
    float animSpeed = 1.0f;
    public float AnimSpeed
    {
        set { animSpeed = value; }
        get { return animSpeed; }
    }
}
