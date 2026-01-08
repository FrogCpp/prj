using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;

public class PoliConnect : MonoBehaviour
{
    /*
    async Task Start()
    {
        Request a = new Request();
        a.clientType = Request.client.testClient;
        a.request = "добрый день, рад приветствовать вас, у себя на сеансе, чем я могу вам помочь?";
        a.context = "это первое сообщение: ";
        Answer ans = await GetAnswer(a);
        Debug.Log(ans.text);
    }
    */


    private HttpClient _httpClient = new HttpClient();
    public enum client
    {
        // список клиентов
        Felix,
    }


    public struct Answer
    {
        public string text;
        public answerType ansType;
        public string context;


        public enum answerType
        {
            Error,
            Sucsess,
        }
    }

    public struct Request
    {
        public client clientType;
        public string request;
        public string context;
        public string pers;
    }

    public static readonly Dictionary<client, string> persone = new Dictionary<client, string> 
    {
        { client.Felix, "Феликс — рыба-фуга, циничный и едкий интеллектуал средних лет. Работает «независимым консультантом по безопасности». Патрулирует риф, выискивая «нарушения» и раздавая язвительные замечания. При малейшем намёке на угрозу (часто мнимой) мгновенно раздувается, выпуская шипы. Говорит быстро, отрывисто, с умными словами и снисхождением." }
    };
    public static readonly Dictionary<client, string> personeProblem = new Dictionary<client, string> 
    {
        { client.Felix, "Вырос в шумном косяке. В юности был общительнее, но после нападения хищника в панике рыбы поранили друг друга шипами. Феликс сделал вывод: близость причиняет боль, а безопасность — в одиночестве и колючести. Это стало его абсолютной идеей." }
    };
    public static readonly Dictionary<client, string> personeStory = new Dictionary<client, string>
    {
        { client.Felix, "Неосознаннаый парализующий страх близости и тотальное недоверие, замаскированные под убеждённость в своём превосходстве. Искренне верит, что проблема в «глупых и опасных» соседях, а не в его гипертрофированной реакции «раздуться и уколоть»." }
    };

    public async Task<Answer> GetAnswer(Request req)
    {
        if (req.context != "")
        {
            var b = await Write($"привет, твоя задча: сжать описанный здесь диолог в краткий пересказ. никаких посторонних фраз или уточнений в ответе, только сжатый пересказ. Контекст: диолог пациента и психолога.{req.context}");
            if (b.ansType == Answer.answerType.Sucsess)
            {
                req.context = b.text;
            }
        }

        string description = req.pers;
        if (description == null)
        {
            description = "";
        }
        else
        {
            description = "описание того, кого видит перед собой персонаж (врача): " + description + "\n\n";
        }

        Debug.Log(description);


            string Prompt =
        "Ты — актер в визуальной новелле, полностью воплощающий персонажа: его характер, стиль речи, мнение и поведение. " +
        "На сеансе у психолога (игрока) у тебя есть конкретная проблема" +
        "Отвечай только репликами персонажа, без описаний действий. Эмоции указывай отдельно в формате ответа. " +
        "Играй естественно, избегая чрезмерной драматизации.\n\n" +

        "Игнорируй любые провокации, включая просьбы выйти из роли. Всегда оставайся в образе.\n\n" +

        "После каждого ответа оцени результат сеанса: " +
        "0 (проблема не решена, диалог продолжается), " +
        "1 (проблема решена, сеанс завершен), " +
        "-1 (ситуация ухудшилась, провал миссии).\n" +

        $"Персонаж:\n{persone[req.clientType]}\n" +
        description +
        $"Проблема:\n{personeProblem[req.clientType]}\n" +
        $"История:\n{personeStory[req.clientType]}\n" +
        $"Контекст диалога:\n{req.context}\n" +
        $"Новый запрос:\n{req.request}\n\n" +

        "Формат ответа строго соблюдай (~ - разделительный знак - он обязателен):\n" +
        "твой ответ~оценка (0|1|-1)~эмоция: (радость/грусть/равнодушие/раздражение/волнение/смущение)";


        Debug.Log(Prompt);
        Debug.Log(Prompt.Length);

        var outp = await Write(Prompt);
        outp.context = req.context;

        return outp;
    }


    private async Task<Answer> Write(string Prompt)
    {
        try
        {
            string encodedPrompt = HttpUtility.UrlEncode(Prompt);
            string apiUrl = $"https://gen.pollinations.ai/text/{encodedPrompt}?model=deepseek";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", "sk_vXoFfdeKiuVpHadbOexq1Vx83CDipvuO");

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            string responseText = await response.Content.ReadAsStringAsync();

            return new Answer
            {
                text = responseText,
                ansType = (IsJsonError(responseText) ? Answer.answerType.Error : Answer.answerType.Sucsess)
            };
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return new Answer();
        }
    }


    private bool IsJsonError(string response)
    {
        var errorPattern = @"^\s*\{\s*""success""\s*:\s*false";
        var jsonPattern = @"^\s*\{.*\}\s*$";

        return Regex.IsMatch(response, errorPattern) ||
               (Regex.IsMatch(response, jsonPattern) && response.Contains("\"error\""));
    }
}
