using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelLoco : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = OVRManager.display.acceleration;
        dir.y = 0;


        transform.Translate(dir);
    }
}
