using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private new GameObject renderer;
    void Start()
    {
        renderer.SetActive(false);
    }
}
