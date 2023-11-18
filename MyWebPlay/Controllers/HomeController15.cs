﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult FindValueCheckInSQL()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            ViewBag.ViDu = "user_name\tvarchar(10)\r\nuser_birth\tdatetime\r\nuser_age\tint";
            return View();
        }

        [HttpPost]
        public ActionResult FindValueCheckInSQL(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            string txtFields = f["txtFields"].ToString();
            txtFields = txtFields.Replace("[TAB-TPLAY]", "\t");
            txtFields = txtFields.Replace("[ENTER-NPLAY]", "\n");
            txtFields = txtFields.Replace("[ENTER-RPLAY]", "\r");

            TempData["dataPost"] = "[" + txtFields.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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
            ViewBag.ViDu = "user_name\tvarchar(10)\r\nuser_birth\tdatetime\r\nuser_age\tint";

            string txtTable = f["txtTable"].ToString();
            txtTable = txtTable.Replace("[TAB-TPLAY]", "\t");
            txtTable = txtTable.Replace("[ENTER-NPLAY]", "\n");
            txtTable = txtTable.Replace("[ENTER-RPLAY]", "\r");

            string txtCheck = f["txtCheck"].ToString();
            txtCheck = txtCheck.Replace("[TAB-TPLAY]", "\t");
            txtCheck = txtCheck.Replace("[ENTER-NPLAY]", "\n");
            txtCheck = txtCheck.Replace("[ENTER-RPLAY]", "\r");

            int txtLoai = int.Parse(f["txtLoai"].ToString());

            int txtLoas = int.Parse(f["txtLoas"].ToString());

            string[] listFields = txtFields.Split("\r\n");

            string result = "PRINT N'* DANH SÁCH CÁC FIELDS CÓ KẾT QUẢ TÌM THẤY - " + txtTable + " :'+CHAR(10)\n\n";

            for (int i =0; i< listFields.Length; i++)
            {
                string[] fix = listFields[i].Split("\t");

 
                    fix[0] = fix[0].ToLower();
                    fix[1] = fix[1].ToLower();

                    string fields = "";

                    string value = "";

                    if (txtLoai == 3 || txtLoai == 5)
                        fields = "CONVERT(nvarchar(10), " + fix[0] + ", 103)";
                    else
                        fields = fix[0];

                    if (txtLoai == 1 || txtLoai == 4 || txtLoai == 5)
                    {
                        if (txtLoas == 1)
                            value = " = '" + txtCheck + "'";
                        else if (txtLoas == 2)
                            value = " LIKE '" + txtCheck + "%'";
                        else if (txtLoas == 3)
                            value = " LIKE '%" + txtCheck + "%'";
                        else if (txtLoas == 4)
                            value = " LIKE '%" + txtCheck + "'";
                    }
                   else if (txtLoai == 2)
                        value = " = " + txtCheck + "";
                    else if (txtLoai == 3)
                        value = " = '" + txtCheck + "'";
                    else if (txtLoai == 6)
                    {
                        if (txtLoas == 1)
                            value = " = N'" + txtCheck + "'";
                        else if (txtLoas == 2)
                            value = " LIKE N'" + txtCheck + "%'";
                        else if (txtLoas == 3)
                            value = " LIKE N'%" + txtCheck + "%'";
                        else if (txtLoas == 4)
                            value = " LIKE N'%" + txtCheck + "'";
                    }

                    if ((txtLoai == 1 || txtLoai == 6) && (fix[1].Contains("char") == false && fix[1].Contains("text") == false && fix[1].Contains("binary") == false && fix[1].Contains("image") == false))
                        continue;

                    if ((txtLoai == 2) && (fix[1].Contains("memo") == false && fix[1].Contains("single") == false && fix[1].Contains("currency") == false && fix[1].Contains("money") == false && fix[1].Contains("double") == false && fix[1].Contains("long") == false && fix[1].Contains("byte") == false && fix[1].Contains("bit") == false && fix[1].Contains("int") == false && fix[1].Contains("decimal") == false && fix[1].Contains("numeric") == false && fix[1].Contains("money") == false && fix[1].Contains("float") == false && fix[1].Contains("real") == false))
                        continue;

                    if ((txtLoai == 3) && (fix[1].Contains("date") == false))
                        continue;

                    if ((txtLoai == 4) && (fix[1].Contains("memo") == false && fix[1].Contains("single") == false && fix[1].Contains("currency") == false && fix[1].Contains("money") == false && fix[1].Contains("double") == false && fix[1].Contains("long") == false && fix[1].Contains("byte") == false && fix[1].Contains("bit") == false && fix[1].Contains("int") == false && fix[1].Contains("decimal") == false && fix[1].Contains("numeric") == false && fix[1].Contains("money") == false && fix[1].Contains("float") == false && fix[1].Contains("real") == false
              && fix[1].Contains("identifier") == false && fix[1].Contains("var") == false && fix[1].Contains("char") == false && fix[1].Contains("text") == false && fix[1].Contains("binary") == false && fix[1].Contains("image") == false))
                        continue;

                    if ((txtLoai == 5) && (fix[1].Contains("date") == false
                        && fix[1].Contains("identifier") == false && fix[1].Contains("var") == false && fix[1].Contains("char") == false && fix[1].Contains("text") == false && fix[1].Contains("binary") == false && fix[1].Contains("image") == false))
                        continue;

                result += "IF ((SELECT COUNT(*) " + fix[0] + " FROM " + txtTable + " WHERE " + fields + value + ") >0)\nBEGIN\n\tPRINT '" + fix[0] + "'\nEND\n\n";
            }

            //TextCopy.ClipboardService.SetText(result);

            // s = "<p style=\"color:blue\"" + s + "</p>";
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        public ActionResult FindCompareValueInSQL()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            return View();
        }

        [HttpPost]
        public ActionResult FindCompareValueInSQL(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            string txtFields = f["txtFields"].ToString().ToLower();
            txtFields = txtFields.Replace("[TAB-TPLAY]", "\t");
            txtFields = txtFields.Replace("[ENTER-NPLAY]", "\n");
            txtFields = txtFields.Replace("[ENTER-RPLAY]", "\r");

            txtFields = txtFields.Replace(" ", "");
            txtFields = txtFields.Replace("\t", "");
            txtFields = txtFields.Replace(",", "");
            txtFields = txtFields.Replace("[", "");
            txtFields = txtFields.Replace("]", "");

            TempData["dataPost"] = "[" + txtFields.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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
            var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingABC.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var flax = 0;
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (flax == 0 && (info[0] == "Email_Upload_User"
                    || info[0] == "MegaIo_Upload_User" || info[0] == "Email_TracNghiem_Create"
                    || info[0] == "Email_TracNghiem_Update" || info[0] == "Email_Question"
                    || info[0] == "Email_User_Website" || info[0] == "Email_User_Continue"
                    || info[0] == "Email_Note"))
                {
                    if (info[1] == "false")
                    {
                        
                        TempData["mau_winx"] = "red";
                        flax = 1;
                    }
                    else
                    {
                        
                        TempData["mau_winx"] = "deeppink";
                        flax = 0;
                    }
                }
            }
           

            var listFields = txtFields.Split("\r\n");

            var listOld = f["txtOld"].ToString().Replace("[TAB-TPLAY]", "\t").Replace("[ENTER-NPLAY]", "\n").Replace("[ENTER-RPLAY]", "\r").Split("\r\n");

            var listNew = f["txtNew"].ToString().Replace("[TAB-TPLAY]", "\t").Replace("[ENTER-NPLAY]", "\n").Replace("[ENTER-RPLAY]", "\r").Split("\r\n");

            int dem = 1;
            var listChangeOld = new List<string>();
            var listChangeNew = new List<string>();

            var listWhere = f["txtWhere"].ToString().Replace("[TAB-TPLAY]", "\t").Replace("[ENTER-NPLAY]", "\n").Replace("[ENTER-RPLAY]", "\r").ToLower().Split(",");

            int[] whereX = new int[listWhere.Length];

            var result = "* Các vị trí fields đang phát hiện sự thay đổi về giá trị :\n\n";

            for (int i = 0; i < listWhere.Length; i++)
            {
                for (int j = 0; j < listFields.Length; j++)
                {
                    if (listWhere[i] == listFields[j])
                    {
                        whereX[i] = j;
                        break;
                    }
                }
            }

            for (int i =0; i< listOld.Length; i++)
            {
                var oldX = listOld[i].Split("\t");
                var newX = listNew[i].Split("\t");
                int flag = 0;
                for (int j =0; j < oldX.Length; j++)
                {
                    if (oldX[j] != newX[j])
                    {
                        if (flag == 0)
                        {
                            result += dem + ". Đặc điểm nhận dạng :\n\n";
                            for (int u = 0; u < listWhere.Length; u++)
                            {
                                 result += "+ " + listWhere[u] + " : " + newX[whereX[u]] + "\n";
                            }
                            result += "\n";
                        }
                        flag = 1;
                        result += "- Tên field : " + listFields[j] + "\n";
                        result += "- Giá trị cũ : " + oldX[j] + "\n";
                        result += "- Giá trị mới : " + newX[j] + "\n\n";
                    }
                }
                if (flag == 1)
                {
                    result += "\n";
                    dem++;
                }
            }

            if (dem == 1)
                result += "\n=> Không phát hiện các field trong dữ liệu của bạn có sự thay đổi giá trị!";

            ////TextCopy.ClipboardService.SetText(result);

            // s = "<p style=\"color:blue\"" + s + "</p>";
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        public ActionResult XuLySQL1()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            ViewBag.Table = "User\r\nProduct\r\nOrder";
            return View();
        }

        [HttpPost]
        public ActionResult XuLySQL1(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            string txtTable = f["txtTables"].ToString();
            txtTable = txtTable.Replace("[TAB-TPLAY]", "\t");
            txtTable = txtTable.Replace("[ENTER-NPLAY]", "\n");
            txtTable = txtTable.Replace("[ENTER-RPLAY]", "\r");

            TempData["dataPost"] = "[" + txtTable.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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
           
            ViewBag.Table = txtTable;
            var listTable = txtTable.Split("\r\n");

            var result = "CREATE PROC PROC_MYWEBPLAY\nAS\nBEGIN";

            for (int i =0; i < listTable.Length;i++)
            {
                result += "\n\tDECLARE @" + listTable[i]+" INT = (SELECT COUNT(*) FROM " + listTable[i]+")\n\tPRINT @" + listTable[i];
            }

            result += "\nEND\n\n";

            result += "--------------------SỬ DỤNG XONG HÃY NHỚ DROP PROC PROC_MYWEBPLAY---------------------\n\n\n";
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        public ActionResult XuLySQL2()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            ViewBag.Table = "User\r\nProduct\r\nOrder";
            return View();
        }

        [HttpPost]
        public ActionResult XuLySQL2(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            string txtTable = f["txtTable"].ToString();

            txtTable = txtTable.Replace("[TAB-TPLAY]", "\t");
            txtTable = txtTable.Replace("[ENTER-NPLAY]", "\n");
            txtTable = txtTable.Replace("[ENTER-RPLAY]", "\r");
            ViewBag.Table = txtTable;

            TempData["dataPost"] = "[" + txtTable.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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

            string chon = f["txtChon"].ToString();
            chon = chon.Replace("[TAB-TPLAY]", "\t");
            chon = chon.Replace("[ENTER-NPLAY]", "\n");
            chon = chon.Replace("[ENTER-RPLAY]", "\r");

            var listTable = txtTable.Split("\r\n");
            var result = "";
            if (chon != "on")
            {
                result += "CREATE TABLE TABLE_MYWEBPLAY\n(\n\tTableName nvarchar(100),\n\tXuLy nvarchar(100),\n\tNgayCapNhat DateTime\n)\n\n";
                result += "-------------------------------------------------------------------------------------\n\n\n";

                // INSERT
                for (int i =0; i< listTable.Length; i++)
                {
                    result += "CREATE TRIGGER TRIGGER_INSERT_MYWEBPLAY_" + listTable[i].ToUpper() + " ON " + listTable[i] + "\nFOR INSERT\nAS\nBEGIN\n\tINSERT INTO TABLE_MYWEBPLAY VALUES ('" + listTable[i] + "', 'Insert', GETDATE())\nEND\n\n";
                }

                // UPDATE
                for (int i = 0; i < listTable.Length; i++)
                {
                    result += "CREATE TRIGGER TRIGGER_UPDATE_MYWEBPLAY_" + listTable[i].ToUpper() + " ON " + listTable[i] + "\nFOR UPDATE\nAS\nBEGIN\n\tINSERT INTO TABLE_MYWEBPLAY VALUES ('" + listTable[i] + "', 'Update', GETDATE())\nEND\n\n";
                }

                // DELETE
                for (int i = 0; i < listTable.Length; i++)
                {
                    result += "CREATE TRIGGER TRIGGER_DELETE_MYWEBPLAY_" + listTable[i].ToUpper() + " ON " + listTable[i] + "\nFOR DELETE\nAS\nBEGIN\n\tINSERT INTO TABLE_MYWEBPLAY VALUES ('" + listTable[i] + "', 'Delete', GETDATE())\nEND\n\n";
                }
            }
            else
            {
                result += "DROP TABLE TABLE_MYWEBPLAY\n\n";
                result += "-------------------------------------------------------------------------------------\n\n\n";

                // INSERT
                for (int i = 0; i < listTable.Length; i++)
                {
                    result += "DROP TRIGGER TRIGGER_INSERT_MYWEBPLAY_" + listTable[i].ToUpper()+"\n";
                }

                // UPDATE
                for (int i = 0; i < listTable.Length; i++)
                {
                    result += "DROP TRIGGER TRIGGER_UPDATE_MYWEBPLAY_" + listTable[i].ToUpper() + "\n";
                }

                // DELETE
                for (int i = 0; i < listTable.Length; i++)
                {
                    result += "DROP TRIGGER TRIGGER_DELETE_MYWEBPLAY_" + listTable[i].ToUpper() + "\n";
                }
            }
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        public ActionResult XuLySQL3()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            ViewBag.Chuoi = "Coumn1\r\nCoumn2\r\nCoumn3\r\n#3275#\r\nEm là hoa\r\nhồng\r\nnhỏ[TAB-TPLAY]12345[TAB-TPLAY]Em là búp\r\nmăng non\r\nhồng hào trắng\r\nsáng\r\n#3275#\r\nTôi là bông\r\nhồng\r\ngià[TAB-TPLAY]06789[TAB-TPLAY]Em là búp\r\nmăng già\r\nnếp nhăn\r\ngoá phụ";
            return View();
        }

        [HttpPost]
        public ActionResult XuLySQL3(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            string chuoi = f["txtChuoi"].ToString();
            chuoi = chuoi.Replace("[TAB-TPLAY]", "\t");
            chuoi = chuoi.Replace("[ENTER-NPLAY]", "\n");
            chuoi = chuoi.Replace("[ENTER-RPLAY]", "\r");
            TempData["dataPost"] = "[" + chuoi.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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
           
            ViewBag.Chuoi = chuoi;
            var listCha = chuoi.Split("\r\n#3275#\r\n");
            var phan1 = listCha[0].Replace(" ", "").Replace("\t", "").Replace(",", "").Replace("[", "").Replace("]", "").Replace("\r\n","\t");
            var phan2 = listCha[1].Replace("\r\n", "  ");
            var phan3 = listCha[2].Replace("\r\n", "  ");

            var result = phan1 + "\n" + phan2 + "\n"+ phan3;
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        public ActionResult XuLyCode9()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            return View();
        }

        [HttpPost]
        public ActionResult XuLyCode9(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            var txtTable = f["txtChuoi"].ToString();
            txtTable = txtTable.Replace("[TAB-TPLAY]", "\t");
            txtTable = txtTable.Replace("[ENTER-NPLAY]", "\n");
            txtTable = txtTable.Replace("[ENTER-RPLAY]", "\r");

            TempData["dataPost"] = "[" + txtTable.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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
            

            var txtChuoi = txtTable.Split("\r\n");
            var result = "";

            var k = 1;
            for (int i = 0; i < txtChuoi.Length; i++)
            {
                var thang = txtChuoi[i].Split("#",StringSplitOptions.RemoveEmptyEntries);

                if (txtChuoi[i].Contains("#") && thang.Length == 2)
                {
                    var tach = thang[1].Split(".", StringSplitOptions.RemoveEmptyEntries);

                    result += "var result" + k + " = (" + thang[0] + ")" + tach[0] + ";\r\nif (result" + k + " != null && ";
                    var xanh = "";
                    for (int j = 1; j<tach.Length-1; j++)
                    {
                        result += "result" + k + "." + xanh + tach[j] + " != null";
                        xanh += tach[j] + ".";
                        if (j < tach.Length - 2)
                            result += " && ";
                    }
                    result += ")\r\n\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"(" + thang[0] + ") " + thang[1] + " = \"+" + thang[1].Replace(tach[0], "result"+k) +"+\"\\n\\n\");\r\nelse\r\n{\r\n\tif (result"+k+" == null)\r\n";
                    result += "\t\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"(" + thang[0] +") " + tach[0] +" = NULL\"+\"\\n\\n\");\r\n";
                    var xam = "";
                    for (int j = 1; j<tach.Length-1;j++)
                    {
                        result += "\telse if (result" + k + "." + xam + tach[j]+ " == null)\r\n\t\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"(" + thang[0]+") "+tach[0] + "."+xam+tach[j]+" = NULL\"+\"\\n\\n\");\r\n";
                        xam += tach[j] + ".";
                    }
                    result += "}\r\n\r\n";
                    k++;
                }
                else if (txtChuoi[i].Contains("#") == false && txtChuoi[i].Contains("."))
                {
                    var tach = txtChuoi[i].Split(".", StringSplitOptions.RemoveEmptyEntries);
                    var xanh = "";
                    result += "if (" + tach[0]+" != null && ";
                    for (int j = 1; j < tach.Length - 1; j++)
                    {
                        result += tach[0] + "." + xanh + tach[j] + " != null";
                        xanh += tach[j] + ".";
                        if (j < tach.Length - 2)
                            result += " && ";
                    }
                    result += ")\r\n\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"" + txtChuoi[i] + " = \"+" + txtChuoi[i] + "+\"\\n\\n\");\r\nelse\r\n{\r\n\tif (" + tach[0] +" == null)\r\n";
                    result += "\t\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"" + tach[0] + " = NULL\"+\"\\n\\n\");\r\n";
                    var xam = "";
                    for (int j = 1; j < tach.Length - 1; j++)
                    {
                        result += "\telse if ("+tach[0] + "." + xam + tach[j] + " == null)\r\n\t\tSystem.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \""+ tach[0] + "." + xam + tach[j] + " = NULL\"+\"\\n\\n\");\r\n";
                        xam += tach[j] + ".";
                    }
                    result += "}\r\n\r\n";
                }
                else
                {
                    result += "System.IO.File.WriteAllText(\"D:/XemCode/ma.txt\", \"\\n\\n\" + System.IO.File.ReadAllText(\"D:/XemCode/ma.txt\") + \"" + txtChuoi[i] +" = \"+" + txtChuoi[i] +"+\"\\n\\n\");\r\n\r\n";
                }
            }
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }


            public ActionResult XuLySQL4()
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            return View();
        }

        [HttpPost]
        public ActionResult XuLySQL4(IFormCollection f)
        {
            khoawebsiteClient(null); if (HttpContext.Session.GetString("userIP") == "0.0.0.0") HttpContext.Session.Remove("userIP");
            if (TempData["ClearWebsite"] == "true" || TempData["UsingWebsite"] == "false")
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
            var txtTable = f["txtTable"].ToString();
            var thieus = f["txtThieu"].ToString().Replace("[TAB-TPLAY]", "\t").Replace("[ENTER-NPLAY]", "\n").Replace("[ENTER-RPLAY]", "\r").Split("\r\n");
            var daydus = f["txtDayDu"].ToString().Replace("[TAB-TPLAY]", "\t").Replace("[ENTER-NPLAY]", "\n").Replace("[ENTER-RPLAY]", "\r").Split("\r\n");
            txtTable = txtTable.Replace("[TAB-TPLAY]", "\t");
            txtTable = txtTable.Replace("[ENTER-NPLAY]", "\n");
            txtTable = txtTable.Replace("[ENTER-RPLAY]", "\r");

            TempData["dataPost"] = "[" + txtTable.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t") + "]";
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

           

            var has = new Hashtable();
            for (int i =0; i < daydus.Length;i++)
            {
                var duday = Regex.Split(daydus[i], "\t| ");
                has[duday[0]] = duday[1];
            }

            var result = "ALTER TABLE " + txtTable + " ADD";
            for (int i=0;i< thieus.Length;i++)
            {
                if (has.ContainsKey(thieus[i]) == true)
                    result += "\n\t"+thieus[i] + " " + has[thieus[i]]+",\n";
                else
                    result += "\n\t" + thieus[i] + " nvarchar(max),\n";
            }
            var nix = result;
            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

              Calendar soi = CultureInfo.InvariantCulture.Calendar; var cuctac = soi.AddHours(DateTime.UtcNow, 7); var sox = Path.Combine(_webHostEnvironment.WebRootPath, "POST_DataResult", cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "") + "_dataresult.txt"); TempData["fileResult"] = cuctac.ToString().Replace("\\", "").Replace("/", "").Replace(":", "")  + "_dataresult.txt"; new FileInfo(sox).Create().Dispose(); System.IO.File.WriteAllText(sox, nix); ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }
    }
}

