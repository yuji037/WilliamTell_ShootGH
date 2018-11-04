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

    bool isTargetingPlayer = true;

    protected override void childAIStart()
    {
        moveVec = Camera.main.transform.position - transform.position;
        enemy.deathActions += (a, b) => isTargetingPlayer = false;
        moveVec.Normalize();
        moveVec.y += init_vy;
        StartCoroutine(AppleFlyRoutine());
    }

    IEnumerator AppleFlyRoutine()
    {
        while (isTargetingPlayer)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME || enemy.HP <= 0 || transform.position.y <= -2f)
            {
                break;
            }
            moveVec.y -= gravity * Time.deltaTime;
            transform.Translate(moveVec * enemy.Speed * Time.deltaTime, Space.World);
            appleModel.up = moveVec;
            if (radius >= Vector3.Distance(Camera.main.transform.position, transform.position))
            {
                G20_EnemyAttack.GetInstance().Attack(1);
                enemy.RecvDamage(enemy.HP, G20_Unit.G20_DamageType.System);
            }
            yield return null;
        }
        GetComponent<SphereCollider>().enabled = true;
        var rh = gameObject.AddComponent<Rigidbody>();
        rh.isKinematic = false;
        var vec = transform.position - Camera.main.transform.position;
        rh.AddForce(vec * rollingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(rollingDuration);
        Destroy(gameObject);
    }

}
