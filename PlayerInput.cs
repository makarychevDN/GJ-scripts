using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player m_Player;
    [SerializeField] private float m_ReturnProjectileTime;

    [SerializeField] private RestartManager m_RestartMangager;
    [SerializeField] private float m_RestartTime;

    private float m_ProjectileTimer;
    private float m_RestartTimer;

    public void Reset()
    {
        m_Player = FindObjectOfType<Player>();
    }

    void LateUpdate()
    {
        m_Player.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Player.ThrowTheBlackHoleGenerator();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            m_ProjectileTimer += Time.deltaTime;
        }
        else
        {
            m_ProjectileTimer = 0;
        }

        if (m_ProjectileTimer > m_ReturnProjectileTime)
            m_Player.ReturnBlackHoleGenerator();

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Player.ActivateBlackHole();
        }

        if (Input.GetKey(KeyCode.R))
        {
            m_RestartTimer += Time.deltaTime;
        }
        else
        {
            m_RestartTimer = 0;
        }

        if (m_RestartTimer > m_RestartTime)
        {            
            m_RestartMangager.Restart();
            m_RestartTimer = 0;

        }

    }


}
