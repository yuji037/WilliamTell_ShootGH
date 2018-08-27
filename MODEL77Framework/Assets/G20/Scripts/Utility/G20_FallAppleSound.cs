using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_FallAppleSound : MonoBehaviour {

    Rigidbody _rigidbody;
    
    [SerializeField] int playSEMaxCount = 2;
    int playSECount = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (    playSECount < playSEMaxCount 
            && _rigidbody.velocity.sqrMagnitude > 1.0f )
        {
            G20_SEManager.GetInstance().Play(G20_SEType.APPLE_FALL, transform.position);
            playSECount++;
        }
    }
}
