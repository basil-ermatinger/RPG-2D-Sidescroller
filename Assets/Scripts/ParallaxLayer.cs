using System;
using UnityEngine;

namespace Rpg2dSidescroller
{
  [Serializable]
  public class ParallaxLayer
  {
    [SerializeField]
    private Transform _background;

    [SerializeField]
    private float _parallaxMultiplier;

    public void Move(float distanceToMove)
    {
      _background.position +=  Vector3.right * (distanceToMove * _parallaxMultiplier);
    }
  }
}
