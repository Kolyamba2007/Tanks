using UnityEngine;

public class GUIIcon : MonoBehaviour
{
    [SerializeField]
    private Sprite _icon;
    private Vector2 Size => Vector3.one;
    private void OnDrawGizmos()
    {
        if (_icon != null)
        {
            Debug.Log(_icon.textureRect);
            var texture = Sprite.Create(_icon.texture, new Rect(0, 0, _icon.texture.width, _icon.texture.height), Vector3.one, 8).texture;
            Gizmos.DrawGUITexture(new Rect((Vector2)transform.position - Size / 2, Size), texture);
        }
    }
}
