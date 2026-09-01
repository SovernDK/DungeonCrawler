using UnityEngine;

public interface IAtlasGuiList 
{
    public void Initialize(CoreContext context, Transform content);
    public void Draw();
}