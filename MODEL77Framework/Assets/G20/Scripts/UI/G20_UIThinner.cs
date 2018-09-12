using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_UIThinner : MonoBehaviour {
    [SerializeField] Vector3 halfBox;
    [SerializeField] CanvasGroup paramGroup;
    [SerializeField] float hittingAlpha=0.5f;
    private void Start()
    {
        G20_GameManager.GetInstance().ChangedStateAction += StartRoutine;
    }
    void StartRoutine(G20_GameState _state)
    {
        if (_state == G20_GameState.INGAME)
        {
            StartCoroutine(CheckEnemyHit());
        }
    }
    IEnumerator CheckEnemyHit()
    {
        while (true)
        {
            //インゲームじゃなかったら終了
            if (G20_GameManager.GetInstance().gameState != G20_GameState.INGAME) yield break;
            var colliders=Physics.OverlapBox(transform.position,halfBox,transform.rotation);
            bool isHit=false;
            foreach (var col in colliders)
            {
                if (col.GetComponent<G20_HitDamage>())
                {
                    isHit = true;
                    break;
                }
            }
            if (isHit)
            {
                paramGroup.alpha= hittingAlpha;
            }
            else
            {
                paramGroup.alpha = 1.0f;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
