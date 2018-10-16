using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitClearApple : G20_HitAction {

    [SerializeField] float colliderExpansionRate = 10.0f;
    [SerializeField] bool isVisibleExpand = false;

    int expandCount = 0;

    // 爆発に見せかけた膨張
    public override void Execute(Vector3 hit_point)
    {
        // 爆発をなくした

        //var col = GetComponent<SphereCollider>();
        //if ( isVisibleExpand )
        //{
        //    // 見た目ごと膨張
        //    transform.localScale = transform.localScale * colliderExpansionRate;
        //    expandCount++;
        //    // でかすぎるとブルブルするので当たり判定をOFF
        //    if ( expandCount >= 4 ) StartCoroutine(ColliderOffCoroutine(col));
        //}
        //else
        //{
        //    // 当たり判定だけ膨張
        //    col.radius *= colliderExpansionRate;
        //    StartCoroutine(ColliderOffCoroutine(col));
        //}
    }

    // 一度爆発したものが再び床に乗ると浮いてしまうので、すり抜けさせる
    IEnumerator ColliderOffCoroutine(Collider col)
    {
        yield return new WaitForSeconds(0.2f);
        col.enabled = false;
    }
}
