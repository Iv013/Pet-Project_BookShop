using System.Collections.ObjectModel;

namespace MyBookShop_Utility
{
    public static class WC
    {
        public static string ImagePath = @"\Images\Book\";
        public static string SessionCart = "ShopingCartSession";
        public const string SessionOrderId = "OrderSession";
        public static string AdminRole = "Admin";
        public static string CostumerRole = "Costumer";
        public const string AdminEmail = "iamolokov@gmail.com";
        public static string AuthorName = "Author";
        public const string GenreName = "Genre";
        public const string Success = "Success";
        public const string Error = "Error";

        public const string StatusPending = "Ожидание подтверждения";
        public const string StatusInProcess = "В процессе сборки";
        public const string StatusDelivery = "Доставка";
        public const string StatusCancellind = "Отменен";
        public const string StatusComplited = "Выполнен";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(

        new List<string>
        {StatusPending,  StatusInProcess, StatusDelivery,  StatusCancellind, StatusComplited

        });


    }
}
