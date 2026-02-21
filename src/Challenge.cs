using System;

namespace DesignPatternChallenge
{
    // ============================
    // 1) IMPLEMENTOR (Plataforma)
    // ============================
    public interface INotificationRenderer
    {
        void RenderText(string title, string content);
        void RenderImage(string title, string content, string imageUrl);
        void RenderVideo(string title, string content, string videoUrl);
    }

    // ============================
    // 2) CONCRETE IMPLEMENTORS
    // ============================
    public class WebRenderer : INotificationRenderer
    {
        public void RenderText(string title, string content)
        {
            Console.WriteLine("[Web - HTML] <div class='notification'>");
            Console.WriteLine($"  <h3>{title}</h3>");
            Console.WriteLine($"  <p>{content}</p>");
            Console.WriteLine("</div>");
        }

        public void RenderImage(string title, string content, string imageUrl)
        {
            Console.WriteLine("[Web - HTML] <div class='notification-image'>");
            Console.WriteLine($"  <img src='{imageUrl}' />");
            Console.WriteLine($"  <h3>{title}</h3>");
            Console.WriteLine($"  <p>{content}</p>");
            Console.WriteLine("</div>");
        }

        public void RenderVideo(string title, string content, string videoUrl)
        {
            Console.WriteLine("[Web - HTML] <div class='notification-video'>");
            Console.WriteLine($"  <video src='{videoUrl}' controls></video>");
            Console.WriteLine($"  <h3>{title}</h3>");
            Console.WriteLine($"  <p>{content}</p>");
            Console.WriteLine("</div>");
        }
    }

    public class MobileRenderer : INotificationRenderer
    {
        public void RenderText(string title, string content)
        {
            Console.WriteLine("[Mobile - Native] Push Notification:");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Body: {content}");
            Console.WriteLine("Icon: notification_icon.png");
        }

        public void RenderImage(string title, string content, string imageUrl)
        {
            Console.WriteLine("[Mobile - Native] Rich Push Notification:");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Body: {content}");
            Console.WriteLine($"Image: {imageUrl}");
            Console.WriteLine("Style: BigPictureStyle");
        }

        public void RenderVideo(string title, string content, string videoUrl)
        {
            Console.WriteLine("[Mobile - Native] Video Push Notification:");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Body: {content}");
            Console.WriteLine($"Video: {videoUrl}");
            Console.WriteLine("Action: Tap to play");
        }
    }

    public class DesktopRenderer : INotificationRenderer
    {
        public void RenderText(string title, string content)
        {
            Console.WriteLine("[Desktop - Toast] Windows Notification:");
            DrawBox(title, content);
        }

        public void RenderImage(string title, string content, string imageUrl)
        {
            Console.WriteLine("[Desktop - Toast] Windows Notification with Image:");
            DrawBox($"[IMG: {Short(imageUrl, 15)}...]", title);
            Console.WriteLine($"    {content}");
        }

        public void RenderVideo(string title, string content, string videoUrl)
        {
            Console.WriteLine("[Desktop - Toast] Windows Notification with Video:");
            DrawBox($"▶ {Short(videoUrl, 20)}...", title);
            Console.WriteLine($"    {content}");
        }

        private static void DrawBox(string line1, string line2)
        {
            Console.WriteLine("╔══════════════════════════╗");
            Console.WriteLine($"║ {Pad(line1, 24)} ║");
            Console.WriteLine($"║ {Pad(line2, 24)} ║");
            Console.WriteLine("╚══════════════════════════╝");
        }

        private static string Pad(string s, int len) => (s ?? "").Length > len ? (s[..len]) : (s ?? "").PadRight(len);
        private static string Short(string s, int len) => string.IsNullOrEmpty(s) ? "" : (s.Length <= len ? s : s[..len]);
    }

    // ============================
    // 3) ABSTRACTION (Tipo)
    // ============================
    public abstract class Notification
    {
        protected readonly INotificationRenderer _renderer;

        protected Notification(INotificationRenderer renderer)
        {
            _renderer = renderer;
        }

        public abstract void Render();
    }

    // ============================
    // 4) REFINED ABSTRACTIONS
    // ============================
    public class TextNotification : Notification
    {
        private readonly string _title;
        private readonly string _content;

        public TextNotification(INotificationRenderer renderer, string title, string content)
            : base(renderer)
        {
            _title = title;
            _content = content;
        }

        public override void Render()
        {
            _renderer.RenderText(_title, _content);
        }
    }

    public class ImageNotification : Notification
    {
        private readonly string _title;
        private readonly string _content;
        private readonly string _imageUrl;

        public ImageNotification(INotificationRenderer renderer, string title, string content, string imageUrl)
            : base(renderer)
        {
            _title = title;
            _content = content;
            _imageUrl = imageUrl;
        }

        public override void Render()
        {
            _renderer.RenderImage(_title, _content, _imageUrl);
        }
    }

    public class VideoNotification : Notification
    {
        private readonly string _title;
        private readonly string _content;
        private readonly string _videoUrl;

        public VideoNotification(INotificationRenderer renderer, string title, string content, string videoUrl)
            : base(renderer)
        {
            _title = title;
            _content = content;
            _videoUrl = videoUrl;
        }

        public override void Render()
        {
            _renderer.RenderVideo(_title, _content, _videoUrl);
        }
    }

    // ============================
    // 5) DEMO
    // ============================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Notificações (Bridge) ===\n");

            // Renderers (plataformas)
            INotificationRenderer web = new WebRenderer();
            INotificationRenderer mobile = new MobileRenderer();
            INotificationRenderer desktop = new DesktopRenderer();

            // Tipos (abstrações) combinados com plataformas (implementação)
            Notification textWeb = new TextNotification(web, "Novo Pedido", "Você tem um novo pedido");
            textWeb.Render();
            Console.WriteLine();

            Notification textMobile = new TextNotification(mobile, "Novo Pedido", "Você tem um novo pedido");
            textMobile.Render();
            Console.WriteLine();

            Notification imageWeb = new ImageNotification(web, "Promoção", "50% de desconto!", "promo.jpg");
            imageWeb.Render();
            Console.WriteLine();

            Notification videoMobile = new VideoNotification(mobile, "Tutorial", "Aprenda a usar o app", "tutorial.mp4");
            videoMobile.Render();
            Console.WriteLine();

            Notification imageDesktop = new ImageNotification(desktop, "Oferta Relâmpago", "Só hoje!", "oferta.png");
            imageDesktop.Render();
            Console.WriteLine("\n✅ Sem explosão combinatória: tipos e plataformas evoluem independentemente.");
        }
    }
}