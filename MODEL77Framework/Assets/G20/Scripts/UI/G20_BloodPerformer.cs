using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_BloodPerformer : MonoBehaviour
{
    [SerializeField] GameObject[] bloodPanel;
    [SerializeField] G20_Player player;
    int damageCount=0;
    // Update is called once per frame
    void Start()
    {
        player.recvDamageActions += ReceivedDamage;
    }
    void ReceivedDamage(G20_Unit _unit)
    {
        if(CheckBloodPanel(damageCount))bloodPanel[damageCount].SetActive(true);
        damageCount++;
    }
    bool CheckBloodPanel(int num)
    {
        return (0 <= num && num < bloodPanel.Length);
    }
}
