using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public static ActorsManager Instance { get; private set; }

    List<Actor> actors;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        actors = new List<Actor>(25);
    }

    public static void AddActor(Actor actorToAdd)
    {
        Instance.actors.Add(actorToAdd);
    }

    public static void RemoveActor(Actor actorToRemove)
    {
        Instance.actors.Remove(actorToRemove);
    }

    public static List<Actor> GetActors()
    {
        return Instance.actors;
    }
}
