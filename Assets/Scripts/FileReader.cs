using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FileReader : MonoBehaviour
{
    public int count = 0;

    public Texture2D levelMap;
    public TileBase wallTile;
    public Grid m_Grid;
    public Tilemap Walls;
    //private GridInformation m_Info;
    // Start is called before the first frame update
    void Start()
    {

        //m_Grid = GetComponent<Grid>();
        //Walls = GetComponent<Tilemap>();
        //LoadMap();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Grid && Input.GetMouseButtonDown(0))
        {
            print("ok");
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = m_Grid.WorldToCell(world);
            print(gridPos);

            //Walls.SetTile(gridPos, wallTile);
        }
    }
    //-31,31,0 is top left corner
    void LoadMap()
    {
        Vector3Int gridPos = new Vector3Int(-32, -32, 0);
        string line = "";
        Color[] pixels = levelMap.GetPixels();
       
        int width = levelMap.width;
        int height = levelMap.height;

        for (int y = height-1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {

                if(pixels[y * width + x] == Color.black)
                    Walls.SetTile(gridPos + Vector3Int.right*x - Vector3Int.down*y, wallTile);
                //line.Insert(line.Length, pixels[(y * width) + x].ToString());
                //print(pixels[y*width+x].ToString());              
            }
       
            //print(line);
            line = "";
        }

    }
}
