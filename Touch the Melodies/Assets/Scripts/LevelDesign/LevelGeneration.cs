using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;
    public int[,] levelMap;
    public bool levelGenerated;
    [SerializeField] private GameObject Env;
    private GameObject[,] _obstacleMap;

    void Awake()
    {
        instance = this;
    }

    public void GetMapFromFile(int levelID)
    {
        string pathToLevelMapFile = $"Levels/level{levelID}";
        var mapStrings = Resources.Load<TextAsset>(pathToLevelMapFile).ToString();
        string[] levelInfo = mapStrings.Split('\n');
        levelMap = new int[5, levelInfo.Length];
        _obstacleMap = new GameObject[5, levelInfo.Length];
        int yInverse = levelMap.GetLength(1);
        for(int y = 0; y < levelMap.GetLength(1); y++)
        {
            yInverse--;
            for(int x = 0; x < levelMap.GetLength(0); x++)
            {
                levelMap[x,y] = int.Parse(levelInfo[yInverse].Split(',')[x]);
            }
        }
        Debug.Log($"{levelMap[0, 1]}, {levelMap[1, 2]}, {levelMap[2, 2]}, {levelMap[3, 0]}, {levelMap[4, 0]}");
    }

    public void GenerateLevel()
    {
        for (int y = 0; y < levelMap.GetLength(1); y++)
        {
            for (int x = 0; x < levelMap.GetLength(0); x++)
            {
                if (levelMap[x, y] == 0)
                {
                    continue;
                }
                GameObject obs = Resources.Load<GameObject>($"Obstacles/{Item_IDManager.ItemNames[levelMap[x, y]]}");
                GameObject newItem = Instantiate(obs, new Vector3(x - 2, obs.transform.position.y, y - 3), obs.transform.rotation);
                newItem.transform.parent = Env.transform;
                if(Item_IDManager.ItemNames[levelMap[x, y]] == "05_Fragile" && Item_IDManager.fragileIdentificationIDs[levelMap[x, y]] == 0)
                {
                    newItem.AddComponent<Fragile_Activation>();
                    newItem.GetComponent<Fragile_ConnectionIdentity>().f_single = true;
                }
                if (levelMap[x, y] >= 3 && levelMap[x, y] <= 16)
                {
                    ChechUniTiles(levelMap[x, y], newItem);
                }
                if (newItem.CompareTag("Enemy") || newItem.CompareTag("Pick-up") || newItem.CompareTag("Crown"))
                {
                    if (Item_IDManager.ObstacleOrientation.ContainsKey(levelMap[x,y]) && Item_IDManager.ObstacleOrientation[levelMap[x, y]] == "Right")
                    {
                        newItem.transform.localScale = new Vector3(-newItem.transform.localScale.x, newItem.transform.localScale.y, newItem.transform.localScale.z);
                    }
                    GameObject tileAsset = Resources.Load<GameObject>($"Obstacles/{Item_IDManager.TileType[levelMap[x, y]]}");
                    GameObject tile = Instantiate(tileAsset, new Vector3(x - 2, obs.transform.position.y, y - 3), tileAsset.transform.rotation, Env.transform);
                    newItem.transform.parent = tile.transform;
                    newItem = tile;
                    if (Item_IDManager.TileType[levelMap[x, y]] == "05_Fragile" && Item_IDManager.fragileIdentificationIDs[levelMap[x, y]] == 0)
                    {
                        newItem.AddComponent<Fragile_Activation>();
                        newItem.GetComponent<Fragile_ConnectionIdentity>().f_single = true;
                    }
                }
                _obstacleMap[x, y] = newItem;
            }
        }
        levelGenerated = true;
        SliderUnion();
        for (int y = 0; y < levelMap.GetLength(1); y++)
        {
            for (int x = 0; x < levelMap.GetLength(0); x++)
            {
                if (_obstacleMap[x, y] != null && _obstacleMap[x, y].CompareTag("Fragile") && !_obstacleMap[x, y].GetComponent<Fragile_ConnectionIdentity>().f_single && !_obstacleMap[x, y].GetComponent<Fragile_ConnectionIdentity>().f_attached)
                {
                    ComplexFragilesUnion(x, y);
                }
                if(_obstacleMap[x, y] != null && (_obstacleMap[x, y].CompareTag("MoverArrow") || _obstacleMap[x, y].CompareTag("MoverArrowAuto")) && !_obstacleMap[x, y].GetComponent<Mover_Identificator>().m_single && !_obstacleMap[x, y].GetComponent<Mover_Identificator>().m_attached)
                {
                    ComplexMoverUnion(x, y);
                }
            }
        }
        _obstacleMap = null;
    }
    private void SliderUnion()
    {
        for(int sY = 0; sY < levelMap.GetLength(1); sY++)
        {
            int itemID = 0;
            GameObject sliderTile = new GameObject("Slider");
            sliderTile.transform.parent = Env.transform;
            sliderTile.transform.SetPositionAndRotation(new Vector3(0.0f, -0.45f, sY - 3.0f), Quaternion.identity);
            for (int sX = 0; sX < levelMap.GetLength(0); sX++)
            {
                if (Item_IDManager.ItemNames[levelMap[sX,sY]] == "04_Slider" || (Item_IDManager.TileType.ContainsKey(levelMap[sX, sY])  && Item_IDManager.TileType[levelMap[sX,sY]] == "04_Slider"))
                {
                    itemID = levelMap[sX, sY];
                    for(int t = 0; t < Env.transform.childCount; t++)
                    {
                        if(Env.transform.GetChild(t).transform.position == new Vector3(sX - 2, Env.transform.GetChild(t).transform.position.y, sY - 3))
                        {
                            Env.transform.GetChild(t).transform.SetParent(sliderTile.transform);
                        }
                    }
                }
            }
            if(sliderTile.transform.childCount > 0)
            {
                sliderTile.SetActive(false);
                sliderTile.AddComponent<Rigidbody>();
                sliderTile.GetComponent<Rigidbody>().useGravity = false;
                sliderTile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                sliderTile.AddComponent<Slider_Floating>();
                if (Item_IDManager.ObstacleOrientation[itemID] == "Right")
                {
                    sliderTile.GetComponent<Slider_Floating>().Right = true;
                }
                else
                {
                    sliderTile.GetComponent<Slider_Floating>().Left = true;
                }
            }
            sliderTile.SetActive(true);
            if(sliderTile.transform.childCount == 0)
            {
                Destroy(sliderTile.gameObject);
            }
        }
    }
    private void ComplexFragilesUnion(int xPos, int yPos)
    {
        GameObject fragileToComplicate = _obstacleMap[xPos, yPos];
        fragileToComplicate.GetComponent<Fragile_ConnectionIdentity>().f_attached = true;
        List<GameObject> adjacentTiles = new List<GameObject>();
        adjacentTiles.Add(fragileToComplicate);
        GameObject Fragile = new GameObject("Fragile");
        Fragile.transform.parent = Env.transform;
        Fragile.AddComponent<Fragile_Activation>();
        Fragile.AddComponent<Rigidbody>();
        Fragile.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Fragile.GetComponent<Rigidbody>().useGravity = false;
        Fragile.tag = "Fragile";
        for (int f = 0; f < adjacentTiles.Count; f++)
        {
            bool right = true, left = true, forward = true, backward = true, forwardleft = true, forwardright = true, backwardleft = true, backwardright = true;
            int xPosOnMap = 0, yPosOnMap = 0;
            xPosOnMap = 2 + Mathf.RoundToInt(adjacentTiles[f].transform.position.x);
            yPosOnMap = 3 + Mathf.RoundToInt(adjacentTiles[f].transform.position.z);
            if (yPosOnMap < levelMap.GetLength(1) - 1) //f
            {
                if (_obstacleMap[xPosOnMap, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap, yPosOnMap + 1].tag == "Fragile" && !_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap + 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    if(!_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Fragile_ConnectionIdentity>().f_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap, yPosOnMap + 1]);
                        _obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Fragile_ConnectionIdentity>().f_attached = true;
                        Destroy(_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Fragile_Activation>());
                    }
                    forward = false;
                }
            }
            if (yPosOnMap > 0) //b
            {
                if (_obstacleMap[xPosOnMap, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap, yPosOnMap - 1].tag == "Fragile" && !_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap - 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    if(!_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Fragile_ConnectionIdentity>().f_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap, yPosOnMap - 1]);
                        _obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Fragile_ConnectionIdentity>().f_attached = true;
                        Destroy(_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Fragile_Activation>());
                    }
                    backward = false;
                }
            }
            if (xPosOnMap < 4) //r
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap].tag == "Fragile" && !_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap + 1, yPosOnMap]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    if(!_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap + 1, yPosOnMap]);
                        _obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_attached = true;
                        Destroy(_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Fragile_Activation>());
                    }
                    right = false;
                }
            }
            if (xPosOnMap > 0) //l
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap].tag == "Fragile" && !_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap - 1, yPosOnMap]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    if(!_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap - 1, yPosOnMap]);
                        _obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Fragile_ConnectionIdentity>().f_attached = true;
                        Destroy(_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Fragile_Activation>());
                    }
                    left = false;
                }
            }
            if (yPosOnMap < levelMap.GetLength(1) - 1 && xPosOnMap > 0) // fl
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap + 1].tag == "Fragile" && !_obstacleMap[xPosOnMap - 1, yPosOnMap + 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap - 1, yPosOnMap + 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    forwardleft = false;
                }
            }
            if (yPosOnMap < levelMap.GetLength(1) - 1 && xPosOnMap < 4) // fr
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap + 1].tag == "Fragile" && !_obstacleMap[xPosOnMap + 1, yPosOnMap + 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap + 1, yPosOnMap + 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    forwardright = false;
                }
            }
            if (yPosOnMap > 0 && xPosOnMap > 0) // bl
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap - 1].tag == "Fragile" && !_obstacleMap[xPosOnMap - 1, yPosOnMap - 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap - 1, yPosOnMap - 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    backwardleft = false;
                }
            }
            if (yPosOnMap > 0 && xPosOnMap < 4) // br
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap - 1].tag == "Fragile" && !_obstacleMap[xPosOnMap + 1, yPosOnMap - 1].GetComponent<Fragile_ConnectionIdentity>().f_single && Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap + 1, yPosOnMap - 1]] == Item_IDManager.fragileIdentificationIDs[levelMap[xPosOnMap, yPosOnMap]])
                {
                    backwardright = false;
                }
            }
            RedrawComplexTiles(adjacentTiles[f], forward, backward, left, right, forwardleft, forwardright, backwardleft, backwardright, true, false, false);
        }
        for(int fa = 0; fa < adjacentTiles.Count; fa++)
        {
            adjacentTiles[fa].transform.parent = Fragile.transform;
        }
    }

    private void RedrawComplexTiles(GameObject tileToRedraw, bool F, bool B, bool L, bool R, bool FL, bool FR, bool BL, bool BR, bool fragileMat, bool moverMat, bool moverautoMat)
    {
        string pathToUniTiles = "Obstacles/UniversalTiles";
        if(fragileMat)
        {
            tileToRedraw.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Fragile");
        }
        if (moverMat)
        {
            tileToRedraw.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Mover");
        }
        if (moverautoMat)
        {
            tileToRedraw.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/MoverAuto");
        }
        int sides = 0;
        List<bool> boolSides = new List<bool>() { F, B, L, R };
        for(int b = 0; b < 4; b++)
        {
            if (boolSides[b])
            {
                sides++;
            }
        }
        if(sides == 3)
        {
            tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/D").GetComponent<MeshFilter>().sharedMesh;
            int rotation = 0;
            if (!F && B && R && L)
            {
                rotation = 0;
            }
            else if (F && B && !R && L)
            {
                rotation = 90;
            }
            else if (F && !B && R && L)
            {
                rotation = 180;
            }
            else if (F && B && R && !L)
            {
                rotation = 270;
            }
            tileToRedraw.transform.rotation = Quaternion.Euler(tileToRedraw.transform.rotation.eulerAngles.x, rotation, tileToRedraw.transform.rotation.eulerAngles.z);
            if(tileToRedraw.CompareTag("MoverArrow") || tileToRedraw.CompareTag("MoverArrowAuto"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y, rotation);
            }
            if (tileToRedraw.transform.childCount != 0 && tileToRedraw.transform.GetChild(0).gameObject.CompareTag("Enemy"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y - rotation, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.z);
            }
        }
        if(sides == 2)
        {
            int rotation = 0;
            if (F && B || L & R)
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/I").GetComponent<MeshFilter>().sharedMesh;
                if (F && B)
                {
                    rotation = 0;
                }
                else if (L && R)
                {
                    rotation = 90;
                }
            }
            else if((F && R) || (B && R) || (B && L) || (F && L))
            {
                if ((F && R && !BL) || (B && R && !FL) || (B && L && !FR) || (F && L && !BR))
                {
                    tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/L").GetComponent<MeshFilter>().sharedMesh;
                }
                else if ((F && R && BL) || (B && R && FL) || (B && L && FR) || (F && L && BR))
                {
                    tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/L1").GetComponent<MeshFilter>().sharedMesh;
                }

                if (F && R && !B && !L)
                {
                    rotation = 0;
                }
                else if (B && R && !F && !L)
                {
                    rotation = 90;
                }
                else if (B && L && !F && !R)
                {
                    rotation = 180;
                }
                else if (F && L && !B && !R)
                {
                    rotation = 270;
                }
            }
            tileToRedraw.transform.rotation = Quaternion.Euler(tileToRedraw.transform.rotation.eulerAngles.x, rotation, tileToRedraw.transform.rotation.eulerAngles.z);
            if (tileToRedraw.CompareTag("MoverArrow") || tileToRedraw.CompareTag("MoverArrowAuto"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y, rotation);
            }
            if (tileToRedraw.transform.childCount != 0 && tileToRedraw.transform.GetChild(0).gameObject.CompareTag("Enemy"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y - rotation, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.z);
            }
        }
        if(sides == 1)
        {
            int rotation = 0;
            if((F && !BL && !BR) || (B && !FL && ! FR) || (L && !FR && ! BR) || (R && !FL && !BL))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T").GetComponent<MeshFilter>().sharedMesh;
            }
            else if((F && !BL && BR) || (B && FL && !FR) || (L && FR && !BR) || (R && !FL && BL))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T1a").GetComponent<MeshFilter>().sharedMesh;
            }
            else if ((F && BL && !BR) || (B && !FL && FR) || (L && !FR && BR) || (R && FL && !BL))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T1b").GetComponent<MeshFilter>().sharedMesh;
            }
            else if ((F && BL && BR) || (B && FL && FR) || (L && FR && BR) || (R && FL && BL))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T2").GetComponent<MeshFilter>().sharedMesh;
            }

            if (F)
            {
                rotation = 0;
            }
            else if (R)
            {
                rotation = 90;
            }
            else if (B)
            {
                rotation = 180;
            }
            else if (L)
            {
                rotation = 270;
            }
            tileToRedraw.transform.rotation = Quaternion.Euler(tileToRedraw.transform.rotation.eulerAngles.x, rotation, tileToRedraw.transform.rotation.eulerAngles.z);
            if (tileToRedraw.CompareTag("MoverArrow") || tileToRedraw.CompareTag("MoverArrowAuto"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y, rotation);
            }
            if (tileToRedraw.transform.childCount != 0 && tileToRedraw.transform.GetChild(0).gameObject.CompareTag("Enemy"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y - rotation, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.z);
            }
        }
        if(sides == 0)
        {
            int rotation = 0;
            if (FR || BR || BL || FR)
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/X1").GetComponent<MeshFilter>().sharedMesh;
                if(FR && !FL && !BR && !BL)
                {
                    rotation = 0;
                }
                if (!FR && !FL && BR && !BL)
                {
                    rotation = 90;
                }
                if (!FR && !FL && !BR && BL)
                {
                    rotation = 180;
                }
                if (!FR && FL && !BR && !BL)
                {
                    rotation = 270;
                }
            }
            else if((FR && BR) || (BR && BL) || (BL && FL) || (FL && FR))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T2a").GetComponent<MeshFilter>().sharedMesh;
                if (FR && !FL && BR && !BL)
                {
                    rotation = 0;
                }
                if (!FR && !FL && BR && BL)
                {
                    rotation = 90;
                }
                if (!FR && FL && !BR && BL)
                {
                    rotation = 180;
                }
                if (FR && FL && !BR && !BL)
                {
                    rotation = 270;
                }
            }
            else if ((FR && BL) || (FL && BR))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T2b").GetComponent<MeshFilter>().sharedMesh;
                if (FR && !FL && !BR && BL)
                {
                    rotation = 0;
                }
                if (FR && !FL && !BR && BL)
                {
                    rotation = 90;
                }
            }
            else if ((FL && BL && BR) || (FL && FR && BL) || (FL && FR && BR) || (FR && BR && BL))
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T3").GetComponent<MeshFilter>().sharedMesh;
                if (!FR && FL && BR && BL)
                {
                    rotation = 0;
                }
                if (FR && FL && !BR && BL)
                {
                    rotation = 90;
                }
                if (FR && FL && BR && !BL)
                {
                    rotation = 180;
                }
                if (FR && !FL && BR && BL)
                {
                    rotation = 270;
                }
            }
            else if(FR && FL && BR && BL)
            {
                tileToRedraw.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>($"{pathToUniTiles}/T4").GetComponent<MeshFilter>().sharedMesh;
            }
            tileToRedraw.transform.localRotation = Quaternion.Euler(tileToRedraw.transform.localRotation.eulerAngles.x, rotation, tileToRedraw.transform.localRotation.eulerAngles.z);
            if (tileToRedraw.CompareTag("MoverArrow") || tileToRedraw.CompareTag("MoverArrowAuto"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y, rotation);
            }
            if (tileToRedraw.transform.childCount != 0 && tileToRedraw.transform.GetChild(0).gameObject.CompareTag("Enemy"))
            {
                tileToRedraw.transform.GetChild(0).rotation = Quaternion.Euler(tileToRedraw.transform.GetChild(0).rotation.eulerAngles.x, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.y - rotation, tileToRedraw.transform.GetChild(0).rotation.eulerAngles.z);
            }
        }
    }
    private void ComplexMoverUnion(int xPos, int yPos)
    {
        GameObject moverToComplicate = _obstacleMap[xPos, yPos];
        moverToComplicate.GetComponent<Mover_Identificator>().m_attached = true;
        bool isMover = true;
        if(moverToComplicate.GetComponent<Mover_Identificator>().tag == "MoverArrowAuto")
        {
            isMover = false;
        }
        List<GameObject> adjacentTiles = new List<GameObject>();
        adjacentTiles.Add(moverToComplicate);
        GameObject MoverObstacle = new GameObject("Mover");
        MoverObstacle.AddComponent<AutoMover_Detector>();
        if (isMover)
        {
            MoverObstacle.name = "Mover";
            MoverObstacle.transform.parent = Env.transform;
        }
        else
        {
            MoverObstacle.name = "MoverAuto";
            MoverObstacle.transform.parent = Env.transform;

        }
        for (int f = 0; f < adjacentTiles.Count; f++)
        {
            bool right = true, left = true, forward = true, backward = true, forwardleft = true, forwardright = true, backwardleft = true, backwardright = true;
            int xPosOnMap = 0, yPosOnMap = 0;
            xPosOnMap = 2 + Mathf.RoundToInt(adjacentTiles[f].transform.position.x);
            yPosOnMap = 3 + Mathf.RoundToInt(adjacentTiles[f].transform.position.z);
            if (yPosOnMap < levelMap.GetLength(1) - 1 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_forward) //f
            {
                if (_obstacleMap[xPosOnMap, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_backward && !_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_single)
                {
                    if (!_obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap, yPosOnMap + 1]);
                        _obstacleMap[xPosOnMap, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_attached = true;
                    }
                    forward = false;
                }
            }
            if (yPosOnMap > 0 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_backward) //b
            {
                if (_obstacleMap[xPosOnMap, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_forward && !_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_single)
                {
                    if (!_obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap, yPosOnMap - 1]);
                        _obstacleMap[xPosOnMap, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_attached = true;
                    }
                    backward = false;
                }
            }
            if (xPosOnMap < 4 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_right) //r
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Mover_Identificator>().m_left && !_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Mover_Identificator>().m_single)
                {
                    if (!_obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Mover_Identificator>().m_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap + 1, yPosOnMap]);
                        _obstacleMap[xPosOnMap + 1, yPosOnMap].GetComponent<Mover_Identificator>().m_attached = true;
                    }
                    right = false;
                }
            }
            if (xPosOnMap > 0 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_left) //l
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Mover_Identificator>().m_right && !_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Mover_Identificator>().m_single)
                {
                    if (!_obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Mover_Identificator>().m_attached)
                    {
                        adjacentTiles.Add(_obstacleMap[xPosOnMap - 1, yPosOnMap]);
                        _obstacleMap[xPosOnMap - 1, yPosOnMap].GetComponent<Mover_Identificator>().m_attached = true;
                    }
                    left = false;
                }
            }
            if (yPosOnMap < levelMap.GetLength(1) - 1 && xPosOnMap > 0 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_forward && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_left) // fl
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap + 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap - 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_right && !_obstacleMap[xPosOnMap - 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_backward && !_obstacleMap[xPosOnMap - 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_single)
                {
                    forwardleft = false;
                }
            }
            if (yPosOnMap < levelMap.GetLength(1) - 1 && xPosOnMap < 4 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_forward && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_right) // fr
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap + 1] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap + 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap + 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_left && !_obstacleMap[xPosOnMap + 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_backward && !_obstacleMap[xPosOnMap + 1, yPosOnMap + 1].GetComponent<Mover_Identificator>().m_single)
                {
                    forwardright = false;
                }
            }
            if (yPosOnMap > 0 && xPosOnMap > 0 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_backward && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_left) // bl
            {
                if (_obstacleMap[xPosOnMap - 1, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap - 1, yPosOnMap - 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap - 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_right && !_obstacleMap[xPosOnMap - 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_forward && !_obstacleMap[xPosOnMap - 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_single)
                {
                    backwardleft = false;
                }
            }
            if (yPosOnMap > 0 && xPosOnMap < 4 && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_backward && !_obstacleMap[xPosOnMap, yPosOnMap].GetComponent<Mover_Identificator>().m_right) // br
            {
                if (_obstacleMap[xPosOnMap + 1, yPosOnMap - 1] != null && _obstacleMap[xPosOnMap + 1, yPosOnMap - 1].GetComponent<Mover_Identificator>() != null && !_obstacleMap[xPosOnMap + 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_left && !_obstacleMap[xPosOnMap + 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_forward && !_obstacleMap[xPosOnMap + 1, yPosOnMap - 1].GetComponent<Mover_Identificator>().m_single)
                {
                    backwardright = false;
                }
            }
            if(isMover)
            {
                RedrawComplexTiles(adjacentTiles[f], forward, backward, left, right, forwardleft, forwardright, backwardleft, backwardright, false, true, false);
            }
            else
            {
                RedrawComplexTiles(adjacentTiles[f], forward, backward, left, right, forwardleft, forwardright, backwardleft, backwardright, false, false, true);
            }
        }
        for (int ma = 0; ma < adjacentTiles.Count; ma++)
        {
            adjacentTiles[ma].transform.parent = MoverObstacle.transform;
            adjacentTiles[ma].AddComponent<AutoMover_Detector>();
            adjacentTiles[ma].AddComponent<Rigidbody>();
            adjacentTiles[ma].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            adjacentTiles[ma].GetComponent<Rigidbody>().useGravity = false;
            adjacentTiles[ma].GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/{MoverObstacle.name}");
            if(!isMover)
            {
                adjacentTiles[ma].tag = "MoverAuto";
            }
        }
    }

    private void ChechUniTiles(int id, GameObject uniTile)
    {
        uniTile.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/General");
        uniTile.GetComponent<MeshFilter>().mesh = Resources.Load<GameObject>("Obstacles/UniversalTiles/O").GetComponent<MeshFilter>().sharedMesh;
        if (id == 3)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 4)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 5)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 6)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 7)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 8)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 9)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 10)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 11)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = false;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 12)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 13)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 14)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
        if (id == 15)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = false;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = true;
            uniTile.GetComponent<Mover_Identificator>().m_right = true;
            return;
        }
        if (id == 16)
        {
            uniTile.GetComponent<Mover_Identificator>().m_forward = true;
            uniTile.GetComponent<Mover_Identificator>().m_backward = true;
            uniTile.GetComponent<Mover_Identificator>().m_left = false;
            uniTile.GetComponent<Mover_Identificator>().m_right = false;
            return;
        }
    }
}
