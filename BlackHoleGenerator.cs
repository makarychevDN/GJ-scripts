using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGenerator : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private float m_ThrowSpeed;
    [SerializeField] private float m_TimeBeforeTurnOnGravity;
    [SerializeField] private float m_MovementDelta;

    [Header("Black Hole Setup")]
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_GravitySpeed;

    [Header("Functional Points")]
    [SerializeField] private Transform m_BlackHoleCenter;

    [Header("Animations")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AnimationClip m_Idle;
    [SerializeField] private AnimationClip m_Open;
    [SerializeField] private AnimationClip m_Loop;
    [SerializeField] private AnimationClip m_Close;

    [Header("Sounds")]
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_ThrowSound;
    [SerializeField] private AudioClip m_LandingSound;
    [SerializeField] private AudioClip m_OpenSound;
    [SerializeField] private AudioClip m_ACtiveSound;
    [SerializeField] private AudioClip m_CloseSound;

    private bool m_isPicked = true;
    private bool m_IsBlackHoleActiveNow;

    private float m_PlaytActiveAnimationTimer = 0.4f;
    private float m_PlayIdleAnimationTimer = 0.2f;
    private float m_PlayOpenSoundTimer = 0.65f;


    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_IsBlackHoleActiveNow)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.clip = m_ACtiveSound;
                m_AudioSource.Play();
            }

            Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(transform.position, 4);

            foreach (var i in collidersInRadius)
            {
                var tempRigidBody = i.GetComponent<Rigidbody2D>();
                var tempPlayer = i.GetComponent<Player>();

                if (tempRigidBody != null)
                {
                    if(tempPlayer != null)
                    {
                        if (!tempPlayer.IsGrounded && !tempPlayer.ClimbNow)
                        {
                            tempRigidBody.velocity = (m_BlackHoleCenter.position - i.transform.position).normalized * m_GravitySpeed;
                        }
                    }

                    else
                    {
                        if(tempRigidBody != m_RB)
                            tempRigidBody.velocity = (m_BlackHoleCenter.position - i.transform.position).normalized * m_GravitySpeed;
                    }
                }
            }
        }
    }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision((int)Layers.BlackHoleGenerator, (int)Layers.Player, true);
    }

    public void Throw(Transform thrower, Vector2 throwersVelocity)
    {
        CancelInvoke();

        m_AudioSource.clip = m_ThrowSound;
        m_AudioSource.Play();

        m_isPicked = false;
        m_RB.gravityScale = 0;
        m_RB.velocity = thrower.transform.right * m_ThrowSpeed + (Vector3)throwersVelocity;
        Physics2D.IgnoreLayerCollision((int)Layers.BlackHoleGenerator, (int)Layers.Boxes, false);
        Invoke("StopFly", m_TimeBeforeTurnOnGravity);
    }

    public void StopFly()
    {
        m_RB.velocity = Vector2.zero;
        m_RB.gravityScale = 1;
        Physics2D.IgnoreLayerCollision((int)Layers.BlackHoleGenerator, (int)Layers.Boxes, true);
    }

    public void ReturnToPlayer()
    {
        m_isPicked = true;
        m_IsBlackHoleActiveNow = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == (int)Layers.Boxes)
        {
            ReturnToPlayer();
        }

        if (collision.gameObject.layer == (int)Layers.Floor)
        {
            m_AudioSource.clip = m_LandingSound;
            m_AudioSource.Play();
        }
    }

    public bool IsPicked { get => m_isPicked; set => m_isPicked = value; }
    public bool IsBlackHoleActiveNow
    {
        get => m_IsBlackHoleActiveNow;
        set 
        { 
            if(value == true)
            {
                if(m_RB.velocity.magnitude < m_MovementDelta)
                {
                    m_IsBlackHoleActiveNow = true;
                    m_Animator.Play(m_Open.name);

                    m_AudioSource.clip = m_OpenSound;
                    m_AudioSource.Play();

                    Invoke("StopPlayOpenSound", m_PlayOpenSoundTimer);
                    Invoke("PlayActiveAnimation", m_PlaytActiveAnimationTimer);
                    return;
                }
            }

            m_IsBlackHoleActiveNow = false;

            m_AudioSource.clip = m_CloseSound;
            m_AudioSource.Play();

            Invoke("PlayIdleAnimation", m_PlayIdleAnimationTimer);
            m_Animator.Play(m_Close.name);
        }
    }

    public void StopPlayOpenSound()
    {
        m_AudioSource.clip = m_ACtiveSound;
        m_AudioSource.Play();
    }

    public void PlayIdleAnimation()
    {
        m_Animator.Play(m_Idle.name);
    }

    public void PlayActiveAnimation()
    {
        m_Animator.Play(m_Loop.name);
    }
}
