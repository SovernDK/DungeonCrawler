using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    [SerializeField] private Database database;

    private string cursorType = "Default";

    public string CursorType { get => cursorType; set => cursorType = value; }

    public void Initialize()
    {
        SetCursor("Default");
    }

    public void SetCursor(string name)
    {
        CursorEntry cursorEntry = database.cursors.Find(cursor => cursor.name == name);
        Cursor.SetCursor(cursorEntry.texture, cursorEntry.hotSpot, CursorMode.Auto);
        cursorType = name;
    }

    Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (!sprite.texture.isReadable)
        {
            Debug.LogError("Sprite texture is not readable. Please enable 'Read/Write Enabled' in the import settings.");
            return null;
        }

        int x = (int)sprite.textureRect.x;
        int y = (int)sprite.textureRect.y;
        int width = (int)sprite.textureRect.width;
        int height = (int)sprite.textureRect.height;

        Texture2D newText = new Texture2D(width, height);
        
        Color[] pixels = sprite.texture.GetPixels(x, y, width, height);
        if (pixels.Length != width * height)
        {
            Debug.LogError("Mismatch in the size of the color array and the new texture's dimensions.");
            return null;
        }
        newText.SetPixels(pixels);
        newText.Apply();

        return newText;
    }

}