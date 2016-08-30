using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Passport
{
    public struct CommonData
    {
        // 프로그램 버전
        public const string ProgramVersion = "1.1";

        // 암호화 기본 정보
        public static readonly byte[] EncryptSalt = { 67, 182, 141, 79, 171, 211, 150, 133 };
        public const string HashSalt = "OMRFKS2IJX";

        // 데이터 저장 경로
        public const string Folder = "Info";
        public const string PasswordFile = "Password";
        public const string DataFile = "Data";
    }
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (File.Exists(Path.Combine(Application.StartupPath, CommonData.Folder, CommonData.PasswordFile)))
            {
                Application.Run(new Login());
            }
            else
            {
                MessageBox.Show("파일이 없습니다. 어여 프로그램 만듭시다.");
                //Application.Run(new (비밀번호 생성 폼 열기));
            }
        }
    }
}
