using System;
using System.Collections.Generic;

namespace _Scripts
{
    public class PenaltyPoints
    {
        private static Dictionary<String,int> penatlies = new Dictionary<string, int>()
        {
            {TrafficSystemVehiclePlayer.SIDEWALK_PENALTY,5},
            {TrafficSystemVehiclePlayer.CRASH_PENALTY,100},
            {TrafficSystemVehiclePlayer.CAR_CRASH_PENALTY,100},
            {TrafficSystemVehiclePlayer.RED_LIGHT_PENALTY,20},
            {TrafficSystemVehiclePlayer.LANE_SWITCH_PENALTY,15},
        };

        public static int get(String type)
        {
            return penatlies[type];
        }

    }
}