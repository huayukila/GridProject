using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float Speed;
    public Transform target;
    public GameObject impactParticle; // bullet impact

    public Vector3 impactNormal;
    public Tower twr;
    private readonly float i = 0.05f; // delay time of bullet destruction
    private Vector3 lastBulletPosition;


    private void Update()
    {
        // Bullet move

        if (target)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * Speed);
            lastBulletPosition = target.transform.position;
        }

        // Move bullet ( enemy was disapeared )

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, lastBulletPosition, Time.deltaTime * Speed);

            if (transform.position == lastBulletPosition)
            {
                Destroy(gameObject, i);

                // Bullet hit ( enemy was disapeared )

                if (impactParticle != null)
                {
                    impactParticle = Instantiate(impactParticle, transform.position,
                        Quaternion.FromToRotation(Vector3.up, impactNormal)); // Tower`s hit
                    Destroy(impactParticle, 3);
                }
            }
        }
    }

    // Bullet hit

    private void OnTriggerEnter(Collider other) // tower`s hit if bullet reached the enemy
    {
        if (other.gameObject.transform == target)
        {
            target.GetComponent<EnemyHp>().Dmg(twr.dmg);
            Destroy(gameObject, i); // destroy bullet
            impactParticle = Instantiate(impactParticle, target.transform.position,
                Quaternion.FromToRotation(Vector3.up, impactNormal));
            impactParticle.transform.parent = target.transform;
            Destroy(impactParticle, 3);
        }
    }
}