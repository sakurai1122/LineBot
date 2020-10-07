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
            var res = "";//�Ԏ��i�[�p
            //Debug.WriteLine($"Sentiment Score2: {score:0.00}");
            /*
            if (0.5 <= score)
            {
                res = getShioComment();
                Debug.WriteLine($"���Ή�:{res}");//�o�͊m�F3
            }
            else
            {
                res = getTweet();
                Debug.WriteLine($"��܂��Ή�:{res}");//�o�͊m�F4
            }
            */
            res = getTweet();
            Debug.WriteLine($"��܂��Ή�:{res}");//�o�͊m�F4

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
        static double sentimentAnalysisExample(ITextAnalyticsClient client, string message)//�߂�l��void����double�^�ɕύX
        {
            var result = client.Sentiment(message, "ja");//����message�𕪐͑Ώۂɂ��A����͓��{��ɐݒ�
            Debug.WriteLine($"User Message: {message}");//����message�̓��e���o��
            Debug.WriteLine($"Sentiment Score1: {result.Score:0.00}");//�o�͊m�F1
            return (double)result.Score;//���̓X�R�A��Ԃ�
        }

        static string getTweet()
        {
            Random cRandom = new System.Random(); //�擾�����c�C�[�g���烉���_����1�I�����邽�߂̗���
            var tokens = Tokens.Create("2sDVuWIkQBkLh6v4QCfvwSTQx", "a8mj3uIKNswujUORpvGrwEiM2JjVyZKxEo0WRQRwzJBeOPARL4", "3289430005-dsIgaJ2YCGLz02MoIVPSsInf1MRhfNevnmVF38Y", "ybTxbLDJAtPGvPKS2bpW9Ep2HSo4orRYc7hjgtGYAXytZ");  //�ڑ��p�g�[�N�����s
            var tweet = "";//�擾�����c�C�[�g���i�[����ϐ�

            var parm = new Dictionary<string, object>();  //�����w��pDictionary
            parm["count"] = 60;  //�擾����c�C�[�g��
            parm["screen_name"] = "hagemasi1_bot";  //�擾���������[�U�[ID


            Task task = Task.Factory.StartNew(async () =>
            {
                var tweets = await tokens.Statuses.UserTimelineAsync(parm); //parm�̓��e�ɏ]���ăc�C�[�g���擾

                var random = cRandom.Next(61); //0�`60�̊Ԃ̗����𐶐�
                tweet = tweets[random].Text; //�擾����60�c�C�[�g����random�Ԗڂ̃c�C�[�g���i�[

            }).Unwrap();

            task.Wait();

            return tweet; //�I�񂾃c�C�[�g��߂�l�Ƃ��ĕԂ�
        }

        static string getShioComment()
        {
            Random cRandom = new System.Random(); //����
            string res = "";
            var shio = new string[] { "�ց[�E�E�E�B", "�E�E�E������H", "�m����[�B", "�����Ȃ��ˁB", "����A�����ĂȂ����B", "�Ӂ[��E�E�E�B�ŁH", "���������̂�������B", "���[������ƍ��Z��������܂����x�B", "���߂���Ă��","�͂�","���₷�݁[�I","M�Ȃ́H","���邳���Ȃ�","...", "...", "...", "...","������", "������" };

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
