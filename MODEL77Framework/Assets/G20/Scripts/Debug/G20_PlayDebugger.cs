using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G20_PlayDebugger : MonoBehaviour {
    [SerializeField] G20_Player player;
    [SerializeField] GameObject invinObj;
    [SerializeField] GameObject cheatShotObj;
    [SerializeField] GameObject minusSpeed;
    [SerializeField] GameObject plusSpeed;
    private void Start()
    {

    }
    private void Update()
    {
        ChangePlayerInvin();
        ChangeCheatShoot();
    }
    void ChangePlayerInvin()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.IsInvincible = !player.IsInvincible;
        }
    }
    void ChangeCheatShoot()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            G20_BulletShooter.GetInstance().isCheating = !G20_BulletShooter.GetInstance().isCheating;
        }
    }
}
