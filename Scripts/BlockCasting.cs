using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCasting : MonoBehaviour
{
    public Rigidbody block;
    public Transform blockTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void evoqueBlock()
    {
        Rigidbody bulletInstance = Instantiate(block, blockTransform.position, blockTransform.rotation) as Rigidbody;
    }
}
