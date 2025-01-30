using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    private Transform player;
    private SpriteRenderer SR;
    private SpriteRenderer playerSR;
    private Color color;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
        transform.localScale = player.localScale;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        //color = new Color(1f, 1f, 1f, alpha);
        color = new Color(1f, 0.5f, 0.2f, alpha);
        SR.color = color;
        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
