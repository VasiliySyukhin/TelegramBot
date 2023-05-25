using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;
using System.Security.Claims;

namespace Telegram_Bot_Project
{
    public class ButtonProcessing
    { 
        public async Task KeyboardButtonHandlingg(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Message message)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
                if (callbackQuery.Data == "category")
                {
                    
                    using (CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext db = new CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext())
                    {

                        Catalog Pr = new Catalog();
                        decimal id = 0;
                        CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext context = new CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext();
                        Catalog catalog = context.Catalogs.Find(id = 0);
                        Catalog catalog1 = context.Catalogs.Find(id = 1);
                        var productname = catalog.ProductName;
                        var productname1 = catalog1.ProductName;
                        var price = catalog.Price.ToString();
                        var price1 = catalog1.Price.ToString();


                        {
                            InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                            // first row
                            new []

                            {
                            InlineKeyboardButton.WithCallbackData(text: productname + " " + price + "руб", callbackData: "product"),
                            },
                            // second row
                            new []
                            {
                            InlineKeyboardButton.WithCallbackData(text: productname1 + " " + price1 + "руб", callbackData: "product1"),
                            },
                            });

                            Message sentMessage = await botClient.SendTextMessageAsync(
                                callbackQuery.Message.Chat.Id,
                                text: "Цена",
                            replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);

                            await botClient.DeleteMessageAsync(
                           callbackQuery.Message.Chat.Id,
                           callbackQuery.Message.MessageId);

                        }

                        
                    }

                }
                if(callbackQuery.Data == "product")
                {
                    await botClient.DeleteMessageAsync(
                           callbackQuery.Message.Chat.Id,
                           callbackQuery.Message.MessageId);

                    CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext context = new CUsersVasilSourceReposTelegramBotProjectTelegramBotProjectCatalogMdfContext();
                    CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext context1 = new CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext();
                    CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext db = new CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext();
                    decimal id = 0;
                    //var ChatIdUser = context1.Chat.Where(c => c.ChatId == callbackQuery.Message.Chat.Id).FirstOrDefault();
                    decimal UserId = callbackQuery.Message.Chat.Id;
                    Catalog catalog = context.Catalogs.Find(id);
                    Chat chat = context1.Chat.FirstOrDefault(x => x.ChatId == UserId);          
                    var bank = chat.Money;
                    var price = catalog.Price;
                    var baza = catalog.LoginPassword;
                    if (bank >= price)
                    {
                        Console.WriteLine("Ok");
                        chat.Money = chat.Money - price;
                        db.Chat.Update(chat);
                        db.SaveChanges();
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: callbackQuery.Message.Chat.Id,
                            text: baza,
                            cancellationToken: cancellationToken);
                        context.Catalogs.Remove(catalog);
                        context.SaveChanges();



                    }
                    else
                       {
                         InlineKeyboardMarkup inlineKeyboard = new(new[]
                       {
                    InlineKeyboardButton.WithUrl(
                    text: "У вас нехватает средств",
                    url: "https://github.com/TelegramBots/Telegram.Bot")
                    });

                            Message sentMessage = await botClient.SendTextMessageAsync(
                                callbackQuery.Message.Chat.Id,
                                text: "A message with an inline keyboard markup",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);
                        }
                    
                }
                if (callbackQuery.Data == "qiwi")
                {
                    AccountReplenishment.CreateForm(5);

                   InlineKeyboardMarkup inlineKeyboard = new(new[]
                                        {
                    InlineKeyboardButton.WithUrl(
                    text: "Оплатите",
                    url: AccountReplenishment.CreateForm(5))
                    });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        text: "A message with an inline keyboard markup",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                }
            }

        }
    }
}
