/// <summary>
/// 
/// Traffic system vehicle player.
/// 
/// All you have to do is drop the "TrafficSystemVehiclePlayer.cs" script onto any part of your player vehicle that has a collider on it,
/// or to be more correct, any part of your vehicle that you "want" to collider with the Traffic System vehicles.
/// 
/// The colliders for the player can be collision or triggers, the script will detect both. The Traffic System vehicles will then detect
/// that a player controlled vehicle has collidered with them and they will brake and use the correct physics for collision.
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Linq;

public class TrafficSystemVehiclePlayerAuto : TrafficSystemVehicle
{
}