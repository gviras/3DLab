using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{

    void Update()
    {
        Spin();
    }

    void Spin()
    {
        transform.Rotate(0, 50*Time.deltaTime, 0);
    }
}
