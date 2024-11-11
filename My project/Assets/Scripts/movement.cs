using UnityEngine;
using System.Collections;

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

    // Dashing-Variablen
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Bodenprüfung
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public TrailRenderer tr; // Korrigiert von trailRenderer auf TrailRenderer

    private bool facingRight = false;
    public Transform PlayerTransform;

    public float maxJumpHoldTime = 0.5f; // Maximale Dauer, um die Leertaste zu halten
    private float jumpHoldTime = 0f;     // Zeit, wie lange die Taste gehalten wird
    public float jumpHoldForce = 2f;     // Zusätzliche Kraft für den anhaltenden Sprung
    private bool isJumping = false;      // Variable, um festzustellen, ob der Spieler springt

    void Update()
    {
        // Bodenprüfung mit zusätzlicher Geschwindigkeitsüberprüfung
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) && Mathf.Abs(rb.linearVelocity.y) < 0.1f;

        // Wenn der Spieler gerade dabei ist zu dashen, dann breche die Bewegung ab
        if (isDashing)
        {
            return; // Dash-Coroutine läuft, also keine weiteren Eingaben verarbeiten
        }

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

        // Dashing ausführen, wenn Shift gedrückt wird
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) 
        {
            StartCoroutine(Dash());
        }

        // Bewegung nach rechts (D-Taste)
        if (Input.GetKey("d")) 
        {
            leftTimer = 0;
            if (rightTimer < TimerEnd)
            {
                rightTimer++;
            }

            if (!facingRight)
            {
                PlayerTransform.rotation = Quaternion.Euler(0, -180, 0); // Drehung nach rechts
            }
            facingRight = true;

            // Normale Bewegung (mit Damping)
            if (!isDashing) 
            {
                rb.linearVelocity = new Vector2(speed * rightTimer / TimerEnd, rb.linearVelocity.y);
            }
        }
        // Bewegung nach links (A-Taste)
        else if (Input.GetKey("a")) 
        {
            rightTimer = 0;
            if (leftTimer < TimerEnd)
            {
                leftTimer++;
            }
            if (facingRight)
            {
                PlayerTransform.rotation = Quaternion.Euler(0, 0, 0); // Drehung nach links
            }
            facingRight = false;

            // Normale Bewegung (mit Damping)
            if (!isDashing) 
            {
                rb.linearVelocity = new Vector2(-speed * leftTimer / TimerEnd, rb.linearVelocity.y);
            }
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

    private IEnumerator Dash() 
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Schwerkraft während des Dashs ausschalten

        // Bestimme die Richtung des Dashes basierend auf der Blickrichtung des Spielers
        float dashDirection = facingRight ? 1f : -1f; // Verwende 'facingRight', um die Richtung festzulegen

        // Setze die Geschwindigkeit für den Dash nur auf der X-Achse
        rb.linearVelocity = new Vector2(dashDirection * dashingPower, rb.linearVelocity.y);

        tr.emitting = true; // TrailRenderer Spur aktivieren
        yield return new WaitForSeconds(dashingTime); // Warte die Dash-Dauer ab

        tr.emitting = false; // TrailRenderer Spur deaktivieren
        rb.gravityScale = originalGravity; // Stelle die ursprüngliche Schwerkraft wieder her
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown); // Warte die Abkühlzeit ab
        canDash = true; // Erlaube den nächsten Dash
    }



}
