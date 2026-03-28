using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorsManager : MonoBehaviour
{
    public static ActorsManager Instance { get; private set; }

    List<Actor> actors;

    public static event Action<Actor> OnActorAdded;
    public static event Action<Actor> OnActorRemoved;

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
        OnActorAdded?.Invoke(actorToAdd);
    }

    public static void RemoveActor(Actor actorToRemove)
    {
        Instance.actors.Remove(actorToRemove);
        OnActorRemoved?.Invoke(actorToRemove);
    }

    public static List<Actor> GetActors()
    {
        return Instance.actors;
    }
}
