﻿using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult Regex_Replace_Multiple()
        {
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
        public ActionResult Regex_Replace_Multiple(IFormCollection f)
        {
            //HttpContext.Session.Remove("ok-data");
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
                TempData["nav_link"] = "text-dark"; TempData["winx"] = "❤";
            }
            else
            {
                TempData["mau_background"] = "black";
                TempData["mau_text"] = "white";TempData["mau_nen"] = "rebeccapurple";
                TempData["nav_link"] = "text-light"; TempData["winx"] = "❤";
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
            string chuoi = f["Chuoi"].ToString();
            chuoi = chuoi.Replace("[TAB-TPLAY]", "\t");
            chuoi = chuoi.Replace("[ENTER-NPLAY]", "\n");
            chuoi = chuoi.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(chuoi))
            {
                ViewData["Loi1"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string dukien1 = f["DuKien1"].ToString();
            dukien1 = dukien1.Replace("[TAB-TPLAY]", "\t");
            dukien1 = dukien1.Replace("[ENTER-NPLAY]", "\n");
            dukien1 = dukien1.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(dukien1))
            {
                ViewData["Loi4"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string input = f["Input"].ToString();
            input = input.Replace("[TAB-TPLAY]", "\t");
            input = input.Replace("[ENTER-NPLAY]", "\n");
            input = input.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(input))
            {
                ViewData["Loi2"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string yes = f["DuKien3"].ToString();
            yes = yes.Replace("[TAB-TPLAY]", "\t");
            yes = yes.Replace("[ENTER-NPLAY]", "\n");
            yes = yes.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(yes))
            {
                ViewData["Loi6"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string dukien2 = f["DuKien2"].ToString();
            dukien2 = dukien2.Replace("[TAB-TPLAY]", "\t");
            dukien2 = dukien2.Replace("[ENTER-NPLAY]", "\n");
            dukien2 = dukien2.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(dukien2))
            {
                ViewData["Loi5"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string dukien4 = f["DuKien4"].ToString();
            dukien4 = dukien4.Replace("[TAB-TPLAY]", "\t");
            dukien4 = dukien4.Replace("[ENTER-NPLAY]", "\n");
            dukien4 = dukien4.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(dukien4))
            {
                ViewData["Loi7"] = "Trường này không được để trống!";
                return this.Regex_Replace_Multiple();
            }

            string output = f["Output"].ToString();
            output = output.Replace("[TAB-TPLAY]", "\t");
            output = output.Replace("[ENTER-NPLAY]", "\n");
            output = output.Replace("[ENTER-RPLAY]", "\r");
            //if (string.IsNullOrEmpty(output))
            //{
            //    ViewData["Loi3"] = "Trường này không được để trống!";
            //    return this.Regex_Replace_Multiple();
            //}

            var listInput = Regex.Split(input, "\r\n");
            var listOutput = Regex.Split(output, "\r\n");

            if (listInput.Length != listOutput.Length)
            {
                ViewData["Loi3"] = "Số lượng thành phần pattern input và output không tương xứng...";
                return this.Regex_Replace_Multiple();
            }

            if (yes == "0")
            {
                for (int i = 0; i < listInput.Length - 1; i++)
                {
                    for (int j = i + 1; j < listInput.Length; j++)
                    {
                        if (listInput[j].Contains(listInput[i]))
                        {
                            string t = listInput[i];
                            listInput[i] = listInput[j];
                            listInput[j] = t;

                            string tt = listOutput[i];
                            listOutput[i] = listOutput[j];
                            listOutput[j] = tt;
                        }
                    }
                }
            }

            string result = "\r\n";

            if (int.Parse(dukien4) != 0)
            {
                    for (int i = 0; i < listInput.Length; i++)
                    {
                      for (int k = 0; k < int.Parse(dukien4); k++)
                      {
                        listInput[i] = listInput[i].Replace("\\n", "\n");
                        listInput[i] = listInput[i].Replace("\\r\\n", "\r\n");
                        listInput[i] = listInput[i].Replace("\\t", "\t");

                        listOutput[i] = listOutput[i].Replace("\\n", "\n");
                        listOutput[i] = listOutput[i].Replace("\\r\\n", "\r\n");
                        listOutput[i] = listOutput[i].Replace("\\t", "\t");

                        Regex regex = new Regex(listInput[i]);

                        if (int.Parse(dukien1) == 1)
                        {
                            while (listInput[i].Contains(@"\\") == true)
                            {
                                listInput[i] = listInput[i].Replace(@"\\", @"\");
                            }

                            while (listOutput[i].Contains(@"\\") == true)
                            {
                                listOutput[i] = listOutput[i].Replace(@"\\", @"\");
                            }

                            if (int.Parse(dukien2) == -1)
                            {
                                chuoi = regex.Replace(chuoi, listOutput[i]);
                                result = chuoi;
                            }
                            else
                            {
                                chuoi = regex.Replace(chuoi, listOutput[i], int.Parse(dukien2));
                                result = chuoi;
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace(listInput[i], listOutput[i]);
                            result = chuoi;
                        }
                    }
                }
            }
            else
            {
                    for (int i = 0; i < listInput.Length; i++)
                    {
                      while (chuoi.Contains(listInput[i]) == true)
                       {
                        listInput[i] = listInput[i].Replace("\\n", "\n");
                        listInput[i] = listInput[i].Replace("\\r\\n", "\r\n");
                        listInput[i] = listInput[i].Replace("\\t", "\t");

                        listOutput[i] = listOutput[i].Replace("\\n", "\n");
                        listOutput[i] = listOutput[i].Replace("\\r\\n", "\r\n");
                        listOutput[i] = listOutput[i].Replace("\\t", "\t");
                        Regex regex = new Regex(listInput[i]);

                        if (int.Parse(dukien1) == 1)
                        {
                            while (listInput[i].Contains(@"\\") == true)
                            {
                                listInput[i] = listInput[i].Replace(@"\\", @"\");
                            }

                            while (listOutput[i].Contains(@"\\") == true)
                            {
                                listOutput[i] = listOutput[i].Replace(@"\\", @"\");
                            }

                            if (int.Parse(dukien2) == -1)
                            {
                                chuoi = regex.Replace(chuoi, listOutput[i]);
                                result = chuoi;
                            }
                            else
                            {
                                chuoi = regex.Replace(chuoi, listOutput[i], int.Parse(dukien2));
                                result = chuoi;
                            }
                        }
                        else
                        {
                            chuoi = chuoi.Replace(listInput[i], listOutput[i]);
                            result = chuoi;
                        }
                    }
                }
            }

            //TextCopy.ClipboardService.SetText(result);

            result = "<button id=\"click_copy\" onclick=\"copyResult()\"><b style=\"color:red\">COPY RESULT</b></button><br><br><textarea id=\"txtResultX\" style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

            ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";


            return View();
        }
    }
}
