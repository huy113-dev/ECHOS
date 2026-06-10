using UnityEngine;

public class MysFController : MonoBehaviour
{
    public bool isGameStartScene;
    public Animator Fire;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isGameStartScene)
        {
            Fire.SetTrigger("StartGame");
            
        }
    }
    public void TransformPos()
    {
        transform.position = offset;
    }


}
