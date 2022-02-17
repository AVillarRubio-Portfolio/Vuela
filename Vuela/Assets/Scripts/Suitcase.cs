using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using uAdventure.Runner;

public struct SuitcaseRect
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}

public struct MatrixPos
{
    public int _x;
    public int _y;
    public MatrixPos(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
public class Suitcase : MonoBehaviour
{
    public GameObject referenceCell;
    public int numOfFloor;
    public GameObject[] objectsByFloor;
    public int floor;
    public float alpha;
    public int suitcaseSizeX;
    public int suitcaseSizeY;

    public SuitcaseRect suitcaseRect;
    public Transform[] suitcaseRectTransforms;

    public Transform[,,] tilesMatrix;
    public bool[,,] tilesWithObject;

    public AudioSource wrong;
    public AudioSource pop;

    // Start is called before the first frame update
    void Start()
    {
        tilesMatrix = new Transform[numOfFloor, suitcaseSizeY, suitcaseSizeX];
        tilesWithObject = new bool[numOfFloor, suitcaseSizeY, suitcaseSizeX];

        objectsByFloor[0].SetActive(true);
        objectsByFloor[1].SetActive(false);

        GenerateGrid();
        SetSuitcaseRect();
    }

    void SetSuitcaseRect()
    {
        suitcaseRect = new SuitcaseRect();
        suitcaseRect.left = suitcaseRectTransforms[0].position.x;
        suitcaseRect.top = suitcaseRectTransforms[0].position.y;

        suitcaseRect.right = suitcaseRectTransforms[1].position.x;
        suitcaseRect.bottom = suitcaseRectTransforms[1].position.y;
    }


    private void FadeOutObjects()
    {
        for (int i = 0; i < objectsByFloor[0].transform.childCount; i++)
        {
            objectsByFloor[0].transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
            Color tmp = objectsByFloor[0].transform.GetChild(i).GetComponent<SpriteRenderer>().color;
            tmp.a -= tmp.a / 10;
            objectsByFloor[0].transform.GetChild(i).GetComponent<SpriteRenderer>().color = tmp;
        }
        if (objectsByFloor[0].transform.GetChild(0).GetComponent<SpriteRenderer>().color.a > alpha)
        {
            Invoke("FadeOutObjects", 0.1f);
        }
    }
    private void NextFloor()
    {
        floor++;
        FadeOutObjects();
        objectsByFloor[1].SetActive(true);
        GameManager.Instance.SiguientePlantaMaleta();
    }
    public void Finish()
    {
        bool finished = true;
        for(int y = 0; y < suitcaseSizeY && finished; y++)
        {
            for(int x = 0; x < suitcaseSizeX && finished; x++)
            {
                if (!tilesWithObject[floor, y, x])
                    finished = false;
            }
        }

        if (finished)
        {
            if (floor < numOfFloor - 1)
                NextFloor();
            else
                Game.Instance.RunTarget("Menu_Options", null);
        }
    }

    public void GenerateGrid()
    {
        for (int y = 0; y < suitcaseSizeY; y++)
        {
            for (int x = 0; x < suitcaseSizeX; x++)
            {
                GameObject tile = (GameObject)Instantiate(referenceCell, transform);
                float posX = transform.position.x + x;
                float posY = transform.position.y - y;

                tile.transform.position = new Vector2(posX, posY);

                for (int i = 0; i < numOfFloor; i++)
                {
                    tilesMatrix[i, y, x] = tile.transform;
                }
            }
        }
    }

    public void PlayWrongEffect()
    {
        wrong.Play();
    }

    public void PlayPopEffect()
    {
        pop.Play();
    }
}
