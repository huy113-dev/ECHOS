using UnityEngine;

public class InvestigateState : IState
{
    private float timer;
    private bool hasReachedPoint;
    private int lookCount;

    public void OnEnter(Enemy enemy)
    {
        timer = 0f;
        hasReachedPoint = false;
        lookCount = 0;
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        if (!hasReachedPoint)
        {
            if (Mathf.Abs(enemy.transform.position.x - enemy.investigateTarget.x) > 0.5f)
            {
                enemy.MoveToPoint(enemy.investigateTarget);
            }
            else
            {
                hasReachedPoint = true;
                enemy.StopMoving();
                timer = 0f;
                lookCount = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > 1f)
            {
                enemy.EnemyRotation(!enemy.isRight);
                lookCount++;
                timer = 0f;

                if (lookCount >= 2)
                {
                    enemy.ChangeState(new IdleState());
                }
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
    }
}