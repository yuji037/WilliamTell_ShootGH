using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_EnemyAnimation : MonoBehaviour {
    [SerializeField] Animator animator;
    public void Attack()
    {
        animator.CrossFadeInFixedTime("Attack", 0.4f);
    }
    public void Run()
    {
        animator.CrossFadeInFixedTime("Run", 0.4f);
    }
    public void Death()
    {
        animator.CrossFadeInFixedTime("Death", 0.4f);
    }
    public void Falter()
    {
        animator.CrossFadeInFixedTime("Falter", 0.4f);
    }
    public void Suicide()
    {
        animator.CrossFadeInFixedTime("Suicide", 0.4f);
    }
    public int GetCurrentState()
    {
       return animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
    }
    public void SetState(int stateName)
    {
        animator.CrossFadeInFixedTime(stateName, 0.4f);
    }
}
