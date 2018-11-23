using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MovePath
{
    public Transform[] Positions;
    public float OneLoopTime;
    public Color gizmoColor;
    public AnimationCurve curve;
}
public class G20_GesslerShootPerformer : MonoBehaviour
{
    [SerializeField]
    GameObject[] activateObjs;

    [SerializeField]
    GameObject shootEffect;

    [SerializeField]
    G20_Enemy stageBoss;


    [SerializeField]
    List<MovePath> pathList;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    GameObject counterWall;

    //移動時のスピードに補正する値
    float gesslerMoveTimeScale = 1.0f;

    [SerializeField]
    Camera shootCamera;

    //ボスが自発的に弾を打つ確率のカーブ
    [SerializeField]
    AnimationCurve AutoShootCurve;
    //ボスが弾を打つ頻度が最大まで高くなるまでの時間
    [SerializeField]
    float maxBossBattleTime;

    [SerializeField]
    G20_GesslerAnimController gesslerAnim;

    G20_HitEffect bossHitEffect;

    Coroutine currentMoveCoroutine;
    float shootEfectDistance;

    [SerializeField]
    ParticleSystem damageEffect;

    public void ToGesslerBattle(System.Action on_end_action)
    {
        StartCoroutine(GesslerShootCoroutine(on_end_action));
    }
    private void BattleSetUP()
    {
        Destroy(stageBoss.GetComponent<G20_HitReactionVoice>());
        Destroy(stageBoss.GetComponent<G20_HitCounterApple>());
        stageBoss.recvDamageActions += (a, b) => StartCoroutine(NextGesslerMove());
        stageBoss.recvDamageActions += (a, b) => HitGesslerHirumi();
        stageBoss.recvDamageActions += (a, b) =>
        {
            PlayDamageVoice();
        };
        stageBoss.recvDamageActions += (a, b) =>
        {
            damageEffect.Play(true);
        };
        stageBoss.deathActions += (a, b) => player.GetComponent<G20_Player>().IsInvincible = true;

    }
    [SerializeField] List<G20_VoiceType> voices;
    IEnumerator<G20_VoiceType> currentVoice;
    void PlayDamageVoice()
    {
        if (stageBoss.HP <= 0) return;
        if (currentVoice == null)
        {
            currentVoice = voices.GetEnumerator();
            currentVoice.MoveNext();
        }

        G20_VoicePerformer.GetInstance().PlayWithNoControll(currentVoice.Current);
        currentVoice.MoveNext();
    }

    int pathIndexNumber;
    void HitGesslerHirumi()
    {
        if (stageBoss.HP > 0)
        {
            gesslerAnim.PlayAnim(G20_GesslerAnimType.Hirumi);
        }
    }
    IEnumerator NextGesslerMove()
    {
        if (stageBoss.HP <= 0)
        {
            StopCoroutine(currentMoveCoroutine);
            yield break;
        }
        if (currentMoveCoroutine != null)
        {
            //無敵化し、ゆっくり動く
            stageBoss.IsInvincible = true;
            gesslerMoveTimeScale = 0.3f;
            ActivateShootObject(false);
            //1F待ってbossEffectを非アクティブに
            yield return null;
            bossHitEffect.IsActive = false;
            yield return new WaitForSeconds(3.0f);
            gesslerMoveTimeScale = 1.0f;
            bossHitEffect.IsActive = true;
            ActivateShootObject(true);
            stageBoss.IsInvincible = false;

            //前の動きコルーチンを廃棄
            StopCoroutine(currentMoveCoroutine);
            pathIndexNumber++;
            if (pathIndexNumber >= pathList.Count)
            {
                pathIndexNumber = 0;
            }
        }
        currentMoveCoroutine = StartCoroutine(GesslerFollowsPath(pathList[pathIndexNumber]));
    }
    IEnumerator GesslerShootCoroutine(System.Action on_end_action)
    {
        BattleSetUP();

        var anim = GameObject.Find("G20_Root").GetComponent<Animator>();
        anim.enabled = true;
        anim.CrossFade("ToGesslerBattle", 0.3f);
		G20_BGMManager.GetInstance().FadeOut();
        yield return new WaitForSeconds(1.0f);
		G20_BGMManager.GetInstance().Play(G20_BGMType.BOSS);
        yield return new WaitForSeconds(1.9f);
		anim.enabled = false;
        G20_VoicePerformer.GetInstance().PlayWithNoControll(G20_VoiceType.INGAME2);

        //counterWall(ゲスラーの反撃判定壁)をアクティブに
        counterWall.SetActive(true);

        // 「SHOOT」表示
        ActivateShootObject(true);

        StartCoroutine(NextGesslerMove());

        // ゲスラー撃てるようになる
        stageBoss.IsInvincible = false;


        //gesslerにhiteefect追従するように
        bossHitEffect = stageBoss.GetComponent<G20_HitEffect>();
        bossHitEffect.effctParent = stageBoss.transform;
        bossHitEffect.ChangeEffectType(G20_EffectType.HIT_GESSLER_BODY);

        Destroy(stageBoss.GetComponent<G20_HitSE>());

        stageBoss.GetComponent<G20_HitObject>().ChangeHitTag(G20_HitTag.ASSIST);
        stageBoss.GetComponent<G20_HitObject>().IsHitRateUp = true;

        const float CheckShootTime = 1.0f;
        float fightingTime = 0f;
        float timer = 0f;
        while (stageBoss.HP > 0)
        {
            if (timer >= CheckShootTime)
            {
                var rate = AutoShootCurve.Evaluate(fightingTime / maxBossBattleTime);
                float rand = UnityEngine.Random.Range(0, 1.0f);
                if (rand <= rate)
                {
                    //攻撃
                    gesslerAnim.PlayAnim(G20_GesslerAnimType.Attack);
                    G20_BulletAppleCreator.GetInstance().Create(stageBoss.transform.position);
                }
                fightingTime += CheckShootTime;
                timer = 0f;
            }
            timer += Time.deltaTime;
            yield return null;
        }


        //counterWall(ゲスラーの反撃判定壁)を非アクティブに
        counterWall.SetActive(false);

        gesslerAnim.PlayAnim(G20_GesslerAnimType.Yarare);

        // 「SHOOT」非表示
        foreach (var o in activateObjs)
        {
            o.SetActive(false);
        }
        StartCoroutine(GesslerDownMove());
        StartCoroutine(ApprochToGessler());
        G20_VoicePerformer.GetInstance().PlayWithCaption(G20_VoiceType.GAME_CLEAR1);

        yield return new WaitForSeconds(1.0f);
        // ゲスラー撃破
        if (on_end_action != null) on_end_action();
    }
    void ActivateShootObject(bool _active)
    {
        foreach (var o in activateObjs)
        {
            o.SetActive(_active);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var i in pathList)
        {
            Vector3 pre = i.Positions[i.Positions.Length - 1].position;
            Gizmos.color = i.gizmoColor;
            foreach (var k in i.Positions)
            {
                Gizmos.DrawLine(pre, k.position);
                pre = k.position;
            }
        }

    }
    [SerializeField]
    Transform player;
    [SerializeField]
    Vector3 DefaultPosition;

