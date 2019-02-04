using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G20_HitBomb : G20_HitAction {

    [SerializeField] int damage = 9;
    [SerializeField] float explosionRadius=3.0f;
    public override void Execute(Vector3 hit_point)
    {
        Camera.main.GetComponent<G20_CameraShake>().Shake(G20_CameraShakeType.DOWNWARD, 0.4f, 20f, 1.4f);
        G20_FernPerformer.GetInstance().Shake();
		G20_EnemyCabinet.GetInstance().ExplodeCapsuleRange(
			transform.position + new Vector3(0, -5, 0),
			transform.position + new Vector3(0, 5, 0),
			explosionRadius);

		Destroy(gameObject);
    }

}