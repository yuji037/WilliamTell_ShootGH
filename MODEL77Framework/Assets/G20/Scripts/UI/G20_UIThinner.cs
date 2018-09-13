using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_UIThinner : MonoBehaviour
{
    [SerializeField] Vector3 halfBox;
    [SerializeField] Image[] paramImages;
    [SerializeField] Color changeColor;
    Color[] defaultColors;
    private void Start()
    {
        defaultColors = new Color[paramImages.Length];
        for(int i=0;i<defaultColors.Length;i++)
        {
            defaultColors[i]=paramImages[i].color;
        }
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
            var colliders = Physics.OverlapBox(transform.position, halfBox, transform.rotation);
            bool isHit = false;
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
                foreach (var i in paramImages)
                {
                    i.color = changeColor;
                }
            }
            else
            {

                for (int i =0;i<paramImages.Length;i++)
                {
                    paramImages[i].color = defaultColors[i];
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
