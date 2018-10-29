using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_RigidHeadChanger : G20_HitAction {
    [SerializeField] GameObject rigidHead;
    [SerializeField] SkinnedMeshRenderer headMesh;
    [SerializeField] G20_Enemy enemy;
    public float rollingPower=10.0f;
	// Use this for initialization
	void ChangeRigidHead(Vector3 hit_point)
    {
        //mesh非表示
        headMesh.enabled = false;

        var rh =Instantiate(rigidHead);
        rh.transform.position = enemy.Head.position;
        rh.transform.rotation = enemy.Head.rotation;
        var vec= (enemy.Head.position - hit_point).normalized;
        rh.GetComponent<Rigidbody>().AddForce(vec*rollingPower,ForceMode.Impulse);
    }

    public override void Execute(Vector3 hit_point)
    {
        if (enemy.HP <= 0)
        {
            ChangeRigidHead(hit_point);
        }
    }
}
