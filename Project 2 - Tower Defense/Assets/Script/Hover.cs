using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    SpriteRenderer sr;

    SpriteRenderer rangeSr;
    // Start is called before the first frame update
    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.rangeSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        if(sr.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public void Activate(Sprite sprite)
    {
        this.sr.sprite = sprite;
        sr.enabled = true;
        rangeSr.enabled = true;
    }
    public void Deactivate()
    {
        sr.enabled = false;
        GameManager.Instance.ClickedBtn = null;
        rangeSr.enabled = false;
    }
}
