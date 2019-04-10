using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public float switchTime;
    int sizeOfSprites;
    SpriteRenderer render;
    void Start()
    {
        sizeOfSprites = sprites.Length;
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int x = (int)(Time.time / switchTime) % sizeOfSprites;
        render.sprite = sprites[x];
    }
}
