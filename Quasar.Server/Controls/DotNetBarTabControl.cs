﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// thanks to Mavamaarten~ for coding this

namespace Quasar.Server.Controls
{
    internal class DotNetBarTabControl : TabControl
    {
        public DotNetBarTabControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(44, 136);
            Alignment = TabAlignment.Left;
            SelectedIndex = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap b = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(b);

            if (!DesignMode)
                SelectedTab.BackColor = Color.FromArgb(43, 43, 43); // Dark background for selected tab

            g.Clear(Color.FromArgb(43, 43, 43)); // Overall background color
            g.FillRectangle(new SolidBrush(Color.FromArgb(43, 43, 43)),
                new Rectangle(0, 0, ItemSize.Height + 4, Height)); // Sidebar background
            g.DrawLine(new Pen(Color.FromArgb(80, 80, 80)), new Point(ItemSize.Height + 3, 0),
                new Point(ItemSize.Height + 3, 999)); // Divider line
            g.DrawLine(new Pen(Color.FromArgb(80, 80, 80)), new Point(0, Size.Height - 1),
                new Point(Width + 3, Size.Height - 1));

            for (int i = 0; i <= TabCount - 1; i++)
            {
                if (i == SelectedIndex)
                {
                    Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2),
                        new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(70, 70, 70)), x2); // Selected tab background
                    g.DrawRectangle(new Pen(Color.FromArgb(43, 43, 43)), x2);

                    g.SmoothingMode = SmoothingMode.HighQuality;
                    if (ImageList != null)
                    {
                        try
                        {
                            g.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                new Point(x2.Location.X + 8, x2.Location.Y + 6));
                            g.DrawString("  " + TabPages[i].Text, Font, Brushes.White, x2, new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                        }
                        catch (Exception)
                        {
                            g.DrawString(TabPages[i].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                                Brushes.White, x2, new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                });
                        }
                    }
                    else
                    {
                        g.DrawString(TabPages[i].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                            Brushes.White, x2, new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                    }
                }
                else
                {
                    Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2),
                        new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(43, 43, 43)), x2); // Unselected tab background
                    g.DrawLine(new Pen(Color.FromArgb(80, 80, 80)), new Point(x2.Right, x2.Top),
                        new Point(x2.Right, x2.Bottom));

                    if (ImageList != null)
                    {
                        try
                        {
                            g.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                new Point(x2.Location.X + 8, x2.Location.Y + 6));
                            g.DrawString("  " + TabPages[i].Text, Font, Brushes.LightGray, x2, new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                        }
                        catch (Exception)
                        {
                            g.DrawString(TabPages[i].Text, Font, Brushes.LightGray, x2, new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                        }
                    }
                    else
                    {
                        g.DrawString(TabPages[i].Text, Font, Brushes.LightGray, x2, new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        });
                    }
                }
            }

            e.Graphics.DrawImage(b, new Point(0, 0));
            g.Dispose();
            b.Dispose();
        }
    }
}
