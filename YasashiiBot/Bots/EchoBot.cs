// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Net.Http;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System.Diagnostics;
using CoreTweet;

namespace YasashiiBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private static readonly string key = "c5551c1e1bed4a98b6c91154c89610ee";
        private static readonly string endpoint = "https://westus2.api.cognitive.microsoft.com/";
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var replyText = $"Echo: {turnContext.Activity.Text}";
            //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            //For TextAnalytics
            var client = authenticateClient();
            var score = sentimentAnalysisExample(client, turnContext.Activity.Text);
            var res = "";//返事格納用
            //Debug.WriteLine($"Sentiment Score2: {score:0.00}");
            /*
            if (0.5 <= score)
            {
                res = getShioComment();
                Debug.WriteLine($"塩対応:{res}");//出力確認3
            }
            else
            {
                res = getTweet();
                Debug.WriteLine($"励まし対応:{res}");//出力確認4
            }
            */
            res = getTweet();
            Debug.WriteLine($"励まし対応:{res}");//出力確認4

            //res = $"Score:{score:0.00}" + Environment.NewLine + res;
            res = Environment.NewLine + res;
            await turnContext.SendActivityAsync(MessageFactory.Text(res), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        //For TextAnalytics
        static TextAnalyticsClient authenticateClient()
        {
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(key);
            TextAnalyticsClient client = new TextAnalyticsClient(credentials)
            {
                Endpoint = endpoint
            };
            return client;
        }

        //For TextAnalytics
        static double sentimentAnalysisExample(ITextAnalyticsClient client, string message)//戻り値をvoidからdouble型に変更
        {
            var result = client.Sentiment(message, "ja");//引数messageを分析対象にし、言語は日本語に設定
            Debug.WriteLine($"User Message: {message}");//引数messageの内容を出力
            Debug.WriteLine($"Sentiment Score1: {result.Score:0.00}");//出力確認1
            return (double)result.Score;//分析スコアを返す
        }

        static string getTweet()
        {
            Random cRandom = new System.Random(); //取得したツイートからランダムに1つ選択するための乱数
            var tokens = Tokens.Create("2sDVuWIkQBkLh6v4QCfvwSTQx", "a8mj3uIKNswujUORpvGrwEiM2JjVyZKxEo0WRQRwzJBeOPARL4", "3289430005-dsIgaJ2YCGLz02MoIVPSsInf1MRhfNevnmVF38Y", "ybTxbLDJAtPGvPKS2bpW9Ep2HSo4orRYc7hjgtGYAXytZ");  //接続用トークン発行
            var tweet = "";//取得したツイートを格納する変数

            var parm = new Dictionary<string, object>();  //条件指定用Dictionary
            parm["count"] = 60;  //取得するツイート数
            parm["screen_name"] = "hagemasi1_bot";  //取得したいユーザーID


            Task task = Task.Factory.StartNew(async () =>
            {
                var tweets = await tokens.Statuses.UserTimelineAsync(parm); //parmの内容に従ってツイートを取得

                var random = cRandom.Next(61); //0～60の間の乱数を生成
                tweet = tweets[random].Text; //取得した60ツイートからrandom番目のツイートを格納

            }).Unwrap();

            task.Wait();

            return tweet; //選んだツイートを戻り値として返す
        }

        static string getShioComment()
        {
            Random cRandom = new System.Random(); //乱数
            string res = "";
            var shio = new string[] { "へー・・・。", "・・・だから？", "知らんわー。", "興味ないね。", "いや、聞いてないし。", "ふーん・・・。で？", "そういうのいいから。", "あーちょっと今忙しいからまた今度。", "ごめん疲れてるの","はあ","おやすみー！","Mなの？","うるさいなあ","...", "...", "...", "...","しつこい", "しつこい" };

            var random = cRandom.Next(11);
            res = shio[random];
            return res;
        }
    }

    //For TextAnalytics
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string apiKey;

        public ApiKeyServiceClientCredentials(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            request.Headers.Add("Ocp-Apim-Subscription-Key", this.apiKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);

        }
    }
}
