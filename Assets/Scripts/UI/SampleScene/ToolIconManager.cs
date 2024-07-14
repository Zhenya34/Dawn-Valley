using UnityEngine;

public class ToolIconManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _image;
    [SerializeField] private Sprite[] _sprites = new Sprite[6];

    public void UpdateToolIcon(int toolType)
    {
        if(toolType == 0)
        {
            _image.sprite = _sprites[0];
        }
        else if(toolType == 1)
        {
            _image.sprite = _sprites[1];
        }
        else if (toolType == 2)
        {
            _image.sprite = _sprites[2];
        }
        else if (toolType == 3)
        {
            _image.sprite = _sprites[3];
        }
        else if (toolType == 4)
        {
            _image.sprite = _sprites[4];
        }
        else if (toolType == 5)
        {
            _image.sprite = _sprites[5];
        }
    }
}
