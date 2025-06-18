using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public GameObject effect_par;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().PersistentSetSpeed(1f);
            GameManager.Instance.GetHealth(0.5f);
            GameManager.Instance.GetScore(5000f, "Flower");
            GameManager.Instance.CountFlower();
            GameObject particle_tepm = Instantiate(effect_par, transform.position, transform.rotation);
            Destroy(particle_tepm, 1f);
            Destroy(this.gameObject);
        }
    }
}
