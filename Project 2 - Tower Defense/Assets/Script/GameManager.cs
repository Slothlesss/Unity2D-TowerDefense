using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public delegate void CurrencyChanged();

public class GameManager : Singleton<GameManager>
{
    public event CurrencyChanged Changed;
    public TowerBtn ClickedBtn { get; set ;}

    int currency;
    int wave = 0;
    int lives;
    int health = 15;

    bool gameOver = false;

    [SerializeField]
    TextMeshProUGUI waveText;
    
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI livesText;

    [SerializeField]
    GameObject waveBtn;

    [SerializeField] GameObject gameOverMenu;

    [SerializeField] GameObject upgradePanal;

    [SerializeField] TextMeshProUGUI sellPriceText;
    [SerializeField] TextMeshProUGUI upgradePriceText;
    Tower selectedTower;

    List<Monster> activeMonster = new List<Monster>();


    public ObjectPool Pool { get; set; }

    public bool WaveActive
    {
        get
        {
            return activeMonster.Count > 0;
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            this.currencyText.text = value.ToString() + "$";
            OnCurrencyChanged();
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            this.lives = value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
            this.livesText.text = lives.ToString();
            
        }
    }




    void Start()
    {
        Lives = 1;
        Currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && !WaveActive)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }
    public void BuyTower()
    {
        if(Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;

            Hover.Instance.Deactivate();
        }
    }

    public void OnCurrencyChanged()
    {
        if(Changed != null)
        {
            Changed();
        }
    }
    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();

        sellPriceText.text = (selectedTower.Price/2).ToString() + "$";
        if(selectedTower.NextUpgrade != null)
        { 
            upgradePriceText.text = (selectedTower.NextUpgrade.Price).ToString() + "$";
        }
        else
        {
            upgradePriceText.text = string.Empty;
        }
        
        upgradePanal.SetActive(true);
    }

    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        upgradePanal.SetActive(false);
        selectedTower = null;

    }
    void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        wave++;

        waveText.text = "Wave: " + wave; 

        StartCoroutine(SpawnWave());

        waveBtn.SetActive(false);
    }    
    IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        for(int i=0; i < wave; i++)
        {
            int monsterIndex = 0; // Random.Range(0, 4);

            string type = string.Empty;
            switch (monsterIndex)
            {
                case 0:
                    type = "ArcherMonster";
                    break;
                case 1:
                    type = "ThiefMonster";
                    break;
                case 2:
                    type = "WarriowMonster";
                    break;
                case 3:
                    type = "Villager01Monster";
                    break;
            }

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn(health);
            if(wave % 3== 0)
            {
                health += 5;
            }
            activeMonster.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
       
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonster.Remove(monster);
        if(!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
        
    }

    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }    

    public void SellTower()
    {
        if(selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            selectedTower.GetComponentInParent<TileScript>().WalkAble = true;
            Destroy(selectedTower.transform.parent.gameObject);
            DeselectTower();
        }
    }

    public void UpgradeTower()
    {
        if(selectedTower != null)
        {
            if(selectedTower.Level <= selectedTower.Upgrades.Length 
                && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
                sellPriceText.text = (selectedTower.Price / 2).ToString() + "$";
                if (selectedTower.NextUpgrade != null)
                {
                    upgradePriceText.text = (selectedTower.NextUpgrade.Price).ToString() + "$";
                }
                else
                {
                    upgradePriceText.text = string.Empty;
                }
            }
        }
    }
}
