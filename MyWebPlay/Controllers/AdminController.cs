﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Extension;
using MyWebPlay.Model;
using System.Globalization;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using System.Reflection.Metadata.Ecma335;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace MyWebPlay.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult SettingXYZ()
        {
            var path1 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/ListIPComeHere.txt");
            var noidung1 = docfile(path1);
            TempData["showIPCome"] = noidung1;

            Calendar x = CultureInfo.InvariantCulture.Calendar;

            var xuxu = x.AddHours(DateTime.UtcNow, 7);

            if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
            {
                TempData["mau_background"] = "white";
                TempData["mau_text"] = "black";TempData["mau_nen"] = "dodgerblue";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white";TempData["mau_nen"] = "rebeccapurple";
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidung = System.IO.File.ReadAllText(path);

            var listSetting = noidung.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            SettingAdmin settingAdmin = new SettingAdmin();
            settingAdmin.Topics = new List<SettingAdmin.Topic>();
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);
                settingAdmin.Topics.Add(new SettingAdmin.Topic(info[0], info[2], bool.Parse(info[1])));

                if (info[0] == "Password_Admin")
                    ViewBag.Password = info[3];

                if (info[0] == "MatDoTuyetDoi")
                    ViewBag.MaMatDo = info[3];

                if (info[0] == "Code_LockedClient")
                    ViewBag.CodeSocolar = info[3];

                if (info[0] == "Believe_IP")
                {
                    if (info[3] == "[NULL]")
                        ViewBag.Believe = "";
                    else
                        ViewBag.Believe = info[3];
                }

                if (info[0] == "Save_ComeHere")
                {
                    if (info[1] == "false")
                    {
                        TempData["SaveComeHere"] = "false";
                    }
                    else
                    {
                        TempData["SaveComeHere"] = "true";
                    }
                }
            }

            var path2 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/ListIPOnWebPlay.txt");
            var noidung2 = docfile(path2);
            TempData["ListIPActive"] = noidung2;

            var path3 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/ListIPLock.txt");
            var noidung3 = docfile(path3);
            TempData["ListIPLockAdmin"] = noidung3;

            var path4 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/LockedIPClient.txt");
            var noidung4 = docfile(path4);
            TempData["ListIPLockClient"] = noidung4;

            return View(settingAdmin);
        }

        [HttpPost]
        public ActionResult SettingXYZ(IFormCollection f)
        {
            var non = TempData["SaveComeHere"];
            TempData["SaveComeHere"] = non;

            Calendar x = CultureInfo.InvariantCulture.Calendar;

            var xuxu = x.AddHours(DateTime.UtcNow, 7);

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

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidung = System.IO.File.ReadAllText(path);

            var listSetting = noidung.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            var cometo = "#";
            var dix = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);
       
                        var xi = "false";
                        if (f[info[0]] == "on")
                        {
                            xi = "true";
                        }

                if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note"))
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


                if (info[0] != "Password_Admin" && info[0] != "Believe_IP" && info[0] != "Code_LockedClient" && info[0] != "MatDoTuyetDoi")
                {
                    if (xi != info[1])
                    {
                        cometo = "#come-" + i;
                        dix++;
                    }

                    noidung = noidung.Replace(info[0] + "<3275>" + info[1], info[0] + "<3275>" + xi);
                }
                else if (info[0] == "Password_Admin")
                {
                    var xinh = f[info[0]];
                    if (string.IsNullOrEmpty(xinh))
                        xinh = "mywebplay-ADMIN";

                    if (xinh != info[3])
                    {
                        cometo = "#come-" + i;
                        dix++;
                    }

                    noidung = noidung.Replace(listSetting[i], info[0] + "<3275>" + info[1] + "<3275>" + info[2] + "<3275>" + xinh);
                }
                else if (info[0] == "Believe_IP")
                {
                    var xinh = f[info[0]].ToString();
                    if (string.IsNullOrEmpty(xinh))
                        xinh = "[NULL]";
                    else
                    {
                        if (xinh.StartsWith(",") == false)
                            xinh = "," + xinh;

                        if (xinh.EndsWith(",") == false)
                            xinh = xinh + ",";
                    }

                    if (xinh != info[3])
                    {
                        cometo = "#come-" + i;
                        dix++;
                    }

                    noidung = noidung.Replace(listSetting[i], info[0] + "<3275>" + info[1] + "<3275>" + info[2] + "<3275>" + xinh);
                }
                else if (info[0] == "Code_LockedClient")
                {
                    var xinh = f[info[0]];
                    if (string.IsNullOrEmpty(xinh))
                        xinh = "abc-xyz";

                    if (xinh != info[3])
                    {
                        cometo = "#come-" + i;
                        dix++;
                    }

                    noidung = noidung.Replace(listSetting[i], info[0] + "<3275>" + info[1] + "<3275>" + info[2] + "<3275>" + xinh);
                }
                else if (info[0] == "MatDoTuyetDoi")
                {
                    var xinh = f[info[0]];
                    if (string.IsNullOrEmpty(xinh))
                        xinh = "believix-123";

                    if (xinh != info[3])
                    {
                        cometo = "#come-" + i;
                        dix++;
                    }

                    noidung = noidung.Replace(listSetting[i], info[0] + "<3275>" + info[1] + "<3275>" + info[2] + "<3275>" + xinh);
                }
            }
            System.IO.File.WriteAllText(path, noidung);
            if (dix == listSetting.Length - 2)
                cometo = "#welcome";
            return Redirect("/Admin/SettingXYZ"+cometo);
        }

        //-------------------------------------------------

        private String docfile(String filename)
        {
            string[] a = System.IO.File.ReadAllLines(filename);
            String s = "";
            for (int i = 0; i < a.Length; ++i)
            {
                s = s + a[i];
                if (i < a.Length - 1)
                    s = s + "\n";
            }
            return s;
        }

        public ActionResult TracNghiemOnline_ViewMark()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "TracNghiem_XOnline", "DiemHocSinh.txt");

            var file = new FileInfo(path);

            if (System.IO.File.Exists(path))
            {
                ViewBag.Text1 = System.IO.File.ReadAllText(path);
                ViewBag.Text2 = "<p id=\"preX\" style=\"color:" + TempData["mau_text"] + ";font-size:22px; display:none\">" + ViewBag.Text1.Replace("\n", "<br>") + "</p>";
                Calendar x = CultureInfo.InvariantCulture.Calendar;
                ViewBag.DateTime = x.AddHours(file.LastWriteTimeUtc, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            return View();
        }

        public ActionResult EditStudentMark()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "TracNghiem_XOnline", "DiemHocSinh.txt");
            if (System.IO.File.Exists(path))
            {
                ViewBag.Text = System.IO.File.ReadAllText(path);
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditStudentMark(string? txtText)
        {

            Calendar xi = CultureInfo.InvariantCulture.Calendar;

            var xuxu1 = xi.AddHours(DateTime.UtcNow, 7);

            if (xuxu1.Hour >= 6 && xuxu1.Hour <= 17)
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
            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flag = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (flag == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note"))
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
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "TracNghiem_XOnline", "DiemHocSinh.txt");

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            if (string.IsNullOrEmpty(txtText) ==  false)
                System.IO.File.WriteAllText(path, txtText);
            else
            {
                var noidung = "Thời gian bắt đầu\tIP Address\tMSSV\tID Link\tThời gian kết thúc\tĐiểm\r\n";
                System.IO.File.WriteAllText(path, noidung);
            }


            return RedirectToAction("TracNghiemOnline_ViewMark");
        }

        public ActionResult XoaViewMark()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "TracNghiem_XOnline", "DiemHocSinh.txt");

            if (System.IO.File.Exists(path))
            {
                var noidung = "Thời gian bắt đầu\tIP Address\tMSSV\tID Link\tThời gian kết thúc\tĐiểm\r\n";
                System.IO.File.WriteAllText(path, noidung);
            }
            return RedirectToAction("TracNghiemOnline_ViewMark");
        }

        public ActionResult ReportListIPComeHere() 
        {
            var path1 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/ListIPComeHere.txt");
            var noidung1 = docfile(path1);

            System.IO.File.WriteAllText(path1, "");

            Calendar x = CultureInfo.InvariantCulture.Calendar;

            string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);


            string host = "{" + Request.Host.ToString() + "}"
                .Replace("http://", "")
            .Replace("http://", "")
            .Replace("/", "");

            var non = TempData["SaveComeHere"];
            TempData["SaveComeHere"] = non;

            if (string.IsNullOrEmpty(noidung1) == false)
            SendEmail.SendMail2Step("mywebplay.savefile@gmail.com",
               "mywebplay.savefile@gmail.com", host + "[ADMIN] Báo cáo thủ công danh sách các IP user đã ghé thăm và request từng chức năng của trang web (tất cả/có thể chưa đầy đủ) In " + xuxu, noidung1, "teinnkatajeqerfl");

            return RedirectToAction("SettingXYZ", new { key = "code" });
        }

        public ActionResult DeleteIPComeHere()
        {
            var path1 = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/ListIPComeHere.txt");
            var noidung1 = docfile(path1);

            System.IO.File.WriteAllText(path1, "");
       
            return RedirectToAction("SettingXYZ", new { key = "code"});
        }

        public ActionResult QuickDataInWeb(string? first)
        {
            HttpContext.Session.Remove("ok-data");
            if (string.IsNullOrEmpty(first) == false)
                ViewBag.FirstGoTo = "true";
            else
                ViewBag.FirstGoTo = "false";

            Random ri = new Random();
            ViewBag.NumberRandom = ri.Next(3) + 1;

            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flix = 0;
            var flox = 0;
            var flx = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (HttpContext.Session.GetString("TuyetDoi") != "true" && info[0] == "OffWebsite_All")
                {
                    if (info[1] == "true")
                        return RedirectToAction("Error","Home");
                }

                        if (info[0] == "Post_Clipboard")
                {
                    if (info[1] == "false")
                    {
                        TempData["PostResult"] = "false";
                    }
                    else
                    {
                        TempData["PostResult"] = "true";
                    }
                }

                if (info[0] == "Random_Layout")
                {
                    if (info[1] == "false")
                    {
                        TempData["Layout_Random"] = "false";
                    }
                    else
                    {
                        TempData["Layout_Random"] = "true";
                    }
                }

                if (info[0] == "Clear_Website")
                {
                    if (info[1] == "true")
                    {
                        flix = 1;
                    }
                }

                if (info[0] == "Using_QuickData")
                {
                    if (info[1] == "true")
                    {
                        flx = 1;
                    }
                }

                if (flix == 1 && flx == 1 && info[0] == "NotAlert_QuickData")
                {
                    if (info[1] == "true")
                    {
                        flox = 1;
                    }
                }

                if (flox == 0)
                {
                    if (HttpContext.Session.GetString("TuyetDoi") != "true" && (info[0] == "Using_QuickData" || info[0] == "Using_Website"))
                    {
                        if (info[1] == "false")
                        {
                            var r = new Random();
                            var x = r.Next(0, 2);
                            if (x == 1)
                                return Redirect("https://learn.microsoft.com/vi-vn/training/paths/get-started-c-sharp-part-1/?WT.mc_id=dotnet-35129-website");
                            return Redirect("https://stackoverflow.com/questions");
                        }
                    }
                }

                if (info[0] == "Off_RandomTab")
                {
                    if (info[1] == "false")
                    {
                        var pathX1 = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomTab_Image.txt");
                        var hinh = System.IO.File.ReadAllText(pathX1).Split("\n", StringSplitOptions.RemoveEmptyEntries);

                        var pathX2 = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomTab_Tittle.txt");
                        var tittle = System.IO.File.ReadAllText(pathX2).Split("\n", StringSplitOptions.RemoveEmptyEntries);

                        var r = new Random();
                        var ix = r.Next(0, hinh.Length);
                        var iy = r.Next(0, tittle.Length);

                        TempData["OffRandomTab"] = "false";
                        TempData["Tab_Image"] = hinh[ix];
                        TempData["Tab_Tittle"] = tittle[iy];
                    }
                    else
                    {
                        TempData["OffRandomTab"] = "true";
                    }
                }
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomLayOut.txt");
            TempData["RandomLayout"] = docfile(path);

            return View();
        }

        public ActionResult RemoveAllSessionAndTempData()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return Redirect("https://stackoverflow.com/questions");
        }

        [HttpPost]
        public ActionResult QuickDataInWeb(IFormCollection f)
        {
            HttpContext.Session.Remove("ok-data");
            try
            {
                var name = f["txtNoiDung"].ToString().Split("\r\n|\r\n", StringSplitOptions.RemoveEmptyEntries);
                TempData["Name"] = name[0];

                var s = name[1].Split("\r\n#\r\n", StringSplitOptions.RemoveEmptyEntries);
                var chuoi = "";
                for (int i = 0; i < s.Length; i++)
                {
                    var ss = s[i].Split("\r\n*\r\n", StringSplitOptions.RemoveEmptyEntries);
                    ss[1] = ss[1].Replace("[NULL]", "");

                    chuoi += "<textarea name=\"" + ss[0] + "\" cols=\"80\" rows=\"30\">" + ss[1].Trim('\"').Replace("[ngoackep_0000]", "\"") + "</textarea><br>\n";
                }

                TempData["Data"] = chuoi;

                HttpContext.Session.SetString("ok-data", "true");

                return RedirectToAction("PlayDataInWeb");
            }
            catch
            {
                return Redirect("http://stackoverflow.com/questions/4733878/how-to-debug-a-stackoverflowexception-in-net");
            }
        }

        public ActionResult PlayDataInWeb()
        {
            Random ri = new Random();
            ViewBag.NumberRandom = ri.Next(3) + 1;

            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (HttpContext.Session.GetString("TuyetDoi") != "true" &&  info[0] == "OffWebsite_All")
                {
                    if (info[1] == "true")
                        return RedirectToAction("Error","Home");
                }

                if (HttpContext.Session.GetString("ok-data") != "true")
                {
                    return Redirect("http://stackoverflow.com/questions/206820/how-do-i-prevent-and-or-handle-a-stackoverflowexception");
                }

                if (TempData["NotAlertQuickData"] == "false")
                {
                    if (HttpContext.Session.GetString("TuyetDoi") != "true" && (info[0] == "Using_QuickData" || info[0] == "Using_Website"))
                    {
                        if (info[1] == "false")
                        {
                            var r = new Random();
                            var x = r.Next(0, 2);
                            if (x == 1)
                                return Redirect("https://learn.microsoft.com/vi-vn/training/paths/get-started-c-sharp-part-1/?WT.mc_id=dotnet-35129-website");
                            return Redirect("https://stackoverflow.com/questions");
                        }
                    }
                }

                if (info[0] == "Random_Layout")
                {
                    if (info[1] == "false")
                    {
                        TempData["Layout_Random"] = "false";
                    }
                    else
                    {
                        TempData["Layout_Random"] = "true";
                    }
                }

                if (info[0] == "Off_RandomTab")
                {
                    if (info[1] == "false")
                    {
                        var pathX1 = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomTab_Image.txt");
                        var hinh = System.IO.File.ReadAllText(pathX1).Split("\n", StringSplitOptions.RemoveEmptyEntries);

                        var pathX2 = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomTab_Tittle.txt");
                        var tittle = System.IO.File.ReadAllText(pathX2).Split("\n", StringSplitOptions.RemoveEmptyEntries);

                        var r = new Random();
                        var ix = r.Next(0, hinh.Length);
                        var iy = r.Next(0, tittle.Length);

                        TempData["OffRandomTab"] = "false";
                        TempData["Tab_Image"] = hinh[ix];
                        TempData["Tab_Tittle"] = tittle[iy];
                    }
                    else
                    {
                        TempData["OffRandomTab"] = "true";
                    }
                }
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/RandomTab/RandomLayOut.txt");
            TempData["RandomLayout"] = docfile(path);
            return View();
        }

        public ActionResult RefreshInfoIPRegist()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "ClientConnect/InfoIPRegist.txt");
            System.IO.File.WriteAllText(path, "IP\tDateTime\tInfo");
            return Redirect("/Admin/SettingXYZ#labelActive");
        }
    }
}
