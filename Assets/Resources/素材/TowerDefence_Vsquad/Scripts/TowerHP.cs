using UnityEngine;

public class TowerHP : MonoBehaviour
{
    public int CastleHp = 20;

    private void Update()
    {
        if (CastleHp <= 0)
        {
            gameObject.tag = "Castle_Destroyed"; // send it to TowerTrigger to stop the shooting
            Destroy(gameObject);
        }
    }


    public void Dmg_2(int DMG_2count)
    {
        CastleHp -= DMG_2count;
    }
}