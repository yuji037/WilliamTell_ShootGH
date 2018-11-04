using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_FollowBombAOE : MonoBehaviour {
    [SerializeField]Transform followBomb;

    private void Awake()
    {
        transform.parent = G20_ComponentUtility.Root;
    }
    // Update is called once per frame
    void Update () {
        if (followBomb == null)
        {
            Destroy(gameObject);
            return;
        }
        var followPos = followBomb.transform.position;
        followPos.y = 0.1f;
        transform.position = followPos;
	}
}
