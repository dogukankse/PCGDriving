using System;
using System.Collections.Generic;

namespace _Scripts
{
    public class Strings
    {
        private static Dictionary<String,String> strings = new Dictionary<string, string>()
        {
            {TrafficSystemVehiclePlayer.SIDEWALK_PENALTY,"Kaldırım ihlali cezası"},
            {TrafficSystemVehiclePlayer.CRASH_PENALTY,"Trafik kazası cezası"},
            {TrafficSystemVehiclePlayer.CAR_CRASH_PENALTY,"Araca çarpma cezası"},
            {TrafficSystemVehiclePlayer.RED_LIGHT_PENALTY,"Kırmızı ışık cezası"},
            {TrafficSystemVehiclePlayer.LANE_SWITCH_PENALTY,"Şerit ihlali cezası"},
        };

        public static String get(String id)
        {
            return strings[id];
        }
    }
}