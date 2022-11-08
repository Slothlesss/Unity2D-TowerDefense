using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element {Fire, Nature, Ice, Electric}

public abstract class Tower : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string projectileType;

    [SerializeField] float projectileSpeed;

    [SerializeField] float range;
    [SerializeField] int damage;

    [SerializeField]
    float debuffDuration;

    [SerializeField]
    float proc;

    

    public Element ElementType { get; protected set; }

    public int Price { get; set; }

    public int Damage
    {
        get { return damage; }
    }
    public float ProjecttileSpeed
    {
        get { return projectileSpeed; }
    }
    public float DebuffDuration
    {
        get { return debuffDuration; }
        set
        {
            this.debuffDuration = value;
        }
    }

    public float Proc
    {
        get { return proc; }
    }
    SpriteRenderer sr;
    SpriteRenderer parentSr;
    Monster target;

    public int Level { get; protected set; }

    public Monster Target
    {
        get { return target; }
    }
    Queue<Monster> monsters = new Queue<Monster>();

    bool canAttack = true;
    float attackTimer;

    [SerializeField]
    float attackCooldown;

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public TowerUpgrade[] Upgrades { get; protected set; }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if(Upgrades.Length > Level - 1)
            {
                return Upgrades[Level - 1];
            }
            return null;
        }
    }
    private void Awake()
    {
        Level = 1;
        sr = GetComponent<SpriteRenderer>();
        parentSr = transform.parent.GetComponent<SpriteRenderer>();
        range = GetComponent<CircleCollider2D>().radius;
    }
    private void Start()
    {
    }
    private void Update()
    {
        Attack();
    }
    public void Select()
    {
        sr.enabled = !sr.enabled;
    }
    void Attack()
    {
        if(target == null && monsters.Count > 0 && monsters.Peek().IsActive)
        {
            target = monsters.Dequeue();
        }
        if(target != null && target.IsActive)
        {
            if(!canAttack)
            {
                attackTimer += Time.deltaTime;
                if(attackTimer >= attackCooldown)
                {
                    canAttack = true;
                    attackTimer = 0;
                }
            }
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
        if(target != null && !target.IsAlive || target != null && !target.IsActive)
        {
            target = null;
        }
    }

    void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    public virtual void Upgrade()
    {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price; //Update price of towers
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.attackCooldown += NextUpgrade.AttackCoolDown;

        this.parentSr.sprite = NextUpgrade.sprite;
        this.parentSr.material = NextUpgrade.material;
        Level++;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    public abstract Debuffs GetDebuff();
    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            target = null;
        }
    }
}
