using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    public int jumpHeight;
    private int jumpCounter = 0;
    private bool isGrounded;
    public int jumpMaxCount = 2;
    public float DampingSpeed;

    public int leftTimer = 0;
    public int rightTimer = 0;

    public int TimerEnd;

    // Bodenprüfung:
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool facingRight = false;
    public Transform PlayerTransform;

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
        if (Input.GetKeyDown("space") && jumpCounter < jumpMaxCount)
        {
            Jump();
        }

        // Bewegung nach links und rechts mit "A" und "D"
        if (Input.GetKey("d")) // rechts
        {
            leftTimer = 0;
            if(rightTimer < TimerEnd)
            {
                rightTimer++;
            }

            if (!facingRight)
            {
                PlayerTransform.rotation = Quaternion.Euler(0, -180, 0);
            }
            facingRight = true;
            rb.linearVelocity = new Vector2(speed * rightTimer / TimerEnd, rb.linearVelocity.y); // Setzt die Geschwindigkeit in x-Richtung auf `speed`
            //rb.AddForce(new Vector2(speed, rb.linearVelocity.y));
        }
        else if (Input.GetKey("a")) // links
        {
            rightTimer = 0;
            if (leftTimer < TimerEnd)
            {
                leftTimer++;
            }
            if (facingRight)
            {
                PlayerTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
            facingRight = false;
            rb.linearVelocity = new Vector2(-speed * leftTimer / TimerEnd, rb.linearVelocity.y); // Setzt die Geschwindigkeit in x-Richtung auf `-speed`
            //rb.AddForce(new Vector2(-speed, rb.linearVelocity.y));
        }
        else
        {
            // Keine Eingabe: Bewegung stoppen
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * DampingSpeed, rb.linearVelocity.y);

            leftTimer = rightTimer = 0;
        }
    }

    private void Jump()
    {
        // Geschwindigkeit in Y-Richtung zurücksetzen für konsistente Sprünge
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        // Kraft für den Sprung hinzufügen
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

        // JumpCounter erhöhen
        jumpCounter++;
    }
}
