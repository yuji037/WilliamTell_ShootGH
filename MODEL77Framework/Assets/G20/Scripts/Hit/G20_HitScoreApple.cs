using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitScoreApple : G20_HitAction {

    [SerializeField] int score = 1;

    G20_HitAction[] hitActions;

    Renderer meshRenderer;

    private void Start()
    {
        hitActions = GetComponentsInChildren<G20_HitAction>();
        meshRenderer = GetComponent<Renderer>();
    }

    public override void Execute(Vector3 hit_point)
    {
        G20_Score.GetInstance().AddScore(1);
        

        // 落ちて消える処理
        if(score <= 0 )
        {

        }
    }

    IEnumerator FallCoroutine()
    {
        Color c = meshRenderer.material.color;
        for(float t = 0; t < 1; t+=Time.deltaTime )
        {
            meshRenderer.material.color = new Color(c.r, c.b, c.g, 1f - t);
        }

        yield return null;
    }
}
