using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_GesslerShootPerformer : MonoBehaviour {

    [SerializeField]
    GameObject[] activateObjs;

    [SerializeField]
    G20_Enemy stageBoss;

    [SerializeField]
    G20_HitObject headHitObject;

    public void ToGesslerBattle(System.Action on_end_action)
    {
        StartCoroutine(GesslerShootCoroutine(on_end_action));
    }

    IEnumerator GesslerShootCoroutine(System.Action on_end_action)
    {
        var anim = GameObject.Find("G20_Root").GetComponent<Animator>();
        anim.enabled = true;
        anim.CrossFade("ToGesslerBattle", 0.3f);

        yield return new WaitForSeconds(2.9f);
        anim.enabled = false;

        // 「SHOOT」表示
        foreach(var o in activateObjs )
        {
            o.SetActive(true);
        }
        // ゲスラー撃てるようになる
        stageBoss.IsInvincible = false;
        stageBoss.GetComponent<G20_HitEffect>().ChangeEffectType(G20_EffectType.HIT_APPLE_BODY);
        Destroy(stageBoss.GetComponent<G20_HitSE>());
        Destroy(stageBoss.GetComponent<G20_HitReactionVoice>());
        headHitObject.ChangeHitTag(G20_HitTag.ASSIST);


        while ( stageBoss.HP > 0 )
        {

            yield return null;
        }
        // ゲスラー撃破
        G20_VoicePerformer.GetInstance().PlayWithCaption(G20_VoiceType.GAME_CLEAR1);

        // 「SHOOT」非表示
        foreach ( var o in activateObjs )
        {
            o.SetActive(false);
        }

        if ( on_end_action != null ) on_end_action();
    }
}
