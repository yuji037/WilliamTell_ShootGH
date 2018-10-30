using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitBuff : G20_HitAction {

    [SerializeField]
    G20_Enemy enemy;
    bool isBuffed = false;
    [SerializeField]
    GameObject buffEffect;

    public override void Execute(Vector3 hit_point)
    {
        if ( !isBuffed )
        {
            enemy.HealHP(2);
            var enemyAnim = enemy.GetComponent<G20_EnemyAnimation>();
            var lastAnim = enemyAnim.lastAnim;
            StartCoroutine(BuffEffectActivateCoroutine());
            enemyAnim.PlayAnimation(G20_AnimType.PowerUp,null,
                ()=> {
                    if ( !enemy || enemy.HP <= 0 ) return;
                    enemy.AddBuff(new G20_SpeedBuff(enemy, 100f, 0.5f));
                    enemyAnim.PlayAnimation(lastAnim);
                });
            Debug.Log(lastAnim.ToString());
        }
        isBuffed = true;
    }

    IEnumerator BuffEffectActivateCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        if ( buffEffect )
            buffEffect.SetActive(true);
    }
}
