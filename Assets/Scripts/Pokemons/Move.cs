using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move //Crea las caracteriticas basica de una movimiento
{
    public MoveBase Base { get; set; }

    public int PP { get; set; }

    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}
