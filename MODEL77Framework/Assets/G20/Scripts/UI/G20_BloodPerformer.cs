using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_BloodPerformer : MonoBehaviour
{
    [SerializeField] GameObject[] bloodPanel;
    [SerializeField] G20_Player player;
    [SerializeField, Range(0, 10f)] float fadeDuration;
    [SerializeField,Range(0,1.0f)] float fadeValue;
    int damageCount = 0;
    // Update is called once per frame
    void Start()
    {
        player.recvDamageActions += (x, y) => ReceivedDamage();
    }
    void ReceivedDamage()
    {
        if (CheckBloodPanel(damageCount))
        {
            bloodPanel[damageCount].SetActive(true);
            StartCoroutine(FadeBlood(bloodPanel[damageCount].GetComponent<Image>()));
        }
        damageCount++;
    }
    bool CheckBloodPanel(int num)
    {
        return (0 <= num && num < bloodPanel.Length);
    }
    IEnumerator FadeBlood(Image bloodimage)
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bloodimage.color -= new Color(0, 0, 0, Time.deltaTime*(fadeValue/fadeDuration));
            yield return null;
        }
    }
}
