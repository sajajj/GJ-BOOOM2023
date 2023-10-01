using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DashEffect : MonoBehaviour
{
    public static void CreateDashEffect( Vector3 position, Vector3 dir, float dashSize, GameObject effectPrefab)
    {

        Transform dashTransform = Instantiate(effectPrefab, position, Quaternion.identity).transform;
        dashTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        dashTransform.localScale = new Vector3(dashSize / 35f, 1, 1);
    }

    static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if ( n < 0 )
        {
            n += 360;
        }
        return n;
    }
}
