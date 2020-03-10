using _Scripts.BuildingGeneration.Parts;
using JetBrains.Annotations;
using UnityEngine;

namespace _Scripts.BuildingGeneration
{
    /// <summary>
    /// <T>: Terminal
    /// T: non-Terminal
    /// Building Grammar:
    /// <Building>:=<Wings>
    /// <Wings>:=<Wing>|<Wing><Wings>
    /// <Wing>:=<Stories><Roof>
    /// <Stories>:=<Story>|<Story><Stories>
    /// <Roof>:=Point|Peak|Slope|Flat
    /// <Walls>:=<Wall>|<Wall><Walls>
    /// <Wall>:=SimpleWall|Window|Door
    /// </summary>
    public class BuildingGenerator
    {
       
    }
}