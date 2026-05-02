using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ThrowingProjectile : MonoBehaviour

{
    [SerializeField] private Transform ParentScene;
    [SerializeField] private GameObject Stone;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int frameStep;

    private GameObject ghostStone;
    private Rigidbody2D stoneRB;

    private Scene simulationScene;
    private PhysicsScene2D physicsScene;



    void Start()
    {
        createPhysicsScene();
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {

    }
    private void createPhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        GameObject envClone = Instantiate(ParentScene.gameObject, ParentScene.position, ParentScene.rotation);

        Renderer[] renderers = envClone.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        Collider2D[] envColliders = envClone.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in envColliders)
        {
            col.enabled = false;
            col.enabled = true;
        }

        SceneManager.MoveGameObjectToScene(envClone, simulationScene);

        ghostStone = Instantiate(Stone);
        if (ghostStone.TryGetComponent<Renderer>(out Renderer rStone)) rStone.enabled = false;

        MonoBehaviour[] scripts = ghostStone.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        stoneRB = ghostStone.GetComponent<Rigidbody2D>();
        SceneManager.MoveGameObjectToScene(ghostStone, simulationScene);
    }
    public void TrajectorySimulation(Vector2 startPos, Vector2 throwVelocity)
    {
        ghostStone.transform.position = startPos;
        stoneRB.linearVelocity = throwVelocity;
        stoneRB.angularVelocity = 0f;

        Physics2D.SyncTransforms();

        lineRenderer.positionCount = frameStep;
        for (int i = 0; i < frameStep; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, ghostStone.transform.position);
        }
    }
    public void hideTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
