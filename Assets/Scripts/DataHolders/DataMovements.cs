using UnityEngine;

namespace SAMODEUS.Movements
{
    [CreateAssetMenu(fileName = "DataMovements", menuName = "Data/Movements")]
    public class DataMovements : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 5;
        [SerializeField] private float _sprintMaxMult = 3;
        [SerializeField] private float _sprintMult = 1;
        [SerializeField] private float _gravity = 9.81f;

        public float MoveSpeed { get { return _moveSpeed; } }
        public float SprintMaxMult { get { return _sprintMaxMult; } }
        public float SprintMult { get { return _sprintMult; } set { _sprintMult = value; } }
        public float Gravity { get { return _gravity; } }
    }
}