using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_ClimbSpeed;

    [Header("Projectiles")]
    [SerializeField] private BlackHoleGenerator m_BlackHoleGenerator;
    [SerializeField] private float m_MovementDelta;

    [Header("Functional Points")]
    [SerializeField] private Transform m_GroundChecker;
    [SerializeField] private Transform m_BHGeneratorSpawnPoint;

    [Header("Animations")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AnimationClip m_IdleAnim;
    [SerializeField] private AnimationClip m_RunAnim;
    [SerializeField] private AnimationClip m_ClimbAnim;
    [SerializeField] private AnimationClip m_StayAtStairsAnim;
    [SerializeField] private AnimationClip m_FallAnim;

    [Header("Sounds")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_ClimbSound;
    [SerializeField] private AudioClip m_RunSound;
    [SerializeField] private AudioClip m_FallSound;

    private StairTrigger m_LastStair;
    private ClimbDirection m_CurentClimbDirection;

    private bool m_CanClimb;
    private bool m_ClimbNow;
    private bool m_IsGrounded;
    private bool m_WasGroundedBefore;

    private const float m_CheckGroundRange = 0.05f;
    private const int m_XorCheckGroundLayers = 1 << (int)Layers.Default | 1 << (int)Layers.Floor | 1 << (int)Layers.Boxes;



    public void Move(float Xvalue, float Yvalue)
    {
        if (m_IsGrounded)
        {
            if(Xvalue > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Xvalue < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (Yvalue > 0)
            m_CurentClimbDirection = ClimbDirection.top;
        else
            m_CurentClimbDirection = ClimbDirection.down;


        if (m_ClimbNow)
        {
            m_RB.velocity = new Vector2(0, Yvalue * m_ClimbSpeed);
            m_CanClimb = false;

            if (Yvalue != 0)
            {
                m_Animator.Play(m_ClimbAnim.name);
                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.clip = m_ClimbSound;
                    m_AudioSource.Play();
                }
            }

            else
                m_Animator.Play(m_StayAtStairsAnim.name);
        }

        else if (m_CanClimb && Yvalue != 0)
        {
            if(m_CurentClimbDirection != m_LastStair.TriggerSide)
            {
                m_RB.velocity = Vector2.zero;
                Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Floor, true);
                ClimbNow = true;
                transform.position = m_LastStair.StartClimbPoint.position;
            }
            else
            {
                ClimbNow = false;
            }
        }

        else
        {
            if (m_IsGrounded && Xvalue != 0)
            {
                m_Animator.Play(m_RunAnim.name);
                m_RB.velocity = new Vector2(Xvalue * m_MoveSpeed, m_RB.velocity.y);

                if (!m_AudioSource.isPlaying)
                {
                    m_AudioSource.clip = m_RunSound;
                    m_AudioSource.Play();
                }

            }
            else if (!m_IsGrounded)
            {
                m_Animator.Play(m_FallAnim.name);
            }
            else
            {
                m_Animator.Play(m_IdleAnim.name);
            }
        }
    }

    public void ActivateStayAnimation()
    {
        m_Animator.Play(m_IdleAnim.name);
    }

    public void ReturnBlackHoleGenerator()
    {
        m_BlackHoleGenerator.ReturnToPlayer();
    }

    public void ActivateBlackHole()
    {
        if(!m_BlackHoleGenerator.IsPicked)
            m_BlackHoleGenerator.IsBlackHoleActiveNow = !m_BlackHoleGenerator.IsBlackHoleActiveNow;
    }

    public void ThrowTheBlackHoleGenerator()
    {
        if(BlackHoleGenerator.IsPicked)
        {
            if (m_IsGrounded && m_RB.velocity.x < m_MovementDelta || !m_IsGrounded)
            {
                if (!m_ClimbNow)
                {
                    m_BlackHoleGenerator.gameObject.SetActive(true);
                    m_BlackHoleGenerator.transform.position = m_BHGeneratorSpawnPoint.position;
                    m_BlackHoleGenerator.Throw(transform, m_RB.velocity);
                }
            }
        }
    }

    private void Update()
    {
        CheckGround();
    }

    public void CheckGround()
    {
        m_IsGrounded = Physics2D.BoxCast(m_GroundChecker.position, new Vector2(1f, 0.5f), 0, Vector2.down, m_CheckGroundRange, m_XorCheckGroundLayers);

        if(!m_WasGroundedBefore && m_IsGrounded && !m_ClimbNow)
        {
            m_AudioSource.clip = m_FallSound;
            m_AudioSource.Play();
        }

        m_WasGroundedBefore = m_IsGrounded;
    }

    public void Reset()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    public bool CanClimb { get => m_CanClimb; set => m_CanClimb = value; }
    public StairTrigger LastStair { get => m_LastStair; set => m_LastStair = value; }
    public bool ClimbNow { get => m_ClimbNow; set => m_ClimbNow = value; }
    public BlackHoleGenerator BlackHoleGenerator { get => m_BlackHoleGenerator; set => m_BlackHoleGenerator = value; }
    public bool IsGrounded { get => m_IsGrounded; set => m_IsGrounded = value; }
    public bool ClimbNow1 { get => m_ClimbNow; set => m_ClimbNow = value; }
}
