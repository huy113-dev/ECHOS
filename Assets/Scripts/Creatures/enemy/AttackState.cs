using UnityEngine;

public class AttackState : IState
{
    private float timer;
    public void OnEnter(Enemy enemy)
    {
        if (enemy.Target != null)
        {
            enemy.EnemyRotation(enemy.Target.transform.position.x > enemy.transform.position.x);
            enemy.StopMoving();
            enemy.EAttack();
            timer = 0f;
        }
    }
    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            enemy.ChangeState(new PatrolState());
        }

    }
    public void OnExit(Enemy enemy)
    {

    }
}
