using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BombController : MonoBehaviour {
    //向かってくる向きのスピード
    [SerializeField] float speed=1;
    //重力
    [SerializeField] float gravity = 100.0f;
    //最高到達点の変化
    [SerializeField] float init_v = 0.5f;

    GameObject target;
     Vector3 targetPos;
     Vector3 distanceVec = Vector3.zero;
    float distance = 9999;
    // Use this for initialization
    void Start () {

        target = GameObject.FindGameObjectWithTag("MainCamera");

    }

	// Update is called once per frame
	void Update () {
       
    }

    public void Bombthrow(float attackRange,int damage)
    {
        StartCoroutine(BombthrowCoroutine(attackRange,damage));

    }


    IEnumerator BombthrowCoroutine(float attackRange, int damage)
    {
        
        Vector3 moveVec=Vector3.zero;


        while (true)
        {


            init_v -= gravity * Time.deltaTime;
            moveVec.y = init_v;
            transform.position += moveVec * Time.deltaTime;


            distanceVec = target.transform.position - transform.position;
            moveVec = distanceVec.normalized;
            distanceVec.y = 0;
            distance = distanceVec.magnitude;
            if (distance < attackRange)
            {
                G20_EnemyAttack.GetInstance().Attack(1);
                Destroy(this.gameObject);

            }

            yield return null;
        }
    }


	
}
