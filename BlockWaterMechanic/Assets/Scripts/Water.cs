using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    //Acces to the blocks
    private World scWorld;

    //Amount of water in the block
    private float fFill;
    //Minimum Amount of water to be a bloc of water
    private float fLimitFill = 0.000f;
    //Maximum Amount of water to considerat falling straight
    private float fLimitStraightFalling = 0.5f;

    //water "speed" of spreading
    private static float fSpreadingSpeed;
    private float fNextSpreading;

    public static float GetSpreadingSpeed() { return fSpreadingSpeed; }
    public static void SetSpreadingSpeed(float _fSpreadingSpeed) { fSpreadingSpeed = _fSpreadingSpeed; }

    public float GetFill(){ return fFill; }
    public void SetFill(float _fFill)
    {
        fFill = _fFill;
        CalculExactPos();
    }

    //calculate the Scaling and the position of the water Block
    public void CalculExactPos()
    {
        int y = Mathf.RoundToInt(transform.position.y + 0.25f);
        transform.localScale = (new Vector2(1, fFill));
        transform.position = (new Vector2(transform.position.x, y - (1 - fFill) / 2));
    }

    // Use this for initialization
    void Start ()
    {
        scWorld = GameObject.Find("World").GetComponent<World>();
        fNextSpreading = fSpreadingSpeed;
    }

    //WaterFilling Horizontal if at least one of the blocks in null
    private void FillWaterBlockNullH(GameObject block1, GameObject block2)
    {
        int x1 = (int)transform.position.x - 1;
        int x2 = (int)transform.position.x + 1;
        int y = Mathf.RoundToInt(transform.position.y + 0.25f);

        if (block1 == null && block2 == null)
        {
            scWorld.CreateWaterBlock(x1, y);
            block1 = scWorld.GetBlock(x1, y);
            block1.GetComponent<Water>().SetFill(fFill / 3f);

            scWorld.CreateWaterBlock(x2, y);
            block2 = scWorld.GetBlock(x2, y);
            block2.GetComponent<Water>().SetFill(fFill / 3f);

            SetFill(fFill / 3f);
        }
        else
        {
            if (block1 == null)
            {
                GameObject temp = block2;
                block2 = block1;
                block1 = temp;
                x1 = x2;
                x2 = x1 - 2;
            }
            Debug.Assert(block1 != null, "1 - Erreur script Water : FillWaterBlockNullH()");
            Debug.Assert(block2 == null, "2 - Erreur script Water : FillWaterBlockNullH()");

            if (block1.tag == "Water")
            {
                Water script = block1.GetComponent<Water>();
                float fill = script.GetFill();
                if (fill != 1)
                {
                    if (fill < (fFill / 2))
                    {
                        script.SetFill((fill + fFill) / 3f);
                        this.SetFill((fill + fFill) * 2f / 3f);
                    }
                }
            }
            else
                Debug.Assert(block1.tag == "Block", "3 - Erreur script Water : FillWaterBlockNullH()");
            scWorld.CreateWaterBlock(x2, y);
            block2 = scWorld.GetBlock(x2, y);
            block2.GetComponent<Water>().SetFill(fFill / 2f);

            SetFill(fFill / 2f);
        }
    }

    //WaterFilling Horizontal with no empty block
    private void FillWaterBlockH(GameObject block1, GameObject block2)
    {
        if (block1.tag == "Water" && block2.tag == "Water")
        {
            Water script1 = block1.GetComponent<Water>();
            float fill1 = script1.GetFill();
            Water script2 = block2.GetComponent<Water>();
            float fill2 = script2.GetFill();
            if (fill1 != fill2 || fill2 != fFill)
            {
                script1.SetFill((fill1 + fill2 + fFill) / 3f);
                script2.SetFill((fill1 + fill2 + fFill) / 3f);
                this.SetFill((fill1 + fill2 + fFill) / 3f);
            }
        }
        else
        {
            if (block1.tag == "Water")
            {
                Water script = block1.GetComponent<Water>();
                float fill = script.GetFill();
                if (fill != fFill)
                {
                    script.SetFill((fill + fFill) / 2f);
                    this.SetFill((fill + fFill) / 2f);
                }
            }
            else if (block2.tag == "Water")
            {
                Water script = block2.GetComponent<Water>();
                float fill = script.GetFill();
                if (fill != fFill)
                {
                    script.SetFill((fill + fFill) / 2f);
                    this.SetFill((fill + fFill) / 2f);
                }
            }
        }
    }

    //It simulate the flow of the water if the water is placed on solid block.
    private void FillWaterH()
    {
        int x = (int)transform.position.x;
        int y = Mathf.RoundToInt(transform.position.y + 0.25f);

        if (x > 0 && x < (scWorld.GetWidth() - 1))
        {
            GameObject blockL = scWorld.GetBlock(x - 1, y);
            GameObject blockR = scWorld.GetBlock(x + 1, y);
            if (blockL == null || blockR == null)
            {
                FillWaterBlockNullH(blockL, blockR);
            }
            else
                FillWaterBlockH(blockL, blockR);
        }
        else if (x == 0) //if the block if on the left side
        {
            GameObject blockR = scWorld.GetBlock(x + 1, y);
            if (blockR == null)
            {
                scWorld.CreateWaterBlock(x + 1, y);
                blockR = scWorld.GetBlock(x + 1, y);
                blockR.GetComponent<Water>().SetFill(fFill / 2f);
                SetFill(fFill / 2f);
            }
            else if (blockR.tag == "Water")
            {
                Water script = blockR.GetComponent<Water>();
                float fill = script.GetFill();
                if (fill != fFill)
                {
                    script.SetFill((fill + fFill) / 2f);
                    this.SetFill((fill + fFill) / 2f);
                }
            }
        }
        else //if the block if on the right side
        {
            Debug.Assert(x == scWorld.GetWidth() - 1, "1 - Erreur script Water : FillWaterH()");
            GameObject blockL = scWorld.GetBlock(x - 1, y);
            if (blockL == null)
            {
                scWorld.CreateWaterBlock(x - 1, y);
                blockL = scWorld.GetBlock(x - 1, y);
                blockL.GetComponent<Water>().SetFill(fFill / 2f);
                SetFill(fFill / 2f);
            }
            else if (blockL.tag == "Water")
            {
                Water script = blockL.GetComponent<Water>();
                float fill = script.GetFill();
                if (fill != fFill)
                {
                    script.SetFill((fill + fFill) / 2f);
                    this.SetFill((fill + fFill) / 2f);
                }
            }
        }
    }

    private void interconnectedWaterUpdate()
    {
        int x = (int)transform.position.x;
        int y = Mathf.RoundToInt(transform.position.y + 0.25f);

        bool testVase = false;
        //we test only if the upper block is not a water block
        if ((y + 1) == scWorld.GetHeight())
            testVase = true;
        else if (scWorld.GetBlock(x, y + 1) == null)
            testVase = true;
        else if (scWorld.GetBlock(x, y + 1).tag == "Block")
            testVase = true;

        if (testVase)
        {
            Debug.Assert(scWorld.GetBlock(x, y) != null, "0 - err Water - interconnectedWaterUpdate()");
            BitArray tabVisited = new BitArray(scWorld.GetWidth() * scWorld.GetHeight(), false);

            tabVisited[scWorld.GetHeight() * x + y] = true;
            Debug.Assert(scWorld.GetBlock(x, y - 1).tag == "Water");
            interconnectedFill(ref tabVisited, 1, x, y, x, y - 1);
        }
    }

    //it give as much water as possible from a block to a block
    //return true if fromX, fromY gave all its water
    private bool giveWater(int fromX, int fromY, int toX, int toY)
    {
        Debug.Assert(fromY > toY, "err script water - giveWater");
        GameObject fromBlock = scWorld.GetBlock(fromX, fromY);
        GameObject toBlock = scWorld.GetBlock(toX, toY);
        Debug.Assert(fromBlock != null);
        Debug.Assert(fromBlock.tag == "Water");
        if (toBlock == null)
        {
            scWorld.SetBlock(toX, toY, fromBlock);
            fromBlock.transform.position = new Vector2(fromBlock.transform.position.x - fromX + toX, fromBlock.transform.position.y - fromY + toY);
            scWorld.SetNullBlock(fromX, fromY);
            return true;
        }
        else
        {
            Debug.Assert(toBlock.tag == "Water");
            float fromFill = fromBlock.GetComponent<Water>().GetFill();
            float toFill = toBlock.GetComponent<Water>().GetFill();
            if (fromFill == 1f)
            {
                toBlock.GetComponent<Water>().SetFill(1f);
                fromBlock.GetComponent<Water>().SetFill(toFill);
                return false;
            }
            else
            {
                if (fromFill + toFill > 1f)
                {
                    fromBlock.GetComponent<Water>().SetFill(fromFill - (1f - toFill));
                    toBlock.GetComponent<Water>().SetFill(1f);
                    return false;
                }
                else
                {
                    toBlock.GetComponent<Water>().SetFill(fromFill + toFill);
                    scWorld.DestroyBlock(fromX, fromY);
                    return true;
                }
            }
        }
    }

    //egalize water from x1, y1 and x2, y2, only x2 y2 can be null
    private void egalizeWater(int x1, int y1, int x2, int y2)
    {
        Debug.Assert(y1 == y2, "0 err script water - egalizeWater");
        GameObject block1 = scWorld.GetBlock(x1, y1);
        Debug.Assert(block1 != null, "1 err script water - egalizeWater");
        float fill1 = block1.GetComponent<Water>().GetFill();

        GameObject block2 = scWorld.GetBlock(x2, y2);
        if(block2 == null)
        {
            scWorld.CreateWaterBlock(x2, y2, (fill1/2f));
            block2 = scWorld.GetBlock(x2, y2);
            block1.GetComponent<Water>().SetFill(fill1 / 2f);
        }
        else
        {
            float fill2 = block2.GetComponent<Water>().GetFill();
            if (fill1 != fill2)
            {
                block1.GetComponent<Water>().SetFill((fill1 + fill2) / 2f);
                block2.GetComponent<Water>().SetFill((fill1 + fill2) / 2f);
            }
        }
    }

    //interconnected blocks
    //return true if all water has been passed
    private bool interconnectedFill(ref BitArray tabVisited, int dephtPressure, int startingX, int startingY, int x, int y)
    {
        tabVisited[scWorld.GetHeight() * x + y] = true;
        Debug.Assert(scWorld.GetBlock(startingX, startingY) != null, "0 - err Water - interconnectedFill");
        Debug.Assert(scWorld.GetBlock(startingX, startingY).tag == "Water", "1 - err Water - interconnectedFill");
        Debug.Assert(scWorld.GetBlock(x, y) != null, "2 - err Water - interconnectedFill");
        Debug.Assert(scWorld.GetBlock(x, y).tag == "Water", "3 - err Water - interconnectedFill");
        if (dephtPressure < 0)
        {
            GameObject block = scWorld.GetBlock(startingX, startingY + 1);
            if (block == null)
            {
                if (dephtPressure == -1)
                    egalizeWater(x, y, startingX, startingY + 1);
                else if (giveWater(x, y, startingX, startingY + 1))
                    return false;
            }
            else
            {
                giveWater(x, y, startingX, startingY);
                if (scWorld.GetBlock(x, y) == null)
                    return false;
                Debug.Assert(scWorld.GetBlock(x, y) != null, "4 - err Water - interconnectedFill");
                Debug.Assert(scWorld.GetBlock(x, y).tag == "Water", "5 - err Water - interconnectedFill");
            }
        }
        if (y < (scWorld.GetHeight() - 1))
            if (!tabVisited[scWorld.GetHeight() * x + (y + 1)])
            {
                GameObject block = scWorld.GetBlock(x, y + 1);
                if (block == null)
                {
                    //case current block is under starting block
                    if (dephtPressure > 0)
                    {
                        if (dephtPressure == 1)
                            egalizeWater(startingX, startingY, x, y + 1);
                        else if (giveWater(startingX, startingY, x, y + 1))
                            return true;
                    }
                    else if (dephtPressure == 0)
                    {
                        egalizeWater(startingX, startingY, x, y);
                    }
                }
                else if (block.tag == "Water")
                {
                    if (scWorld.GetBlock(x, y).GetComponent<Water>().GetFill() != 1f)
                    {
                        if (dephtPressure > 0)
                        {
                            if (giveWater(startingX, startingY, x, y))
                            {
                                //Debug.Log("test x : " + x + " y : " + y + " sx : " + startingX + " sy : " + startingY);
                                return true;
                            }
                        }
                        else if (dephtPressure == 0)
                        {
                            egalizeWater(startingX, startingY, x, y);
                        }
                    }
                    else if (interconnectedFill(ref tabVisited, dephtPressure - 1, startingX, startingY, x, y + 1))
                        return true;
                }
                else if (block.tag == "Block")
                {
                    if (dephtPressure > 0)
                    {
                        if (giveWater(startingX, startingY, x, y))
                            return true;
                    }
                    else if (dephtPressure == 0)
                    {
                        egalizeWater(startingX, startingY, x, y);
                    }
                }
            }
        if ((x + 1) < scWorld.GetWidth())
        {
            if (!tabVisited[scWorld.GetHeight() * (x + 1) + y])
            {
                GameObject block = scWorld.GetBlock(x + 1, y);
                //the case block == null is treated elsewhere
                if (block != null)
                    if (block.tag == "Water")
                        if (interconnectedFill(ref tabVisited, dephtPressure, startingX, startingY, x + 1, y))
                            return true;
            }
        }
        if (x > 0)
            if (!tabVisited[scWorld.GetHeight() * (x - 1) + y])
            {
                GameObject block = scWorld.GetBlock(x - 1, y);
                //the case block == null is treated elsewhere
                if (block != null)
                    if (block.tag == "Water")
                        if (interconnectedFill(ref tabVisited, dephtPressure, startingX, startingY, x - 1, y))
                            return true;
            }
        if (y > 0)
            if (!tabVisited[scWorld.GetHeight() * x + (y - 1)])
            {
                GameObject block = scWorld.GetBlock(x, y - 1);
                //the case block == null is treated elsewhere
                if (block != null)
                    if (block.tag == "Water")
                        if (interconnectedFill(ref tabVisited, dephtPressure + 1, startingX, startingY, x, y - 1))
                            return true;
            }
        //there is still water
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        fNextSpreading -= Time.deltaTime;
        if (fNextSpreading < 0)
        {
            int x = (int)transform.position.x;
            int y = Mathf.RoundToInt(transform.position.y + 0.25f);
            Debug.Assert(scWorld.GetBlock(x, y) != null, "0 - err Water - Update()");
            //we limit the minimum amount of water for it to be a block
            if (fFill < fLimitFill)
            {
                scWorld.DestroyBlock(x, y);
            }
            fNextSpreading = fSpreadingSpeed;

            if (y > 0)
            {
                GameObject block = scWorld.GetBlock(x, y - 1); // we are looking at the block under.
                if (block == null)
                {
                    scWorld.SetBlock(x, y - 1, scWorld.GetBlock(x, y));
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    scWorld.SetNullBlock(x, y);
                }
                else if (block.tag == "Block")
                    FillWaterH();
                else if (block.tag == "Water")
                {
                    //we refill the block under before finding a way down horizontaly
                    Water script = block.GetComponent<Water>();
                    float fill = script.GetFill();
                    if (fill != 1f)
                    {
                        if (fill + fFill > 1)
                        {
                            this.SetFill(fFill - (1 - fill));
                            script.SetFill(1f);
                            //if we imaginate the water falling, it'll be straight down as long as at least "fLimitStraightFalling" of a cube can "fall"
                            if (fill > fLimitStraightFalling)
                            {
                                FillWaterH();
                                if (scWorld.GetBlock(x, y) != null) //Destroy isn't immediate
                                {
                                    interconnectedWaterUpdate();
                                }
                            }
                        }
                        else
                        {
                            script.SetFill(fill + fFill);
                            scWorld.DestroyBlock(x, y);
                        }
                    }
                    else
                    {
                        FillWaterH();
                        if (scWorld.GetBlock(x, y) != null) //Destroy isn't immediate
                        {
                            interconnectedWaterUpdate();
                        }
                    }
                }
                else
                    Debug.Assert(false, "Erreur script Water Update");
            }
            else // if the water is at the bottom of the world
                FillWaterH();
        }
    }
}
