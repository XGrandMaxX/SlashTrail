using UnityEngine;

namespace Develop._Scripts.Enemy
{
    public class PlayerMoveImitation : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.position += new Vector3(1, 0, 0) * (_speed * Time.deltaTime);
        }
    }
}
