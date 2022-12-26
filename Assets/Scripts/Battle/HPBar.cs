using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health; //Crea el HameObject de la barra de salud

    public void SetHP(float hpNormalized)//Modifica la escala de la barra de salud
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}
