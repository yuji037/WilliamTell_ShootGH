using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_PlusOneText : MonoBehaviour {
    [SerializeField] TextMesh textMesh;
    [SerializeField] float lifeDuration=1.0f;
    [SerializeField] float floatingValue=1.0f;
    // Use this for initialization
	void Start () {
        StartCoroutine(MoveCoroutine(lifeDuration));
	}
	IEnumerator MoveCoroutine(float fade_time)
    {
        for (float i=0;i<fade_time;i+=Time.deltaTime)
        {
            textMesh.color -= new Color(0,0,0,(1.0f/lifeDuration)*Time.deltaTime);
            transform.Translate(0,floatingValue* (1.0f / lifeDuration)*Time.deltaTime,0);
            yield return null;
        }
        Destroy(gameObject);
    }
}
