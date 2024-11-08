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

    public float maxJumpHoldTime = 0.5f; // maximale Dauer, um die Leertaste zu halten
    private float jumpHoldTime = 0f;     // Zeit, wie lange die Taste gehalten wird
    public float jumpHoldForce = 2f;     // zusätzliche Kraft für den anhaltenden Sprung
    private bool isJumping = false;      // Variable, um festzustellen, ob der Spieler springt

    void Update()
    {
        // Bodenprüfung mit zusätzlicher Geschwindigkeitsüberprüfung
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) && Mathf.Abs(rb.linearVelocity.y) < 0.1f;

        // Sprunganzahl begrenzen
        if (isGrounded)
        {
            jumpCounter = 0;
            isJumping = false;
            jumpHoldTime = 0f; // Haltezeit zurücksetzen, wenn Spieler Boden berührt
        }

        if (Input.GetKeyDown("space") && jumpCounter < jumpMaxCount)
        {
            StartJump(); 
        }

        // Fortsetzen des Sprungs, wenn die Leertaste gehalten wird und die maximale Haltezeit noch nicht erreicht ist
        if (Input.GetKey("space") && isJumping && jumpHoldTime < maxJumpHoldTime)
        {
            HoldJump();
        }

        // Beenden des Sprungs, wenn die Leertaste losgelassen wird
        if (Input.GetKeyUp("space"))
        {
            isJumping = false; 
        }

        if (Input.GetKey("d")) // rechts
        {
            leftTimer = 0;
            if (rightTimer < TimerEnd)
            {
                rightTimer++;
            }

            if (!facingRight)
            {
                PlayerTransform.rotation = Quaternion.Euler(0, -180, 0);
            }
            facingRight = true;
            rb.linearVelocity = new Vector2(speed * rightTimer / TimerEnd, rb.linearVelocity.y);
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
            rb.linearVelocity = new Vector2(-speed * leftTimer / TimerEnd, rb.linearVelocity.y);
        }
        else
        {
            // Keine Eingabe: Bewegung stoppen
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * DampingSpeed, rb.linearVelocity.y);
            leftTimer = rightTimer = 0;
        }
    }

    private void StartJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Geschwindigkeit in Y-Richtung zurücksetzen
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        jumpCounter++; // Zähler für Sprünge erhöhen
        isJumping = true; // Setzt isJumping auf true, für doubleJump
    }

    // Methode, um den Sprung zu halten und eine höhere Sprunghöhe zu ermöglichen
    private void HoldJump()
    {
        rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Force); // Zusätzliche Kraft während des Haltens
        jumpHoldTime += Time.deltaTime; // Erhöht die Haltezeit, solange die Taste gehalten wird
    }
}
