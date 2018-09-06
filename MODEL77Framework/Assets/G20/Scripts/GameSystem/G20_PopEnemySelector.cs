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
        GameObject enemy = null;
        switch (enemyType)
        {
            case G20_EnemyType.NORMAL:
                enemy=Instantiate(enemyPrefab[(int)G20_EnemyModelType.Normal]);
                Destroy(enemy.GetComponent<G20_NormalStraightAI>());
                break;
            case G20_EnemyType.GOLDEN:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Golden]);
                Destroy(enemy.GetComponent<G20_NormalStraightAI>());
                break;
            case G20_EnemyType.BOMB:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Bomb]);
                Destroy(enemy.GetComponent<G20_StraightBombAI>());
                break;
            case G20_EnemyType.SMALL:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Small]);
                break;
            case G20_EnemyType.NORMAL_STRAIGHT:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Normal]);
                Destroy(enemy.GetComponent<G20_NormalAI>());
                break;
            case G20_EnemyType.GOLDEN_STRAIGHT:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Golden]);
                Destroy(enemy.GetComponent<G20_NormalAI>());
                break;
            case G20_EnemyType.BOMB_STRAIGHT:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Bomb]);
                Destroy(enemy.GetComponent<G20_BombAI>());
                break;
            case G20_EnemyType.SMALL_STRAIGHT:
                enemy = Instantiate(enemyPrefab[(int)G20_EnemyModelType.Small]);
                break;
        }
        return enemy;
    }
}
