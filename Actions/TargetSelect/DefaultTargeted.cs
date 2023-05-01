using UnityEngine;

public class DefaultTargeted : ITargeted
{
    public void SetOutline(Outline mesh, bool isActive, Color color, float width)
    {
        mesh.enabled = isActive;
        mesh.OutlineColor = color;
        mesh.OutlineWidth = width;
    }
}
