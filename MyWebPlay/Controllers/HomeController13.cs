﻿using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Extension;
using MyWebPlay.Model;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyWebPlay.Controllers
{
    public partial class HomeController : Controller
    {
        public ActionResult ViewNoteFile()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "note", "textnote.txt");
            if (System.IO.File.Exists(path))
            {
                ViewBag.Text = System.IO.File.ReadAllText(path);
            }
            return View();
        }

        public ActionResult EditTextNote()
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "note", "textnote.txt");
            if (System.IO.File.Exists(path))
            {
                ViewBag.Text = System.IO.File.ReadAllText(path);
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditTextNote(string? txtText)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "note", "textnote.txt");
            
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            if (txtText != null)
            System.IO.File.WriteAllText(path, txtText);


            Calendar x = CultureInfo.InvariantCulture.Calendar;

            string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            //DateTime dt = DateTime.ParseExact(x.AddHours(DateTime.UtcNow, 7).ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string name = Request.HttpContext.Connection.RemoteIpAddress + ":" + Request.HttpContext.Connection.RemotePort + " - " + xuxu;

            if (txtText != null)
                SendEmail.SendMail2Step("mywebplay.savefile@gmail.com",
    "mywebplay.savefile@gmail.com", "Save Temp - Edit Text Note In " + name, txtText, "teinnkatajeqerfl");

            return RedirectToAction("ViewNoteFile");
        }

        public ActionResult PlayQuestion_Multiple()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PlayQuestion_Multiple (IFormCollection f, List<IFormFile> txtFile)
        {
            int sl = txtFile.Count();
            string txtSoCau = f["txtSoCau"].ToString();
            string txtTime = f["txtTime"].ToString();
            string txtMon = f["txtMon"].ToString();

            if (string.IsNullOrEmpty(txtMon))
            {
                ViewData["Loi"] = "Không được bỏ trống trường này";
                return this.PlayQuestion_Multiple();
            }

            if (string.IsNullOrEmpty(txtSoCau))
            {
                ViewData["Loi2"] = "Không được bỏ trống trường này";
                return this.PlayQuestion_Multiple();
            }

            if (string.IsNullOrEmpty(txtTime))
            {
                ViewData["Loi3"] = "Không được bỏ trống trường này";
                return this.PlayQuestion_Multiple();
            }

            int time = int.Parse(txtTime);
            if (time < 1 || time > 1200)
            {
                ViewData["Loi3"] = "Thời gian làm bài phải tối thiểu 1 phút và không vượt quá 20 giờ...";

                return this.PlayQuestion_Multiple();
            }

            if (txtFile.Count() <= 0)
            {
                ViewData["Loi1"] = "Mời bạn chọn file TXT trắc nghiệm (có thể chọn nhiều file thể hiện một môn học trắc nghiệm có nhiều chương/mục/phần/bài)...";
                return this.PlayQuestion_Multiple();
            }


            int n9_S = 0;

            //------

            TracNghiem[] tn = new TracNghiem[sl];

            for (int h = 0; h < txtFile.Count(); h++)
            {
                tn[h] = new TracNghiem();

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem", Path.GetFileName(txtFile[h].FileName));

                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    txtFile[h].CopyTo(fileStream);

                }

                String ND_file = docfile(path);

                FileInfo fx = new FileInfo(path);
                fx.Delete();

                if (ND_file.Length == 0)
                {
                    ViewData["Loi1"] = "Bài kiểm tra hay file văn bản chương (hoặc file của bạn tải lên thứ) " + (h + 1) + " của bạn hiện chưa có nội dung nào!";
                    return this.PlayQuestion_Multiple();
                }

                String[] split = { "\n#\n" };
                String[] t1 = ND_file.Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < t1.Length; i++)
                {
                    String[] t2 = t1[i].Split('\n');
                    if (t2.Length != 6)
                    {

                        string err = "WRONG INDEX QUESTION [CHƯƠNG/FILE " + (i + 1) + "] : " + t2[0] + "";
                        //err += "Định dạng file bạn đã chọn không đúng cú pháp (vui lòng kiểm tra và thử chọn lại file văn bản hoặc liên hệ Admin)! \n\n[CHÚ Ý : Kí tự # dùng để báo hiệu khoảng cách biệt mỗi câu, vì vậy tránh sử dụng # xuất hiện trong mỗi phần câu hỏi, còn $ dùng đế xác định câu hỏi không cần hoán vị đáp án. Xin lỗi vì sự bất tiện này! ]";
                        ViewData["Loi1"] = err;
                        return this.PlayQuestion_Multiple();
                    }

                    char[] t2_x = t2[5].ToCharArray();
                    if (t2_x[0] != '[' || t2_x[t2[5].Length - 1] != ']')
                    {
                        string err = "WRONG INDEX QUESTION [CHƯƠNG/FILE " + (i + 1) + "] : " + t2[0] + "";
                        //err += "Định dạng file bạn đã chọn không đúng cú pháp (vui lòng kiểm tra và thử chọn lại file văn bản hoặc liên hệ Admin)! \n\n[CHÚ Ý : Kí tự # dùng để báo hiệu khoảng cách biệt mỗi câu, vì vậy tránh sử dụng # xuất hiện trong mỗi phần câu hỏi, còn $ dùng đế xác định câu hỏi không cần hoán vị đáp án. Xin lỗi vì sự bất tiện này! ]";

                        ViewData["Loi1"] = err;
                        return this.PlayQuestion_Multiple();
                    }
                }

                //-------------------

                for (int i = 0; i < t1.Length; i++)
                {
                    String[] t2 = t1[i].Split('\n');
                    int flag = 0;
                    String DA = t2[t2.Length - 1].Replace("[", "");
                    DA = DA.Replace("]", "");
                    for (int j = t2.Length - 2; j > 0; j--)
                    {
                        if (DA.CompareTo(t2[j]) == 0)
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        //MessageBox.Show("Định dạng file bạn đã chọn không đúng cú pháp (vui lòng kiểm tra và thử chọn lại file văn bản hoặc liên hệ Admin)! \n\n[CHÚ Ý : Kí tự # dùng để báo hiệu khoảng cách biệt mỗi câu, vì vậy tránh sử dụng # xuất hiện trong mỗi phần câu hỏi.\nXin lỗi vì sự bất tiện này! ]");
                        ViewData["Loi1"] = "WRONG INDEX ANSWER OF QUESTION [CHƯƠNG/FILE " + (h + 1) + "] : " + t2[0] + "";
                        return this.TracNghiem_Multiple(ViewBag.SL);
                    }
                }

                int[] chuaxet_ch;
                int[][] chuaxet_da;

                int n9 = t1.Length;
                n9_S += n9;

                tn[h].ch = new String[t1.Length];
                tn[h].a = new String[t1.Length];
                tn[h].b = new String[t1.Length];
                tn[h].c = new String[t1.Length];
                tn[h].d = new String[t1.Length];
                tn[h].dung = new String[t1.Length];

                chuaxet_ch = new int[t1.Length];

                chuaxet_da = new int[t1.Length][];

                for (int i = 0; i < t1.Length; i++)
                    chuaxet_da[i] = new int[5];

                //=======

                int dem = 0;
                while (true)
                {
                    if (dem == t1.Length)
                        break;


                    Random r = new Random();
                    double x = r.Next(0, t1.Length);

                    if (chuaxet_ch[int.Parse(x.ToString())] == 1)
                        continue;

                    chuaxet_ch[int.Parse(x.ToString())] = 1;
                    int i = int.Parse(x.ToString());
                    String[] t2 = t1[i].Split('\n');

                    char[] CH = t2[0].ToCharArray();

                    if (CH[0] == '$')
                    {
                        t2[0].Remove(0, 1);
                        tn[h].ch[dem] = t2[0].Replace("$", "");
                        tn[h].a[dem] = t2[1];
                        tn[h].b[dem] = t2[2];
                        tn[h].c[dem] = t2[3];
                        tn[h].d[dem] = t2[4];
                        String DAx = t2[5].Replace("[", "");
                        DAx = DAx.Replace("]", "");
                        tn[h].dung[dem] = DAx;
                    }
                    else
                    {
                        int aa, bb, cc, dd;

                        tn[h].ch[dem] = t2[0];

                        do
                        {
                            aa = r.Next(1, 5);
                        }
                        while (chuaxet_da[dem][int.Parse(aa.ToString())] == 1);
                        chuaxet_da[dem][int.Parse(aa.ToString())] = 1;


                        tn[h].a[dem] = t2[aa];

                        do
                        {
                            bb = r.Next(1, 5);
                        }
                        while (chuaxet_da[dem][int.Parse(bb.ToString())] == 1);
                        chuaxet_da[dem][int.Parse(bb.ToString())] = 1;


                        tn[h].b[dem] = t2[bb];

                        do
                        {
                            cc = r.Next(1, 5);
                        }
                        while (chuaxet_da[dem][int.Parse(cc.ToString())] == 1);
                        chuaxet_da[dem][int.Parse(cc.ToString())] = 1;

                        tn[h].c[dem] = t2[cc];

                        do
                        {
                            dd = r.Next(1, 5);
                        }
                        while (chuaxet_da[dem][int.Parse(dd.ToString())] == 1);
                        chuaxet_da[dem][int.Parse(dd.ToString())] = 1;

                        tn[h].d[dem] = t2[dd];
                        String DA = t2[5].Replace("[", "");
                        DA = DA.Replace("]", "");
                        tn[h].dung[dem] = DA;
                    }
                    dem++;
                }
                tn[h].tongsocau = n9;
            }

            TracNghiem tnX = new TracNghiem();
            tnX.ch = new String[int.Parse(txtSoCau)];
            tnX.a = new String[int.Parse(txtSoCau)];
            tnX.b = new String[int.Parse(txtSoCau)];
            tnX.c = new String[int.Parse(txtSoCau)];
            tnX.d = new String[int.Parse(txtSoCau)];
            tnX.dung = new String[int.Parse(txtSoCau)];

            tnX.tongsocau = n9_S;

            if (txtSoCau.Length > 0 && int.Parse(txtSoCau) > n9_S)
            {
                txtSoCau = n9_S.ToString();
            }

            tnX.gioihancau = int.Parse(txtSoCau);

            int[][] chuaxetX = new int[sl][];

            for (int i = 0; i < sl; i++)
            {
                chuaxetX[i] = new int[tn[i].tongsocau];
                for (int j = 0; j < chuaxetX[i].Length; j++)
                {
                    chuaxetX[i][j] = 0;
                }
            }


            for (int i = 0; i < tnX.gioihancau; i++)
            {
                Random r = new Random();
                int chuong;
                int soluong;
                do
                {
                    chuong = r.Next(0, sl);
                    soluong = r.Next(0, tn[chuong].tongsocau);
                }
                while (chuaxetX[chuong][soluong] == 1);

                chuaxetX[chuong][soluong] = 1;

                tnX.ch[i] = tn[chuong].ch[soluong];
                tnX.a[i] = tn[chuong].a[soluong];
                tnX.b[i] = tn[chuong].b[soluong];
                tnX.c[i] = tn[chuong].c[soluong];
                tnX.d[i] = tn[chuong].d[soluong];
                tnX.dung[i] = tn[chuong].dung[soluong];
            }

            tnX.timelambai = int.Parse(txtTime);
            tnX.tenmon = txtMon;

            ViewBag.TimeLamBai = tnX.timelambai;

            HttpContext.Session.SetObject("TracNghiem", tnX);
            ViewBag.KetQuaDung = "";

            return View("PlayQuestion", tnX);
        }

        public ActionResult PlayQuestion(TracNghiem tn)
        {
            return View(tn);
        }

        [HttpPost]
        public ActionResult PlayQuestion(IFormCollection f)
        {
            TracNghiem tn = HttpContext.Session.GetObject<TracNghiem>("TracNghiem");

            int dung = 0;
            int sai = 0;
            int chualam = 0;

            for (int i = 0; i < tn.gioihancau; i++)
            {
                string da = f["dapan-" + i].ToString();

                if (da == "")
                {
                    chualam++;

                    ViewData["KetQua-" + i] = "<h2 style=\"color:orange\">CHƯA TRẢ LỜI</h2>";
                    ViewData["dapandung-" + i] = "<b><span style=\"color:deeppink\">Câu trả lời đúng</span> : " + tn.dung[i] + "</b>";
                }
                else if (da == tn.dung[i])
                {
                    dung++;
                    ViewData["dapandachon-" + i] = "<b><span style=\"color:deeppink\">Câu trả lời của bạn</span> : " + da + "</b>";
                    ViewData["KetQua-" + i] = "<h2 style=\"color:green\">ĐÚNG</h2>";
                }
                else if (da != tn.dung[i])
                {
                    ViewData["KetQua-" + i] = "<h2 style=\"color:red\">SAI</h2><b style=\"color:purple\">Nội dung answer của bạn có thể đúng, nhưng cách mà bạn nhập nó không match fit với answer trong file của bạn đã tải lên<br>(bao gồm phân biệt kí tự in hoa/thường, có dấu, các khoảng trắng,...vv...)</b><br><br>";
                    ViewData["dapandachon-" + i] = "<b><span style=\"color:deeppink\">Câu trả lời của bạn </span> : " + da + "</b>";
                    ViewData["dapandung-" + i] = "<b><span style=\"color:deeppink\">Câu trả lời đúng</span> : " + tn.dung[i] + "</b>";
                    sai++;
                }
            }

            ViewBag.KetQuaDung = "Số câu đúng : " + dung;
            ViewBag.KetQuaSai = "Số câu sai : " + sai;
            ViewBag.KetQuaChuaLam = "Số câu chưa làm : " + chualam;

            double diem = ((double)10 / (double)tn.gioihancau) * dung;

            diem = Math.Round(diem, 1);

            ViewBag.KetQuaDiem = "Điểm Đánh Giá : " + diem + "/10";
            return View(tn);
        }

        public ActionResult CreateFile_Question()
        {
            if (ViewBag.ChuoiVD == null)
                ViewBag.ChuoiVD = "1+1=?\r\n2\r\nHà có 5 quả cam, Hà được Lan cho thêm 3 quả cam. Hỏi Hà có tất cả bao nhiêu quả cam?\r\n8 quả\r\nTìm x biết x * 2 = 18?\r\nx = 9\r\nĐây là ai trong nhóm Winx?<br><img src=\"https://i.redd.it/dlrwc6cqztg61.jpg\" alt=\"Image Error\"><br>\r\nStella\r\n<span style=\"color:red\">Hạnh phúc</span> là gì?\r\nLà niềm vui, là sự bình yên trong tâm hồn, là những ước mơ...";

            return View();
        }

        [HttpPost]
        public ActionResult CreateFile_Question(IFormCollection f)
        {
            var listFile = System.IO.Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem"));

            foreach (var file in listFile)
            {
                FileInfo fx = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem", file));
                fx.Delete();
            }

            String[] ss = Regex.Split(f["txtChuoi"].ToString(),"\r\n");

            String s = "";
            for (int i =0; i<ss.Length;i = i+2)
            {
                s += ss[i] + "\n\n\n\n" + ss[i + 1] + "\n[" + ss[i + 1] + "]\n#\n";
            }

            char[] dd = { '\n' , '#', '\n' };
            s = s.TrimEnd(dd);

            Calendar x = CultureInfo.InvariantCulture.Calendar;

            string xuxu = x.AddHours(DateTime.UtcNow, 7).ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            string fi = Request.HttpContext.Connection.RemoteIpAddress + "_Question_" + xuxu + ".txt";
            fi = fi.Replace("\\", "");
            fi = fi.Replace("/", "");
            fi = fi.Replace(":", "");

            var path = Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem", fi);

            System.IO.File.WriteAllText(path, s);

            //------------------------------------

            ViewBag.ChuoiVD = "1+1=?\r\n2\r\nHà có 5 quả cam, Hà được Lan cho thêm 3 quả cam. Hỏi Hà có tất cả bao nhiêu quả cam?\r\n8 quả\r\nTìm x biết x * 2 = 18?\r\nx = 9\r\nĐây là ai trong nhóm Winx?<br><img src=\"https://i.redd.it/dlrwc6cqztg61.jpg\" alt=\"Image Error\"><br>\r\nStella\r\n<span style=\"color:red\">Hạnh phúc</span> là gì?\r\nLà niềm vui, là sự bình yên trong tâm hồn, là những ước mơ...";

            //DateTime dt = DateTime.ParseExact(x.AddHours(DateTime.UtcNow, 7).ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string name = Request.HttpContext.Connection.RemoteIpAddress + ":" + Request.HttpContext.Connection.RemotePort + " - " + xuxu;

            SendEmail.SendMail2Step("mywebplay.savefile@gmail.com",
"mywebplay.savefile@gmail.com", "Save Temp Create Question Answer File In " + name, s, "teinnkatajeqerfl");


            ViewBag.KetQua = "<p style=\"color:blue\">Thành công, một file TXT question/answer của bạn đã được xử lý...</p><a href=\"/tracnghiem/" + fi + "\" download>Click vào đây để tải về</a><br><p style=\"color:red\">Hãy nhanh tay tải về vì sau <span style=\"color:yellow\" id=\"thoigian2\" class=\"thoigian2\">20</span> giây nữa, file này sẽ bị xoá hoặc sẽ bị lỗi nếu có!<br>Nếu file tải về của bạn bị lỗi hoặc chưa kịp tải về, hãy refresh/quay lại trang này và thử lại...<br><span style=\"color:aqua\">Mặc dù file này đã được thông qua một số xử lý, tuy nhiên nó vẫn có thể xảy ra lỗi và sai sót không mong muốn...</span></p>";

            return View();
        }

        public ActionResult XoaAllFile_X2()
        {
            var listFile = System.IO.Directory.GetFiles(Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem"));

            foreach (var file in listFile)
            {
                FileInfo f = new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, "tracnghiem", file));
                f.Delete();
            }
            return RedirectToAction("PlayQuestion_Multiple");
        }
    }
}
