﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyWebPlay.Model;

namespace MyWebPlay.Controllers
{
    public class CoverController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CoverController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Ee90d45ca0d59031d2a3b6dc488187c00()
        {
            try
            {
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingXYZ_DarkAdmin.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (info[0] == "LockedAll_Web")
                {
                    if (info[1] == "true")
                    {
                        return RedirectToAction("Error","Home");
                    }
                }

                if (info[0] == "Play_EncodeUrl")
                {
                    if (info[1] == "false")
                        return RedirectToAction("Error","Home");
                }

                if (info[0] == "Encode_Url")
                {
                    ViewBag.Link = info[3].Split("***", StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            }
            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult E41fb870a2ab7c2470cb35f51171e20ee()
        {
            try
            {
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingXYZ_DarkAdmin.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (info[0] == "LockedAll_Web")
                {
                    if (info[1] == "true")
                    {
                        return RedirectToAction("Error","Home");
                    }
                }

                if (info[0] == "Play_EncodeUrl")
                {
                    if (info[1] == "false")
                        return RedirectToAction("Error","Home");
                }

                if (info[0] == "Encode_Url")
                {
                    ViewBag.Link = info[3].Split("***", StringSplitOptions.RemoveEmptyEntries)[1];
                    break;
                }
            }

            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }

        public ActionResult E8ecb5a3a2b4fdbda327335d19f3ca7fa()
        {
            try
            {
                var pathX = Path.Combine(_webHostEnvironment.WebRootPath, "Admin/SettingXYZ_DarkAdmin.txt");
            var noidungX = System.IO.File.ReadAllText(pathX);
            var listSetting = noidungX.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < listSetting.Length; i++)
            {
                var info = listSetting[i].Split("<3275>", StringSplitOptions.RemoveEmptyEntries);

                if (info[0] == "LockedAll_Web")
                {
                    if (info[1] == "true")
                    {
                        return RedirectToAction("Error","Home");
                    }
                }

                if (info[0] == "Play_EncodeUrl")
                {
                    if (info[1] == "false")
                        return RedirectToAction("Error","Home");
                }

                if (info[0] == "Encode_Url")
                {
                    ViewBag.Link = info[3].Split("***", StringSplitOptions.RemoveEmptyEntries)[2];
                    break;
                }
            }

            return View();
            }
            catch (Exception ex)
            {
                var req = Request.Path;

                if (req == "/" || string.IsNullOrEmpty(req))
                    req = "/Home/Index";

                HttpContext.Session.SetObject("error_exception_log", "[Exception/error log - " + req + " - " + ex.Source + "] : " + ex.Message + "\n\n" + ex.StackTrace);
                return RedirectToAction("Error", new { exception = "true" });
            }
        }
    }
}
