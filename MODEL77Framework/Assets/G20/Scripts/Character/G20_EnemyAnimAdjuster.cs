using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
class G20_AdjustParam
{
    public G20_AnimType animType;
    public AnimationCurve animCurve;
    public Vector3 move;
    public float endTime;
    public float enemyHeight;
}

public class G20_EnemyAnimAdjuster : G20_Singleton<G20_EnemyAnimAdjuster>
{
    [SerializeField]
    List<G20_AdjustParam> adjustParams;
    public void AdjustEnemy(G20_AnimType animType, Transform controllTransform, Action onEndAction = null)
    {
        var adjustParam = FindAdjustParam(animType);
        if (adjustParam != null)
        {
            StartCoroutine(AdjustPosition(controllTransform, adjustParam, onEndAction));
        }
    }
    G20_AdjustParam FindAdjustParam(G20_AnimType animType)
    {
        return adjustParams.Find(x => x.animType == animType);
    }
    IEnumerator AdjustPosition(Transform target, G20_AdjustParam adjustParam, Action onEndAction = null)
    {
        var startPos = target.position;
        var requiredTime = adjustParam.endTime;
        var cameraDir = (Camera.main.transform.position - target.position);
        cameraDir.y = 0;
        //向きを変更
        target.forward = cameraDir;
        cameraDir.Normalize();
        float moveDis = Vector3.Dot(adjustParam.move, cameraDir);

        var move = moveDis * cameraDir;
        var nextPosition = startPos + AdjustMove(startPos, move,adjustParam.enemyHeight);
        for (float t = 0; t < requiredTime; t += Time.deltaTime)
        {
            var curveRate = adjustParam.animCurve.Evaluate(t / requiredTime);
            target.position = Vector3.Lerp(startPos, nextPosition, curveRate);
            yield return null;
        }
        if (onEndAction != null) onEndAction();
    }
    Vector3 AdjustMove(Vector3 startPos, Vector3 move, float height)
    {
        Ray ray = new Ray(startPos, move);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, move.magnitude);
        foreach (var h in hits)
        {
            var hitObj = h.transform.GetComponent<G20_RoadWall>();
            if (hitObj)
            {
                var retMove = h.point - startPos;
                var adjustZ = -height * 0.5f;
                retMove += retMove.normalized * adjustZ;
                Debug.Log("調整前" + move);
                Debug.Log("調整後" + retMove);
               
                return retMove;
            }
        }
        return move;
    }

}
