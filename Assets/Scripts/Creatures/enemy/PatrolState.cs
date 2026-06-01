using UnityEngine;

public class PatrolState : IState
{
    private float timer, randomTimer;

    public void OnEnter(Enemy enemy)
    {
        timer = 0f;
        randomTimer = Random.Range(5f, 7f);
    }
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.Target != null)
        {
            enemy.EnemyRotation(enemy.Target.transform.position.x > enemy.transform.position.x);
            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Chasing();
            }

        }
        else
        {
            if (timer <= randomTimer)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }

    }
    public void OnExit(Enemy enemy)
    {

    }
}
