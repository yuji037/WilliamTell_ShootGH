using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_FlyAI : G20_AI
{
    Vector3 moveVec;

    [SerializeField]
    float radius;

    [SerializeField]
    float rollingPower;

    [SerializeField]
    float rollingDuration = 3.0f;

    [SerializeField]
    float init_vy = 1.0f;

    [SerializeField]
    float gravity = 1.0f;

    [SerializeField]
    Transform appleModel;

    [SerializeField]
    float spinSpeed = 1.0f;
    bool isTargeting = true;
    protected override void ChildInit()
    {
        enemy.deathActions += (a,b)=> StartCoroutine(BoundDeath());
    }
    protected override void childAIStart()
    {
        moveVec = Camera.main.transform.position - transform.position;
        moveVec.Normalize();
        moveVec.y += init_vy;
        StartCoroutine(AppleFlyRoutine());
    }
    IEnumerator AppleFlyRoutine()
    {
        while (G20_GameManager.GetInstance().gameState == G20_GameState.INGAME&&isTargeting)
        {
            moveVec.y -= gravity * Time.deltaTime;
            transform.Translate(moveVec * enemy.Speed *Time.deltaTime, Space.World);
            transform.up = moveVec;
            appleModel.Rotate(0,360*spinSpeed*Time.deltaTime,0);
            if (radius >= Vector3.Distance(Camera.main.transform.position, transform.position))
            {
                G20_EnemyAttack.GetInstance().Attack(enemy.Attack);
                yield return StartCoroutine(SusideCoroutine());
            }
            if (deathposition_y>=transform.position.y)
            {
                yield return StartCoroutine(SusideCoroutine());
            }
            yield return null;
        }
        
    }

    IEnumerator BoundDeath()
    {
        isTargeting = false;
        GetComponent<SphereCollider>().enabled = true;
        var rh = gameObject.AddComponent<Rigidbody>();
        rh.isKinematic = false;
        var vec = transform.position - Camera.main.transform.position;
        rh.AddForce(vec * rollingPower, ForceMode.Impulse);
        rh.AddTorque(new Vector3(10,0,0),ForceMode.Impulse);
        yield return new WaitForSeconds(rollingDuration);
        Destroy(gameObject);
    }

}
