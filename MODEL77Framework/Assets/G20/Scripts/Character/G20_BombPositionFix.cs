using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死んだときにpivotのpositionまで1秒かけて戻る
public class G20_BombPositionFix : MonoBehaviour {
    [SerializeField] Transform model;
	// Use this for initialization
	void Start () {
        GetComponent<G20_Enemy>().deathActions += (x, y) => StartCoroutine(FixPosition());
    }
	IEnumerator FixPosition()
    {
        var startY = model.position.y;
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            model.localPosition = new Vector3(0, 0, Mathf.Lerp(startY, 0, t));
            if (model.localPosition.y <= 0) yield break;
            yield return null;
        }
    }
}
