using UnityEngine;

[CreateAssetMenu(fileName = "CharacterState", menuName = "HackathonProject/CharacterState")]
public class CharacterState : ScriptableObject
{
    [field: SerializeField] public bool CanMove { get; set; } = true;
    [field: SerializeField] public bool CanExitWhilePlaying { get; set; } = true;
}
