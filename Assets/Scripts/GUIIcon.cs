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
            Gizmos.DrawGUITexture(new Rect((Vector2)transform.position - Size / 2, Size), _icon.texture);
        }
    }
}
