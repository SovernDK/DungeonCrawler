using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGUIElement
{
    public void Initialize(CoreContext context);
    public void Draw();
}
