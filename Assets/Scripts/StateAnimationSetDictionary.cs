using System;
using UnityEngine;

[Serializable]
public class StateAnimationSetDictionary : SerializableDictionary<CharacterState, DirectionalAnimations>
{
    public AnimationClip GetFacingClipFromState(CharacterState characterState, Vector2 facingDirection)
    {
        if (TryGetValue(characterState, out DirectionalAnimations animationClip))
        {
            return animationClip.GetFacingClip(facingDirection);
        }
        else
        {
            Debug.Log($"No animation found for {characterState}");
        }
        //animation fails
        return null;
    }
    
    
}
