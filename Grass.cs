using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem fxHit;
    private bool isCut;

    //Todo objeto que for receber dano utilizará o amount, quantidade de hits necessários, do GetHit
    void GetHit(int amount) {

        if (isCut == false) {
            isCut = true; 
            //Fazendo a grama diminuir de tamanho ao ser cortada
            transform.localScale = new Vector3(1f, 1f, 1f);
            //exibindo particula grama
            fxHit.Emit(20);

        }        

    }
}
