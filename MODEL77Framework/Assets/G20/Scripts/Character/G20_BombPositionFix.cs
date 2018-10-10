using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死んだときにpivotのpositionまで1秒かけて戻る
public class G20_BombPositionFix : MonoBehaviour {
    [SerializeField] Transform model;
    [SerializeField] Transform pivot;
	// Use this for initialization
	void Start () {
        GetComponent<G20_Enemy>().deathActions += (x, y) => StartCoroutine(FixPosition());
    }
	IEnumerator FixPosition()
    {
        var startPos = pivot.position;
        var fixVec=pivot.position-model.position;
        fixVec = fixVec.normalized;
        var dis = Vector3.Distance(pivot.position, model.position);
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            pivot.position=startPos+ fixVec*Mathf.Lerp(0,dis,t);
            if (model.position.y <= 0) yield break;
            yield return null;
        }
    }
}
