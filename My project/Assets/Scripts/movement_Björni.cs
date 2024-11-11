using UnityEngine;

public class movement_Björni : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    private int jumpCounter = 0;
    private bool isGrounded;
    public int jumpMaxCount = 2;

    // Bodenprüfung:
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    void Update()
    {
        // Bodenprüfung mit zusätzlicher Geschwindigkeitsüberprüfung
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) && Mathf.Abs(rb.linearVelocity.y) < 0.1f;

        // Sprunganzahl begrenzen
        if (isGrounded)
        {
            jumpCounter = 0;
        }

        // Doppelsprung: Leertaste zum Springen
        if (Input.GetKeyDown("up") && jumpCounter < jumpMaxCount)
        {
            Jump();
        }

        // Bewegung nach links und rechts mit "A" und "D"
        if (Input.GetKey("right")) // rechts
        {
            rb.AddForce(new Vector2(speed, 0));
        }

        if (Input.GetKey("left")) // links
        {
            rb.AddForce(new Vector2(-speed, 0));
        }
    }

    private void Jump()
    {
        // Geschwindigkeit in Y-Richtung zurücksetzen für konsistente Sprünge
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        // Kraft für den Sprung hinzufügen
        rb.AddForce(Vector2.up * speed, ForceMode2D.Impulse);

        // JumpCounter erhöhen
        jumpCounter++;
    }
}
