using UnityEngine;

public class Bell : MonoBehaviour
{
    [SerializeField] private ParticleSystem bellWavePrefab;
    [SerializeField] private int BellId;
    public BellPuzzleManager Manager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            Instantiate(bellWavePrefab, transform.position, Quaternion.identity).Play();
            Manager.BellHit(BellId);
        }
    }

}
