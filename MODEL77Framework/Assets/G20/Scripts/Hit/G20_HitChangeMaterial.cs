using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitChangeMaterial : G20_HitAction
{
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material[] changeMaterials;
    int currentNum = 0;
    public override void Execute(Vector3 hit_point)
    {
        if (currentNum < changeMaterials.Length)
        {
            mesh.material = changeMaterials[currentNum];
            currentNum++;
        }
    }
}
