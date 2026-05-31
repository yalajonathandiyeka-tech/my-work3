using System;
using System.Drawing;
using System.Windows.Forms;
using ETS.Models;

namespace ETS.Forms
{
    public class MemberForm : Form
    {
        private Member _member;

        // Header
        private Panel pnlHeader;
        private Label lblTitle, lblWelcome;
        private Button btnLogout, btnViewTx;

        // Member info panel
        private Panel pnlInfo;
        private Label lblNameVal, lblEmailVal, lblBalanceVal;

        // Booking section
        private GroupBox grpBook;
        private ComboBox cmbEvent;
        private NumericUpDown nudQty;
        private Label lblEvent, lblQty, lblCost, lblCostVal;
        private Button btnBook, btnRefresh;

        public MemberForm(Member member)
        {
            _member = member;
            InitializeComponent();
            PopulateEvents();
            RefreshInfo();
        }

        private void InitializeComponent()
        {
            this.Text = "ETS – Member Portal";
            this.Size = new Size(700, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Header
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = Color.FromArgb(30, 60, 120) };

            lblTitle = new Label
            {
                Text = "Member Portal",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Left = 20, Top = 15, AutoSize = true
            };

            btnLogout = new Button
            {
                Text = "Logout", Width = 90, Height = 32,
                Top = 17, Left = 580,
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => this.Close();

            btnViewTx = new Button
            {
                Text = "My Transactions", Width = 130, Height = 32,
                Top = 17, Left = 440,
                BackColor = Color.FromArgb(60, 130, 70),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand, Font = new Font("Segoe UI", 9)
            };
            btnViewTx.FlatAppearance.BorderSize = 0;
            btnViewTx.Click += (s, e) =>
            {
                var txForm = new TransactionsForm(_member);
                txForm.ShowDialog(this);
            };

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, btnLogout, btnViewTx });

            // Info Panel
            pnlInfo = new Panel
            {
                Left = 20, Top = 85, Width = 640, Height = 100,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblWelcome = new Label
            {
                Text = "Account Information",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 120),
                Left = 12, Top = 10, AutoSize = true
            };

            lblNameVal = new Label
            {
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 80),
                Left = 12, Top = 36, AutoSize = true
            };

            lblEmailVal = new Label
            {
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 80),
                Left = 12, Top = 58, AutoSize = true
            };

            lblBalanceVal = new Label
            {
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 130, 60),
                Left = 420, Top = 36, AutoSize = true
            };

            pnlInfo.Controls.AddRange(new Control[]
            {
                lblWelcome, lblNameVal, lblEmailVal, lblBalanceVal
            });

            // Booking Group
            grpBook = new GroupBox
            {
                Text = "Book Tickets",
                Left = 20, Top = 205,
                Width = 640, Height = 270,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 120),
                BackColor = Color.White
            };

            lblEvent = new Label
            {
                Text = "Select Event:", Left = 20, Top = 35,
                AutoSize = true, Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 80)
            };

            cmbEvent = new ComboBox
            {
                Left = 20, Top = 58, Width = 380, Height = 32,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbEvent.SelectedIndexChanged += UpdateCostPreview;

            lblQty = new Label
            {
                Text = "Number of Tickets:", Left = 20, Top = 105,
                AutoSize = true, Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 80)
            };

            nudQty = new NumericUpDown
            {
                Left = 20, Top = 128, Width = 120, Height = 32,
                Minimum = 1, Maximum = 100,
                Font = new Font("Segoe UI", 10), Value = 1
            };
            nudQty.ValueChanged += UpdateCostPreview;

            lblCost = new Label
            {
                Text = "Estimated Total:", Left = 20, Top = 175,
                AutoSize = true, Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(50, 50, 80)
            };

            lblCostVal = new Label
            {
                Text = "R 0.00", Left = 155, Top = 175,
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 80, 10)
            };

            btnBook = new Button
            {
                Text = "Confirm Booking",
                Left = 20, Top = 215,
                Width = 180, Height = 42,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(30, 60, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBook.FlatAppearance.BorderSize = 0;
            btnBook.Click += BtnBook_Click;

            btnRefresh = new Button
            {
                Text = "Refresh Events",
                Left = 220, Top = 215,
                Width = 150, Height = 42,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(80, 120, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => PopulateEvents();

            grpBook.Controls.AddRange(new Control[]
            {
                lblEvent, cmbEvent,
                lblQty, nudQty,
                lblCost, lblCostVal,
                btnBook, btnRefresh
            });

            this.Controls.AddRange(new Control[] { pnlHeader, pnlInfo, grpBook });
        }

        private void RefreshInfo()
        {
            lblNameVal.Text = $"Name:   {_member.Name}";
            lblEmailVal.Text = $"Email:  {_member.Email}";
            lblBalanceVal.Text = $"Balance: R {_member.Balance:N2}";
        }

        private void PopulateEvents()
        {
            cmbEvent.DataSource = null;
            cmbEvent.DataSource = new System.Collections.Generic.List<Event>(AppData.Events);
            cmbEvent.DisplayMember = "EventName";
            UpdateCostPreview(null, null);
        }

        private void UpdateCostPreview(object sender, EventArgs e)
        {
            try
            {
                if (cmbEvent.SelectedItem is Event ev)
                {
                    decimal total = ev.Price * nudQty.Value;
                    lblCostVal.Text = $"R {total:N2}";
                    lblCostVal.ForeColor = total > _member.Balance
                        ? Color.FromArgb(180, 30, 30)
                        : Color.FromArgb(30, 130, 60);
                }
            }
            catch { }
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbEvent.SelectedItem == null)
                    throw new Exception("Please select an event.");

                var ev = (Event)cmbEvent.SelectedItem;
                int qty = (int)nudQty.Value;
                decimal total = ev.Price * qty;

                if (!ev.HasAvailableTickets(qty))
                    throw new Exception($"Only {ev.AvailableTickets} ticket(s) available for '{ev.EventName}'.");

                if (_member.Balance < total)
                    throw new Exception($"Insufficient balance. You need R {total:N2} but have R {_member.Balance:N2}.");

                // Confirm
                var confirm = MessageBox.Show(
                    $"Book {qty} ticket(s) for '{ev.EventName}'?\nTotal: R {total:N2}",
                    "Confirm Booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                // Process
                ev.BookTickets(qty);
                _member.Deduct(total);

                var tx = new Transaction(ev.EventName, total, _member.Email, qty);
                AppData.Transactions.Add(tx);

                RefreshInfo();
                UpdateCostPreview(null, null);

                MessageBox.Show(
                    $"Booking successful!\n{qty} ticket(s) booked for '{ev.EventName}'.\nR {total:N2} deducted.\nNew balance: R {_member.Balance:N2}",
                    "Booking Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Booking Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
