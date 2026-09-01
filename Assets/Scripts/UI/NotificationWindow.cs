using System.Collections;
using DG.Tweening;
using AtlasGUI;
using TMPro;
using UnityEngine;

public class NotificationWindow : Window
{
    public GameObject content;
    public GameObject item;

    public float fadeOutTime;

    public override void Initialize(CoreContext context)
    {
        base.Initialize(context);

        Gamemaster.Instance.gamelog.onAddEntry += Draw;
    }

    public override void Draw()
    {
        base.Draw();

        GameObject go = Instantiate(item, content.transform);
        string notification = Gamemaster.Instance.gamelog.Pull();
        StartCoroutine(ShowNotification(go, notification));
    }

    public IEnumerator ShowNotification(GameObject item, string notification)
    {
        item.GetComponent<TMP_Text>().text = notification;
        Color endColor = item.GetComponent<TMP_Text>().color;
        endColor.a = 0;
        
        yield return item.GetComponent<TMP_Text>().DOColor(endColor, fadeOutTime).SetEase(Ease.InOutSine).WaitForCompletion();

        Destroy(item);
    }
}
