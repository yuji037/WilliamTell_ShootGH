using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class G20_UIScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    //0除算で作成
    [SerializeField] float scoreChangeInterval = 0.1f;
    private void Start()
    {
        G20_ScoreCountUpPerformer.GetInstance().StartCountUpScore(scoreText,0,()=>G20_ScoreManager.GetInstance().GetBaseScore(), scoreChangeInterval);
    }

}
