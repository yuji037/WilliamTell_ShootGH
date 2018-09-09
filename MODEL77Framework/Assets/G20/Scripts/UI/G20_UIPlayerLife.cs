using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//lifeがゲーム中10くらい回復すると厳しい設計
public class G20_UIPlayerLife : MonoBehaviour {
    [SerializeField]List<Image> lifeImages=new List<Image>();
    [SerializeField] G20_Player player;
    [SerializeField] float fadeDuration;
    int activeCount=0;
    // Use this for initialization
	void Start () {
        player.recvDamageActions += ChangeLife;
        int pLife= player.HP;
        for (int i=0;i<pLife-1;i++)
        {
            lifeImages[i].gameObject.SetActive(true);
            activeCount++;
        }
    }
	void ChangeLife(G20_Unit _unit)
    {
        if (_unit.HP < 0) return;
        //増やす
        while (lifeImages.Count < _unit.HP-1)
        {
            lifeImages[activeCount].gameObject.SetActive(true);
            activeCount++;
        }
        if (_unit.HP <= 0) return;
        //減らす
        while (activeCount> _unit.HP-1)
        {
            var img=lifeImages[0];
            lifeImages.RemoveAt(0);
            StartCoroutine(lifeImageFade(img));
            activeCount--;
        }
    }
    IEnumerator lifeImageFade(Image img)
    {
        for (float t=0;t<fadeDuration;t+=Time.deltaTime)
        {
            img.fillAmount = 1.0f-Mathf.Lerp(0,1,t/fadeDuration);
            yield return null;
        }
        Destroy(img);
    }
}
