using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
    public TypeAttack tipoDeAtaque;
    public string nombreAtaque;
    public int cantidadDeDaño;
    public Transform controladorAtaque;
    public string stringAnimacion;
    public float radioAtaque;
    public Vector2 dimensionesCaja;
}
