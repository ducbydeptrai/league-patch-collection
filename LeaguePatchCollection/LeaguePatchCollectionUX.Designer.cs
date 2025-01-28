using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LeaguePatchCollection
{
    partial class LeaguePatchCollectionUX
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeaguePatchCollectionUX));
            MainControllerBackdrop = new Panel();
            StartButton = new Guna.UI2.WinForms.Guna2Button();
            MainHeaderBackdrop = new Panel();
            pictureBox1 = new PictureBox();
            CloseButton = new Guna.UI2.WinForms.Guna2ControlBox();
            WindowTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            MinimizeButton = new Guna.UI2.WinForms.Guna2ControlBox();
            SectionLabelConfig = new Guna.UI2.WinForms.Guna2HtmlLabel();
            DisableVanguard = new Guna.UI2.WinForms.Guna2CheckBox();
            LegacyHonor = new Guna.UI2.WinForms.Guna2CheckBox();
            NameChangeBypass = new Guna.UI2.WinForms.Guna2CheckBox();
            SupressBehavior = new Guna.UI2.WinForms.Guna2CheckBox();
            ChatSeperatorLeft = new Guna.UI2.WinForms.Guna2Separator();
            ChatLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ChatSeperatorRight = new Guna.UI2.WinForms.Guna2Separator();
            NoBloatware = new Guna.UI2.WinForms.Guna2CheckBox();
            ConfigSeperatorRight = new Guna.UI2.WinForms.Guna2Separator();
            AppearAsLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ShowOnlineButton = new Guna.UI2.WinForms.Guna2RadioButton();
            ShowOfflineButton = new Guna.UI2.WinForms.Guna2RadioButton();
            ShowAwayButton = new Guna.UI2.WinForms.Guna2RadioButton();
            ShowMobileButton = new Guna.UI2.WinForms.Guna2RadioButton();
            DisconnectChatButton = new Guna.UI2.WinForms.Guna2Button();
            MiscSeperatorRight = new Guna.UI2.WinForms.Guna2Separator();
            MiscLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            MiscSeperatorLeft = new Guna.UI2.WinForms.Guna2Separator();
            CleanLogsButton = new Guna.UI2.WinForms.Guna2Button();
            BanReasonButton = new Guna.UI2.WinForms.Guna2Button();
            OldPatch = new Guna.UI2.WinForms.Guna2CheckBox();
            ConfigSeperatorLeft = new Guna.UI2.WinForms.Guna2Separator();
            MainControllerBackdrop.SuspendLayout();
            MainHeaderBackdrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // MainControllerBackdrop
            // 
            MainControllerBackdrop.BackColor = Color.FromArgb(60, 60, 60);
            MainControllerBackdrop.Controls.Add(StartButton);
            MainControllerBackdrop.Location = new Point(0, 340);
            MainControllerBackdrop.Margin = new Padding(0);
            MainControllerBackdrop.Name = "MainControllerBackdrop";
            MainControllerBackdrop.Size = new Size(600, 60);
            MainControllerBackdrop.TabIndex = 12;
            // 
            // StartButton
            // 
            StartButton.BorderColor = Color.FromArgb(0, 155, 0);
            StartButton.BorderRadius = 2;
            StartButton.BorderThickness = 2;
            StartButton.Cursor = Cursors.Hand;
            StartButton.CustomizableEdges = customizableEdges1;
            StartButton.DisabledState.BorderColor = Color.DarkGray;
            StartButton.DisabledState.CustomBorderColor = Color.DarkGray;
            StartButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            StartButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            StartButton.FillColor = Color.FromArgb(0, 118, 0);
            StartButton.Font = new Font("Inter Tight ExtraBold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            StartButton.ForeColor = SystemColors.ButtonFace;
            StartButton.HoverState.BorderColor = Color.FromArgb(0, 192, 0);
            StartButton.HoverState.CustomBorderColor = Color.Transparent;
            StartButton.HoverState.FillColor = Color.FromArgb(0, 155, 0);
            StartButton.HoverState.ForeColor = Color.White;
            StartButton.Location = new Point(409, 11);
            StartButton.Name = "StartButton";
            StartButton.ShadowDecoration.BorderRadius = 2;
            StartButton.ShadowDecoration.CustomizableEdges = customizableEdges2;
            StartButton.Size = new Size(179, 37);
            StartButton.TabIndex = 0;
            StartButton.Text = "LAUNCH CLIENT";
            StartButton.Click += StartButton_Click;
            // 
            // MainHeaderBackdrop
            // 
            MainHeaderBackdrop.BackColor = Color.FromArgb(60, 60, 60);
            MainHeaderBackdrop.Controls.Add(pictureBox1);
            MainHeaderBackdrop.Controls.Add(CloseButton);
            MainHeaderBackdrop.Controls.Add(WindowTitle);
            MainHeaderBackdrop.Controls.Add(MinimizeButton);
            MainHeaderBackdrop.Location = new Point(0, 0);
            MainHeaderBackdrop.Name = "MainHeaderBackdrop";
            MainHeaderBackdrop.Size = new Size(600, 29);
            MainHeaderBackdrop.TabIndex = 16;
            MainHeaderBackdrop.MouseDown += MainHeaderBackdrop_MouseDown;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.LPC;
            pictureBox1.Location = new Point(12, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(24, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CloseButton.BackColor = Color.Transparent;
            CloseButton.BorderColor = Color.Transparent;
            CloseButton.ControlBoxStyle = Guna.UI2.WinForms.Enums.ControlBoxStyle.Custom;
            CloseButton.Cursor = Cursors.Hand;
            CloseButton.CustomizableEdges = customizableEdges3;
            CloseButton.FillColor = Color.Transparent;
            CloseButton.HoverState.BorderColor = Color.Red;
            CloseButton.HoverState.FillColor = Color.Red;
            CloseButton.HoverState.IconColor = Color.White;
            CloseButton.IconColor = Color.FromArgb(175, 175, 175);
            CloseButton.Location = new Point(570, 0);
            CloseButton.Name = "CloseButton";
            CloseButton.ShadowDecoration.BorderRadius = 0;
            CloseButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
            CloseButton.Size = new Size(30, 29);
            CloseButton.TabIndex = 28;
            // 
            // WindowTitle
            // 
            WindowTitle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            WindowTitle.BackColor = Color.Transparent;
            WindowTitle.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Bold);
            WindowTitle.ForeColor = Color.FromArgb(225, 225, 225);
            WindowTitle.IsContextMenuEnabled = false;
            WindowTitle.IsSelectionEnabled = false;
            WindowTitle.Location = new Point(42, 3);
            WindowTitle.Name = "WindowTitle";
            WindowTitle.Size = new Size(177, 25);
            WindowTitle.TabIndex = 27;
            WindowTitle.Text = "League Patch Collection";
            WindowTitle.TextAlignment = ContentAlignment.MiddleCenter;
            WindowTitle.MouseDown += MainHeaderBackdrop_MouseDown;
            // 
            // MinimizeButton
            // 
            MinimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MinimizeButton.BackColor = Color.Transparent;
            MinimizeButton.BorderColor = Color.Transparent;
            MinimizeButton.ControlBoxStyle = Guna.UI2.WinForms.Enums.ControlBoxStyle.Custom;
            MinimizeButton.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            MinimizeButton.Cursor = Cursors.Hand;
            MinimizeButton.CustomizableEdges = customizableEdges5;
            MinimizeButton.FillColor = Color.Transparent;
            MinimizeButton.HoverState.BorderColor = SystemColors.WindowFrame;
            MinimizeButton.HoverState.FillColor = SystemColors.WindowFrame;
            MinimizeButton.HoverState.IconColor = Color.White;
            MinimizeButton.IconColor = Color.FromArgb(175, 175, 175);
            MinimizeButton.Location = new Point(540, 0);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.ShadowDecoration.BorderRadius = 0;
            MinimizeButton.ShadowDecoration.CustomizableEdges = customizableEdges6;
            MinimizeButton.Size = new Size(30, 29);
            MinimizeButton.TabIndex = 2;
            // 
            // SectionLabelConfig
            // 
            SectionLabelConfig.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SectionLabelConfig.BackColor = Color.Transparent;
            SectionLabelConfig.Font = new Font("Roboto", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SectionLabelConfig.ForeColor = Color.White;
            SectionLabelConfig.IsContextMenuEnabled = false;
            SectionLabelConfig.IsSelectionEnabled = false;
            SectionLabelConfig.Location = new Point(252, 35);
            SectionLabelConfig.Name = "SectionLabelConfig";
            SectionLabelConfig.Size = new Size(106, 17);
            SectionLabelConfig.TabIndex = 0;
            SectionLabelConfig.Text = "CONFIGURATION";
            SectionLabelConfig.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // DisableVanguard
            // 
            DisableVanguard.AutoSize = true;
            DisableVanguard.CheckedState.BorderColor = Color.DodgerBlue;
            DisableVanguard.CheckedState.BorderRadius = 1;
            DisableVanguard.CheckedState.BorderThickness = 1;
            DisableVanguard.CheckedState.FillColor = Color.DeepSkyBlue;
            DisableVanguard.Cursor = Cursors.Hand;
            DisableVanguard.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DisableVanguard.ForeColor = Color.FromArgb(225, 225, 225);
            DisableVanguard.Location = new Point(12, 55);
            DisableVanguard.Name = "DisableVanguard";
            DisableVanguard.Size = new Size(155, 27);
            DisableVanguard.TabIndex = 22;
            DisableVanguard.Text = "Vanguard Bypass";
            DisableVanguard.UncheckedState.BorderColor = Color.Gray;
            DisableVanguard.UncheckedState.BorderRadius = 1;
            DisableVanguard.UncheckedState.BorderThickness = 1;
            DisableVanguard.UncheckedState.FillColor = Color.Silver;
            DisableVanguard.CheckedChanged += DisableVanguard_CheckedChanged;
            // 
            // LegacyHonor
            // 
            LegacyHonor.AutoSize = true;
            LegacyHonor.CheckedState.BorderColor = Color.DodgerBlue;
            LegacyHonor.CheckedState.BorderRadius = 1;
            LegacyHonor.CheckedState.BorderThickness = 1;
            LegacyHonor.CheckedState.FillColor = Color.DeepSkyBlue;
            LegacyHonor.Cursor = Cursors.Hand;
            LegacyHonor.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LegacyHonor.ForeColor = Color.FromArgb(225, 225, 225);
            LegacyHonor.Location = new Point(12, 84);
            LegacyHonor.Name = "LegacyHonor";
            LegacyHonor.Size = new Size(160, 27);
            LegacyHonor.TabIndex = 24;
            LegacyHonor.Text = "Use Legacy Honor";
            LegacyHonor.UncheckedState.BorderColor = Color.Gray;
            LegacyHonor.UncheckedState.BorderRadius = 1;
            LegacyHonor.UncheckedState.BorderThickness = 1;
            LegacyHonor.UncheckedState.FillColor = Color.Silver;
            LegacyHonor.CheckedChanged += LegacyHonor_CheckedChanged;
            // 
            // NameChangeBypass
            // 
            NameChangeBypass.AutoSize = true;
            NameChangeBypass.CheckedState.BorderColor = Color.DodgerBlue;
            NameChangeBypass.CheckedState.BorderRadius = 1;
            NameChangeBypass.CheckedState.BorderThickness = 1;
            NameChangeBypass.CheckedState.FillColor = Color.DeepSkyBlue;
            NameChangeBypass.Cursor = Cursors.Hand;
            NameChangeBypass.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NameChangeBypass.ForeColor = Color.FromArgb(225, 225, 225);
            NameChangeBypass.Location = new Point(381, 55);
            NameChangeBypass.Name = "NameChangeBypass";
            NameChangeBypass.Size = new Size(187, 27);
            NameChangeBypass.TabIndex = 25;
            NameChangeBypass.Text = "Bypass Name Change";
            NameChangeBypass.UncheckedState.BorderColor = Color.Gray;
            NameChangeBypass.UncheckedState.BorderRadius = 1;
            NameChangeBypass.UncheckedState.BorderThickness = 1;
            NameChangeBypass.UncheckedState.FillColor = Color.Silver;
            NameChangeBypass.CheckedChanged += NameChangeBypass_CheckedChanged;
            // 
            // SupressBehavior
            // 
            SupressBehavior.AutoSize = true;
            SupressBehavior.CheckedState.BorderColor = Color.DodgerBlue;
            SupressBehavior.CheckedState.BorderRadius = 1;
            SupressBehavior.CheckedState.BorderThickness = 1;
            SupressBehavior.CheckedState.FillColor = Color.DeepSkyBlue;
            SupressBehavior.Cursor = Cursors.Hand;
            SupressBehavior.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SupressBehavior.ForeColor = Color.FromArgb(225, 225, 225);
            SupressBehavior.Location = new Point(173, 55);
            SupressBehavior.Name = "SupressBehavior";
            SupressBehavior.Size = new Size(202, 27);
            SupressBehavior.TabIndex = 26;
            SupressBehavior.Text = "Hide Behavior Warnings";
            SupressBehavior.UncheckedState.BorderColor = Color.Gray;
            SupressBehavior.UncheckedState.BorderRadius = 1;
            SupressBehavior.UncheckedState.BorderThickness = 1;
            SupressBehavior.UncheckedState.FillColor = Color.Silver;
            SupressBehavior.CheckedChanged += SupressBehavior_CheckedChanged;
            // 
            // ChatSeperatorLeft
            // 
            ChatSeperatorLeft.FillThickness = 2;
            ChatSeperatorLeft.Location = new Point(12, 113);
            ChatSeperatorLeft.Name = "ChatSeperatorLeft";
            ChatSeperatorLeft.Size = new Size(266, 17);
            ChatSeperatorLeft.TabIndex = 29;
            // 
            // ChatLabel
            // 
            ChatLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ChatLabel.BackColor = Color.Transparent;
            ChatLabel.Font = new Font("Roboto", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ChatLabel.ForeColor = Color.White;
            ChatLabel.IsContextMenuEnabled = false;
            ChatLabel.IsSelectionEnabled = false;
            ChatLabel.Location = new Point(281, 113);
            ChatLabel.Name = "ChatLabel";
            ChatLabel.Size = new Size(38, 17);
            ChatLabel.TabIndex = 30;
            ChatLabel.Text = "CHAT";
            ChatLabel.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // ChatSeperatorRight
            // 
            ChatSeperatorRight.FillThickness = 2;
            ChatSeperatorRight.Location = new Point(322, 113);
            ChatSeperatorRight.Name = "ChatSeperatorRight";
            ChatSeperatorRight.Size = new Size(266, 17);
            ChatSeperatorRight.TabIndex = 31;
            // 
            // NoBloatware
            // 
            NoBloatware.AutoSize = true;
            NoBloatware.CheckedState.BorderColor = Color.DodgerBlue;
            NoBloatware.CheckedState.BorderRadius = 1;
            NoBloatware.CheckedState.BorderThickness = 1;
            NoBloatware.CheckedState.FillColor = Color.DeepSkyBlue;
            NoBloatware.Cursor = Cursors.Hand;
            NoBloatware.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NoBloatware.ForeColor = Color.FromArgb(224, 224, 224);
            NoBloatware.Location = new Point(178, 84);
            NoBloatware.Name = "NoBloatware";
            NoBloatware.Size = new Size(166, 27);
            NoBloatware.TabIndex = 32;
            NoBloatware.Text = "Remove Bloatware";
            NoBloatware.UncheckedState.BorderColor = Color.Gray;
            NoBloatware.UncheckedState.BorderRadius = 1;
            NoBloatware.UncheckedState.BorderThickness = 1;
            NoBloatware.UncheckedState.FillColor = Color.Silver;
            NoBloatware.CheckedChanged += NoBloatware_CheckedChanged;
            // 
            // ConfigSeperatorRight
            // 
            ConfigSeperatorRight.FillThickness = 2;
            ConfigSeperatorRight.Location = new Point(364, 35);
            ConfigSeperatorRight.Name = "ConfigSeperatorRight";
            ConfigSeperatorRight.Size = new Size(224, 17);
            ConfigSeperatorRight.TabIndex = 34;
            // 
            // AppearAsLabel
            // 
            AppearAsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AppearAsLabel.BackColor = Color.Transparent;
            AppearAsLabel.Font = new Font("Inter", 11F, FontStyle.Bold);
            AppearAsLabel.ForeColor = SystemColors.ScrollBar;
            AppearAsLabel.IsContextMenuEnabled = false;
            AppearAsLabel.IsSelectionEnabled = false;
            AppearAsLabel.Location = new Point(12, 136);
            AppearAsLabel.Name = "AppearAsLabel";
            AppearAsLabel.Size = new Size(61, 24);
            AppearAsLabel.TabIndex = 36;
            AppearAsLabel.Text = "Appear:";
            AppearAsLabel.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // ShowOnlineButton
            // 
            ShowOnlineButton.AutoSize = true;
            ShowOnlineButton.CheckedState.BorderColor = Color.Lime;
            ShowOnlineButton.CheckedState.BorderThickness = 0;
            ShowOnlineButton.CheckedState.FillColor = Color.Lime;
            ShowOnlineButton.CheckedState.InnerColor = Color.FromArgb(0, 192, 0);
            ShowOnlineButton.CheckedState.InnerOffset = -6;
            ShowOnlineButton.Cursor = Cursors.Hand;
            ShowOnlineButton.Font = new Font("Roboto", 12F);
            ShowOnlineButton.ForeColor = Color.FromArgb(175, 175, 175);
            ShowOnlineButton.Location = new Point(79, 137);
            ShowOnlineButton.Name = "ShowOnlineButton";
            ShowOnlineButton.Size = new Size(72, 23);
            ShowOnlineButton.TabIndex = 37;
            ShowOnlineButton.Text = "Online";
            ShowOnlineButton.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            ShowOnlineButton.UncheckedState.BorderThickness = 2;
            ShowOnlineButton.UncheckedState.FillColor = Color.Transparent;
            ShowOnlineButton.UncheckedState.InnerColor = Color.Transparent;
            ShowOnlineButton.CheckedChanged += ShowOnlineButton_CheckedChanged;
            // 
            // ShowOfflineButton
            // 
            ShowOfflineButton.AutoSize = true;
            ShowOfflineButton.CheckedState.BorderColor = Color.Silver;
            ShowOfflineButton.CheckedState.BorderThickness = 0;
            ShowOfflineButton.CheckedState.FillColor = Color.Silver;
            ShowOfflineButton.CheckedState.InnerColor = Color.Gray;
            ShowOfflineButton.CheckedState.InnerOffset = -6;
            ShowOfflineButton.Cursor = Cursors.Hand;
            ShowOfflineButton.Font = new Font("Roboto", 12F);
            ShowOfflineButton.ForeColor = Color.FromArgb(175, 175, 175);
            ShowOfflineButton.Location = new Point(310, 137);
            ShowOfflineButton.Name = "ShowOfflineButton";
            ShowOfflineButton.Size = new Size(73, 23);
            ShowOfflineButton.TabIndex = 38;
            ShowOfflineButton.Text = "Offline";
            ShowOfflineButton.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            ShowOfflineButton.UncheckedState.BorderThickness = 2;
            ShowOfflineButton.UncheckedState.FillColor = Color.Transparent;
            ShowOfflineButton.UncheckedState.InnerColor = Color.Transparent;
            ShowOfflineButton.CheckedChanged += ShowOfflineButton_CheckedChanged;
            // 
            // ShowAwayButton
            // 
            ShowAwayButton.AutoSize = true;
            ShowAwayButton.CheckedState.BorderColor = Color.Red;
            ShowAwayButton.CheckedState.BorderThickness = 2;
            ShowAwayButton.CheckedState.FillColor = Color.Red;
            ShowAwayButton.CheckedState.InnerColor = Color.FromArgb(192, 0, 0);
            ShowAwayButton.CheckedState.InnerOffset = -6;
            ShowAwayButton.Cursor = Cursors.Hand;
            ShowAwayButton.Font = new Font("Roboto", 12F);
            ShowAwayButton.ForeColor = Color.FromArgb(175, 175, 175);
            ShowAwayButton.Location = new Point(157, 137);
            ShowAwayButton.Name = "ShowAwayButton";
            ShowAwayButton.Size = new Size(66, 23);
            ShowAwayButton.TabIndex = 39;
            ShowAwayButton.Text = "Away";
            ShowAwayButton.UncheckedState.BorderColor = Color.FromArgb(125, 125, 125);
            ShowAwayButton.UncheckedState.BorderThickness = 2;
            ShowAwayButton.UncheckedState.FillColor = Color.Transparent;
            ShowAwayButton.UncheckedState.InnerColor = Color.Transparent;
            ShowAwayButton.CheckedChanged += ShowAwayButton_CheckedChanged;
            // 
            // ShowMobileButton
            // 
            ShowMobileButton.AutoSize = true;
            ShowMobileButton.CheckedState.BorderColor = Color.FromArgb(255, 128, 0);
            ShowMobileButton.CheckedState.BorderThickness = 0;
            ShowMobileButton.CheckedState.FillColor = Color.FromArgb(255, 128, 0);
            ShowMobileButton.CheckedState.InnerColor = Color.FromArgb(192, 64, 0);
            ShowMobileButton.CheckedState.InnerOffset = -6;
            ShowMobileButton.Cursor = Cursors.Hand;
            ShowMobileButton.Font = new Font("Roboto", 12F);
            ShowMobileButton.ForeColor = Color.FromArgb(175, 175, 175);
            ShowMobileButton.Location = new Point(229, 137);
            ShowMobileButton.Name = "ShowMobileButton";
            ShowMobileButton.Size = new Size(75, 23);
            ShowMobileButton.TabIndex = 40;
            ShowMobileButton.Text = "Mobile";
            ShowMobileButton.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            ShowMobileButton.UncheckedState.BorderThickness = 2;
            ShowMobileButton.UncheckedState.FillColor = Color.Transparent;
            ShowMobileButton.UncheckedState.InnerColor = Color.Transparent;
            ShowMobileButton.CheckedChanged += ShowMobileButton_CheckedChanged;
            // 
            // DisconnectChatButton
            // 
            DisconnectChatButton.BorderColor = Color.FromArgb(60, 60, 60);
            DisconnectChatButton.BorderRadius = 2;
            DisconnectChatButton.BorderThickness = 2;
            DisconnectChatButton.Cursor = Cursors.Hand;
            DisconnectChatButton.CustomBorderColor = Color.FromArgb(60, 60, 60);
            DisconnectChatButton.CustomBorderThickness = new Padding(1);
            DisconnectChatButton.CustomizableEdges = customizableEdges7;
            DisconnectChatButton.DisabledState.BorderColor = Color.DarkGray;
            DisconnectChatButton.DisabledState.CustomBorderColor = Color.DarkGray;
            DisconnectChatButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            DisconnectChatButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            DisconnectChatButton.FillColor = Color.FromArgb(40, 40, 40);
            DisconnectChatButton.Font = new Font("Inter Medium", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DisconnectChatButton.ForeColor = Color.FromArgb(225, 225, 225);
            DisconnectChatButton.HoverState.BorderColor = Color.FromArgb(80, 80, 80);
            DisconnectChatButton.HoverState.FillColor = Color.FromArgb(60, 60, 60);
            DisconnectChatButton.HoverState.ForeColor = Color.Yellow;
            DisconnectChatButton.Location = new Point(389, 134);
            DisconnectChatButton.Name = "DisconnectChatButton";
            DisconnectChatButton.ShadowDecoration.CustomizableEdges = customizableEdges8;
            DisconnectChatButton.Size = new Size(199, 26);
            DisconnectChatButton.TabIndex = 41;
            DisconnectChatButton.Text = "DISCONNECT FROM CHAT";
            // 
            // MiscSeperatorRight
            // 
            MiscSeperatorRight.FillThickness = 2;
            MiscSeperatorRight.Location = new Point(364, 166);
            MiscSeperatorRight.Name = "MiscSeperatorRight";
            MiscSeperatorRight.Size = new Size(224, 17);
            MiscSeperatorRight.TabIndex = 42;
            // 
            // MiscLabel
            // 
            MiscLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MiscLabel.BackColor = Color.Transparent;
            MiscLabel.Font = new Font("Roboto", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MiscLabel.ForeColor = Color.White;
            MiscLabel.IsContextMenuEnabled = false;
            MiscLabel.IsSelectionEnabled = false;
            MiscLabel.Location = new Point(253, 166);
            MiscLabel.Name = "MiscLabel";
            MiscLabel.Size = new Size(107, 17);
            MiscLabel.TabIndex = 43;
            MiscLabel.Text = "MISCELLANEOUS";
            MiscLabel.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // MiscSeperatorLeft
            // 
            MiscSeperatorLeft.FillThickness = 2;
            MiscSeperatorLeft.Location = new Point(12, 166);
            MiscSeperatorLeft.Name = "MiscSeperatorLeft";
            MiscSeperatorLeft.Size = new Size(235, 17);
            MiscSeperatorLeft.TabIndex = 44;
            // 
            // CleanLogsButton
            // 
            CleanLogsButton.BorderColor = Color.FromArgb(60, 60, 60);
            CleanLogsButton.BorderRadius = 2;
            CleanLogsButton.BorderThickness = 2;
            CleanLogsButton.Cursor = Cursors.Hand;
            CleanLogsButton.CustomBorderColor = Color.FromArgb(60, 60, 60);
            CleanLogsButton.CustomBorderThickness = new Padding(1);
            CleanLogsButton.CustomizableEdges = customizableEdges9;
            CleanLogsButton.DisabledState.BorderColor = Color.DarkGray;
            CleanLogsButton.DisabledState.CustomBorderColor = Color.DarkGray;
            CleanLogsButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            CleanLogsButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            CleanLogsButton.FillColor = Color.FromArgb(40, 40, 40);
            CleanLogsButton.Font = new Font("Inter SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CleanLogsButton.ForeColor = Color.FromArgb(225, 225, 225);
            CleanLogsButton.HoverState.BorderColor = Color.FromArgb(80, 80, 80);
            CleanLogsButton.HoverState.CustomBorderColor = Color.FromArgb(80, 80, 80);
            CleanLogsButton.HoverState.FillColor = Color.FromArgb(60, 60, 60);
            CleanLogsButton.HoverState.ForeColor = Color.White;
            CleanLogsButton.Image = Properties.Resources.uac;
            CleanLogsButton.Location = new Point(12, 189);
            CleanLogsButton.Name = "CleanLogsButton";
            CleanLogsButton.ShadowDecoration.CustomizableEdges = customizableEdges10;
            CleanLogsButton.Size = new Size(125, 33);
            CleanLogsButton.TabIndex = 47;
            CleanLogsButton.Text = "Clean logs";
            CleanLogsButton.Click += CleanLogsButton_Click;
            // 
            // BanReasonButton
            // 
            BanReasonButton.BorderColor = Color.FromArgb(60, 60, 60);
            BanReasonButton.BorderRadius = 2;
            BanReasonButton.BorderThickness = 2;
            BanReasonButton.Cursor = Cursors.Hand;
            BanReasonButton.CustomBorderColor = Color.FromArgb(60, 60, 60);
            BanReasonButton.CustomBorderThickness = new Padding(1);
            BanReasonButton.CustomizableEdges = customizableEdges11;
            BanReasonButton.DisabledState.BorderColor = Color.DarkGray;
            BanReasonButton.DisabledState.CustomBorderColor = Color.DarkGray;
            BanReasonButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BanReasonButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BanReasonButton.FillColor = Color.FromArgb(40, 40, 40);
            BanReasonButton.Font = new Font("Inter SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BanReasonButton.ForeColor = Color.FromArgb(225, 225, 225);
            BanReasonButton.HoverState.BorderColor = Color.FromArgb(80, 80, 80);
            BanReasonButton.HoverState.CustomBorderColor = Color.FromArgb(80, 80, 80);
            BanReasonButton.HoverState.FillColor = Color.FromArgb(60, 60, 60);
            BanReasonButton.HoverState.ForeColor = Color.White;
            BanReasonButton.Location = new Point(143, 189);
            BanReasonButton.Name = "BanReasonButton";
            BanReasonButton.ShadowDecoration.CustomizableEdges = customizableEdges12;
            BanReasonButton.Size = new Size(174, 33);
            BanReasonButton.TabIndex = 48;
            BanReasonButton.Text = "Check ban reason";
            BanReasonButton.Click += BanReasonButton_Click;
            // 
            // OldPatch
            // 
            OldPatch.AutoSize = true;
            OldPatch.CheckedState.BorderColor = Color.DodgerBlue;
            OldPatch.CheckedState.BorderRadius = 1;
            OldPatch.CheckedState.BorderThickness = 1;
            OldPatch.CheckedState.FillColor = Color.DeepSkyBlue;
            OldPatch.Cursor = Cursors.Hand;
            OldPatch.Font = new Font("Inter Tight SemiBold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OldPatch.ForeColor = Color.FromArgb(225, 225, 225);
            OldPatch.Location = new Point(350, 85);
            OldPatch.Name = "OldPatch";
            OldPatch.Size = new Size(188, 27);
            OldPatch.TabIndex = 49;
            OldPatch.Text = "Legacy Patch Number";
            OldPatch.UncheckedState.BorderColor = Color.Gray;
            OldPatch.UncheckedState.BorderRadius = 1;
            OldPatch.UncheckedState.BorderThickness = 1;
            OldPatch.UncheckedState.FillColor = Color.Silver;
            OldPatch.CheckedChanged += OldPatch_CheckedChanged;
            // 
            // ConfigSeperatorLeft
            // 
            ConfigSeperatorLeft.FillThickness = 2;
            ConfigSeperatorLeft.Location = new Point(12, 35);
            ConfigSeperatorLeft.Name = "ConfigSeperatorLeft";
            ConfigSeperatorLeft.Size = new Size(235, 17);
            ConfigSeperatorLeft.TabIndex = 28;
            // 
            // LeaguePatchCollectionUX
            // 
            AutoScaleDimensions = new SizeF(12F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.FromArgb(20, 20, 20);
            ClientSize = new Size(600, 400);
            ControlBox = false;
            Controls.Add(OldPatch);
            Controls.Add(BanReasonButton);
            Controls.Add(CleanLogsButton);
            Controls.Add(MiscSeperatorLeft);
            Controls.Add(MiscLabel);
            Controls.Add(MiscSeperatorRight);
            Controls.Add(DisconnectChatButton);
            Controls.Add(ShowMobileButton);
            Controls.Add(ShowAwayButton);
            Controls.Add(ShowOfflineButton);
            Controls.Add(ShowOnlineButton);
            Controls.Add(AppearAsLabel);
            Controls.Add(ConfigSeperatorRight);
            Controls.Add(NoBloatware);
            Controls.Add(ChatSeperatorRight);
            Controls.Add(ChatLabel);
            Controls.Add(ChatSeperatorLeft);
            Controls.Add(ConfigSeperatorLeft);
            Controls.Add(SupressBehavior);
            Controls.Add(NameChangeBypass);
            Controls.Add(LegacyHonor);
            Controls.Add(DisableVanguard);
            Controls.Add(SectionLabelConfig);
            Controls.Add(MainHeaderBackdrop);
            Controls.Add(MainControllerBackdrop);
            Font = new Font("Beaufort for LOL", 16F, FontStyle.Bold);
            ForeColor = Color.Transparent;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5);
            MaximizeBox = false;
            Name = "LeaguePatchCollectionUX";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "League Patch Collection";
            MainControllerBackdrop.ResumeLayout(false);
            MainHeaderBackdrop.ResumeLayout(false);
            MainHeaderBackdrop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel MainControllerBackdrop;
        private Guna.UI2.WinForms.Guna2Button StartButton;
        private Panel MainHeaderBackdrop;
        private Guna.UI2.WinForms.Guna2HtmlLabel SectionLabelConfig;
        private Guna.UI2.WinForms.Guna2CheckBox DisableVanguard;
        private Guna.UI2.WinForms.Guna2CheckBox LegacyHonor;
        private Guna.UI2.WinForms.Guna2CheckBox NameChangeBypass;
        private Guna.UI2.WinForms.Guna2CheckBox SupressBehavior;
        private Guna.UI2.WinForms.Guna2ControlBox MinimizeButton;
        private Guna.UI2.WinForms.Guna2HtmlLabel WindowTitle;
        private Guna.UI2.WinForms.Guna2Separator ChatSeperatorLeft;
        private Guna.UI2.WinForms.Guna2HtmlLabel ChatLabel;
        private Guna.UI2.WinForms.Guna2Separator ChatSeperatorRight;
        private Guna.UI2.WinForms.Guna2CheckBox NoBloatware;
        private Guna.UI2.WinForms.Guna2Separator ConfigSeperatorRight;
        private Guna.UI2.WinForms.Guna2HtmlLabel AppearAsLabel;
        private Guna.UI2.WinForms.Guna2RadioButton ShowOnlineButton;
        private Guna.UI2.WinForms.Guna2RadioButton ShowOfflineButton;
        private Guna.UI2.WinForms.Guna2RadioButton ShowAwayButton;
        private Guna.UI2.WinForms.Guna2RadioButton ShowMobileButton;
        private Guna.UI2.WinForms.Guna2Button DisconnectChatButton;
        private Guna.UI2.WinForms.Guna2Separator MiscSeperatorRight;
        private Guna.UI2.WinForms.Guna2HtmlLabel MiscLabel;
        private Guna.UI2.WinForms.Guna2Separator MiscSeperatorLeft;
        private Guna.UI2.WinForms.Guna2Button CleanLogsButton;
        private Guna.UI2.WinForms.Guna2ControlBox CloseButton;
        private Guna.UI2.WinForms.Guna2Button BanReasonButton;
        private Guna.UI2.WinForms.Guna2CheckBox OldPatch;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Separator ConfigSeperatorLeft;
    }
}
