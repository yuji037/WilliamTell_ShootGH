using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitClearApple : G20_HitAction {

    [SerializeField] float colliderExpansionRate = 10.0f;

    // 爆発に見せかけた膨張
    public override void Execute(Vector3 hit_point)
    {
        var col = GetComponent<SphereCollider>();
        col.radius *= colliderExpansionRate;
        StartCoroutine(ColliderOffCoroutine(col));
    }

    // 一度膨張したものが再び床に乗ると浮いてしまうので、すり抜けさせる
    IEnumerator ColliderOffCoroutine(Collider col)
    {
        yield return new WaitForSeconds(0.2f);
        col.enabled = false;
    }
}
