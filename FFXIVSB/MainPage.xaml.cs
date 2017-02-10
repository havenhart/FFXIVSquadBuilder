using FFXIVSB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FFXIVSB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<SquadMember> SquadMembers = new ObservableCollection<SquadMember>();
        private List<int[]> combos = new List<int[]>();
        private List<int[]> training = new List<int[]>();
        private List<string> trainingTypes = new List<string>();
        private int SelectedID = -1;

        public MainPage()
        {
            this.InitializeComponent();

            ReadSquadData();
            ReadSquadStats();

            #region training
            training.AddRange(new[]
            {
                new[] {40,-20,-20},
                new[] {-20,40,-20},
                new[] {-20,-20,40},
                new[] {20,20,-40},
                new[] {20,-40,20},
                new[] {-40,20,20}
            });
            #endregion

            #region trainingTypes
            trainingTypes.AddRange(new string[]
            {
                "Physical",
                "Mental",
                "Tactical",
                "Physical & Mental",
                "Physical & Tactical",
                "Mental & Tactical"
            });
            #endregion

            #region combos
            combos.AddRange(new []{
                new [] { 1,2,3,4},
                new [] { 1,2,3,5},
                new [] { 1,2,3,6},
                new [] { 1,2,3,7},
                new [] { 1,2,3,8},
                new [] { 1,2,4,5},
                new [] { 1,2,4,6},
                new [] { 1,2,4,7},
                new [] { 1,2,4,8},
                new [] { 1,2,5,6},
                new [] { 1,2,5,7},
                new [] { 1,2,5,8},
                new [] { 1,2,6,7},
                new [] { 1,2,6,8},
                new [] { 1,2,7,8},
                new [] { 1,3,4,5},
                new [] { 1,3,4,6},
                new [] { 1,3,4,7},
                new [] { 1,3,4,8},
                new [] { 1,3,5,6},
                new [] { 1,3,5,7},
                new [] { 1,3,5,8},
                new [] { 1,3,6,7},
                new [] { 1,3,6,8},
                new [] { 1,3,7,8},
                new [] { 1,4,5,6},
                new [] { 1,4,5,7},
                new [] { 1,4,5,8},
                new [] { 1,4,6,7},
                new [] { 1,4,6,8},
                new [] { 1,4,7,8},
                new [] { 1,5,6,7},
                new [] { 1,5,6,8},
                new [] { 1,5,7,8},
                new [] { 1,6,7,8},
                new [] { 2,3,4,5},
                new [] { 2,3,4,6},
                new [] { 2,3,4,7},
                new [] { 2,3,4,8},
                new [] { 2,3,5,6},
                new [] { 2,3,5,7},
                new [] { 2,3,5,8},
                new [] { 2,3,6,7},
                new [] { 2,3,6,8},
                new [] { 2,3,7,8},
                new [] { 2,4,5,6},
                new [] { 2,4,5,7},
                new [] { 2,4,5,8},
                new [] { 2,4,6,7},
                new [] { 2,4,6,8},
                new [] { 2,4,7,8},
                new [] { 2,5,6,7},
                new [] { 2,5,6,8},
                new [] { 2,5,7,8},
                new [] { 2,6,7,8},
                new [] { 3,4,5,6},
                new [] { 3,4,5,7},
                new [] { 3,4,5,8},
                new [] { 3,4,6,7},
                new [] { 3,4,6,8},
                new [] { 3,4,7,8},
                new [] { 3,5,6,7},
                new [] { 3,5,6,8},
                new [] { 3,5,7,8},
                new [] { 3,6,7,8},
                new [] { 4,5,6,7},
                new [] { 4,5,6,8},
                new [] { 4,5,7,8},
                new [] { 4,6,7,8},
                new [] { 5,6,7,8}
            });
#endregion

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SquadMember member = (SquadMember)e.ClickedItem;
            SelectedID = member.ID;
            txtName.Text = member.Name;
            txtPhysical.Text = member.Physical.ToString();
            txtMental.Text = member.Mental.ToString();
            txtTactical.Text = member.Tactical.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtPhysical.Text == "" || txtMental.Text == "" || txtTactical.Text == "")
            {
                ShowAlert("All fields are required.", "Add / Edit Squad Member");
                return;
            }

            if (lstSquad.SelectedIndex > -1)
            {
                using (var db = new SBContext())
                {
                    var member = (from m in db.SquadMembers
                                 where m.ID == SelectedID
                                 select m).FirstOrDefault();

                    member.Name = txtName.Text;
                    member.Physical = Convert.ToInt32(txtPhysical.Text);
                    member.Mental = Convert.ToInt32(txtMental.Text);
                    member.Tactical = Convert.ToInt32(txtTactical.Text);

                    db.SaveChanges();

                    SquadMembers = new ObservableCollection<SquadMember>(db.SquadMembers.ToList());
                    lstSquad.ItemsSource = null;
                    lstSquad.ItemsSource = SquadMembers;
                }
            }
            else
            {
                if (lstSquad.Items.Count < 8)
                {
                    using (var db = new SBContext())
                    {
                        db.SquadMembers.Add(new SquadMember() { Name = txtName.Text, Physical = Convert.ToInt32(txtPhysical.Text), Mental = Convert.ToInt32(txtMental.Text), Tactical = Convert.ToInt32(txtTactical.Text) });
                        db.SaveChanges();

                        SquadMembers = new ObservableCollection<SquadMember>(db.SquadMembers.ToList());
                        lstSquad.ItemsSource = null;
                        lstSquad.ItemsSource = db.SquadMembers.ToList();
                    }
                }
                else
                {
                    ShowAlert("You can have a maximum of 8 squad members.", "Squad Members");
                    return;
                }                
            }

            ClearSquadForm();
            //WriteSquadData();
            txtName.Focus(FocusState.Programmatic);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearSquadForm();
        }

        private void ClearSquadForm()
        {
            SelectedID = -1;
            txtName.Text = "";
            txtPhysical.Text = txtMental.Text = txtTactical.Text = "0";
            lstSquad.SelectedIndex = -1;
        }

        private void Attribute_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            CorrectInvalidStat(sender);
        }

        private void CorrectInvalidStat(TextBox txt)
        {
            if (txt.Text.Length > 0)
            {
                Int32 pos;
                if(txt.SelectionStart < txt.Text.Length)
                {
                    pos = txt.SelectionStart - 1;
                }
                else
                {
                    pos = txt.Text.Length - 1;
                }
                if (pos > -1)
                {
                    if (!char.IsNumber(txt.Text[pos]))
                    {
                        txt.Text = txt.Text.Remove(pos, 1);
                        if (txt.Text.Length > 0)
                        {
                            txt.SelectionStart = txt.Text.Length;
                        }
                    }
                }
            }
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            WriteSquadStats();
            if(lstSquad.Items.Count < 4)
            {
                ShowAlert("You need at least 4 squad members.", "Squad Members");
                return;
            }
            else
            {    
                if(txtRPhysical.Text == "" || txtRMental.Text == "" || txtRTactical.Text == "" ||
                    txtSPhysical.Text == "" || txtSMental.Text == "" || txtSTactical.Text == "")
                {
                    ShowAlert("All Physical, Mental, and Tactical fields are required.", "Squad Configuration");
                    return;
                }            
                var reqPhysical = Convert.ToInt32(txtRPhysical.Text);
                var reqMental = Convert.ToInt32(txtRMental.Text);
                var reqTactical = Convert.ToInt32(txtRTactical.Text);

                Int32 cdx = -1;
                Int32 tdx = -1;
                bool passed = false;

                var p1 = 0;
                var m1 = 0;
                var t1 = 0;

                var sp = Convert.ToInt32(txtSPhysical.Text);
                var sm = Convert.ToInt32(txtSMental.Text);
                var st = Convert.ToInt32(txtSTactical.Text);

                while (!passed)
                {
                    for (var cnt = 0; cnt < combos.Count; cnt++)
                    {
                        p1 = 0;
                        m1 = 0;
                        t1 = 0;

                        int[] arr = combos[cnt];

                        var tmp1 = from s in SquadMembers
                                  where arr.Contains(SquadMembers.IndexOf(s) + 1)
                                  select s;

                        foreach (var itm in tmp1)
                        {
                            p1 += itm.Physical;
                            m1 += itm.Mental;
                            t1 += itm.Tactical;
                        }

                        p1 += sp;
                        m1 += sm;
                        t1 += st;

                        if (p1 >= Convert.ToInt32(txtRPhysical.Text) && m1 >= Convert.ToInt32(txtRMental.Text) && t1 >= Convert.ToInt32(txtRTactical.Text))
                        {
                            cdx = cnt;
                            passed = true;
                            break;
                        }
                    }

                    if (cdx == -1)
                    {
                        tdx++;
                        if(tdx > training.Count - 1)
                        {
                            // can't complete yet...
                            ShowAlert("Consider leveling your current recruits.", "Squad Configuration");
                            return;
                        }
                        sp = Convert.ToInt32(txtSPhysical.Text);
                        sm = Convert.ToInt32(txtSMental.Text);
                        st = Convert.ToInt32(txtSTactical.Text);

                        sp += training[tdx][0];
                        sm += training[tdx][1];
                        st += training[tdx][2];
                    }
                }

                ClearHighlightedMembers();

                var p2 = 0;
                var m2 = 0;
                var t2 = 0;

                for (var i = 0; i < combos[cdx].Length; i++)
                {
                    SquadMembers[combos[cdx][i] - 1] = new SquadMember(SquadMembers[combos[cdx][i] - 1].Name, SquadMembers[combos[cdx][i] - 1].Physical, SquadMembers[combos[cdx][i] - 1].Mental, SquadMembers[combos[cdx][i] - 1].Tactical) { ID = SquadMembers[combos[cdx][i] - 1].ID, DisplayColor = new SolidColorBrush(Colors.Cyan) };
                    p2 += SquadMembers[combos[cdx][i] - 1].Physical;
                    m2 += SquadMembers[combos[cdx][i] - 1].Mental;
                    t2 += SquadMembers[combos[cdx][i] - 1].Tactical;
                }

                txtCPhysical.Text = (Convert.ToInt32(txtSPhysical.Text) + p2).ToString();
                txtCMental.Text = (Convert.ToInt32(txtSMental.Text) + m2).ToString();
                txtCTactical.Text = (Convert.ToInt32(txtSTactical.Text) + t2).ToString();

                txtRecTraining.Text = (tdx == -1 ? "None" : trainingTypes[tdx]); 
            }
        }

        private void ClearHighlightedMembers()
        {
            for(Int32 i = 0; i < SquadMembers.Count;i++)
            {
                SquadMembers[i] = new SquadMember(SquadMembers[i].Name, SquadMembers[i].Physical, SquadMembers[i].Mental, SquadMembers[i].Tactical) { ID = SquadMembers[i].ID, DisplayColor = new SolidColorBrush(Colors.White) };
            }

            lstSquad.ItemsSource = null;
            lstSquad.ItemsSource = SquadMembers;
        }

        private async void ShowAlert(string msg = "", string ttl = "")
        {
            ContentDialog message = new ContentDialog()
            {
                Title = ttl,
                Content = msg
            };

            message.PrimaryButtonText = "Close";

            await message.ShowAsync(); 
        }

        private async void WriteSquadData()
        {
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync("ffxivsb-squad.json", CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                string json = JsonConvert.SerializeObject(SquadMembers);
                await writer.WriteAsync(json);
            }
        }

        private void ReadSquadData()
        {
            try
            {
                using (var db = new SBContext())
                {
                    SquadMembers = new ObservableCollection<SquadMember>(db.SquadMembers.ToList());
                    lstSquad.ItemsSource = SquadMembers;
                }
            }catch(Exception ex)
            {
                
            }
        }

        private void WriteSquadStats()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["SquadPhysical"] = txtSPhysical.Text;
            settings.Values["SquadMental"] = txtSMental.Text;
            settings.Values["SquadTactical"] = txtSTactical.Text;
        }

        private void ReadSquadStats()
        {
            var settings = ApplicationData.Current.LocalSettings;
            txtSPhysical.Text = (settings.Values["SquadPhysical"] ?? "0").ToString();
            txtSMental.Text = (settings.Values["SquadMental"] ?? "0").ToString();
            txtSTactical.Text = (settings.Values["SquadTactical"] ?? "0").ToString();
        }

        private void Attribute_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectionStart = 0;
            ((TextBox)sender).SelectionLength = ((TextBox)sender).Text.Length;
        }

        private void Attribute_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key != Windows.System.VirtualKey.Tab)
            {
                ((TextBox)sender).SelectedText = "";
            }
        }
    }
}
