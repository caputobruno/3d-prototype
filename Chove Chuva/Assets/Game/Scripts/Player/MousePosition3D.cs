using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;

    PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.position = GetMousePosition();
    }

    Vector3 GetMousePosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
            return _player.transform.TransformDirection(Vector3.forward) * 100;

    }
}
