using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform Target;

    public Transform Head;
    public Transform Body;

    private void Update()
    {
        // Vector3 targetVerProjector = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.right);
        // Vector3 objVerProjector = Vector3.ProjectOnPlane(Head.forward, Vector3.right);
        //
        // Vector3 targetHorProjector = Vector3.ProjectOnPlane(Target.position - transform.position, Vector3.up);
        // Vector3 objHorProjector = Vector3.ProjectOnPlane(Body.forward, Vector3.up);
        //
        // float xAngle = Vector3.SignedAngle(objVerProjector, targetVerProjector, Vector3.right);
        // float yAngle = Vector3.SignedAngle(objHorProjector, targetHorProjector, Vector3.up);
        //
        // Head.rotation = Quaternion.Lerp(Head.rotation,
        //     Quaternion.Euler(xAngle, 0, 0), 1f);
        // Body.rotation = Quaternion.Lerp(Body.rotation,
        //     Quaternion.Euler(0, yAngle, 0), 1f);


        Vector3 dir = Target.position - transform.position;

        Vector3 dirXZ = new Vector3(dir.x, 0, dir.z);

        Head.rotation = Quaternion.Lerp(Head.transform.rotation, Quaternion.LookRotation(dir), 0.8f);
        Body.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.LookRotation(dirXZ), 0.8f);
    }
}