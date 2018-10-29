using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct MovePath
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
    G20_HitObject headHitObject;

    [SerializeField]
    List<MovePath> pathList;

    [SerializeField]
    GameObject[] gesslerMeshs;

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

    Coroutine currentMoveCoroutine;
    float shootEfectDistance;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gesslerMeshs[0].SetActive(true);
            gesslerMeshs[1].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gesslerMeshs[1].SetActive(true);
            gesslerMeshs[0].SetActive(false);
        }
    }
    public void ToGesslerBattle(System.Action on_end_action)
    {
        StartCoroutine(GesslerShootCoroutine(on_end_action));
    }
    private void BattleSetUP()
    {
        Destroy(stageBoss.GetComponent<G20_HitReactionVoice>());
        stageBoss.recvDamageActions += (a, b) => StartCoroutine(NextGesslerMove());
    }
    int pathIndexNumber;
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
            yield return new WaitForSeconds(3.0f);
            gesslerMoveTimeScale = 1.0f;
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
        G20_VoicePerformer.GetInstance().PlayWithNoControll(G20_VoiceType.INGAME6);
        BattleSetUP();

        var anim = GameObject.Find("G20_Root").GetComponent<Animator>();
        anim.enabled = true;
        anim.CrossFade("ToGesslerBattle", 0.3f);
        yield return new WaitForSeconds(2.9f);
        anim.enabled = false;

        //counterWall(ゲスラーの反撃判定壁)をアクティブに
        counterWall.SetActive(true);

        // 「SHOOT」表示
        ActivateShootObject(true);

        StartCoroutine(NextGesslerMove());

        // ゲスラー撃てるようになる
        stageBoss.IsInvincible = false;
        var hitEffect = stageBoss.GetComponent<G20_HitEffect>();

        //gesslerにhiteefect追従するように
        hitEffect.effctParent = stageBoss.transform;
        hitEffect.ChangeEffectType(G20_EffectType.HIT_GESSLER_BODY);
        Destroy(stageBoss.GetComponent<G20_HitSE>());
        headHitObject.ChangeHitTag(G20_HitTag.ASSIST);

        float oneLoopTime =1.0f;
        float timer = 0f;
        while (stageBoss.HP > 0)
        {
            var rate = AutoShootCurve.Evaluate(timer / maxBossBattleTime);
            float rand=UnityEngine.Random.Range(0,1.0f);
            if (rand<=rate)
            {
                G20_BulletAppleCreator.GetInstance().Create(stageBoss.transform.position);
            }
            timer += oneLoopTime;
            yield return new WaitForSeconds(oneLoopTime);
        }

        //counterWall(ゲスラーの反撃判定壁)を非アクティブに
        counterWall.SetActive(false);


        // ゲスラー撃破
        G20_VoicePerformer.GetInstance().PlayWithCaption(G20_VoiceType.GAME_CLEAR1);

        // 「SHOOT」非表示
        foreach (var o in activateObjs)
        {
            o.SetActive(false);
        }

        if (on_end_action != null) on_end_action();
    }
    void ActivateShootObject(bool _active)
    {
        foreach (var o in activateObjs)
        {
            o.SetActive(_active);
        }
    }
    void FollowShootEffectToGessler()
    {
        var gesslerPos = headHitObject.transform.position;
        var shootVec = gesslerPos - Camera.main.transform.position;
        shootEffect.transform.position = shootCamera.transform.position + shootVec.normalized * shootEfectDistance;
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
    IEnumerator GesslerFollowsPath(MovePath movePath)
    {

        while (true)
        {
            foreach (var k in movePath.Positions)
            {
                yield return MoveNextPosition(k.position, movePath.curve);
            }
            //配列の最後まで行くと、最初に戻る
            yield return MoveNextPosition(movePath.Positions[0].position, movePath.curve);
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
