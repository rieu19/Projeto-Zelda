using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    private GameManager _GameManager;
    public GameObject vCam2;


    private void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                vCam2.SetActive(true);
                break;

            case "Coletavel":

                _GameManager.setGem(1);
                Destroy(other.gameObject);

                break;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "CamTrigger":
                vCam2.SetActive(false);
                break;

        }
    }

}
