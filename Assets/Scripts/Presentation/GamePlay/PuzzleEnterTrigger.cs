using Domain.Constants;
using Presentation.GamePlay.Balls.Interface;
using UnityEngine;

namespace Presentation.GamePlay
{
    public class PuzzleEnterTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TagNames.BALL))
            {
                IBall ball = other.GetComponent<IBall>();
                if (ball != null)
                {
                    ball.EnterPuzzle();
                }
            }
        }
    }
}