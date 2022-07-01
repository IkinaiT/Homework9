using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Homework9
{
    class Program
    {

        //token: "5415590144:AAF00dfQJhKefmgRzL3sAaeepB_JYAcjCsk"
        static TelegramBotClient botClient;
        static string path = "UserData";
        static async Task Main(string[] args)
        {
            string token = "5415590144:AAF00dfQJhKefmgRzL3sAaeepB_JYAcjCsk";
            if (!Directory.Exists("UserData"))
            {
                Directory.CreateDirectory("UserData");
                Console.WriteLine("Папка создана");
            }
            else
            {
                Console.WriteLine("Папка существует");
            }

            botClient = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token    
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                var message = update.Message;
                var messageText = message.Text;
                var messageType = message.Type;
                var chatId = message.Chat.Id;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}. Include {messageType}");

                switch (messageType)
                {
                    case Telegram.Bot.Types.Enums.MessageType.Text:
                        await GetTextMessage(messageText, chatId, cancellationToken);
                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Photo:

                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Video:

                        break;

                    case Telegram.Bot.Types.Enums.MessageType.Document:

                        break;
                    case Telegram.Bot.Types.Enums.MessageType.Audio:

                        break;
                    default:
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "Простите, я не знаю что вы от меня хотите",
                            cancellationToken: cancellationToken);
                        break;
                }
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }

        static async Task GetTextMessage(string messageText, long chatId, CancellationToken cancellationToken)
        {

            Message sentMessage;

            if (messageText.Contains("скач") || messageText.Contains("download"))
            {
                string[] splitedArr = messageText.Split(' ');
                try
                {
                    await using Stream stream = System.IO.File.OpenRead(path + splitedArr[1]);
                    sentMessage = await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: splitedArr[1]),
                        cancellationToken: cancellationToken);
                }
                catch
                {
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Файл не найден!",
                           cancellationToken: cancellationToken);
                }

                return;
            }

            switch (messageText.ToLower())
            {
                case "/start":

                    break;

                case "help":
                case "помощь":

                    break;

                case "8ball":
                case "шар":

                    break;

                case "chance":
                case "шанс":

                    break;

                case "files": 
                case "файлы":

                    break;

                default:

                    break;
            }
            return;
        }

        static async Task GetPhotosMessage()
        {

            return;
        }

        static async Task GetVideoMessage()
        {

            return;
        }

        static async Task GetDocumentMessage()
        {

            return;
        }

        static async Task GetAudioMessage()
        {

            return;
        }
    }
}
