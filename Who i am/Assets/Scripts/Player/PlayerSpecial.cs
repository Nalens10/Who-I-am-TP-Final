using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpecial : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorE;

    [Header("Animator Parameters")]
    [SerializeField] private string dashParam;
    [SerializeField] private string parryParam;
    [SerializeField] private string specialTrigger;

    [Header("Dash Configuracion")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private int dashDamage;

    private bool dashCanHit = false;
    private bool canDash = true;

    [Header("Parry Configuracion")]
    [SerializeField] private float parryDuration;
    [SerializeField] private float parryCooldown;
    [SerializeField] private float parryPerfectDuration;

    private bool perfectParry = false;
    private bool canParry = true;

    [Header("Habilidades Especiales")]
    [SerializeField] private float specialRange;
    [SerializeField] private int specialDamage;
    [SerializeField] private LayerMask enemyLayer;

    private void Update()
    {
        HandleDash();
        HandleParry();
        HandleSpecialAttack();
    }

    //Dash
    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector2 inputDir = Vector2.zero;

            if (Mathf.Abs(v) > 0)
                inputDir = v > 0 ? Vector2.up : Vector2.down;
            else if (Mathf.Abs(h) > 0)
                inputDir = h > 0 ? Vector2.right : Vector2.left;
            else
                inputDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            StartCoroutine(DashRoutine(inputDir));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dashCanHit) return;

        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TomarDaño(dashDamage, transform);  //pasa el daño y el jugador como sender
            dashCanHit = false;  //solo 1 impacto por dash
        }
    }


    private IEnumerator DashRoutine(Vector2 direction)
    {
        canDash = false;
        dashCanHit = true;

        animator.SetBool(dashParam, true);
        animatorE.SetBool(dashParam, true);
        ToggleInvulnerability(true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float finalSpeed = dashSpeed;

        // Si el dash es horizontal,multiplicar por 4, es la unica foma que encontre de que sean parejo los dashes
        if (direction.x != 0 && direction.y == 0)
            finalSpeed *= 4f;

        rb.velocity = direction * finalSpeed;

        

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;

        dashCanHit = false;

        animator.SetBool(dashParam, false);
        animatorE.SetBool(dashParam, false);
        ToggleInvulnerability(false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    //Parry
    private void HandleParry()
    {
        if (Input.GetKeyDown(KeyCode.E) && canParry)
        {
            StartCoroutine(ParryRoutine());
        }
    }

    private IEnumerator ParryRoutine()
    {
        canParry = false;
        perfectParry = false; // Reset al iniciar

        animator.SetBool(parryParam, true);
        animatorE.SetBool(parryParam, true);
        ToggleInvulnerability(true);

        //Perfect Parry
        yield return new WaitForSeconds(parryPerfectDuration);

        // Si el jugador acertó el perfect parry, reseteo instantáneo y salir
        if (perfectParry)
        {
            ToggleInvulnerability(false);
            animator.SetBool(parryParam, false);
            animatorE.SetBool(parryParam, false);

            canParry = true;
            yield break;
        }

        //Parry Normalito
        float remainingWindow = parryDuration - parryPerfectDuration;

        if (remainingWindow > 0)
            yield return new WaitForSeconds(remainingWindow);

        // Fin del parry
        ToggleInvulnerability(false);
        animator.SetBool(parryParam, false);
        animatorE.SetBool(parryParam, false);

        // Cooldown normal
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }


    //Special
    private void HandleSpecialAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger(specialTrigger);
            DoSpecialDamage();
        }
    }

    private void DoSpecialDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, specialRange, enemyLayer);

        foreach (var enemy in hits)
        {
            enemy.GetComponent<EnemyHealth>()?.TomarDaño(specialDamage, transform);
        }
    }

    //Invulnerability toggle
    private void ToggleInvulnerability(bool state)
    {
        // acá activás/desactivás tu sistema de daño
        // ejemplo: GetComponent<PlayerHealth>().invulnerable = state;
    }


    //Gizmos para visualizar el rango del special y la distancia del dash
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, specialRange);

        float dashDistance = dashSpeed * dashDuration;

        Gizmos.color = Color.blue;
        Vector3 dirHorizontal = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + dirHorizontal * dashDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * dashDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * dashDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + dirHorizontal * dashDistance, 0.1f);
    }
}


