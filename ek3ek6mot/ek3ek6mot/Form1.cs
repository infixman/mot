using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Data.SQLite;

namespace MOT
{
    public partial class Form1 : Form
    {
        private Configuration _Config;
        private string _LpmsPath;
        private string _PicPath;
        private string _PicPath2;
        //private string _DbPath;
        //private string _CnString;
        private bool _ProcessStatus;
        //private byte[] _PicByte;
        private readonly byte[] _PngStartSequence = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
		private readonly byte[] _JpgStartSequence = new byte[] { 0x4A, 0x46, 0x49, 0x46 }; //JFIF
        private readonly byte[] _JpgStartSequence2 = new byte[] { 0xFF, 0xD8 };
        //private readonly byte[] _PngEndSequence = new byte[] { 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };
        //private readonly byte[] _JpgEndSequence = new byte[] { 0xFF, 0xD9 };
        private readonly byte[] _Magic4Sequence = new byte[] { 0x34, 0x00, 0x00, 0x00 };        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SetLpmsPath();
            txt_PicPath.Text = GetConfig("PicPath");
            txt_PicPath2.Text = GetConfig("PicPath2");
            chk_MotOldFile.Checked = (GetConfig("MotOldFile") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            chk_PrintWaterMark.Checked = (GetConfig("PrintWaterMark") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            chk_PrintClock.Checked = (GetConfig("PrintClock") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            chk_StartAfterRun.Checked = (GetConfig("StartAfterRun") ?? "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            notify_icon.Text = "MOT執行中";
            //_DbPath = @".\Mot.sqlite";
            //_CnString = $"data source={_DbPath}";
            _ProcessStatus = false;

            if (chk_StartAfterRun.Checked)
            {
                btn_Start.PerformClick();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void btn_PickPath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txt_LpmsPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btn_PickPic_Click(object sender, EventArgs e)
        {
            txt_PicPath.Text = PickPic();
            SetConfig("PicPath", txt_PicPath.Text);
        }

        private void btn_PickPic2_Click(object sender, EventArgs e)
        {
            txt_PicPath2.Text = PickPic();
            SetConfig("PicPath2", txt_PicPath2.Text);
        }

        private void btn_Start_Click(object sender, EventArgs e)
		{
			if (_ProcessStatus) //要停止
            {
                _ProcessStatus = false;
                EnableUI(true); //UI自由
                Log("已停止母湯");
                notify_icon.Text = "MOT執行中，狀態為: 停止";
            }
            else //要開始
            {
                _LpmsPath = txt_LpmsPath.Text.Trim();
                _PicPath = txt_PicPath.Text.Trim();
                _PicPath2 = txt_PicPath2.Text.Trim();

                SetConfig("LpmsPath", txt_LpmsPath.Text);
                SetConfig("PicPath", txt_PicPath.Text);
                SetConfig("PicPath2", txt_PicPath2.Text);

                var check = CheckBeforeStart();
				if (!check.Checked)
				{
					Log(check.Message);
					return;
				}

				EnableUI(false); //UI唯讀
                txt_Log.Text = "";
                Log("LPM逐漸開始母湯");
                notify_icon.Text = "MOT執行中，狀態為: 開始";
                _ProcessStatus = true;
                    
                StartWork();
            }
        }

        private void SetLpmsPath()
        {
            txt_LpmsPath.Text = GetConfig("LpmsPath");
            if (string.IsNullOrWhiteSpace(txt_LpmsPath.Text))
            {
                txt_LpmsPath.Text = $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\JLI_XFORT\\HelpClr\\LOGS\\LPMs";
            }
        }

        private string PickPic()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "PNG、JPG (*.png;*.jpg)|*.png;*.jpg";
                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                {
                    return ofd.FileName;
                }
            }

            return string.Empty;
        }

        private (bool Checked, string Message) CheckBeforeStart()
        {
            if (string.IsNullOrWhiteSpace(_LpmsPath)
                || string.IsNullOrWhiteSpace(_PicPath)
                || string.IsNullOrWhiteSpace(_PicPath2))
            {
                return (false, "路徑為空，請重新選擇");
            }

            if (!Directory.Exists(_LpmsPath))
            {
                return (false, "LPMs資料夾不存在");
            }

            if (!File.Exists(_PicPath))
            {
                return (false, "螢幕1圖檔不存在");
            }
            if (!File.Exists(_PicPath2))
            {
                return (false, "螢幕2圖檔不存在");
            }

            //var checkSqlite = CheckForSQLite();
            //return (checkSqlite.Check, checkSqlite.Message);

            return (true, null);
        }

        //private (bool Check, string Message) CheckForSQLite()
        //{
        //    try
        //    {
        //        if (!File.Exists(_DbPath))
        //        {
        //            SQLiteConnection.CreateFile(@".\Mot.sqlite");
        //        };
        //        using (var cn = new SQLiteConnection(_CnString))
        //        {
        //            var findLPMS = false;
        //            cn.Open();
        //            var cmd = new SQLiteCommand("SELECT [name] FROM [sqlite_master] WHERE type = 'table' AND name = 'LPMS';", cn);
        //            SQLiteDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                if (reader[0].ToString() == "LPMS")
        //                {
        //                    findLPMS = true;
        //                }
        //            }
        //            reader.Close();
        //            reader.Dispose();
        //            cmd.Dispose();
        //            if (!findLPMS)
        //            {
        //                cmd = new SQLiteCommand("CREATE TABLE [LPMS] ([FileName] nvarchar(32), [UpdateTime] datetime);", cn);
        //                cmd.ExecuteNonQuery();
        //            }
        //            cmd.Dispose();                    
        //            cmd = new SQLiteCommand($"INSERT INTO [LPMS] ([FileName], [UpdateTime]) VALUES ('TEST', '{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}');", cn);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();
        //            cmd = new SQLiteCommand($"DELETE FROM [LPMS] WHERE [FileName] = 'TEST';", cn);
        //            cmd.ExecuteNonQuery();
        //            cmd.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, $"Log資料庫建立失敗\r\n{ex.Message}");
        //    }
        //    return (true, null);
        //}

        private void Log(string log)
        {
            if (txt_Log.InvokeRequired)
            {
                txt_Log.Invoke((ThreadStart)(() => Log(log)));
            }
            else
            {
				if (txt_Log.Lines.Count() > 99)
				{
					txt_Log.Text = txt_Log.Text.Remove(0, ($"{txt_Log.Lines[0]}\n").Length);
				}

				txt_Log.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss - ")}{log}"+ Environment.NewLine);
            }
        }

        private string GetConfig(string key)
        {
            if (!_Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                SetConfig(key, string.Empty);
            }
            return _Config.AppSettings.Settings[key].Value.Trim();
        }

        private void SetConfig(string key, string value)
        {
            if (_Config.AppSettings.Settings.AllKeys.Contains(key))
            {
                _Config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                _Config.AppSettings.Settings.Add(key, value);
            }
            _Config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void EnableUI(bool status)
        {
            txt_LpmsPath.ReadOnly = !status;
            txt_PicPath.ReadOnly = !status;
            txt_PicPath2.ReadOnly = !status;
            btn_PickPath.Enabled = status;
            btn_PickPic.Enabled = status;
            btn_PickPic2.Enabled = status;
            chk_MotOldFile.Enabled = status;
            chk_PrintClock.Enabled = status;
            chk_PrintWaterMark.Enabled = status;
            chk_StartAfterRun.Enabled = status;
            btn_Start.Text = status ? "開始" : "停止";
            //notify_icon.Visible = false;
            notify_icon.Icon = status ? Properties.Resources.icon_b : Properties.Resources.icon_w;
            notify_icon.Visible = true;
        }

        private void StartWork()
        {
            //_PicByte = File.ReadAllBytes(_PicPath);

            if (chk_MotOldFile.Checked)
            {
                //先把資料夾內的檔案都替換一次
                var files = Directory.GetFiles(_LpmsPath);
                foreach (var file in files)
                {
                    if (file.Split('.').Last().Equals("LPM", StringComparison.OrdinalIgnoreCase))
                    {
                        var result = ReplaceLpmPic(file);
                        if (result.Done)
                        {
                            Log($"已替換{file.Split('\\').Last()}");
                        }
                        else
                        {
                            Log($"無法替換{file.Split('\\').Last()}: {result.Msg}");
                        }
                    }
                }
            }

            //開始監視截圖LPM
            var fswPicLpm = new FileSystemWatcher(_LpmsPath, "*.LPM")
            {
                IncludeSubdirectories = true
            };

            fswPicLpm.Created += FswPicLpm_Changed;
            fswPicLpm.EnableRaisingEvents = true;

            //開始監視瀏覽器LPM
            var tmpPathArr = _LpmsPath.Split('\\');
            var browserLpmsPath = string.Join("\\", tmpPathArr.Take(tmpPathArr.Count() - 1).ToArray());
            var fswBrowserLpm = new FileSystemWatcher(browserLpmsPath, "*.LPM")
            {
                IncludeSubdirectories = true
            };

            fswBrowserLpm.Created += FswBrowserLpm_Changed;
            fswBrowserLpm.EnableRaisingEvents = true;
        }

        private (bool Done, string Msg) ReplaceLpmPic(string lpmFilePath)
        {
            //先取消唯讀
            var attr = File.GetAttributes(lpmFilePath);
            attr = attr & ~FileAttributes.ReadOnly;
            File.SetAttributes(lpmFilePath, attr);
            
            try
            {
                var lpmFile = File.ReadAllBytes(lpmFilePath);
                var magic4Index = IndexOf(lpmFile, _Magic4Sequence);
                var picStartIndex = IndexOf(lpmFile, _PngStartSequence);

                //找不到PNG，找JPG(JFIF)減6就是JPG開頭
                if (picStartIndex == -1)
                {
                    picStartIndex = IndexOf(lpmFile, _JpgStartSequence);

                    //找到JFIF但是檔頭不是JPG規範的開頭 FF D8
                    if (picStartIndex != -1)
                    {
                        picStartIndex = picStartIndex - 6;

                        if (lpmFile[picStartIndex] != 0xFF
                        || lpmFile[picStartIndex + 1] != 0xD8)
                        {
                            return (false, "Find JFIF, But not jpg format.");
                        }
                    }

                    //找不到PNG，找JPG(最不保險的方法，FF D8開頭)
                    if (picStartIndex == -1)
                    {
                        picStartIndex = IndexOf(lpmFile, _JpgStartSequence2);
                    }
                }

                if (magic4Index != -1 && picStartIndex != -1)
                {
                    //1.準備header
                    var header = new byte[56];
                    Array.Copy(lpmFile, 0, header, 0, 56);

                    //2.準備假Title
                    //Windows Default Lock Screen  LockApp.exe
                    var title = new byte[] { 0x57, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x64, 0x00, 0x6F, 0x00, 0x77, 0x00, 0x73, 0x00, 0x20, 0x00, 0x44, 0x00, 0x65, 0x00, 0x66, 0x00, 0x61, 0x00, 0x75, 0x00, 0x6C, 0x00, 0x74, 0x00, 0x20, 0x00, 0x4C, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x6B, 0x00, 0x20, 0x00, 0x53, 0x00, 0x63, 0x00, 0x72, 0x00, 0x65, 0x00, 0x65, 0x00, 0x6E, 0x00, 0x16, 0x00, 0x00, 0x00, 0x4C, 0x00, 0x6F, 0x00, 0x63, 0x00, 0x6B, 0x00, 0x41, 0x00, 0x70, 0x00, 0x70, 0x00, 0x2E, 0x00, 0x65, 0x00, 0x78, 0x00, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    //3.準備footer
                    var footer = new byte[4];
                    Array.Copy(lpmFile, lpmFile.Length - 4, footer, 0, 4);

                    //4.假圖上浮水印
                    var picPath = footer[0] == 0x01 ? _PicPath : _PicPath2;
                    var dateString = lpmFilePath.Split('\\').Last().Split('_').Last().Replace("-", "").Split('.').First();
                    var fileTime = Convert.ToDateTime($"{dateString.Substring(0, 4)}-{dateString.Substring(4, 2)}-{dateString.Substring(6, 2)} {dateString.Substring(8, 2)}:{dateString.Substring(10, 2)}:{dateString.Substring(12, 2)}");
                    var fakePic = WaterMark(picPath, fileTime, BitConverter.ToInt32(footer, 0).ToString());

                    //5.準備假圖片
                    var fakePicMs = new MemoryStream();
                    fakePic.Save(fakePicMs, ImageFormat.Jpeg);
                    var fakePicByte = fakePicMs.ToArray();
                    var fakePicFileSize = new byte[4];
                    fakePicFileSize = BitConverter.GetBytes(fakePicByte.Length);

                    //6.準備LPM檔名
                    var orgLpmFileNameLength = picStartIndex - magic4Index - 4;
                    var orgLpmFileName = new byte[orgLpmFileNameLength];
                    Array.Copy(lpmFile, magic4Index + 4, orgLpmFileName, 0, orgLpmFileNameLength);

                    //7.組成假LPM
                    var nowLength = 0;
                    var fakeLpmFile = new byte[header.Length + title.Length + _Magic4Sequence.Length + orgLpmFileName.Length + fakePicFileSize.Length + fakePicByte.Length + footer.Length];

                    header.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + header.Length;

                    title.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + title.Length;

                    _Magic4Sequence.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + _Magic4Sequence.Length;

                    orgLpmFileName.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + orgLpmFileName.Length;

                    fakePicFileSize.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + fakePicFileSize.Length;

                    fakePicByte.CopyTo(fakeLpmFile, nowLength);
                    nowLength = nowLength + fakePicByte.Length;

                    footer.CopyTo(fakeLpmFile, nowLength);                    

                    //8.準備假LPM大小
                    var fakeLpmFileSize = new byte[4];
                    fakeLpmFileSize = BitConverter.GetBytes(fakeLpmFile.Length);

                    //9.更改圖片大小
                    var fakePicFileSizeIndex = picStartIndex - 4;
                    fakeLpmFile[fakePicFileSizeIndex] = fakePicFileSize[0];
                    fakeLpmFile[fakePicFileSizeIndex + 1] = fakePicFileSize[1];
                    fakeLpmFile[fakePicFileSizeIndex + 2] = fakePicFileSize[2];
                    fakeLpmFile[fakePicFileSizeIndex + 3] = fakePicFileSize[3];

                    //10.更改LPM大小
                    fakeLpmFile[8] = fakeLpmFileSize[0];
                    fakeLpmFile[9] = fakeLpmFileSize[1];
                    fakeLpmFile[10] = fakeLpmFileSize[2];
                    fakeLpmFile[11] = fakeLpmFileSize[3];

                    //11.寫入LPM
                    using (Stream stream = File.Open(lpmFilePath, FileMode.Open))
                    {
                        stream.Position = 0;
                        stream.Write(fakeLpmFile, 0, fakeLpmFile.Length);
                    }

                    return (true, null);
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return (false, $"Something wrong, But i don't know why~\r\nException:{ex.Message}");
            }

            return (false, "Maybe file not PNG and JPG.");
        }

        private (bool Done, string Msg) ReplaceLpmBrowser(string lpmFilePath)
        {

            return (false, "Q");
        }

        private void FswPicLpm_Changed(object sender, FileSystemEventArgs e)
        {
            if (WatcherChangeTypes.Created == e.ChangeType)
            {
                if (_ProcessStatus)
                {
                    if (e.FullPath.Split('.').Last().Equals("LPM", StringComparison.OrdinalIgnoreCase))
                    {
                        var result = ReplaceLpmPic(e.FullPath);
                        if (result.Done)
                        {
                            Log($"已替換{e.FullPath.Split('\\').Last()}");
                        }
                        else
                        {
                            Log($"無法替換{e.FullPath.Split('\\').Last()}: {result.Msg}");
                        }
                    }
                }
            }
        }

        private void FswBrowserLpm_Changed(object sender, FileSystemEventArgs e)
        {
            if (WatcherChangeTypes.Created == e.ChangeType)
            {
                if (_ProcessStatus)
                {
                    if (e.FullPath.Split('.').Last().Equals("LPM", StringComparison.OrdinalIgnoreCase))
                    {
                        var result = ReplaceLpmBrowser(e.FullPath);
                        if (result.Done)
                        {
                            Log($"已替換{e.FullPath.Split('\\').Last()}");
                        }
                        else
                        {
                            Log($"無法替換{e.FullPath.Split('\\').Last()}: {result.Msg}");
                        }
                    }
                }
            }
        }

        private int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        private Image WaterMark(string fileName, DateTime now, string monitor)
        {
            var fontSizeWaterMark = 60;

            //準備一個1920*1080全黑畫布
            var fakeBitmap = new Bitmap(1920, 1080, PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, 1920, 1080);
            var fakeGraph = Graphics.FromImage(fakeBitmap);
            fakeGraph.DrawRectangle(Pens.Black, rect);
            fakeGraph.FillRectangle(Brushes.Black, rect);

            //讀取檔案
            var orgImg = Bitmap.FromFile(fileName);
            fakeGraph.DrawImage(orgImg, new Rectangle(0, 0, orgImg.Width, orgImg.Height), new Rectangle(0, 0, orgImg.Width, orgImg.Height), GraphicsUnit.Pixel);
            fakeGraph.Save();
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;
            var f = new Font("Verdana", fontSizeWaterMark, FontStyle.Bold, GraphicsUnit.Pixel);
            var p = new Pen(ColorTranslator.FromHtml("#FFFFFF"), 2);
            p.LineJoin = LineJoin.Round;

            #region 右下浮水印
            var fr = new Rectangle(0, fakeBitmap.Height - f.Height, fakeBitmap.Width, f.Height);
            var b = new LinearGradientBrush(fr, ColorTranslator.FromHtml("#000000"), ColorTranslator.FromHtml("#000000"), 90);
            var x = 150;
            var y = 974;
            var xShift = 38;

            if (chk_PrintWaterMark.Checked)
            {
                foreach (char c in now.ToString("yyyy-MM-dd"))
                {
                    var r = new Rectangle(x, y, fakeBitmap.Width, fakeBitmap.Height);
                    var gp = new GraphicsPath();
                    gp.AddString(c.ToString(), f.FontFamily, (int)f.Style, fontSizeWaterMark, r, sf);
                    fakeGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    fakeGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    fakeGraph.DrawPath(p, gp);
                    fakeGraph.FillPath(b, gp);
                    gp.Dispose();
                    x += xShift;
                }

                x = 150 + 390;
                foreach (char c in now.ToString($"HH:mm:ss[{monitor}]"))
                {
                    var r = new Rectangle(x, y, fakeBitmap.Width, fakeBitmap.Height);
                    var gp = new GraphicsPath();
                    gp.AddString(c.ToString(), f.FontFamily, (int)f.Style, fontSizeWaterMark, r, sf);
                    fakeGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    fakeGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    fakeGraph.DrawPath(p, gp);
                    fakeGraph.FillPath(b, gp);
                    gp.Dispose();
                    x += xShift - 2;
                }
            }
            #endregion

            #region 左邊
            if (chk_PrintClock.Checked)
            {
                #region 左邊時間
                var fontSizeTime = 125;
                var f2 = new Font("Segoe UI Light", fontSizeTime, FontStyle.Regular, GraphicsUnit.Pixel);
                var b2 = new LinearGradientBrush(fr, ColorTranslator.FromHtml("#FFFFFF"), ColorTranslator.FromHtml("#FFFFF"), 1);
                var r2 = new Rectangle(-70, 760, 500, 500);
                var gp2 = new GraphicsPath();
                gp2.AddString(now.ToString("hh:mm"), f2.FontFamily, (int)f2.Style, fontSizeTime, r2, sf);
                fakeGraph.SmoothingMode = SmoothingMode.AntiAlias;
                fakeGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                fakeGraph.FillPath(b2, gp2);
                gp2.Dispose();
                #endregion

                #region 左邊日期
                var fontSizeDate = 55;
                var f3 = new Font("Segoe UI Light", fontSizeDate, FontStyle.Bold, GraphicsUnit.Pixel);
                var b3 = new LinearGradientBrush(fr, ColorTranslator.FromHtml("#FFFFFF"), ColorTranslator.FromHtml("#FFFFF"), 1);
                var r3 = new Rectangle(-45, 910, 500, 500);
                var gp3 = new GraphicsPath();
                gp3.AddString(now.ToString($"M月d日星期{GetChtDayOfWeek(now.DayOfWeek)}"), f3.FontFamily, (int)f3.Style, fontSizeDate, r3, sf);
                fakeGraph.SmoothingMode = SmoothingMode.AntiAlias;
                fakeGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                fakeGraph.FillPath(b3, gp3);
                gp3.Dispose();
                #endregion
            }
            #endregion

            b.Dispose();
            b.Dispose();
            f.Dispose();
            sf.Dispose();
            fakeGraph.Dispose();

            return fakeBitmap;
            //return orgImg;
        }

        private string GetChtDayOfWeek(DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Monday:
                    return "一";
                case DayOfWeek.Tuesday:
                    return "二";
                case DayOfWeek.Wednesday:
                    return "三";
                case DayOfWeek.Thursday:
                    return "四";
                case DayOfWeek.Friday:
                    return "五";
                case DayOfWeek.Saturday:
                    return "六";
                default:
                    return "日";
            }
        }

        private void chk_MotOldFile_CheckedChanged(object sender, EventArgs e)
        {
            SetConfig("MotOldFile", chk_MotOldFile.Checked.ToString());
        }

        private void chk_PrintWaterMark_CheckedChanged(object sender, EventArgs e)
        {
            SetConfig("PrintWaterMark", chk_PrintWaterMark.Checked.ToString());
        }

        private void chk_PrintClock_CheckedChanged(object sender, EventArgs e)
        {
            SetConfig("PrintClock", chk_PrintClock.Checked.ToString());
        }

        private void chk_StartAfterRun_CheckedChanged(object sender, EventArgs e)
        {
            SetConfig("StartAfterRun", chk_StartAfterRun.Checked.ToString());
        }
        
        private void notify_icon_DoubleClick(object sender, EventArgs e)
        {
            ShowFormFromNotifyIcon();
        }

        private void notify_icon_BalloonTipClicked(object sender, EventArgs e)
        {
            ShowFormFromNotifyIcon();
        }

        private void ShowFormFromNotifyIcon()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = true;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                var status = _ProcessStatus ? "開始" : "停止";
                var notifyMsg = $"MOT執行中，狀態為: {status}\r\n點擊系統列圖示喚回視窗\r\n　";
                notify_icon.ShowBalloonTip(2000, "MOT", notifyMsg, ToolTipIcon.Info);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notify_icon.Dispose();
        }
    }
}
