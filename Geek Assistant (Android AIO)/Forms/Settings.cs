﻿using System; 
using System.Diagnostics;
using System.Drawing; 
using System.Windows.Forms; 

namespace GeekAssistant.Forms {
    public partial class Settings : Form {
        private readonly string PopupMessages_SwitchButton_ToolTip = "View a window for messages like errors and info... etc";
        private readonly string VerbousLogging_SwitchButton_ToolTip = $"Add more details to logs like current ongoing actions{common.n}and behind the scenes actions that may not be interesting";
        private readonly string VerbousLoggingPrompt_Tooltip = "Ask to enable verbous logging when an error occurs";
        private readonly string ShowTooltips_SwitchButton_ToolTip = "Show tooltips on various controls for clarification";
        private readonly string ToU_SwitchButton_ToolTip = "Skip Terms of Use window when starting Geek Assistant. [= Accept always]";
        private readonly string AppMode_SwitchButton_ToolTip = $"Skip App Mode window and enter the last chosen{common.n}mode the next time starting Geek Assistant";
        private readonly string AutoClearLogs_SwitchButton_ToolTip = "Auto delete log files that are older than 30 days";
        private readonly string PerformAnimation_SwitchButton_ToolTip = $"Perform theme and movement animations.{common.n}Recommended: Disable for old systems.";

        //private ToolTip tooltip = new ToolTip {};
    public Settings() {
            InitializeComponent();
        }

