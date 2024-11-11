using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public float speed = 1f;
    public LayerMask groundLayer;

    public BoxCollider2D boxCollider;
    public Transform checkIsGrounded;
    public float groundCheckRadius = 0.5f;   
    public bool isGrounded;
    public Transform PlayerTransform;

    private float currentRotationY = 0;

    // Update is called once per frame
   
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        isGrounded = Physics2D.OverlapCircle(checkIsGrounded.position, groundCheckRadius, groundLayer);
        // Wenn der Gegner den Boden verliert und nicht schon dreht
        if (!isGrounded )
        {

            // Drehen und Richtung ändern

            currentRotationY = (currentRotationY == 0) ? 180 : 0;

            PlayerTransform.rotation = Quaternion.Euler(new Vector3(
                PlayerTransform.rotation.eulerAngles.x,
                currentRotationY,
                PlayerTransform.rotation.eulerAngles.z
            ));

        }
        
    }

    // Methode zur Prüfung, ob das Objekt den Boden berührt
    
}
