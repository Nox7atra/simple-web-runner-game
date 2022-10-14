
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.transform.GetComponent<Player>();
        player.Hit();
    }
}
