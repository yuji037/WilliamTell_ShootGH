using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum G20_EnemyModelType
{
    Normal,
    Golden,
    Bomb,
    Small,
}
public class G20_PopEnemySelector : MonoBehaviour {
    [SerializeField]
    GameObject[] enemyPrefab;
    public GameObject GetPopEnemy(G20_EnemyType enemyType)
    {
        switch (enemyType)
        {
            case G20_EnemyType.NORMAL:
                return SelectEnemyAI<G20_NormalAI>(G20_EnemyModelType.Normal);
            case G20_EnemyType.GOLDEN:
                return SelectEnemyAI<G20_NormalAI>(G20_EnemyModelType.Golden);
            case G20_EnemyType.BOMB:
                return SelectEnemyAI<G20_BombAI>(G20_EnemyModelType.Bomb);
            case G20_EnemyType.SMALL:
                return SelectEnemyAI<G20_SmallAI>(G20_EnemyModelType.Small);
            case G20_EnemyType.NORMAL_STRAIGHT:
                return SelectEnemyAI<G20_NormalStraightAI>(G20_EnemyModelType.Normal);
            case G20_EnemyType.GOLDEN_STRAIGHT:
                return SelectEnemyAI<G20_NormalStraightAI>(G20_EnemyModelType.Golden);
            case G20_EnemyType.BOMB_STRAIGHT:
                return SelectEnemyAI<G20_BombStraightAI>(G20_EnemyModelType.Bomb);
            case G20_EnemyType.SMALL_STRAIGHT:
                return SelectEnemyAI<G20_SmallAI>(G20_EnemyModelType.Small);
        }
        return null;
    }
    GameObject SelectEnemyAI<T>(G20_EnemyModelType enemyModelType)
        where T:G20_AI
    {
        var enemyObj = Instantiate(enemyPrefab[(int)enemyModelType]);
        var enemy = enemyObj.GetComponent<G20_Enemy>();
        var AIs =enemy.GetComponentsInChildren<G20_AI>();
        T selectAI=null;
        for (int i=0;i<AIs.Length;i++)
        {
            if(AIs[i] is T)
            {
                selectAI=(T)AIs[i];
            }
            else
            {
                Destroy(AIs[i]);
            }
        }
        enemy.SetEnemyAI(selectAI);
        return enemyObj;
    }
}
