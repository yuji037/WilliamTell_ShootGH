using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Rendering;

public class G20_GesslerColor : MonoBehaviour {

    public float Color = 1f;
    private float _color = 1f;

    CubismRenderer[] cubismRenderers;

    private void Start()
    {
        cubismRenderers = GetComponentsInChildren<CubismRenderer>();
    }

    private void Update()
    {
        if(_color != Color )
        {
            foreach (var cr in cubismRenderers )
            {
                Debug.Log("iro : " + Color);
                cr.Color = new Color(Color, Color, Color, 1f);
            }
            _color = Color;
        }
    }
}
