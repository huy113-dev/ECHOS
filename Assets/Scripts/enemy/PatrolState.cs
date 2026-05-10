using Unity.VisualScripting;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.animator.Play("monster walk");
    }

    public void OnExecute(Enemy enemy)
    {
        // Vừa đi vừa quét tìm người chơi
        if (enemy.player != null && Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.sightRange)
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Xác định điểm đến
        Vector3 destination = enemy.movingRight ? enemy.targetPosition : enemy.startPosition;

        // Xoay mặt nếu cần
        float directionX = destination.x - enemy.transform.position.x;
        if (directionX > 0 && !enemy.facingRight) enemy.Flip();
        else if (directionX < 0 && enemy.facingRight) enemy.Flip();

        // Di chuyển
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(destination.x, enemy.transform.position.y, enemy.transform.position.z), enemy.walkSpeed * Time.deltaTime);

        // Nếu đi đến nơi -> Chuyển về đứng im
        if (Mathf.Abs(enemy.transform.position.x - destination.x) < 0.1f)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy) { }
}
