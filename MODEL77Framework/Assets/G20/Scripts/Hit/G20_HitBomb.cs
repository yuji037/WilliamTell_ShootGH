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
        Explosion();
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,explosionRadius);
    }
    void Explosion()
    {
        var colliders=Physics.OverlapCapsule(transform.position + new Vector3(0, -5, 0),transform.position+new Vector3(0,5,0),explosionRadius);
        foreach (var col in colliders)
        {
            var hitDamage = col.GetComponent<G20_HitDamage>();
            if (hitDamage)
            {
                hitDamage.Target.RecvDamage(hitDamage.Target.HP,G20_Unit.G20_DamageType.Player);
            }
        }
    }
}