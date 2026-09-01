using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheet 
{
    [CreateAssetMenu(fileName = "NewCharacterStateCard", menuName = "Data/CharacterState")]
    public class CharacterStateCard : ScriptableObject
    {
        [SerializeField] [InlineProperty] [HideLabel] 
        private CharacterState state;
        public CharacterState State { get => state; set => state = value; }
    }
}
