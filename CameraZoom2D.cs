using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom2D : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;
    private void Update()
    {
        
    }
    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }
}
