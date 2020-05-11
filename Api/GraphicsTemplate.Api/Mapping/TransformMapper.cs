
namespace GraphicsTemplate.Api.Mapping
{
    public class TransformMapper
    {
        public Models.Transform Map(Dto.Transform dto)
        {
            return new Models.Transform
            {
                X = dto.X,
                Y = dto.Y,
                Z = dto.Z,
                Rx = dto.Rx,
                Ry = dto.Ry,
                Rz = dto.Rz,
            };
        }


        public Dto.Transform Map(Models.Transform model)
        {
            return new Dto.Transform
            {
                X = model.X,
                Y = model.Y,
                Z = model.Z,
                Rx = model.Rx,
                Ry = model.Ry,
                Rz = model.Rz,
            };
        }
    }
}
