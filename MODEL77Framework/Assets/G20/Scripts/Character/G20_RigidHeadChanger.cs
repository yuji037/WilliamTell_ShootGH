using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_RigidHeadChanger : MonoBehaviour {
    [SerializeField] GameObject rigidHead;
    [SerializeField] SkinnedMeshRenderer headMesh;
    [SerializeField] G20_Enemy enemy;
    public float rollingPower=10.0f;
	// Use this for initialization
	void Awake() {
        enemy.deathActions +=(a,b)=>ChangeRigidHead();
	}
	void ChangeRigidHead()
    {
        //mesh非表示
        headMesh.enabled = false;

        var rh =Instantiate(rigidHead);
        rh.transform.position = enemy.Head.position;
        rh.transform.rotation = enemy.Head.rotation;
        var vec=rh.transform.position - Camera.main.transform.position;
        rh.GetComponent<Rigidbody>().AddForce(vec*rollingPower,ForceMode.Impulse);
    }

}