    IEnumerator ApprochToGessler()
    {
        var preRot = player.rotation;

        //徐々に回転
        Vector3 direction = stageBoss.transform.position - player.transform.position;
        direction.y += 0.5f;
        Quaternion toRotation = Quaternion.FromToRotation(player.transform.forward, direction);
        for (float t = 0f; t <= 1.0f; t += Time.deltaTime * 2.5f)
        {
            player.rotation = Quaternion.Lerp(preRot, toRotation, t);
            yield return null;
        }
        //徐々に近づく
        for (float t = 0f; t <= 2.0f; t += Time.deltaTime)
        {
            player.transform.Translate(0, 0, Time.deltaTime * 1.5f, Space.Self);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        player.position = DefaultPosition;
        player.rotation = Quaternion.identity;

    }
    IEnumerator GesslerFollowsPath(MovePath movePath)
    {

        while (true)
        {
            Vector3 prePos = stageBoss.transform.position;
            bool wasRight = false;
            bool isFirstPath = true;
            foreach (var k in movePath.Positions)
            {
                var isRight = ((k.position.x - prePos.x) > 0) ? true : false;
                if ((wasRight != isRight) || isFirstPath)
                {
                    if (!gesslerAnim.IsAttacking())
                    {
                        if (isRight)
                        {
                            gesslerAnim.PlayAnim(G20_GesslerAnimType.RightMove);
                        }
                        else
                        {
                            gesslerAnim.PlayAnim(G20_GesslerAnimType.LeftMove);
                        }
                    }
                    wasRight = isRight;
                }
                yield return MoveNextPosition(k.position, movePath.curve);
                prePos = k.position;
                isFirstPath = false;
            }

            //配列の最後まで行くと、最初に戻る
            yield return MoveNextPosition(movePath.Positions[0].position, movePath.curve);
        }
    }
    IEnumerator GesslerDownMove()
    {
        while (true)
        {
            stageBoss.transform.Translate(0, -Time.deltaTime, 0);
            if (stageBoss.transform.position.y <= 1.0f)
            {
                break;
            }
            yield return null;
        }
    }
    IEnumerator MoveNextPosition(Vector3 nextPosition, AnimationCurve curve)
    {
        var startPos = stageBoss.transform.position;
        var dis = Vector3.Distance(nextPosition, stageBoss.transform.position);
        var requiredTime = dis / moveSpeed;
        for (float t = 0; t < requiredTime; t += Time.deltaTime * gesslerMoveTimeScale)
        {
            var curvePos = curve.Evaluate(t / requiredTime);
            stageBoss.transform.position = Vector3.Lerp(startPos, nextPosition, curvePos);
            yield return null;
        }
    }
}