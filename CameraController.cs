using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    private void Update()
    {
        transform.position = new Vector3(m_Target.position.x, m_Target.position.y, -10);
    }
}
