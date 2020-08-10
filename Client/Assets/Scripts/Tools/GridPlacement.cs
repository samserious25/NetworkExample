using UnityEngine;

//Хэлпер для размещения по сетке по иксу, юзается только в пределах юнити

public class GridPlacement : MonoBehaviour
{
    public enum PlacementType
    {
        Horizontal,
        Vertical
    }

    public PlacementType placementType;
    public float step = 2.2f;
    public float offset = 0.5f;

    public void PlaceObjects()
    {
        float pos = (-transform.childCount / 2) + 1 - offset;
        float horizontal = 0f;
        float vertical = 0f;

        switch (placementType)
        {
            case PlacementType.Horizontal:
                horizontal = pos * step;
                vertical = transform.position.y;
                break;
            case PlacementType.Vertical:
                horizontal = transform.position.x;
                vertical = pos * step;
                break;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(horizontal, vertical, 0f);
            pos += 1f;
        }
    }
}
