using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BossWeakpointmove : MonoBehaviour
{

    [SerializeField] float rotspeed = 100;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotspeed*Time.deltaTime, 0);
    }
}
