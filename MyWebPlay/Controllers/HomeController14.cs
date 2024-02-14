﻿using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using System.Drawing;
using System;
using System.Formats.Tar;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using MyWebPlay.Extension;
using Microsoft.Extensions.Hosting;
using MyWebPlay.Model;
using Microsoft.AspNetCore.Http;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult PlayKaraoke()
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            ViewBag.Music = "";
            ViewBag.Musix = "";

            ViewBag.Karaoke = "karaoke";

            if (new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Exists)
                new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Delete(true);

            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music")).Create();
            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text")).Create();

            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        [HttpPost]
        public ActionResult PlayKaraoke(IFormCollection f, IFormFile txtKaraoke, IFormFile txtMusic, IFormFile txtMusix)
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            /*HttpContext.Session.Remove("ok-data");*/
            TempData["dataPost"] = "[POST]"; HttpContext.Session.SetString("data-result", "true");
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            HttpContext.Session.Remove("ok-data");
            Calendar xi = CultureInfo.InvariantCulture.Calendar;

            var xuxu = xi.AddHours(DateTime.UtcNow, 7);

            if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
            {
                TempData["mau_background"] = "white";
                TempData["mau_text"] = "black";TempData["mau_nen"] = "dodgerblue";
                 TempData["winx"] = "❤";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white";TempData["mau_nen"] = "rebeccapurple";
                 TempData["winx"] = "❤";
            }
            var pathX1  = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC_DarkBVL.txt");
            var noidungX = System.IO.File.ReadAllText(pathX1);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
                {
                    if (info[1] == "false")
                    {
                        
                        TempData["mau_winx"] = "red";
                        flag = 1;
                    }
                    else
                    {
                        
                        TempData["mau_winx"] = "deeppink";
                        flag = 0;
                    }
                }
            }
            if (txtMusic == null || txtMusic.Length == 0)
                txtMusic = txtMusix;

            if (txtMusix == null || txtMusix.Length == 0)
                txtMusix = txtMusic;

            ViewBag.Karaoke = "";

            ViewBag.Show = "show";

            var chon = f["KaraChon"].ToString();

            if (chon == "1")
            {
                var r = new Random();
                int x = r.Next(10);

                ViewBag.Background = "/karaoke_Example/background/" + (x + 1) + ".jpg";
                ViewBag.SuDung = "";
            }
            else if (chon == "2")
            {
                var link = f["txtOnline"].ToString();
                ViewBag.Background = link;
                ViewBag.SuDung = "";
            }
            else if (chon == "3")
            {
                var link = f["txtOnline"].ToString();
                ViewBag.Background = link;
                ViewBag.SuDung = "Video";
            }
            else if (chon == "4")
            {
                var link = f["txtOnline"].ToString();
                link = link.Replace("&", "");
                link = link.Replace("loop", "");
                link = link.Replace("autoplay", "");
                link = link.Replace("controls", "");
                link = link.Replace("mute", "");
                link = link.Replace("youtu.be/", "youtube.com/embed/");
                link = link.Replace("youtube.com/watch?v=", "youtube.com/embed/");

                if (link.Contains("?"))
                    link += "&autoplay=1&loop=1&controls=0&mute=1";
                else
                    link += "?autoplay=1&loop=1&controls=0&mute=1";

                ViewBag.Background = link;
                ViewBag.SuDung = "Youtube";
            }

            if (f["txtChon"].ToString() != "on")
            {
                var fileName = Path.GetFileName(txtMusic.FileName);
                var nameFile = Path.GetFileName(txtMusix.FileName);

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music", fileName);
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music", nameFile);

                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    txtMusic.CopyTo(fileStream);
                }

                using (Stream fileStream = new FileStream(pathX, FileMode.Create))
                {
                    txtMusix.CopyTo(fileStream);
                }

                ViewBag.Music = "/karaoke/music/" + fileName;
                ViewBag.Musix = "/karaoke/music/" + nameFile;

                fileName = Path.GetFileName(txtKaraoke.FileName);

                path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text", fileName);

                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    txtKaraoke.CopyTo(fileStream);
                }

                ViewBag.Karaoke = System.IO.File.ReadAllText(path);
            }
            else
            {
                ViewBag.Karaoke = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke_Example", "Tinh_Text.txt"));
                ViewBag.Music = "/karaoke_Example/Tinh_Karaoke.mp3";
                ViewBag.Musix = "/karaoke_Example/Tinh.mp3";
            }

            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult CreateFile_Karaoke()
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            if (new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Exists)
                new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Delete(true);

            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music")).Create();
            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text")).Create();

                ViewBag.LyricVD = "Tình như gió mơn man tóc em hiền\r\nTình như suối reo bên ánh trăng vàng\r\nTình như nắng vui bên bờ cát trắng\r\nTình có thông xanh, có anh và em đứng ngóng\r\nTình như nói cho nhau tiếng yêu đầu\r\nTình như giấc mơ như có anh cùng vào\r\nTình làm em nói bâng quơ và quên lối bước\r\nKhi xót xa anh, ngỡ như ngày tàn phai\r\nChàng đến với ánh mắt sáng với môi cười\r\nCho em bao tin vui và quên nỗi buồn.\r\nNgười yêu ơi có anh xoá tan đi ngày u tối trên thế gian\r\nÁnh mắt đó đắm đuối những ân tình\r\nCho em quên đi ưu phiền và bao nỗi sầu\r\nNgười yêu ơi giấc mơ gối trăng\r\nEm thầm mơ chỉ riêng có anh\r\nChỉ có riêng anh em trong đời.\r\n[Empty]\r\nTình như chiếc hôn khi đón em về\r\nTình như mắt môi em thắm yêu ngày đầu\r\nTình làm ta đến bên nhau và yêu đắm đuối\r\nNhư đã quen nhau, đã quen từ muôn kiếp trước\r\nTình như nói cho nhau tiếng yêu đầu\r\nTình như giấc mơ như có anh cùng vào\r\nTình làm em nói bâng quơ và quên lối bước\r\nKhi xót xa anh, ngỡ như ngày tàn phai\r\nChàng đến với ánh mắt sáng với môi cười\r\nCho em bao tin vui và quên nỗi buồn.\r\nNgười yêu ơi có anh xoá tan đi ngày u tối trên thế gian\r\nÁnh mắt đó đắm đuối những ân tình\r\nCho em quên đi ưu phiền và bao nỗi sầu\r\nNgười yêu ơi giấc mơ gối trăng\r\nEm thầm mơ chỉ riêng có anh\r\nChỉ có riêng anh em trong đời.\r\nChỉ có riêng anh em trong đời.\r\n[Empty]";
                return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateFile_Karaoke (IFormFile txtMusic, IFormCollection f)
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            /*HttpContext.Session.Remove("ok-data");*/
            TempData["dataPost"] = "[POST]"; HttpContext.Session.SetString("data-result", "true");
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            HttpContext.Session.Remove("ok-data");
            Calendar xi = CultureInfo.InvariantCulture.Calendar;

            string xuxuX = xi.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            var xuxu = xi.AddHours(DateTime.UtcNow, 7);

            if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
            {
                TempData["mau_background"] = "white";
                TempData["mau_text"] = "black";TempData["mau_nen"] = "dodgerblue";
                 TempData["winx"] = "❤";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white";TempData["mau_nen"] = "rebeccapurple";
                 TempData["winx"] = "❤";
            }
            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC_DarkBVL.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            var email = false;

            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (info[0] == "Email_Karaoke")
                {
                    if (info[1] == "true")
                        email = true;
                }

                    if (info[0] == "Email_Karaoke_OnlyText")
                    {
                        if (info[1] == "true")
                            email = false;
                    }


                    if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
                {
                    if (info[1] == "false")
                    {
                        
                        TempData["mau_winx"] = "red";
                        flag = 1;
                    }
                    else
                    {
                        
                        TempData["mau_winx"] = "deeppink";
                        flag = 0;
                    }
                }
            }

            TempData["Lyric"] = f["txtLyric"].ToString();
            TempData["Music"] = "/karaoke/music/"+txtMusic.FileName;

            var fileName = Path.GetFileName(txtMusic.FileName);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music", fileName);

            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                txtMusic.CopyTo(fileStream);
            }

            TempData["IDemail-Karaoke"] = TempData["userIP"] + "_" + HttpContext.Connection.Id + "_" + xuxuX.Replace(":", "").Replace(" ", "").Replace("/", "");

            if (email == true)
            {
                var ID = TempData["IDemail-Karaoke"];

                string host = "{" + Request.Host.ToString() + "}"
                  .Replace("http://", "")
              .Replace("http://", "")
              .Replace("/", "");

                MailRequest mail = new MailRequest();
                mail.Subject = host + "[ID email Karaoke : " + ID + "] Báo cáo bản mp3 tạo mới Karaoke của user (email bản text tương ứng sẽ được gửi sau khi user tạo xong - cũng tương ứng với ID email này, lưu ý : có thể user sẽ huỷ bỏ bản tạo Karaoke này và sau đó sẽ không nhận được email tương ứng nào; hãy tự chờ đợi và kiểm tra) lúc " + xuxuX;
                mail.ToEmail = "mywebplay.savefile@gmail.com";
                mail.Attachments = new List<IFormFile>();
                mail.Attachments.Add(txtMusic);

                 await _mailService.SendEmailAsync(mail, _webHostEnvironment.WebRootPath);
            }

            return RedirectToAction("PlayCreateFile_Karaoke");
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult PlayCreateFile_Karaoke()
        {
            try
            {
                var IDemail = TempData["IDemail-Karaoke"];
            TempData["IDemail-Karaoke"] = IDemail;


             TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            HttpContext.Session.SetString("data-result", "true");

            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            ViewBag.KaraX = "";
            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        [HttpPost]
        public ActionResult PlayCreateFile_Karaoke(IFormCollection f)
        {
            try
            {
                /*HttpContext.Session.Remove("ok-data");*/
                TempData["dataPost"] = "[POST]"; HttpContext.Session.SetString("data-result", "true");
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            HttpContext.Session.Remove("ok-data");
            Calendar xi = CultureInfo.InvariantCulture.Calendar;

            var xuxu1 = xi.AddHours(DateTime.UtcNow, 7);

            if (xuxu1.Hour >= 6 && xuxu1.Hour <= 17)
            {
                TempData["mau_background"] = "white";
                TempData["mau_text"] = "black";TempData["mau_nen"] = "dodgerblue";
                 TempData["winx"] = "❤";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white";TempData["mau_nen"] = "rebeccapurple";
                 TempData["winx"] = "❤";
            }
            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC_DarkBVL.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            var email = false;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (info[0] == "Email_Karaoke")
                {
                    if (info[1] == "true")
                        email = true;
                }

                    if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
                {
                    if (info[1] == "false")
                    {
                        
                        TempData["mau_winx"] = "red";
                        flag = 1;
                    }
                    else
                    {
                        
                        TempData["mau_winx"] = "deeppink";
                        flag = 0;
                    }
                }
            }
            Calendar x = CultureInfo.InvariantCulture.Calendar;

            string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            string fi = HttpContext.Session.GetString("userIP") + "_Karaoke_" + xuxu + ".txt";
            fi = fi.Replace("\\", "");
            fi = fi.Replace("/", "");
            fi = fi.Replace(":", "");

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text", fi);

            System.IO.File.WriteAllText(path, f["txtLyric"].ToString().Replace("undefined","").Replace(" *","*"));
            ViewBag.KaraX = "OK";
            ViewBag.FileKaraoke = "<p style=\"color:blue\">Thành công, một file TXT Karaoke của bạn đã được xử lý...</p><a href=\"/karaoke/text/" + fi + "\" download>Click vào đây để tải về</a><br><p style=\"color:red\">Hãy nhanh tay tải về vì sau <span style=\"color:deeppink\" id=\"thoigian3\" class=\"thoigian3\">30</span> giây nữa, file này sẽ bị xoá hoặc sẽ bị lỗi nếu có!<br>";

            if (email == true)
            {
                var ID = TempData["IDemail-Karaoke"];

                string host = "{" + Request.Host.ToString() + "}"
                  .Replace("http://", "")
              .Replace("http://", "")
              .Replace("/", "");

                SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com",
                 "mywebplay.savefile@gmail.com", host + "[ID email Karaoke : " + ID + "] Báo cáo bản text tạo mới Karaoke của user (email bản mp3 tương ứng đã được gửi trước đó/hoặc cũng có thể bạn đã setting không nhận thông báo dữ liệu bản mp3 - vui lòng kiểm tra lại, cũng phù hợp với ID email này) lúc " + xuxu, f["txtLyric"].ToString().Replace("undefined", "").Replace(" *", "*"), "teinnkatajeqerfl");
            }
                return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult XoaKaraoke(string? id, bool? cancel)
        {
            try
            {
                if (new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Exists)
                new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Delete(true);

            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music")).Create();
            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text")).Create();

            if (cancel == true || HttpContext.Session.GetString("auto-kara-index") == HttpContext.Session.GetString("length-list-auto"))
             {
                    HttpContext.Session.Remove("auto-kara-index");
                    HttpContext.Session.Remove("length-list-auto");
                    HttpContext.Session.Remove("content-listsong");
                }

            return RedirectToAction("Share_Karaoke", new { id = id });
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult PlayKaraokeX()
        {
            try
            {
                if (HttpContext.Session.GetString("length-list-auto") != null || HttpContext.Session.GetString("length-list-auto") != "")
                    TempData["length-list-auto"] = HttpContext.Session.GetString("length-list-auto");

                if (HttpContext.Session.GetString("auto-kara-index") != null || HttpContext.Session.GetString("auto-kara-index") != "")
                    TempData["index-auto"] = HttpContext.Session.GetString("auto-kara-index");

                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";  if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi");  if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");

                    var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);

            if (TempData["url"] == null)
                TempData["url"] = "[NOT]";

            if (TempData["baihat"] == null)
                TempData["baihat"] = "[NOT]";

            if (TempData["background"] == null)
                TempData["background"] = "[NOT]";

            if (TempData["option"] == null)
                TempData["option"] = 0;

            string? url = TempData["url"].ToString();
            string? baihat = TempData["baihat"].ToString();
            string? background = TempData["background"].ToString();
            int? option = int.Parse(TempData["option"].ToString());

            var n1 = StringMaHoaExtension.Encrypt(url);
            var n2 = StringMaHoaExtension.Encrypt(baihat);
            var n3 = StringMaHoaExtension.Encrypt(background);

            var share = n1 + "-.-" + n2 + "-.-" + n3 + "-.-" + option;
            ViewBag.LinkKaraoke = share;

            TempData["url"] = null;
            TempData["baihat"] = null;
            TempData["background"] = null;
            TempData["option"] = null;

            ViewBag.Music = "";
            ViewBag.Musix = "";

            ViewBag.Karaoke = "karaoke";

                if (HttpContext.Session.GetString("auto-kara-index") != null || HttpContext.Session.GetString("auto-kara-index") != "")
                {
                    TempData["index-auto"] = HttpContext.Session.GetString("auto-kara-index");
                }

                TempData["post-kara"] = "false";

                if (new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Exists)
                new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke")).Delete(true);

            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music")).Create();
            new System.IO.DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text")).Create();

            if (url != null && baihat != null && background != null && option != null
                && url != "[NOT]" && baihat != "[NOT]" && background != "[NOT]" && option != 0
                && url != "" && baihat != "" && background != "" && option != 0)
            {
                url = url.Replace("http://", "");
                url = url.Replace("http://", "");
                url = url.Replace("/", "");

                ViewBag.Server = url;
                ViewBag.BaiHatSV = baihat;
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://" + url + "/MyListSong.txt");
                    StreamReader reader = new StreamReader(stream);
                    String content = reader.ReadToEnd();
                    ViewBag.ListSong = content;
                    ViewBag.Background = background;
                    ViewBag.Option = option;
                    ViewBag.Share = "YES";

                        HttpContext.Session.SetString("content-listsong", content);

                        TempData["length-list-auto"] = (content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length).ToString();
                        HttpContext.Session.SetString("length-list-auto", TempData["length-list-auto"].ToString());

                        ViewBag.ListSong = content;
                    }
                catch
                {
                    ViewBag.Share = "ERROR";
                }

            }
            else
            if (url != null && url != "[NOT]" && url != "")
            {
                url = url.Replace("http://", "");
                url = url.Replace("http://", "");
                url = url.Replace("/", "");

                ViewBag.Server = url;
                ViewBag.BaiHatSV = baihat;

                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://" + url + "/MyListSong.txt");
                    StreamReader reader = new StreamReader(stream);
                    String content = reader.ReadToEnd();
                        HttpContext.Session.SetString("content-listsong", content);
 
                         TempData["length-list-auto"] = (content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length).ToString();
                        HttpContext.Session.SetString("length-list-auto", TempData["length-list-auto"].ToString());

                        ViewBag.ListSong = content;
                    ViewBag.Share = "OK";
                    }
                catch
                {
                    ViewBag.Share = "ERROR";
                }
            }

            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        [HttpPost]
        public ActionResult PlayKaraokeX(IFormCollection f, IFormFile txtKaraoke, IFormFile txtMusic, IFormFile txtMusix)
        {
            try
            {
                if (HttpContext.Session.GetString("length-list-auto") != null || HttpContext.Session.GetString("length-list-auto") != "")
                    TempData["length-list-auto"] = HttpContext.Session.GetString("length-list-auto");

                if (HttpContext.Session.GetString("auto-kara-index") != null || HttpContext.Session.GetString("auto-kara-index") != "")
                    TempData["index-auto"] = HttpContext.Session.GetString("auto-kara-index");

                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", ""); khoawebsiteClient(null); if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home"); if (TempData["errorXY"] == "true") return RedirectToAction("Error"); if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true"; if (HttpContext.Session.GetString("TuyetDoi") != null) { TempData["UyTin"] = "true"; var td = HttpContext.Session.GetString("TuyetDoi"); if (td == "true") { TempData["TestTuyetDoi"] = "true"; /*return View();*/ } else { TempData["TestTuyetDoi"] = "false"; } } if (TempData["tathoatdong"] == "true") { return RedirectToAction("Error"); } if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP"); if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */)
            {
                HttpContext.Session.Remove("userIP"); HttpContext.Session.SetString("userIP", "0.0.0.0");
                TempData["skipIP"] = "true";
            }
            /*HttpContext.Session.Remove("ok-data");*/
            TempData["dataPost"] = "[POST]"; HttpContext.Session.SetString("data-result", "true");
            TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
            var listIP = new List<string>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                listIP.Add(HttpContext.Session.GetString("userIP"));
            else
            {
                TempData["GetDataIP"] = "true";
                return RedirectToAction("Index");
            }
            khoawebsiteClient(listIP);
            HttpContext.Session.Remove("ok-data");
            Calendar xi = CultureInfo.InvariantCulture.Calendar;

            var xuxu = xi.AddHours(DateTime.UtcNow, 7);

            if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
            {
                TempData["mau_background"] = "white";
                TempData["mau_text"] = "black"; TempData["mau_nen"] = "dodgerblue";
                TempData["winx"] = "❤";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white"; TempData["mau_nen"] = "rebeccapurple";
                TempData["winx"] = "❤";
            }
            var pathX1 = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC_DarkBVL.txt");
            var noidungX = System.IO.File.ReadAllText(pathX1);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
                {
                    if (info[1] == "false")
                    {

                        TempData["mau_winx"] = "red";
                        flag = 1;
                    }
                    else
                    {

                        TempData["mau_winx"] = "deeppink";
                        flag = 0;
                    }
                }
            }

                TempData["post-kara"] = "true";

                if (txtMusic == null || txtMusic.Length == 0)
                txtMusic = txtMusix;

            if (txtMusix == null || txtMusix.Length == 0)
                txtMusix = txtMusic;

            ViewBag.Host = Request.Host;

            ViewBag.Karaoke = "";
            ViewBag.Share = f["txtShare"] + " - OK";

            ViewBag.Show = "show";

            TempData["fontKara"] = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "karaoke_font.txt"));

                ViewBag.LoginServer = f["txtServer"].ToString();
            ViewBag.BHServer = f["txtSong"].ToString();

            var chon = f["KaraChon"].ToString();

            if (f["auto_play"].ToString() == "on")
                {
                    TempData["auto-play"] = "false";
                }
                else
                {
                    TempData["auto-play"] = "true";
                }

            ViewBag.Option = chon;

            if (chon == "1")
            {
                var r = new Random();
                int x = r.Next(10);

                ViewBag.Background = "/karaoke_Example/background/" + (x + 1) + ".jpg";
                ViewBag.SuDung = "";
            }
            else if (chon == "2")
            {
                var link = f["txtOnline"].ToString();
                ViewBag.Background = link;
                ViewBag.SuDung = "";
            }
            else if (chon == "3")
            {
                var link = f["txtOnline"].ToString();
                ViewBag.Background = link;
                ViewBag.SuDung = "Video";
            }
            else if (chon == "4")
            {
                ViewBag.BehindYoutube = "YES";

                var link = f["txtOnline"].ToString();

                link = link.Replace("&", "");
                link = link.Replace("loop", "");
                link = link.Replace("autoplay", "");
                link = link.Replace("controls", "");
                link = link.Replace("mute", "");
                link = link.Replace("youtu.be/", "youtube.com/embed/");
                link = link.Replace("youtube.com/watch?v=", "youtube.com/embed/");

                if (link.Contains("youtube") == false)
                    ViewBag.Share = "ERROR";

                if (link.Contains("?"))
                    link += "&autoplay=1&loop=1&controls=0&mute=1";
                else
                    link += "?autoplay=1&loop=1&controls=0&mute=1";

                ViewBag.Background = link;
                ViewBag.SuDung = "Youtube";
            }
            else if (chon == "5")
            {
                ViewBag.BehindYoutube = "YES";

                var listYoutube = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke_Example", "Video_Youtube", "randomlink.txt")).Split("\r\n");

                var rand = new Random();
                var link = listYoutube[rand.Next(0, listYoutube.Length)];

                link = link.Replace("&", "");
                link = link.Replace("loop", "");
                link = link.Replace("autoplay", "");
                link = link.Replace("controls", "");
                link = link.Replace("mute", "");
                link = link.Replace("youtu.be/", "youtube.com/embed/");
                link = link.Replace("youtube.com/watch?v=", "youtube.com/embed/");

                if (link.Contains("youtube") == false)
                    ViewBag.Share = "ERROR";

                if (link.Contains("?"))
                    link += "&autoplay=1&loop=1&controls=0&mute=1";
                else
                    link += "?autoplay=1&loop=1&controls=0&mute=1";

                ViewBag.Background = link;
                ViewBag.SuDung = "Youtube";
            }

                var ct = HttpContext.Session.GetString("content-listsong");

            if (f["txtOnlineServer"].ToString() == "on")
            {
                var server = f["txtServer"].ToString();
                var song = f["txtSong"].ToString();

                var url_txt = server + "/" + song + "/" + song + ".txt";
                var url_goc = server + "/" + song + "/" + song + "_Goc.mp3";
                var url_kara = server + "/" + song + "/" + song + ".mp3";

                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("http://" + url_txt);
                    StreamReader reader = new StreamReader(stream);
                    String content = reader.ReadToEnd();

                    ViewBag.Music = "http://" + url_kara;
                    ViewBag.Musix = "http://" + url_goc;

                    if (content.Contains("<>") == false)
                    {
                        ViewBag.Karaoke = content;
                        TempData["TK-KARA"] = "";
                    }
                    else
                    {
                        var xa = content.Split("\n");
                        var noidung = "";
                        for (int i = 0; i < xa.Length; i++)
                        {
                            if (xa[i].Contains("<>"))
                            {
                                var xb = xa[i].Split("<>");
                                var xc = xb[1].Split("#");
                                var xd = xb[0].Split("-");
                                noidung += xc[0] + "=" + xd[0] + "=" + xd[1];

                                content = content.Replace(xb[0] + "<>", "");

                                if (i < xa.Length - 1)
                                    noidung += "\n";
                            }
                        }
                        TempData["TK-KARA"] = noidung;
                        ViewBag.Karaoke = content;
                    }
                }
                catch
                {
                    ViewBag.Share = "ERROR";
                }
            }
            else
            if (f["txtChon"].ToString() != "on")
            {
                var fileName = Path.GetFileName(txtMusic.FileName);
                var nameFile = Path.GetFileName(txtMusix.FileName);

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music", fileName);
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/music", nameFile);

                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    txtMusic.CopyTo(fileStream);
                }

                using (Stream fileStream = new FileStream(pathX, FileMode.Create))
                {
                    txtMusix.CopyTo(fileStream);
                }

                ViewBag.Music = "/karaoke/music/" + fileName;
                ViewBag.Musix = "/karaoke/music/" + nameFile;

                fileName = Path.GetFileName(txtKaraoke.FileName);

                path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text", fileName);

                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    txtKaraoke.CopyTo(fileStream);
                }
                var nd = System.IO.File.ReadAllText(path);
                if (nd.Contains("<>") == false)
                {
                    ViewBag.Karaoke = System.IO.File.ReadAllText(path);
                    TempData["TK-KARA"] = "";
                }
                else
                {
                    var xa = nd.Split("\n");
                    var noidung = "";
                    for (int i = 0; i< xa.Length;i++)
                    {
                        if (xa[i].Contains("<>"))
                        {
                            var xb = xa[i].Split("<>");
                            var xc = xb[1].Split("#");
                            var xd = xb[0].Split("-");
                            noidung += xc[0] + "=" + xd[0] + "=" + xd[1];

                            nd = nd.Replace(xb[0] + "<>", "");

                            if (i < xa.Length - 1)
                                noidung += "\n";
                        }
                    }
                    TempData["TK-KARA"] = noidung;
                    ViewBag.Karaoke = nd;
                }
            }
            else
            {
                ViewBag.Karaoke = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "karaoke_Example", "Tinh_Text.txt"));
                ViewBag.Music = "/karaoke_Example/Tinh_Karaoke.mp3";
                ViewBag.Musix = "/karaoke_Example/Tinh.mp3";
            }

                if (f["txtAutoSong"].ToString() == "on")
                {
                    if (HttpContext.Session.GetString("auto-kara-index") == null || HttpContext.Session.GetString("auto-kara-index") == "")
                    {
                        HttpContext.Session.SetString("auto-kara-index", "1");
                    }
                    else
                    {
                        int x = (int.Parse(HttpContext.Session.GetString("auto-kara-index")) + 1);
                        HttpContext.Session.SetString("auto-kara-index", x + "");
                    }
                    TempData["index-auto"] = HttpContext.Session.GetString("auto-kara-index");
                    TempData["length-list-auto"] = (ct.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Length).ToString();
                    HttpContext.Session.SetString("length-list-auto", TempData["length-list-auto"].ToString());
                }

                var n1 = StringMaHoaExtension.Encrypt(ViewBag.LoginServer);
            var n2 = StringMaHoaExtension.Encrypt(ViewBag.BHServer);
            var n3 = StringMaHoaExtension.Encrypt(ViewBag.Background);
            ViewBag.LinkKaraoke = n1 + "-.-" + n2 + "-.-" + n3 + "-.-" + chon;
            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }
    }
}
