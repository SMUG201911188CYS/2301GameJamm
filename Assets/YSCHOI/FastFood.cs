using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFood : MonoBehaviour
{
    public GameObject effect_par;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().TemporalSetSpeed(5.0f, 5.0f);
            GameManager.Instance.GetScore(1500, "FastFood");
            GameManager.Instance.LostHealth(0.25f);
            GameObject particle_tepm = Instantiate(effect_par, transform.position, transform.rotation);
            Destroy(particle_tepm, 1f);
            Destroy(this.gameObject);
        }
    }
}
