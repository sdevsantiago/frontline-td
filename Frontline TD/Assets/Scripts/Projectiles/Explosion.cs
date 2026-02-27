using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 2f);
    }
}
