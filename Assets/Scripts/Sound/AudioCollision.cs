using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollisionType
{
    public string tagName;
    public string[] sfxName;
}


public class AudioCollision : MonoBehaviour
{
    public CollisionType[] collisionCollection;

    void OnCollisionEnter(Collision collision)
    {   
        CollisionType collisionObject = System.Array.Find(collisionCollection, x => x.tagName == collision.gameObject.tag);
        
        if(collisionObject != null && collision.gameObject.tag == collisionObject.tagName)
        {   
            // Select a random song, if there more than one
            int index = Random.Range(0, collisionObject.sfxName.Length);
            string soundName = collisionObject.sfxName[index];

            // Play the audio
            AudioManager.Instance.SFX_PlayAtSource(soundName, this.transform.position);
        }
    }
}
