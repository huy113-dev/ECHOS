//using Cinemachine;
//using UnityEngine;

//public class LockCameraY : CinemachineExtension
//{
//    [Tooltip("Tọa độ Y bạn muốn khóa chết")]
//    public float yPosition = 0f;

//    protected override void PostPipelineStageCallback(
//        CinemachineVirtualCameraBase vcam,
//        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
//    {
//        if (stage == CinemachineCore.Stage.Body)
//        {
//            Vector3 pos = state.RawPosition;
//            pos.y = yPosition;
//            state.RawPosition = pos;
//        }
//    }
//}