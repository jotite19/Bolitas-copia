using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangleController : MonoBehaviour
{
    public float m_MaxDmg = 50f;
    public float m_MaxLifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
