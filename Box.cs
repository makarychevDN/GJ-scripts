using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D m_RB;

    [Header("Functional Points")]
    [SerializeField] private Transform m_GroundChecker;

    [Header("Sounds")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_DragSound;
    [SerializeField] private AudioClip m_LandingSound;

    [SerializeField]private bool m_IsGrounded;
    private bool m_WasGroundedBefore;

    private const float m_CheckGroundRange = 0.05f;
    private const int m_XorCheckGroundLayers = 1 << (int)Layers.Default | 1 << (int)Layers.Floor | 1 << (int)Layers.Boxes;

    // Start is called before the first frame update
    private void Update()
    {
        CheckGround();

        if (!m_AudioSource.isPlaying && m_IsGrounded && m_RB.velocity.magnitude != 0)
        {
            m_AudioSource.clip = m_DragSound;
            m_AudioSource.Play();
        }
        else if(m_RB.velocity.magnitude == 0 | !m_IsGrounded)
        {
            m_AudioSource.Stop();
        }
    }

    public void CheckGround()
    {
        m_IsGrounded = Physics2D.BoxCast(m_GroundChecker.position, new Vector2(1f, 0.1f), 0, Vector2.down, m_CheckGroundRange, m_XorCheckGroundLayers);

        if (!m_WasGroundedBefore && m_IsGrounded)
        {
            m_AudioSource.clip = m_LandingSound;
            m_AudioSource.Play();
        }

        m_WasGroundedBefore = m_IsGrounded;
    }
}
