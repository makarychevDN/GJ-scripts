using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartReplacer : MonoBehaviour
{
    [SerializeField] private Vector2 m_RestartPosition;

    public Vector2 RestartPosition { get => m_RestartPosition; set => m_RestartPosition = value; }

    public void Restart()
    {
        transform.position = m_RestartPosition;

        if(GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void Reset()
    {
        m_RestartPosition = transform.position;
    }

}
