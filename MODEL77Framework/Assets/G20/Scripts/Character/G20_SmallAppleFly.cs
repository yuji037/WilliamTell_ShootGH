using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_SmallAppleFly : G20_Unit
{
    Vector3 moveVec;

    [SerializeField]
    float radius;

    [SerializeField]
    float moveSpeed = 1.0f;

    [SerializeField]
    float rollingPower;

    [SerializeField]
    float rollingDuration = 3.0f;

    [SerializeField]
    float init_vy = 1.0f;

    [SerializeField]
    float gravity= 1.0f;

    [SerializeField]
    Vector3 rot = new Vector3(0, 0, 0);

    bool isTargetingPlayer = true;

    private void Start()
    {
        transform.Rotate(90, 0, 0);
        moveVec = Camera.main.transform.position - transform.position;
        deathActions += (a, b) => isTargetingPlayer = false;
        moveVec.Normalize();
        moveVec.y += init_vy;
        StartCoroutine(AppleFlyRoutine());
    }

    IEnumerator AppleFlyRoutine()
    {
        while (isTargetingPlayer)
        {
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME)
            {
                break;
            }
            moveVec.y -= gravity*Time.deltaTime;
            transform.Translate(moveVec * moveSpeed * Time.deltaTime, Space.World);
            transform.Rotate(rot);
            if (radius >= Vector3.Distance(Camera.main.transform.position, transform.position))
            {
                G20_EnemyAttack.GetInstance().Attack(1);
                RecvDamage(HP, G20_DamageType.System);
            }
            yield return null;
        }
        var rh = GetComponent<Rigidbody>();
        rh.isKinematic = false;
        var vec = transform.position - Camera.main.transform.position;
        rh.AddForce(vec * rollingPower, ForceMode.Impulse);
        yield return new WaitForSeconds(rollingDuration);
        Destroy(gameObject);
    }


}
