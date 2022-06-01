using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    public class Splasher
    {
        private static Form m_SplashForm = null;
        private static ISplashForm m_SplashInterface = null;
        private static Thread m_SplashThread = null;
        private static string m_TempStatus = string.Empty;

        /// <summary>
        /// Show the SplashForm
        /// </summary>
        public static void Show(Type splashFormType)
        {
            if (m_SplashThread != null)
                return;
            if (splashFormType == null)
            {
                throw (new Exception("splashFormType is null"));
            }

            m_SplashThread = new Thread(new ThreadStart(delegate ()
            {
                CreateInstance(splashFormType);
                Application.Run(m_SplashForm);
            }));

            m_SplashThread.IsBackground = true;
            m_SplashThread.SetApartmentState(ApartmentState.STA);
            m_SplashThread.Start();
        }



        /// <summary>
        /// set the loading Status
        /// </summary>
        public static string Status
        {
            set
            {
                if (m_SplashInterface == null || m_SplashForm == null)
                {
                    m_TempStatus = value;
                    return;
                }

                m_SplashForm.Invoke(
                        new SplashStatusChangedHandle(delegate (string str) { m_SplashInterface.SetStatusInfo(str); }),
                        new object[] { value }
                    );
            }

        }

        /// <summary>
        /// Colse the SplashForm
        /// </summary>
        public static void Close()
        {
            if (m_SplashThread == null || m_SplashForm == null) return;

            m_SplashForm.Invoke(new MethodInvoker(m_SplashForm.Close));
            m_SplashThread = null;
            m_SplashForm = null;
        }

        private static void CreateInstance(Type FormType)
        {

            //利用反射创建对象
            object obj = Activator.CreateInstance(FormType);

            m_SplashForm = obj as Form;
            m_SplashInterface = obj as ISplashForm;
            if (m_SplashForm == null)
            {
                throw (new Exception("Splash Screen must inherit from System.Windows.Forms.Form"));
            }
            if (m_SplashInterface == null)
            {
                throw (new Exception("must implement interface ISplashForm"));
            }

            if (!string.IsNullOrEmpty(m_TempStatus))
                m_SplashInterface.SetStatusInfo(m_TempStatus);
        }


        private delegate void SplashStatusChangedHandle(string NewStatusInfo);

    }
    /// <summary>
    /// interface for Splash Screen
    /// </summary>
    public interface ISplashForm
    {
        void SetStatusInfo(string NewStatusInfo);
    }
}
