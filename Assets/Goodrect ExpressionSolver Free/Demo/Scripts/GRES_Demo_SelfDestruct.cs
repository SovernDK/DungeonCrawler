using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GRES_Demo_SelfDestruct : MonoBehaviour
{
    float lifetime = 1.5f;
    void Start()
    {
        Invoke("SelfDestruct", lifetime);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
