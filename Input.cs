using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libLeMS
{
    public class InputBox: Shell
    {
        private Config config;
        private string v1;
        private string v2;

        public InputBox(string v1, string v2, Config config)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.config = config;
        }

        public string Response { get; set; }

        public System.Windows.Forms.DialogResult ShowDialog()
        {
            return System.Windows.Forms.DialogResult.Ignore;
        }
    }
}
