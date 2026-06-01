using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IState
{
    private float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.animator.Play("monster idle");
        timer = 2f; // Thời gian đứng im thở là 2 giây
    }

    public void OnExecute(Enemy enemy)
    {
        // Nếu thấy người chơi trong tầm nhìn -> Đổi sang rượt đuổi
        if (enemy.player != null && Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.sightRange)
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Đếm ngược thời gian
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemy.movingRight = !enemy.movingRight; // Chuẩn bị đổi hướng
            enemy.ChangeState(new PatrolState());   // Đổi sang đi tuần
        }
    }

    public void OnExit(Enemy enemy) { }
}