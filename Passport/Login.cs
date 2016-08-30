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
using System.Collections;
//txtID는 ID, txtPW는 AES키
namespace Passport
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AES256 AES = new AES256();
            string hashString = null; byte[] encryptedBytes = null;

            // 암호화 기본 설정
            AES.KeyString = txtID.Text;
            AES.SaltBytes = CommonData.EncryptSalt;

            // 비밀번호 암호화
            hashString = Convert.ToBase64String(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(CommonData.HashSalt + txtID.Text)));
            encryptedBytes = AES.EncryptByte(Encoding.UTF8.GetBytes(hashString + txtID.Text));

            // 정보 로드 및 배열 비교 준비
            byte[] loadData = File.ReadAllBytes(Path.Combine(Application.StartupPath, CommonData.Folder, CommonData.PasswordFile));
            IStructuralEquatable SE = loadData;

            // 배열 서로 비교
            if (SE.Equals(encryptedBytes, StructuralComparisons.StructuralEqualityComparer))
            {
                frmMain form = new frmMain();
                form.Show(); this.Hide();
            }
            else
            {
                MessageBox.Show("로그인에 실패하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
        }
    }
}
