using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundManager
{
    ButtonPress,
    DialogueInstantiation1,
    DialogueInstantiation2,
    RoundEnd,
}

public class SoundAndAnimation : MonoBehaviour, ISoundAndAnimation
{
    public AudioClip ButtonPress;
    public AudioClip DialogueInstantiation1;
    public AudioClip DialogueInstantiation2;
    public AudioClip RoundEnd;

    public AudioSource audioSource;

    public void PlayAnimation()
    {
        
    }

    public void PlaySound(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio); // Plays the sound without interrupting other sounds
        }
    }

    public void SwitchOnSoundType(SoundManager soundManager)
    {
        switch (soundManager)
        {
            case SoundManager.ButtonPress:
                PlaySound(ButtonPress);
                break;
            case SoundManager.DialogueInstantiation1:
                PlaySound(DialogueInstantiation1);
                break;
            case SoundManager.DialogueInstantiation2:
                PlaySound(DialogueInstantiation2);
                break;
            case SoundManager.RoundEnd:
                PlaySound(RoundEnd);
                break;
        }
    }
}
