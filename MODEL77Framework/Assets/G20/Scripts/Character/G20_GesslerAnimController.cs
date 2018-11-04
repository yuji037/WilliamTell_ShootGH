using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags]
public enum G20_GesslerAnimType{
    Taiki=1,
    LeftMove=2,
    RightMove=4,
    Hirumi=8,
    Yarare=16,
    Attack=32,
}
[System.Serializable]
class G20_GesslerObject
{
    public GameObject gesslerModel;
    public Animator animator;
    [G20_EnumFlags]
    public G20_GesslerAnimType haveAnim;
}

public class G20_GesslerAnimController : MonoBehaviour {
    [SerializeField] G20_GesslerObject[] gesslerObjs;
    GameObject currentActiveGesslerObject;

    private void Awake()
    {
        currentActiveGesslerObject=gesslerObjs[0].gesslerModel;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayAnim(G20_GesslerAnimType.LeftMove);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayAnim(G20_GesslerAnimType.RightMove);
        }
    }
    public void PlayAnim(G20_GesslerAnimType animType)
    {
        //指定animを持っているゲスラーオブジェクト探し再生
        foreach (var i in gesslerObjs)
        {
            if ((i.haveAnim & animType)>0)
            {
                Debug.Log(Enum.GetName(typeof(G20_GesslerAnimType), animType));
                ChangeActiveModel(i.gesslerModel);
                currentAnimator = i.animator;
                currentAnimType = animType;
                i.animator.CrossFade(Enum.GetName(typeof(G20_GesslerAnimType), animType), 0.3f);
                break;
            }
        }
    }
    Animator currentAnimator;
    G20_GesslerAnimType currentAnimType;
    public bool IsAttacking()
    {
        if (currentAnimType != G20_GesslerAnimType.Attack || !currentAnimator) return false;
        return (currentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
    }
    void ChangeActiveModel(GameObject activateModel)
    {
        if (activateModel != currentActiveGesslerObject)
        {
            currentActiveGesslerObject.SetActive(false);
            currentActiveGesslerObject = activateModel;
            currentActiveGesslerObject.SetActive(true);
        }
    }
}
