using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BulletApple : G20_Unit {
    Vector3 moveVec;

    [SerializeField]
    float radius;

    [SerializeField]
    float moveSpeed=1.0f;

    [SerializeField]
    float rollingPower;

    [SerializeField]
    float rollingDuration=3.0f;

    [SerializeField]
    GameObject particle;

    bool isTargetingPlayer=true;
    
    private void Start()
    {
        moveVec= Camera.main.transform.position - transform.position;
        deathActions +=(a,b) =>isTargetingPlayer=false;
        moveVec.Normalize();
        StartCoroutine(BulletRoutine());
    }
    IEnumerator BulletRoutine()
    {
        while (isTargetingPlayer)
        {
            transform.Translate(moveVec * moveSpeed * Time.deltaTime,Space.World);
            transform.Rotate(-100.0f*Time.deltaTime*moveSpeed,0,0);
            if (radius >= Vector3.Distance(Camera.main.transform.position, transform.position))
            {
                G20_EnemyAttack.GetInstance().Attack(1);
                RecvDamage(HP, G20_DamageType.System);
            }
            yield return null;
        }
        particle.SetActive(false);
        var rh = GetComponent<Rigidbody>();
        rh.isKinematic = false;
        var vec = transform.position - Camera.main.transform.position;
        rh.AddForce(vec * rollingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(rollingDuration);
        Destroy(gameObject);
    }
}
