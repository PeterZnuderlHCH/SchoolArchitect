using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Path;
using RPG;

[System.Serializable]
public class Tile : ITile {

    public int x, y;
    public List<ITile> neighbours = new List<ITile>();
    public float difficulty = 1;

    public Area partOfArea;

    //public TextMesh text1, text2;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        //text1 = GetComponentsInChildren<TextMesh>(true)[0];
        //text2 = GetComponentsInChildren<TextMesh>(true)[1];
    }

    public override List<ITile> GetNeighbours()
    {
        return neighbours;
    }

    public override Vector2 getPosition()
    {
        return new Vector2(x, y);
    }

    public override void SetGScore(float score)
    {
        base.SetGScore(score);
        //text1.text = "gScore= " + gCost;
    }

    public override void SetHScore(float score)
    {
        base.SetHScore(score);
        //text2.text = "fScore= " + fCost;
    }

    public void CalculateNeighbours(List<List<Tile>> map)
    {
        Tile t = this;
        int MapWidth = map.Count; 
        int MapHeight = map[0].Count;
        List<ITile> n = new List<ITile>();

        //Add only tiles that exist otherwise there would be a null exception (we would be trying to look for a target in a non existing place => BAD
        if (t.x > 0)
        {
            n.Add(map[t.x - 1][t.y]);
        }
        if (t.x < MapWidth - 1)
        {
            n.Add(map[t.x + 1][t.y]);
        }
        if (t.y > 0)
        {
            n.Add(map[t.x][t.y - 1]);
        }
        if (t.y < MapHeight - 1)
        {
            n.Add(map[t.x][t.y + 1]);
        }
        if(t.x > 0 && t.y > 0)
        {
            n.Add(map[t.x - 1][t.y - 1]);
        }
        if (t.x > 0 && t.y < MapHeight -1)
        {
            n.Add(map[t.x - 1][t.y + 1]);
        }
        if (t.x < MapWidth -1 && t.y > 0)
        {
            n.Add(map[t.x + 1][t.y - 1]);
        }
        if (t.x < MapWidth - 1 && t.y < MapHeight - 1)
        {
            n.Add(map[t.x + 1][t.y + 1]);
        }
        

        neighbours = n;
    }

    public override void ChangeColourTo(Color newCol)
	{
		sprite.color = newCol;
	}

    public override float TileDiff()
    {
        float diff = 1;
        float keepFromWallsPenalty = 0f;
        foreach(var neighbour in neighbours)
        {
            if (!neighbour.isWalkable)
            {
                keepFromWallsPenalty += 0.25f;
            }
        }
    

        diff = (isWalkable) ? difficulty + keepFromWallsPenalty: 9999;

        return diff;
    }

	public void ChangeColourBy(Color newCol)
	{
		sprite.color -= newCol;
	}

    void OnMouseEnter()
    {
        if(AreaManager.instance.HandleClick(this, false, false)) { return; }
        if (Input.GetMouseButton(0))
        {
            MouseCheck();
        }
        
    }

    private void OnMouseDown()
    {
        if (AreaManager.instance.HandleClick(this, true, false)) { return; }
        MouseCheck();
        
    }

    private void OnMouseUp()
    {
        if (AreaManager.instance.HandleClick(this, false, true)) { return; }
    }

    void MouseCheck()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            SetDifficulty(5.0f);
        }
        else
        {
            if (isWalkable)
            {
                ChangeWalkable(false);
            }
            else
            {
                ChangeWalkable(true);
            }
        }
    }

    public void SetDifficulty(float diff)
    {
        if (diff == 1.0f) { ChangeColourTo(Color.white); } else { ChangeColourTo(Color.blue); }
        difficulty = diff;
    }

    public void ChangeWalkable(bool walk)
    {
        if (!walk)
        {
            ChangeColourTo(Color.black);
            isWalkable = false;
        }
        else
        {
            isWalkable = true;
        }
        if (isWalkable && difficulty <= 1.0f)
        {
            ChangeColourTo(Color.white);
        }
        else if(isWalkable && difficulty > 1.0f)
        {
            ChangeColourTo(Color.blue);
        }
    }
    

}
