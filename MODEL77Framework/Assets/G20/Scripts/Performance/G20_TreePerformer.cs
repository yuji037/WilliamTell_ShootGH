using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_TreePerformer : MonoBehaviour {

    [SerializeField]
    float animInterval = 5.0f;

    Animator[] animators;

    float nextInterval = 0f;
    float timer = 0;

	// Use this for initialization
	void Start () {
        animators = GetComponentsInChildren<Animator>();
        //foreach(var anim in animators )
        //{
        //    anim.CrossFade("WindReceive", 0.4f);
        //    anim.speed = Random.Range(0.1f, 0.4f);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if ( nextInterval <= timer )
        {
            timer = 0;

            int num = Random.Range(0, animators.Length);
            animators[num].CrossFade("WindReceive", 0.4f);
            animators[num].speed = Random.Range(0.1f, 0.4f);

            float intervalMin = animInterval - 0.5f;
            float intervalMax = animInterval + 0.5f;
            if ( intervalMin < 0 ) intervalMin = 0;
            if ( intervalMax < 0 ) intervalMax = 0;

            nextInterval = Random.Range(intervalMin, intervalMax);

        }
    }


}
