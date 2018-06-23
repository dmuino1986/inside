namespace Inside.Xamarin.Helpers
{
    public static class HostSetting
    {
        // No es necesario que estas settings esten salvadas en disco!
        private static readonly string InsideApiUrl = "http://localhost:5041/api"; //"http://149.202.41.48:5041/api";

        public static string AuthEndPoint => string.Format("{0}/auth", InsideApiUrl);
        public static string OrderEndPoint => string.Format("{0}/order", InsideApiUrl);
        public static string ParkingCategoryEndPoint => string.Format("{0}/parkingcategory", InsideApiUrl);
        public static string ParkingTypeEndPoint => string.Format("{0}/parkingtype", InsideApiUrl);
        public static string ParkingEndPoint => string.Format("{0}/parking", InsideApiUrl);
        public static string LoginEndpoint => string.Format("{0}/auth/login", InsideApiUrl);
        public static string RegisterUserEndPoint => string.Format("{0}/auth/register", InsideApiUrl);
        public static string EventEndPoint => string.Format("{0}/event", InsideApiUrl);
        public static string EditProfileEndPoint => string.Format("{0}/auth/editprofile", InsideApiUrl);
        public static string ChangePasswordEndPoint => string.Format("{0}/auth/changepassword", InsideApiUrl);
    }
}