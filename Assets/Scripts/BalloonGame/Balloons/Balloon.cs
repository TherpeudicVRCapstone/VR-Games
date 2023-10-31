/**
 * The Balloon class represent a generic balloon. A generic balloon is a balloon that is worth 
 * one point. In the normal and endless modes, it is the only balloon that results in loss 
 * of lives.
 */

using System.Collections;
using System.Collections.Generic;
using Classes.Managers;
using UnityEngine;

public class Balloon : _BaseBalloon
{
    private void Update()
    {
        Transform transform = gameObject.transform;
        transform.position = Vector3.Lerp(transform.position, transform.position 
                                        + new Vector3(0f, 1f, 0f), Time.deltaTime * floatStrength);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DartPoint") && this.IsCorrectDart(other.gameObject.transform.parent.gameObject))
        {
            this.PlayEffects();
            this.AddPoints();
            BalloonManager.Instance.KillBalloon(gameObject);
            DartManager.Instance.DestroyDart(other.gameObject.transform.parent.gameObject);
        }
    }

    public virtual void PlayEffects()
    {
        this.PlaySound();
        this.PlayParticles();
    }

    private void PlaySound()
    {
        GameObject audioSource = this.transform.Find("AudioSource").gameObject;
        /* Decouple the child object from the parent to avoid destroying the parent (along with
           the child audio object) before the audio is done playing. */
        audioSource.transform.parent = null;
        audioSource.GetComponent<AudioSource>().Play();
        /* Then, make sure to destroy the audio object, but with a delay. */
        Destroy(audioSource, audioSource.GetComponent<AudioSource>().clip.length);
    }

    private void PlayParticles()
    {
        /* Same reasoning as in the PlaySound method. */
        GameObject particleEffect = this.transform.Find("ParticleEffects").gameObject;
        particleEffect.transform.parent = null;
        particleEffect.GetComponent<ParticleSystem>().Play();
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.duration);
    }

    /**
     * The AddPoints method adds the point value of the balloon to the total points and to the left 
     * or right points depending on where the balloon was spawned.
     */
    protected void AddPoints()
    {
        if (this.spawnLoc.CompareTag("BalloonSpawn_Left")) {
            PointsManager.addLeftPoints(this.pointValue);
        } else {
            PointsManager.addRightPoints(this.pointValue);
        }
        PointsManager.addPoints(this.pointValue);

        Debug.Log(  "Left points: " + PointsManager.getLeftPoints() 
                  + ". Right points: " + PointsManager.getRightPoints() 
                  + ". Total points: " + PointsManager.getPoints() + ".");
    }

    /**
     * The IsCorrectDart method returns true or false depending on whether the balloon and the dart 
     * are on the same side. 
     */
    protected bool IsCorrectDart(GameObject dart)
    {
        return 
        
            (this.spawnLoc.CompareTag("BalloonSpawn_Left")  && DartManager.Instance.IsLeftDart(dart)) 
         || (this.spawnLoc.CompareTag("BalloonSpawn_Right") && DartManager.Instance.IsRightDart(dart));
    }

    /* For testing purposes. Useful for testing on the computer rather than in the headset. */
    public virtual void OnMouseDown()
    {
        Debug.Log(this.ToString() + " popped. Worth " + this.pointValue + " points.");
        
        this.PlayEffects();
        this.AddPoints();

        BalloonManager.Instance.KillBalloon(gameObject);
    }
}
