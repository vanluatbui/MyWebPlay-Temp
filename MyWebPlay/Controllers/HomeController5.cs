﻿using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Extension;
using MyWebPlay.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        [HttpGet]
        public ActionResult String_Split_Regex()
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                khoawebsiteClient(null);
                var dua = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[14];
                if (dua == "DIRECT_GOOGLE.COM_10_TIMES_AFTER_TO_COME_MYWEBPLAY_ON")
                {
                    if (this.HttpContext.Request.Method == "GET")
                    {
                        if (HttpContext.Session.GetObject<int>("google-trick-web") == null)
                        {
                            HttpContext.Session.SetObject("google-trick-web", 1);
                            return Redirect("https://google.com");
                        }
                        else
                        {
                            var lan = HttpContext.Session.GetObject<int>("google-trick-web");
                            if (lan != 10)
                            {
                                HttpContext.Session.SetObject("google-trick-web", lan + 1);
                                return Redirect("https://google.com");
                            }
                        }
                    }
                }
                if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home");
                if (TempData["errorXY"] == "true") return Redirect("https://google.com");
                if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";
                if (HttpContext.Session.GetString("TuyetDoi") != null)
                {
                    TempData["UyTin"] = "true";
                    var td = HttpContext.Session.GetString("TuyetDoi");
                    if (td == "true")
                    {
                        TempData["TestTuyetDoi"] = "true"; /*return View();*/
                    }
                    else
                    {
                        TempData["TestTuyetDoi"] = "false";
                    }
                }
                if (TempData["tathoatdong"] == "true")
                {
                    return RedirectToAction("Error");
                }
                if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */ )
                {
                    HttpContext.Session.Remove("userIP");
                    HttpContext.Session.SetString("userIP", "0.0.0.0");
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
                return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                var errx = (HttpContext.Session.GetString("hanhdong_3275") != null) ? HttpContext.Session.GetString("hanhdong_3275") : string.Empty;
                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON" && HttpContext.Session.GetString("IsAdminUsing") != "true")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    var xuxuz = xz.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", ""));
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
        }

        [HttpPost]
        public ActionResult String_Split_Regex(IFormCollection f, IFormFile fileData)
        {
            var nix = "";
            var exter = false;
            var linkdown = false;
            var listIP = new List<string>();
            try
            {
                var fix = "";
                foreach (var item in f.Keys)
                {
                    fix += string.Format("{0} : {1}\n", item, f[item]);
                }

                HttpContext.Session.SetString("hanhdong_3275", fix);

                var path = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidung = System.IO.File.ReadAllText(path);

                var listSettingS = noidung.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

                var infoX = listSettingS[48].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (infoX[1] == "true")
                    exter = true;
                var infoY = listSettingS[8].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);
                if (infoY[1] == "true") linkdown = true;

               ghilogrequest(f); if (exter == false)
                {
                    TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                    khoawebsiteClient(null);
                    var dua = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[14];
                    if (dua == "DIRECT_GOOGLE.COM_10_TIMES_AFTER_TO_COME_MYWEBPLAY_ON")
                    {
                        if (this.HttpContext.Request.Method == "GET")
                        {
                            if (HttpContext.Session.GetObject<int>("google-trick-web") == null)
                            {
                                HttpContext.Session.SetObject("google-trick-web", 1);
                                return Redirect("https://google.com");
                            }
                            else
                            {
                                var lan = HttpContext.Session.GetObject<int>("google-trick-web");
                                if (lan != 10)
                                {
                                    HttpContext.Session.SetObject("google-trick-web", lan + 1);
                                    return Redirect("https://google.com");
                                }
                            }
                        }
                    }
                    if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home");
                    if (TempData["errorXY"] == "true") return Redirect("https://google.com");
                    if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";
                    if (HttpContext.Session.GetString("TuyetDoi") != null)
                    {
                        TempData["UyTin"] = "true";
                        var td = HttpContext.Session.GetString("TuyetDoi");
                        if (td == "true")
                        {
                            TempData["TestTuyetDoi"] = "true"; /*return View();*/
                        }
                        else
                        {
                            TempData["TestTuyetDoi"] = "false";
                        }
                    }
                    if (TempData["tathoatdong"] == "true")
                    {
                        return RedirectToAction("Error");
                    }
                    if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                    if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                    if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */ )
                    {
                        HttpContext.Session.Remove("userIP");
                        HttpContext.Session.SetString("userIP", "0.0.0.0");
                        TempData["skipIP"] = "true";
                    }
                    /*HttpContext.Session.Remove("ok-data");*/
                    TempData["dataPost"] = "[POST]";
                    HttpContext.Session.SetString("data-result", "true");
                    TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                    // var listIP = new List<string>();

                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                        listIP.Add(HttpContext.Session.GetString("userIP"));
                    else
                    {
                        TempData["GetDataIP"] = "true";
                        return RedirectToAction("Index");
                    }
                }
                string chuoi = f["Chuoi"].ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
                chuoi = chuoi.Replace("[T-PLAY]", "\t");
                chuoi = chuoi.Replace("[N-PLAY]", "\n");
                chuoi = chuoi.Replace("[R-PLAY]", "\r");

                Calendar xi = CultureInfo.InvariantCulture.Calendar;

                var delayTime = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "");
                var partDelayTime = delayTime.Split("#");
                var hourDL = partDelayTime[0].Replace("H", "");
                var minDL = partDelayTime[1].Replace("M", "");
                var secDL = partDelayTime[2].Replace("S", "");

                var xuxu = xi.AddHours(DateTime.UtcNow, 7);

                if (hourDL.Contains("-"))
                {
                   xuxu = xuxu.AddHours(-1 * int.Parse(hourDL.Replace("-", "")));
                }
                else
                {
                    xuxu = xuxu.AddHours(int.Parse(hourDL));
                }

                if (minDL.Contains("-"))
                {
                    xuxu = xuxu.AddMinutes(-1 * int.Parse(minDL.Replace("-", "")));
                }
                else
                {
                    xuxu = xuxu.AddHours(int.Parse(minDL));
                }

                if (secDL.Contains("-"))
                {
                    xuxu = xuxu.AddSeconds(-1 * int.Parse(secDL.Replace("-", "")));
                }
                else
                {
                    xuxu.AddSeconds(int.Parse(secDL));
                }

                TempData["dataPost"] = "[" + chuoi.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
               ghilogrequest(f); if (exter == false)
                {
                    khoawebsiteClient(listIP);
                    HttpContext.Session.Remove("ok-data");

                    if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
                    {
                        TempData["mau_background"] = "white";
                        TempData["mau_text"] = "black";
                        TempData["mau_nen"] = "dodgerblue";
                        TempData["winx"] = "❤";
                    }
                    else
                    {
                        TempData["mau_background"] = "black";
                        TempData["mau_text"] = "white";
                        TempData["mau_nen"] = "rebeccapurple";
                        TempData["winx"] = "❤";
                    }
                    var pathX = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                    var noidungX = System.IO.File.ReadAllText(pathX);
                    var listSetting = noidungX.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    var flag = 0;
                    for (int i = 0; i < listSetting.Length; i++)
                    {
                        var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                        if (flag == 0 && (info[0] == "Email_Upload_User" ||
                            info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create" ||
                            info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question" ||
                            info[0] == "Email_User_Website" || info[0] == "Email_User_Continue" ||
                            info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
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
                }

                if (string.IsNullOrEmpty(chuoi))
                {
                    ViewData["Loi1"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.String_Split_Regex();
                }

                string dukien1 = f["DuKien1"].ToString();
                dukien1 = dukien1.Replace("[T-PLAY]", "\t");
                dukien1 = dukien1.Replace("[N-PLAY]", "\n");
                dukien1 = dukien1.Replace("[R-PLAY]", "\r");
                if (string.IsNullOrEmpty(dukien1))
                {
                    ViewData["Loi2"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.String_Split_Regex();
                }

                string pattern = f["Pattern"].ToString();
                if (string.IsNullOrEmpty(pattern))
                {
                    ViewData["Loi3"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.String_Split_Regex();
                }

                pattern = pattern.Replace("[T-PLAY]", "\t");
                pattern = pattern.Replace("[N-PLAY]", "\n");
                pattern = pattern.Replace("[R-PLAY]", "\r");

                string dukien2 = f["DuKien2"].ToString();
                if (string.IsNullOrEmpty(dukien2))
                {
                    ViewData["Loi4"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.String_Split_Regex();
                }

                dukien2 = dukien2.Replace("[T-PLAY]", "\t");
                dukien2 = dukien2.Replace("[N-PLAY]", "\n");
                dukien2 = dukien2.Replace("[R-PLAY]", "\r");

                string X = f["X"].ToString();

                X = X.Replace("[T-PLAY]", "\t");
                X = X.Replace("[N-PLAY]", "\n");
                X = X.Replace("[R-PLAY]", "\r");
                //if (string.IsNullOrEmpty(X))
                //{
                //    ViewData["Loi5"] = "Trường này không được để trống!";
                //    HttpContext.Session.SetString("data-result", "true"); return this.String_Split_Regex();
                //}

                string Y = f["Y"].ToString();
                Y = Y.Replace("[T-PLAY]", "\t");
                Y = Y.Replace("[N-PLAY]", "\n");
                Y = Y.Replace("[R-PLAY]", "\r");
                //if (string.IsNullOrEmpty(Y))
                //{
                //    ViewData["Loi6"] = "Trường này không được để trống!";
                //    HttpContext.Session.SetString("data-result", "true"); return this.String_Split_Regex();
                //}


                if (f.ContainsKey("txtAPI") || (fileData != null && string.IsNullOrEmpty(fileData.FileName) == false))
                {
                    var txtAPI = f["txtAPI"].ToString().Replace("[T-PLAY]", "\t").Replace("[N-PLAY]", "\n").Replace("[R-PLAY]", "\r");
                    if (fileData != null && string.IsNullOrEmpty(fileData.FileName) == false)
                    {
                        if (fileData.FileName.EndsWith(".txt"))
                        {
                            using (var reader = new StreamReader(fileData.OpenReadStream()))
                            {
                                string content = reader.ReadToEnd();
                                txtAPI = content;
                            }
                        }
                    }
                    var apiValue = txtAPI.ToString().Replace("\r", "").Split("\n||\n");
                    chuoi = apiValue[0].Replace("[Empty]", "");
                    dukien1 = apiValue[1].Replace("[Empty]", "");
                    pattern = apiValue[2].Replace("[Empty]", "");
                    dukien2 = apiValue[3].Replace("[Empty]", "");
                    X = apiValue[4].Replace("[Empty]", "");
                    Y = apiValue[5].Replace("[Empty]", "");
                }
                string[] DS = Regex.Split(chuoi, "\r\n");
                Regex regex = new Regex(pattern);

                string result = "\r\n";

                int stt = 0;
                for (int i = 0; i < DS.Length; i++)
                {
                    string[] ds;
                    if (int.Parse(dukien1) == 1)
                        ds = regex.Split(DS[i]);
                    else
                    {
                        ds = DS[i].Split(pattern.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    }

                    int x, y;

                    if (int.Parse(dukien2) == 1)
                    {
                        x = int.Parse(X);

                        if (Y.ToCharArray()[0] == 'N')
                        {
                            if (Y.Length > 1 && Y.ToCharArray()[1] == '-')
                                y = ds.Length - int.Parse(Y.ToCharArray()[2].ToString());
                            else
                                y = ds.Length;
                        }
                        else
                            y = int.Parse(Y);
                    }
                    else
                    if (int.Parse(dukien2) == 2)
                    {
                        x = int.Parse(X);
                        y = ds.Length - int.Parse(Y);
                    }
                    else
                    if (int.Parse(dukien2) == 3)
                    {
                        x = int.Parse(X);
                        y = x + 1;
                    }
                    else
                    //if (radioButton3.Checked == true)
                    {
                        x = ds.Length - (int.Parse(Y) + 1);
                        y = ds.Length;
                    }

                    //int j = x;
                    //while ((radioButton4.Checked != true && j < y) || (radioButton4.Checked == true && j >=y-1))
                    //{
                    //    txtOutput.Text += ds[j] + " ";
                    //    if (radioButton4.Checked == true)
                    //        j--;
                    //    else
                    //        j++;
                    //}
                    stt++;

                    for (int j = x; j < y; j++)
                    {
                        result += ds[j];
                        if (j < y - 1)
                            result += " ";
                    }
                    if (i < DS.Length - 1)
                        result += "\r\n";
                }

                //TextCopy.ClipboardService.SetText(result);

                //            string re = "\r\n" + result;

                //re = re.Replace("\r\n", "<br>");
                nix = result;
                result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button>&nbsp;&nbsp;&nbsp;<a style=\"color:deeppink\" href=\""+  HttpContext.Request.Path.ToString() + "\">Làm mới</a><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

                var chim = HttpContext.Request.Path.ToString().Replace("/", "").Replace("Home", "");
                if (string.IsNullOrEmpty(chim)) chim = "Default";
                var sox = (f.ContainsKey("resultX") == false || f["resultX"] == "false") ?  Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", xuxu.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_" + chim + "_dataresult.txt") : Path.Combine(_webHostEnvironment.WebRootPath, "ResultExternal", "data.txt"); ;
                TempData["fileResult"] = xuxu.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_" + chim + "_dataresult.txt";
                new FileInfo(sox).Create().Dispose();
                System.IO.File.WriteAllText(sox, nix);
                ViewBag.Result = result;

                ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";
                if (TempData["ConnectLinkDown"] == "true") return Redirect("/POST_DataResult/" + TempData["fileResult"]);

               ghilogrequest(f); if (exter == false)
                    return View();
                else
                {
                    if (linkdown == true) return Redirect("/POST_DataResult/" + TempData["fileResult"]);
                    return Ok(new
                    {
                        result = "http://" + Request.Host + "/POST_DataResult/" + TempData["fileResult"].ToString().Replace(" ", "%20")
                    });
                }
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                var errx = (HttpContext.Session.GetString("hanhdong_3275") != null) ? HttpContext.Session.GetString("hanhdong_3275") : string.Empty;
                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON" && HttpContext.Session.GetString("IsAdminUsing") != "true")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    var xuxuz = xz.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", ""));
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
               ghilogrequest(f); if (exter == false)
                    return RedirectToAction("Error", new
                    {
                        exception = "true"
                    });
                return Ok(new
                {
                    error = HttpContext.Session.GetObject<string>("error_exception_log")
                });
            }
        }

        //--------------------------------------------

        [HttpGet]
        public ActionResult Check_Regex()
        {
            try
            {
                TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                khoawebsiteClient(null);
                var dua = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[14];
                if (dua == "DIRECT_GOOGLE.COM_10_TIMES_AFTER_TO_COME_MYWEBPLAY_ON")
                {
                    if (this.HttpContext.Request.Method == "GET")
                    {
                        if (HttpContext.Session.GetObject<int>("google-trick-web") == null)
                        {
                            HttpContext.Session.SetObject("google-trick-web", 1);
                            return Redirect("https://google.com");
                        }
                        else
                        {
                            var lan = HttpContext.Session.GetObject<int>("google-trick-web");
                            if (lan != 10)
                            {
                                HttpContext.Session.SetObject("google-trick-web", lan + 1);
                                return Redirect("https://google.com");
                            }
                        }
                    }
                }
                if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home");
                if (TempData["errorXY"] == "true") return Redirect("https://google.com");
                if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";
                if (HttpContext.Session.GetString("TuyetDoi") != null)
                {
                    TempData["UyTin"] = "true";
                    var td = HttpContext.Session.GetString("TuyetDoi");
                    if (td == "true")
                    {
                        TempData["TestTuyetDoi"] = "true"; /*return View();*/
                    }
                    else
                    {
                        TempData["TestTuyetDoi"] = "false";
                    }
                }
                if (TempData["tathoatdong"] == "true")
                {
                    return RedirectToAction("Error");
                }
                if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */ )
                {
                    HttpContext.Session.Remove("userIP");
                    HttpContext.Session.SetString("userIP", "0.0.0.0");
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
                return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                var errx = (HttpContext.Session.GetString("hanhdong_3275") != null) ? HttpContext.Session.GetString("hanhdong_3275") : string.Empty;
                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON" && HttpContext.Session.GetString("IsAdminUsing") != "true")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    var xuxuz = xz.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", ""));
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
        }

        [HttpPost]
        public ActionResult Check_Regex(IFormCollection f, IFormFile fileData)
        {
            var nix = "";
            var exter = false;
            var linkdown = false;
            var listIP = new List<string>();
            try
            {
                var fix = "";
                foreach (var item in f.Keys)
                {
                    fix += string.Format("{0} : {1}\n", item, f[item]);
                }

                HttpContext.Session.SetString("hanhdong_3275", fix);
                var path = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidung = System.IO.File.ReadAllText(path);

                var listSettingS = noidung.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

                var infoX = listSettingS[48].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (infoX[1] == "true")
                    exter = true;
                var infoY = listSettingS[8].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);
                if (infoY[1] == "true") linkdown = true;

               ghilogrequest(f); if (exter == false)
                {
                    TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                    khoawebsiteClient(null);
                    var dua = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[14];
                    if (dua == "DIRECT_GOOGLE.COM_10_TIMES_AFTER_TO_COME_MYWEBPLAY_ON")
                    {
                        if (this.HttpContext.Request.Method == "GET")
                        {
                            if (HttpContext.Session.GetObject<int>("google-trick-web") == null)
                            {
                                HttpContext.Session.SetObject("google-trick-web", 1);
                                return Redirect("https://google.com");
                            }
                            else
                            {
                                var lan = HttpContext.Session.GetObject<int>("google-trick-web");
                                if (lan != 10)
                                {
                                    HttpContext.Session.SetObject("google-trick-web", lan + 1);
                                    return Redirect("https://google.com");
                                }
                            }
                        }
                    }
                    if (TempData["locked-app"] == "true") return RedirectToAction("Error", "Home");
                    if (TempData["errorXY"] == "true") return Redirect("https://google.com");
                    if (TempData["TestTuyetDoi"] == "true") TempData["TestTuyetDoi"] = "true";
                    if (HttpContext.Session.GetString("TuyetDoi") != null)
                    {
                        TempData["UyTin"] = "true";
                        var td = HttpContext.Session.GetString("TuyetDoi");
                        if (td == "true")
                        {
                            TempData["TestTuyetDoi"] = "true"; /*return View();*/
                        }
                        else
                        {
                            TempData["TestTuyetDoi"] = "false";
                        }
                    }
                    if (TempData["tathoatdong"] == "true")
                    {
                        return RedirectToAction("Error");
                    }
                    if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                    if (HttpContext.Session.GetString("userIP") == "0.0.0.0" && TempData["skipOK"] == "false") HttpContext.Session.Remove("userIP");
                    if (TempData["ClearWebsite"] == "true" /*|| TempData["UsingWebsite"] == "false" */ )
                    {
                        HttpContext.Session.Remove("userIP");
                        HttpContext.Session.SetString("userIP", "0.0.0.0");
                        TempData["skipIP"] = "true";
                    }
                    /*HttpContext.Session.Remove("ok-data");*/
                    TempData["dataPost"] = "[POST]";
                    HttpContext.Session.SetString("data-result", "true");
                    TempData["urlCurrent"] = Request.Path.ToString().Replace("/Home/", "");
                    //  var listIP = new List<string>();

                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                        listIP.Add(HttpContext.Session.GetString("userIP"));
                    else
                    {
                        TempData["GetDataIP"] = "true";
                        return RedirectToAction("Index");
                    }
                }
                string chuoi = f["Chuoi"].ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
                chuoi = chuoi.Replace("[T-PLAY]", "\t");
                chuoi = chuoi.Replace("[N-PLAY]", "\n");
                chuoi = chuoi.Replace("[R-PLAY]", "\r");

                if (f.ContainsKey("txtAPI") || (fileData != null && string.IsNullOrEmpty(fileData.FileName) == false))
                {
                    var txtAPI = f["txtAPI"].ToString().Replace("[T-PLAY]", "\t").Replace("[N-PLAY]", "\n").Replace("[R-PLAY]", "\r");
                    if (fileData != null && string.IsNullOrEmpty(fileData.FileName) == false)
                    {
                        if (fileData.FileName.EndsWith(".txt"))
                        {
                            using (var reader = new StreamReader(fileData.OpenReadStream()))
                            {
                                string content = reader.ReadToEnd();
                                txtAPI = content;
                            }
                        }
                    }
                    var apiValue = txtAPI.ToString().Replace("\r", "").Split("\n||\n");
                    chuoi = apiValue[0].Replace("[Empty]", "");
                }

                TempData["dataPost"] = "[" + chuoi.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
               ghilogrequest(f); if (exter == false)
                {
                    khoawebsiteClient(listIP);
                    HttpContext.Session.Remove("ok-data");
                    Calendar xi = CultureInfo.InvariantCulture.Calendar;

                    var delayTime = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "");
                    var partDelayTime = delayTime.Split("#");
                    var hourDL = partDelayTime[0].Replace("H", "");
                    var minDL = partDelayTime[1].Replace("M", "");
                    var secDL = partDelayTime[2].Replace("S", "");

                    var xuxu = xi.AddHours(DateTime.UtcNow, 7);

                    if (hourDL.Contains("-"))
                    {
                       xuxu = xuxu.AddHours(-1 * int.Parse(hourDL.Replace("-", "")));
                    }
                    else
                    {
                        xuxu = xuxu.AddHours(int.Parse(hourDL));
                    }

                    if (minDL.Contains("-"))
                    {
                        xuxu = xuxu.AddMinutes(-1 * int.Parse(minDL.Replace("-", "")));
                    }
                    else
                    {
                        xuxu = xuxu.AddHours(int.Parse(minDL));
                    }

                    if (secDL.Contains("-"))
                    {
                        xuxu = xuxu.AddSeconds(-1 * int.Parse(secDL.Replace("-", "")));
                    }
                    else
                    {
                        xuxu.AddSeconds(int.Parse(secDL));
                    }

                    if (xuxu.Hour >= 6 && xuxu.Hour <= 17)
                    {
                        TempData["mau_background"] = "white";
                        TempData["mau_text"] = "black";
                        TempData["mau_nen"] = "dodgerblue";
                        TempData["winx"] = "❤";
                    }
                    else
                    {
                        TempData["mau_background"] = "black";
                        TempData["mau_text"] = "white";
                        TempData["mau_nen"] = "rebeccapurple";
                        TempData["winx"] = "❤";
                    }
                    var pathX = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                    var noidungX = System.IO.File.ReadAllText(pathX);
                    var listSetting = noidungX.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    var flag = 0;
                    for (int i = 0; i < listSetting.Length; i++)
                    {
                        var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                        if (flag == 0 && (info[0] == "Email_Upload_User" ||
                            info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create" ||
                            info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question" ||
                            info[0] == "Email_User_Website" || info[0] == "Email_User_Continue" ||
                            info[0] == "Email_Note" || info[0] == "Email_Karaoke"))
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
                }

                if (string.IsNullOrEmpty(chuoi))
                {
                    ViewData["Loi1"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.Check_Regex();
                }

                string pattern = f["Pattern"].ToString();
                pattern = pattern.Replace("[T-PLAY]", "\t");
                pattern = pattern.Replace("[N-PLAY]", "\n");
                pattern = pattern.Replace("[R-PLAY]", "\r");
                if (string.IsNullOrEmpty(pattern))
                {
                    ViewData["Loi2"] = "Trường này không được để trống!";
                    HttpContext.Session.SetString("data-result", "true");
                    return this.Check_Regex();
                }

                while (pattern.Contains(@"\\") == true)
                {
                    pattern = pattern.Replace(@"\\", @"\");
                  }

                bool result = Regex.IsMatch(chuoi, pattern);

                ViewBag.KetQua = "Chuỗi : " + chuoi.Replace(" ", "[Space]").Replace("\t", "[Tab]") + " |-| Pattern : " + pattern.Replace(" ", "[Space]").Replace("\t", "[Tab]") + " |-| ==> KẾT QUẢ :  " + result;
                nix = ViewBag.KetQua;

                Calendar soi = CultureInfo.InvariantCulture.Calendar;
                var chim = HttpContext.Request.Path.ToString().Replace("/", "").Replace("Home", "");
                if (string.IsNullOrEmpty(chim)) chim = "Default";
                var cuctac = soi.AddHours(DateTime.UtcNow, 7) + "_" + chim;
                var sox = (f.ContainsKey("resultX") == false || f["resultX"] == "false") ? Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt") : Path.Combine(_webHostEnvironment.WebRootPath, "ResultExternal", "data.txt");
                TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt";
                new FileInfo(sox).Create().Dispose();
                System.IO.File.WriteAllText(sox, nix);

                ghilogrequest(f); if (exter == false)
                    return View();
                else
                {
                    if (linkdown == true) return Redirect("/POST_DataResult/" + TempData["fileResult"]);
                    return Ok(new
                    {
                        result = "http://" + Request.Host + "/POST_DataResult/" + TempData["fileResult"].ToString().Replace(" ", "%20")
                    });
                }
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                var errx = (HttpContext.Session.GetString("hanhdong_3275") != null) ? HttpContext.Session.GetString("hanhdong_3275") : string.Empty;
                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON" && HttpContext.Session.GetString("IsAdminUsing") != "true")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    var xuxuz = xz.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", ""));
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + CultureInfo.InvariantCulture.Calendar.AddHours(DateTime.UtcNow, 7).SendToDelaySetting(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries)[25].Replace("DELAY_DATETIME:", "")) + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
               ghilogrequest(f); if (exter == false)
                    return RedirectToAction("Error", new
                    {
                        exception = "true"
                    });
                return Ok(new
                {
                    error = HttpContext.Session.GetObject<string>("error_exception_log")
                });
            }
        }
    }
}