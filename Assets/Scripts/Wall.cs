using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private int hp = 2;
    public Sprite damageSprite;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

    public void TakeDamage() {

        hp -= 1;
        GetComponent<SpriteRenderer>().sprite = damageSprite;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        if (hp == 0)
        {
            Destroy(this.gameObject);
        }

    }
}
