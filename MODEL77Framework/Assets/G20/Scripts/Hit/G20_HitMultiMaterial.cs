using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitMultiMaterial : G20_HitAction
{
    //金リンゴ人間に対応するためにSkinnedMeshrenderでも入れれるようにしている
    [SerializeField] SkinnedMeshRenderer mesh;
    [SerializeField] MeshRenderer meshR;
    [SerializeField] Material[] changeMaterials;
    int currentNum = 0;
    public override void Execute(Vector3 hit_point)
    {
        if (currentNum < changeMaterials.Length)
        {
            if(mesh)mesh.material = changeMaterials[currentNum];
            if(meshR)meshR.material= changeMaterials[currentNum];
            currentNum++;
        }
    }
}
