using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player.gameObject) return;
        _player.GroundState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player.gameObject) return;
        _player.GroundState(false);
    }
}
