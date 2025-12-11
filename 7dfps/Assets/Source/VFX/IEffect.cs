using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IEffect
{
    public Action OnEffectComplete { get; set; }

    public GameObject Parent { get; set; }
    public float Duration { get; set; }
    public bool IsPlaying { get; set; }
}
