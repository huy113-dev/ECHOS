using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IState
{
    private float attackTimer;

    public void OnEnter(Enemy enemy)
    {
        enemy.animator.Play("monster attack");
        attackTimer = 1.5f; // Thời gian vung đòn và hồi chiêu là 1.5 giây

        Debug.Log("Quái đang tấn công!");
        // Gọi hàm gây sát thương ở đây sau
    }

    public void OnExecute(Enemy enemy)
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            // Kiểm tra xem đánh xong người chơi còn đứng đó không
            if (enemy.player != null && Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.attackRange)
            {
                enemy.ChangeState(new AttackState()); // Vẫn ở đó thì đánh tiếp
            }
            else
            {
                enemy.ChangeState(new IdleState()); // Đã chạy ra xa thì chuyển về đứng thở
            }
        }
    }

    public void OnExit(Enemy enemy) { }
}