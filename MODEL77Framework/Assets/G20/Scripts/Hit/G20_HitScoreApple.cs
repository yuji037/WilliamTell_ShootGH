using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitScoreApple : G20_HitAction {

    [SerializeField] int scoreMax = 1;
    int score = 0;

    G20_HitAction[] hitActions;

    Renderer meshRenderer;

    [SerializeField] float hitGroundHeight = 1f;
    [SerializeField] float bounceRate = 0.2f;

    ParticleSystem[] particleSystems;

    Rigidbody rb;

    private void Start()
    {
        hitActions = GetComponentsInChildren<G20_HitAction>();
        meshRenderer = GetComponent<Renderer>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        score = scoreMax;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        //Color c = meshRenderer.material.color;
        //for ( float t = 0; t < 1; t += Time.deltaTime )
        //{
        //    meshRenderer.material.color = new Color(c.r, c.g, c.b, t);
        //    yield return null;
        //}
        for ( float t = 0; t < 1; t += Time.deltaTime )
        {
            transform.parent.localScale = new Vector3(t, t, t);
            yield return null;
        }
        transform.parent.localScale = Vector3.one;
    }

    public override void Execute(Vector3 hit_point)
    {
        G20_EffectManager.GetInstance().Create(G20_EffectType.PLUS_ONE_SCORE, hit_point);
        G20_Score.GetInstance().AddScore(1);
        score--;

        //foreach(var ps in particleSystems )
        //{

        //    var cMin = ps.main.startColor.colorMin;
        //    var cMax = ps.main.startColor.colorMax;
        //    if ( score >= 0 ) {
        //        cMin = new Color(cMin.r, cMin.g, cMin.b, cMin.a * (float)score / (float)( score + 1f ));
        //        cMax = new Color(cMax.r, cMax.g, cMax.b, cMax.a * (float)score / (float)( score + 1f ));
        //    }
        //    ParticleSystem.MinMaxGradient grad = new ParticleSystem.MinMaxGradient(cMin, cMax);

        //    //ps.SetParticles(;
        //}

        // 落ちて消える処理
        if( score <= 0 )
        {
            if(scoreMax == 3 )
            {
                G20_Score.GetInstance().AddGoldPoint(1);
            }

            GetComponent<Collider>().enabled = false;
            GetComponent<G20_HitObject>().ChangeHitTag(G20_HitTag.NORMAL);
            StartCoroutine(FallCoroutine());
        }
    }

    IEnumerator FallCoroutine()
    {
        // 落下
        Vector3 velocity = Vector3.zero;
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.parent.GetComponent<Animator>().enabled = false;
        rb.velocity = Vector3.zero;
        while ( transform.position.y > hitGroundHeight )
        {
            //velocity += Physics.gravity * Time.deltaTime;
            //transform.position += velocity * Time.deltaTime;
            yield return null;
        }
        //while ( transform.localPosition.y > -0.6f )
        //{
        //    velocity += Physics.gravity * Time.deltaTime;
        //    transform.localPosition += velocity * Time.deltaTime;
        //    yield return null;
        //}

        //GetComponent<Animator>().enabled = true;
        //GetComponent<Animator>().CrossFade("AppleFall", 0f);

        //yield return new WaitForSeconds(1);
        //// 地面に当たる
        //velocity.y *= -1;
        //bool isRightSide = transform.position.x > 0;
        //velocity.x = 3f * ( isRightSide ? -1 : 1 );

        G20_ScoreApplePopper.GetInstance().UnregisterApple(this);

        Vector3 pos = transform.position;
        pos.y = hitGroundHeight + 0.02f;
        transform.position = pos;
        //transform.position += velocity * Time.deltaTime;
        //yield return null;

        //velocity.y *= bounceRate;

        //// もう一度放物線運動
        //while ( transform.position.y > hitGroundHeight )
        //{
        //    velocity += Physics.gravity * Time.deltaTime;
        //    velocity.x -= 6f * Time.deltaTime * ( velocity.x > 0 ? 1 : -1 );
        //    transform.position += velocity * Time.deltaTime;
        //    transform.rotation = Quaternion.Euler(
        //        0, 0, 180 * Time.deltaTime * ( isRightSide ? 1 : -1 ))
        //        * transform.rotation;
        //    yield return null;
        //}

        rb.velocity *= -bounceRate;
        rb.AddForce(transform.right, ForceMode.VelocityChange);
        var rate = Random.Range(1f, 2f);
        rb.AddTorque(transform.forward * -rate, ForceMode.VelocityChange);
        //rb.velocity += 
        while(transform.position.y > hitGroundHeight )
        {
            yield return null;
        }

        // 消える
        yield return StartCoroutine(FadeOut());

        Destroy(transform.parent.gameObject);
    }

    //IEnumerator DestroyCoroutine()
    //{

    //}

    IEnumerator FadeOut()
    {
        Color c = meshRenderer.material.color;
        for ( float t = 0; t < 1; t += Time.deltaTime )
        {
            meshRenderer.material.color = new Color(c.r, c.g, c.b, 1f - t);
            var _pos = transform.position;
            transform.position = new Vector3(_pos.x, hitGroundHeight, _pos.z);
            yield return null;
        }
    }
}
