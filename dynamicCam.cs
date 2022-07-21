using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicCam : MonoBehaviour
{
    //-----------Cameras-----------
    public GameObject vCam2;

    //Verificando se o trigger da camera foi acionado
    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                vCam2.SetActive(true);
                break;
        }
    }

    //retornando para a c�mera normal

    private void OnTriggerExit(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                vCam2.SetActive(false);
                break;
        }
    }
}
