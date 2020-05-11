namespace GraphicsTemplate.Models
{
    public class Transform
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }

        public Transform
        (
            double x = 0, 
            double y = 0, 
            double z = 0, 
            double rx = 0, 
            double ry = 0, 
            double rz = 0
        )
        {
            X = x;
            Y = y;
            Z = z;
            Rx = rx;
            Ry = ry;
            Rz = rz;
        }
    }
}
