using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    [SerializeField] private Transform m_StartClimbPoint;
    [SerializeField] private ClimbDirection m_TriggerSide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var temp = collision.GetComponent<Player>();

        if (temp != null)
        {
            temp.LastStair = this;
            temp.CanClimb = true;

            if (temp.ClimbNow)
            {
                temp.ClimbNow = false;
                temp.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Physics2D.IgnoreLayerCollision((int)Layers.Player, (int)Layers.Floor, false);
                temp.ActivateStayAnimation();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var temp = collision.GetComponent<Player>();

        if (temp != null)
        {
            temp.CanClimb = false;
        }
    }

    public ClimbDirection TriggerSide { get => m_TriggerSide; set => m_TriggerSide = value; }
    public Transform StartClimbPoint { get => m_StartClimbPoint; set => m_StartClimbPoint = value; }
}

public enum ClimbDirection
{
    top, down, none
}

