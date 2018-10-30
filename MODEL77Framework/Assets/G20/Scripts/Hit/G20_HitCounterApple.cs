using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class G20_HitCounterApple : G20_HitAction {

    [SerializeField]
    bool isRandomPos = false;

    [SerializeField]
    float randRadius = 1f;

    [SerializeField]
    Transform popPositionParent;

    [SerializeField]
    Transform[] popPositions;

    private void Start()
    {
        if ( popPositionParent ) // popPositionParentの子のtransformのみ選んで取得
            popPositions = popPositionParent.GetComponentsInChildren<Transform>()
                    .Where(t => t != popPositionParent).ToArray();
    }

    public override void Execute(Vector3 hit_point)
    {
        var create_point = hit_point;
        if ( isRandomPos )
        {
            var dif = new Vector3(Random.Range(0, randRadius), 0, 0);
            dif = Quaternion.Euler(0, 0, Random.Range(0, 360)) * dif;
            create_point = popPositions[Random.Range(0, popPositions.Length)].position + dif;
        }
        G20_BulletAppleCreator.GetInstance().Create(create_point);
    }
}
