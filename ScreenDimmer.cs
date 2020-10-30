using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDimmer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_ScreenDimmer;
    [SerializeField] private float m_DimTimer;

    private bool isDimmingNow;
    private bool isUnDimmingNow;
    private float m_Step;
    private float m_CurrentAlpha;

    private void Start()
    {
        m_Step = 1 / m_DimTimer;
        m_CurrentAlpha = 0;
    }

    public void StartDimming(float delay)
    {
        Invoke("StartDimming", delay);
    }

    public void StartDimming()
    {
        isDimmingNow = true;
        isUnDimmingNow = false;
    }

    private void FixedUpdate()
    {

        if (isDimmingNow)
        {
            m_CurrentAlpha += m_Step * Time.fixedDeltaTime;
        }

        if (isUnDimmingNow)
        {
            m_CurrentAlpha -= m_Step * Time.fixedDeltaTime;
        }

        if (m_CurrentAlpha > 0.99f)
        {
            isUnDimmingNow = true;
            isDimmingNow = false;
        }

        m_CurrentAlpha = Mathf.Clamp(m_CurrentAlpha, 0, 1);
        m_ScreenDimmer.color = new Color(0, 0, 0, m_CurrentAlpha);
    }
}
