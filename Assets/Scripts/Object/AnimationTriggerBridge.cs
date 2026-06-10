using UnityEngine;

public class AnimationTriggerBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Gateway gateway;

    private void EnableGateWayCollider()
    {
        gateway.EnableDoorCollider();
    }
}
