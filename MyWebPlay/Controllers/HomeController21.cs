﻿using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Extension;
using MyWebPlay.Model;
using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult InsertSQL()
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

                ViewBag.Chuoi = "user_id\tnvarchar\tNULL\r\nuser_age\tint\t((1))\r\nbirth_date\tdatetime\tNULL\r\nschool_date\tdatetime\tNULL\r\nweight\tdouble\tNULL\r\nheight\tdecimal\tNULL\r\nuser_name\tntext\tNULL\r\nuser_demo\tnvarchar\t(N'ON')\r\nfly_date\tdatetime\t(getdate())";
                ViewBag.Default = "birth_date=12/10/2024\r\nweight=50.8\r\nuser_name=Trần Văn A";
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult InsertSQL(IFormCollection f)
        {
            var nix = "";
            var exter = false;
            var linkdown = false;
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

                var listIP = new List<string>();
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

                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                        listIP.Add(HttpContext.Session.GetString("userIP"));
                    else
                    {
                        TempData["GetDataIP"] = "true";
                        return RedirectToAction("Index");
                    }
                }

                string chuoi = f["Chuoi"].ToString();
                chuoi = chuoi.Replace("[T-PLAY]", "\t");
                chuoi = chuoi.Replace("[N-PLAY]", "\n");
                chuoi = chuoi.Replace("[R-PLAY]", "\r");

                string txtDefault = f["Default"].ToString();
                txtDefault = txtDefault.Replace("[T-PLAY]", "\t");
                txtDefault = txtDefault.Replace("[N-PLAY]", "\n");
                txtDefault = txtDefault.Replace("[R-PLAY]", "\r");

                string boqua = f["BoQua"].ToString();
                boqua = boqua.Replace("[T-PLAY]", "\t");
                boqua = boqua.Replace("[N-PLAY]", "\n");
                boqua = boqua.Replace("[R-PLAY]", "\r");

                ViewBag.Chuoi = f["Chuoi"].ToString();
                ViewBag.Default = f["Default"].ToString();
                ViewBag.BoQua = f["BoQua"].ToString();

                TempData["dataPost"] = "[" + chuoi.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";

               ghilogrequest(f); if (exter == false)
                {
                    khoawebsiteClient(listIP);
                    HttpContext.Session.Remove("ok-data");
                    Calendar xi = CultureInfo.InvariantCulture.Calendar;

                    var xuxu = xi.AddHours(DateTime.UtcNow, 7);

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

                var sox = "";
                var cuctac = "";
                var chim = "";

                var s = "";
                var repl = "";

                var songDefault = new Hashtable();

                var DefaultList = txtDefault.Replace("\r", "").Split("\n#3275#\n", StringSplitOptions.RemoveEmptyEntries);
                for (int ii = 0; ii < DefaultList.Length; ii++)
                {
                    var listDefault = DefaultList[ii].Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    s += "INSERT INTO XXX VALUES (@replace)";
                    repl = "";
                    songDefault.Clear();
                    for (int i = 0; i < listDefault.Length; i++)
                    {
                        var t = listDefault[i].Split("=");
                        songDefault.Add(t[0], t[1]);
                    }

                    var listChuoi = chuoi.Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < listChuoi.Length; i++)
                    {
                        var t = listChuoi[i].Split("\t");

                        if (boqua.Contains(t[0])) continue;

                        if (songDefault.ContainsKey(t[0]))
                        {
                            repl += "N'" + songDefault[t[0]] + "'";
                        }
                        else
                        if (t[2] != "NULL")
                        {
                            if (t[2] == "(getdate())")
                                repl += "getdate()";
                            else
                                repl += t[2].Replace("(", "").Replace(")", "");
                        }
                        else
                        {
                            if (t[1] == "datetime")
                                repl += "getdate()";
                            else
                            if (t[1] == "int" || t[1] == "float" || t[1] == "decimal" || t[1] == "double" || t[1] == "long")
                                repl += "0";
                            else
                                repl += "N'A'";
                        }

                        if (i < listChuoi.Length - 1)
                            repl += ",";
                    }

                    s = s.Replace("@replace", repl);
                    s += "\r\n\r\n";
                }

                    nix = s;

                    s = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + s + "</textarea>";
                    Calendar x = CultureInfo.InvariantCulture.Calendar;

                    cuctac = x.AddHours(DateTime.UtcNow, 7).ToString();
                    chim = HttpContext.Request.Path.ToString().Replace("/", "").Replace("Home", "");
                    if (string.IsNullOrEmpty(chim)) chim = "Default";

                    sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_" + chim + "_dataresult.txt");
                    TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_" + chim + "_dataresult.txt";
                    new FileInfo(sox).Create().Dispose();
                    System.IO.File.WriteAllText(sox, nix);
                    ViewBag.Result = s;

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

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
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
           ghilogrequest(f); if (exter == false)
                return View();
            else
            {
                if (linkdown == true)
                    return Redirect("/POST_DataResult/" + TempData["fileResult"]);
                return Ok(new
                {
                    result = "http://" + Request.Host + "/POST_DataResult/" + TempData["fileResult"].ToString().Replace(" ", "%20")
                });
            }
        }

        public ActionResult PhanDoanMember_Karaoke()
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

                TempData["karax-cre"] = "0";
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
            return View();
        }

        [HttpPost]
        public ActionResult PhanDoanMember_Karaoke(IFormCollection f)
        {
            try
            {
                var fix = "";
                foreach (var item in f.Keys)
                {
                    fix += string.Format("{0} : {1}\n", item, f[item]);
                }

                HttpContext.Session.SetString("hanhdong_3275", fix);
                Calendar x = CultureInfo.InvariantCulture.Calendar;

                string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

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

                if (f.ContainsKey("txtText"))
                {
                    TempData["karax-cre"] = "1";

                    TempData["dataPost"] = "[POST - 1]";

                    var karaoke = new Karaoke();

                    var text = f["txtText"].ToString();

                    var encrypt = false;
                    try
                    {
                        StringMaHoaExtension.Decrypt(text);
                        encrypt = true;
                    }
                    catch
                    {
                        encrypt = false;
                    }

                    if (encrypt == true)
                    {
                        text = StringMaHoaExtension.Decrypt(text);
                    }

                    var tex = text.Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);

                    TempData["soluong2"] = tex.Length.ToString();

                    for (int i = 0; i < tex.Length; i++)
                    {
                        karaoke.text.Add(tex[i]);
                    }

                    var xilip = f["txtXiLip"].ToString().Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);

                    TempData["soluong1"] = xilip.Length.ToString();

                    for (int i = 0; i < xilip.Length; i++)
                    {
                        var xi = xilip[i].Split("\t");

                        karaoke.mausac.Add(xi[1]);
                        karaoke.member.Add(xi[0]);
                    }

                    TempData["boqua-trustedIP"] = "true";
                    return View(karaoke);
                }
                else if (f.ContainsKey("txtNoiDung"))
                {
                    TempData["karax-cre"] = "2";

                    TempData["dataPost"] = "[POST - 2]";

                    var nd = f["txtNoiDung"].ToString().Replace("# *#", "#\t*#");

                    var pathX = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                    var noidungX = System.IO.File.ReadAllText(pathX);
                    var listSetting = noidungX.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    var email = false;

                    var infoX = listSetting[49].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                    if (infoX[1] == "true")
                        nd = StringMaHoaExtension.Encrypt(nd);

                    string fi = HttpContext.Connection.Id.ToString() + "_Karaoke-members_" + xuxu + ".txt";
                    fi = fi.Replace("\\", "");
                    fi = fi.Replace("/", "");
                    fi = fi.Replace(":", "");

                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "karaoke/text", fi);

                    System.IO.File.WriteAllText(path, nd);
                    ViewBag.FileKaraoke = "<p style=\"color:blue\">Thành công, một file TXT Karaoke của bạn đã được xử lý (được tạo lại với phân đoạn hát cho các member tham gia)...</p><a href=\"/karaoke/text/" + fi + "\" download>Click vào đây để tải về</a><br><p style=\"color:red\">Hãy nhanh tay tải về vì sau <span style=\"color:deeppink\" id=\"thoigian3\" class=\"thoigian3\">30</span> giây nữa, file này sẽ bị xoá hoặc sẽ bị lỗi nếu có!<br>";

                    for (int i = 0; i < listSetting.Length; i++)
                    {
                        var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                        if (info[0] == "Email_Karaoke")
                        {
                            if (info[1] == "true")
                                email = true;
                        }
                    }

                    if (email == true)
                    {
                        HttpContext.Session.SetString("mail-karaoke", nd.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
            TempData["boqua-trustedIP"] = "true";
            return View();
        }

        public ActionResult EncryptTxtFile()
        {
            try
            {
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidungX = System.IO.File.ReadAllText(pathX);
                var listSetting = noidungX.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var lockedApp = listSetting[43].Split("<3275>");
                if (lockedApp[3] != "[NULL]")
                {
                    var xa = Request.Path;
                    if (xa == "" || xa == "/" || xa == null) xa = "/Home/Index";
                    if (lockedApp[3].Contains(xa))
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }

                var pathWW = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidungWW = System.IO.File.ReadAllText(pathWW);

                var listSettingSWW = noidungWW.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

                var infoXWW = listSettingSWW[22].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (infoXWW[1] == "true")
                {
                    var pathS = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "ClientConnect/ListIPComeHere.txt");
                    var noidungS = docfile(pathS);

                    var noidungZ = noidungS + "\n" + HttpContext.Session.GetString("userIP") + "\t" + DateTime.Now + "\t" + this.Request.Path + "\t[GET]";

                    System.IO.File.WriteAllText(pathS, noidungZ.Trim('\n'));
                }

                for (int i = 0; i < listSetting.Length; i++)
                {
                    var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                    if (info[0] == "LockedAll_Web")
                    {
                        if (info[1] == "true")
                        {
                            return RedirectToAction("Error", "Home");
                        }
                    }
                }

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
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + DateTime.Now + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    string xuxuz = xz.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + DateTime.Now + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
                return RedirectToAction("Error", "Home", new
                {
                    exception = "true"
                });
            }
        }

        [HttpPost]
        public ActionResult EncryptTxtFile(IFormCollection f)
        {
            try
            {
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidungX = System.IO.File.ReadAllText(pathX);
                var listSetting = noidungX.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var lockedApp = listSetting[43].Split("<3275>");
                if (lockedApp[3] != "[NULL]")
                {
                    var xa = Request.Path;
                    if (xa == "" || xa == "/" || xa == null) xa = "/Home/Index";
                    if (lockedApp[3].Contains(xa))
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }

                var pathWW = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries)[4]);
                var noidungWW = System.IO.File.ReadAllText(pathWW);

                var listSettingSWW = noidungWW.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

                var infoXWW = listSettingSWW[22].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (infoXWW[1] == "true")
                {
                    var pathS = Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot", ""), "PrivateFileAdmin", "ClientConnect/ListIPComeHere.txt");
                    var noidungS = docfile(pathS);

                    var noidungZ = noidungS + "\n" + HttpContext.Session.GetString("userIP") + "\t" + DateTime.Now + "\t" + this.Request.Path + "\t[GET]";

                    System.IO.File.WriteAllText(pathS, noidungZ.Trim('\n'));
                }

                for (int i = 0; i < listSetting.Length; i++)
                {
                    var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                    if (info[0] == "LockedAll_Web")
                    {
                        if (info[1] == "true")
                        {
                            return RedirectToAction("Error", "Home");
                        }
                    }
                }

                //var listIP = new List<string>();

                //if (string.IsNullOrEmpty(HttpContext.Session.GetString("userIP")) == false)
                //    listIP.Add(HttpContext.Session.GetString("userIP"));
                //else
                //{
                //    TempData["GetDataIP"] = "true";

                //    return RedirectToAction("Index");
                //}

                //khoawebsiteClient(listIP);

                var noidung = f["txtNoiDung"].ToString();
                var fix = string.Format("txtNoiDung : {0}\n", noidung);
                HttpContext.Session.SetString("hanhdong_3275", fix);

                bool check_var = false;
                try
                {
                    StringMaHoaExtension.Decrypt(noidung);
                    check_var = true;
                }
                catch
                {
                    check_var = false;
                }

                TempData["noidungTXT"] = "[Đã xảy ra lỗi. Vui lòng kiểm tra và thử lại sau]";

                if (check_var == true)
                TempData["noidungTXT"] = StringMaHoaExtension.Decrypt(noidung);
                return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                var errx = (HttpContext.Session.GetString("hanhdong_3275") != null) ? HttpContext.Session.GetString("hanhdong_3275") : string.Empty;
                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + Request.Method + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                System.IO.File.WriteAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "EXCEPTION_ERROR_LOG.txt"), "[Exception/error log - " + req + " - " + Request.Method + " - " + DateTime.Now + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx);
                var mailError = System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath.Replace("\\wwwroot",""), "PrivateFileAdmin", "Admin", "SecureSettingAdmin.txt")).Replace("\r", "").Split("\n")[11];
                if (mailError == "SEND_MAIL_WHEN_ERROR_EXCEPTION_ON")
                {
                    Calendar xz = CultureInfo.InvariantCulture.Calendar;
                    string xuxuz = xz.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    string hostz = "{" + Request.Host.ToString() + "}".Replace("http://", "").Replace("https://", "").Replace("/", "");
                    SendEmail.SendMail2Step(_webHostEnvironment.WebRootPath, "mywebplay.savefile@gmail.com", "mywebplay.savefile@gmail.com", hostz + "[ADMIN - " + ((TempData["userIP"] != null) ? TempData["userIP"] : HttpContext.Session.GetString("userIP")) + "] REPORT ERROR/ECEPTION LOG OF USER In " + xuxuz, "[Exception/error log - " + req + " - " + Request.Method + " - " + DateTime.Now + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace + "\n\n====================\n\n" + errx, "teinnkatajeqerfl");
                }
                return RedirectToAction("Error", new
                {
                    exception = "true"
                });
            }
        }
    }
}