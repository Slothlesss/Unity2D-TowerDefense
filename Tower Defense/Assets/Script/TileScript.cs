using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TileScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Point GridPosition { get; set; }

    public bool IsEmpty { get; set; }

    Tower myTower;


    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    SpriteRenderer sr;


    public bool WalkAble { get; set; }

    public bool Debugging { get; set; } 
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(
                transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x / 2,
                transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y / 2
            );
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Point gridPos, Vector3 worldPos, Transform parent)
    {
        WalkAble = true;
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    private void OnMouseOver()
    {

        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty && !Debugging)
            {
                ColorTile(emptyColor);
            }

            
            if (!IsEmpty && !Debugging)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if(myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        if(!Debugging)
            ColorTile(Color.white);
    }

    void PlaceTower()
    {
        WalkAble = false;
        if(AStar.GetPath(LevelManager.Instance.BlueSpawn,LevelManager.Instance.RedSpawn) == null)
        {
            WalkAble = true;
            return;
        }
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
        IsEmpty = false;
        ColorTile(Color.white);

        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        GameManager.Instance.BuyTower();
        WalkAble = false;
    }

    void ColorTile(Color32 newColor)
    {
        sr.color = newColor;
    }
}
