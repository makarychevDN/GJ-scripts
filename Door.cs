using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform m_NextLevelRespawn;
    [SerializeField] private float m_TimerToNextLevel;
    [SerializeField] private float m_DimmingStartTimer;
    [SerializeField] private GameObject m_SomePhrase;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tempPlayer = collision.GetComponent<Player>();

        if (tempPlayer != null)
        {
            tempPlayer.ReturnBlackHoleGenerator();
            tempPlayer.GetComponent<RestartReplacer>().RestartPosition = m_NextLevelRespawn.position;

            if (m_SomePhrase != null)
                m_SomePhrase.SetActive(true);


            FindObjectOfType<ScreenDimmer>().StartDimming(m_DimmingStartTimer);
            Invoke("GoToTheNextLevel", m_TimerToNextLevel);
        }
    }

    public void GoToTheNextLevel()
    {
        FindObjectOfType<RestartManager>().Restart();
    }
}
