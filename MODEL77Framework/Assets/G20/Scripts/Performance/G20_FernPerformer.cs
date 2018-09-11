using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_FernPerformer : G20_Singleton<G20_FernPerformer> {

    Animator anim;
    float defaultSpeed = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if ( anim ) defaultSpeed = anim.speed;
    }

    public void Shake()
    {
        if ( !anim ) return;
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        anim.speed = 20;
        yield return new WaitForSeconds(1.5f);
        anim.speed = defaultSpeed;
    }
}
