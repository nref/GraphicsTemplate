using System;
using GraphicsTemplate.Shared;
using GraphicsTemplate.Models;

namespace GraphicsTemplate.Irrlicht
{
    public class IrrlichtGraphicsService : IGraphicsService
    {
        private readonly IIrrlichtGraphicsAdapter _adapter;

        public IrrlichtGraphicsService(IIrrlichtGraphicsAdapter adapter)
        {
            _adapter = adapter;
        }

        public void SetTransform(Guid id, Transform t)
        {
            _adapter.SetTransform(id, t);
        }
    }
}
