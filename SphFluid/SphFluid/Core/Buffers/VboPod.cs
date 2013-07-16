namespace SphFluid.Core.Buffers
{
    public class VboPod
    {
        public Vbo Ping { private set; get; }
        public Vbo Pong { private set; get; }

        public VboPod()
        {
            Ping = new Vbo();
            Pong = new Vbo();
        }

        public void Swap()
        {
            var tmp = Ping;
            Ping = Pong;
            Pong = tmp;
        }
    }
}