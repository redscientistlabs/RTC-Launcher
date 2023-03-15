namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Newtonsoft.Json.Linq;
    using RTCV.Launcher.Components;

#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    internal partial class MusicPanel : Form
    {

        public static MusicPanel mpForm
        {
            get
            {
                if (_mpForm == null)
                    _mpForm = new MusicPanel();

                return _mpForm;
            }
        }
        public static MusicPanel _mpForm = null;

        public MusicPanel()
        {
            InitializeComponent();
        }

        private void MusicPanel_Load(object sender, EventArgs e)
        {
            ProcessUI(new RedscientistMusicRequest()
            {
                action = RedscientistMusicActions.ShowMainPage
            });

        }

        private void ProcessUI(RedscientistMusicRequest request)
        {
            flowVisiblePanel.Controls.Clear();

            switch (request.action)
            {
                case RedscientistMusicActions.ShowMainPage:
                    DrawMainPage(request);
                    break;
                case RedscientistMusicActions.ShowMusicArtists:
                    DrawMusicArtistsPage(request);
                    break;
                case RedscientistMusicActions.ShowArtist:
                    DrawArtistPage(request);
                    break;
            }

            flowVisiblePanel.Focus();
        }

        private void DrawMainPage(RedscientistMusicRequest request)
        {
            lbPanelText.Text = "Redscientist Music > News";

            DrawHeader();

            var newsDB = GetDB("news");
            List<dynamic> lastNews = new List<dynamic>();
            foreach (var news in newsDB.news)
            {
                lastNews.Add(news);

                if (lastNews.Count >= 5)
                    break;
            }

            foreach (var news in lastNews)
            {
                /*
      "date": "2022-10-15",
      "writer": "Janco",
      "albumid": "HammeredWorld",
      "artistid": "MorphingBytes",
      "content": "Morphing Bytes releases album <br>\"Hammered World\"",
      "type": "MUSIC"
                 */
                var stripe = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    MinimumSize = new Size(666, 0),
                    Margin = new Padding(32, 0, 32, 8),
                    //BackColor = Color.FromArgb(16, 16, 16),
                };

                string artistId = news.artistid;
                string albumId = news.albumid;
                Image folderArt = GetAlbumFolderArt(artistId, albumId);

                var panelArt = new Panel()
                {
                    Width = 70,
                    Height = 70,
                    BackgroundImage = folderArt,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(8, 8, 8, 8),
                    BackgroundImageLayout = ImageLayout.Stretch,
                };
                stripe.Controls.Add(panelArt);

                var textBlock = new FlowLayoutPanel()
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    Margin = new Padding(8, 16, 8, 0),
                };

                string text1 = $"Date: {news.date}     By: {news.writer}";
                string text2 = $"{(news.content.ToString()).Replace("<br>", " ").Replace("  ", " ")}";

                var labelTop = new Label()
                {
                    AutoSize = true,
                    Text = text1,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular),
                    Margin = new Padding(0, 0, 8, 0),
                };
                textBlock.Controls.Add(labelTop);

                var labelBottom = new Label()
                {
                    AutoSize = true,
                    Text = text2,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular),
                    Margin = new Padding(0, 8, 8, 0),
                };
                textBlock.Controls.Add(labelBottom);

                stripe.Controls.Add(textBlock);

                Action clickMethod = new Action(() =>
                {
                    ProcessUI(new RedscientistMusicRequest()
                    {
                        action = RedscientistMusicActions.ShowAlbum,
                        album = news.albumid,
                        artist = news.artistid,
                    });
                });

                stripe.Click += (o, e) => clickMethod.Invoke();
                panelArt.Click += (o, e) => clickMethod.Invoke();
                labelTop.Click += (o, e) => clickMethod.Invoke();
                labelBottom.Click += (o, e) => clickMethod.Invoke();

                flowVisiblePanel.Controls.Add(stripe);
            }
        }

        private void DrawMusicArtistsPage(RedscientistMusicRequest request)
        {
            lbPanelText.Text = "Redscientist Music > Artists";

            DrawHeader();

            var newsDB = GetDB("music");
            List<dynamic> artists = new List<dynamic>();
            foreach (var artist in newsDB.artists)
                artists.Add(artist);

            var scroll = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
            };

            foreach (var artist in artists)
            {
                /*
                {
                  "name": "Morphing Bytes",
                  "id": "morphingbytes"
                },
                 */
                var stripe = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    Margin = new Padding(32, 0, 0, 8),
                    MinimumSize = new System.Drawing.Size(666, 0),
                    //BackColor = Color.FromArgb(16, 16, 16),
                };

                string artistId = artist.id;
                Image bannerArt = GetBannerArt(artistId);
                var pictureBanner = new PictureBox()
                {
                    Width = 90,
                    Height = 60,
                    Image = bannerArt,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0, 0, 0, 0),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                stripe.Controls.Add(pictureBanner);

                string text = artist.name;
                var labelArtistName = new Label()
                {
                    AutoSize = true,
                    Text = text,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI Light", 22F, System.Drawing.FontStyle.Regular),
                    Margin = new Padding(8, 16, 8, 0),
                };
                stripe.Controls.Add(labelArtistName);

                Action clickMethod = new Action(() =>
                {
                    ProcessUI(new RedscientistMusicRequest()
                    {
                        action = RedscientistMusicActions.ShowArtist,
                        artist = artist.id,
                    });
                });

                stripe.Click += (o, e) => clickMethod.Invoke();
                labelArtistName.Click += (o, e) => clickMethod.Invoke();
                pictureBanner.Click += (o, e) => clickMethod.Invoke();
                scroll.Controls.Add(stripe);
            }

            flowVisiblePanel.Controls.Add(scroll);
        }

        private void DrawArtistPage(RedscientistMusicRequest request)
        {
            /*
{
"artist_id":"xyno",
"artist_name":"XyNo ?!",
"artist_subtitle":"AtariST Shredder",
"albums":
[
    {
    "id" : "GameJAHboy",
    "name" : "GameJAHBoy!",
    "year" : "2015",
    "buyseller" : "Bandcamp",
    "buyurl" : "https://xyno88.bandcamp.com/album/gamejahboy-2015",
    },
             */


            var artistDB = GetDB(request.artist);
            string artistID = request.artist;
            string artistName = artistDB.artist_name;
            string artistSubtitle = artistDB.artist_subtitle;

            string artistDescription = string.Empty;
            if (artistID != "graveyard")
                artistDescription = GetData($"{artistID}_details");


            IEnumerable<dynamic> albums = artistDB.albums;



            lbPanelText.Text = $"Redscientist Music > Artists > {artistName}";

            DrawHeader();


            var scroll = new FlowLayoutPanel()
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
            };

            {
                var stripe = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Margin = new Padding(32, 0, 0, 8),
                    MinimumSize = new System.Drawing.Size(666, 0),
                    //BackColor = Color.FromArgb(16, 16, 16),
                };

                var labelArtistName = new Label()
                {
                    AutoSize = true,
                    Text = artistName,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular),
                    Margin = new Padding(0, 0, 0, 0),
                };
                stripe.Controls.Add(labelArtistName);

                if(artistID != "graveyard")
                {

                    var labelArtistSubtitle = new Label()
                    {
                        AutoSize = true,
                        Text = artistSubtitle,
                        ForeColor = Color.White,
                        Font = new System.Drawing.Font("Segoe UI Light", 20F, System.Drawing.FontStyle.Regular),
                        Margin = new Padding(8, 0, 0, 0),
                    };
                    stripe.Controls.Add(labelArtistSubtitle);


                    if (artistDescription.Contains("href") && artistDescription.Contains("src"))
                    {
                        string pattern = "<a href=\"(?<link>.*?)\" target=\"_blank\"><img src=\"(?<image>.*?)\" style=\"cursor: pointer;width:(?<width>.*?)px;height:(?<height>.*?)px\" \\/><\\/a>";

                        Match match = Regex.Match(artistDescription, pattern);

                        //if (match.Success)
                        //{
                        //    Console.WriteLine("Link: " + match.Groups["link"].Value);
                        //    Console.WriteLine("Image: " + match.Groups["image"].Value);
                        //    Console.WriteLine("Width: " + match.Groups["width"].Value);
                        //    Console.WriteLine("Height: " + match.Groups["height"].Value);
                        //}


                        var pictureBanner = new PictureBox()
                        {
                            Width = Convert.ToInt32(match.Groups["width"].Value),
                            Height = Convert.ToInt32(match.Groups["height"].Value),
                            ImageLocation = match.Groups["image"].Value,
                            BorderStyle = BorderStyle.None,
                            Margin = new Padding(16, 8, 16, 32),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                        };
                        stripe.Controls.Add(pictureBanner);
                    }
                    else
                    {
                        var labelArtistDescription = new Label()
                        {
                            AutoSize = true,
                            Text = artistDescription.Replace("<br>", "\n"),
                            ForeColor = Color.White,
                            Font = new System.Drawing.Font("Segoe UI Semilight", 13F, System.Drawing.FontStyle.Regular),
                            Margin = new Padding(16, 8, 16, 32),
                        };
                        stripe.Controls.Add(labelArtistDescription);
                    }
                }


                scroll.Controls.Add(stripe);

            }

            foreach (var album in albums)
            {
                /*
                {
                    "id" : "RhythmPerMinute",
                    "name" : "Rhythm Per Minute",
                    "year" : "2013",
                    "buyseller" : "Bandcamp",
                    "buyurl" : "https://xyno88.bandcamp.com/album/rhythm-per-minute-2013",
                },
                 */

                string albumID = album.id;
                string albumName = album.name;
                string albumYear = album.year;
                string buySeller = album.buyseller;
                string buyUrl = album.buyurl;

                var stripe = new FlowLayoutPanel()
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    AutoSize = true,
                    Margin = new Padding(32, 0, 0, 16),
                    MinimumSize = new System.Drawing.Size(666, 0),
                    //BackColor = Color.FromArgb(16, 16, 16),
                };

                Image albumArt = GetAlbumFolderArt(artistID, albumID);
                var pictureBanner = new PictureBox()
                {
                    Width = 70,
                    Height = 70,
                    Image = albumArt,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0, 0, 0, 0),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                stripe.Controls.Add(pictureBanner);

                string text = $"{albumName} ({albumYear})";
                var labelAlbumName = new Label()
                {
                    AutoSize = true,
                    Text = text,
                    ForeColor = Color.White,
                    Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular),
                    Margin = new Padding(8, 8, 0, 0),
                };
                stripe.Controls.Add(labelAlbumName);

                Action clickMethod = new Action(() =>
                {
                    ProcessUI(new RedscientistMusicRequest()
                    {
                        action = RedscientistMusicActions.ShowAlbum,
                        album = albumID,
                        artist = artistID,
                    });
                });

                stripe.Click += (o, e) => clickMethod.Invoke();
                labelAlbumName.Click += (o, e) => clickMethod.Invoke();
                pictureBanner.Click += (o, e) => clickMethod.Invoke();
                scroll.Controls.Add(stripe);
            }

            flowVisiblePanel.Controls.Add(scroll);
        }


        public Button GetCleanButton(string text)
        {
            var btn = new Button()
            {
                AutoSize = true,
                Text = text,
                BackColor = Color.FromArgb(32, 32, 32),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(128, 32),
                Margin = new Padding(8,8,10,8),
                Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular),
            };

            btn.FlatAppearance.BorderColor = Color.Black;
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void DrawHeader()
        {
            FlowLayoutPanel stride = new FlowLayoutPanel()
            {
                Margin = new Padding(24, 0, 0, 0),
            };
            stride.AutoSize = true;
            stride.FlowDirection = FlowDirection.LeftToRight;

            var btnNews = GetCleanButton("News");
            btnNews.Click += (o, e) =>
            {
                ProcessUI(new RedscientistMusicRequest()
                {
                    action = RedscientistMusicActions.ShowMainPage,
                });
            };
            stride.Controls.Add(btnNews);

            var btnArtists = GetCleanButton("Artists");
            btnArtists.Click += (o, e) =>
            {
                ProcessUI(new RedscientistMusicRequest()
                {
                    action = RedscientistMusicActions.ShowMusicArtists,
                });
            };
            stride.Controls.Add(btnArtists);

            var btnRadio = GetCleanButton("Radio");
            btnRadio.Click += (o, e) =>
            {
                ProcessUI(new RedscientistMusicRequest()
                {
                    action = RedscientistMusicActions.ShowMusicArtists,
                });
            };
            stride.Controls.Add(btnRadio);

            flowVisiblePanel.Controls.Add(stride);
        }

        public dynamic GetDB(string dbName)
        {
            var filename = $"{dbName}.json";
            var cachePath = Path.Combine(MainForm.rsdbDir, filename);
            var onlinePath = $"https://redscientist.com/Content/data/{filename}";

            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = ASCIIEncoding.UTF8;
                var jsonDb = wc.DownloadString(onlinePath);
                File.WriteAllText(cachePath, jsonDb);
                dynamic newsObject = JObject.Parse(jsonDb);
                return newsObject;
            }
            catch (Exception ex)
            {
                //assume server was unreachable
                try
                {
                    var jsonDb = File.ReadAllText(cachePath);
                    dynamic newsObject = JObject.Parse(jsonDb);
                    return newsObject;
                }
                catch (Exception ex2)
                {
                    return null; //rip
                }
            }
        }

        public string GetData(string dataFileName)
        {
            var filename = $"{dataFileName}.txt";
            var cachePath = Path.Combine(MainForm.rsdbDir, filename);
            var onlinePath = $"https://redscientist.com/Content/data/{filename}";

            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = ASCIIEncoding.UTF8;
                var text = wc.DownloadString(onlinePath);
                File.WriteAllText(cachePath, text);
                return text;
            }
            catch (Exception ex)
            {
                //assume server was unreachable
                try
                {
                    var text = File.ReadAllText(cachePath);
                    return text;
                }
                catch (Exception ex2)
                {
                    return null; //rip
                }
            }
        }

        private Image GetAlbumFolderArt(string artistId, string albumId)
        {
            var filename = $"folder.jpg";

            var artistFolder = Path.Combine(MainForm.musicDir, artistId);
            var albumFolder = Path.Combine(artistFolder, albumId);
            var cachePath = Path.Combine(albumFolder, filename);

            var onlinePath = $"https://redscientist.com/Content/music/{albumId}/{filename}";

            if (File.Exists(cachePath))
                return Bitmap.FromFile(cachePath);

            try
            {
                if (!Directory.Exists(artistFolder))
                    Directory.CreateDirectory(artistFolder);

                if (!Directory.Exists(albumFolder))
                    Directory.CreateDirectory(albumFolder);

                WebClient wc = new WebClient();
                var BitmapBlob = wc.DownloadData(onlinePath);
                File.WriteAllBytes(cachePath, BitmapBlob);

                return Bitmap.FromStream(new MemoryStream(BitmapBlob));
            }
            catch (Exception ex)
            {
                return null; //rip
            }
        }

        private Image GetBannerArt(string artistId)
        {
            var filename = $"{artistId}.jpg";

            var bannersFolder = Path.Combine(MainForm.rsdbDir, "banners");
            var cachePath = Path.Combine(bannersFolder, filename);

            var onlinePath = $"https://redscientist.com/Content/banners/{filename}";

            if (File.Exists(cachePath))
                return Bitmap.FromFile(cachePath);

            try
            {
                if (!Directory.Exists(bannersFolder))
                    Directory.CreateDirectory(bannersFolder);

                WebClient wc = new WebClient();
                var BitmapBlob = wc.DownloadData(onlinePath);
                File.WriteAllBytes(cachePath, BitmapBlob);

                return Bitmap.FromStream(new MemoryStream(BitmapBlob));
            }
            catch (Exception ex)
            {
                return null; //rip
            }
        }

        private Image GetAlbumFrontWebArt(string artistId, string albumId)
        {
            var filename = $"frontweb.jpg";

            var artistFolder = Path.Combine(MainForm.musicDir, artistId);
            var albumFolder = Path.Combine(artistFolder, albumId);
            var cachePath = Path.Combine(albumFolder, filename);

            var onlinePath = $"https://redscientist.com/Content/music/{albumId}/{filename}";

            if (File.Exists(cachePath))
                return Bitmap.FromFile(cachePath);

            try
            {
                if (!Directory.Exists(artistFolder))
                    Directory.CreateDirectory(artistFolder);

                if (!Directory.Exists(albumFolder))
                    Directory.CreateDirectory(albumFolder);

                WebClient wc = new WebClient();
                var BitmapBlob = wc.DownloadData(onlinePath);
                File.WriteAllBytes(cachePath, BitmapBlob);

                return Bitmap.FromStream(new MemoryStream(BitmapBlob));
            }
            catch (Exception ex)
            {
                return null; //rip
            }
        }

        private void flowVisiblePanel_MouseEnter(object sender, EventArgs e)
        {
            flowVisiblePanel.Focus();
        }
    }

    public class RedscientistMusicRequest
    {
        public RedscientistMusicActions action;
        public string artist = null;
        public string album = null;
        public int songId = -1;
    }

    public enum RedscientistMusicActions
    {
        ShowMainPage,
        ShowMusicArtists,
        ShowArtist,
        ShowAlbum,
        PlayAlbum,
        QueueAlbum,
        PlaySong,
        QueueSong,
    }
}
