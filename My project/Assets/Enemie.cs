using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 rotation;
    public float groundCheckDistance = 1f; // Distanz zur Bodenerkennung
    public LayerMask groundLayer;
    private bool isTurning = false; // Flag, um mehrfaches Drehen zu verhindern

    public BoxCollider2D boxCollider;
    public Transform checkIsGrounded;
    public float groundCheckRadius = 0.5f;   
    public bool isGrounded;
    public Transform PlayerTransform;
    private int temp = 1;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        isGrounded = Physics2D.OverlapCircle(checkIsGrounded.position, groundCheckRadius, groundLayer);
        // Wenn der Gegner den Boden verliert und nicht schon dreht
        if (!isGrounded)
        {
            
            // Drehen und Richtung ändern
            //transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            //rotation = transform.eulerAngles;
            if(speed < 0) {
                PlayerTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else PlayerTransform.rotation = Quaternion.Euler(0, 0, 0);
            

            speed = -speed; // Geschwindigkeit umdrehen
            
            
        }
        
    }

    // Methode zur Prüfung, ob das Objekt den Boden berührt
    
}
