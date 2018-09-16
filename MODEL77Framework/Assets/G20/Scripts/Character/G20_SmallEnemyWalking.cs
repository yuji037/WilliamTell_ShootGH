using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_SmallEnemyWalking : MonoBehaviour
{
    G20_FallAppleSound fallAppleSound;
    G20_EnemyAnimation enemyAnim;
    CharacterController characon;
    [SerializeField]float speed = 1.0f;
    // Use this for initialization
    void Awake()
    {
        fallAppleSound=GetComponent<G20_FallAppleSound>();
        enemyAnim = GetComponent<G20_EnemyAnimation>();
        characon = GetComponent<CharacterController>();
        fallAppleSound.firstCollisionHItAction += _=>ChangeWalk();
    }
    private void FixedUpdate()
    {
        characon.Move(Vector3.down * Time.deltaTime);
    }
    void ChangeWalk()
    {
        StartCoroutine(WalkRoutine());
    }
    IEnumerator WalkRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(JumpToCameraDir());
        Vector3 dir =UnityEngine.Random.Range(0, 2)==0? Vector3.right : Vector3.left;
        while (true)
        {
            yield return StartCoroutine(WalkGruGru(dir));
            dir *= -1;
        }
    }
    IEnumerator JumpToCameraDir()
    {
        enemyAnim.PlayAnimation(G20_AnimType.Jump);
        var dir=(Camera.main.transform.position - transform.position).normalized;
        float jumpValue = 1.5f;
        while(transform.position.y>=0.2f)
        {
            characon.Move(Time.deltaTime*dir);
            characon.Move(Vector3.up*jumpValue*Time.deltaTime);
            jumpValue -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator WalkGruGru(Vector3 _dir)
    {
        enemyAnim.PlayAnimation(G20_AnimType.Run,speed);
        transform.LookAt(transform.position+_dir);
        for (float t = 0; t < 5.0f; t += Time.deltaTime)
        {
            characon.Move(Time.deltaTime * _dir* speed);
            yield return null;
        }
    }
}
