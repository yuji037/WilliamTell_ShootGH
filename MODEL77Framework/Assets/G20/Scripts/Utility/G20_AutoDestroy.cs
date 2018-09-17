using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_AutoDestroy : MonoBehaviour {

    public float destroyTime = 3.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);
	}
    private void OnDestroy()
    {
        // レンダラのマテリアルを破棄(パーティクルシステムのレンダラも含まれる)
        var thisRenderer = this.GetComponent<Renderer>();
        if (thisRenderer != null && thisRenderer.materials != null)
        {
            foreach (var m in thisRenderer.materials)
            {
                DestroyImmediate(m);
            }
        }
    }
}
