using System;
using System.Collections.Generic;
using System.Linq;

namespace Labarator_work_Module_11
{
    // Интерфейс для управления пользователями
    public interface IUserService
    {
        User Register(string username, string password);
        User Login(string username, string password);
    }

    // Интерфейс для управления товарами
    public interface IProductService
    {
        List<Product> GetProducts();
        Product AddProduct(Product product);
    }

    // Интерфейс для управления заказами
    public interface IOrderService
    {
        Order CreateOrder(int userId, List<int> productIds);
        Order GetOrderStatus(int orderId);
    }

    // Интерфейс для обработки платежей
    public interface IPaymentService
    {
        bool ProcessPayment(int orderId, decimal amount);
    }

    // Интерфейс для отправки уведомлений
    public interface INotificationService
    {
        void SendNotification(int userId, string message);
    }
    // Модель для пользователя
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Модель для товара
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Модель для заказа
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<Product> Products { get; set; }
        public string Status { get; set; }
    }
    public class UserService : IUserService
    {
        private List<User> users = new List<User>();

        public User Register(string username, string password)
        {
            var user = new User { Id = users.Count + 1, Username = username, Password = password };
            users.Add(user);
            return user;
        }

        public User Login(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
    public class ProductService : IProductService
    {
        private List<Product> products = new List<Product>();

        public List<Product> GetProducts()
        {
            return products;
        }

        public Product AddProduct(Product product)
        {
            products.Add(product);
            return product;
        }
    }
    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(int orderId, decimal amount)
        {
            // Симуляция успешного платежа
            return true; // Для примера всегда возвращаем успешную оплату
        }
    }
    public class NotificationService : INotificationService
    {
        public void SendNotification(int userId, string message)
        {
            // Симуляция отправки уведомления
            Console.WriteLine($"Уведомление пользователю {userId}: {message}");
        }
    }
    public class OrderService : IOrderService
    {
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;

        public OrderService(IProductService productService, IPaymentService paymentService, INotificationService notificationService)
        {
            _productService = productService;
            _paymentService = paymentService;
            _notificationService = notificationService;
        }

        public Order CreateOrder(int userId, List<int> productIds)
        {
            // Проверка наличия продуктов
            var products = _productService.GetProducts().Where(p => productIds.Contains(p.Id)).ToList();
            if (!products.Any())
            {
                throw new Exception("Выбранные товары не найдены.");
            }

            // Создание заказа
            var order = new Order { Id = new Random().Next(1, 1000), UserId = userId, Products = products, Status = "Created" };

            // Обработка платежа
            decimal totalAmount = products.Sum(p => p.Price);
            if (_paymentService.ProcessPayment(order.Id, totalAmount))
            {
                order.Status = "Paid";
                _notificationService.SendNotification(userId, "Ваш заказ успешно оплачен.");
            }
            else
            {
                order.Status = "Payment Failed";
                _notificationService.SendNotification(userId, "Платеж не прошел. Попробуйте снова.");
            }

            return order;
        }

        public Order GetOrderStatus(int orderId)
        {
            // Возвращает статус заказа
            return new Order { Id = orderId, Status = "In Progress" };
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Создание сервисов
            IUserService userService = new UserService();
            IProductService productService = new ProductService();
            IPaymentService paymentService = new PaymentService();
            INotificationService notificationService = new NotificationService();
            IOrderService orderService = new OrderService(productService, paymentService, notificationService);

            // Регистрация пользователя
            var user = userService.Register("john_doe", "password123");

            // Добавление товаров
            productService.AddProduct(new Product { Id = 1, Name = "Laptop", Price = 1000m });
            productService.AddProduct(new Product { Id = 2, Name = "Smartphone", Price = 500m });

            // Создание заказа
            var order = orderService.CreateOrder(user.Id, new List<int> { 1, 2 });

            // Получение статуса заказа
            var status = orderService.GetOrderStatus(order.Id);
            Console.WriteLine($"Статус заказа: {status.Status}");

            Console.ReadKey();
        }
    }


}
