using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.Authority.Contract;

namespace NJIS.Windows.TemplateBase.AuthorityControl
{
    public partial class AuthorityTool : Component, IAuthorityControl
    {
        private List<Control> _authorityList = new List<Control>();

        public AuthorityTool()
        {
            InitializeComponent();
        }

        public AuthorityTool(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [Category("NJAuthority")]
        public string AuthorityPath => this.GetType().ReflectedType.FullName;


        [Browsable(true)]
        [Description("指定权限控制的控件"), Category("NJAuthority")]
        public List<Control> AuthorityList
        {
            get { return _authorityList; }
            set { _authorityList = value; }
        }

        public string InstanceName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAuthorityControl
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetValue(bool holdAuthority)
        {
            throw new NotImplementedException();
        }
    }
}
