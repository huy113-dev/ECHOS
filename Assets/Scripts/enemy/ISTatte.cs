public interface IState
{
    void OnEnter(Enemy enemy);    // Chạy 1 lần khi bắt đầu vào trạng thái
    void OnExecute(Enemy enemy);  // Chạy liên tục mỗi frame (giống Update)
    void OnExit(Enemy enemy);     // Chạy 1 lần khi thoát khỏi trạng thái
}