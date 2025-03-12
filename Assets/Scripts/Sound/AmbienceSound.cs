using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    public Collider Area;
    public GameObject Player;
    public string soundName;

    void Start()
    {
        AudioManager.Instance.Ambience_PlayAttached(soundName, this.transform);
    }

    void Update()
    {
        // Locate closest point on the collider to the player
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);

        // Set position to closest point to the player
        transform.position = closestPoint;
    }
}
