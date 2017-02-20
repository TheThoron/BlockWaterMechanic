using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEdionController : MonoBehaviour
{
    private Camera CMainCamera;
    private World scWorld;

    private bool bLeftMouseDown;
    private bool bRightMouseDown;
    private bool bCreateBlock;

    private int nLastPosx;
    private int nLastPosy;

    // Use this for initialization
    void Start ()
    {
        CMainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        scWorld = GameObject.Find("World").GetComponent<World>();

    }

    private void BlockEditor(int x, int y)
    {
        GameObject block = scWorld.GetBlock(x, y);
        if (bLeftMouseDown)
        {
            if (bCreateBlock)
            {
                if (block == null)
                    scWorld.CreateBlock(x, y);
                else if (block.tag == "Water")
                {
                    scWorld.DestroyBlock(x, y);
                    scWorld.CreateBlock(x, y);
                }
            }
            else
            {
                if (block != null)
                    if (block.tag == "Block")
                    {
                        scWorld.DestroyBlock(x, y);
                    }
            }
        }
        else
        {
            bCreateBlock = true;
            if (block == null)
                scWorld.CreateBlock(x, y);
            else if (block.tag == "Water")
            {
                scWorld.DestroyBlock(x, y);
                scWorld.CreateBlock(x, y);
            }
            else
            {
                bCreateBlock = false;
                scWorld.DestroyBlock(x, y);
            }
            bLeftMouseDown = true;
        }
    }

    private void WaterBlockEditor(int x, int y)
    {
        GameObject block = scWorld.GetBlock(x, y);
        if (bRightMouseDown)
        {
            if (bCreateBlock)
            {
                if (block == null)
                    scWorld.CreateWaterBlock(x, y);
                else if (block.tag == "Block")
                {
                    scWorld.DestroyBlock(x, y);
                    scWorld.CreateWaterBlock(x, y);
                }
                else if (block.tag == "Water")
                {
                    if (nLastPosx != x || nLastPosy != y)
                    {
                        block.GetComponent<Water>().SetFill(1f);
                    }
                }
            }
            else
            {
                if (block != null)
                    if (block.tag == "Water")
                    {
                        scWorld.DestroyBlock(x, y);
                    }
            }
        }
        else
        {
            bCreateBlock = true;
            if (block == null)
            {
                scWorld.CreateWaterBlock(x, y);
            }
            else if (block.tag == "Block")
            {
                scWorld.DestroyBlock(x, y);
                scWorld.CreateWaterBlock(x, y);
            }
            else
            {
                bCreateBlock = false;
                scWorld.DestroyBlock(x, y);
            }
            bRightMouseDown = true;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            bRightMouseDown = false;
            //we get the mouse pointer and transform it into the world.
            Vector3 vMouse = Input.mousePosition;
            vMouse = CMainCamera.ScreenToWorldPoint(new Vector3(vMouse.x, vMouse.y, 19.5f));
            int x = Mathf.RoundToInt(vMouse.x);
            int y = Mathf.RoundToInt(vMouse.y);
            
            if (x >= 0 && y >= 0 && x < scWorld.GetWidth() && y < scWorld.GetHeight())
                BlockEditor(x, y);
        }
        else
            bLeftMouseDown = false;

        if (Input.GetMouseButton(1))
        {
            bLeftMouseDown = false;
            //we get the mouse pointer and transform it into the world.
            Vector3 vMouse = Input.mousePosition;
            vMouse = CMainCamera.ScreenToWorldPoint(new Vector3(vMouse.x, vMouse.y, 19.5f));
            int x = Mathf.RoundToInt(vMouse.x);
            int y = Mathf.RoundToInt(vMouse.y);

            if (x >= 0 && y >= 0 && x < scWorld.GetWidth() && y < scWorld.GetHeight())
                WaterBlockEditor(x, y);
            nLastPosx = x;
            nLastPosy = y;
        }
        else
            bRightMouseDown = false;
    }
}
