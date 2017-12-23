using iSched3.Data;
using iSched3.Services;
using Prism.Unity;
using iSched3.Views;
using Microsoft.Practices.Unity;
using Prism.Navigation;
using Xamarin.Forms;

namespace iSched3
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterType<IApiService, ApiService>();
            Container.RegisterType<IAppointmentsRepository, AppointmentRepository>();
            Container.RegisterTypeForNavigation<RegisterPage>();
            Container.RegisterTypeForNavigation<DashboardPage>();
            Container.RegisterTypeForNavigation<AppointmentDetailPage>();
            Container.RegisterTypeForNavigation<SaveAppointmentPage>();
        }
    }
}
