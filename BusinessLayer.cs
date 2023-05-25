using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot_Project
{
    public class BusinessLayer
    {
        public long Init(long ChatId)
        {
            using (CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext db = new CUsersVasilSourceReposTelegramBotTelegramBotChatMdfContext())
            {
                var ChSel = db.Chat.Where(c => c.ChatId == ChatId).FirstOrDefault();

                if (null == ChSel)
                {
                    Chat Ch = new Chat();
                    Ch.ChatId = ChatId;
                    Ch.Money = 5000;
                    
                    db.Chat.Add(Ch);

                    db.SaveChanges();

                    return (long)Ch.Id;
                }
                

                return (long)ChSel.Id;
            }
        }
    }
}
