using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        //transform.position = new Vector3(transform.position.x, character.transform.position.y, character.transform.position.z)+ offset;
    }
}
