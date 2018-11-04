using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMove : MonoBehaviour {


    [SerializeField]
    GameObject bomb;
	// Use this for initialization
	void Start () {
        StartCoroutine(parentChange());
	}
	
    IEnumerator parentChange()
    {
        yield return new WaitForSeconds(0.1f);

        transform.parent = bomb.transform.parent;

        yield return null;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
