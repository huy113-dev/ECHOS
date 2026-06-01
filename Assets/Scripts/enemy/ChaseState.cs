using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.animator.Play("monster run");
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // Nếu sát lại gần người chơi -> Đánh
        if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(new AttackState());
            return;
        }
        // Nếu người chơi chạy thoát khỏi tầm nhìn -> Quay về đứng im (bỏ cuộc)
        else if (distance > enemy.sightRange)
        {
            enemy.ChangeState(new IdleState());
            return;
        }

        // Xoay mặt về phía người chơi
        float directionX = enemy.player.position.x - enemy.transform.position.x;
        if (directionX > 0 && !enemy.facingRight) enemy.Flip();
        else if (directionX < 0 && enemy.facingRight) enemy.Flip();

        // Rượt theo
        Vector3 targetPos = new Vector3(enemy.player.position.x, enemy.transform.position.y, enemy.transform.position.z);
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPos, enemy.runSpeed * Time.deltaTime);
    }

    public void OnExit(Enemy enemy) { }
}
