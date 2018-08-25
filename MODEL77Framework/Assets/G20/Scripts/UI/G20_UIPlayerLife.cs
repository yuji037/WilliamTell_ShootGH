using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//lifeがゲーム中10くらい回復すると厳しい設計
public class G20_UIPlayerLife : MonoBehaviour {
    [SerializeField]List<GameObject> lifeImages=new List<GameObject>();
    [SerializeField] G20_Player player;
    int activeCount=0;
    // Use this for initialization
	void Start () {
        player.recvDamageActions += ChangeLife;
        int pLife= player.HP;
        for (int i=0;i<pLife;i++)
        {
            lifeImages[i].SetActive(true);
            activeCount++;
        }
    }
	void ChangeLife(G20_Unit _unit)
    {
        if (_unit.HP < 0) return;
        //増やす
        while (lifeImages.Count < _unit.HP)
        {
            lifeImages[activeCount].SetActive(true);
            activeCount++;
        }
        //減らす
        while (activeCount> _unit.HP)
        {
            var img=lifeImages[0];
            lifeImages.RemoveAt(0);
            Destroy(img);
            activeCount--;
        }
    }
	// Update is called once per frame
	void Update () {
	}
}
