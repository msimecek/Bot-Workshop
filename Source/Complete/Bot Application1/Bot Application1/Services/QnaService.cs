using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Services
{
    public class QnaService
    {
        public string KnowledgeBaseId { get; set; }
        public string SubscriptionKey { get; set; }

        public QnaService(string knowledgeBaseId, string subscriptionKey)
        {
            KnowledgeBaseId = knowledgeBaseId;
            SubscriptionKey = subscriptionKey;
        }


        public async Task<string> QnAMakerQueryAsync(string query)
        {
            using (HttpClient hc = new HttpClient())
            {
                string url = $"https://westus.api.cognitive.microsoft.com/qnamaker/v1.0/knowledgebases/{KnowledgeBaseId}/generateAnswer";
                var content = new StringContent($"{{\"question\": \"{query}\"}}", Encoding.UTF8, "application/json");
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

                var response = await hc.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var answer = JsonConvert.DeserializeObject<QnAMakerResult>(await response.Content.ReadAsStringAsync());

                    if (answer.Score >= 0.3)
                    {
                        return HttpUtility.HtmlDecode(answer.Answer);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new QnAMakerException();
                }
            }
        }

    }

    public class QnAMakerResult
    {
        /// <summary>
        /// The top answer found in the QnA Service.
        /// </summary>
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        /// <summary>
        /// The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }

    public class QnAMakerException : Exception { }

}