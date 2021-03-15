using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
            InstanceBot(token);
        }

        public static void InstanceBot(string token)
        {
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
        }
    }
}
