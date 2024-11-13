using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;
    public int jumpHeight;
    public int jumpCounter = 0;
    public bool isGrounded;
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
    public TrailRenderer tr;

    private bool facingRight = false;
    public Transform PlayerTransform;

    // Jump Charge Variablen
    public float maxChargeTime = 1f;    // Maximale Aufladezeit für den Sprung
    private float chargeTime = 0f;      // Aktuelle Aufladezeit
    public float minJumpForce = 5f;     // Minimale Sprungkraft
    public float maxJumpForce = 15f;    // Maximale Sprungkraft bei voller Aufladung
    private bool isChargingJump = false;// Ob der Spieler gerade den Sprung auflädt

    public float groundedDelay = 0.1f; // Verzögerung in Sekunden
    private float groundedTimer = 0f; // Timer für die Verzögerung

    void Update()
    {
        // Bodenprüfung
        bool currentlyGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)
                                  && Mathf.Abs(rb.linearVelocity.y) < 0.1f;
        // Wenn der Spieler gerade dabei ist zu dashen, dann breche die Bewegung ab
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpCounter == 1)
        {
            JumpSmall();
        }
        // Aufladen des Sprungs, wenn die Leertaste gedrückt wird und die Sprunganzahl erlaubt ist
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter == 0)
        {
            isChargingJump = true;
            chargeTime = 0f;
        }

        // Erhöhe die Aufladung, solange die Leertaste gedrückt wird und die maximale Zeit noch nicht erreicht ist
        if (Input.GetKey(KeyCode.Space) && isChargingJump && isGrounded)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0, maxChargeTime); // Begrenze die Aufladung auf die maximale Zeit
            
        }

        // Springen, wenn die Leertaste losgelassen wird
        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
        {
            currentlyGrounded = false;
            Jump();
            isChargingJump = false;
        }
        

        // Sprunganzahl zurücksetzen und Aufladezeit stoppen, wenn der Spieler den Boden berührt




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

        if (currentlyGrounded)
        {
            groundedTimer += Time.deltaTime;
            if (groundedTimer >= groundedDelay && !isChargingJump)
            {
                jumpCounter = 0; // Reset only after delay
            }
            isGrounded = true;
        }
        else
        {
            groundedTimer = 0f;
            isGrounded = false;
        }

    }

    public void Jump()
    {
        print("jump");
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeTime / maxChargeTime); // (1.Parameter, 2.Parameter, prozent von Aufladen)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Setzt die Y-Geschwindigkeit zurück, um den Sprung zu starten
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Wendet die berechnete Sprungkraft an
        jumpCounter++; // Erhöht den Sprungzähler
    }

    public void JumpSmall()
    {
        print("jumo small");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
        jumpCounter++; // Erhöht den Sprungzähler
    }

    private IEnumerator Dash() 
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Schwerkraft während des Dashs ausschalten

        float dashDirection = facingRight ? 1f : -1f; // Bestimmt die Dash-Richtung basierend auf der Blickrichtung
        rb.linearVelocity = new Vector2(dashDirection * dashingPower, rb.linearVelocity.y);

        tr.emitting = true; // TrailRenderer Spur aktivieren
        yield return new WaitForSeconds(dashingTime); // Warte die Dash-Dauer ab

        tr.emitting = false; // TrailRenderer Spur deaktivieren
        rb.gravityScale = originalGravity; // Stelle die ursprüngliche Schwerkraft wieder her
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown); // Warte die Abkühlzeit ab
        canDash = true;
    }
}
