using UnityEngine;

public class TowerTrigger : MonoBehaviour
{
    public Tower twr;
    public bool lockE;
    public GameObject curTarget;

    private void Update()
    {
        if (curTarget)
            if (curTarget.CompareTag("Dead")) // get it from EnemyHealth
            {
                lockE = false;
                twr.target = null;
            }


        if (!curTarget) lockE = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyBug") && !lockE)
        {
            twr.target = other.gameObject.transform;
            curTarget = other.gameObject;
            lockE = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemyBug") && other.gameObject == curTarget)
        {
            lockE = false;
            twr.target = null;
        }
    }
}