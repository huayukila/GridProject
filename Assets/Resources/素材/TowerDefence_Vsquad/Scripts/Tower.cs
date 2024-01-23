using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool Catcher;
    public Transform shootElement;
    public GameObject Towerbug;
    public Transform LookAtObj;
    public GameObject bullet;
    public GameObject DestroyParticle;
    public Vector3 impactNormal_2;
    public Transform target;
    public int dmg = 10;
    public float shootDelay;
    public Animator anim_2;
    public TowerHP TowerHp;
    private float homeY;
    private bool isShoot;

    // for Catcher tower 

    private void Start()
    {
        anim_2 = GetComponent<Animator>();
        homeY = LookAtObj.transform.localRotation.eulerAngles.y;
        TowerHp = Towerbug.GetComponent<TowerHP>();
    }


    private void Update()
    {
        // Tower`s rotate

        if (target)
        {
            var dir = target.transform.position - LookAtObj.transform.position;
            dir.y = 0;
            var rot = Quaternion.LookRotation(dir);
            LookAtObj.transform.rotation = Quaternion.Slerp(LookAtObj.transform.rotation, rot, 5 * Time.deltaTime);
        }

        else
        {
            var home = new Quaternion(0, homeY, 0, 1);

            LookAtObj.transform.rotation = Quaternion.Slerp(LookAtObj.transform.rotation, home, Time.deltaTime);
        }


        // Shooting


        if (!isShoot) StartCoroutine(shoot());


        if (Catcher)
            if (!target || target.CompareTag("Dead"))
                StopCatcherAttack();

        // Destroy

        if (TowerHp.CastleHp <= 0)
        {
            Destroy(gameObject);
            DestroyParticle = Instantiate(DestroyParticle, Towerbug.transform.position,
                Quaternion.FromToRotation(Vector3.up, impactNormal_2));
            Destroy(DestroyParticle, 3);
        }
    }


    // for Catcher tower attack animation

    private void GetDamage()

    {
        if (target) target.GetComponent<EnemyHp>().Dmg(dmg);
    }

    private IEnumerator shoot()
    {
        isShoot = true;
        yield return new WaitForSeconds(shootDelay);


        if (target && Catcher == false)
        {
            var b = Instantiate(bullet, shootElement.position, Quaternion.identity);
            b.GetComponent<TowerBullet>().target = target;
            b.GetComponent<TowerBullet>().twr = this;
        }

        if (target && Catcher)
        {
            anim_2.SetBool("Attack", true);
            anim_2.SetBool("T_pose", false);
        }


        isShoot = false;
    }


    private void StopCatcherAttack()

    {
        target = null;
        anim_2.SetBool("Attack", false);
        anim_2.SetBool("T_pose", true);
    }
}