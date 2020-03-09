using System;
using UnityEngine;

namespace _Scripts
{
    public static class Utils
    {
        /// <summary>
        /// Returns road piece type
        /// </summary>
        /// <param name="roadPiece">Road piece gameobject</param>
        /// <returns>Type of road</returns>
        internal static int GetRoadType(GameObject roadPiece)
        {
            int.TryParse(roadPiece.name.Split('(')[0], out int roadType);
            return roadType;
        }
    }
}