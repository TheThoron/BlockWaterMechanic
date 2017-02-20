using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject cSolidBlock;
    public GameObject cSolidBlock2;
    public GameObject cWaterBlock;

    private int nSolidBlocInGame;
    private int nWaterBlocInGame;
    public float fSumWaterInGame;

    public int GetSolidBlocInGame() { return nSolidBlocInGame; }
    public int GetWaterBlocInGame() { return nWaterBlocInGame; }
    public float GetSumWaterInGame() { return fSumWaterInGame; }

    private const int nWidth = 32;
    private const int nHeight = 19;
    private GameObject[,] tabBlocks;

    public int GetWidth() { return nWidth; }
    public int GetHeight() { return nHeight; }

    public GameObject GetBlock(int x, int y) { return tabBlocks[x, y]; }
    public void DestroyBlock(int x, int y)
    {
        Destroy(tabBlocks[x, y]);
        tabBlocks[x, y] = null;
    }
    public void SetNullBlock(int x, int y)
    {
        Debug.Assert(tabBlocks[x, y] != null);
        tabBlocks[x, y] = null;
    }
    public void SetBlock(int x, int y, GameObject block)
    {
        Debug.Assert(tabBlocks[x, y] == null);
        tabBlocks[x, y] = block;
    }
    public void CreateWaterBlock(int x, int y, float fill = 1f)
    {
        Debug.Assert(tabBlocks[x, y] == null);
        tabBlocks[x, y] = (GameObject)Instantiate(cWaterBlock, transform);
        tabBlocks[x, y].transform.position = new Vector2(x, y);
        tabBlocks[x, y].GetComponent<Water>().SetFill(fill);
    }
    public void CreateBlock(int x, int y)
    {
        Debug.Assert(tabBlocks[x, y] == null);
        tabBlocks[x, y] = (GameObject)Instantiate(cSolidBlock, transform);
        tabBlocks[x, y].transform.position = new Vector2(x, y);
    }

    //It calculate the amount of water and the nuber of water block.
    void Update()
    {
        float sum = 0;
        int nWaterBlock = 0;
        int nSolidBlock = 0;
        for (int x = 0; x < nWidth; ++x)
            for (int y = 0; y < nHeight; ++y)
            {
                if (tabBlocks[x, y] != null)
                {
                    if (tabBlocks[x, y].tag == "Water")
                    {
                        ++nWaterBlock;
                        sum += tabBlocks[x, y].GetComponent<Water>().GetFill();
                    }
                    else
                        ++nSolidBlock;
                }
            }
        fSumWaterInGame = sum;
        nWaterBlocInGame = nWaterBlock;
        nSolidBlocInGame = nSolidBlock;
    }

    public void GenerateWorldScene1()
    {
        tabBlocks = new GameObject[nWidth, nHeight];
        tabBlocks[16, 7] = cSolidBlock;
        tabBlocks[15, 5] = cSolidBlock;
        tabBlocks[17, 5] = cSolidBlock;
        tabBlocks[14, 3] = cSolidBlock;
        tabBlocks[16, 3] = cSolidBlock;
        tabBlocks[18, 3] = cSolidBlock;
        tabBlocks[13, 1] = cSolidBlock;
        tabBlocks[15, 1] = cSolidBlock;
        tabBlocks[17, 1] = cSolidBlock;
        tabBlocks[19, 1] = cSolidBlock;

        tabBlocks[16, 9] = cWaterBlock;
        tabBlocks[16, 12] = cWaterBlock;
        tabBlocks[16, 13] = cWaterBlock;
        tabBlocks[16, 14] = cWaterBlock;
        tabBlocks[16, 15] = cWaterBlock;
        tabBlocks[16, 16] = cWaterBlock;
        tabBlocks[16, 17] = cWaterBlock;
        tabBlocks[16, 18] = cWaterBlock;

        InstenciateWorld();
    }

    public void GenerateWorldScene2()
    {
        GameObject n = null;
        GameObject s = cSolidBlock;
        GameObject w = cWaterBlock;
        //work with Json . load too
        tabBlocks = new GameObject[nWidth, nHeight]
        {   { s, s, s, s, s, s, s, s, s, s, s, s, s, s, s, s, w, w, w,},
            { s, s, s, s, s, n, n, s, w, w, s, w, w, w, w, w, w, w, w,},
            { s, s, s, s, n, n, n, n, n, w, w, w, w, w, w, w, w, w, w,},
            { s, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w,},
            { s, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w,},
            { s, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w,},
            { s, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w,},
            { s, n, s, s, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w,},
            { s, n, n, s, s, w, s, w, w, w, w, w, w, w, w, w, w, w, w,},
            { n, n, n, s, s, s, s, s, s, s, s, s, w, w, w, w, w, w, w,},
            { s, n, n, s, n, s, n, n, n, n, n, s, s, s, w, w, w, w, w,},
            { s, n, s, s, n, s, n, s, s, n, n, n, n, s, w, w, w, w, w,},
            { s, n, n, n, n, s, n, n, s, s, s, s, n, s, w, w, w, w, w,},
            { s, n, s, s, s, s, n, n, n, s, s, n, n, s, w, w, w, w, w,},
            { n, n, s, s, s, s, s, n, n, n, s, n, s, s, w, w, w, w, w,},
            { n, n, s, s, s, s, s, s, n, n, s, n, s, s, w, w, w, w, w,},
            { n, n, n, s, s, s, s, s, n, n, s, n, n, s, s, w, w, w, w,},
            { n, n, n, n, n, s, s, n, n, n, s, s, n, n, s, w, w, w, w,},
            { s, s, s, n, n, s, n, n, s, s, s, n, n, n, s, w, w, w, w,},
            { s, s, n, n, n, s, s, n, n, s, s, n, s, s, s, w, w, w, w,},
            { s, s, n, n, n, s, s, s, n, s, s, n, n, s, s, w, w, w, w,},
            { s, n, n, n, n, n, n, n, n, s, s, s, n, s, s, w, w, w, w,},
            { s, n, n, n, n, n, n, n, s, s, s, s, n, s, s, w, w, w, w,},
            { s, n, n, n, n, n, n, s, s, s, s, n, n, s, s, w, w, w, w,},
            { s, n, n, n, s, s, n, s, n, n, n, n, n, n, s, w, w, w, w,},
            { s, n, n, s, s, n, s, s, s, s, n, n, n, n, s, s, w, w, w,},
            { s, n, s, s, n, n, n, n, n, s, s, s, s, s, s, s, w, w, w,},
            { s, n, n, s, s, n, n, n, n, n, n, n, s, s, s, s, w, w, w,},
            { s, n, n, n, n, n, n, n, n, n, n, n, n, n, s, s, w, w, w,},
            { s, n, n, n, n, s, s, s, s, s, s, s, n, n, s, s, w, w, w,},
            { n, n, n, s, s, s, n, n, n, s, n, n, n, n, n, s, s, w, w,},
            { n, n, s, n, n, n, n, n, n, n, n, n, n, n, n, s, s, w, w,}, };

        InstenciateWorld();
    }

    private void InstenciateWorld()
    {
        for (int x = 0; x < nWidth; ++x)
            for (int y = 0; y < nHeight; ++y)
            {
                if (tabBlocks[x, y] != null)
                {
                    tabBlocks[x, y] = (GameObject)Instantiate(tabBlocks[x, y], transform);
                    tabBlocks[x, y].transform.position = new Vector2(x, y);
                    if (tabBlocks[x, y].tag == "Water")
                    {
                        tabBlocks[x, y].GetComponent<Water>().SetFill(1f);
                    }
                }
            }
    }
}
