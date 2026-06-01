using UnityEngine;

public class IdleState : IState
{
    private float timer, randomTime;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0f;
        randomTime = Random.Range(3f, 6f);
    }
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
    public void OnExit(Enemy enemy)
    {

    }

}
