using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KRIP2_LAB3_HESH
{
    public partial class Form1 : Form
    {
        public static uint leftRotate(uint x, int c)
        {
            return (x << c) | (x >> (32 - c));
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] s = new int[64] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
     
            uint[] K = new uint[64] {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

            String open_text = (textBox5.Text);
            byte[] str_code_ascii_orig = Encoding.ASCII.GetBytes(open_text);
 

            var addLength = (56 - ((str_code_ascii_orig.Length + 1) % 64)) % 64; // (448 - (длина_строки + 8) mod 512) mod 512;
            var str_code_ascii = new byte[str_code_ascii_orig.Length + 1 + addLength + 8]; // длина строки + 1 + добавочные биты + 8(информация о длинне сообщения);

            // ШАГ 1
            str_code_ascii[str_code_ascii_orig.Length] = 0x80; // добавяляем единичный бит

            Array.Copy(str_code_ascii_orig, str_code_ascii, str_code_ascii_orig.Length);

            

            byte[] length = BitConverter.GetBytes(str_code_ascii_orig.Length * 8); // получаем длинну искомой последовательности битов

            // ШАГ 2
            Array.Copy(length, 0, str_code_ascii, str_code_ascii.Length - 8, 4); // записываем длинну искомой последовательности вконец

            // ШАГ 3
            uint a0 = 0x67452301;
            uint b0 = 0xefcdab89;
            uint c0 = 0x98badcfe;
            uint d0 = 0x10325476;

            for (int i = 0; i < str_code_ascii.Length / 64; ++i)
            {             
                uint[] M = new uint[16];
                for (int j = 0; j < 16; ++j)
                    M[j] = BitConverter.ToUInt32(str_code_ascii, (i * 64) + (j * 4)); // разбиваем на блоки по 32 бита


                uint A = a0, B = b0, C = c0, D = d0, F = 0;
                int g = 0;

                for (int j = 0; j < 64; ++j)
                {
                    if (j <= 15)
                    {
                        F = (B & C) | (~B & D);
                        g = j;
                    }
                    else if (j >= 16 && j <= 31)
                    {
                        F = (D & B) | (~D & C);
                        g = ((5 * j) + 1) % 16;
                    }
                    else if (j >= 32 && j <= 47)
                    {
                        F = B ^ C ^ D;
                        g = ((3 * j) + 5) % 16;
                    }
                    else if (j >= 48)
                    {
                        F = C ^ (B | ~D);
                        g = (7 * j) % 16;
                    }

                    F = F + A + K[j] + M[g];
                    A = D;
                    D = C;
                    C = B;
                    B = B + leftRotate(F, s[j]);                
                    
                }

                a0 += A;
                b0 += B;
                c0 += C;
                d0 += D;
            }
            // шаг 5
            textBox6.Text = String.Join("", BitConverter.GetBytes(a0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(b0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(c0).Select(y => y.ToString("x2"))) + String.Join("", BitConverter.GetBytes(d0).Select(y => y.ToString("x2")));
        }
    }
}
