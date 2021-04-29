﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeekAssistant.Forms {
    public partial class Info : Form {
        public Info() {
            InitializeComponent();
        }
        private void AssignEvents() {
            Yes_Button.MouseDown += new(Yes_Button_Mousedown);
            Yes_Button.MouseUp += new(Yes_Button_MouseUp);
            Yes_Button.Click += new(Yes_Button_Click);

            No_Button.Click += new(No_Button_Click);

            Copy_Button.MouseDown += new(Copy_Button_MouseDown);
            Copy_Button.MouseUp += new(Copy_Button_MouseUp);
            Copy_Button.Click += new(Copy_Button_Click);

            CopyToClipboard_Timer.Tick += new(CopyToClipboard_Timer_Tick);

            title_Label.TextChanged += new(title_Label_TextChanged);
            title_Label.Click += new(title_Label_Click);
            title_Label_Click_Timer.Tick += new(title_Label_Click_Timer_Tick);
        }
         
        private void Info_Load(object sender, EventArgs e) {
            AssignEvents();

            //Reset info answer
            inf.infoAnswer = false;
            No_Button.Select();
            //reset Yes_Button props
            Yes_Button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Yes_Button.SetBounds(260, 276, 155, 30);

            if (string.IsNullOrEmpty(inf.detail.title) | !Enum.IsDefined(typeof(inf.lvls), inf.detail.lvl)) {
                Close();
                Dispose(); //dispose before starting again
                inf.Run(inf.lvls.FatalError, $"Where//s the error message??",
                         "if ( you see this please contact the developer to fix this bug.\nPlease provide the log when reporting.",
                        (null, "Close"));
            }

            //Reset to default
            if (!string.IsNullOrEmpty(inf.ButtonText.YesButton)) {
                Yes_Button.Text = inf.ButtonText.YesButton;
                Yes_Button.Visible = true;
                if (string.IsNullOrEmpty(inf.ButtonText.NoButton)) {
                    Yes_Button.SetBounds(No_Button.Bounds.X, No_Button.Bounds.Y, No_Button.Bounds.Width, No_Button.Bounds.Height);
                    Yes_Button.Anchor = No_Button.Anchor;
                    No_Button.Visible = false;
                }
            }
            if (!string.IsNullOrEmpty(inf.ButtonText.NoButton))
                No_Button.Text = inf.ButtonText.NoButton;
            else No_Button.Text = "Close";


            if (inf.theme.color != null) //failsafe (Don't merge failsafe layers)
                if (inf.theme.color.Length == 2) //2nd failsafe 
                    if (inf.theme.color[0] != Color.Empty & inf.theme.color[1] != Color.Empty) //3rd failsafe
                         title_Label.ForeColor = inf.theme.color[txt.ConvertBoolToInt(c.S.DarkTheme)];
            if (inf.theme.icon != null) //failsafe (Don't merge failsafe layers)
                if (inf.theme.icon.Length == 2) //2nd failsafe 
                    if (inf.theme.icon[0] != null & inf.theme.icon[1] != null) //3rd failsafe
                         info_PictureBox.Image = inf.theme.icon[txt.ConvertBoolToInt(c.S.DarkTheme)];

            info_PictureBox.Image = inf.infIcon(inf.detail.lvl);
            title_Label.ForeColor = inf.infColor(info_PictureBox.Image);
            Text = inf.WindowTitle;
            title_Label.Text = inf.detail.title;
            msg_Textbox.Text = inf.detail.msg; 
            Yes_Button.ForeColor = title_Label.ForeColor;
            Yes_Button.FlatAppearance.MouseDownBackColor = title_Label.ForeColor;
             

            ////////Special Cases
            //if (title_Label.Text == "Send Feedback") if (c.S.DarkTheme) info_PictureBox.Image = prop.x64.Smile_dark_64; else info_PictureBox.Image = prop.x64.Smile_64;


            //// Set size
            Width = 566;
            Height = 355;
            switch (msg_Textbox.Text.Length) {
            case < 150:
                Width = 520;
                Height -= 50;
                break;
            case > 400:
                Width += 60;
                Height += 75;
                break;
            }
            Home Home = new Home();
            SetBounds((Home.Width / 2) - (Width / 2) + Home.Location.X, Home.Top, Width, Height);
             
            GA_SetTheme.Run(Name);
        }

        private void Yes_Button_Mousedown(object sender, EventArgs e) {
            if (Yes_Button.ForeColor.GetBrightness() >= .5)
                Yes_Button.ForeColor = Color.Black;
            else Yes_Button.ForeColor = Color.White;

        }
        private void Yes_Button_MouseUp(object sender, EventArgs e) {
            Yes_Button.ForeColor = title_Label.ForeColor;
        }
        private void Yes_Button_Click(object sender, EventArgs e) {
            if (inf.detail.lvl == inf.lvls.Question) inf.infoAnswer = true;
            Close();
            Home Home = new Home();
            Home.BringToFront();
        }

        //private void Close_Button_MouseDown(object sender, EventArgs e) { Close_Button.MouseDown
        //    Close_Button.ForeColor = Color.White
        //}
        //private void Close_Button_MouseUp(object sender, EventArgs e) { Close_Button.MouseUp
        //    Close_Button.ForeColor = SystemColors.ControlText
        //}
        private void No_Button_Click(object sender, EventArgs e) {
            if (!c.S.VerboseLogging & c.S.VerboseLoggingPrompt
            & inf.detail.lvl != inf.lvls.Question) { //&& ! title_Label.Text = "Enable Verbose Logging?" && ! title_Label.Text = "Send Feedback" ) {
                                                     //Close();
                Dispose();
                if (inf.Run(inf.lvls.Question,
                            "Enable Verbose Logging?",
                              $"For better troubleshooting, enable verbose logging in Settings() and try again.", 
                            ("Enable", "Close"))) { Settings Settings = new Settings(); Settings.ShowDialog(); }

                c.S.VerboseLoggingPrompt = false;
            }
            if (inf.detail.lvl == inf.lvls.Question) inf.infoAnswer = false;
            Close();
            Home Home = new Home();
            Home.BringToFront();
            //Dispose();
        }

        private void Copy_Button_MouseDown(object sender, EventArgs e) {
            Copy_Button.Image = prop.x24.Copy_W_24;
            Copy_Button.ForeColor = Color.White;
        }
        private void Copy_Button_MouseUp(object sender, EventArgs e) {
            Copy_Button.Image = prop.x24.Copy_B_24;
            Copy_Button.ForeColor = c.colors.Green;
        }
        private void Copy_Button_Click(object sender, EventArgs e) {
            CopyToClipboard_Timer.Enabled = false;
            Clipboard.SetText($"{title_Label.Text}\n\n{msg_Textbox.Text}");
            Copy_Button.ForeColor = Color.DarkGreen;
            Copy_Button.Text = "Copied ";
            CopyToClipboard_Timer.Enabled = true;
        }

        private void CopyToClipboard_Timer_Tick(object sender, EventArgs e) {
            CopyToClipboard_Timer.Enabled = false;
            Copy_Button.Text = "Copy  ";
            Copy_Button.ForeColor = c.colors.fg;
        }

        private void title_Label_TextChanged(object sender, EventArgs e) {
            if (title_Label.Text.Length < 25)
                title_Label.Font = new Font("Segoe UI", 15.75f);
            else title_Label.Font = new Font("Segoe UI", 12.0f);

        }

        private Timer title_Label_Click_Timer = new Timer { Interval = 1500 };
        private string savedTitle;
        private string CopiedText = "Copied information...";
        private void title_Label_Click(object sender, EventArgs e) {
            if (title_Label.Text == CopiedText) return;
            if (inf.detail.lvl != inf.lvls.Question) {
                savedTitle = title_Label.Text;
                title_Label.Text = CopiedText;
                Clipboard.SetText(savedTitle);
                title_Label_Click_Timer.Start();
            }
        }
        private void title_Label_Click_Timer_Tick(object sender, EventArgs e) {
            title_Label.Text = savedTitle;
            title_Label_Click_Timer.Stop();
        } 
    }
}
