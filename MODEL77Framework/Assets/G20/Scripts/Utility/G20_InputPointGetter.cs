using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_InputPointGetter : G20_Singleton<G20_InputPointGetter> {
    CoordinateManager CM;
    private void Start()
    {
        CM = GameObject.Find("GameManager").GetComponent<CoordinateManager>();
    }
    bool hitted;
    private void Update()
    {
#if UNITY_EDITOR

#endif
        hitted = CM.isUpdate();
    }
    public Vector2? GetInputPoint()
    {
//#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            return Input.mousePosition;
        }
//#endif
        if (hitted)
        {
            Hashtable ht = CM.Get();
            new Vector2((float)ht["x"],(float)ht["y"]);
        }

        return null;
    }
}
