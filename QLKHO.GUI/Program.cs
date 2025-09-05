using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKHO.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Hiển thị form đăng nhập trước
            var frmDangNhap = new FrmDangNhap();
            if (frmDangNhap.ShowDialog() == DialogResult.OK)
            {
                // Nếu đăng nhập thành công, mở form chính
                var frmMain = new FrmMain(frmDangNhap.NguoiDungDangNhap);
                Application.Run(frmMain);
            }
            else
            {
                // Nếu hủy đăng nhập, thoát ứng dụng
                Application.Exit();
            }
        }
    }
}
