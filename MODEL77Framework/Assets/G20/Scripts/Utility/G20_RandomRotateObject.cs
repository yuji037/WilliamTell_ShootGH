using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_RandomRotateObject : MonoBehaviour {

    [SerializeField]
    float speed = 100f;

    Vector3 rotateVec = new Vector3(1, 0, 0);

	// Use this for initialization
	void Start () {
        rotateVec = Quaternion.Euler(0, 0, Random.Range(0, 360)) * rotateVec;
        rotateVec = Quaternion.Euler(Random.Range(0, 360), 0, 0) * rotateVec;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotateVec * speed * Time.deltaTime);
	}
}
