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
        static string path = @"UserData";
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

                Message sentMessage;

                int fileNums = Directory.GetFiles(path).Length;

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}. Include {messageType}");

                if (messageType == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    await GetTextMessage(messageText, chatId, cancellationToken);
                }
                else if (messageType == Telegram.Bot.Types.Enums.MessageType.Photo)
                {
                    var fileID = update.Message.Photo[0].FileId;
                    var fileInfo = await botClient.GetFileAsync(fileID);
                    var filePath = fileInfo.FilePath;

                    string[] fileExtension = filePath.Split('/')[1].Split('.');

                    

                    string destinationFilePath = $"{path}/{fileExtension[0]}.{fileExtension[1]}";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                    await botClient.DownloadFileAsync(
                        filePath: filePath,
                        destination: fileStream);

                    fileNums++;

                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Выполнено",
                           cancellationToken: cancellationToken);
                }
                else if (messageType == Telegram.Bot.Types.Enums.MessageType.Video)
                {
                    var fileID = update.Message.Video.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileID);
                    var filePath = fileInfo.FilePath;

                    string[] fileExtension = filePath.Split('/')[1].Split('.');



                    string destinationFilePath = $"{path}/{fileExtension[0]}.{fileExtension[1]}";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                    await botClient.DownloadFileAsync(
                        filePath: filePath,
                        destination: fileStream);

                    fileNums++;

                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Выполнено",
                           cancellationToken: cancellationToken);
                }
                else if (messageType == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    var fileID = update.Message.Document.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileID);
                    var filePath = fileInfo.FilePath;

                    string[] fileExtension = filePath.Split('/')[1].Split('.');



                    string destinationFilePath = $"{path}/{fileExtension[0]}.{fileExtension[1]}";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                    await botClient.DownloadFileAsync(
                        filePath: filePath,
                        destination: fileStream);

                    fileNums++;

                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Выполнено",
                           cancellationToken: cancellationToken);
                }
                else if (messageType == Telegram.Bot.Types.Enums.MessageType.Audio)
                {
                    var fileID = update.Message.Audio.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileID);
                    var filePath = fileInfo.FilePath;

                    string[] fileExtension = filePath.Split('/')[1].Split('.');



                    string destinationFilePath = $"{path}/{fileExtension[0]}.{fileExtension[1]}";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                    await botClient.DownloadFileAsync(
                        filePath: filePath,
                        destination: fileStream);

                    fileNums++;

                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Выполнено",
                           cancellationToken: cancellationToken);
                }
                else
                {
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Простите, я еще не умею работать с файлами данного типа(((",
                           cancellationToken: cancellationToken);
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
                    await using Stream stream = System.IO.File.OpenRead($@"../net5.0/{path}/{splitedArr[1]}");
                    sentMessage = await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputOnlineFile(content: stream, fileName: splitedArr[1]),
                        cancellationToken: cancellationToken);
                }
                catch
                {
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Файл не найден или пуст!",
                           cancellationToken: cancellationToken);
                }

                return;
            }
            Random rnd = new Random();
            string[] textArr = messageText.Split(' ');
            string otherText = "";
            for (int i = 1; i < textArr.Length; i++)
                otherText += $" {textArr[i]}";

            switch (textArr[0].ToLower())
            {
                case "/start":
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Привет! Вот что я умею:\n" +
                                 "help или помощь - напоминю что я умею\n" +
                                 "8ball или шар [текст] - потрясу волшебный шар\n" +
                                 "chance или шанс [текст] - шанс на что угодно\n" +
                                 "files или файлы - список файлов, доступных для скачивания\n" +
                                 "download или скач [имя файла] - скачать файл, если такой, конечно, существует\n" +
                                 "Также можете отправить файлы и я их сохраню чтобы Вы могли их скачать позже. ",
                           cancellationToken: cancellationToken);
                    break;

                case "help":
                case "помощь":
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: "Вот что я умею:\n" +
                                 "help или помощь - напоминю что я умею\n" +
                                 "8ball или шар [текст] - потрясу волшебный шар\n" +
                                 "chance или шанс [текст] - шанс на что угодно\n" +
                                 "files или файлы - список файлов, доступных для скачивания\n" +
                                 "download или скач [имя файла] - скачать файл, если такой, конечно, существует\n" +
                                 "Также можете отправить файлы и я их сохраню чтобы Вы могли их скачать позже",
                           cancellationToken: cancellationToken);
                    break;

                case "8ball":
                case "шар":
                    string[] answers = {"Бесспорно",
                                        "Предрешено",
                                        "Никаких сомнений",
                                        "Определённо да",
                                        "Можешь быть уверен в этом",
                                        "Мне кажется — «да»",
                                        "Вероятнее всего",
                                        "Хорошие перспективы",
                                        "Знаки говорят — «да»",
                                        "Да",
                                        "Пока не ясно, попробуй снова",
                                        "Спроси позже",
                                        "Лучше не рассказывать",
                                        "Сейчас нельзя предсказать",
                                        "Сконцентрируйся и спроси опять",
                                        "Даже не думай",
                                        "Мой ответ — «нет»",
                                        "По моим данным — «нет»",
                                        "Перспективы не очень хорошие",
                                        "Весьма сомнительно"};
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: answers[rnd.Next(0, 19)],
                           cancellationToken: cancellationToken);
                    break;

                case "chance":
                case "шанс":
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Ваш шанс на{otherText}: {rnd.Next(0, 100)}%",
                           cancellationToken: cancellationToken);
                    break;

                case "files": 
                case "файлы":
                    string files = "";
                    string[] filesArr = Directory.GetFiles(path + "/");

                    foreach (string s in filesArr)
                    {
                        
                        files += s.Remove(0, path.Length + 1) + "\n";
                    }
                        

                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Список файлов доступных для скачивания:\n" + files,
                           cancellationToken: cancellationToken);
                    break;

                default:
                    sentMessage = await botClient.SendTextMessageAsync(
                           chatId: chatId,
                           text: $"Простите, но когда-то я научусь вас понимать(((",
                           cancellationToken: cancellationToken);
                    break;
            }
            otherText = "";
            return;
        }
    }
}
