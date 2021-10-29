using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControler : MonoBehaviour
{
    // Start is called before the first frame update

    public float m_MaxHp = 100f;
    public float m_MaxLifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
