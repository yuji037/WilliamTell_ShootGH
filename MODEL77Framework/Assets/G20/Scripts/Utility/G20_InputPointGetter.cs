using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_InputPointGetter : G20_Singleton<G20_InputPointGetter> {
    CoordinateManager CM;
    private void Start()
    {
        CM = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
    }
    //1Fに1回呼ばれる
    public Vector2? GetInputPoint()
    {
        if (CM.isUpdate())
        {
            Hashtable ht = CM.Get();
            return new Vector2((float)ht["x"],(float)ht["y"]);
        }

        return null;
    }
}
