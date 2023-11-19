using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    protected bool IsAvailable = true;

    float TimeForAction;
    float elapsedTime = 0;

    public virtual void OnEnable()
    {
        //Don't do anything
    }

    public virtual void CallForAction(float time)
    {
        TimeForAction = time;
        IsAvailable = false;
    }

    public virtual void Update()
    {
        if (!IsAvailable)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > TimeForAction)
            {
                elapsedTime = 0f;
                IsAvailable = true;
            }
        }
    }
}
