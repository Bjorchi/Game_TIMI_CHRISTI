using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown("w"))     //jump
     {
        rb.AddForce(new Vector2(0,1) * 300);
     }

     if (Input.GetKeyDown("d"))   //rechts
     {
        rb.AddForce(new Vector2(1,0) * 300);
     }

     if (Input.GetKeyDown("a"))    // links
     {
        rb.AddForce(new Vector2(1,0) * -300);
     }
     
     if (Input.GetKeyDown("s"))     //jump
     {
        rb.AddForce(new Vector2(0,1) * -300);
     }

    }
}
