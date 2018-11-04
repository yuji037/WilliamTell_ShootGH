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
        var vec2= (transform.position - Camera.main.transform.position).normalized;
        rh.GetComponent<Rigidbody>().AddForce(vec2* rollingPower,ForceMode.Impulse);
    }

    public override void Execute(Vector3 hit_point)
    {
        if (enemy.HP <= 0)
        {

            ChangeRigidHead(hit_point);
        }
    }
}
