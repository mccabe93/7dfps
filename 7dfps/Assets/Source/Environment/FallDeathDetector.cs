using UnityEngine;

public class FallDeathDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(playerHealth.HealthMax);
        }
        var enemyActor = collision.gameObject.GetComponent<EnemyActor>();
        if (enemyActor != null && enemyActor.IsAlive)
        {
            enemyActor.Die();
        }
    }
}
