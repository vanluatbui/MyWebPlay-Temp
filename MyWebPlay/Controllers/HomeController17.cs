﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyWebPlay.Extension;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Xml.Linq;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult Share_Karaoke(string? id)
        {
            //System.IO.File.WriteAllText("D:/XemCode/ma.txt", id);
            if (id != null)
            {
                var noidung = id.Split("-.-", StringSplitOptions.RemoveEmptyEntries);

                if (noidung.Length == 4)
                {
                    var url = (id.Contains("[NONE]") == false) ? StringMaHoaExtension.Decrypt(noidung[0]) : noidung[0].Replace("[NONE]", "");
                    var baihat = (id.Contains("[NONE]") == false) ? StringMaHoaExtension.Decrypt(noidung[1]) : noidung[1].Replace("[NONE]", "");
                    var background = (id.Contains("[NONE]") == false) ? StringMaHoaExtension.Decrypt(noidung[2]) : noidung[2];
                    var option = int.Parse(noidung[3]);

                    //System.IO.File.WriteAllText("D:/XemCode/ma.txt", url);

                    TempData["url"] = url;
                    TempData["baihat"] = baihat;
                    TempData["background"] = background;
                    TempData["option"] = option;
                }
            }
            return RedirectToAction("PlayKaraokeX");
        }

        public ActionResult SendMailSave (string? email, string message)
        {
            Calendar x = CultureInfo.InvariantCulture.Calendar;

            string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string name = "[IP Khách : " + Request.HttpContext.Connection.RemoteIpAddress + ":" + Request.HttpContext.Connection.RemotePort + " | IP máy chủ : " + Request.HttpContext.Connection.LocalIpAddress + ":" + Request.HttpContext.Connection.LocalPort + "] - " + xuxu;
            string host = "{" + Request.Host.ToString() + "}"
                       .Replace("http://", "")
                   .Replace("https://", "")
                   .Replace("/", "");

            var loi = 0;
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(email))
                {
                    email = "mywebplay.savefile@gmail.com";
                }

                SendEmail.SendMail2Step("mywebplay.savefile@gmail.com",
   email, host + " Quick Send Text Mail By Url To Save In " + name, message, "teinnkatajeqerfl");
            }
            catch(Exception ex) 
            {
                loi = 1;
            }
            finally
            {
                if (email != "mywebplay.savefile@gmail.com")
                {
                    var err = (loi == 0) ? " - SUCCESS # "+email : " - ERROR # "+email;
                    SendEmail.SendMail2Step("mywebplay.savefile@gmail.com",
                    "mywebplay.savefile@gmail.com", host + " [THONG BAO ADMIN" + err + "] Quick Send Text Mail By Url To Save In " + name, message, "teinnkatajeqerfl");
                }
            }

            return Redirect("https://google.com");
        }
    }
}
