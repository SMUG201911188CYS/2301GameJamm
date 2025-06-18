using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public GameObject effect_par;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject particle_tepm = Instantiate(effect_par, transform.position, transform.rotation);
            GameManager.Instance.GetScore(100f, "Trash");
            Destroy(particle_tepm, 1f);
            Destroy(this.gameObject);
        }
    }
}