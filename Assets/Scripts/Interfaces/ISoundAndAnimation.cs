using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundAndAnimation
{
    public void SwitchOnSoundType(SoundManager soundManager);
    public void PlaySound(AudioClip audio);
    public void PlayAnimation();
}
