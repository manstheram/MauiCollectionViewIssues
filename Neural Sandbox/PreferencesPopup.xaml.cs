using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;

namespace Neural_Sandbox
{
    public partial class PreferencesPopup : Popup
    {
        public PreferencesPopup()
        {
            InitializeComponent();
        }
        public void PressedYes(object sender, EventArgs e)
        {
            Close(true);
        }
        public void PressedNo(object sender, EventArgs e)
        {
            Close(false);
        }
    }
}
