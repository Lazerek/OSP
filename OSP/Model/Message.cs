
namespace OSP.Model
{
    /// <summary>
    /// Klasa wiadomości przesyłająca token
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Token wiadomości
        /// </summary>
        public NotificationToken Token { get; set; }

        public User User { get; set; }

        public Message(NotificationToken token)
        {
            Token = token;
        }

        public Message(User user)
        {
            User = user;
        }
    }
    /// <summary>
    /// Klasa wiadomości przesyłającej zawrtość oraz token
    /// </summary>
    /// <typeparam name="T">Zawartość wiadomości</typeparam>
    public class Message<T>
    {
        public NotificationToken Token { get; set; } //Token wiadomości

        public T Content { get; set; } //Zawartość

        public Message(NotificationToken token, T content) //Konstruktor wymagający zarówno tokenu jak i zawartości
        {
            Token = token;
            Content = content;
        }
    }

    public class Message<T1, T2>
    {
        /// <summary>
        /// Token wiadomości
        /// </summary>
        public NotificationToken Token { get; set; }

        /// <summary>
        /// Zawartość wiadomości
        /// </summary>
        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }

        public Message(NotificationToken token, T1 content1, T2 content2)
        {
            Token = token;
            Content1 = content1;
            Content2 = content2;
        }
    }

    public class Message<T1, T2, T3>
    {
        /// <summary>
        /// Token wiadomości
        /// </summary>
        public NotificationToken Token { get; set; }

        /// <summary>
        /// Zawartość wiadomości
        /// </summary>
        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }

        public T3 Content3 { get; set; }

        public Message(NotificationToken token, T1 content1, T2 content2, T3 content3)
        {
            Token = token;
            Content1 = content1;
            Content2 = content2;
            Content3 = content3;
        }
    }
}