using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;

    Stack<Node> path;

    List<Debuffs> debuffs = new List<Debuffs>();
    List<Debuffs> debuffsToRemove = new List<Debuffs>();
    List<Debuffs> newDebuffs = new List<Debuffs>();

    Animator anim;
    SpriteRenderer sr;

    [SerializeField] Slider healthSlider;

    [SerializeField] Element elementType;

    int invulnerability = 2;

    public Point GridPosition { get; set; }

    Vector3 destination;

    public bool IsActive { get; set; }
    
    public bool IsAlive
    {
        get { return healthSlider.value > 0; }
    }

    public Element ElementType
    {
        get { return elementType; }
    }

    private void Update()
    {
        HandleDebuffs();
        Move();
    }

    public void Spawn(int health)
    {
        debuffs.Clear();
        transform.position = LevelManager.Instance.BluePortal.transform.position;
        this.healthSlider.maxValue = health;
        this.healthSlider.value = health;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1f, 1f), false));

        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        float progress = 0;
        while(progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.localScale = to;
        IsActive = true;
        if(remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if(IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime); 

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);

                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }
        
    }

    void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath;
            Animate(GridPosition, path.Peek().GridPosition);
            //Sets new Grid Pos
            GridPosition = path.Peek().GridPosition;
            //Sets new Des
            destination = path.Pop().WorldPosition;
        }
    }

    void Animate(Point currentPos, Point newPos)
    {
        if(currentPos.Y > newPos.Y) //Move Down
        {
            anim.SetInteger("Horizontal", 0);
            anim.SetInteger("Vertical", 1);
        }
        else if(currentPos.Y < newPos.Y) //Move up
        {
            anim.SetInteger("Horizontal", 0);
            anim.SetInteger("Vertical", -1);
        }

        if (currentPos.Y == newPos.Y)
        {
            if (currentPos.X > newPos.X) //Move left
            {
                anim.SetInteger("Horizontal", 1);
                anim.SetInteger("Vertical", 0);
                sr.flipX = false;
            }
            else if (currentPos.X < newPos.X) //Move right
            {
                anim.SetInteger("Horizontal", 1);
                anim.SetInteger("Vertical", 0);
                sr.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "RedPortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));

            GameManager.Instance.Lives--;
        }
    }

    void Release()
    {
        IsActive = false;
        GridPosition = LevelManager.Instance.BlueSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);
    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if(IsActive)
        { 
            if(dmgSource == elementType)
            {
                damage = damage / invulnerability;
                invulnerability++;
            }
            healthSlider.value -= damage;
            if(healthSlider.value <= 0)
            {
                GameManager.Instance.Currency += 2;
                Release();
            }
        }
    }

    public void AddDebuff(Debuffs debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            newDebuffs.Add(debuff);
        }
    }
    public void RemoveDebuff(Debuffs debuff)
    {
        debuffsToRemove.Add(debuff);
    }
    void HandleDebuffs()
    {
        if(newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        
        }
        foreach(Debuffs debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
        }
        debuffsToRemove.Clear();

        foreach (Debuffs debuff in debuffs)
        {
            debuff.Update();
        }
    }
}
