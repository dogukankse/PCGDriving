using System;
using System.Collections.Generic;
using System.Linq;
using UnityScript.Steps;

[Serializable]
public class Tile
{
    public int id;
    public string leftS;
    public string downS;
    public List<int> left;
    public List<int> down;
    private byte[] pattern;

   
}