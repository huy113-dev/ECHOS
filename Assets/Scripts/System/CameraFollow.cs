using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int currentFPS;
    public Transform Target;
    public Vector3 offset;
    private float deltaTime;
    GUIStyle style = new GUIStyle();
    private float timer;


    void Start()
    {
        Application.targetFrameRate = 120;
        style.fontSize = 50;
        style.normal.textColor = Color.green;
        timer = 0f;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Target.transform.position.x + offset.x, transform.position.y, transform.position.z);
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        timer += Time.unscaledDeltaTime;
        if (timer >= 0.5f)
        {
            currentFPS = Mathf.RoundToInt(1.0f / deltaTime);
            timer = 0f;
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 50), $"FPS: {currentFPS}", style);
    }
}
