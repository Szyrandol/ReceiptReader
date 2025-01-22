using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOCRFlaskAPITest
{
    internal class Item
    {
        public string Name { get; set; }
        public double Value { get; set; }

        //quantity?
        //total value?
        public Item(string newline)
        {
            StringBuilder sb = new ();
            bool isNamed = false;
            bool isPrevWS = false;

            foreach (char c in newline)
            {
                sb.Append(c);

                if (!isNamed)
                {
                    if (isPrevWS && char.IsWhiteSpace(c))
                    {
                        sb.Length -= 1;
                        Name = sb.ToString().Trim();
                        isNamed = true;
                        continue;
                    }
                    isPrevWS = char.IsWhiteSpace(c);
                }
            }

            for (int i = sb.Length - 1; i > 0; --i)
            {
                if (sb[i] == ' ') // ocr sometimes sees a space where there is none
                {
                    if (sb[i - 1] == ',' || sb[i - 1] == '\'') // or an apostrophe instead of a comma
                        continue;

                    sb.Remove(0, i);
                    //sb.Remove(sb.Length - 1, 1);
                    break;
                }
            }

            sb.Remove(sb.Length - 1, 1);

            sb.Replace(" ", ""); // ocr workaround again

            Value = double.Parse(sb.ToString()) / 100;
        }
    }
}
