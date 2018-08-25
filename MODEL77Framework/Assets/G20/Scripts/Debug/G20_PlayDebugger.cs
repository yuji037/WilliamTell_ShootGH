using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_PlayDebugger : MonoBehaviour {
    [SerializeField] G20_Player player;
    [SerializeField] Text invinText;
    private void Start()
    {
        invinText.gameObject.SetActive(player.IsInvincible);
    }
    private void Update()
    {
        ChangePlayerInvin();
    }
    void ChangePlayerInvin()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.IsInvincible = !player.IsInvincible;
            invinText.gameObject.SetActive(player.IsInvincible);
        }
    }
}
