using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ObjectRect
{
    public float left;
    public float right;
    public float top;
    public float bottom;

    public float width;
    public float height;
}
public class ObjectToSuitcaseController : MonoBehaviour
{
    public Vector3 oriPos;

    public Suitcase suitcase;
    public int sizeX = 0;
    public int sizeY = 0;
    public float deltaX = 0;
    public float deltaY = 0;

    public List<string> objectSpaces;

    private List<MatrixPos> suitcaseSpaces;

    private ObjectRect objectRect;

    private BoxCollider2D collider;

    private float subSizeX;
    private float subSizeY;

    private bool canDrag;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();

        SetObjectRect();
        suitcaseSpaces = new List<MatrixPos>();

        oriPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectRect();
    }

    private void SetObjectRect()
    {

        objectRect = new ObjectRect();

        objectRect.width = collider.bounds.size.x;
        objectRect.height = collider.bounds.size.y;

        subSizeX = objectRect.width / sizeX;
        subSizeY = objectRect.height / sizeY;

        objectRect.left = transform.position.x - (objectRect.width / 2);
        objectRect.right = transform.position.x + (objectRect.width / 2);

        objectRect.top = transform.position.y + (objectRect.height / 2);
        objectRect.bottom = transform.position.y - (objectRect.height / 2);



    }

    private void UpdateObjectRect()
    {
        objectRect.left = transform.position.x - (objectRect.width / 2);
        objectRect.right = transform.position.x + (objectRect.width / 2);

        objectRect.top = transform.position.y + (objectRect.height / 2);
        objectRect.bottom = transform.position.y - (objectRect.height / 2);
    }
    private bool InsideTheSuitcase()
    {
        return objectRect.left >= suitcase.suitcaseRect.left && objectRect.right <= suitcase.suitcaseRect.right &&
            objectRect.top <= suitcase.suitcaseRect.top && objectRect.bottom >= suitcase.suitcaseRect.bottom;
    }

    private Vector3 distance;
    private Vector3 mPos;
    private void OnMouseDown()
    {
        mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPos.z = 0;
        distance = mPos - transform.position;
        if (objectSpaces[Mathf.Abs((int)((objectRect.top - mPos.y) % sizeY))][Mathf.Abs((int)((mPos.x - objectRect.left) % sizeX))] == '1')
            canDrag = true;
        else canDrag = false;
    }
    private void OnMouseDrag()
    { 
        if (canDrag)
        {
            Vector3 distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen.z));
            transform.position = new Vector3(pos_move.x - distance.x, pos_move.y - distance.y, 0);


            foreach (MatrixPos p in suitcaseSpaces)
            {
                if (objectSpaces[suitcaseSpaces.IndexOf(p) / sizeX][suitcaseSpaces.IndexOf(p) % sizeX] == '1')
                    suitcase.tilesWithObject[suitcase.floor, p._y, p._x] = false;
            }

            suitcaseSpaces.Clear();
        }
    }

    private void OnMouseUp()
    {
        if (this.gameObject.tag != "ObjetoPeligroso")
        {
            if (InsideTheSuitcase())
            {
                for (int y = 0; y < suitcase.suitcaseSizeY; y++)
                {
                    for (int x = 0; x < suitcase.suitcaseSizeX; x++)
                    {
                        Transform tileTransform = suitcase.tilesMatrix[suitcase.floor, y, x];
                        if (tileTransform.position.x >= (objectRect.left) && tileTransform.position.x <= (objectRect.right) &&
                            tileTransform.position.y <= (objectRect.top) && tileTransform.position.y >= (objectRect.bottom) &&
                            (!suitcase.tilesWithObject[suitcase.floor, y, x] ||
                            suitcase.tilesWithObject[suitcase.floor, y, x] && objectSpaces[suitcaseSpaces.Count / sizeX][suitcaseSpaces.Count % sizeX] == '0'))
                        {
                            suitcaseSpaces.Add(new MatrixPos(x, y));
                        }
                    }
                }
                if (sizeX * sizeY == suitcaseSpaces.Count)
                {
                    float midX = 0;
                    float midY = 0;
                    Transform t;
                    foreach (MatrixPos p in suitcaseSpaces)
                    {
                        t = suitcase.tilesMatrix[suitcase.floor, p._y, p._x];
                        midX += t.position.x;
                        midY += t.position.y;
                    }
                    midX /= suitcaseSpaces.Count;
                    midY /= suitcaseSpaces.Count;

                    if (Mathf.Abs(midX - transform.position.x) <= deltaX &&
                        Mathf.Abs(midY - transform.position.y) <= deltaY)
                    {
                        transform.position = new Vector3(midX, midY, transform.position.z);
                        foreach (MatrixPos p in suitcaseSpaces)
                        {
                            if (objectSpaces[suitcaseSpaces.IndexOf(p) / sizeX][suitcaseSpaces.IndexOf(p) % sizeX] == '1')
                                suitcase.tilesWithObject[suitcase.floor, p._y, p._x] = true;
                        }
                        suitcase.PlayPopEffect();
                    }
                }
            }
            suitcase.Finish();
        }
        else
        {
            if (InsideTheSuitcase())
            {
                suitcase.PlayWrongEffect();
                transform.position = oriPos;
            }
        }
    }
}
