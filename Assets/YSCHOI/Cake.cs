using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject effect_par;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().PersistentSetSpeed(1.0f);
            GameManager.Instance.GetScore(1000f, "Cake");
            GameObject particle_tepm = Instantiate(effect_par, transform.position, transform.rotation);
            Destroy(particle_tepm, 1f);
            Destroy(this.gameObject);
        }
    }
}
