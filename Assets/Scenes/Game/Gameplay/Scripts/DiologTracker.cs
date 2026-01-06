using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static PoliConnect;

public class DiologTracker : MonoBehaviour
{
    [SerializeField] private PoliConnect.client client;
    [SerializeField] private TMP_InputField getMsg;
    [SerializeField] private ScrolPanControl scrolPan; 

    private Dictionary<string, string> story = new(); // слева - юзер, с права - нейро
    private PoliConnect _poli;
    private bool update = false;
    private KeyValuePair<string, string> lastPair;

    void Start()
    {
        _poli = GetComponent<PoliConnect>();
    }

    void Update()
    {
        if (update)
        {
            if (story.Count == 0) return;
            var currentLast = story.Last();

            if (!lastPair.Equals(currentLast))
            {
                scrolPan.Write(currentLast.Value);
                lastPair = currentLast;
                update = false;
            }
        }
    }

    public void SendReq()
    {
        update = true;
        string text = getMsg.text;
        getMsg.text = "";

        Request req = new();
        req.clientType = client;
        req.request = text;
        string stry = "начало диолога:\n\n";
        foreach (var a in story)
        {
            stry += "пользователь: " + a.Key + "\n\n";
            stry += "твой предыдущий ответ: " + a.Value + "\n\n";
        }
         
        req.context = stry;

        scrolPan.Write(text);

        var b = Task.Run(() => CompliteReq(req));
    }

    private async Task CompliteReq(Request a)
    {
        Answer ans = await _poli.GetAnswer(a);
        string[] answer;
        if (ans.ansType == Answer.answerType.Error)
        {
            answer = new string[] { "Err: сообщение слишком некорректно" };
        }
        else
        {
            answer = ans.text.Split("[~|~]");
        }
        story.Add(a.request, answer[0]);
    }
}