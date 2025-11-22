using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string STRING_VELOCIDAD_HORIZONTAL = "VelocidadHorizontal";
    private const string STRING_VELOCIDAD_VERTICAL = "VelocidadVertical";
    private const string STRING_EN_SUELO = "EnSuelo";

    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

    [Header("Movimiento Horizontal")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float suavizadoMovimiento;
    private float velocidadSuavizada;
    private float entradaHorizontal;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector2 dimensionesCajaSuelo;
    [SerializeField] private LayerMask capasSuelo;
    private bool enSuelo;
    private bool entradaSalto;

    [Header("Doble Salto")]
    [SerializeField] private int maxSaltos;
    private int contadorSaltos;

    [Header("Paredes")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Vector2 dimensionesCajaPared;
    [SerializeField] private float velocidadDeslizamiento;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoBloqueoControl;

    private bool enPared;
    private bool deslizando;
    private bool bloqueandoControl;

    private void Update()
    {
        entradaHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            entradaSalto = true;

        // Suelo y pared
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSuelo, 0f, capasSuelo);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, capasSuelo);

        // Reset de saltos
        if (enSuelo)
            contadorSaltos = maxSaltos;
        if (enPared)
            contadorSaltos = maxSaltos;

        // Wall slide
        deslizando = !enSuelo && enPared && entradaHorizontal != 0f;

        if (deslizando)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x,Mathf.Clamp(rb2D.velocity.y, -velocidadDeslizamiento, float.MaxValue));
        }

        ControlarAnimaciones();
    }

    private void FixedUpdate()
    {
        ControlarMovimientoHorizontal();
        ControlarSalto();
        entradaSalto = false;
    }

    private void ControlarMovimientoHorizontal()
    {
        if (bloqueandoControl) return;

        float objetivoVelX = entradaHorizontal * velocidadMovimiento;
        float velX = Mathf.SmoothDamp(rb2D.velocity.x, objetivoVelX,
            ref velocidadSuavizada, suavizadoMovimiento);

        rb2D.velocity = new Vector2(velX, rb2D.velocity.y);

        if (entradaHorizontal > 0 && !MirandoALaDerecha()) Girar();
        if (entradaHorizontal < 0 && MirandoALaDerecha()) Girar();
    }

    private void ControlarSalto()
    {
        // Salto normal y doble salto
        if (entradaSalto && contadorSaltos > 0 && !deslizando)
        {
            contadorSaltos--;
            rb2D.velocity = new Vector2(rb2D.velocity.x, fuerzaSalto);
            return;
        }

        // Sakto desde pared
        if (entradaSalto && deslizando)
        {
            float direccion = MirandoALaDerecha() ? -1 : 1;

            bloqueandoControl = true;
            Invoke(nameof(DesbloquearControl), tiempoBloqueoControl);

            rb2D.velocity = new Vector2(
                direccion * fuerzaSaltoParedX,
                fuerzaSaltoParedY
            );

            GirarInstantaneo();
            return;
        }
    }

    private void DesbloquearControl()
    {
        bloqueandoControl = false;
    }

    private void Girar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void GirarInstantaneo()
    {
        transform.localScale =
            new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private bool MirandoALaDerecha()
    {
        return transform.localScale.x > 0;
    }

    private void ControlarAnimaciones()
    {
        animator.SetBool(STRING_EN_SUELO, enSuelo);
        animator.SetFloat(STRING_VELOCIDAD_HORIZONTAL, Mathf.Abs(rb2D.velocity.x));
        animator.SetFloat(STRING_VELOCIDAD_VERTICAL, rb2D.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSuelo);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }
}