        private void Settings_Load(Object sender, EventArgs e) {


            GA_SetTheme.Run(Name);
            SetBounds((common.Home.Width / 2) - (Width / 2) + common.Home.Location.X, common.Home.Top, Width, Height);

            ResetGA_Settings_CheckBox.Checked = false;
            ResetGA_LogsOnly_CheckBox.Checked = false;
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref ToU_SwitchButton,   common.S.ToU_dontShow);
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref AppMode_SwitchButton, common.S.AppMode_dontshow);
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref PopupMessages_SwitchButton, common.S.PopupMessages);
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref VerbousLogging_SwitchButton, common.S.VerboseLogging);
            VerbousLoggingStateSet();
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref ShowToolTips_SwitchButton, common.S.ShowToolTips);
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref AutoClearLogs_SwitchButton, common.S.AutoClearLogs);
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref PerformAnimation_SwitchButton, common.S.PerformAnimations);

        }
        #region ResetGA
        private Timer ResetGA_NoItems_Timer = new Timer { Interval = 1500 };
        private void ResetGA_Click(Object sender, EventArgs e) {
            common.ErrorInfo.code = "GAr-00";
            if (!ResetGA_LogsOnly_CheckBox.Checked && !ResetGA_Settings_CheckBox.Checked) {
                ResetGA.Text = "⠀⠀⠀⠀⠀ Select something ...";
                ResetGA_NoItems_Timer.Start();
                ResetGA_NoItems_Timer.Tick += ResetGA_NoItems_Timer_Tick;
                return;
            }
            GA_Reset.Run(Data, logs);
        }
        private void ResetGA_NoItems_Timer_Tick(Object sender, EventArgs e) {
            ResetGA.Text = "⠀⠀⠀⠀⠀ &Reset Geek Assistant";
        }

        private void ResetGA_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref (object)ResetGA, "Reset Geek Assistant", willClear);
            if (willClear == null) {
                GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA, "Reset Geek Assistant", "First, select something of the above.");
                return;
            }
            if (willClear.Length <= 9)
                GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA, "Reset Geek Assistant", "First, select something of the above.");
        }
        private void ResetGA_MouseDown(Object sender, EventArgs e) {
            ResetGA.ForeColor = GA_SetTheme.Current_bgColor();
        }
        private void ResetGA_MouseUp(Object sender, EventArgs e) {
            ResetGA.ForeColor = GA_SetTheme.Current_fgColor();
        }

        //private void Close_Button_MouseDown(Object sender, EventArgs e) { Close_Button.MouseDown
        //    Close_Button.ForeColor = GA_SetTheme.Current_fgColor()
        //}
        //private void Close_Button_MouseUp(Object sender, EventArgs e) { Close_Button.MouseUp
        //    Close_Button.ForeColor = GA_SetTheme.GA_SetTheme.Current_bgColor()
        //}
        private void Close_Button_Click(Object sender, EventArgs e) {
            Close();
        }

        #endregion

        #region "ResetGA_SelectAll"
        private void ResetGA_SelectAll_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA_SelectAll, "Select All", tooltip.GetToolTip(ResetGA_SelectAll));
        }
        private void ResetGA_SelectAll_MouseDown(Object sender, EventArgs e) {
            if (common.S.DarkTheme)
                ResetGA_SelectAll.Image = prop.x24.SelectAll_B_24;
            else ResetGA_SelectAll.Image = prop.x24.SelectAll_W_24;

        }
        private void ResetGA_SelectAll_MouseUp(Object sender, EventArgs e) {
            if (common.S.DarkTheme)
                ResetGA_SelectAll.Image = prop.x24.SelectAll_W_24;
            else ResetGA_SelectAll.Image = prop.x24.SelectAll_B_24;

        }
        private void ResetGA_SelectAll_Click(Object sender, EventArgs e) {
            if (ResetGA_Settings_CheckBox.Checked && ResetGA_LogsOnly_CheckBox.Checked) {
                ResetGA_Settings_CheckBox.Checked = false;
                ResetGA_LogsOnly_CheckBox.Checked = false;
                return;
            }
            ResetGA_Settings_CheckBox.Checked = true;
            ResetGA_LogsOnly_CheckBox.Checked = true;
        }
        #endregion


        public static string willClear;
        private string andstr = " and ";
        private bool Data = false;
        private bool logs = false;
        private void ResetGA_Settings_LogsOnly_CheckBox_CheckedChanged(Object sender, EventArgs e) {
            //Directories = false
            Data = false;

            logs = false;

            willClear = "clear";
            if (ResetGA_LogsOnly_CheckBox.Checked) {
                logs = true;
                willClear += " logs";
            }
            if (ResetGA_Settings_CheckBox.Checked) {
                Data = true;
                andstr = " and";
                if (!ResetGA_LogsOnly_CheckBox.Checked) {
                    andstr = "";
                }
                willClear += andstr + " internal data";
            }
            //if ( ResetGA_Settings_CheckBox.Checked <> ResetGA_LogsOnly_CheckBox.Checked ) {
            //    willClear += " only"
            //}
            //willClear += "?"
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA, "Reset Geek Assistant", willClear);
            if (!ResetGA_LogsOnly_CheckBox.Checked && !ResetGA_Settings_CheckBox.Checked)
                GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA, "Reset Geek Assistant", "First, select something of the above.");
        }

        ////ResetGA_Settings_CheckBox - Label
        private void ResetGA_Settings_CheckBox_AndLabel_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA_Settings_CheckBox, "Internal Data", tooltip.GetToolTip(ResetGA_Settings_CheckBox));
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA_Settings_CheckBox_Label, "Internal Data", tooltip.GetToolTip(ResetGA_Settings_CheckBox_Label));
        }
        private void ClearData_Settings_CheckBox_Label_Click(Object sender, EventArgs e) {
            ResetGA_Settings_CheckBox.Checked = !ResetGA_Settings_CheckBox.Checked;
        }

        ////ResetGA_LogsOnly_CheckBox - Label
        private void ResetGA_LogsOnly_CheckBox_AndLabel_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA_LogsOnly_CheckBox, "Log files", tooltip.GetToolTip(ResetGA_LogsOnly_CheckBox));
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ResetGA_LogsOnly_CheckBox_Label, "Log files", tooltip.GetToolTip(ResetGA_LogsOnly_CheckBox_Label));
        }
        private void ResetGA_LogsOnly_CheckBox_Label_Click(Object sender, EventArgs e) {
            ResetGA_LogsOnly_CheckBox.Checked = !ResetGA_LogsOnly_CheckBox.Checked;
        }

        ////ToU_SwitchButton
        private void ToU_SwitchButton_MouseDown(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref ToU_SwitchButton);
        }
        private void ToU_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref ToU_SwitchButton);
        }
        private void ToU_SwitchButton_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ToU_SwitchButton, "Skip Terms of Use on startup", tooltip.GetToolTip(ToU_SwitchButton)); // tooltip.GetToolTip(ShowToolTips_SwitchButton))
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref ToU_SwitchButton, common.S.ToU_dontShow);
        }
        private void ToU_SwitchButton_MouseLeave(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref ToU_SwitchButton, common.S.ToU_dontShow);
        }
        private void ToU_SwitchButton_Click(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref ToU_SwitchButton, common.S.ToU_dontShow, ref tooltip, ToU_SwitchButton_ToolTip);
        }

        ////AppMode_SwitchButton
        private void AppMode_SwitchButton_MouseDown(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref AppMode_SwitchButton);
        }
        private void AppMode_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref AppMode_SwitchButton);
        }
        private void AppMode_SwitchButton_MouseEnter(Object sender, EventArgs e) {

            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref AppMode_SwitchButton, "Skip App Mode on startup", tooltip.GetToolTip(AppMode_SwitchButton));
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref AppMode_SwitchButton, common.S.AppMode_dontshow);
        }
        private void AppMode_SwitchButton_MouseLeave(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref AppMode_SwitchButton, common.S.AppMode_dontshow);
        }
        private void AppMode_SwitchButton_Click(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref AppMode_SwitchButton, common.S.AppMode_dontshow, ref tooltip, AppMode_SwitchButton_ToolTip);
        }

        ////PopupMessages_SwitchButton
        private void PopupMessages_SwitchButton_MouseDown(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref PopupMessages_SwitchButton);
        }
        private void PopupMessages_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref PopupMessages_SwitchButton);
            }
        private void PopupMessages_SwitchButton_MouseEnter(Object sender, EventArgs e) {

            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref PopupMessages_SwitchButton, "Pop-up Messages", tooltip.GetToolTip(PopupMessages_SwitchButton));
               GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref PopupMessages_SwitchButton, common.S.PopupMessages);
            }
        private void PopupMessages_SwitchButton_MouseLeave(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref PopupMessages_SwitchButton, common.S.PopupMessages);
            }
        private void PopupMessages_SwitchButton_Click(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref PopupMessages_SwitchButton, ref common.S.PopupMessages, tooltip, PopupMessages_SwitchButton_ToolTip);
            }

        ////VerbousLogging_SwitchButton
        private void VerbousLogging_SwitchButton_MouseDown(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref VerbousLogging_SwitchButton);
            }
        private void VerbousLogging_SwitchButton_MouseUp(Object sender, EventArgs e) { 
               GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref VerbousLogging_SwitchButton);
            }
        private void VerbousLogging_SwitchButton_MouseEnter(Object sender, EventArgs e) {

            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref VerbousLogging_SwitchButton, "Verbous Logging", tooltip.GetToolTip(VerbousLogging_SwitchButton));
               GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref VerbousLogging_SwitchButton, common.S.VerboseLogging);
            }
        private void VerbousLogging_SwitchButton_MouseLeave(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref VerbousLogging_SwitchButton, common.S.VerboseLogging);
            }
        private void VerbousLogging_SwitchButton_Click(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref VerbousLogging_SwitchButton, common.S.VerboseLogging, ref tooltip, VerbousLogging_SwitchButton_ToolTip);
                VerbousLoggingStateSet();
        }
        private void VerbousLoggingStateSet() {
            if (common.S.VerboseLogging) {
                if (common.S.VerboseLoggingPrompt) {
                    VerbousLoggingPrompt_SwitchButton.PerformClick();
                    if (common.S.DarkTheme)
                        VerbousLoggingPrompt_SwitchButton.BackColor = Color.FromArgb(64, 64, 64);
                    else VerbousLoggingPrompt_SwitchButton.BackColor = Color.FromArgb(191, 191, 191);
                }
                    VerbousLoggingPrompt_SwitchButton.Enabled = false;
                } else {
                    VerbousLoggingPrompt_SwitchButton.Enabled = true;
                    GA_SwitchButton_Style.SettingsButtonSwitch_Style_EnableIfTrue(ref VerbousLoggingPrompt_SwitchButton, common.S.VerboseLoggingPrompt);
                }
            } 

        ////VerbousLoggingPrompt_SwitchButton
        private void VerbousLoggingPrompt_SwitchButton_MouseDown(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref  VerbousLoggingPrompt_SwitchButton);
        }
        private void VerbousLoggingPrompt_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref VerbousLoggingPrompt_SwitchButton);
        }
        private void VerbousLoggingPrompt_SwitchButton_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref VerbousLoggingPrompt_SwitchButton, "Verbous Logging", tooltip.GetToolTip(VerbousLoggingPrompt_SwitchButton));
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref VerbousLoggingPrompt_SwitchButton, common.S.VerboseLoggingPrompt);
        }
        private void VerbousLoggingPrompt_SwitchButton_MouseLeave(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref VerbousLoggingPrompt_SwitchButton, common.S.VerboseLoggingPrompt);
        }
        private void VerbousLoggingPrompt_SwitchButton_Click(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref this.VerbousLoggingPrompt_SwitchButton, common.S.VerboseLoggingPrompt, ref tooltip, VerbousLoggingPrompt_Tooltip);
        }

        ////ShowToolTips_SwitchButton
        private void ShowToolTips_SwitchButton_MouseDown(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref ShowToolTips_SwitchButton);
        }
        private void ShowToolTips_SwitchButton_MouseUp(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref ShowToolTips_SwitchButton);
        }
        private void ShowToolTips_SwitchButton_MouseEnter(Object sender, EventArgs e) {

            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref ShowToolTips_SwitchButton, "Show Tooltips", tooltip.GetToolTip(ShowToolTips_SwitchButton));
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref ShowToolTips_SwitchButton, common.S.ShowToolTips);
        }
        private void ShowToolTips_SwitchButton_MouseLeave(Object sender, EventArgs e) {

            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref ShowToolTips_SwitchButton, common.S.ShowToolTips);
        }
        private void ShowToolTips_SwitchButton_Click(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref ShowToolTips_SwitchButton, common.S.ShowToolTips, ref tooltip, ShowTooltips_SwitchButton_ToolTip);
        }

        ////OpenLogsFolder
        private void OpenLogsFolder_MouseEnter(Object sender, EventArgs e) {
            OpenLogsFolder.BackColor = Color.Silver;
        }
        private void OpenLogsFolder_MouseLeave(Object sender, EventArgs e) {
            OpenLogsFolder.BackColor = Color.Transparent;
        }
        private void OpenLogsFolder_MouseDown(Object sender, EventArgs e) {
            OpenLogsFolder.ForeColor = Color.White;
            OpenLogsFolder.BackColor = Color.FromArgb(64, 64, 64);
        }
        private void OpenLogsFolder_MouseUp(Object sender, EventArgs e) {

            OpenLogsFolder.ForeColor = Color.FromArgb(64, 64, 64);
            OpenLogsFolder.BackColor = Color.Transparent;
        }
        private void OpenLogsFolder_Click(Object sender, EventArgs e) {
            Process.Start(common.GA_logs);
        }

        ////AutoClearLogs_SwitchButton
        private void AutoClearLogs_SwitchButton_MouseDown(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref AutoClearLogs_SwitchButton);
        }
        private void AutoClearLogs_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref AutoClearLogs_SwitchButton);
        }
        private void AutoClearLogs_SwitchButton_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref AutoClearLogs_SwitchButton, "Auto Clear Logs", tooltip.GetToolTip(AutoClearLogs_SwitchButton));
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref AutoClearLogs_SwitchButton, common.S.AutoClearLogs);
                }
        private void AutoClearLogs_SwitchButton_MouseLeave(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref AutoClearLogs_SwitchButton, common.S.AutoClearLogs);
                }
        private void AutoClearLogs_SwitchButton_Click(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref AutoClearLogs_SwitchButton, common.S.AutoClearLogs, ref tooltip, AutoClearLogs_SwitchButton_ToolTip);
                }

        ////PerformAnimation_SwitchButton
        private void PerformAnimation_SwitchButton_MouseDown(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseDown_KeyDown(ref PerformAnimation_SwitchButton);
                }
        private void PerformAnimation_SwitchButton_MouseUp(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseUp_KeyUp(ref PerformAnimation_SwitchButton);
                }
        private void PerformAnimation_SwitchButton_MouseEnter(Object sender, EventArgs e) {
            GA_SetTooltipInfo.SetToolTipInfo(ref tooltip, ref PerformAnimation_SwitchButton, "Perform Theme Animation", tooltip.GetToolTip(PerformAnimation_SwitchButton));
                   GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseEnter(ref PerformAnimation_SwitchButton, common.S.PerformAnimations);
                }
        private void PerformAnimation_SwitchButton_MouseLeave(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseLeave(ref PerformAnimation_SwitchButton, common.S.PerformAnimations);
                }
        private void PerformAnimation_SwitchButton_Click(Object sender, EventArgs e) {
            GA_SwitchButton_Style.SettingsButtonSwitch_Style_MouseClick(ref PerformAnimation_SwitchButton, common.S.PerformAnimations, ref tooltip, PerformAnimation_SwitchButton_ToolTip);
                }
    }
}