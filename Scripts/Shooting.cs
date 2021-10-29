using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public Rigidbody bullet;
    public Rigidbody triangle;

    public Transform m_FireTransform;        

    [SerializeField] Transform orientation;
    [SerializeField] KeyCode fireButton = KeyCode.Mouse0;
    Vector3 shootDirection;
    Vector3 moveDirection;

    public int m_PlayerNumber = 1;
    public float shootForce = 150f;
         
    private void Update()
    {
        if (!drawingMenu.isDrawing)
        {
            if (Input.GetKeyDown(fireButton))
            {
                //Fire();
            }
        }
    }

    private void Fire()
    {
        Rigidbody bulletInstance = Instantiate(bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shootDirection = orientation.forward * shootForce + moveDirection;
        bulletInstance.AddForce(shootDirection, ForceMode.Impulse);
    }

    public void TriangleShooting()
    {
        Rigidbody triangleInstance = Instantiate(triangle, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shootDirection = orientation.forward * shootForce + moveDirection;
        triangleInstance.AddForce(shootDirection, ForceMode.Impulse);
    }
}