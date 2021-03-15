using NapoletanBot.Net.Environment.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telegram.Bot;
using Telegram.Bot.Args;

using msgType = Telegram.Bot.Types.Enums.MessageType;

namespace NapoletanBot.Net.Environment.Code
{
    class TelegramBot
    {
        public static MainWindow mainWindow1;
        public TelegramBot(string token, MainWindow mainWindow)
        {
            mainWindow1 = mainWindow;
            mainWindow.DebugConsole.AppendText("Inside Cstor\n");
            InstanceBot(token);
            mainWindow.DebugConsole.AppendText("Outside Cstor\n");
        }

        public static void InstanceBot(string token)
        {
            mainWindow1.DebugConsole.AppendText("Inside InstanceBot\n");
            try
            {
                var botClient = new TelegramBotClient(token);
                var botInstance = new Instance(botClient);
                botInstance.Listen(true);
            }
            catch
            {
                mainWindow1.DebugConsole.AppendText("Error, enter a valid token.");
            }
        }

        public class Instance
        {
            public static ITelegramBotClient telegramBotClient;
            public Instance(ITelegramBotClient tBotClient)
            {
                tBotClient.OnMessage += Events.OnMessage;
                telegramBotClient = tBotClient;
            }

            public void Listen(bool status)
            {
                if (status)
                {
                    telegramBotClient.StartReceiving();
                }
                else
                {
                    telegramBotClient.StopReceiving();
                }
            }
        }
    }

    class Events
    {
        public static async void OnMessage(object sender, MessageEventArgs msgEvent)
        {
            var botClient = TelegramBot.Instance.telegramBotClient;

            var msgStruct = new MessageStruct(botClient, msgEvent.Message.Chat.Id, msgEvent.Message.MessageId);
            var unallowedMessage = msgEvent.Message switch
            {
                _ when msgEvent.Message.Animation is not null && !Settings.MsgTypes.AnimationAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Video is not null || msgEvent.Message.Photo is not null && !Settings.MsgTypes.MediaAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Audio is not null && !Settings.MsgTypes.AudioAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Contact is not null && !Settings.MsgTypes.ContactAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Document is not null && !Settings.MsgTypes.DocumentAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Game is not null && !Settings.MsgTypes.GameAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Text is not null && !Settings.MsgTypes.TextAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.Sticker is not null && !Settings.MsgTypes.StickerAllowed
                    => Worker.DeleteUnallowedMessage(msgStruct),

                _ when msgEvent.Message.NewChatMembers is not null && Settings.MsgTypes.JoinBanner
                    => Worker.SendJoinBanner(msgEvent.Message.From.Username, await TelegramBot.Instance.telegramBotClient.GetUserProfilePhotosAsync(msgEvent.Message.From.Id)),

                _ => Worker.DoNothing(),
            };
            await unallowedMessage;
        }
    }

    public class MessageStruct
    {
        public ITelegramBotClient BotClient { get; set; }
        public long ChatId { get; set; }
        public int MsgId { get; set; }
        public MessageStruct(ITelegramBotClient botClient, long chatId, int msgId)
        {
            BotClient = botClient;
            ChatId = chatId;
            MsgId = msgId;
        }
    }

    class Worker
    {
        public static async Task DeleteUnallowedMessage(MessageStruct msgStruct)
        {
            await msgStruct.BotClient.DeleteMessageAsync(msgStruct.ChatId, msgStruct.MsgId);
            return;
        }

        public static async Task SendJoinBanner(string user, Telegram.Bot.Types.UserProfilePhotos userPhotos)
        {
            var rand = new Random();
            var fileName = $"{rand.Next(0, 99999)}{rand.Next(0, 99999)}{rand.Next(0, 99999)}";
            using (var image = Image.FromFile("Background.png"))
            {
                var tempBitmap = new Bitmap(image.Width, image.Height);
                using (Graphics g = Graphics.FromImage(tempBitmap))
                {
                    g.DrawImage(image, 0, 0);
                    tempBitmap.Save(fileName+".bmp", ImageFormat.Png);
                }

                var font = new Font("Segoe UI", 40);
                var textSize = ImageService.GetTextSizeF(user, font, System.Drawing.Size.Empty);
                var centeredPos = ImageService.CenterPoint(((int)textSize.Width, (int)textSize.Height / 3), (tempBitmap.Width, tempBitmap.Height / 3));

                await TelegramBot.Instance.telegramBotClient.GetInfoAndDownloadFileAsync(userPhotos.Photos[0][0].FileId, File.OpenWrite($"{fileName}_logo.bmp"));

                using (var profilePic = Image.FromFile($"{fileName}_logo.bmp"))
                {
                    var centeredPosImg = ImageService.CenterPoint((profilePic.Width, profilePic.Height / 2), (profilePic.Width, profilePic.Height / 3));
                    var newImg = ImageService.WriteTextOnImage(tempBitmap, user, font, System.Drawing.Color.FromArgb(19, 19, 19), centeredPos);
                    ImageService.WriteImageOnImage(newImg, profilePic, centeredPosImg).Save(fileName+".bmp");
                }

                using (Stream source = File.OpenRead(fileName + ".bmp"))
                {
                    await TelegramBot.Instance.telegramBotClient.SendPhotoAsync(-1001398644811, new Telegram.Bot.Types.InputFiles.InputOnlineFile(source));
                }
            }
        }

        public static async Task DoNothing() { return;  }
    }

    class Settings
    {
        public static class MsgTypes
        {
            public static bool MediaAllowed { get; set; } = true;
            public static bool AnimationAllowed { get; set; } = true;
            public static bool AudioAllowed { get; set; } = true;
            public static bool ContactAllowed { get; set; } = true;
            public static bool DocumentAllowed { get; set; } = true;
            public static bool GameAllowed { get; set; } = true;
            public static bool TextAllowed { get; set; } = true;
            public static bool StickerAllowed { get; set; } = true;
            public static bool JoinBanner { get; set; } = true;
        }
    }
}
