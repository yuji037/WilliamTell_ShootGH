using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class G20_FallAppleSound : MonoBehaviour {

    public event Action<int> firstCollisionHItAction;
    public int eventArgInteger;

    Rigidbody _rigidbody;

    bool isScoreCounted = false;

    [SerializeField] int playSEMaxCount = 2;
    int playSECount = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isScoreCounted)
        {
            if(firstCollisionHItAction!=null)firstCollisionHItAction(eventArgInteger);
            isScoreCounted = true;
        }

        if (    playSECount < playSEMaxCount 
            && _rigidbody.velocity.sqrMagnitude > 1.0f )
        {
            G20_SEManager.GetInstance().Play(G20_SEType.APPLE_FALL, transform.position);
            playSECount++;
        }
       
    }
}
