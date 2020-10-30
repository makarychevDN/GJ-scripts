using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform m_ExitPoint;
    [SerializeField] private Teleporter m_OppositeTeleporter;
    [SerializeField] private AudioSource m_AudioSource;

    private Collider2D[] m_LastInteractedCollider;
    private float m_StopIgnoreCollisionTime = 0.25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var temp = collision.GetComponent<Rigidbody2D>();

        if (temp != null)
        {
            m_LastInteractedCollider = collision.gameObject.GetComponents<Collider2D>();

            foreach (var i in m_LastInteractedCollider)
            {
                Physics2D.IgnoreCollision(m_OppositeTeleporter.GetComponent<Collider2D>(), i, true);
            }

            if (temp.GetComponent<Player>() != null)
            {
                temp.GetComponent<Player>().Move(0, 0);
            }

            m_AudioSource.Play();

            var tempVelocity = temp.velocity.magnitude;
            temp.transform.position = m_OppositeTeleporter.ExitPoint.position;
            temp.velocity = m_OppositeTeleporter.ExitPoint.right * tempVelocity;
            Invoke("StopIgnoreCollision", m_StopIgnoreCollisionTime);
        }
    }

    private void StopIgnoreCollision()
    {
        foreach (var i in m_LastInteractedCollider)
        {
            Physics2D.IgnoreCollision(m_OppositeTeleporter.GetComponent<Collider2D>(), i, false);
        }
    }

    public Transform ExitPoint { get => m_ExitPoint; set => m_ExitPoint = value; }
}
