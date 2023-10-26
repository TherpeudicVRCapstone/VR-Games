using System.Collections;
using System.Collections.Generic;
using Classes.Managers;
using UnityEngine;

public class BalloonKillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /* Note that what I am doing here is slightly dangerous. I am assuming the other game 
           object is a balloon. This should not be a problem since where the collider is positioned,
           the only objects that should be colliding with the kill zone are balloons.*/
        Debug.Log(other + " collided with the kill zone.");

        switch(other.gameObject.tag) {
            case "Balloon_General":
                --BalloonGameplayManager.Instance.playerLives;
                BalloonManager.Instance.KillBalloon(other.gameObject);
                Debug.Log("Lost a life. Remaining lives: " + BalloonGameplayManager.Instance.playerLives);
                break;
            case "Balloon_OnionLayer1":
            case "Balloon_OnionLayer2":
            case "Balloon_OnionLayer3":
                BalloonManager.Instance.KillBalloon(other.gameObject.transform.parent.gameObject);
                break;
            default:
                Debug.Log("No tag matches.");
                BalloonManager.Instance.KillBalloon(other.gameObject);
                break;
        }
    }
}
