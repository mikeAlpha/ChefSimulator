using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    protected bool IsAvailable = true;
    protected SpriteRenderer spriteRender;
    protected float TimeForAction;
    protected float elapsedTime = 0;
    protected float percentage = 0;
    Color startAlpha;

    public virtual void Start()
    {
        //Do no do anything
    }

    public virtual void OnEnable()
    {
        if(transform.childCount != 0)
            spriteRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public virtual void CallForAction(float time)
    {
        TimeForAction = time;
        elapsedTime = TimeForAction;

        if(spriteRender!=null)
            startAlpha = spriteRender.color;
        
        IsAvailable = false;
    }

    public virtual void Update()
    {
        if (!IsAvailable)
        {
            elapsedTime -= Time.deltaTime;
            if (spriteRender != null) {
                var ratio = (elapsedTime / TimeForAction);
                percentage = ratio * 100f;
                float newAlpha = Mathf.Lerp(0f, 1f, ratio);

                spriteRender.color = new Color(startAlpha.r, startAlpha.g, startAlpha.b, newAlpha);
            }
            if (elapsedTime <= 0)
            {
                elapsedTime = 0f;
                IsAvailable = true;
            }
        }
    }
}
