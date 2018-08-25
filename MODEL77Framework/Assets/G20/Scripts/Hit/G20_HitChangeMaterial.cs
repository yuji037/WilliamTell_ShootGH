using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitChangeMaterial : G20_HitAction
{
    [SerializeField] MeshRenderer mesh;
    [SerializeField] Material changeMaterial;
    public override void Execute(Vector3 hit_point)
    {
        mesh.material= changeMaterial;
        Debug.Log("changeMat");
    }
}
