using UnityEngine;

namespace Tanks.Configuration
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "Configuration/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField]
        private byte _botsLimit;

        public byte BotsLimit => _botsLimit;

        public void SetBotsLimit(byte value) => _botsLimit = value;
    }
}