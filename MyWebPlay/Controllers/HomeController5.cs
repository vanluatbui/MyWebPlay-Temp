﻿using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        [HttpGet]
        public ActionResult String_Split_Regex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult String_Split_Regex(IFormCollection f)
        {
            string chuoi = f["Chuoi"].ToString();

            if (string.IsNullOrEmpty(chuoi))
            {
                ViewData["Loi1"] = "Trường này không được để trống!";
                return this.String_Split_Regex();
            }

            chuoi = chuoi.Replace("[TAB-TPLAY]", "\t");
            chuoi = chuoi.Replace("[ENTER-NPLAY]", "\n");
            chuoi = chuoi.Replace("[ENTER-RPLAY]", "\r");

            string dukien1 = f["DuKien1"].ToString();
            dukien1 = dukien1.Replace("[TAB-TPLAY]", "\t");
            dukien1 = dukien1.Replace("[ENTER-NPLAY]", "\n");
            dukien1 = dukien1.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(dukien1))
            {
                ViewData["Loi2"] = "Trường này không được để trống!";
                return this.String_Split_Regex();
            }

            string pattern = f["Pattern"].ToString();
            if (string.IsNullOrEmpty(pattern))
            {
                ViewData["Loi3"] = "Trường này không được để trống!";
                return this.String_Split_Regex();
            }

            pattern = pattern.Replace("[TAB-TPLAY]", "\t");
            pattern = pattern.Replace("[ENTER-NPLAY]", "\n");
            pattern = pattern.Replace("[ENTER-RPLAY]", "\r");

            string dukien2 = f["DuKien2"].ToString();
            if (string.IsNullOrEmpty(dukien2))
            {
                ViewData["Loi4"] = "Trường này không được để trống!";
                return this.String_Split_Regex();
            }

            dukien2 = dukien2.Replace("[TAB-TPLAY]", "\t");
            dukien2 = dukien2.Replace("[ENTER-NPLAY]", "\n");
            dukien2 = dukien2.Replace("[ENTER-RPLAY]", "\r");

            string X = f["X"].ToString();

            X = X.Replace("[TAB-TPLAY]", "\t");
            X = X.Replace("[ENTER-NPLAY]", "\n");
            X = X.Replace("[ENTER-RPLAY]", "\r");
            //if (string.IsNullOrEmpty(X))
            //{
            //    ViewData["Loi5"] = "Trường này không được để trống!";
            //    return this.String_Split_Regex();
            //}

            string Y = f["Y"].ToString();
            Y = Y.Replace("[TAB-TPLAY]", "\t");
            Y = Y.Replace("[ENTER-NPLAY]", "\n");
            Y = Y.Replace("[ENTER-RPLAY]", "\r");
            //if (string.IsNullOrEmpty(Y))
            //{
            //    ViewData["Loi6"] = "Trường này không được để trống!";
            //    return this.String_Split_Regex();
            //}

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

            TextCopy.ClipboardService.SetText(result);

            //            string re = "\r\n" + result;

            //re = re.Replace("\r\n", "<br>");

            result = "<textarea style=\"color:blue\" rows=\"50\" cols=\"150\" readonly=\"true\" autofocus>" + result + "</textarea>";

            ViewBag.Result = result;

            ViewBag.KetQua = "Thành công! Một kết quả đã được hiển thị ở cuối trang này!";

            return View();
        }

        //--------------------------------------------

        [HttpGet]
        public ActionResult Check_Regex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Check_Regex(IFormCollection f)
        {
            string chuoi = f["Chuoi"].ToString();
            if (string.IsNullOrEmpty(chuoi))
            {
                ViewData["Loi1"] = "Trường này không được để trống!";
                return this.Check_Regex();
            }
            chuoi = chuoi.Replace("[TAB-TPLAY]", "\t");
            chuoi = chuoi.Replace("[ENTER-NPLAY]", "\n");
            chuoi = chuoi.Replace("[ENTER-RPLAY]", "\r");

            string pattern = f["Pattern"].ToString();
            pattern = pattern.Replace("[TAB-TPLAY]", "\t");
            pattern = pattern.Replace("[ENTER-NPLAY]", "\n");
            pattern = pattern.Replace("[ENTER-RPLAY]", "\r");
            if (string.IsNullOrEmpty(pattern))
            {
                ViewData["Loi2"] = "Trường này không được để trống!";
                return this.Check_Regex();
            }

            while (pattern.Contains(@"\\") == true)
            {
                pattern = pattern.Replace(@"\\", @"\");
            }

            bool result = Regex.IsMatch(chuoi, pattern);
            
            ViewBag.KetQua = "Chuỗi : "+chuoi.Replace(" ","[Space]").Replace("\t","[Tab]")+" |-| Pattern : "+pattern.Replace(" ", "[Space]").Replace("\t", "[Tab]") + " |-| ==> KẾT QUẢ :  " + result;
            return View();
        }
    }
}
