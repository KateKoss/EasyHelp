using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcApplication1.Filters;
using System.Text;
using System.IO;
using MvcApplication1.Contexts;

namespace MvcApplication1.Controllers
{
    public class ChatController : Controller
    {
        static ChatModel chatModel;

        /// <summary>
        /// When the method is called with no arguments, just return the view
        /// When argument logOn is true, a user logged on
        /// When argument logOff is true, a user closed their browser or navigated away (log off)
        /// When argument chatMessage is specified, the user typed something in the chat
        /// </summary>
        public ActionResult Index(string user, bool? logOn, bool? logOff, string chatMessage)
        {
            ////logOn = true;
            //RequestsList requestsModel = new RequestsList();
            ////user = "user2";
            //try
            //{
            //    if (chatModel == null) chatModel = new ChatModel();

            //    //trim chat history if needed
            //    if (chatModel.ChatHistory.Count > 100)
            //        chatModel.ChatHistory.RemoveRange(0, 90);

            //    if (!Request.IsAjaxRequest())
            //    {
            //        //first time loading
            //        return View(chatModel);
            //    }
            //    else 
            //    if (logOn != null && (bool)logOn)
            //    {
                    
            //        //string currentPerson;
            //        //if (Request.Cookies["UserId"] != null)
            //        //    currentPerson = Convert.ToString(Request.Cookies["UserId"].Value);
            //        //else currentPerson = "user1";
            //        //using (CustomDbContext db = new CustomDbContext())
            //        //{
            //        //    DateTime date = DateTime.Now;
            //        //    date.AddMinutes(-2);
            //        //    var r = db.RequestsModel.Where(x => x.createdAt == DateTime.Today);
            //        //    if (r != null)
            //        //        foreach (var item in r)
            //        //        {
            //        //            requestsModel.reqests.Add(item);
            //        //        }
            //        //    chatMessage = requestsModel.reqests.Count.ToString();
            //        //}
                    

            //        //return View("StudentRequests", requestsModel.reqests);

            //        //check if nickname already exists
            //        if (chatModel.Users.FirstOrDefault(u => u.NickName == user) != null)
            //        {
            //            throw new Exception("Такий нік вже існує");
            //        }
            //        else 
            //        if (chatModel.Users.Count > 100)
            //        {
            //            throw new Exception("Кімната повна!");
            //        }
            //        else
            //        {
            //            #region create new user and add to lobby
            //            chatModel.Users.Add(new ChatModel.ChatUser()
            //            {
            //                NickName = user,
            //                LoggedOnTime = DateTime.Now,
            //                LastPing = DateTime.Now
            //            });

            //            //inform lobby of new user
            //            chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
            //            {
            //                Message = "Користувач '" + user + "' ввійшов в чат.",
            //                When = DateTime.Now
            //            });
            //            #endregion

            //        }

            //        return PartialView("Lobby", chatModel);
            //    }
            //    else if (logOff != null && (bool)logOff)
            //    {
            //        LogOffUser(chatModel.Users.FirstOrDefault(u => u.NickName == user));
            //        return PartialView("Lobby", chatModel);
            //    }
            //    else
            //    {

            //        ChatModel.ChatUser currentUser = chatModel.Users.FirstOrDefault(u => u.NickName == user);

            //        //remember each user's last ping time
            //        currentUser.LastPing = DateTime.Now;

            //        #region remove inactive users
            //        List<ChatModel.ChatUser> removeThese = new List<ChatModel.ChatUser>();
            //        foreach (Models.ChatModel.ChatUser usr in chatModel.Users)
            //        {
            //            TimeSpan span = DateTime.Now - usr.LastPing;
            //            if (span.TotalSeconds > 15)
            //                removeThese.Add(usr);
            //        }
            //        foreach (ChatModel.ChatUser usr in removeThese)
            //        {
            //            LogOffUser(usr);
            //        }
            //        #endregion

            //        #region if there is a new message, append it to the chat
            //        if (!string.IsNullOrEmpty(chatMessage))
            //        {
            //            chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
            //            {
            //                ByUser = currentUser,
            //                Message = chatMessage,
            //                When = DateTime.Now
            //            });
            //        }
            //        #endregion

            //        return PartialView("ChatHistory", chatModel);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //return error to AJAX function
            //    Response.StatusCode = 500;
            //    return Content(ex.Message);
            //}
            return PartialView("ChatSmallBox");
        }

        ///// <summary>
        ///// Remove this user from the lobby and inform others that he logged off
        ///// </summary>
        ///// <param name="user"></param>
        //public void LogOffUser(ChatModel.ChatUser user)
        //{
        //    chatModel.Users.Remove(user);
        //    chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
        //    {
        //        Message = "Користувач '" + user.NickName + "' вийшов.",
        //        When = DateTime.Now
        //    });
        //}

        public ActionResult LoadChatHistory(ChatMessageModel data)
        {
            List<ChatMessageModel> modelList = new List<ChatMessageModel>();
            using(var db = new CustomDbContext())
            {
                var list = db.ChatMessages.Where(x => (x.FromUser == data.FromUser || x.ToUser == data.FromUser) && (x.FromUser == data.ToUser || x.ToUser == data.ToUser)).ToList();
                foreach (var item in list)
		            modelList.Add(item);      
            }            
            return Json(modelList);
        }

        public ActionResult IsMessageRead(ChatMessageModel data)
        {
            using (var db = new CustomDbContext())
            {
                var list = db.ChatMessages.Where(x => (x.FromUser == data.FromUser || x.ToUser == data.FromUser) && (x.FromUser == data.ToUser || x.ToUser == data.ToUser)).ToList();
                foreach (var item in list)
                    item.isMessageSent = true;
                
                db.SaveChanges();
            }       

            return Content("");
        }

        public ActionResult ChatFullScreen()
        {
            return View("ChatFullScreen");
        }

        //get list of people with who user has been cominicated
        public ActionResult GetInterlocutorsList()
        {
            return PartialView("InterlocutorsList", new List<string>());
        }
    }
}
